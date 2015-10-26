using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Bsuir.Misoi.Core.Images.Selection.Implementation
{
    public class SelectionService : ISelectionService
    {
        private readonly IImageFactory _imageFactory;
        private readonly IEnumerable<ISelector> _selectors; 

        public SelectionService(IImageFactory imageFactory)
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
            var selectedAreas = _selectors.Single(f => f.Name == selector).Select(image);
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

        private IEnumerable<ISelector> Register()
        {
            yield return new FakeSelector();
        }
    }
}
