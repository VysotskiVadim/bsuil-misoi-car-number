using System.Collections.Generic;

namespace Bsuir.Misoi.Core.Images.Implementation.Segmentation
{
    public class AnalyzeSegmentsFindAloritm : IFindAlgoritm
    {
        private readonly ISegmentationAlgorithm _segmentationAlgoritm;
        private readonly ISegmentFindAnalyzer _segmentFindAnalyzer;

        public AnalyzeSegmentsFindAloritm(ISegmentationAlgorithm segmentationAlgoritm, ISegmentFindAnalyzer segmentFindAnalyzer, string name)
        {
            _segmentationAlgoritm = segmentationAlgoritm;
            _segmentFindAnalyzer = segmentFindAnalyzer;
            Name = name;
        }

        public string Name { get; }

        public IEnumerable<IFindResult> Find(IImage image)
        {
            var segments = _segmentationAlgoritm.ProcessImage(image);
            return _segmentFindAnalyzer.Find(segments);
        }
    }
}
