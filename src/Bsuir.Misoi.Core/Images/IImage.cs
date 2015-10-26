using System.IO;

namespace Bsuir.Misoi.Core.Images
{
	public interface IImage
    {
		string Name { get; set; }

		IImage GetPixel(int x, int y);

		void SetPixel(int x, int y, IImage pixel);

		void Save(Stream saveStream);

		int Height { get; }

		int Width { get; }
	}
}
