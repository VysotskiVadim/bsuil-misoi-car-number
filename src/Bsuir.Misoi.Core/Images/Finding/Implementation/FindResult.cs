namespace Bsuir.Misoi.Core.Images.Finding.Implementation
{
    public class FindResult : IFindResult
    {
        public FindResult(int x, int y, int height, int width)
        {
            this.X = x;
            this.Y = y;
            this.Height = height;
            this.Width = width;
        }

        public int X { get; }
        public int Y { get; }
        public int Height { get; }
        public int Width { get; }
    }
}
