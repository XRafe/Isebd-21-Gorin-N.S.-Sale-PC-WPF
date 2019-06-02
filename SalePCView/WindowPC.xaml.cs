using SalePCServiceDAL.BindingModels;
using SalePCServiceDAL.Interfaces;
using SalePCServiceDAL.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Unity;

namespace SalePCView
{
    /// <summary>
    /// Логика взаимодействия для WindowPC.xaml
    /// </summary>
    public partial class WindowPC : Window
    {
        [Dependency]
        public IUnityContainer Container { get; set; }

        public int Id { set { id = value; } }

        private readonly IPCService service;

        private int? id;

        private List<PCHardwareViewModel> PCHardwares;

        public WindowPC(IPCService service)
        {
            InitializeComponent();
            Loaded += WindowPC_Load;
            this.service = service;
        }

        private void WindowPC_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    PCViewModel view = service.GetElement(id.Value);
                    if (view != null)
                    {
                        textBoxName.Text = view.PCName;
                        textBoxPrice.Text = view.Price.ToString();
                        PCHardwares = view.PCHardwares;
                        LoadData();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
                PCHardwares = new List<PCHardwareViewModel>();
        }

        private void LoadData()
        {
            try
            {
                if (PCHardwares != null)
                {
                    dataGridView.ItemsSource = null;
                    dataGridView.ItemsSource = PCHardwares;
                    dataGridView.Columns[0].Visibility = Visibility.Hidden;
                    dataGridView.Columns[1].Visibility = Visibility.Hidden;
                    dataGridView.Columns[2].Visibility = Visibility.Hidden;
                    dataGridView.Columns[3].Width = DataGridLength.Auto;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<WindowPCHardware>();
            if (form.ShowDialog() == true)
            {
                if (form.Model != null)
                {
                    if (id.HasValue)
                        form.Model.PCId = id.Value;
                    PCHardwares.Add(form.Model);
                }
                LoadData();
            }
        }

        private void buttonUpd_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedItem != null)
            {
                var form = Container.Resolve<WindowPCHardware>();
                form.Model = PCHardwares[dataGridView.SelectedIndex];
                if (form.ShowDialog() == true)
                {
                    PCHardwares[dataGridView.SelectedIndex] = form.Model;
                    LoadData();
                }
            }
        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedItem != null)
            {
                if (MessageBox.Show("Удалить запись?", "Внимание",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        PCHardwares.RemoveAt(dataGridView.SelectedIndex);
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

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxName.Text))
            {
                MessageBox.Show("Заполните название", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrEmpty(textBoxPrice.Text))
            {
                MessageBox.Show("Заполните цену", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (PCHardwares == null || PCHardwares.Count == 0)
            {
                MessageBox.Show("Заполните заготовки", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                List<PCHardwareBindingModel> PCHardwareBM = new List<PCHardwareBindingModel>();
                for (int i = 0; i < PCHardwares.Count; ++i)
                {
                    PCHardwareBM.Add(new PCHardwareBindingModel
                    {
                        Id = PCHardwares[i].Id,
                        PCId = PCHardwares[i].PCId,
                        HardwareId = PCHardwares[i].HardwareId,
                        Count = PCHardwares[i].Count
                    });
                }
                if (id.HasValue)
                {
                    service.UpdElement(new PCBindingModel
                    {
                        Id = id.Value,
                        PCName = textBoxName.Text,
                        Price = Convert.ToInt32(textBoxPrice.Text),
                        PCHardwares = PCHardwareBM
                    });
                }
                else
                {
                    service.AddElement(new PCBindingModel
                    {
                        PCName = textBoxName.Text,
                        Price = Convert.ToInt32(textBoxPrice.Text),
                        PCHardwares = PCHardwareBM
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
