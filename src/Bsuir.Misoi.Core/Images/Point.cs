namespace Bsuir.Misoi.Core.Images
{
    public struct Point
    {
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; }

        public int Y { get; }

        public bool IsTheSame(Point point)
        {
            return point.X == this.X && point.Y == this.Y;
        }
    }
}
