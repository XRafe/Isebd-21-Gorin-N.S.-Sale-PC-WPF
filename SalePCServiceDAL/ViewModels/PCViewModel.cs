using System.Collections.Generic;

namespace SalePCServiceDAL.ViewModels
{
    public class PCViewModel
    {
        public int Id { get; set; }
        public string PCName { get; set; }
        public decimal Price { get; set; }
        public List<PCHardwareViewModel> PCHardwares { get; set; }
    }
}
