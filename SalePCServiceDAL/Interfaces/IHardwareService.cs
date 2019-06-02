using System.Collections.Generic;
using SalePCServiceDAL.BindingModels;
using SalePCServiceDAL.ViewModels;

namespace SalePCServiceDAL.Interfaces
{
    public interface IHardwareService
    {
        List<HardwareViewModel> GetList();
        HardwareViewModel GetElement(int id);
        void AddElement(HardwareBindingModel model);
        void UpdElement(HardwareBindingModel model);
        void DelElement(int id);

    }
}
