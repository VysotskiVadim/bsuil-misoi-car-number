using System.Drawing;

namespace Bsuir.Misoi.Core.Images
{
    public struct IImage
    {
        internal IImage(Color color)
        {
            this.R = color.R;
            this.G = color.G;
            this.B = color.B;
            this.XCoordinate = 0;
            this.YCoordinate = 0;


        }

        public byte R { get; set; }

        public byte G { get; set; }

        public byte B { get; set; }

        int XCoordinate { get; }

        int YCoordinate { get; }


    }
}
