using System.Collections.Generic;

namespace Bsuir.Misoi.Core.Images
{
    public interface IFindResultsHandler
    {
        void HandleFindResults(IImage image, IEnumerable<IFindResult> results);
    }
}
