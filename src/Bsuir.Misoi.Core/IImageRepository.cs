using System.Threading.Tasks;
using Bsuir.Misoi.Core.Images;

namespace Bsuir.Misoi.Core
{
    public interface IImageRepository
    {
        Task SaveImageAsync(IImage image);

        Task<IImage> GetImageAsync(string name);
    }
}
