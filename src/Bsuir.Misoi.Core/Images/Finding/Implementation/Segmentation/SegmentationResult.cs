using System.Collections.Generic;

namespace Bsuir.Misoi.Core.Images.Finding.Implementation.Segmentation
{
    public class SegmentationResult : ISegmentationResult
    {
        public SegmentationResult(int[,] segmentationMatrix, IList<ISegment> segments)
        {
            SegmentationMatrix = segmentationMatrix;
            Segments = segments;
        }

        public int[,] SegmentationMatrix { get; }

        public IList<ISegment> Segments { get; }
    }
}
