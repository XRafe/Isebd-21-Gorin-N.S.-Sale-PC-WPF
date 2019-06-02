using System.Collections.Generic;
using SalePCServiceDAL.BindingModels;
using SalePCServiceDAL.ViewModels;

namespace SalePCServiceDAL.Interfaces
{
    public interface IStockService
    {
        List<StockViewModel> GetList();
        StockViewModel GetElement(int id);
        void AddElement(StockBindingModel model);
        void UpdElement(StockBindingModel model);
        void DelElement(int id);
    }

}
