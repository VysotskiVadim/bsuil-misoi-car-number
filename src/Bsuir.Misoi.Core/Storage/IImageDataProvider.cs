using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Bsuir.Misoi.Core.Storage
{
	public interface IImageDataProvider
	{
		Stream GetImage(string name);

		Stream GetStreamForSaving(string name);

		Task<IEnumerable<string>> GetAllImagesNames();
	}
}
