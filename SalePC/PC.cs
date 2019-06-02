using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalePC
{
    /// <summary>
    /// Изделие, изготавливаемое в магазине
    /// </summary>
    public class PC
    {
        public int Id { get; set; }
        [Required]
        public string PCName { get; set; }
        [Required]
        public decimal Price { get; set; }
        [ForeignKey("PCId")]
        public virtual List<PCHardwares> PCHardwares { get; set; }
    }

}
