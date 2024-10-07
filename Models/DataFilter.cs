namespace Dynatherm_Eevee.Models
{
    public class DataFilter
    { 
        public string PrimaryTag { get; set; } 
        public string PassingKey { get; set; } //This field is used to pass primary key from view to controller  
        public string txtDate { get; set; }  
        public int SkipCount { get; set; }
        public int TakeCount { get; set; }
        public string SearchText { get; set; }
        public string SortColumn { get; set; }
        public bool SortAscending { get; set; }
        public bool IsExport { get; set; }
        public long? client_id { get; set; }
    }
}
