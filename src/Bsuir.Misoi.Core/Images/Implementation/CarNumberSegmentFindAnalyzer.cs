using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using Bsuir.Misoi.Core.Images.Implementation.Hough;

namespace Bsuir.Misoi.Core.Images.Implementation
{
    public class CarNumberSegmentFindAnalyzer : ISegmentFindAnalyzer
    {
        public CarNumberSegmentFindAnalyzer(string name)
        {
            Name = name;
        }

        public CarNumberSegmentFindAnalyzer()
        {
            Name = "Find car number";
        }

        public string Name { get; }

        public IEnumerable<IFindResult> Find(ISegmentationResult segmantationResult)
        {
            var image = segmantationResult.SegmentationMatrix;
            int width = segmantationResult.SegmentationMatrix.GetLength(0);
            int height = segmantationResult.SegmentationMatrix.GetLength(1);
            var points = new List<Point>();
            foreach (var segment in segmantationResult.Segments.Where(s => s.Square > 50))
            {
                Point minX = new Point(width - 1, 0), maxX = new Point(), maxY = new Point(), minY = new Point(0, height - 1);
                points.Clear();
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        if (image[x, y] == segment.Id)
                        {
                            var currentPoint = new Point(x, y);
                            points.Add(currentPoint);
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

                var minXminY = new Point(minX.X, minY.Y);
                var maxXminY = new Point(maxX.X, minY.Y);
                var minXmaxY = new Point(minX.X, maxY.Y);
                var parWidth = Math.Abs(minXminY.X - maxXminY.X);
                var parHeight = Math.Abs(minXminY.Y - minXmaxY.Y);
                var formFactor = parWidth / (double)parHeight;
               // if ((formFactor > 4.1) && (formFactor < 5.1)) // 520mm X 113mm  form-factor s/p4,64   a/b = 4,6
                if ((formFactor > 3) && (formFactor < 5.1)) // 520mm X 113mm  form-factor s/p4,64   a/b = 4,6
                {
                    //yield return new FindResult(new List<Point> { minXmaxY, minXminY, maxXminY, new Point(maxX.X, maxY.Y) });
                    var hough = new HoughConversion();
                    double imageDiagonal = new Point(0, 0).Distanse(new Point(width, height));
                    IList<Line> lines = hough.AccumulateLines(points, imageDiagonal);
                    lines = this.FilterLines(lines);
                    if (lines.Count != 0)
                    {
                        var result = PrepareResult(lines);
                        if (result != null)
                        {
                            yield return result;
                        }
                    }
                }
            }
        }

        private IEnumerable<IFindResult> DrawLines(IList<Line> lines, int width, int height)
        {
            double x1;
            double y1;
            double x2;
            double y2;

            foreach (var line in lines)
            {
                if (line.F != 0)

                {
                    x1 = 0;
                    y1 = (-Math.Cos(line.FRad) / Math.Sin(line.FRad)) * x1 + (line.R / Math.Sin(line.FRad));
                    x2 = width - 1;
                    y2 = (-Math.Cos(line.FRad) / Math.Sin(line.FRad)) * x2 + (line.R / Math.Sin(line.FRad));

                }
                else
                {
                    x1 = line.R;
                    y1 = 0;
                    x2 = line.R;
                    y2 = height;
                }

                yield return new FindResult(new List<Point> { new Point((int)x1, (int)y1), new Point((int)x2, (int)y2) });
            }
        }

        private Point FindCrossing(Line first, Line second)
        {
            if (Math.Abs(first.F - second.F) < 0.1)
            {
                throw new InvalidOperationException("there is no crossing");
            }

            Func<double, double> getA = (alpha) =>
            {
                return -1.0/Math.Tan(alpha);
            };
            Func<double, double, double> getB = (r, alpha) =>
            {
                return r/Math.Sin(alpha);
            };

            bool hasZero = Math.Abs(first.F) < 0.001;
            if (Math.Abs(second.F) < 0.001)
            {
                hasZero = true;
                var temp = second;
                second = first;
                first = temp;
            }

            double x, y;

            if (hasZero)
            {
                x = first.R;
                double a = getA(second.FRad);
                double b = getB(second.R, second.FRad);
                y = a * x + b;
            }
            else
            {
                double a1 = getA(first.FRad);
                double a2 = getA(second.FRad);
                double b1 = getB(first.R, first.FRad);
                double b2 = getB(second.R, second.FRad);

                x = (b2 - b1)/(a1 - a2);
                y = a1*x + b1;
            }

            return new Point((int)Math.Round(x), (int)Math.Round(y));
        }

        private readonly int _angleDifference = 5;

        private IList<Line> FilterLines(IList<Line> lines)
        {
            var result = new List<Line>();

            foreach (var line in lines)
            {
                result.Clear();
                result.Add(line);

                var parallelLines = lines.Where(l =>
                    Math.Abs((line.F - l.F)%180) < _angleDifference)
                    .OrderByDescending(l => Math.Abs(line.R - l.R));

                if (parallelLines.Any())
                {
                    result.Add(parallelLines.First());
                }
                else
                {
                    continue;
                }

                var perpendicularLines = lines.Where(l => Math.Abs((line.F - l.F) % 180 - 90) < _angleDifference)
                    .OrderByDescending(l => l.R).ToList();

                if (perpendicularLines.Count >= 2)
                {
                    result.Add(perpendicularLines[0]);
                    result.Add(perpendicularLines[perpendicularLines.Count - 1]);
                }
                else
                {
                    continue;
                }

                if (result.Count == 4)
                {
                    break;
                }
            }

            if (result.Count < 4)
            {
                result.Clear();
            }

            return result;
        }

        private IFindResult PrepareResult(IList<Line> lines)
        {
            lines = lines.OrderBy(l => l.F).ToList();
            var first = FindCrossing(lines[0], lines[2]);
            var second = FindCrossing(lines[0], lines[3]);
            var third = FindCrossing(lines[1], lines[2]);
            var fourth = FindCrossing(lines[1], lines[3]);

            var parHeight = Math.Abs(first.Distanse(second));
            var parWidth = Math.Abs(second.Distanse(fourth));
            var formFactor = parWidth / (double)parHeight;
            if ((formFactor > 4.1) && (formFactor < 5.1)) // 520mm X 113mm  form-factor s/p4,64   a/b = 4,6
            {
                return new FindResult(new List<Point> {first, second, fourth, third}, (float) lines[1].F);
            }
            else
            {
                return null;
            }
        }
    }
}
