using Microsoft.AspNet.Mvc;

namespace Bsuir.Misoi.WebUI.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public string Index()
        {
            return "Hello from home controller.";
        }
    }
}
