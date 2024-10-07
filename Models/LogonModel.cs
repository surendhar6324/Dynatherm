using Dynatherm_Eevee.Database;
using static Dynatherm_Eevee.Database.DB_Employee;

namespace Dynatherm_Eevee.Models
{
    public class LogonModel
    { 
        public Employee_DTO GetLogon(string email, string hashPassword)
        {
            DB_Employee dB_Employee = new DB_Employee();

            Employee_DTO employee_DTO = dB_Employee.Get_Employee_Logon(email, hashPassword);

            return employee_DTO;
        }
    }
}
