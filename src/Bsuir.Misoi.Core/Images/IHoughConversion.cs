using System.Collections.Generic;
using Bsuir.Misoi.Core.Images.Implementation.Hough;

namespace Bsuir.Misoi.Core.Images
{
    public interface IHoughConversion
    {
        IList<Line> AccumulateLines(List<Point> points, double diagonal);
    }
}