using System;
using System.Collections.Generic;
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
            List<HardwareViewModel> result = new List<HardwareViewModel>();
            for (int i = 0; i < source.Hardwares.Count; ++i)
            {
                result.Add(new HardwareViewModel
                {
                    Id = source.Hardwares[i].Id,
                    HardwareName = source.Hardwares[i].HardwareName
                });
            }
            return result;
        }
        public HardwareViewModel GetElement(int id)
        {
            for (int i = 0; i < source.Hardwares.Count; ++i)
            {
                if (source.Hardwares[i].Id == id)
                {
                    return new HardwareViewModel
                    {
                        Id = source.Hardwares[i].Id,
                        HardwareName = source.Hardwares[i].HardwareName
                    };
                }
            }
            throw new Exception("Элемент не найден");
        }
        public void AddElement(HardwareBindingModel model)
        {
            int maxId = 0;
            for (int i = 0; i < source.Hardwares.Count; ++i)
            {
                if (source.Hardwares[i].Id > maxId)
                {
                    maxId = source.Hardwares[i].Id;
                }
                if (source.Hardwares[i].HardwareName == model.HardwareName)
                {
                    throw new Exception("Уже есть компонент с таким названием");
                }
            }
            source.Hardwares.Add(new Hardware
            {
                Id = maxId + 1,
                HardwareName = model.HardwareName
            });
        }
        public void UpdElement(HardwareBindingModel model)
        {
            int index = -1;
            for (int i = 0; i < source.Hardwares.Count; ++i)
            {
                if (source.Hardwares[i].Id == model.Id)
                {
                    index = i;
                }
                if (source.Hardwares[i].HardwareName == model.HardwareName &&
                source.Hardwares[i].Id != model.Id)
                {
                    throw new Exception("Уже есть компонент с таким названием");
                }
            }
            if (index == -1)
            {
                throw new Exception("Элемент не найден");
            }
            source.Hardwares[index].HardwareName = model.HardwareName;
        }
        public void DelElement(int id)
        {
            for (int i = 0; i < source.Hardwares.Count; ++i)
            {
                if (source.Hardwares[i].Id == id)
                {
                    source.Hardwares.RemoveAt(i);
                    return;
                }
            }
            throw new Exception("Элемент не найден");
        }
    }

}
