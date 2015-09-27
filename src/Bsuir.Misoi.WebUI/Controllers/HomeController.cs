using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Http;
using System.IO;
using Bsuir.Misoi.Core.Images.Filtering;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Bsuir.Misoi.WebUI.Controllers
{
	public class HomeController : Controller
	{
		private readonly IFilterService _filterService;

		public HomeController(IFilterService filterService)
		{
			_filterService = filterService;
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
		public IActionResult Index(IFormFile file, string filter)
		{
			var image = _filterService.ApplyFilter(filter, "somename.jpg", file.OpenReadStream());
			using (MemoryStream ms = new MemoryStream())
			{
				image.Save(ms);
				return this.File(ms.ToArray(), file.ContentType);
			}
		}
	}
}
