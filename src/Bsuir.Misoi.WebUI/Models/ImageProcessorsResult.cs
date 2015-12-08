namespace Bsuir.Misoi.WebUI.Models
{
    public class ImageProcessorsResult
    {
        public string SourceImageUrl { get; set; }
        public string ProcessedImageUrl { get; set; }
        public bool Successful { get; set; }
        public string ProcessedImageName { get; set; }
        public string SourceImageName { get; set; }
    }
}
