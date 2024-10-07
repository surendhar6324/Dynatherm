using Microsoft.AspNetCore.Mvc;

namespace Dynatherm_Eevee.Controllers
{
    public class QuotationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
