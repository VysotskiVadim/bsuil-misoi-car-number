using System.Collections.Generic;

namespace Bsuir.Misoi.Core.Images
{
    public interface ISegmentFindAnalyzer
    {
        IEnumerable<IFindResult> Find(ISegmentationResult segmentationResult);
    }
}
