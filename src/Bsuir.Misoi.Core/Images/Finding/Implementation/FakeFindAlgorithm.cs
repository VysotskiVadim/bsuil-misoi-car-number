using System.Collections.Generic;

namespace Bsuir.Misoi.Core.Images.Finding.Implementation
{
    public class FakeFindAlgorithm : IFindAlgorithm
    {
        public string Name => "Fake Selector";

        public IEnumerable<IFindResult> Find(IImage image)
        {
            yield return new FindResult(20, 20, 40, 40);
        }
    }
}
