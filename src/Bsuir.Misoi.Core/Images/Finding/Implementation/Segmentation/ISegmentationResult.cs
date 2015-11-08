using System.Collections.Generic;

namespace Bsuir.Misoi.Core.Images.Finding.Implementation.Segmentation
{
    public interface ISegmentationResult
    {
        int[,] SegmentationMatrix { get; }

        IList<ISegment> Segments { get; } 
    }
}
