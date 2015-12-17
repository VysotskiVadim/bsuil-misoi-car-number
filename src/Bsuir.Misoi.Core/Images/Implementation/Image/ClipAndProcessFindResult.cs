using System.Collections.Generic;
using System.Linq;

namespace Bsuir.Misoi.Core.Images.Implementation.Image
{
    public class ClipAndProcessFindResult : IFindResultsHandler
    {
        private readonly IFindAlgoritm _findAlgoritm;
        private readonly IFindResultsHandler _findResultsHandler;

        public ClipAndProcessFindResult(IFindAlgoritm findAlgoritm, IFindResultsHandler findResultsHandler)
        {
            _findAlgoritm = findAlgoritm;
            _findResultsHandler = findResultsHandler;
        }

        public void HandleFindResults(IImage image, IEnumerable<IFindResult> results)
        {
            foreach (var result in results)
            {
                var clone = image.Clone();
                clone.Clip(result.Points, result.Angle);
                var partFindResult = _findAlgoritm.Find(clone);
                _findResultsHandler.HandleFindResults(clone, partFindResult);
            }
        }
    }
}
