using System;
using System.Collections.Generic;
using System.Linq;

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
            foreach (var segment in segmantationResult.Segments.Where(s => s.Square > 50))
            {
                Point minX = new Point(width - 1, 0), maxX = new Point(), maxY = new Point(), minY = new Point(0, height - 1);
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
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
    }
}
