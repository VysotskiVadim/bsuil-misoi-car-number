using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bsuir.Misoi.Core.Images.Filtering;

namespace Bsuir.Misoi.Core.Images.Finding.Implementation
{
    public class HoughLinesFindingAlgorithm : IFindAlgorithm
    {
        private readonly IFilter _binarizationFilter;

        public HoughLinesFindingAlgorithm(IFilter binarizationFilter)
        {
            _binarizationFilter = binarizationFilter;
        }

        public string Name => "Hough Lines Finding";

        public IEnumerable<IFindResult> Find(IImage image)
        {
            _binarizationFilter.Filter(image);
            yield return new FindResult(10, 10, 50, 50);
        }
    }
}
