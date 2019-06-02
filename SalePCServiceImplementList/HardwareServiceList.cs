using System;
using System.Collections.Generic;
using System.Linq;
using SalePC;
using SalePCServiceDAL.BindingModels;
using SalePCServiceDAL.Interfaces;
using SalePCServiceDAL.ViewModels;


namespace SalePCServiceImplementList
{
    public class HardwareServiceList : IHardwareService
    {
        private DataListSingleton source;
        public HardwareServiceList()
        {
            source = DataListSingleton.GetInstance();
        }
        public List<HardwareViewModel> GetList()
        {
            List<HardwareViewModel> result = source.Hardwares.Select(rec => new
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
            Hardware element = source.Hardwares.FirstOrDefault(rec => rec.Id == id);
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
            Hardware element = source.Hardwares.FirstOrDefault(rec => rec.HardwareName
== model.HardwareName);
            if (element != null)
            {
                throw new Exception("Уже есть компонент с таким названием");
            }
            int maxId = source.Hardwares.Count > 0 ? source.Hardwares.Max(rec =>
           rec.Id) : 0;
            source.Hardwares.Add(new Hardware
            {
                Id = maxId + 1,
                HardwareName = model.HardwareName
            });

        }
        public void UpdElement(HardwareBindingModel model)
        {
            Hardware element = source.Hardwares.FirstOrDefault(rec => rec.HardwareName
== model.HardwareName && rec.Id != model.Id);
            if (element != null)
            {
                throw new Exception("Уже есть компонент с таким названием");
            }
            element = source.Hardwares.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.HardwareName = model.HardwareName;
        }
        public void DelElement(int id)
        {
            Hardware element = source.Hardwares.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                source.Hardwares.Remove(element);
            }
            else
            {
                throw new Exception("Элемент не найден");
            }

        }
    }

}
