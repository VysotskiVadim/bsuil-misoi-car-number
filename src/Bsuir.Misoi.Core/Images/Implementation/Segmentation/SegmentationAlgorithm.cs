using System.Collections.Generic;
using System;
using System.Linq;

namespace Bsuir.Misoi.Core.Images.Implementation.Segmentation
{
    public class SegmentationAlgorithm
    {
        private readonly IBinarizationFilter _binarizationFilter;

        public string Name => "Segmentation";

        public SegmentationAlgorithm(IBinarizationFilter binarizationFilter)
        {
            _binarizationFilter = binarizationFilter;
        }


        public ISegmentationResult ProcessImage(IImage inputImage)
        {
            _binarizationFilter.ProcessImage(inputImage);

            var binaryImage = ConvertImageToBinaryMatrix(inputImage);

            ISegmentManager segmentManager = new SegmentsManager(inputImage.Width, inputImage.Height);

            for (int y = 0; y < inputImage.Height; y++)  
            {
                for (int x = 0; x < inputImage.Width; x++)
                {
                    bool leftPixel = false, upperPixel = false, currentPixel = false;
                    int leftPixelXPosition = x - 1;
                    int upperPixelYPosition = y - 1;

                    if (leftPixelXPosition >= 0)
                    {
                        leftPixel = binaryImage[leftPixelXPosition, y];
                    }

                    if (upperPixelYPosition >= 0)
                    {
                        upperPixel = binaryImage[x, upperPixelYPosition];
                    }

                    currentPixel = binaryImage[x, y];
                    if (currentPixel == false)
                    {
                    }
                    else if (leftPixel == false && upperPixel == false)
                    {
                        segmentManager.MarkNewSegment(x, y);
                    }
                    else if (leftPixel && upperPixel == false)
                    {
                        var segmentOfLeftPixel = segmentManager.GetSegmentIdFor(leftPixelXPosition, y);
                        segmentManager.MarkSegment(x, y, segmentOfLeftPixel);
                    }
                    else if (leftPixel == false && upperPixel)
                    {
                        var segmentOfUpperPixel = segmentManager.GetSegmentIdFor(x, upperPixelYPosition);
                        segmentManager.MarkSegment(x, y, segmentOfUpperPixel);
                    }
                    else if (leftPixel & upperPixel)
                    {
                        var segmentOfLeftPixel = segmentManager.GetSegmentIdFor(leftPixelXPosition, y);
                        var segmentOfUpperPixel = segmentManager.GetSegmentIdFor(x, upperPixelYPosition);
                        if (segmentOfLeftPixel == segmentOfUpperPixel)
                        {           
                            segmentManager.MarkSegment(x, y, segmentOfLeftPixel);
                        }
                        else
                        {
                            segmentManager.MergeSegments(segmentOfLeftPixel, segmentOfUpperPixel);
                            segmentManager.MarkSegment(x, y, segmentManager.GetSegmentIdFor(leftPixelXPosition, y)); 
                        }
                    }
                }
            }

            return segmentManager.BuildSegmentationResult();

            //var segments = segmentManager.BuildSegmentationResult();

            //var image = segments.SegmentationMatrix;
            //for (int j = 0; j < inputImage.Height; j++)
            //{
            //    for (int i = 0; i < inputImage.Width; i++)
            //    {
            //        var random = new Random(image[i, j]);
            //        var pixel = new Pixel { R = (byte)random.Next(255), G = (byte)random.Next(255), B = (byte)random.Next(255) };
            //        inputImage.SetPixel(i, j, pixel);
            //    }
            //}

            //foreach (var segment in segments.Segments.Where(s => s.Square > 50))
            //{
            //    Point minX = new Point(inputImage.Width - 1, 0), maxX = new Point(), maxY = new Point(), minY = new Point(0, inputImage.Height - 1);
            //    for (int y = 0; y < inputImage.Height; y++)
            //    {
            //        for (int x = 0; x < inputImage.Width; x++)
            //        {
            //            if (image[x, y] == segment.Id)
            //            {
            //                var currentPoint = new Point(x, y);
            //                if (x <= minX.X)
            //                {
            //                    minX = currentPoint;
            //                }
            //                else if (x >= maxX.X)
            //                {
            //                    maxX = currentPoint;
            //                }
            //                else if (y <= minY.Y)
            //                {
            //                    minY = currentPoint;
            //                }
            //                else if (y >= maxY.Y)
            //                {
            //                    maxY = currentPoint;
            //                }
            //            }
            //        }
            //    }


            //    var middleX = (maxX.X - minX.X) / 2 + minX.X;
            //    var minYInMiddle = GetYsForMiddleX(image, middleX, segment.Id).Min();
            //    var maxYInMiddle = GetYsForMiddleX(image, middleX, segment.Id).Max();

            //    int segmentHeight = maxY.Y - minY.Y;
                
            //    if (Math.Abs(minYInMiddle - minY.Y) / ((double)segmentHeight) < 0.1 && Math.Abs(maxYInMiddle - maxY.Y) / ((double)segmentHeight) < 0.1)
            //    {
            //        var minXminY = new Point(minX.X, minY.Y);
            //        var maxXminY = new Point(maxX.X, minY.Y);
            //        var minXmaxY = new Point(minX.X, maxY.Y);
            //        var parWidth = Math.Abs(minXminY.X - maxXminY.X);
            //        var parHeight = Math.Abs(minXminY.Y - minXmaxY.Y);
            //        var formFactor = parWidth / (double)parHeight;
            //        if ((formFactor > 4.1) && (formFactor < 5.1)) // 520mm X 113mm  form-factor s/p4,64   a/b = 4,6
            //        {
            //            yield return new FindResult(new List<Point> { minXmaxY, minXminY, maxXminY, new Point(maxX.X, maxY.Y) });
            //        }
            //    }

            //}
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
