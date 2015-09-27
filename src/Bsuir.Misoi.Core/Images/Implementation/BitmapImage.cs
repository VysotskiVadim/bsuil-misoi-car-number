using System;
using System.Drawing;

namespace Bsuir.Misoi.Core.Images.Implementation
{
	public class BitmapImage : IImage
	{
		private readonly Bitmap _bitmap;

		public BitmapImage(Bitmap bitmap)
		{
			_bitmap = bitmap;
		}

		public Pixel GetPixel(int x, int y)
		{
			var color = _bitmap.GetPixel(x, y);
			return new Pixel(color);
		}

		public void SetPixel(Pixel pixel, int x, int y)
		{
			var color = Color.FromArgb(0, pixel.R, pixel.G, pixel.B);
			_bitmap.SetPixel(x, y, color);
		}
	}
}
