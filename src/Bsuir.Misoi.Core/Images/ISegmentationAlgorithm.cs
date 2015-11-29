namespace Bsuir.Misoi.Core.Images
{
    public interface ISegmentationAlgorithm
    {
        ISegmentationResult ProcessImage(IImage image);
    }
}
