using System;
using SalePC;
using SalePCServiceDAL.BindingModels;
using SalePCServiceDAL.Interfaces;
using SalePCServiceDAL.ViewModels;
using System.Collections.Generic;

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
            List<PCViewModel> result = new List<PCViewModel>();
            for (int i = 0; i < source.PCs.Count; ++i)
            {
                // требуется дополнительно получить список компонентов для изделия и их количество
                List<PCHardwareViewModel> PCHardwares = new List<PCHardwareViewModel>();
                for (int j = 0; j < source.PCHardwares.Count; ++j)
                {
                    if (source.PCHardwares[j].PCId == source.PCs[i].Id)
                    {
                        string HardwareName = string.Empty;
                        for (int k = 0; k < source.Hardwares.Count; ++k)
                        {
                            if (source.PCHardwares[j].HardwareId ==
                           source.Hardwares[k].Id)
                            {
                                HardwareName = source.Hardwares[k].HardwareName;
                                break;
                            }
                        }
                        PCHardwares.Add(new PCHardwareViewModel
                        {
                            Id = source.PCHardwares[j].Id,
                            PCId = source.PCHardwares[j].PCId,
                            HardwareId = source.PCHardwares[j].HardwareId,
                            HardwareNames = HardwareName,
                            Count = source.PCHardwares[j].Count
                        });
                    }
                }
                result.Add(new PCViewModel
                {
                    Id = source.PCs[i].Id,
                    PCName = source.PCs[i].PCName,
                    Price = source.PCs[i].Price,
                    PCHardwares = PCHardwares
                });
            }
            return result;
        }
        public PCViewModel GetElement(int id)
        {
            for (int i = 0; i < source.PCs.Count; ++i)
            {
                // требуется дополнительно получить список компонентов для изделия и их количество
                List<PCHardwareViewModel> PCHardwares = new List<PCHardwareViewModel>();
                for (int j = 0; j < source.PCHardwares.Count; ++j)
                {
                    if (source.PCHardwares[j].PCId == source.PCs[i].Id)
                    {
                        string HardwareName = string.Empty;
                        for (int k = 0; k < source.Hardwares.Count; ++k)
                        {
                            if (source.PCHardwares[j].HardwareId == source.Hardwares[k].Id)
                            {
                                HardwareName = source.Hardwares[k].HardwareName;
                                break;
                            }
                        }
                        PCHardwares.Add(new PCHardwareViewModel
                        {
                            Id = source.PCHardwares[j].Id,
                            PCId = source.PCHardwares[j].PCId,
                            HardwareId = source.PCHardwares[j].HardwareId,
                            HardwareNames = HardwareName,
                            Count = source.PCHardwares[j].Count
                        });
                    }
                }
                if (source.PCs[i].Id == id)
                {
                    return new PCViewModel
                    {
                        Id = source.PCs[i].Id,
                        PCName = source.PCs[i].PCName,
                        Price = source.PCs[i].Price,
                        PCHardwares = PCHardwares
                    };
                }
            }
            throw new Exception("Элемент не найден");
        }
        public void AddElement(PCBindingModel model)
        {
            int maxId = 0;
            for (int i = 0; i < source.PCs.Count; ++i)
            {
                if (source.PCs[i].Id > maxId)
                {
                    maxId = source.PCs[i].Id;
                }
                if (source.PCs[i].PCName == model.PCName)
                {
                    throw new Exception("Уже есть изделие с таким названием");
                }
            }
            source.PCs.Add(new PC
            {
                Id = maxId + 1,
                PCName = model.PCName,
                Price = model.Price
            });
            // компоненты для изделия
            int maxPCId = 0;
            for (int i = 0; i < source.PCHardwares.Count; ++i)
            {
                if (source.PCHardwares[i].Id > maxPCId)
                {
                    maxPCId = source.PCHardwares[i].Id;
                }
            }
            // убираем дубли по компонентам
            for (int i = 0; i < model.PCHardwares.Count; ++i)
            {
                for (int j = 1; j < model.PCHardwares.Count; ++j)
                {
                    if (model.PCHardwares[i].HardwareId ==
                    model.PCHardwares[j].HardwareId)
                    {
                        model.PCHardwares[i].Count +=
                        model.PCHardwares[j].Count;
                        model.PCHardwares.RemoveAt(j--);
                    }
                }
            }
            // добавляем компоненты
            for (int i = 0; i < model.PCHardwares.Count; ++i)
            {
                source.PCHardwares.Add(new PCHardwares
                {
                    Id = ++maxPCId,
                    PCId = maxId + 1,
                    HardwareId = model.PCHardwares[i].HardwareId,
                    Count = model.PCHardwares[i].Count
                });
            }
        }
        public void UpdElement(PCBindingModel model)
        {
            int index = -1;
            for (int i = 0; i < source.PCs.Count; ++i)
            {
                if (source.PCs[i].Id == model.Id)
                {
                    index = i;
                }
                if (source.PCs[i].PCName == model.PCName &&
                source.PCs[i].Id != model.Id)
                {
                    throw new Exception("Уже есть изделие с таким названием");
                }
            }
            if (index == -1)
            {
                throw new Exception("Элемент не найден");
            }
            source.PCs[index].PCName = model.PCName;
            source.PCs[index].Price = model.Price;
            int maxPCId = 0;
            for (int i = 0; i < source.PCHardwares.Count; ++i)
            {
                if (source.PCHardwares[i].Id > maxPCId)
                {
                    maxPCId = source.PCHardwares[i].Id;
                }
            }
            // обновляем существуюущие компоненты
            for (int i = 0; i < source.PCHardwares.Count; ++i)
            {
                if (source.PCHardwares[i].PCId == model.Id)
                {
                    bool flag = true;
                    for (int j = 0; j < model.PCHardwares.Count; ++j)
                    {
                        // если встретили, то изменяем количество
                        if (source.PCHardwares[i].Id ==
                       model.PCHardwares[j].Id)
                        {
                            source.PCHardwares[i].Count =
                           model.PCHardwares[j].Count;
                            flag = false;
                            break;
                        }
                    }
                    // если не встретили, то удаляем
                    if (flag)
                    {
                        source.PCHardwares.RemoveAt(i--);
                    }
                }
            }
            // новые записи
            for (int i = 0; i < model.PCHardwares.Count; ++i)
            {
                if (model.PCHardwares[i].Id == 0)
                {
                    // ищем дубли
                    for (int j = 0; j < source.PCHardwares.Count; ++j)
                    {
                        if (source.PCHardwares[j].PCId == model.Id &&
                        source.PCHardwares[j].HardwareId ==
                       model.PCHardwares[i].HardwareId)
                        {
                            source.PCHardwares[j].Count +=
                           model.PCHardwares[i].Count;
                            model.PCHardwares[i].Id =
                           source.PCHardwares[j].Id;
                            break;
                        }
                    }
                    // если не нашли дубли, то новая запись
                    if (model.PCHardwares[i].Id == 0)
                    {
                        source.PCHardwares.Add(new PCHardwares
                        {
                            Id = ++maxPCId,
                            PCId = model.Id,
                            HardwareId = model.PCHardwares[i].HardwareId,
                            Count = model.PCHardwares[i].Count
                        });
                    }
                }
            }
        }
        public void DelElement(int id)
        {
            // удаяем записи по компонентам при удалении изделия
            for (int i = 0; i < source.PCHardwares.Count; ++i)
            {
                if (source.PCHardwares[i].PCId == id)
                {
                    source.PCHardwares.RemoveAt(i--);
                }
            }
            for (int i = 0; i < source.PCs.Count; ++i)
            {
                if (source.PCs[i].Id == id)
                {
                    source.PCs.RemoveAt(i);
                    return;
                }
            }
            throw new Exception("Элемент не найден");
        }
    }

}
