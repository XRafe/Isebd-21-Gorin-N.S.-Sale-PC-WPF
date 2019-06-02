using System;

namespace SalePC
{
    /// <summary>
    /// Заказ клиента
    /// </summary>
    public class Order
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int PCId { get; set; }
        public int Count { get; set; }
        public decimal Sum { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime DateCreate { get; set; }
        public DateTime? DateImplement { get; set; }
        public virtual Client Client { get; set; }
        public virtual PC PC { get; set; }
    }

}
