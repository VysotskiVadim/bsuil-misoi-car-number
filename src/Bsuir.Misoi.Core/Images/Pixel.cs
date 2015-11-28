using System.Drawing;

namespace Bsuir.Misoi.Core.Images
{
    public struct Pixel
    {
        internal Pixel(Color color)
        {
            this.R = color.R;
            this.G = color.G;
            this.B = color.B;
        }

        public byte R { get; set; }

        public byte G { get; set; }

        public byte B { get; set; }
    }
}
