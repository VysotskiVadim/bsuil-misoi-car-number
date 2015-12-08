using System.Collections.Generic;

namespace Bsuir.Misoi.Core.Images.Implementation
{
    public class SelectFounedAreaAfterSegmentationImageProcessor: IImageProcessor
    {
        private readonly IFindResultDrawer _findResultDrawer;
        private readonly ISegmentationAlgorithm _segmentationAlgorithm;
        private readonly ISegmentFindAnalyzer _segmentFindAnalyzer;

        public SelectFounedAreaAfterSegmentationImageProcessor(IFindResultDrawer findResultDrawer, ISegmentationAlgorithm segmentationAlgorithm, ISegmentFindAnalyzer segmentFindAnalyzer)
        {
            _findResultDrawer = findResultDrawer;
            _segmentationAlgorithm = segmentationAlgorithm;
            _segmentFindAnalyzer = segmentFindAnalyzer;
        }

        public string Name => _segmentFindAnalyzer.Name;

        public void ProcessImage(IImage image)
        {
            var imageForProcessing = image.Clone();
            var segments = _segmentationAlgorithm.ProcessImage(imageForProcessing);
            IEnumerable<IFindResult> findResult = _segmentFindAnalyzer.Find(segments);
            _findResultDrawer.DrawFindResults(image, findResult);
        }
    }
}
