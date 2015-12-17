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
    [Route("")]
    public class CarNumberController : Controller
    {
        private readonly IImageFactory _imageFactory;
        private readonly ICarNumerIdentifyService _identifyService;
        private readonly IImageRepository _imageRepository;
        private readonly IImageUrlProvider _imageUrlProvider;

        public CarNumberController(
            IImageFactory imageFactory, 
            ICarNumerIdentifyService identifyService, 
            IImageRepository imageRepository,
            IImageUrlProvider imageUrlProvider)
        {
            _imageFactory = imageFactory;
            _identifyService = identifyService;
            _imageRepository = imageRepository;
            _imageUrlProvider = imageUrlProvider;
        }

        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("")]
        [HttpPost]
        public CarNumberResult ProcessImage(IFormFile file)
        {
            var result = new CarNumberResult();
            var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Replace("\"", string.Empty);
            using (var fileStram = file.OpenReadStream())
            {
                var image = _imageFactory.CreateImage(fileName, fileStram);
                var processResult = _identifyService.IdentifyAsync(image).Result;
                processResult.ProcessedImage.Name = Guid.NewGuid() + Path.GetExtension(processResult.ProcessedImage.Name);
                _imageRepository.SaveImageAsync(processResult.ProcessedImage).Wait();
                result.ImageUrl = _imageUrlProvider.GetImageUrl(processResult.ProcessedImage.Name);
            }
            return result;
        }
    }
}
