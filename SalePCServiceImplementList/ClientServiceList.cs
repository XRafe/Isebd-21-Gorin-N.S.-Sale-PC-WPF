using System;
using SalePC;
using SalePCServiceDAL.BindingModels;
using SalePCServiceDAL.Interfaces;
using SalePCServiceDAL.ViewModels;
using System.Collections.Generic;
using System.Linq;


namespace SalePCServiceImplementList
{
    public class ClientServiceList : IClientService
    {
        private DataListSingleton source;
        public ClientServiceList()
        {
            source = DataListSingleton.GetInstance();
        }
        public List<ClientViewModel> GetList()
        {
            List<ClientViewModel> result = source.Clients.Select(rec => new
    ClientViewModel
            {
                Id = rec.Id,
                ClientFIO = rec.ClientFIO
            })
     .ToList();
            return result;
        }
        public ClientViewModel GetElement(int id)
        {
            Client element = source.Clients.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                return new ClientViewModel
                {
                    Id = element.Id,
                    ClientFIO = element.ClientFIO
                };
            }
            throw new Exception("Элемент не найден");
        }
        public void AddElement(ClientBindingModel model)
        {
            Client element = source.Clients.FirstOrDefault(rec => rec.ClientFIO ==
           model.ClientFIO);
            if (element != null)
            {
                throw new Exception("Уже есть клиент с таким ФИО");
            }
            int maxId = source.Clients.Count > 0 ? source.Clients.Max(rec => rec.Id) : 0;
            source.Clients.Add(new Client
            {
                Id = maxId + 1,
                ClientFIO = model.ClientFIO
            });
        }
        public void UpdElement(ClientBindingModel model)
        {
            Client element = source.Clients.FirstOrDefault(rec => rec.ClientFIO ==
           model.ClientFIO && rec.Id != model.Id);
            if (element != null)
            {
                throw new Exception("Уже есть клиент с таким ФИО");
            }
            element = source.Clients.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.ClientFIO = model.ClientFIO;
        }
        public void DelElement(int id)
        {
            Client element = source.Clients.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                source.Clients.Remove(element);
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }
    }
}
