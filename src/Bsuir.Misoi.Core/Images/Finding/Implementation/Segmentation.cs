using System.Collections.Generic;
using System.Diagnostics;
using Bsuir.Misoi.Core.Images.Filtering;
using Bsuir.Misoi.Core.Images.Filtering.Implementation;
using System;

namespace Bsuir.Misoi.Core.Images.Finding.Implementation
{
    public class Segmentation : IFindAlgorithm
    {
        private readonly IFilter _binarizationFilter;
        public string Name => "Segmentation";

        // данные и 3 метода  для работы с системой непересекающихся множеств
        private int[] segment = new int[100000]; // массив эквивалентых класов-сегментов
        private int[] rank = new int[100000]; // ранги эквивалентных классов, в общем-то тут не обязательны

        public Segmentation(IFilter binarizationFilter)
        {
            _binarizationFilter = binarizationFilter;
        }

        private void MakeSet(int x) // создание сегмента
        {
            segment[x] = x;
            rank[x] = 0;
        }

        private int Find(int x)  // главного сегмента в эквивалентных сегментах
        {
            return (x == segment[x] ? x : segment[x] = Find(segment[x]));
        }

        private void Union(int x, int y)   // merge двух сегментов, образующих конфликт
        {
            if ((x = Find(x)) == (y = Find(y)))
                return;

            if (rank[x] < rank[y])
                segment[x] = y;
            else
                segment[y] = x;

            if (rank[x] == rank[y])
            {
                segment[y] = x;
                ++rank[x];
            }
        }

        private int BinaryPixel(Pixel pixel)  // перевод RGB пикселя в строго бинарный вид типа int 
        {
            if (pixel.B != 0) { return 1; }   // или R!=0 или G != 0  ?)) 
            else { return 0; }
        }

        private int[,] image;  //  image in int-matrix type



        private void ConvertImageToBinaryMatrix(IImage inputImage)  // перевод пиксельного изображения в матричный вид типа int
        {
            image = new int[inputImage.Width, inputImage.Height];
            for (int j = 0; j < inputImage.Height; j++)
            {
                for (int i = 0; i < inputImage.Width; i++)
                {
                    image[i, j] = BinaryPixel(inputImage.GetPixel(i, j));
                }
            }
        }

        public IEnumerable<IFindResult> Find(IImage inputImage)   // конкретно сегментация  работает c бинарной матрицой - быстрее будет, чем работа с пиксельным представлением изображения
        {
            _binarizationFilter.Filter(inputImage);
            ConvertImageToBinaryMatrix(inputImage);

            int segmentNum = 1; // номер нового класса, бинарное изображение или 0 или 1, считанём с 2ух :D
            int B = 0, C = 0, A = 0;

            for (int j = 0; j < inputImage.Height; j++)  // Цикл по пикселям изображения
            {
                for (int i = 0; i < inputImage.Width; i++)
                {
                    if (i - 1 < 0)
                    {
                        B = 0;
                    }
                    else
                    {
                        B = image[i - 1, j];
                    }

                    if (j - 1 < 0)
                    {
                        C = 0;
                    }
                    else
                    {
                        C = image[i, j - 1];
                    }

                    A = image[i, j];
                    if (A == 0) { } // Если в текущем пикселе пусто - то ничего не делаем
                    else if (B == 0 & C == 0) // Если вокруг нашего пикселя пусто а он не пуст - то это повод подумать о новом сегменте
                    {
                        image[i, j] = ++segmentNum;  // пометим пикселеь номером нового сегмента 
                        MakeSet(segmentNum);  // выделим новый сегмент, запомним его
                    }
                    else if (B != 0 & C == 0)
                    {
                        image[i, j] = B;
                    }
                    else if (B == 0 & C != 0)
                    {
                        image[i, j] = C;
                    }
                    else if (B != 0 & C != 0)
                    {
                        if (B == C)
                        {           // если сверху и снизу один и тот же класс
                            image[i, j] = B;
                        }
                        else
                        {               // СПОРНАЯ СИТУАЦИЯ! если возникнет конфликт кластеризации - upd: сделаем чтобы А-пиксель отходил так же к сегменту С
                            image[i, j] = C;
                            Union(B, C);   // т.к. B и С с разных сегментов, но соединяются в A - это всё один сегмент, мерджим B и С сегменты
                        }
                    }

                }// for j
            } // for i



            // ВТОРОЙ ПРОХОД, КОРРЕКТИРОВКА:
            int[] squares = new int[20];  
            int[] perimeters = new int[20];
            int L = 0, R = 0, U = 0, D = 0;

            for (int j = 0; j < inputImage.Height; j++)  // цикл по пикселям изображения
            {
                for (int i = 0; i < inputImage.Width; i++)
                {
                    if (image[i, j] != 0)
                    {
                        image[i, j] = Find(image[i, j]);

                        squares[image[i, j]]++;  // инкремент площади сегмента 
                                              // check pixel from
                        U = (j - 1 < 0) ? 1 : image[i, j - 1];   // up
                        L = (i - 1 < 0) ? 1 : image[i - 1, j];   // left
                        R = (i + 1 >= 8) ? 1 : image[i + 1, j];  // right
                        D = (j + 1 >= 8) ? 1 : image[i, j + 1];  // down

                        if (L == 0 || R == 0 || U == 0 || R == 0)
                        {
                            perimeters[image[i, j]]++;  // если в стороне - фон - то пиксель входит в приметр
                        }
                    }
                }
            }

            // вычисляем форм-фактор для фигуры
            double formFactor = 0;
            bool[] results = new bool[100000];
            double[] intResults = new double[100000];
            for (int i = 0; i < segment.Length; i++)
            {
                if (segment[i] != 0)
                {
                    formFactor = perimeters[i] / (2 * Math.Sqrt(Math.PI * squares[i]));
                    if ((formFactor > 0.45) && (formFactor < 0.5)) // 520mm X 113mm  form-factor 0,474
                    {
                        results[i] = true;
                    }
                    intResults[i] = formFactor; // тестовый - чтобы посмотреть чё получилось по сегментам
                }
            }


            // ТУТ НУЖНО НАЙТИ ДАННЫЕ
            //for (int i = 0, )
            //if (results[i])

            yield return new FindResult(10, 10, 20, 20);
        }
    }
}
