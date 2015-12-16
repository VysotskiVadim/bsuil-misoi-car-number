using System.Collections.Generic;

namespace Bsuir.Misoi.Core.Images
{
    public interface IFindResultsHandler
    {
        void DrawFindResults(IImage image, IEnumerable<IFindResult> results);
    }
}
