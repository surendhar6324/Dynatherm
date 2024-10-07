using Dynatherm_Eevee.Models;
using Microsoft.AspNetCore.Mvc;
using MySqlX.XDevAPI;
using Newtonsoft.Json;
using Sahaayak_Board.Models;

namespace Dynatherm_Eevee.Controllers
{
    public class SettingsController : Controller
    {
        public IActionResult Clients()
        {
            return View();
        }

        #region Clients
        public JsonResult Clients_Load(DataFilter datafilter)
        {
            Clients_DTO clients = new Clients_DTO();

            datafilter.SkipCount = (datafilter.SkipCount * datafilter.TakeCount) - datafilter.TakeCount;

            var listReturn = clients.Get_Clients(datafilter);

            var jsonResult = Json(new
            {
                Listdatafilter = JsonConvert.SerializeObject(listReturn),
            });

            return jsonResult;
        }
        public JsonResult UnassignCount_Load(DataFilter datafilter)
        {
            Clients_DTO clients = new Clients_DTO();

            datafilter.SkipCount = (datafilter.SkipCount * datafilter.TakeCount) - datafilter.TakeCount;

            int TotalCount = clients.Get_Clients_Count(datafilter);

            var jsonResult = Json(new
            {
                skipCount = JsonConvert.SerializeObject(datafilter.SkipCount),
                totalCount = JsonConvert.SerializeObject(TotalCount),
            });

            return jsonResult;
        }
        public JsonResult Ëdit_Unassign_Load(DataFilter datafilter)
        {
            Clients_DTO clients = new Clients_DTO(); 

            Clients_DTO clients_DTO = clients.GetServiceRequest_ByID(datafilter); 

            var jsonResult = Json(new
            {
                clients_DTO = JsonConvert.SerializeObject(clients_DTO), 
            });

            return jsonResult;
        }
        public JsonResult Update_Unassign_Clicked(DataFilter datafilter)
        {
            Clients_DTO clients = new Clients_DTO();

            clients.UpdateServiceRequest_ByID(datafilter);

            return new JsonResult("");
        }
        #endregion Unassigned
    }
}
