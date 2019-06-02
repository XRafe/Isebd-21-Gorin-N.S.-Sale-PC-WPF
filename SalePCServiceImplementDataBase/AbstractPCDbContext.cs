using SalePC;
using System.Data.Entity;


namespace SalePCServiceImplementDataBase
{
    public class AbstractPCDbContext : DbContext
    {
    public AbstractPCDbContext() : base("AbstractPCDatabase")
        {
            //настройки конфигурации для entity
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
            var ensureDLLIsCopied =
           System.Data.Entity.SqlServer.SqlProviderServices.Instance;
        }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Hardware> Hardwares { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<PC> PCs { get; set; }
        public virtual DbSet<PCHardwares> PCHardwares { get; set; }
        public virtual DbSet<Stock> Stocks { get; set; }
        public virtual DbSet<StockHardware> StockHardwares { get; set; }
    }
}
