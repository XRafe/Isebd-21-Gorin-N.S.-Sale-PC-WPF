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
        public int Count { get; set; }
    }
}
