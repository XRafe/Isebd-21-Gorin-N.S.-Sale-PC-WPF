using SalePCServiceDAL.Interfaces;
using SalePCServiceImplementDataBase;
using SalePCServiceImplementDataBase.Implementations;
using System;
using System.Data.Entity;
using System.Windows;
using Unity;
using Unity.Lifetime;

namespace SalePCView
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {

        App()
        {
            InitializeComponent();
        }

        [STAThread]
        static void Main()
        {
            App app = new App();
            var container = BuildUnityContainer();
            app.Run(container.Resolve<MainWindow>());
        }

        public static IUnityContainer BuildUnityContainer()
        {
            var currentContainer = new UnityContainer();
            currentContainer.RegisterType<DbContext, AbstractPCDbContext>(new
           HierarchicalLifetimeManager());
            currentContainer.RegisterType<IClientService, ClientServiceDB>(new
           HierarchicalLifetimeManager());
            currentContainer.RegisterType<IHardwareService, HardwareServiceDB>(new
           HierarchicalLifetimeManager());
            currentContainer.RegisterType<IPCService, PCServiceDB>(new
           HierarchicalLifetimeManager());
            currentContainer.RegisterType<IMainService, MainServiceDB>(new
           HierarchicalLifetimeManager());
            currentContainer.RegisterType<IStockService, StockServiceDB>(new 
           HierarchicalLifetimeManager());
            return currentContainer;
        }
    }
}
