using SalePC;
using System.Collections.Generic;

namespace SalePCServiceImplementList
{
    class DataListSingleton
    {
        private static DataListSingleton instance;
        public List<Client> Clients { get; set; }

        public List<Hardware> Hardwares { get; set; }
        public List<Order> Orders { get; set; }
        public List<PC> PCs { get; set; }
        public List<PCHardwares> PCHardwares { get; set; }



        private DataListSingleton()
        {
            Clients = new List<Client>();
            Hardwares = new List<Hardware>();
            Orders = new List<Order>();
            PCs = new List<PC>();
            PCHardwares = new List<PCHardwares>();
        }
        public static DataListSingleton GetInstance()
        {
            if (instance == null)
            {
                instance = new DataListSingleton();
            }
            return instance;
        }

    }
}
