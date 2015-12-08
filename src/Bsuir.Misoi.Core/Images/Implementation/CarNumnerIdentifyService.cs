using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bsuir.Misoi.Core.Images.Implementation
{
    public class CarNumnerIdentifyService : ICarNumerIdentifyService
    {
        private readonly ISegmentationAlgorithm _segmentationAlgoritm;
        private readonly IFindResultDrawer _findResultDrawer;
        private readonly ISegmentFindAnalyzer _segmentFindAnalyzer;

        public CarNumnerIdentifyService(ISegmentationAlgorithm segmentationAlgoritm, IFindResultDrawer findResultDrawer, ISegmentFindAnalyzer segmentFindAnalyzer)
        {
            _segmentationAlgoritm = segmentationAlgoritm;
            _findResultDrawer = findResultDrawer;
            _segmentFindAnalyzer = segmentFindAnalyzer;
        }

        public Task<ICarNumberIdentifyResult> IdentifyAsync(IImage inputImage)
        {
            var result = new IdentifyIdentifyResult();

            var imageClone = inputImage.Clone();
            var segments = _segmentationAlgoritm.ProcessImage(imageClone);

            var selectedAreas = _segmentFindAnalyzer.Find(segments);

            _findResultDrawer.DrawFindResults(inputImage, selectedAreas);
            result.ProcessedImage = inputImage;

            return Task.FromResult((ICarNumberIdentifyResult)result);
        } 
    }
}
