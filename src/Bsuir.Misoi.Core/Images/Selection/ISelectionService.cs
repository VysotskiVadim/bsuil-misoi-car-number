using System.Collections.Generic;
using System.IO;

namespace Bsuir.Misoi.Core.Images.Selection
{
    public interface ISelectionService
    {
        IEnumerable<string> GetAllSelectors();
        IImage ApplySelector(string selector, string fileName, Stream fileStram);
    }
}
