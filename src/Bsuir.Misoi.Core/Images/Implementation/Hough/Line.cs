using System;

namespace Bsuir.Misoi.Core.Images.Implementation.Hough
{
    struct Line
    {
        public Line(int f, int r)
        {
            this.F = f;
            this.R = r;
            FRad = f * Math.PI / 180.0;
        }

        public int F { get; }   //угол
        public int R { get; }   //длина перпендикуляра
        public double FRad { get; }
    }
}
