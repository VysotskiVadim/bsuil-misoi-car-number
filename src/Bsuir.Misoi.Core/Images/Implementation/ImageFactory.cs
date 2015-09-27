using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Bsuir.Misoi.Core.Images.Implementation
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
