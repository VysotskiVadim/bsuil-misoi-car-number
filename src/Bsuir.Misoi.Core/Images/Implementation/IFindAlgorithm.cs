using System.Collections.Generic;

namespace Bsuir.Misoi.Core.Images.Finding
{
    public interface IFindAlgorithm
    {
        string Name { get; }

        IEnumerable<IFindResult> Find(IImage image);
    }
}
