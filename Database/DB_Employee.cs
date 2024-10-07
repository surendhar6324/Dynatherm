using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Dynatherm_Eevee.Database
{
    public class DB_Employee : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL(@"server=dynathermproddb.ctka80ck21tw.ap-south-1.rds.amazonaws.com;uid=admin;pwd=2q1zdMQOcc;database=dynathermdb");
        } 
        public List<Employee_DTO> Get_Employee()
        {
            using (var context = new DB_Employee())
            {
                return context.employee.ToList();
            }
        }
        public Employee_DTO Get_Employee_ByID(int employee_id)
        {
            using (var context = new DB_Employee())
            {
                return context.employee.Where(r=> r.employee_id == employee_id).FirstOrDefault();
            }
        }
        public Employee_DTO Get_Employee_Logon(string email_id, string password)
        {
            using (var context = new DB_Employee())
            {
                return context.employee.Where(r => r.email_id == email_id).Where(r => r.password == password).FirstOrDefault();
            }
        }

        public DbSet<Employee_DTO> employee { get; set; }
        public class Employee_DTO
        {
            [Key]
            public int employee_id { get; set; }
            public string? employee_guid { get; set; }
            public string? full_name { get; set; }
            public string? email_id { get; set; }
            public string? password { get; set; }
            public DateTime? last_active { get; set; }
            public bool? is_active { get; set; }
            public int? role { get; set; }
            public string? image { get; set; }
        }
    }
}
