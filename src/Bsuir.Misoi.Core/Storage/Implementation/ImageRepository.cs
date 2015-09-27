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

		public ImageRepository(IImageDataProvider imageDataProvider)
		{
			_imageDataProvider = imageDataProvider;
        }

		public Task<IImage> GetImageAsync(string name)
		{
			var imageStream = _imageDataProvider.GetImage(name);
			IImage image = new BitmapImage(new System.Drawing.Bitmap(imageStream));
			image.Name = name;
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
