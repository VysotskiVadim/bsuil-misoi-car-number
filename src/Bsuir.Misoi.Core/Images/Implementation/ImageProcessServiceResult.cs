using System;

namespace Bsuir.Misoi.Core.Images.Implementation
{
    internal class ImageProcessServiceResult : IProcessServiceResult
    {
        public string ErrorMessage { get; set; }

        public string Logs { get; set; }

        public IImage ProcessedImage { get; set; }

        public IImage SourceImage { get; set; }

        public bool Successful { get; set; }
    }
}
