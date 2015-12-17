using System.Collections.Generic;
using System.Linq;

namespace Bsuir.Misoi.Core.Images.Implementation.Image
{
    public class SaveClipedResult : IFindResultsHandler
    {
        private readonly IImageRepository _imageRepository;

        public SaveClipedResult(IImageRepository imageRepository)
        {
            _imageRepository = imageRepository;
        }

        public void HandleFindResults(IImage image, IEnumerable<IFindResult> results)
        {
            var findResults = results as IFindResult[] ?? results.ToArray();
            foreach (var findResult in findResults)
            {
                var coppy = image.Clone();
                coppy.Clip(findResult.Points, findResult.Angle);
                _imageRepository.SaveImageAsync(coppy).Wait();
            }
        }
    }
}
