using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bsuir.Misoi.Core.Images
{
    public interface IImageProcessorsService
    {
        IEnumerable<string> GetProcessorsNames();

        Task<IProcessServiceResult> ProcessImageAsync(string processorName, IImage image);
    }
}
