namespace Bsuir.Misoi.Core.Images
{
    public interface IImageProcessor
    {
        string Name { get; }

        void ProcessImage(IImage image);
    }
}
