using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Bsuir.Misoi.Core.Images.Implementation
{
	public class BitmapImage : IImage
	{
		private readonly Bitmap _bitmap;

		public BitmapImage(Bitmap bitmap)
		{
			_bitmap = bitmap;
		}

		public string Name { get; set; }

		public Pixel GetPixel(int x, int y)
		{
			var color = _bitmap.GetPixel(x, y);
			return new Pixel(color);
		}

		public void Save(Stream saveStream)
		{
			var extension = Path.GetExtension(this.Name);
			ImageFormat imageFormat;
			if (extension == "png")
			{
				imageFormat = ImageFormat.Png;
			}
			else
			{
				imageFormat = ImageFormat.Jpeg;
			}
            _bitmap.Save(saveStream, imageFormat);
		}

		public void SetPixel(Pixel pixel, int x, int y)
		{
			var color = Color.FromArgb(0, pixel.R, pixel.G, pixel.B);
			_bitmap.SetPixel(x, y, color);
		}
	}
}
