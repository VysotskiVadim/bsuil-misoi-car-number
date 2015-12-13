using System;
using System.Collections.Generic;
using System.Linq;

namespace Bsuir.Misoi.Core.Images.Implementation
{
    public class FindResultCliper : IFindResultDrawer
    {
        public void DrawFindResults(IImage image, IEnumerable<IFindResult> results)
        {
            var result = results.FirstOrDefault();
            if (result != null)
            {
                image.Clip(result.Points, result.Angle);
            }
        }
    }
}
