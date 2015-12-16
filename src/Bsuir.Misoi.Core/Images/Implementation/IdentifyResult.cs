namespace Bsuir.Misoi.Core.Images.Implementation
{
    internal class IdentifyResult : IIdentifyResult
    {
        public IImage ProcessedImage { get; set; }
        public string CarNumber { get; set; }
    }
}
