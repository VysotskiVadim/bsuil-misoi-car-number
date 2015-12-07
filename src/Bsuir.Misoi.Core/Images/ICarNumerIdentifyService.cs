using System.Threading.Tasks;

namespace Bsuir.Misoi.Core.Images
{
    public interface ICarNumerIdentifyService
    {
        Task<ICarNumberIdentifyResult> IdentifyAsync(IImage image);
    }
}
