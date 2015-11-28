using Bsuir.Misoi.Core.Storage;
using Microsoft.AspNet.Mvc;

namespace Bsuir.Misoi.WebUI.Controllers
{
    [Route("image")]
    public class ImageController : Controller
    {
        private IImageDataProvider _imageDataProvider;

        public ImageController(IImageDataProvider imageDataProvider)
        {
            _imageDataProvider = imageDataProvider;
        }

        [Route("{name}")]
        public IActionResult GetImage(string name)
        {
            var image = _imageDataProvider.GetImage(name);
            return File(image, "img/jpeg");
        }
    }
}
