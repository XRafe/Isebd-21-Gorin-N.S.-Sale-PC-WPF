using SalePCServiceDAL.Interfaces;
using SalePCServiceDAL.ViewModels;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Unity;

namespace SalePCView
{
    /// <summary>
    /// Логика взаимодействия для WindowHardwares.xaml
    /// </summary>
    public partial class WindowHardwares : Window
    {
        [Dependency]
        public IUnityContainer Container { get; set; }

        private readonly IHardwareService service;

        public WindowHardwares(IHardwareService service)
        {
            InitializeComponent();
            Loaded += WindowHardwares_Load;
            this.service = service;
        }

        private void WindowHardwares_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                List<HardwareViewModel> list = service.GetList();
                if (list != null)
                {
                    dataGridViewHardwares.ItemsSource = list;
                    dataGridViewHardwares.Columns[0].Visibility = Visibility.Hidden;
                    dataGridViewHardwares.Columns[1].Width = DataGridLength.Auto;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<WindowHardware>();
            if (form.ShowDialog() == true)
                LoadData();
        }

        private void buttonUpd_Click(object sender, EventArgs e)
        {
            if (dataGridViewHardwares.SelectedItem != null)
            {
                var form = Container.Resolve<WindowHardware>();
                form.Id = ((HardwareViewModel)dataGridViewHardwares.SelectedItem).Id;
                if (form.ShowDialog() == true)
                    LoadData();
            }
        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
            if (dataGridViewHardwares.SelectedItem != null)
            {
                if (MessageBox.Show("Удалить запись?", "Внимание",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    int id = ((HardwareViewModel)dataGridViewHardwares.SelectedItem).Id;
                    try
                    {
                        service.DelElement(id);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    LoadData();
                }
            }
        }

        private void buttonRef_Click(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}
