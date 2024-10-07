using Dynatherm_Eevee.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc; 
using System.Diagnostics;
using static Dynatherm_Eevee.Database.DB_Employee;

namespace Dynatherm_Eevee.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public string salt = "9Ef3L24USmkGpVIEAUsQCA==";

        private const string Employee_Name = "_Employee_Name";
        private const string Employee_ID = "_Employee_ID";
        private const string Employee_GUID = "_Employee_GUID";
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {

            return View();
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            Utils_Encryption encryption = new Utils_Encryption();
            LogonModel home_Logon = new LogonModel(); 

            string hashPassword = encryption.HashPassword(password, salt);

            Employee_DTO employee_DTO = home_Logon.GetLogon(email, hashPassword);

            if (employee_DTO == null)
            {
                ViewBag.ErrorMessage = Utils_Error_Messages.InvalidLogin;
            }
            else
            {
                HttpContext.Session.SetString(Employee_Name, employee_DTO.full_name);
                HttpContext.Session.SetInt32(Employee_ID, employee_DTO.employee_id);
                HttpContext.Session.SetString(Employee_GUID, employee_DTO.employee_guid);
                 
                return RedirectToAction("Showcase", "Dashboard");
            }

            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
