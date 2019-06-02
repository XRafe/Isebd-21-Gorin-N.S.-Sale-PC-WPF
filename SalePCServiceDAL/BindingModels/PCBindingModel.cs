using System.Collections.Generic;

namespace SalePCServiceDAL.BindingModels
{
    public class PCBindingModel
    {
        public int Id { get; set; }
        public string PCName { get; set; }
        public decimal Price { get; set; }
        public List<PCHardwareBindingModel> PCHardwares { get; set; }
    }
}
