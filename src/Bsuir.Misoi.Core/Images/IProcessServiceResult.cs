namespace Bsuir.Misoi.Core.Images
{
    public interface IProcessServiceResult
    {
        bool Successful { get; }
        string ErrorMessage { get; }
        string Logs { get; }
        IImage ProcessedImage { get; }
        IImage SourceImage { get; }
    }
}
