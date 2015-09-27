using Bsuir.Misoi.Core.Images;
using System.Threading.Tasks;

namespace Bsuir.Misoi.Core.Storage
{
	public interface IImageRepository
    {
		Task SaveImageAsync(IImage image);

		Task<IImage> GetImageAsync(string name);
    }
}
