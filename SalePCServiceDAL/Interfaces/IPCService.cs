using System.Collections.Generic;
using SalePCServiceDAL.BindingModels;
using SalePCServiceDAL.ViewModels;

namespace SalePCServiceDAL.Interfaces
{
    public interface IPCService
    {
        List<PCViewModel> GetList();
        PCViewModel GetElement(int id);
        void AddElement(PCBindingModel model);
        void UpdElement(PCBindingModel model);
        void DelElement(int id);
    }
}
