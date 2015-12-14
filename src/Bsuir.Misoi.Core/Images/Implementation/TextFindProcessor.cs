using System.Collections.Generic;
using System.Drawing;
using System.Linq;
namespace Bsuir.Misoi.Core.Images.Implementation
{
    public class TextFindProcessor : IFindImageProcessor
    {
        public string Name => "Text find processor";
        private IImage _image;

        public TextFindProcessor()
        {
        }

        public IEnumerable<IFindResult> Find(IImage image)
        {
            _image = image;
            var histogram = CalculateLineHistogram();
            int threashold = (int)(0.9 * histogram.Max());

            int startX = 0, endX = 0;
            bool inSegment = false;
            var results = new List<Frame>();
            for (int x = 0; x < histogram.Length; x++)
            {
                if (histogram[x] < threashold && !inSegment)
                {
                    inSegment = true;
                    startX = x;
                }
                if (histogram[x] >= threashold && inSegment)
                {
                    endX = x - 1;
                    inSegment = false;
                    //yield return new FindResult(startX, 0, _image.Height - 1, endX - startX);
                    results.Add(new Frame { StartX = startX , Width = endX - startX });
                }
            }

            var middleWidth = results.Average(r => r.Width);
            return results.Where(r => r.Width > middleWidth/2.0).Select(r => new FindResult(r.StartX, 0, _image.Height - 1, r.Width));
        }

        public struct Frame
        {
            public int StartX { get; set; }

            public int Width { get; set; }
        }

        public int[] CalculateLineHistogram()
        {
            int[] histogram = new int[_image.Width];
            //!!!!!!!!!!!! //_binarizationFilter.ProcessImage(_image);
            IImage grayscale = ConverToGrayBitmap(_image);
            _image = OtsuBinarize(grayscale);  // вывод на "дисплей" бинаризированного изображения 

            for (var i = 0; i < _image.Width; i++)
            {
                for (var j = 0; j < _image.Height; j++)
                {
                    if (_image.GetPixel(i, j).R == 255)
                    {
                        histogram[i]++;
                    }
                }
            }
            return histogram;
        }

        // Перевод изображения в серый тон
        public static IImage ConverToGrayBitmap(IImage original)
        {
            int alpha = 0, red = 0, green = 0, blue = 0, pixelLum = 0;
            Color newPixelColor;

            IImage lum = original.Clone();

            for (int i = 0; i < original.Width; i++)
            {
                for (int j = 0; j < original.Height; j++)
                {
                    // Get pixels by R, G, B
                    red = original.GetPixel(i, j).R;
                    green = original.GetPixel(i, j).G;
                    blue = original.GetPixel(i, j).B;
                    pixelLum = (int)(0.21 * red + 0.71 * green + 0.07 * blue);
                    // Return back to original format
                    newPixelColor = Color.FromArgb(alpha, pixelLum, pixelLum, pixelLum);
                    // Write pixels into image
                    lum.SetPixel(i, j, new Pixel(newPixelColor));
                }
            }
            return lum;
        }


        // Получение гистограммы изображения, переведённого в серые тона
        private static int[] ImageHistogram(IImage input)
        {
            int[] histogram = new int[256];

            for (int i = 0; i < histogram.Length; i++)
            {
                histogram[i] = 0;
            }

            for (int i = 0; i < input.Width; i++)
            {
                for (int j = 0; j < input.Height; j++)
                {
                    int red = input.GetPixel(i, j).R;
                    histogram[red]++;
                }
            }
            return histogram;
        }

        // вычисление порога для бинаризиации методом Отсу/Оцу
        private static int OtsuTreshold(IImage original)
        {

            int[] histogram = ImageHistogram(original);
            int total = original.Height * original.Width;

            float sum = 0;
            for (int i = 0; i < 256; i++)
            {
                sum += i * histogram[i];
            }

            float sumB = 0;
            int wB = 0, wF = 0, threshold = 0;
            float varMax = 0, mB = 0, mF = 0, varBetween = 0;

            for (int i = 0; i < 256; i++)
            {
                wB += histogram[i];
                if (wB == 0) continue;
                wF = total - wB;

                if (wF == 0) break;

                sumB += (float)(i * histogram[i]);
                mB = sumB / wB;
                mF = (sum - sumB) / wF;

                varBetween = (float)wB * (float)wF * (mB - mF) * (mB - mF);

                if (varBetween > varMax)
                {
                    varMax = varBetween;
                    threshold = i;
                }
            }

            return threshold;
        }

        // бинаризиация методом Отсу/Оцу
        public static IImage OtsuBinarize(IImage original)
        {
            int red = 0, newPixel = 0, alpha = 0;
            int threshold = OtsuTreshold(original);
            Color newPixelColor;

            IImage binarized = original.Clone();

            for (int i = 0; i < original.Width; i++)
            {
                for (int j = 0; j < original.Height; j++)
                {
                    red = original.GetPixel(i, j).R;
                    if (red > threshold) { newPixel = 255; }
                    else { newPixel = 0; }
                    newPixelColor = Color.FromArgb(alpha, newPixel, newPixel, newPixel);
                    binarized.SetPixel(i, j, new Pixel(newPixelColor));
                }
            }
            return binarized;
        }

    }
}