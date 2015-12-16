using System.Collections.Generic;

namespace Bsuir.Misoi.Core.Images.Implementation.Image
{
    public class FindResult : IFindResult
    {
        public FindResult(List<Point> points, float angle = 0)
        {
            this.Points = points;
            Angle = angle;
        }

        public FindResult(int x, int y, int height, int width)
        {
            this.Points.Add(new Point(x, y));
            this.Points.Add(new Point(x, y + height));
            this.Points.Add(new Point(x + width, y + height));
            this.Points.Add(new Point(x + width, y));
        }

        public IList<Point> Points { get; } = new List<Point>();

        public float Angle { get; }
    }
}
