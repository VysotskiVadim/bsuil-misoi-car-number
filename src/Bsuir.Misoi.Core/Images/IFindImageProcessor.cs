using System.Collections.Generic;

namespace Bsuir.Misoi.Core.Images
{
    public interface IFindImageProcessor
    {
        string Name { get; }

        IEnumerable<IFindResult> Find(IImage image);
    }
}
