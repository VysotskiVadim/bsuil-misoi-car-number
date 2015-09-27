using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bsuir.Misoi.Core.Images;
using Bsuir.Misoi.Core.Images.Implementation;

namespace Bsuir.Misoi.Core.Storage.Implementation
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
			var imageStream = _imageDataProvider.GetImage(name);
			var image = _imageFactory.CreateImage(name, imageStream);
			return Task.FromResult(image);
		}

		public Task SaveImageAsync(IImage image)
		{
			var saveStream = _imageDataProvider.GetStreamForSaving(image.Name);
			image.Save(saveStream);
			return Task.FromResult(true);
		}
	}
}
