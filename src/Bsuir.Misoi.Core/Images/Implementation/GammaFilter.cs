using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace Bsuir.Misoi.Core.Images.Implementation
{
    public class GammaFilter : IImageProcessor
    {
        public string Name => "Gamma filter";

        public double GammaValue => 4;

        public void ProcessImage(IImage image)
        {
            Pixel c;

            byte[] gammaArray = CreateGammaArray(GammaValue);

            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    c = image.GetPixel(i, j);
                    image.SetPixel(i, j, new Pixel { R = gammaArray[c.R], G = gammaArray[c.G], B = gammaArray[c.B] });
                }
            }
        }

        private byte[] CreateGammaArray(double color)
        {
            byte[] gammaArray = new byte[256];
            for (int i = 0; i < 256; ++i)
            {
                gammaArray[i] = (byte)Math.Min(255,
        (int)((255.0 * Math.Pow(i / 255.0, 1.0 / color)) + 0.5));
            }
            return gammaArray;
        }

        
    }
}
