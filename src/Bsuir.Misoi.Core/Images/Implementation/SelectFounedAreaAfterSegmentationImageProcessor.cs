using System.Collections.Generic;

namespace Bsuir.Misoi.Core.Images.Implementation
{
    public class SelectFounedAreaAfterSegmentationImageProcessor: IImageProcessor
    {
        private readonly IFindResultDrawer _findResultDrawer;
        private readonly ISegmentationAlgorithm _segmentationAlgorithm;
        private readonly ISegmentFindAnalyzer _segmentFindAnalyzer;
        private readonly IImageProcessor _edgeFilter;

        public SelectFounedAreaAfterSegmentationImageProcessor(IFindResultDrawer findResultDrawer, ISegmentationAlgorithm segmentationAlgorithm, ISegmentFindAnalyzer segmentFindAnalyzer, IImageProcessor edgeFilter)
        {
            _findResultDrawer = findResultDrawer;
            _segmentationAlgorithm = segmentationAlgorithm;
            _segmentFindAnalyzer = segmentFindAnalyzer;
            _edgeFilter = edgeFilter;
        }

        public string Name => _segmentFindAnalyzer.Name;

        public void ProcessImage(IImage image)
        {
            var imageForProcessing = image.Clone();
            //_edgeFilter.ProcessImage(imageForProcessing);
            var segments = _segmentationAlgorithm.ProcessImage(imageForProcessing);
            IEnumerable<IFindResult> findResult = _segmentFindAnalyzer.Find(segments);
            _findResultDrawer.DrawFindResults(image, findResult);
        }
    }
}
