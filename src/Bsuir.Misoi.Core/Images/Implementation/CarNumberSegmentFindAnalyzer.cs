using System;
using System.Collections.Generic;
using System.Linq;
using Bsuir.Misoi.Core.Images.Implementation.Hough;
using Bsuir.Misoi.Core.Images.Implementation.Image;
using OpenCvSharp.CPlusPlus;

namespace Bsuir.Misoi.Core.Images.Implementation
{
    public class CarNumberSegmentFindAnalyzer : ISegmentFindAnalyzer
    {

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
                if ((formFactor > 3) && (formFactor < 6)) // 520mm X 113mm  form-factor s/p4,64   a/b = 4,6
                {
                    var rect = Cv2.MinAreaRect(points.Select(p => new Point2f(p.X, p.Y)));
                    float resultRectSquare =rect.Size.Width*rect.Size.Height;
                    formFactor = Math.Max(rect.Size.Width, rect.Size.Height) / Math.Min(rect.Size.Width, rect.Size.Height);
                    if (segment.Square / resultRectSquare > 0.5 && (rect.Size.Width > 100 || rect.Size.Height > 100) && (formFactor > 4.1) && (formFactor < 6))
                    {
                        yield return new FindResult(rect.Points().Select(p => new Point((int)p.X, (int)p.Y)).ToList());
                    }
                }
            }
        }
    }
}
