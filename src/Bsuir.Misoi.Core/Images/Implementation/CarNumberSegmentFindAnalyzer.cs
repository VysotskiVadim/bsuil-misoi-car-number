using System;
using System.CodeDom;
using System.Collections.Generic;
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
                        yield return PrepareResult(lines);
                    }
                }
            }
        }

        private Point FindCrossing(Line first, Line second)
        {
            double a = first.R + second.R;
            double b = Math.Sin(first.FRad) + Math.Sin(second.FRad);
            double c = Math.Cos(first.FRad) + Math.Cos(second.FRad);

            double x = (first.R*(b - 1) + second.R)/(Math.Cos(first.FRad)*b - Math.Sin(first.FRad)*c);
            double y = (a - x * c) / b;

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

             return new FindResult(new List<Point> { first, second, third, fourth }, (float)lines[1].F);
        }
    }
}
