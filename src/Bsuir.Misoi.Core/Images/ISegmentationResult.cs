using System.Collections.Generic;

namespace Bsuir.Misoi.Core.Images
{
    public interface ISegmentationResult
    {
        int[,] SegmentationMatrix { get; }

        IList<ISegment> Segments { get; } 
    }
}
