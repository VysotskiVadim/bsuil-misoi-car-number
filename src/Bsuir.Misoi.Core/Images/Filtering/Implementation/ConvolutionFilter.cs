using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Bsuir.Misoi.Core.Images.Filtering.Implementation
{
    public class ConvolutionFilter : IConvolutionFilter
    {
	    private const int Factor = 1;
	    private const int Bias = 0;

	    public void Filter(IImage image, double[,] filter)
	    {
		    if (filter.GetLength(0) != filter.GetLength(1))
		    {
			    throw new ArgumentException("invalid filter", nameof(filter));
		    }


		    var result = new Pixel[image.Width, image.Height];

			double blue = 0.0;
			double green = 0.0;
			double red = 0.0;

			int filterOffset = (filter.GetLength(0) - 1 ) / 2;
			for (int offsetY = filterOffset; offsetY < image.Height - filterOffset; offsetY++)
			{
				for (int offsetX = filterOffset; offsetX < image.Width - filterOffset; offsetX++)
				{
					blue = 0;
					green = 0;
					red = 0;

					for (int filterY = -filterOffset; filterY <= filterOffset; filterY++)
					{
						for (int filterX = -filterOffset; filterX <= filterOffset; filterX++)
						{
							var imagePixel = image.GetPixel(offsetX + filterX, offsetY + filterY);

							blue += (double)(imagePixel.B) * filter[filterY + filterOffset, filterX + filterOffset];
							green += (double)(imagePixel.G) * filter[filterY + filterOffset, filterX + filterOffset];
							red += (double)(imagePixel.R) * filter[filterY + filterOffset, filterX + filterOffset];
						}
					}

					blue = Factor * blue + Bias;
					green = Factor * green + Bias;
					red = Factor * red + Bias;

					result[offsetX, offsetY] = new Pixel { R = this.ToByte(red), G = this.ToByte(green), B = this.ToByte(blue) };
				}
			}

		    for (int x = 0; x < result.GetLength(0); x++)
		    {
			    for (int y = 0; y < result.GetLength(1); y++)
			    {
				    image.SetPixel(x, y, result[x, y]);
			    }
		    }
		}

	    private byte ToByte(double value)
	    {
		    if (value > 255)
		    {
			    value = 255;
		    }
		    else if (value < 0)
			{
			     value = 0;
		    }
		    return (byte)value;
	    }
    }
}
