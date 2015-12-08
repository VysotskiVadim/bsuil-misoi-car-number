using System;
using System.Collections.Generic;
using System.Linq;

namespace Bsuir.Misoi.Core.Images.Implementation.Hough
{
    public class HoughConversion : IHoughConversion
    {
        public IList<Line> AccumulateLines(List<Point> points, double diagonal)
        {
            //максимальное значение R = длине диагонали изображения
            int RMax = (int) Math.Floor(diagonal);
            //мин кол-во точек, принадлежащих прямой, чтобы считать прямую искомой 
            int MinPoints = 20; //14
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
                    double theta = f*Math.PI/180.0; // переводим градусы в радианы
                    // перебираем все возможные расстояния от начала координат
                    int r = (int) Math.Abs(Math.Round(point.Y*Math.Sin(theta) + point.X*Math.Cos(theta)));
                    if (r > 0)
                    {
                        accArr[f, r]++;
                    }
                }
            }

            var lines = IsMaximum(accArr);
            return lines;
        }

        private const int _maximumsCount = 30;

        private IList<Line> IsMaximum(int[,] values)
        {
            int width = values.GetLength(0);
            int height = values.GetLength(1);
            List<Line> maximums = new List<Line>(_maximumsCount);
            for (int f = 0; f < width; f++)
            {
                for (int r = 0; r < height; r++)
                {
                    if (maximums.Count <= _maximumsCount)
                    {
                        maximums.Add(new Line(f, r));
                    }
                    else
                    {
                        int? theLoweresIndex = null;
                        for (int i = 0; i < maximums.Count; i++)
                        {
                            var oneOfMax = maximums[i];
                            if (values[f, r] > values[oneOfMax.F, oneOfMax.R])
                            {
                                if (theLoweresIndex == null)
                                {
                                    theLoweresIndex = i;
                                }
                                else
                                {
                                    var currentTheLoverst = maximums[theLoweresIndex.Value];
                                    if (values[currentTheLoverst.F, currentTheLoverst.R] > values[oneOfMax.F, oneOfMax.R])
                                    {
                                        theLoweresIndex = i;
                                    }
                                }
                            }
                        }
                        if (theLoweresIndex != null)
                        {
                            maximums[theLoweresIndex.Value] = new Line(f, r);
                        }
                    }
                }
            }
            return maximums;
        }
    }
}
