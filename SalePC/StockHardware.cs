using System.ComponentModel.DataAnnotations;

namespace SalePC
{
    /// <summary>
    /// Сколько компонентов хранится на складе
    /// </summary>
    public class StockHardware
    {
        public int Id { get; set; }
        public int StockId { get; set; }
        public int HardwareId { get; set; }
        [Required]
        public int Count { get; set; }
        public virtual Hardware Hardware { get; set; }
        public virtual PC PC { get; set; }

    }
}
