using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;

namespace Bsuir.Misoi.Core.Images.Filtering.Implementation
{
	public class BinarizationFilter : IFilter
	{
		public string Name => "adaptive binarization";

		public void Filter(IImage image)
		{
			for (int x = 0; x < image.Width; x++)
			{
				for (int y = 0; y < image.Height; y++)
				{
					var middle = 20;
					var pixel = image.GetPixel(x, y);
					var rgb = (int)((pixel.R + pixel.G + pixel.B) / 3);
					
					if (rgb > middle)
					{
						image.SetPixel(x, y, new Pixel{R = 255, G = 255, B = 255});
					}
					else{
						image.SetPixel(x, y, new Pixel());
					}
				}
			}
		}
		
		private double GetMiddlePixelArea(int x, int y, IImage image)
		{
			var radius = 10;
			
			var left = (x - radius < 0) ? 0 : x - radius;
			var top = (y - radius < 0) ? 0 : y - radius;
			var right = (x + radius < image.Width) ? x + radius : image.Width - 1;
			var bottom = (y + radius < image.Height) ? y + radius : image.Height - 1;
			
			var intensity = 0;
			var count = 0;
			
			for (int i = left; i < right; i++)
			{
				for (int j = top; j < bottom; j++)
				{
					var pixel = image.GetPixel(i, j);
					var rgb = (int)((pixel.R + pixel.G + pixel.B) / 3);
					intensity += rgb;
					count++;
				}
			}
			
			return (double)(intensity / count);
		}
	}
}
