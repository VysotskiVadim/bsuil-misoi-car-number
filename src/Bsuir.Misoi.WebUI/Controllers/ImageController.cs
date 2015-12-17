using System;
using System.IO;
using System.Threading.Tasks;
using Bsuir.Misoi.Core;
using Bsuir.Misoi.Core.Images;
using Bsuir.Misoi.WebUI.Models;
using Bsuir.Misoi.WebUI.Storage;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using Microsoft.Net.Http.Headers;

namespace Bsuir.Misoi.WebUI.Controllers
{
    [Route("image")]
    public class ImageController : Controller
    {
        private readonly IImageDataProvider _imageDataProvider;
        private readonly IImageFactory _imageFactory;
        private readonly IImageRepository _imageRespository;
        private readonly IImageUrlProvider _imageUrlProvider;

        public ImageController(IImageDataProvider imageDataProvider, IImageFactory imageFactory, IImageRepository imageRespository, IImageUrlProvider imageUrlProvider)
        {
            _imageDataProvider = imageDataProvider;
            _imageFactory = imageFactory;
            _imageRespository = imageRespository;
            _imageUrlProvider = imageUrlProvider;
        }

        [Route("{name}")]
        public IActionResult GetImage(string name)
        {
            var image = _imageDataProvider.GetImage(name);
            return File(image, "img/" + Path.GetExtension(name).Substring(1));
        }

        [Route("upload")]
        [HttpPost]
        public async Task<ImageUploadResult> SaveImage(IFormFile file)
        {
            IImage image;
            using (var fileStram = file.OpenReadStream())
            {
                var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Replace("\"", string.Empty);
                string imageName = Guid.NewGuid() + Path.GetExtension(fileName);
                image = _imageFactory.CreateImage(imageName, fileStram);
            }
            await _imageRespository.SaveImageAsync(image);
            string imageUrl = _imageUrlProvider.GetImageUrl(image.Name);
            return new ImageUploadResult(imageUrl, image.Name);
        } 
    }
}
