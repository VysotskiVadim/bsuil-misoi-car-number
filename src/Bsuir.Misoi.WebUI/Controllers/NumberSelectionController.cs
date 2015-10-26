using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;

namespace Bsuir.Misoi.WebUI.Controllers
{
    [Route("")]
    public class NumberSelectionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
