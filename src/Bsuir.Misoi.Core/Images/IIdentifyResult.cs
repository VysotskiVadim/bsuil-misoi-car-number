namespace Bsuir.Misoi.Core.Images
{
    public interface IIdentifyResult
    {
        IImage ProcessedImage { get; }

        string CarNumber { get; }
    }
}
