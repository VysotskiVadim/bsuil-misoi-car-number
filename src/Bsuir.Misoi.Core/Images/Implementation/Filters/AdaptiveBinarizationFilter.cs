namespace Bsuir.Misoi.Core.Images.Implementation.Filters
{
    public class AdaptiveBinarizationFilter : IBinarizationFilter
    {
        public string Name => "Adaptive binarization";

        private const int noise = 12;

        public void ProcessImage(IImage image)
        {
            byte[,] indencity = GetIndencityFromImage(image);

            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    var middle = GetMiddlePixelArea(x, y, image, indencity);
                    var pixel = indencity[x, y];

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

        private byte[,] GetIndencityFromImage(IImage image)
        {
            var indencity = new byte[image.Width, image.Height];
            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    indencity[x, y] = this.GetIndencity(image, x, y);
                }
            }

            return indencity;
        }

        private double GetMiddlePixelArea(int x, int y, IImage image, byte[,] indencities)
        {
            var radius = 5;

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
                    var pixel = indencities[i, j];
                    intensity += pixel;
                    count++;
                }
            }

            return (double)(intensity / count);
        }

        private byte GetIndencity(IImage image, int x, int y)
        {
            var pixel = image.GetPixel(x, y);
            return (byte)((pixel.R + pixel.G + pixel.B) / 3);
        }
    }
}
