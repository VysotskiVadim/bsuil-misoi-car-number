namespace Bsuir.Misoi.Core.Images.Implementation.Segmentation
{
    internal class Segment : ISegment
    {
        public int Id { get; set; }

        public int Rank { get; set; }

        public int MergedWithSegmentId { get; set; }

        public int Square { get; set; }
    }
}
