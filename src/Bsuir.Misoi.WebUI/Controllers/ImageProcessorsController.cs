using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using Microsoft.Net.Http.Headers;
using Bsuir.Misoi.Core.Images;
using System;
using System.IO;
using Bsuir.Misoi.Core;
using Bsuir.Misoi.WebUI.Storage;
using Bsuir.Misoi.WebUI.Models;

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
        public ImageProcessorsResult ProcessImage(string processorName, string imageName) // We do all async operation sync because of easy exception handline, this controller only for developers.
        {
            var result = new ImageProcessorsResult();
            var sourceImage = _imageRepository.GetImageAsync(imageName).Result; 
            var processResult = _imageProcessorService.ProcessImageAsync(processorName, sourceImage).Result;
            if (processResult.Successful)
            {
                result.Successful = true;
                _imageRepository.SaveImageAsync(processResult.ProcessedImage).Wait();
                result.ProcessedImageUrl = _imageUrlProvider.GetImageUrl(processResult.ProcessedImage.Name);
                result.ProcessedImageName = processResult.ProcessedImage.Name;
                result.SourceImageUrl = _imageUrlProvider.GetImageUrl(sourceImage.Name);
                result.SourceImageName = sourceImage.Name;
            }
            else
            {
                result.Successful = false;
            }
            return result;
        }
    }
}

