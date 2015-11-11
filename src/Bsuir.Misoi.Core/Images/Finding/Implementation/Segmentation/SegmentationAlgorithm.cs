using System.Collections.Generic;
using System.Diagnostics;
using Bsuir.Misoi.Core.Images.Filtering;
using Bsuir.Misoi.Core.Images.Filtering.Implementation;
using System;
using System.Linq;

namespace Bsuir.Misoi.Core.Images.Finding.Implementation.Segmentation
{
    public class SegmentationAlgorithm : IFindAlgorithm
    {
        private readonly IFilter _binarizationFilter;
        public string Name => "Segmentation";


        public SegmentationAlgorithm(IFilter binarizationFilter)
        {
            _binarizationFilter = binarizationFilter;
        }


        public IEnumerable<IFindResult> Find(IImage inputImage)   // конкретно сегментация  работает c бинарной матрицой - быстрее будет, чем работа с пиксельным представлением изображения
        {
            _binarizationFilter.Filter(inputImage);

            var binaryImage = ConvertImageToBinaryMatrix(inputImage);

            ISegmentManager segmentManager = new SegmentsManager(inputImage.Width, inputImage.Height);


            for (int y = 0; y < inputImage.Height; y++)  // Цикл по пикселям изображения
            {
                for (int x = 0; x < inputImage.Width; x++)
                {
                    bool b = false, c = false, a = false;
                    int bx = x - 1;
                    int cy = y - 1;

                    if (bx >= 0)
                    {
                        b = binaryImage[bx, y];
                    }

                    if (cy >= 0)
                    {
                        c = binaryImage[x, cy];
                    }

                    a = binaryImage[x, y];
                    if (a == false) { } // Если в текущем пикселе пусто - то ничего не делаем
                    else if (b == false && c == false) // Если вокруг нашего пикселя пусто а он не пуст - то это повод подумать о новом сегменте
                    {
                        segmentManager.MarkNewSegment(x, y);
                    }
                    else if (b && c == false)
                    {
                        var bSegment = segmentManager.GetSegmentIdFor(x - 1, y);
                        segmentManager.MarkSegment(x, y, bSegment);
                    }
                    else if (b == false && c)
                    {
                        var cSegment = segmentManager.GetSegmentIdFor(x, cy);
                        segmentManager.MarkSegment(x, y, cSegment);
                    }
                    else if (b & c)
                    {
                        var bSegment = segmentManager.GetSegmentIdFor(bx, y);
                        var cSegment = segmentManager.GetSegmentIdFor(x, cy);
                        if (bSegment == cSegment)
                        {           // если сверху и снизу один и тот же класс
                            segmentManager.MarkSegment(x, y, bSegment);
                        }
                        else
                        {               // СПОРНАЯ СИТУАЦИЯ! если возникнет конфликт кластеризации - upd: сделаем чтобы А-пиксель отходил так же к сегменту С
                            segmentManager.MergeSegments(bSegment, cSegment);
                            segmentManager.MarkSegment(x, y, segmentManager.GetSegmentIdFor(bx, y));   // т.к. B и С с разных сегментов, но соединяются в A - это всё один сегмент, мерджим B и С сегменты
                        }
                    }

                }// for j
            } // for i

            var segments = segmentManager.BuildSegmentationResult();

            var image = segments.SegmentationMatrix;
            for (int j = 0; j < inputImage.Height; j++)
            {
                for (int i = 0; i < inputImage.Width; i++)
                {
                    var random = new Random(image[i, j]);
                    var pixel = new Pixel { R = (byte)random.Next(255), G = (byte)random.Next(255), B = (byte)random.Next(255) };
                    inputImage.SetPixel(i, j, pixel);
                }
            }

            foreach (var segment in segments.Segments.Where(s => s.Square > 50))
            {
                Point minX = new Point(inputImage.Width - 1, 0), maxX = new Point(), maxY = new Point(), minY = new Point(0, inputImage.Height - 1);
                for (int y = 0; y < inputImage.Height; y++)
                {
                    for (int x = 0; x < inputImage.Width; x++)
                    {
                        if (image[x, y] == segment.Id)
                        {
                            var currentPoint = new Point(x, y);
                            if (x <= minX.X)
                            {
                                minX = currentPoint;
                            }
                            else if (x >= maxX.X)
                            {
                                maxX = currentPoint;
                            }
                            else if (y <= minY.Y)
                            {
                                minY = currentPoint;
                            }
                            else if (y >= maxY.Y)
                            {
                                maxY = currentPoint;
                            }
                        }
                    }
                }


                var middleX = (maxX.X - minX.X) / 2 + minX.X;
                var minYInMiddle = GetYsForMiddleX(image, middleX, segment.Id).Min();
                var maxYInMiddle = GetYsForMiddleX(image, middleX, segment.Id).Max();

                int segmentHeight = maxY.Y - minY.Y;
                
                if (Math.Abs(minYInMiddle - minY.Y) / ((double)segmentHeight) < 0.1 && Math.Abs(maxYInMiddle - maxY.Y) / ((double)segmentHeight) < 0.1)
                {
                    var minXminY = new Point(minX.X, minY.Y);
                    var maxXminY = new Point(maxX.X, minY.Y);
                    var minXmaxY = new Point(minX.X, maxY.Y);
                    var parWidth = Math.Abs(minXminY.X - maxXminY.X);
                    var parHeight = Math.Abs(minXminY.Y - minXmaxY.Y);
                    var formFactor = parWidth / (double)parHeight;
                    if ((formFactor > 4.1) && (formFactor < 5.1)) // 520mm X 113mm  form-factor s/p4,64   a/b = 4,6
                    {
                        yield return new FindResult(new List<Point> { minXmaxY, minXminY, maxXminY, new Point(maxX.X, maxY.Y) });
                    }
                }

                //yield return new FindResult(new List<Point> { minX, minY, maxY, maxX });
            }
        }

        private IEnumerable<int> GetYsForMiddleX(int[,] image, int middleX, int segmentId)
        {
            for (int y = 0; y < image.GetLength(1); y++)
            {
                if (image[middleX, y] == segmentId)
                {
                    yield return y;
                }
            }
        }

        private IEnumerable<int> ForX(int middle, int end)
        {
            var final = end - middle;
            for (int i = -middle; i < final; i++)
            {
                yield return i;
            }
        }

        private bool PixelToBinary(Pixel pixel)  // перевод RGB пикселя в строго бинарный вид типа int 
        {
            return pixel.B != 0;   // или R!=0 или G != 0  ?)) 
        }

        private bool[,] ConvertImageToBinaryMatrix(IImage inputImage)  // перевод пиксельного изображения в матричный вид типа int
        {
            var image = new bool[inputImage.Width, inputImage.Height];
            for (int j = 0; j < inputImage.Height; j++)
            {
                for (int i = 0; i < inputImage.Width; i++)
                {
                    image[i, j] = PixelToBinary(inputImage.GetPixel(i, j));
                }
            }
            return image;
        }
    }
}
