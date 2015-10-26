using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Bsuir.Misoi.Core.Images.Selection;
using Bsuir.Misoi.Core.Storage;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using Microsoft.Net.Http.Headers;

namespace Bsuir.Misoi.WebUI.Controllers
{
    [Route("")]
    public class NumberSelectionController : Controller
    {
        private readonly ISelectionService _selectionService;
        private readonly IImageRepository _imageRepository;
        private readonly IImageDataProvider _imageDataProvider;

        public NumberSelectionController(ISelectionService selectionService, IImageRepository imageRepository, IImageDataProvider imageDataProvider)
        {
            _selectionService = selectionService;
            _imageRepository = imageRepository;
            _imageDataProvider = imageDataProvider;
        }

        [Route("")]
        [HttpGet]
        public IActionResult Index()
        {
            var filters = _selectionService.GetAllSelectors();
            return View(filters);
        }

        [Route("")]
        [HttpPost]
        public async Task<string> ProcessImage(IFormFile file, string selector)
        {
            var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Replace("\"", string.Empty);
            using (var fileStram = file.OpenReadStream())
            {
                var image = _selectionService.ApplySelector(selector, fileName, fileStram);
                image.Name = Guid.NewGuid() + Path.GetExtension(image.Name);
                await _imageRepository.SaveImageAsync(image);
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
