namespace Bsuir.Misoi.Core.Images.Selection.Implementation
{
    public class SelectionResult : ISelectionResult
    {
        public SelectionResult(int x, int y, int height, int width)
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
