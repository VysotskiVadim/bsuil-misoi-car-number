using System.Collections.Generic;
using System.Linq;

namespace Bsuir.Misoi.Core.Images.Implementation.Image
{
    public class FindResultCliper : IFindResultsHandler
    {
        public void HandleFindResults(IImage image, IEnumerable<IFindResult> results)
        {
            var result = results.FirstOrDefault();
            if (result != null)
            {
                image.Clip(result.Points, result.Angle);
            }
        }
    }
}
