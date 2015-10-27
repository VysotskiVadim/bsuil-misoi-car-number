using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bsuir.Misoi.Core.Images.Finding.Implementation
{
    public class FindResultBuilder
    {
        private List<Point> _points;

        public void Add(int x, int y)
        {
            _points.Add(new Point(x, y));
        }

        public IFindResult GetResult()
        {
            var jarvis = new JarvisAlgorythm();
            var r = jarvis.ConvexHull(_points);
            return new FindResult(r);
        }

        public int GetPerimeter()
        {
            double result = 0;
            var firstPoint = _points[0];
            var previousPoint = firstPoint;
            for (int i = 1; i < _points.Count; i++)
            {
                var currentPoint = _points[i];
                result += this.GetDistanceBetweenPoints(previousPoint, currentPoint);
                previousPoint = currentPoint;
            }
            var lastPoint = _points[_points.Count - 1];
            result += this.GetDistanceBetweenPoints(lastPoint, firstPoint);
            return (int)result;
        }

        private double GetDistanceBetweenPoints(Point first, Point second)
        {
            var dx = first.X - second.X;
            var dy = first.Y - second.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }
    }
}
