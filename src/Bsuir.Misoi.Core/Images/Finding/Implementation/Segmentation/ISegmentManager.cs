namespace Bsuir.Misoi.Core.Images.Finding.Implementation.Segmentation
{
    public interface ISegmentManager
    {
        void MarkSegment(int x, int y, int segmentId);

        int GetSegmentIdFor(int x, int y);

        int MarkNewSegment(int x, int y);

        ISegmentationResult BuildSegmentationResult();

        void MergeSegments(int firstSergmentId, int secondSegmentId);
    }
}
