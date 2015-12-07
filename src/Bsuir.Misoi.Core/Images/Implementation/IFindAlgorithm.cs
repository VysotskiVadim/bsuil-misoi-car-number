using System.Collections.Generic;

namespace Bsuir.Misoi.Core.Images.Implementation
{
    public interface IFindAlgorithm
    {
        string Name { get; }

        IEnumerable<IFindResult> Find(IImage image);
    }
}
