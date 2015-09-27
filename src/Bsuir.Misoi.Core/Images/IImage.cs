namespace Bsuir.Misoi.Core.Images
{
	public interface IImage
    {
		IPixel GetPixel(int x, int y);

		void SetPixel(IPixel pixel, int x, int y);
    }
}
