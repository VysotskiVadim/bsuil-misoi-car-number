using System.Collections.Generic;

namespace Bsuir.Misoi.Core.Images.Implementation
{
    public class TextFindProcessor : IFindImageProcessor
    {
        public string Name => "Text find processor";

        public IEnumerable<IFindResult> Find(IImage image)
        {
            yield return new FindResult(1, 1, 5, 5);
        }
    }
}
