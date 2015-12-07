using System.Collections.Generic;
using Bsuir.Misoi.Core.Images.Implementation;

namespace Bsuir.Misoi.Core.Images
{
    public interface IFindResultDrawer
    {
        void DrawFindResults(IImage image, IEnumerable<IFindResult> results);
    }
}
