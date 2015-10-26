using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bsuir.Misoi.Core.Images.Filtering.Implementation
{
	public class FakeFilter : IFilter
	{
		public string Name => "fake filter";

		public void Filter(IImage image)
		{
			for(int x = 0; x < image.Width / 2; x++)
			{
				for (int y = 0; y < image.Height; y++)
				{
					image.SetPixel(x, y, new IImage());
				}
			}
		}
	}
}
