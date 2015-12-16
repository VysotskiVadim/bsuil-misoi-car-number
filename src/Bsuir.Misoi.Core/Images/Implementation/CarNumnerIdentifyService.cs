using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bsuir.Misoi.Core.Images.Implementation
{
    public class CarNumnerIdentifyService : ICarNumerIdentifyService
    {
        private readonly ISegmentationAlgorithm _segmentationAlgoritm;
        private readonly IFindResultsHandler _findResultsHandler;
        private readonly ISegmentFindAnalyzer _segmentFindAnalyzer;

        public CarNumnerIdentifyService(ISegmentationAlgorithm segmentationAlgoritm, IFindResultsHandler findResultsHandler, ISegmentFindAnalyzer segmentFindAnalyzer)
        {
            _segmentationAlgoritm = segmentationAlgoritm;
            _findResultsHandler = findResultsHandler;
            _segmentFindAnalyzer = segmentFindAnalyzer;
        }

        public Task<IIdentifyResult> IdentifyAsync(IImage inputImage)
        {
            var result = new IdentifyResult();

            var imageClone = inputImage.Clone();
            var segments = _segmentationAlgoritm.ProcessImage(imageClone);

            var selectedAreas = _segmentFindAnalyzer.Find(segments);

            _findResultsHandler.DrawFindResults(inputImage, selectedAreas);
            result.ProcessedImage = inputImage;

            return Task.FromResult((IIdentifyResult)result);
        } 
    }
}
