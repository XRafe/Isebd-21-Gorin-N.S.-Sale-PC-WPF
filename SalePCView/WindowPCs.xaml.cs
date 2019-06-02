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
    /// Логика взаимодействия для WindowPCs.xaml
    /// </summary>
    public partial class WindowPCs : Window
    {
        [Dependency]
        public IUnityContainer Container { get; set; }

        private readonly IPCService service;

        public WindowPCs(IPCService service)
        {
            InitializeComponent();
            Loaded += WindowPCs_Load;
            this.service = service;
        }

        private void WindowPCs_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                List<PCViewModel> list = service.GetList();
                if (list != null)
                {
                    dataGridViewPCs.ItemsSource = list;
                    dataGridViewPCs.Columns[0].Visibility = Visibility.Hidden;
                    dataGridViewPCs.Columns[1].Width = DataGridLength.Auto;
                    dataGridViewPCs.Columns[3].Visibility = Visibility.Hidden;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<WindowPC>();
            if (form.ShowDialog() == true)
                LoadData();
        }

        private void buttonUpd_Click(object sender, EventArgs e)
        {
            if (dataGridViewPCs.SelectedItem != null)
            {
                var form = Container.Resolve<WindowPC>();
                form.Id = ((PCViewModel)dataGridViewPCs.SelectedItem).Id;
                if (form.ShowDialog() == true)
                    LoadData();
            }
        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
            if (dataGridViewPCs.SelectedItem != null)
            {
                if (MessageBox.Show("Удалить запись?", "Внимание",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {

                    int id = ((PCViewModel)dataGridViewPCs.SelectedItem).Id;
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
