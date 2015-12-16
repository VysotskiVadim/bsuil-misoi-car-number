using System.Threading.Tasks;

namespace Bsuir.Misoi.Core.Images
{
    public interface ICarNumerIdentifyService
    {
        Task<IIdentifyResult> IdentifyAsync(IImage image);
    }
}
