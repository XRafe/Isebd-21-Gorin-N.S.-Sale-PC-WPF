using SalePCServiceDAL.BindingModels;
using SalePCServiceDAL.Interfaces;
using SalePCServiceDAL.ViewModels;
using System;
using System.Collections.Generic;
using System.Windows;
using Unity;

namespace SalePCView
{
    /// <summary>
    /// Логика взаимодействия для WindowPutOnStock.xaml
    /// </summary>
    public partial class WindowPutOnStock : Window
    {
        [Dependency]
        public IUnityContainer Container { get; set; }

        private readonly IStockService serviceS;

        private readonly IHardwareService serviceI;

        private readonly IMainService serviceM;

        public WindowPutOnStock(IStockService serviceS, IHardwareService serviceI, IMainService serviceM)
        {
            InitializeComponent();
            Loaded += WindowPutOnStock_Load;
            this.serviceS = serviceS;
            this.serviceI = serviceI;
            this.serviceM = serviceM;
        }

        private void WindowPutOnStock_Load(object sender, EventArgs e)
        {
            try
            {
                List<HardwareViewModel> listI = serviceI.GetList();
                if (listI != null)
                {
                    comboBoxHardware.DisplayMemberPath = "HardwareName";
                    comboBoxHardware.SelectedValuePath = "Id";
                    comboBoxHardware.ItemsSource = listI;
                    comboBoxHardware.SelectedItem = null;
                }
                List<StockViewModel> listS = serviceS.GetList();
                if (listS != null)
                {
                    comboBoxStock.DisplayMemberPath = "StockName";
                    comboBoxStock.SelectedValuePath = "Id";
                    comboBoxStock.ItemsSource = listS;
                    comboBoxStock.SelectedItem = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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
            if (comboBoxStock.SelectedItem == null)
            {
                MessageBox.Show("Выберите базу", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                serviceM.PutHardwareOnStock(new StockHardwareBindingModel
                {
                    HardwareId = Convert.ToInt32(comboBoxHardware.SelectedValue),
                    StockId = Convert.ToInt32(comboBoxStock.SelectedValue),
                    Count = Convert.ToInt32(textBoxCount.Text)
                });
                MessageBox.Show("Сохранение прошло успешно", "Информация",
                    MessageBoxButton.OK, MessageBoxImage.Information);
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
