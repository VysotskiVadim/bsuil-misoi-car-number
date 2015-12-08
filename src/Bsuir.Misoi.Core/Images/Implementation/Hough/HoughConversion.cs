using System;
using System.Collections.Generic;

namespace Bsuir.Misoi.Core.Images.Implementation.Hough
{
    class HoughConversion
    {
        public List<Line> AccumulateLines(List<Point> points, double diagonal)
        {
            //максимальное значение R = длине диагонали изображения
            int RMax = (int)Math.Floor(diagonal);
            //мин кол-во точек, принадлежащих прямой, чтобы считать прямую искомой 
            int MinPoints = 20;//14
            //точность вычисления принадлежности точки прямой
            double accuracy = 1.5;
            //максимальное кол-во градусов угла
            int angle = 360;

            int[,] accArr = new int[angle, RMax];

            foreach (Point point in points)
            {
                //перебираем все возможные углы наклона
                for (int f = 0; f < 360; f++)
                {
                    double theta = f * Math.PI / 180.0; // переводим градусы в радианы
                    // перебираем все возможные расстояния от начала координат
                    int r = (int) Math.Abs(Math.Round(point.Y * Math.Sin(theta) + point.X  * Math.Cos(theta)));
                    if (r > 0)
                    {
                        accArr[f, r]++;
                    }
                }
            }

            List<Line> lines = new List<Line>();
            for (int f = 0; f < angle; f++)
            {
                for (int r = 0; r < RMax; r++)
                {
                    if (IsMaximum(f, r, accArr))
                    {
                        Line line = new Line(f, r);
                        lines.Add(line);
                    }
                }
            }

            return lines;
        }

        private bool IsMaximum(int x, int y, int[,] values)
        {
            var radius = 5;

            int width = values.GetLength(0);
            int height = values.GetLength(1);

            var left = (x - radius < 0) ? 0 : x - radius;
            var top = (y - radius < 0) ? 0 : y - radius;
            var right = (x + radius < width) ? x + radius : width - 1;
            var bottom = (y + radius < height) ? y + radius : height - 1;

            var intensity = 0;
            var count = 0;

            int result = 0;

            int maximum = 0;

            for (int i = left; i < right; i++)
            {
                for (int j = top; j < bottom; j++)
                {
                    if ((x != i || y != j) && values[i, j] > maximum)
                    {
                        maximum = values[i, j];
                    }
                }
            }

            return values[x, y] > maximum;
        }
    }
}
