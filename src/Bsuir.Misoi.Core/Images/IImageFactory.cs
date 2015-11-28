using System.IO;

namespace Bsuir.Misoi.Core.Images
{
    public interface IImageFactory
    {
        IImage CreateImage(string name, Stream data);
    }
}
