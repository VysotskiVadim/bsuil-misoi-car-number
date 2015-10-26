using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;

namespace Bsuir.Misoi.Core.Images.Filtering.Implementation
{
    public class BinarizationFilter : IFilter
    {
        public string Name => "Adaptive binarization";
        private const int noise = 12;

        public void Filter(IImage image)
        {
            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    var middle = GetMiddlePixelArea(x, y, image);
                    var pixel = GetPixel(image, x, y);

                    if (pixel > middle + noise)
                    {
                        image.SetPixel(x, y, new Pixel { R = 255, G = 255, B = 255 });
                    }
                    else
                    {
                        image.SetPixel(x, y, new Pixel());
                    }
                }
            }
        }

        private double GetMiddlePixelArea(int x, int y, IImage image)
        {
            var radius = 13;

            var left = (x - radius < 0) ? 0 : x - radius;
            var top = (y - radius < 0) ? 0 : y - radius;
            var right = (x + radius < image.Width) ? x + radius : image.Width - 1;
            var bottom = (y + radius < image.Height) ? y + radius : image.Height - 1;

            var intensity = 0;
            var count = 0;

            for (int i = left; i < right; i++)
            {
                for (int j = top; j < bottom; j++)
                {
                    var pixel = GetPixel(image, i, j);
                    intensity += pixel;
                    count++;
                }
            }

            return (double)(intensity / count);
        }

        private int GetPixel(IImage image, int x, int y)
        {
            var pixel = image.GetPixel(x, y);
            return (int)((pixel.R + pixel.G + pixel.B) / 3);
        }
    }
}
