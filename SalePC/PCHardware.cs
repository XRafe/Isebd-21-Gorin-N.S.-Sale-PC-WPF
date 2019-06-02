using System.ComponentModel.DataAnnotations;

namespace SalePC
{
    /// <summary>
    /// Сколько компонентов, требуется при изготовлении изделия
    /// </summary>
    public class PCHardwares
    {
        public int Id { get; set; }
        public int PCId { get; set; }
        public int HardwareId { get; set; }
        [Required]
        public int Count { get; set; }
        public virtual Hardware Hardware { get; set; }
        public virtual PC PC { get; set; }
    }
}
