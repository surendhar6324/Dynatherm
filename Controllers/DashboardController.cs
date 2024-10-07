using Microsoft.AspNetCore.Mvc;

namespace Dynatherm_Eevee.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Showcase()
        {
            return View();
        }
    }
}
