using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bsuir.Misoi.Core.Images.Filtering.Implementation
{
    public class LaplacianFiveFilter : IFilter
    {
	    private readonly IConvolutionFilter _convolutionFilter;

	    public LaplacianFiveFilter(IConvolutionFilter convolutionFilter)
	    {
		    _convolutionFilter = convolutionFilter;
	    }

	    public string Name => "Edge filter";

	    public void Filter(IImage image)
	    {
		    var filterMatrix = new double[,]
				{ { -1, -1, -1, -1, -1, },
				  { -1, -1, -1, -1, -1, },
				  { -1, -1, 24, -1, -1, },
				  { -1, -1, -1, -1, -1, },
				  { -1, -1, -1, -1, -1  }, };
			_convolutionFilter.Filter(image, filterMatrix);
		}
    }
}
