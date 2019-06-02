using SalePCServiceDAL.BindingModels;
using SalePCServiceDAL.ViewModels;
using System.Collections.Generic;

namespace SalePCServiceDAL.Interfaces
{
    public interface IMainService
    {
        List<OrderViewModel> GetList();
        void CreateOrder(OrderBindingModel model);
        void TakeOrderInWork(OrderBindingModel model);
        void FinishOrder(OrderBindingModel model);
        void PayOrder(OrderBindingModel model);
        void PutHardwareOnStock(StockHardwareBindingModel model);
    }

}
