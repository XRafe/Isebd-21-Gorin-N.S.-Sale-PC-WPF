using SalePCServiceDAL.BindingModels;
using SalePCServiceDAL.Interfaces;
using SalePCServiceDAL.ViewModels;
using System;
using System.Windows;
using Unity;

namespace SalePCView
{
    /// <summary>
    /// Логика взаимодействия для WindowHardware.xaml
    /// </summary>
    public partial class WindowHardware : Window
    {
        [Dependency]
        public IUnityContainer Container { get; set; }

        public int Id { set { id = value; } }

        private readonly IHardwareService service;

        private int? id;

        public WindowHardware(IHardwareService service)
        {
            InitializeComponent();
            Loaded += FormHardware_Load;
            this.service = service;
        }

        private void FormHardware_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    HardwareViewModel view = service.GetElement(id.Value);
                    if (view != null)
                    {
                        textBoxName.Text = view.HardwareName;
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
                    service.UpdElement(new HardwareBindingModel
                    {
                        Id = id.Value,
                        HardwareName = textBoxName.Text
                    });
                }
                else
                {
                    service.AddElement(new HardwareBindingModel
                    {
                        HardwareName = textBoxName.Text
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
