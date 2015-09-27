using System.IO;

namespace Bsuir.Misoi.Core.Images
{
	public interface IImage
    {
		string Name { get; set; }

		Pixel GetPixel(int x, int y);

		void SetPixel(Pixel pixel, int x, int y);

		void Save(Stream saveStream);
	}
}
