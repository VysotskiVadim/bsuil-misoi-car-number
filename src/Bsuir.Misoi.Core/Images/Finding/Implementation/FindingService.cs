using System.Collections.Generic;
using System.IO;
using System.Linq;
using Bsuir.Misoi.Core.Images.Filtering.Implementation;

namespace Bsuir.Misoi.Core.Images.Finding.Implementation
{
    public class FindingService : IFindingService
    {
        private readonly IImageFactory _imageFactory;
        private readonly IEnumerable<IFindAlgorithm> _selectors; 

        public FindingService(IImageFactory imageFactory)
        {
            _imageFactory = imageFactory;
            _selectors = this.Register();
        }

        public IEnumerable<string> GetAllSelectors()
        {
            return _selectors.Select(s => s.Name);
        }

        public IImage ApplySelector(string selector, string fileName, Stream fileStram)
        {
            var image = _imageFactory.CreateImage(fileName, fileStram);
            var selectedAreas = _selectors.Single(f => f.Name == selector).Find(image);
            foreach (var selectedArea in selectedAreas)
            {
                const int borderWidth = 3;
                int finishX = selectedArea.X + selectedArea.Width;
                int finishY = selectedArea.Y + selectedArea.Height;
                for (int x = selectedArea.X; x < finishX; x++)
                {
                    for (int y = selectedArea.Y; y < finishY; y++)
                    {
                        if ((y < selectedArea.Y + borderWidth || y + borderWidth > finishY) ||
                            (x < selectedArea.X + borderWidth || x + borderWidth > finishX))
                        {
                            image.SetPixel(x, y, new Pixel {R = 255, B = 0, G = 0});
                        }
                    }
                }
            }
            return image;
        }

        private IEnumerable<IFindAlgorithm> Register()
        {
            yield return new FakeFindAlgorithm();
            yield return new HoughLinesFindingAlgorithm(new BinarizationFilter());
        }
    }
}
