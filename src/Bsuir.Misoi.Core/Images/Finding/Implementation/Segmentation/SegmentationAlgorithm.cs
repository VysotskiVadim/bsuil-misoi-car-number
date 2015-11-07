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

        // данные и 3 метода  для работы с системой непересекающихся множеств
        private int[] segment = new int[100000]; // массив эквивалентых класов-сегментов
        private int[] rank = new int[100000]; // ранги эквивалентных классов, в общем-то тут не обязательны

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
                    else if (b  && c == false)
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

            var image = segmentManager.BuildSegmentMatrix();


            //// ВТОРОЙ ПРОХОД, КОРРЕКТИРОВКА:
            //int[] squares = new int[100000];
            //int[] perimeters = new int[100000];
            //int L = 0, R = 0, U = 0, D = 0;

            //for (int y = 0; y < inputImage.Height; y++)  // цикл по пикселям изображения
            //{
            //    for (int x = 0; x < inputImage.Width; x++)
            //    {
            //        if (binaryImage[x, y] != 0)
            //        {
            //            binaryImage[x, y] = Find(binaryImage[x, y]);

            //            squares[binaryImage[x, y]]++;  // инкремент площади сегмента 
            //                                     // check pixel from
            //            U = (y - 1 < 0) ? 1 : binaryImage[x, y - 1];   // up
            //            L = (x - 1 < 0) ? 1 : binaryImage[x - 1, y];   // left
            //            R = (x + 1 >= 8) ? 1 : binaryImage[x + 1, y];  // right
            //            D = (y + 1 >= 8) ? 1 : binaryImage[x, y + 1];  // down

            //            if (L == 0 || R == 0 || U == 0 || R == 0)
            //            {
            //                perimeters[binaryImage[x, y]]++;  // если в стороне - фон - то пиксель входит в приметр
            //            }
            //        }
            //    }
            //}

            for (int j = 0; j < inputImage.Height; j++)
            {
                for (int i = 0; i < inputImage.Width; i++)
                {
                    var random = new Random(image[i, j]);
                    var pixel = new Pixel { R = (byte)random.Next(255), G = (byte)random.Next(255), B = (byte)random.Next(255) };
                    inputImage.SetPixel(i, j, pixel);
                }
            }

            //int parWidth = 0, parHeight = 0;
            //// вычисляем форм-фактор для фигуры
            //double formFactor = 0;
            //// точки описания 
            //Point minX = new Point(inputImage.Width, 0);
            //Point maxX = new Point(0, 0);
            //Point minY = new Point(0, inputImage.Height);
            //Point maxY = new Point(0, 0);
            //// точки пересечения
            //Point minXminY = new Point(0, 0);
            //Point minXmaxY = new Point(0, 0);
            //Point maxXminY = new Point(0, 0);
            //Point maxXmaxY = new Point(0, 0);

            //bool[] results = new bool[100000];
            //double[] intResults = new double[100000];
            //for (int i = 0; i < segment.Length; i++)
            //{
            //    if (segment[i] == i && squares[i] > 70)
            //    {
            //        minX.X = inputImage.Width;
            //        maxX.X = 0;
            //        minY.Y = inputImage.Height;
            //        maxY.Y = 0;

            //        FindResultBuilder resultBuilder = new FindResultBuilder();
            //        for (int y = 0; y < inputImage.Height; y++)  // цикл по пикселям изображения
            //        {
            //            for (int x = 0; x < inputImage.Width; x++)
            //            {
            //                if (image[x, y] == i)
            //                {
            //                    if (x <= minX.X) { minX.X = x; minX.Y = y;}  // left
            //                    if (y <= minY.Y) { minY.X = x; minY.Y = y;  }  // UP
            //                    if (x >= maxX.X) { maxX.X = x; maxX.Y = y; }  // right
            //                    if (y >= maxY.Y) { maxY.X = x; maxY.Y = y; }  // down
            //                    resultBuilder.Add(x, y);

            //                }
            //            }
            //        }
            //        //minXminY minXmaxY  maxYminY  maxXmaxY
            //        minXminY.X = minX.X; minXminY.Y = minY.Y; // UP -LEFT
            //        minXmaxY.X = minX.X; minXmaxY.Y = maxY.Y; // DOWN-LEFT 
            //        maxXminY.X = maxX.X; maxXminY.Y = minY.Y; // UP-RIGHT
            //        maxXmaxY.X = maxX.X; maxXmaxY.Y = maxY.Y; // DOWN-RIGHT

            //        parWidth =  Math.Abs(minXminY.X - maxXminY.X);
            //        parHeight = Math.Abs(minXminY.Y - minXmaxY.Y);

            //        //var perimeter = resultBuilder.GetPerimeter();
            //        //double b = perimeter / 11.2;
            //        //double a = (perimeter / 2.0) - (perimeter / 11.2);
            //        //intResults[i] = Math.Abs(((a * b) - squares[i]));
            //        //results[i] = Math.Abs(((a * b) - squares[i])) < 5;

            //        //formFactor = (double)squares[i] / perimeter; // 520 * 113 /((520 + 113) * 2)
            //        //if ((formFactor > 5) && (formFactor < 9)) // 520mm X 113mm  form-factor 4,64
            //        //{
            //        //    results[i] = true;
            //        //}
            //        //formFactor = (double)squares[i] / perimeter; // 520 * 113 /((520 + 113) * 2)
            //        formFactor =  parWidth / (double)parHeight;
            //        if ((formFactor > 4.1) && (formFactor < 5.1)) // 520mm X 113mm  form-factor s/p4,64   a/b = 4,6
            //        {
            //            results[i] = true;

            //        }
            //        intResults[i] = formFactor;
            //        ; // тестовый - чтобы посмотреть чё получилось по сегментам
            //    }
            //}

            ////var qqq = intResults.Where(r => r != 0).Min();
            ////results[Array.FindIndex(intResults, r => r == qqq)] = true;

            //// ТУТ НУЖНО НАЙТИ ДАННЫЕ
            //for (int x = 0; x < segment.Length; x++)
            //{
            //    if (results[x])
            //    {
            //        FindResultBuilder resultBuilder = new FindResultBuilder();
            //        //int count = 0;
            //        for (int j = 0; j < inputImage.Height; j++)  // цикл по пикселям изображения
            //        {
            //            for (int i = 0; i < inputImage.Width; i++)
            //            {
            //                if (image[i, j] == x)
            //                {
            //                    resultBuilder.Add(i, j);
            //                    //count++;
            //                }
            //            }
            //        }
            //        //if (count > 2)
            //            yield return resultBuilder.GetResult();
            //    }
            //}

            return new List<IFindResult>();
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
