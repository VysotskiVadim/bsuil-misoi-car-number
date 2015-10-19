using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Http;
using System.IO;
using Bsuir.Misoi.Core.Images.Filtering;
using Microsoft.Net.Http.Headers;
using Bsuir.Misoi.Core.Storage;


namespace Bsuir.Misoi.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IFilterService _filterService;
        private readonly IImageRepository _imageRepository;
        private readonly IImageDataProvider _imageDataProvider;

        public HomeController(IFilterService filterService, IImageRepository imageRepository, IImageDataProvider imageDataProvider)
        {
            _filterService = filterService;
            _imageRepository = imageRepository;
            _imageDataProvider = imageDataProvider;
        }

        [Route("")]
        [HttpGet]
        public IActionResult Index()
        {
            var filters = _filterService.GetFilterNames();
            return View(filters);
        }

        [Route("")]
        [HttpPost]
        public async Task<string> Index(IFormFile file, string filter)
        {
            var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Replace("\"", string.Empty);
            var image = _filterService.ApplyFilter(filter, fileName, file.OpenReadStream());
            image.Name = Guid.NewGuid().ToString() + Path.GetExtension(image.Name);
            await _imageRepository.SaveImageAsync(image);
            return "image/" + image.Name;
        }

        [Route("image/{name}")]
        public IActionResult GetImage(string name)
        {
            var image = _imageDataProvider.GetImage(name);
            return File(image, "img/jpeg");
        }
    }
}
