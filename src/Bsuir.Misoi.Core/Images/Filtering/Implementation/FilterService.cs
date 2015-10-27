using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Bsuir.Misoi.Core.Images.Finding.Implementation;

namespace Bsuir.Misoi.Core.Images.Filtering.Implementation
{
    public class FilterService : IFilterService
    {
        private readonly IList<IFilter> _filters = new List<IFilter>();
        private readonly IImageFactory _imageFactory;

        public FilterService(IImageFactory imageFactory)
        {
            _imageFactory = imageFactory;
            this.RegiterFilters(_filters);
        }

        public IImage ApplyFilter(string filterName, string imageName, Stream imageData)
        {
            var image = _imageFactory.CreateImage(imageName, imageData);
            _filters.Single(f => f.Name == filterName).Filter(image);
            return image;
        }

        public IList<string> GetFilterNames()
        {
            return _filters.Select(f => f.Name).ToList();
        }

        private void RegiterFilters(IList<IFilter> filters)
        {
            filters.Add(new BinarizationFilter());
            filters.Add(new GammaFilter());
            filters.Add(new LaplacianFiveFilter(new ConvolutionFilter()));
            filters.Add(new MedianFilter());
        }
    }
}
