using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Http;
using System.IO;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Bsuir.Misoi.WebUI.Controllers
{
	public class HomeController : Controller
	{
		[Route("")]
		[HttpGet]
		public IActionResult Index()
		{
			return View();
		}

		[Route("")]
		[HttpPost]
		public IActionResult Index(IFormFile file)
		{
			using (MemoryStream ms = new MemoryStream())
			{
				file.OpenReadStream().CopyTo(ms);
				return this.File(ms.ToArray(), file.ContentType);
			}
		}
	}
}
