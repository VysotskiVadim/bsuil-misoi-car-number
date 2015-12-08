using System;
using System.Collections.Generic;
using System.Linq;
using Bsuir.Misoi.Core.Images.Implementation.Hough;

namespace Bsuir.Misoi.Core.Images.Implementation
{
    public class CarNumberSegmentFindAnalyzer : ISegmentFindAnalyzer
    {
        public string Name => "Find car number";

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
                    //lines = this.FilterLines(lines);
                    foreach (var result in DrawLines(lines, width, height))
                    {
                        yield return result;
                    }
                }
            }
        }

        private readonly int _coordintatesDifference = 15;
        private readonly int _angleDifference = 5;

        private IList<Line> FilterLines(IList<Line> lines)
        {
            var result = new List<Line>();
            
            foreach (var line in lines)
            {
                bool foundParellel = false;
                bool foundFirstPerpendicular = false;
                Line firstPerpendicular = new Line();
                bool foundSecondPerpendicular = false;
                result.Clear();
                result.Add(line);

                foreach (var testedLine in lines)
                {
                    if (foundParellel == false)
                    {
                        if (Math.Abs((line.F - testedLine.F)%180) < _angleDifference && Math.Abs(line.R - testedLine.R) > _coordintatesDifference)
                        {
                            foundParellel = true;
                            result.Add(testedLine);
                        }
                    }
                    if (foundFirstPerpendicular == false)
                    {
                        if (Math.Abs(((line.F - testedLine.F) % 180) - 90) < _angleDifference)
                        {
                            foundFirstPerpendicular = true;
                            result.Add(testedLine);
                            firstPerpendicular = testedLine;
                        }
                    }
                    if (foundFirstPerpendicular && foundSecondPerpendicular == false)
                    {
                        if (Math.Abs(((line.F - testedLine.F) % 180) - 90) < _angleDifference && Math.Abs(firstPerpendicular.R - testedLine.R) > _coordintatesDifference)
                        {
                            result.Add(testedLine);
                            foundSecondPerpendicular = true;
                        }
                    }

                    if (result.Count == 4)
                    {
                        break;
                    }
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

                yield return new FindResult(new List<Point> { new Point((int)x1, (int)y1), new Point((int)x2, (int)y2)});
            }
        }
    }
}
