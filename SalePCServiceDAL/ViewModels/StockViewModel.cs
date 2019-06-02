using System.Collections.Generic;
using System.ComponentModel;

namespace SalePCServiceDAL.ViewModels
{
    public class StockViewModel
    {
        public int Id { get; set; }
        [DisplayName("Название склада")]
        public string StockName { get; set; }
        public List<StockHardwareViewModel> StockHardware { get; set; }
    }
}
