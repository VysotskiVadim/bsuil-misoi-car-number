using System.Collections.Generic;

namespace Bsuir.Misoi.Core.Images.Selection
{
    public interface ISelector
    {
        string Name { get; }

        IEnumerable<ISelectionResult> Select(IImage image);
    }
}
