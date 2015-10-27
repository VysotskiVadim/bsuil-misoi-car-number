using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Bsuir.Misoi.Core.Images.Finding;
using Bsuir.Misoi.Core.Storage;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using Microsoft.Net.Http.Headers;

namespace Bsuir.Misoi.WebUI.Controllers
{
    [Route("")]
    public class FindingController : Controller
    {
        private readonly IImageRepository _imageRepository;
        private readonly IImageDataProvider _imageDataProvider;
        private readonly IFindingService _findingService;

        public FindingController(IImageRepository imageRepository, IImageDataProvider imageDataProvider, IFindingService findingService)
        {
            _imageRepository = imageRepository;
            _imageDataProvider = imageDataProvider;
            _findingService = findingService;
        }

        [Route("")]
        [HttpGet]
        public IActionResult Index()
        {
            var filters = _findingService.GetAllSelectors();
            return View(filters);
        }

        [Route("")]
        [HttpPost]
        public string ProcessImage(IFormFile file, string selector)
        {
            var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Replace("\"", string.Empty);
            using (var fileStram = file.OpenReadStream())
            {
                var image = _findingService.ApplySelector(selector, fileName, fileStram);
                image.Name = Guid.NewGuid() + Path.GetExtension(image.Name);
                _imageRepository.SaveImageAsync(image).Wait();
                return "image/" + image.Name;
            }
        }

        [Route("image/{name}")]
        public IActionResult GetImage(string name)
        {
            var image = _imageDataProvider.GetImage(name);
            return File(image, "img/jpeg");
        }
    }
}
