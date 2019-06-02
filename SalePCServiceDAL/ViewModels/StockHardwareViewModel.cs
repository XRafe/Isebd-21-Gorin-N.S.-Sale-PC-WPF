using System.ComponentModel;


namespace SalePCServiceDAL.ViewModels
{
    public class StockHardwareViewModel
    {
        public int Id { get; set; }
        public int StockId { get; set; }
        public int HardwareId { get; set; }
        [DisplayName("Название компонента")]
        public string HardwareName { get; set; }
        [DisplayName("Количество")]
        public int Count { get; set; }
    }

}
