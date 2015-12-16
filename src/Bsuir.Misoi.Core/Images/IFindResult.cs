using System.Collections.Generic;

namespace Bsuir.Misoi.Core.Images
{
    public interface IFindResult
    {
        IList<Point> Points { get; }

        /// <summary>
        /// Angle between X axe and top line
        /// </summary>
        float Angle { get; }
    }
}
