using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

namespace Bsuir.Misoi.Core.Images.Implementation
{
	public class BitmapImage : IImage
	{
		private byte[] _pixelBuffer;
		private int _bitMapStride;
		private int _height;
		private int _width;

		public BitmapImage(Bitmap bitmap)
		{
			_height = bitmap.Height;
			_width = bitmap.Width;
			BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
													ImageLockMode.ReadOnly,
												    PixelFormat.Format32bppArgb);
			_bitMapStride = bitmapData.Stride;
            _pixelBuffer = new byte[_bitMapStride * bitmap.Height];
			Marshal.Copy(bitmapData.Scan0, _pixelBuffer, 0, _pixelBuffer.Length);
			bitmap.UnlockBits(bitmapData);
		}

		public int Height => _height;

		public string Name { get; set; }

		public int Width => _width;

		public Pixel GetPixel(int x, int y)
		{
			int byteOffset = y * _bitMapStride + x * 4;
			return new Pixel { B = _pixelBuffer[byteOffset], G = _pixelBuffer[byteOffset + 1], R = _pixelBuffer[byteOffset + 2] };
		}

		public void Save(Stream saveStream)
		{
			Bitmap resultBitmap = new Bitmap(_width, _height);

			BitmapData resultData = resultBitmap.LockBits(new Rectangle(0, 0,
									 resultBitmap.Width, resultBitmap.Height),
													  ImageLockMode.WriteOnly,
												 PixelFormat.Format32bppArgb);

			Marshal.Copy(_pixelBuffer, 0, resultData.Scan0, _pixelBuffer.Length);
			resultBitmap.UnlockBits(resultData);

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
            resultBitmap.Save(saveStream, imageFormat);
		}

		public void SetPixel(int x, int y, Pixel pixel)
		{
			int byteOffset = y * _bitMapStride + x * 4;
			_pixelBuffer[byteOffset] = pixel.B;
			_pixelBuffer[byteOffset + 1] = pixel.G;
			_pixelBuffer[byteOffset + 2] = pixel.R;
		}
	}
}
