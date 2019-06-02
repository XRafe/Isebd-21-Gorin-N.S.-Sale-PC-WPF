using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalePC
{
    public class Client
    {
        public int Id { get; set; }
        [Required]
        public string ClientFIO { get; set; }
        [ForeignKey("ClientId")]
        public virtual List<Order> Orders { get; set; }
        public virtual List<PC> PCs { get; set; }
    }
}
