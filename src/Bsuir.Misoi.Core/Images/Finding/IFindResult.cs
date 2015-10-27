using System.Collections.Generic;

namespace Bsuir.Misoi.Core.Images.Finding
{
    public interface IFindResult
    {
        IList<Point> Points { get; }
    }
}
