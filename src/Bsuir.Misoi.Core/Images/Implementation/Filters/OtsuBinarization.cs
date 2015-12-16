namespace Bsuir.Misoi.Core.Images.Implementation.Filters
{
    public class OtsuBinarization : IBinarizationFilter
    {
        public string Name => "Otsu binarization";

        public void ProcessImage(IImage image)
        {
            byte threshold = OtsuTreshold(image);
            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    byte red = image.GetPixel(i, j).R;
                    byte newPixel = red > threshold ? (byte)255 : (byte)0;
                    image.SetPixel(i, j, new Pixel { B = newPixel, G = newPixel, R = newPixel });
                }
            }
        }

        private byte OtsuTreshold(IImage original)
        {

            int[] histogram = ImageHistogram(original);
            int total = original.Height * original.Width;

            float sum = 0;
            for (int i = 0; i < 256; i++)
            {
                sum += i * histogram[i];
            }

            float sumB = 0;
            int wB = 0, wF = 0;
            byte threshold = 0;
            float varMax = 0, mB = 0, mF = 0, varBetween = 0;

            for (byte i = 0; i < 255; i++)
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

        private int[] ImageHistogram(IImage input)
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
    }
}
