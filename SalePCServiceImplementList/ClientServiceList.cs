using System;
using SalePC;
using SalePCServiceDAL.BindingModels;
using SalePCServiceDAL.Interfaces;
using SalePCServiceDAL.ViewModels;
using System.Collections.Generic;

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
            List<ClientViewModel> result = new List<ClientViewModel>();
            for (int i = 0; i < source.Clients.Count; ++i)
            {
                result.Add(new ClientViewModel
                {
                    Id = source.Clients[i].Id,
                    ClientFIO = source.Clients[i].ClientFIO
                });
            }

            return result;
        }
        public ClientViewModel GetElement(int id)
        {
            for (int i = 0; i < source.Clients.Count; ++i)
            {
                if (source.Clients[i].Id == id)
                {
                    return new ClientViewModel
                    {
                        Id = source.Clients[i].Id,
                        ClientFIO = source.Clients[i].ClientFIO
                    };
                }
            }
            throw new Exception("Элемент не найден");
        }
        public void AddElement(ClientBindingModel model)
        {
            int maxId = 0;
            for (int i = 0; i < source.Clients.Count; ++i)
            {
                if (source.Clients[i].Id > maxId)
                {
                    maxId = source.Clients[i].Id;
                }
                if (source.Clients[i].ClientFIO == model.ClientFIO)
                {
                    throw new Exception("Уже есть клиент с таким ФИО");
                }
            }
            source.Clients.Add(new Client
            {
                Id = maxId + 1,
                ClientFIO = model.ClientFIO
            });
        }
        public void UpdElement(ClientBindingModel model)
        {
            int index = -1;
            for (int i = 0; i < source.Clients.Count; ++i)
            {
                if (source.Clients[i].Id == model.Id)
                {
                    index = i;
                }
                if (source.Clients[i].ClientFIO == model.ClientFIO &&
                source.Clients[i].Id != model.Id)
                {
                    throw new Exception("Уже есть клиент с таким ФИО");
                }
            }
            if (index == -1)
            {
                throw new Exception("Элемент не найден");
            }
            source.Clients[index].ClientFIO = model.ClientFIO;
        }
        public void DelElement(int id)
        {
            for (int i = 0; i < source.Clients.Count; ++i)
            {
                if (source.Clients[i].Id == id)
                {
                    source.Clients.RemoveAt(i);
                    return;
                }
            }
            throw new Exception("Элемент не найден");
        }
    }
}
