using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using Microsoft.Net.Http.Headers;
using Bsuir.Misoi.Core.Images;
using System;
using System.IO;
using Bsuir.Misoi.WebUI.Storage;
using Bsuir.Misoi.WebUI.Models.ImageProcessors;

namespace Bsuir.Misoi.WebUI.Controllers
{
    [Route("processors")]
    public class ImageProcessorsController : Controller
    {
        private readonly IImageRepository _imageRepository;
        private readonly IImageProcessorsService _imageProcessorService;
        private readonly IImageFactory _imageFactory;
        private readonly IImageUrlProvider _imageUrlProvider;

        public ImageProcessorsController(
            IImageRepository imageRepository, 
            IImageProcessorsService imageProcessorService, 
            IImageFactory imageFactory,
            IImageUrlProvider imageUrlProvider)
        {
            _imageRepository = imageRepository;
            _imageProcessorService = imageProcessorService;
            _imageFactory = imageFactory;
            _imageUrlProvider = imageUrlProvider;
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
        public ImageProcessorsResult ProcessImage(IFormFile file, string processorName)
        {
            var result = new ImageProcessorsResult();
            var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Replace("\"", string.Empty);
            using (var fileStram = file.OpenReadStream())
            {
                var image = _imageFactory.CreateImage(fileName, fileStram);
                var processResult = _imageProcessorService.ProcessImageAsync(processorName, image).Result;
                if (processResult.Successful)
                {
                    processResult.ProcessedImage.Name = Guid.NewGuid() + Path.GetExtension(processResult.ProcessedImage.Name);
                    _imageRepository.SaveImageAsync(processResult.ProcessedImage).Wait();
                    result.ProcessedImageUrl = _imageUrlProvider.GetImageUrl(processResult.ProcessedImage.Name);
                    processResult.SourceImage.Name = Guid.NewGuid() + Path.GetExtension(processResult.SourceImage.Name);
                    _imageRepository.SaveImageAsync(processResult.SourceImage).Wait();
                    result.SourceImageUrl = _imageUrlProvider.GetImageUrl(processResult.SourceImage.Name);
                }
                return result;
            }
        }
    }
}
