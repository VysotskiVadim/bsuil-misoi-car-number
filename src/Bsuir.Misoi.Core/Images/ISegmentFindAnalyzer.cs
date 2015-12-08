using System.Collections.Generic;

namespace Bsuir.Misoi.Core.Images
{
    public interface ISegmentFindAnalyzer
    {
        string Name { get; }

        IEnumerable<IFindResult> Find(ISegmentationResult segmentationResult);
    }
}
