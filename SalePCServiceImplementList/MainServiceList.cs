using System;
using SalePC;
using SalePCServiceDAL.BindingModels;
using SalePCServiceDAL.Interfaces;
using SalePCServiceDAL.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace SalePCServiceImplementList
{
    public class MainServiceList : IMainService
    {
        private DataListSingleton source;
        public MainServiceList()
        {
            source = DataListSingleton.GetInstance();
        }
        public List<OrderViewModel> GetList()
        {
            List<OrderViewModel> result = source.Orders.Select(rec => new OrderViewModel
            {
                Id = rec.Id,
                ClientId = rec.ClientId,
                PCId = rec.PCId,
                DateCreate = rec.DateCreate.ToLongDateString(),
                DateImplement = rec.DateImplement?.ToLongDateString(),
                Status = rec.Status.ToString(),
                Count = rec.Count,
                Sum = rec.Sum,
                ClientFIO = source.Clients.FirstOrDefault(recC => recC.Id ==
                rec.ClientId)?.ClientFIO,
                PCName = source.PCs.FirstOrDefault(recP => recP.Id ==
               rec.PCId)?.PCName,
            }).ToList();
            return result;

        }
        public void CreateOrder(OrderBindingModel model)
        {
            int maxId = source.Orders.Count > 0 ? source.Orders.Max(rec => rec.Id) : 0;
            source.Orders.Add(new Order
            {
                Id = maxId + 1,
                ClientId = model.ClientId,
                PCId = model.PCId,
                DateCreate = DateTime.Now,
                Count = model.Count,
                Sum = model.Sum,
                Status = OrderStatus.Принят
            });

        }
        public void TakeOrderInWork(OrderBindingModel model)
        {
            Order element = source.Orders.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            if (element.Status != OrderStatus.Принят)
            {
                throw new Exception("Заказ не в статусе \"Принят\"");
            }
            var PCHardwares = source.PCHardwares.Where(rec => rec.PCId == element.PCId);
            foreach (var PCHardware in PCHardwares)
            {
                int countOnStocks = source.StockHardwares
                .Where(rec => rec.HardwareId ==
               PCHardware.HardwareId)
               .Sum(rec => rec.Count);
                if (countOnStocks < PCHardware.Count * element.Count)
                {
                    var HardwareName = source.Hardwares.FirstOrDefault(rec => rec.Id ==
                   PCHardware.HardwareId);
                    throw new Exception("Не достаточно компонента " +
                   HardwareName?.HardwareName + " требуется " + (PCHardware.Count * element.Count) +
                   ", в наличии " + countOnStocks);
                }
            }
            // списываем
            foreach (var PCHardware in PCHardwares)
            {
                int countOnStocks = PCHardware.Count * element.Count;
                var stockHardwares = source.StockHardwares.Where(rec => rec.HardwareId
               == PCHardware.HardwareId);
                foreach (var stockHardware in stockHardwares)
                {
                    // компонентов на одном слкаде может не хватать
                    if (stockHardware.Count >= countOnStocks)
                    {
                        stockHardware.Count -= countOnStocks;
                        break;
                    }
                    else
                    {
                        countOnStocks -= stockHardware.Count;
                        stockHardware.Count = 0;
                    }
                }
            }
            element.DateImplement = DateTime.Now;
            element.Status = OrderStatus.Выполняется;

        }
        public void FinishOrder(OrderBindingModel model)
        {
            Order element = source.Orders.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            if (element.Status != OrderStatus.Выполняется)
            {
                throw new Exception("Заказ не в статусе \"Выполняется\"");
            }
            element.Status = OrderStatus.Готов;
        }
        public void PayOrder(OrderBindingModel model)
        {
            Order element = source.Orders.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            if (element.Status != OrderStatus.Готов)
            {
                throw new Exception("Заказ не в статусе \"Готов\"");
            }
            element.Status = OrderStatus.Оплачен;

        }
        public void PutHardwareOnStock(StockHardwareBindingModel model)
        {
            StockHardware element = source.StockHardwares.FirstOrDefault(rec =>
           rec.StockId == model.StockId && rec.HardwareId == model.HardwareId);
            if (element != null)
            {
                element.Count += model.Count;
            }
            else
            {
                int maxId = source.StockHardwares.Count > 0 ?
               source.StockHardwares.Max(rec => rec.Id) : 0;
                source.StockHardwares.Add(new StockHardware
                {
                    Id = ++maxId,
                    StockId = model.StockId,
                    HardwareId = model.HardwareId,
                    Count = model.Count
                });
            }
        }

    }

}