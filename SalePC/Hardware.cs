using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalePC
{
    /// <summary>
    /// Компонент, требуемый для изготовления изделия
    /// </summary>
    public class Hardware
    {
        public int Id { get; set; }
        [Required]
        public string HardwareName { get; set; }
        [ForeignKey("HardwareId")]
        public virtual List<StockHardware> StockHardwares { get; set; }
        public virtual List<PCHardwares> PCHardwares { get; set; }
    }
}
