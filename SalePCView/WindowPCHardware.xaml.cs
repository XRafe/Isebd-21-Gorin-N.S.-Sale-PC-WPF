using SalePCServiceDAL.Interfaces;
using SalePCServiceDAL.ViewModels;
using System;
using System.Collections.Generic;
using System.Windows;
using Unity;

namespace SalePCView
{
    /// <summary>
    /// Логика взаимодействия для WindowPCHardware.xaml
    /// </summary>
    public partial class WindowPCHardware : Window
    {
        [Dependency]
        public IUnityContainer Container { get; set; }

        public PCHardwareViewModel Model { set { model = value; } get { return model; } }

        private readonly IHardwareService service;

        private PCHardwareViewModel model;

        public WindowPCHardware(IHardwareService service)
        {
            InitializeComponent();
            Loaded += WindowPCHardware_Load;
            this.service = service;
        }

        private void WindowPCHardware_Load(object sender, EventArgs e)
        {
            List<HardwareViewModel> list = service.GetList();
            try
            {
                if (list != null)
                {
                    comboBoxHardware.DisplayMemberPath = "HardwareName";
                    comboBoxHardware.SelectedValuePath = "Id";
                    comboBoxHardware.ItemsSource = list;
                    comboBoxHardware.SelectedItem = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if (model != null)
            {
                comboBoxHardware.IsEnabled = false;
                foreach (HardwareViewModel item in list)
                {
                    if (item.HardwareName == model.HardwareNames)
                    {
                        comboBoxHardware.SelectedItem = item;
                    }
                }
                textBoxCount.Text = model.Count.ToString();
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxCount.Text))
            {
                MessageBox.Show("Заполните поле Количество", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (comboBoxHardware.SelectedItem == null)
            {
                MessageBox.Show("Выберите заготовку", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                if (model == null)
                {
                    model = new PCHardwareViewModel
                    {
                        HardwareId = Convert.ToInt32(comboBoxHardware.SelectedValue),
                        HardwareNames = comboBoxHardware.Text,
                        Count = Convert.ToInt32(textBoxCount.Text)
                    };
                }
                else
                {
                    model.Count = Convert.ToInt32(textBoxCount.Text);
                }
                MessageBox.Show("Сохранение прошло успешно", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
