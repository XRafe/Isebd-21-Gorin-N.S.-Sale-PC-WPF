using System;
using SalePC;
using SalePCServiceDAL.BindingModels;
using SalePCServiceDAL.Interfaces;
using SalePCServiceDAL.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace SalePCServiceImplementList
{
    public class PCServiceList : IPCService
    {
        private DataListSingleton source;
        public PCServiceList()
        {
            source = DataListSingleton.GetInstance();
        }
        public List<PCViewModel> GetList()
        {
            List<PCViewModel> result = source.PCs
 .Select(rec => new PCViewModel
 {
     Id = rec.Id,
     PCName = rec.PCName,
     Price = rec.Price,
     PCHardwares = source.PCHardwares
 .Where(recPC => recPC.PCId == rec.Id)
.Select(recPC => new PCHardwareViewModel
{
    Id = recPC.Id,
    PCId = recPC.PCId,
    HardwareId = recPC.HardwareId,
    HardwareNames = source.Hardwares.FirstOrDefault(recC =>
    recC.Id == recPC.HardwareId)?.HardwareName,
    Count = recPC.Count
})
.ToList()
 })
 .ToList();
            return result;
        }
        public PCViewModel GetElement(int id)
        {
            PC element = source.PCs.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                return new PCViewModel
                {
                    Id = element.Id,
                    PCName = element.PCName,
                    Price = element.Price,
                    PCHardwares = source.PCHardwares
                .Where(recPC => recPC.PCId == element.Id)
                .Select(recPC => new PCHardwareViewModel
                {
                    Id = recPC.Id,
                    PCId = recPC.PCId,
                    HardwareId = recPC.HardwareId,
                    HardwareNames = source.Hardwares.FirstOrDefault(recC =>
     recC.Id == recPC.HardwareId)?.HardwareName,
                    Count = recPC.Count
                })
               .ToList()
                };
            }
            throw new Exception("Элемент не найден");
        }
        public void AddElement(PCBindingModel model)
        {
            PC element = source.PCs.FirstOrDefault(rec => rec.PCName ==
model.PCName);
            if (element != null)
            {
                throw new Exception("Уже есть изделие с таким названием");
            }
            int maxId = source.PCs.Count > 0 ? source.PCs.Max(rec => rec.Id) :
           0;
            source.PCs.Add(new PC
            {
                Id = maxId + 1,
                PCName = model.PCName,
                Price = model.Price
            });
            // компоненты для изделия
            int maxPCId = source.PCHardwares.Count > 0 ?
           source.PCHardwares.Max(rec => rec.Id) : 0;
            // убираем дубли по компонентам
            var groupHardwares = model.PCHardwares
            .GroupBy(rec => rec.HardwareId)
           .Select(rec => new
           {
               HardwareId = rec.Key,
               Count = rec.Sum(r => r.Count)
           });
            // добавляем компоненты
            foreach (var groupHardware in groupHardwares)
            {
                source.PCHardwares.Add(new PCHardwares
                {
                    Id = ++maxPCId,
                    PCId = maxId + 1,
                HardwareId = groupHardware.HardwareId,
                    Count = groupHardware.Count
                });
            }

        }
        public void UpdElement(PCBindingModel model)
        {
            PC element = source.PCs.FirstOrDefault(rec => rec.PCName ==
  model.PCName && rec.Id != model.Id);
            if (element != null)
            {
                throw new Exception("Уже есть изделие с таким названием");
            }
            element = source.PCs.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.PCName = model.PCName;
            element.Price = model.Price;
            int maxPCId = source.PCHardwares.Count > 0 ?
           source.PCHardwares.Max(rec => rec.Id) : 0;
            // обновляем существуюущие компоненты
            var compIds = model.PCHardwares.Select(rec =>
           rec.HardwareId).Distinct();
            var updateHardwares = source.PCHardwares.Where(rec => rec.PCId ==
           model.Id && compIds.Contains(rec.HardwareId));
            foreach (var updateHardware in updateHardwares)
            {
                updateHardware.Count = model.PCHardwares.FirstOrDefault(rec =>
               rec.Id == updateHardware.Id).Count;
            }
            source.PCHardwares.RemoveAll(rec => rec.PCId == model.Id &&
           !compIds.Contains(rec.HardwareId));
            // новые записи
            var groupHardwares = model.PCHardwares
            .Where(rec => rec.Id == 0)
           .GroupBy(rec => rec.HardwareId)
           .Select(rec => new
           {
               HardwareId = rec.Key,
               Count = rec.Sum(r => r.Count)
           });
            foreach (var groupHardware in groupHardwares)
            {
                PCHardwares elementPC = source.PCHardwares.FirstOrDefault(rec
               => rec.PCId == model.Id && rec.HardwareId == groupHardware.HardwareId);
                if (elementPC != null)
                {
                    elementPC.Count += groupHardware.Count;
                }
                else
                {
                    source.PCHardwares.Add(new PCHardwares
                    {
                        Id = ++maxPCId,
                        PCId = model.Id,
                        HardwareId = groupHardware.HardwareId,
                        Count = groupHardware.Count
                    });
                }
            }
        }
        public void DelElement(int id)
        {
            PC element = source.PCs.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                // удаяем записи по компонентам при удалении изделия
                source.PCHardwares.RemoveAll(rec => rec.PCId == id);
                source.PCs.Remove(element);
            }
            else
            {
                throw new Exception("Элемент не найден");
            }

        }
    }

}