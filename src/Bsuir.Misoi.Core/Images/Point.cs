namespace Bsuir.Misoi.Core.Images
{
    public struct Point
    {
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; set; }

        public int Y { get; set; }

        public bool IsTheSame(Point point)
        {
            return point.X == this.X && point.Y == this.Y;
        }
    }
}
