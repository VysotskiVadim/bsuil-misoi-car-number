using System;
using System.Collections.Generic;

namespace Bsuir.Misoi.Core.Images.Finding.Implementation
{
    public class FindResult : IFindResult
    {
        public FindResult(int x, int y, int height, int width)
        {
            this.Points.Add(new Point(x, y));
            this.Points.Add(new Point(x, y + height));
            this.Points.Add(new Point(x + width, y + height));
            this.Points.Add(new Point(x + width, y));
        }

        public IList<Point> Points { get; private set; } = new List<Point>();
    }
}
