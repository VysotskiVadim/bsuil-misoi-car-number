using System.Collections.Generic;

namespace Bsuir.Misoi.Core.Images.Implementation
{
    public class SelectFoundedAreaImageProcessor : IImageProcessor
    {
        private readonly IFindImageProcessor _findImageProcessor;
        private readonly IFindResultDrawer _findResultDrawer;

        public SelectFoundedAreaImageProcessor(IFindImageProcessor findImageProcessor, IFindResultDrawer findResultDrawer)
        {
            _findImageProcessor = findImageProcessor;
            _findResultDrawer = findResultDrawer;
        }

        public string Name => _findImageProcessor.Name;

        public void ProcessImage(IImage image)
        {
            IEnumerable<IFindResult> findResult = _findImageProcessor.Find(image);
            _findResultDrawer.DrawFindResults(image, findResult);
        }
    }
}
