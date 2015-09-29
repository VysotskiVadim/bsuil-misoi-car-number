using System;
using System.Collections.Generic;
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
		    var result = new Pixel[image.Width, image.Height];

			double blue = 0.0;
			double green = 0.0;
			double red = 0.0;

			int filterOffset = (filter.Length - 1 ) / 2;
			for (int offsetY = filterOffset; offsetY < image.Height - filterOffset; offsetY++)
			{
				for (int offsetX = filterOffset; offsetX < image.Width - filterOffset; offsetX++)
				{
					var imagePixel = image.GetPixel(offsetX, offsetY);
				
					for (int filterY = -filterOffset; filterY <= filterOffset; filterY++)
					{
						for (int filterX = -filterOffset; filterX <= filterOffset; filterX++)
						{
							blue += (double)(imagePixel.B) * filter[filterY + filterOffset, filterX + filterOffset];
							green += (double)(imagePixel.G) * filter[filterY + filterOffset, filterX + filterOffset];
							red += (double)(imagePixel.R) * filter[filterY + filterOffset, filterX + filterOffset];
						}
					}

					blue = Factor * blue + Bias;
					green = Factor * green + Bias;
					red = Factor * red + Bias;

					result[offsetX, offsetY].R = this.ToByte(red);
					result[offsetX, offsetY].G = this.ToByte(green);
					result[offsetX, offsetY].B = this.ToByte(blue);
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
