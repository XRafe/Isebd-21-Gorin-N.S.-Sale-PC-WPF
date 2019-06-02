using System;
using System.Collections.Generic;
using System.Linq;
using SalePC;
using SalePCServiceDAL.BindingModels;
using SalePCServiceDAL.Interfaces;
using SalePCServiceDAL.ViewModels;

namespace SalePCServiceImplementDataBase.Implementations
{
    public class PCServiceDB : IPCService
    {
        private AbstractPCDbContext context;
        public PCServiceDB(AbstractPCDbContext context)
        {
            this.context = context;
        }
        public List<PCViewModel> GetList()
        {
            List<PCViewModel> result = context.PCs.Select(rec => new
           PCViewModel
            {
                Id = rec.Id,
                PCName = rec.PCName,
                Price = rec.Price,
                PCHardwares = context.PCHardwares
            .Where(recPC => recPC.PCId == rec.Id)
           .Select(recPC => new PCHardwareViewModel
           {
               Id = recPC.Id,
               PCId = recPC.PCId,
               HardwareId = recPC.HardwareId,
               HardwareNames = recPC.Hardware.HardwareName,
               Count = recPC.Count
           })
           .ToList()
            })
            .ToList();
            return result;
        }
        public PCViewModel GetElement(int id)
        {
            PC element = context.PCs.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                return new PCViewModel
                {
                    Id = element.Id,
                    PCName = element.PCName,
                    Price = element.Price,
                    PCHardwares = context.PCHardwares
 .Where(recPC => recPC.PCId == element.Id)
 .Select(recPC => new PCHardwareViewModel
 {
     Id = recPC.Id,
     PCId = recPC.PCId,
     HardwareId = recPC.HardwareId,
     HardwareNames = recPC.Hardware.HardwareName,
     Count = recPC.Count
 })
 .ToList()
                };
            }
            throw new Exception("Элемент не найден");
        }
        public void AddElement(PCBindingModel model)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    PC element = context.PCs.FirstOrDefault(rec =>
                   rec.PCName == model.PCName);
                    if (element != null)
                    {
                        throw new Exception("Уже есть изделие с таким названием");
                    }
                    element = new PC
                    {
                        PCName = model.PCName,
                        Price = model.Price
                    };
                    context.PCs.Add(element);
                    context.SaveChanges();
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
                        context.PCHardwares.Add(new PCHardwares
                        {
                            PCId = element.Id,
                            HardwareId = groupHardware.HardwareId,
                            Count = groupHardware.Count
                        });
                        context.SaveChanges();
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
        public void UpdElement(PCBindingModel model)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    PC element = context.PCs.FirstOrDefault(rec =>
                   rec.PCName == model.PCName && rec.Id != model.Id);
                    if (element != null)
                    {
                        throw new Exception("Уже есть изделие с таким названием");
                    }
                    element = context.PCs.FirstOrDefault(rec => rec.Id == model.Id);
                    if (element == null)
                    {
                        throw new Exception("Элемент не найден");
                    }
                    element.PCName = model.PCName;
                    element.Price = model.Price;
                    context.SaveChanges();
                    // обновляем существуюущие компоненты
                    var compIds = model.PCHardwares.Select(rec =>
                   rec.HardwareId).Distinct();
                    var updateHardwares = context.PCHardwares.Where(rec =>
                   rec.PCId == model.Id && compIds.Contains(rec.HardwareId));
                    foreach (var updateHardware in updateHardwares)
                    {
                        updateHardware.Count =
                       model.PCHardwares.FirstOrDefault(rec => rec.Id == updateHardware.Id).Count;
                    }
                    context.SaveChanges();
                    context.PCHardwares.RemoveRange(context.PCHardwares.Where(rec =>
                    rec.PCId == model.Id && !compIds.Contains(rec.HardwareId)));
                    context.SaveChanges();
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
                        PCHardwares elementPC =
                       context.PCHardwares.FirstOrDefault(rec => rec.PCId == model.Id &&
                       rec.HardwareId == groupHardware.HardwareId);
                        if (elementPC != null)
                        {
                            elementPC.Count += groupHardware.Count;
                            context.SaveChanges();
                        }
                        else
                        {
                            context.PCHardwares.Add(new PCHardwares
                            {
                                PCId = model.Id,
                                HardwareId = groupHardware.HardwareId,
                                Count = groupHardware.Count
                            });
                            context.SaveChanges();
                        }
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
        public void DelElement(int id)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    PC element = context.PCs.FirstOrDefault(rec => rec.Id ==
                   id);
                    if (element != null)
                    {
                        // удаяем записи по компонентам при удалении изделия
                        context.PCHardwares.RemoveRange(context.PCHardwares.Where(rec =>
                        rec.PCId == id));
                        context.PCs.Remove(element);
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
