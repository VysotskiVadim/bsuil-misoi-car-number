using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Bsuir.Misoi.Core.Images.Filtering
{
    public interface IFilterService
    {
        IList<string> GetFilterNames();

        IImage ApplyFilter(string filterName, string imageName, Stream imageData);
    }
}
