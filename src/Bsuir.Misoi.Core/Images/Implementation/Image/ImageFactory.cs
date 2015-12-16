using System.IO;

namespace Bsuir.Misoi.Core.Images.Implementation.Image
{
    public class ImageFactory : IImageFactory
    {
        public IImage CreateImage(string name, Stream data)
        {
            IImage image = new BitmapImage(new System.Drawing.Bitmap(data));
            image.Name = name;
            return image;
        }
    }
}
