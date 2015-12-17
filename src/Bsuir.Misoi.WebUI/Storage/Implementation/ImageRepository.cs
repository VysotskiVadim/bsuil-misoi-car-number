using System.Threading.Tasks;
using Bsuir.Misoi.Core;
using Bsuir.Misoi.Core.Images;

namespace Bsuir.Misoi.WebUI.Storage.Implementation
{
    public class ImageRepository : IImageRepository
    {
        private readonly IImageDataProvider _imageDataProvider;
        private readonly IImageFactory _imageFactory;

        public ImageRepository(IImageDataProvider imageDataProvider, IImageFactory imageFactory)
        {
            _imageDataProvider = imageDataProvider;
            _imageFactory = imageFactory;
        }

        public Task<IImage> GetImageAsync(string name)
        {
            using (var imageStream = _imageDataProvider.GetImage(name))
            {
                var image = _imageFactory.CreateImage(name, imageStream);
                return Task.FromResult(image);
            }
        }

        public Task SaveImageAsync(IImage image)
        {
            using (var saveStream = _imageDataProvider.GetStreamForSaving(image.Name))
            {
                image.Save(saveStream);
                return Task.FromResult(true);
            }
        }
    }
}
