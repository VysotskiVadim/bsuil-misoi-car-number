using System.Collections.Generic;
using System.IO;

namespace Bsuir.Misoi.Core.Images.Finding
{
    public interface IFindingService
    {
        IEnumerable<string> GetAllSelectors();
        IImage ApplySelector(string selector, string fileName, Stream fileStram);
    }
}
