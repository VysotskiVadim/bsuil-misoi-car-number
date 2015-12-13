using System.Collections.Generic;
using System.IO;
using Bsuir.Misoi.Core.Images.Implementation.Hough;

namespace Bsuir.Misoi.Core.Images
{
    public interface IImage
    {
        string Name { get; set; }

        Pixel GetPixel(int x, int y);

        void SetPixel(int x, int y, Pixel pixel);

        void Save(Stream saveStream);

        int Height { get; }

        int Width { get; }

        IImage Clone();

        void Clip(IList<Point> points, float angle);
    }
}
