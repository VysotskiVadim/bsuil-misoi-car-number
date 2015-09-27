namespace Bsuir.Misoi.Core.Images
{
	public interface IImage
    {
		Pixel GetPixel(int x, int y);

		void SetPixel(Pixel pixel, int x, int y);
    }
}
