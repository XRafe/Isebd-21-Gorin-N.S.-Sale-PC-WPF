using SalePCServiceDAL.Interfaces;
using SalePCServiceImplementList;
using System;
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
            currentContainer.RegisterType<IClientService, ClientServiceList>(new
           HierarchicalLifetimeManager());
            currentContainer.RegisterType<IHardwareService, HardwareServiceList>(new
           HierarchicalLifetimeManager());
            currentContainer.RegisterType<IPCService, PCServiceList>(new
           HierarchicalLifetimeManager());
            currentContainer.RegisterType<IMainService, MainServiceList>(new
           HierarchicalLifetimeManager());
            return currentContainer;
        }
    }
}
