using Dynatherm_Eevee.Database;
using Dynatherm_Eevee.Models;
using static Dynatherm_Eevee.Database.DB_Clients;

namespace Sahaayak_Board.Models
{
    public class ClientsModel
    {

        public List<Clients_DTO> Get_Clients(DataFilter dataFilter)
        { 
            DB_Clients dB_Clients = new DB_Clients();

            List<Clients_DTO> clients_DTOs = dB_Clients.Get_Clients(dataFilter.SkipCount, dataFilter.TakeCount, dataFilter.SearchText, dataFilter.SortColumn, dataFilter.SortAscending);
             
            return clients_DTOs;
        }
        public int Get_Clients_Count(DataFilter dataFilter)
        {
            DB_Clients dB_Clients = new DB_Clients();

            int count_Clients = dB_Clients.Get_Clients_Count(dataFilter.SkipCount, dataFilter.TakeCount, dataFilter.SearchText, dataFilter.SortColumn, dataFilter.SortAscending);

            return count_Clients;
        }
        public Clients_DTO Get_Clients_ByID(DataFilter dataFilter)
        {
            DB_Clients dB_Clients = new DB_Clients();

            DTO_Assign dTO_Assigns = db_Service_Request.Get_Service_Request_Archive_Count(dTO_Assign.Service_Request_ID);

            DB_Location dB_Location = new DB_Location();
            Location_Values_DTO location_Values_DTO = new Location_Values_DTO();
            if (dTO_Assigns.Postal_code != null)
            {
                location_Values_DTO = dB_Location.Get_Location_Values_ByPincode((int)dTO_Assigns.Postal_code);
            }
            dTO_Assigns.Duration = dTO_Assigns.StartDate.ToString("yyyy-MM-dd") + " " + dTO_Assigns.StartTime + "<br/>" + dTO_Assigns.EndDate.ToString("yyyy-MM-dd");
            dTO_Assigns.Address = dTO_Assigns.Building_Name + " " + dTO_Assigns.Address + "<br/>" + dTO_Assigns.Area + " " + location_Values_DTO.city_name;

            return dTO_Assigns;
        }
        public async Task<bool> UpdateServiceRequest_ByID(DTO_Assign dTO_Assign)
        {
            // Create and initialize necessary objects
            var db_Service_Request = new DB_Service_Request();
            var aPI_EmailTemplate = new API_EmailTemplate();
            var dB_View_Service_Request = new DB_View_Service_Request();

            // Prepare service request DTO for update
            var service_Request_DTO = new Service_Request_DTO
            {
                provider_id = dTO_Assign.Provider_ID,
                service_request_id = dTO_Assign.Service_Request_ID
            };

            // Update the service request and send email notification
            bool bReturn = db_Service_Request.Update_Service_Request_Assign(service_Request_DTO);
            bool bResult = await aPI_EmailTemplate.Get_Async_Service_Status_Email(dTO_Assign.Service_Request_ID);

            // Retrieve the view data for the service request
            var view_Service_Request_DTO = await dB_View_Service_Request.Get_View_Service_Request_ByServiceRequestId(dTO_Assign.Service_Request_ID);

            // If view_Service_Request_DTO is not null, proceed with email details setup
            if (view_Service_Request_DTO != null)
            {
                // Concatenate address parts with null and empty checks
                var fullAddress = string.Join(", ", new[]
                {
                    view_Service_Request_DTO.building_name,
                    view_Service_Request_DTO.address,
                    view_Service_Request_DTO.area,
                    view_Service_Request_DTO.city,
                    view_Service_Request_DTO.state,
                    view_Service_Request_DTO.postal_code?.ToString()}.Where(part => !string.IsNullOrWhiteSpace(part)));

                // Prepare service assignment details for email
                var serviceAssignmentDetails = new ServiceAssignmentDetails
                {
                    CustomerAddress = fullAddress,
                    CustomerName = view_Service_Request_DTO.member_name ?? string.Empty,
                    AssignedDate = DateTime.Now.ToString(),
                    EmailID = view_Service_Request_DTO.provider_emailid ?? string.Empty,
                    ServiceProviderName = view_Service_Request_DTO.provider_name ?? string.Empty,
                    ServiceName = view_Service_Request_DTO.services_name ?? string.Empty
                };

                // Send service assignment email to provider
                await aPI_EmailTemplate.Send_Service_Assignment_Notice_To_Provider(serviceAssignmentDetails);
            }

            return true;
        }
    }
}
