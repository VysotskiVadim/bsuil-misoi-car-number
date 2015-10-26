using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bsuir.Misoi.Core.Images.Selection.Implementation
{
    public class FakeSelector : ISelector
    {
        public string Name => "Fake Selector";

        public IEnumerable<ISelectionResult> Select(IImage image)
        {
            yield return new SelectionResult(20, 20, 40, 40);
        }
    }
}
