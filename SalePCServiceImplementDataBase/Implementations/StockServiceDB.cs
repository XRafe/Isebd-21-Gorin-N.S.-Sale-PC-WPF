using System;
using System.Collections.Generic;
using System.Linq;
using SalePC;
using SalePCServiceDAL.BindingModels;
using SalePCServiceDAL.Interfaces;
using SalePCServiceDAL.ViewModels;

namespace SalePCServiceImplementDataBase.Implementations
{
    public class StockServiceDB : IStockService
    {
        private AbstractPCDbContext context;

        public StockServiceDB(AbstractPCDbContext context)
        {
            this.context = context;
        }
        public List<StockViewModel> GetList()
        {
            List<StockViewModel> result = context.Stocks
            .Select(rec => new StockViewModel
            {
                Id = rec.Id,
                StockName = rec.StockName,
                StockHardware = context.StockHardwares
            .Where(recPC => recPC.StockId == rec.Id)
            .Select(recPC => new StockHardwareViewModel
            {
                Id = recPC.Id,
                StockId = recPC.StockId,
                HardwareId = recPC.HardwareId,
                HardwareName = recPC.Hardware.HardwareName,
                Count = recPC.Count
            })
            .ToList()
            })
            .ToList();
            return result;
        }
        public StockViewModel GetElement(int id)
        {
            Stock element = context.Stocks.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                return new StockViewModel
                {
                    Id = element.Id,
                    StockName = element.StockName,
                    StockHardware = context.StockHardwares
                .Where(recPC => recPC.StockId == element.Id)
                .Select(recPC => new StockHardwareViewModel
                {
                    Id = recPC.Id,
                    StockId = recPC.StockId,
                    HardwareId = recPC.HardwareId,
                    HardwareName = recPC.Hardware.HardwareName,
                    Count = recPC.Count
                })
                .ToList()
                };
            }
            throw new Exception("Элемент не найден");
        }
        public void AddElement(StockBindingModel model)
        {
            Stock element = context.Stocks.FirstOrDefault(rec => rec.StockName == model.StockName);
            if (element != null)
            {
                throw new Exception("Уже есть склад с таким названием");
            }
            //int maxId = context.Stocks.Count > 0 ? context.Stocks.Max(rec => rec.Id) : 0; 
            context.Stocks.Add(new Stock
            {

                StockName = model.StockName
            });
            context.SaveChanges();
        }
        public void UpdElement(StockBindingModel model)
        {
            Stock element = context.Stocks.FirstOrDefault(rec =>
            rec.StockName == model.StockName && rec.Id != model.Id);
            if (element != null)
            {
                throw new Exception("Уже есть склад с таким названием");
            }
            element = context.Stocks.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.StockName = model.StockName;
            context.SaveChanges();
        }
        public void DelElement(int id)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    Stock element = context.Stocks.FirstOrDefault(rec => rec.Id ==
                    id);
                    if (element != null)
                    {
                        // удаяем записи по компонентам при удалении изделия 
                        context.StockHardwares.RemoveRange(context.StockHardwares.Where(rec =>
                        rec.StockId == id));
                        context.Stocks.Remove(element);
                        context.SaveChanges();
                    }
                    else
                    {
                        throw new Exception("Элемент не найден");
                    }
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}
