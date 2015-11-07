using System.Collections.Generic;
using System.IO;
using System.Linq;
using Bsuir.Misoi.Core.Images.Filtering.Implementation;
using Bsuir.Misoi.Core.Images.Finding.Implementation.Segmentation;

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
                if (selectedArea.Points.Count > 3)
                {
                    DrawLineAlgorithm.PlotFunction plotFunction = (x, y) =>
                    {
                        image.SetPixel(x, y, new Pixel { R = 255, G = 0, B = 0 });
                        return true;
                    };
                    var firstPoint = selectedArea.Points[0];
                    var previousPoint = firstPoint;
                    for (int i = 1; i < selectedArea.Points.Count; i++)
                    {
                        var currentPoint = selectedArea.Points[i];
                        DrawLineAlgorithm.Line(previousPoint.X, previousPoint.Y, currentPoint.X, currentPoint.Y, plotFunction);
                        previousPoint = currentPoint;
                    }
                    var lastPoint = selectedArea.Points[selectedArea.Points.Count - 1];
                    DrawLineAlgorithm.Line(lastPoint.X, lastPoint.Y, firstPoint.X, firstPoint.Y, plotFunction);
                }
            }
            return image;
        }


        private IEnumerable<IFindAlgorithm> Register()
        {
            yield return new FakeFindAlgorithm();
            yield return new HoughLinesFindingAlgorithm(new BinarizationFilter());
            yield return new SegmentationAlgorithm(new BinarizationFilter());
        }
    }
}
