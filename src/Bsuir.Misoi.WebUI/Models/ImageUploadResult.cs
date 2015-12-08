namespace Bsuir.Misoi.WebUI.Models
{
    public class ImageUploadResult
    {
        public ImageUploadResult(string url, string name)
        {
            this.Url = url;
            this.Name = name;
        }

        public string Url { get; }

        public string Name { get; }
    }
}
