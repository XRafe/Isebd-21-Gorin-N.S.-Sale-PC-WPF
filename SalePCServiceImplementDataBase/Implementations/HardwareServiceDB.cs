using System;
using System.Collections.Generic;
using System.Linq;
using SalePC;
using SalePCServiceDAL.BindingModels;
using SalePCServiceDAL.Interfaces;
using SalePCServiceDAL.ViewModels;

namespace SalePCServiceImplementDataBase.Implementations
{
    public class HardwareServiceDB : IHardwareService
    {
        private AbstractPCDbContext context;
        public HardwareServiceDB(AbstractPCDbContext context)
        {
            this.context = context;
        }
        public List<HardwareViewModel> GetList()
        {
            List<HardwareViewModel> result = context.Hardwares.Select(rec => new
           HardwareViewModel
            {
                Id = rec.Id,
                HardwareName = rec.HardwareName
            })
            .ToList();
            return result;
        }
        public HardwareViewModel GetElement(int id)
        {
            Hardware element = context.Hardwares.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                return new HardwareViewModel
                {
                    Id = element.Id,
                    HardwareName = element.HardwareName
                };
            }
            throw new Exception("Элемент не найден");
        }
        public void AddElement(HardwareBindingModel model)
        {
            Hardware element = context.Hardwares.FirstOrDefault(rec => rec.HardwareName ==
           model.HardwareName);
            if (element != null)
            {
                throw new Exception("Уже есть клиент с таким ФИО");
            }
            context.Hardwares.Add(new Hardware
            {
                HardwareName = model.HardwareName
            });
            context.SaveChanges();
        }
        public void UpdElement(HardwareBindingModel model)
        {
            Hardware element = context.Hardwares.FirstOrDefault(rec => rec.HardwareName ==
           model.HardwareName && rec.Id != model.Id);
            if (element != null)
            {
                throw new Exception("Уже есть клиент с таким ФИО");
            }
            element = context.Hardwares.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.HardwareName = model.HardwareName;
            context.SaveChanges();
        }
        public void DelElement(int id)
        {
            Hardware element = context.Hardwares.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                context.Hardwares.Remove(element);
                context.SaveChanges();
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }
    }
}
