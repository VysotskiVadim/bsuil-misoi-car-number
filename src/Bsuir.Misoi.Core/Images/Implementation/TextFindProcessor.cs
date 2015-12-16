using System.Collections.Generic;
using System.Linq;
using Bsuir.Misoi.Core.Images.Implementation.Image;

namespace Bsuir.Misoi.Core.Images.Implementation
{
    public class TextFindProcessor : IFindAlgoritm
    {
        private readonly IBinarizationFilter _binarizationFilter;

        public TextFindProcessor(IBinarizationFilter binarizationFilter)
        {
            _binarizationFilter = binarizationFilter;
        }

        public string Name => "Text find processor";

        public IEnumerable<IFindResult> Find(IImage image)
        {
            var histogram = CalculateLineHistogram(image);
            int threashold = (int)(0.95 * histogram.Max());

            int startX = 0, endX = 0;
            bool inSegment = false;
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
                    yield return new FindResult(startX, 0, image.Height - 1, endX - startX);
                }
            }
        }

        public int[] CalculateLineHistogram(IImage image)
        {
            int[] histogram = new int[image.Width];;
            ConverToGrayColors(image);
            _binarizationFilter.ProcessImage(image);

            for (var i = 0; i < image.Width; i++)
            {
                for (var j = 0; j < image.Height; j++)
                {
                    if (image.GetPixel(i, j).R == 255)
                    {
                        histogram[i]++;
                    }
                }
            }
            return histogram;
        }

        public void ConverToGrayColors(IImage image)
        {
            var original = image.Clone();

            for (int i = 0; i < original.Width; i++)
            {
                for (int j = 0; j < original.Height; j++)
                {
                    var red = original.GetPixel(i, j).R;
                    var green = original.GetPixel(i, j).G;
                    var blue = original.GetPixel(i, j).B;
                    var pixelLum = (byte)(0.21 * red + 0.71 * green + 0.07 * blue);
                    image.SetPixel(i, j, new Pixel { R = pixelLum, G = pixelLum, B = pixelLum });
                }
            }
        }
    }
}