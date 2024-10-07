using LinqKit;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Dynatherm_Eevee.Database
{
    public class DB_Clients : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL(@"server=dynathermproddb.ctka80ck21tw.ap-south-1.rds.amazonaws.com;uid=admin;pwd=2q1zdMQOcc;database=dynathermdb");
        }
        public List<Clients_DTO> Get_Clients(int skipCount, int takeCount, string searchValue, string SortColumn, bool SortAscending)
        {
            var PredicateTextFilter = PredicateBuilder.True<Clients_DTO>();

            if (searchValue != null && searchValue != "")
            {
                PredicateTextFilter = PredicateBuilder.False<Clients_DTO>();

                PredicateTextFilter = PredicateTextFilter.Or
                    (
                        el => el.client_name.ToUpper().Contains(searchValue.ToUpper()) ||
                        el.purchaser.ToUpper().Contains(searchValue.ToUpper()) ||
                        el.location.ToUpper().Contains(searchValue.ToUpper())
                    );
            }

            using (var context = new DB_Clients())
            {
                var data_clients = context.clients.AsExpandable().Where(PredicateTextFilter).AsQueryable();

                var orderedQuery = data_clients.OrderByDescending(r => r.created_date);

                if (null != SortColumn && SortColumn.Length > 0)
                {
                    if (SortAscending)
                    {
                        switch (SortColumn.ToLower())
                        {
                            case "clientname":
                                orderedQuery = data_clients.OrderBy(r => r.client_name);
                                break;
                            case "purchaser":
                                orderedQuery = data_clients.OrderBy(r => r.purchaser);
                                break;
                            case "state":
                                orderedQuery = data_clients.OrderBy(r => r.state);
                                break;
                            case "location":
                                orderedQuery = data_clients.OrderBy(r => r.location);
                                break;
                            case "createddate":
                                orderedQuery = data_clients.OrderBy(r => r.created_date);
                                break;
                        }
                    }
                    else
                    {
                        switch (SortColumn.ToLower())
                        {
                            case "clientname":
                                orderedQuery = data_clients.OrderByDescending(r => r.client_name);
                                break;
                            case "purchaser":
                                orderedQuery = data_clients.OrderByDescending(r => r.purchaser);
                                break;
                            case "state":
                                orderedQuery = data_clients.OrderByDescending(r => r.state);
                                break;
                            case "location":
                                orderedQuery = data_clients.OrderByDescending(r => r.location);
                                break;
                            case "createddate":
                                orderedQuery = data_clients.OrderByDescending(r => r.created_date);
                                break;
                        }
                    }
                }
                else
                {
                    if (SortAscending)
                    {
                        orderedQuery = data_clients.OrderBy(r => r.created_date);
                    }
                }
                // Establish the page size and drastically reduce the overall query time
                var pageQuery = orderedQuery.Skip(skipCount).Take(takeCount);

                return pageQuery.ToList();
            }
        }
        public int Get_Clients_Count(int skipCount, int takeCount, string searchValue, string SortColumn, bool SortAscending)
        {
            var PredicateTextFilter = PredicateBuilder.True<Clients_DTO>();

            if (searchValue != null && searchValue != "")
            {
                PredicateTextFilter = PredicateBuilder.False<Clients_DTO>();

                PredicateTextFilter = PredicateTextFilter.Or
                    (
                        el => el.client_name.ToUpper().Contains(searchValue.ToUpper()) ||
                        el.purchaser.ToUpper().Contains(searchValue.ToUpper()) ||
                        el.location.ToUpper().Contains(searchValue.ToUpper())
                    );
            }

            using (var context = new DB_Clients())
            {
                var data_clients = context.clients.AsExpandable().Where(PredicateTextFilter).AsQueryable();

                return data_clients.Count();
            }
        }
        public Clients_DTO? Get_Clients_ByID(int client_id)
        {
            using (var context = new DB_Clients())
            {
                return context.clients.Where(r=> r.client_id == client_id).FirstOrDefault();
            }
        } 
        public void Add_Clients(Clients_DTO clients_DTO)
        {
            using (var context = new DB_Clients())
            {
                context.clients.Add(clients_DTO);
                context.SaveChanges();
            }
        }
        public Task<bool> Update_Clients(Clients_DTO clients_DTO)
        {
            using (var context = new DB_Clients())
            {
                var updateUser = context.clients.Where(x => x.client_id == clients_DTO.client_id).FirstOrDefault();

                if (updateUser != null)
                {
                    updateUser.client_name = clients_DTO.client_name;
                    updateUser.purchaser = clients_DTO.purchaser;
                    updateUser.state = clients_DTO.state;
                    updateUser.location = clients_DTO.location;
                    context.SaveChanges();
                }
            }
            return Task.FromResult(true);
        }

        public DbSet<Clients_DTO> clients { get; set; }
        public class Clients_DTO
        {
            [Key]
            public int client_id { get; set; }
            public string? client_name { get; set; }
            public string? purchaser { get; set; }
            public string? state { get; set; }
            public string? location { get; set; }
            public DateTime created_date { get; set; }
            public int created_by { get; set; }
        }
    }
}
