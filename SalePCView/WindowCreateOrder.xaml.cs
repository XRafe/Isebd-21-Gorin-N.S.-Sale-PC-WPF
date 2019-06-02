using SalePCServiceDAL.BindingModels;
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
    /// Логика взаимодействия для WindowCreateOrder.xaml
    /// </summary>
    public partial class WindowCreateOrder : Window
    {
        [Dependency]
        public IUnityContainer Container { get; set; }

        private readonly IClientService serviceC;

        private readonly IPCService serviceCF;

        private readonly IMainService serviceM;


        public WindowCreateOrder(IClientService serviceC, IPCService serviceCF, IMainService serviceM)
        {
            InitializeComponent();
            Loaded += FormCreateOrder_Load;
            comboBoxPC.SelectionChanged += comboBoxPC_SelectedIndexChanged;

            comboBoxPC.SelectionChanged += new SelectionChangedEventHandler(comboBoxPC_SelectedIndexChanged);
            this.serviceC = serviceC;
            this.serviceCF = serviceCF;
            this.serviceM = serviceM;
        }

        private void FormCreateOrder_Load(object sender, EventArgs e)
        {
            try
            {
                List<ClientViewModel> listC = serviceC.GetList();
                if (listC != null)
                {
                    comboBoxClient.DisplayMemberPath = "ClientFIO";
                    comboBoxClient.SelectedValuePath = "Id";
                    comboBoxClient.ItemsSource = listC;
                    comboBoxPC.SelectedItem = null;
                }
                List<PCViewModel> listCF = serviceCF.GetList();
                if (listCF != null)
                {
                    comboBoxPC.DisplayMemberPath = "PCName";
                    comboBoxPC.SelectedValuePath = "Id";
                    comboBoxPC.ItemsSource = listCF;
                    comboBoxPC.SelectedItem = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CalcSum()
        {
            if (comboBoxPC.SelectedItem != null && !string.IsNullOrEmpty(textBoxCount.Text))
            {
                try
                {
                    int id = ((PCViewModel)comboBoxPC.SelectedItem).Id;
                    PCViewModel product = serviceCF.GetElement(id);
                    int count = Convert.ToInt32(textBoxCount.Text);
                    textBoxSum.Text = (count * product.Price).ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void textBoxCount_TextChanged(object sender, EventArgs e)
        {
            CalcSum();
        }

        private void comboBoxPC_SelectedIndexChanged(object sender, EventArgs e)
        {
            CalcSum();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxCount.Text))
            {
                MessageBox.Show("Заполните поле Количество", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (comboBoxClient.SelectedItem == null)
            {
                MessageBox.Show("Выберите получателя", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (comboBoxPC.SelectedItem == null)
            {
                MessageBox.Show("Выберите изделие", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                serviceM.CreateOrder(new OrderBindingModel
                {
                    ClientId = ((ClientViewModel)comboBoxClient.SelectedItem).Id,
                    PCId = ((PCViewModel)comboBoxPC.SelectedItem).Id,
                    Count = Convert.ToInt32(textBoxCount.Text),
                    Sum = Convert.ToInt32(textBoxSum.Text)
                });
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
