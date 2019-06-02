using SalePCServiceDAL.BindingModels;
using SalePCServiceDAL.Interfaces;
using SalePCServiceDAL.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;
using Unity;

namespace SalePCView
{
    /// <summary>
    /// Логика взаимодействия для WindowStock.xaml
    /// </summary>
    public partial class WindowStock : Window
    {
        [Dependency]
        public IUnityContainer Container { get; set; }

        public int Id { set { id = value; } }

        private readonly IStockService service;

        private int? id;

        public WindowStock(IStockService service)
        {
            InitializeComponent();
            Loaded += WindowStock_Load;
            this.service = service;
        }

        private void WindowStock_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    StockViewModel view = service.GetElement(id.Value);
                    if (view != null)
                    {
                        textBoxName.Text = view.StockName;
                        dataGridViewStock.ItemsSource = view.StockHardware;
                        dataGridViewStock.Columns[0].Visibility = Visibility.Hidden;
                        dataGridViewStock.Columns[1].Visibility = Visibility.Hidden;
                        dataGridViewStock.Columns[2].Visibility = Visibility.Hidden;
                        dataGridViewStock.Columns[3].Width = DataGridLength.Auto;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxName.Text))
            {
                MessageBox.Show("Заполните название", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                if (id.HasValue)
                {
                    service.UpdElement(new StockBindingModel
                    {
                        Id = id.Value,
                        StockName = textBoxName.Text
                    });
                }
                else
                {
                    service.AddElement(new StockBindingModel
                    {
                        StockName = textBoxName.Text
                    });
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
