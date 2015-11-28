using Microsoft.AspNet.Mvc;

namespace Bsuir.Misoi.WebUI.Controllers
{
    [Route("")]
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
