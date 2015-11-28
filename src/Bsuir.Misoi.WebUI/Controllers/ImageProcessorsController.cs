using Bsuir.Misoi.Core.Storage;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using Microsoft.Net.Http.Headers;
using Bsuir.Misoi.Core.Images;
using System;
using System.IO;

namespace Bsuir.Misoi.WebUI.Controllers
{
    [Route("processors")]
    public class ImageProcessorsController : Controller
    {
        private readonly IImageRepository _imageRepository;
        private readonly IImageProcessorsService _imageProcessorService;
        private readonly IImageFactory _imageFactory;

        public ImageProcessorsController(IImageRepository imageRepository, IImageProcessorsService imageProcessorService, IImageFactory imageFactory)
        {
            _imageRepository = imageRepository;
            _imageProcessorService = imageProcessorService;
            _imageFactory = imageFactory;
        }

        [Route("")]
        [HttpGet]
        public IActionResult Index()
        {
            var filters = _imageProcessorService.GetProcessorsNames();
            return View(filters);
        }

        [Route("")]
        [HttpPost]
        public string ProcessImage(IFormFile file, string processorName)
        {
            var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Replace("\"", string.Empty);
            using (var fileStram = file.OpenReadStream())
            {
                var image = _imageFactory.CreateImage(fileName, fileStram);
                var processResult = _imageProcessorService.ProcessImageAsync(processorName, image).Result;
                if (processResult.Successful)
                {
                    var processedImage = processResult.ProcessedImage;
                    processedImage.Name = Guid.NewGuid() + Path.GetExtension(processedImage.Name);
                    _imageRepository.SaveImageAsync(processedImage).Wait();
                    return "/image/" + processedImage.Name;
                }
                return string.Empty;
            }
        }
    }
}
