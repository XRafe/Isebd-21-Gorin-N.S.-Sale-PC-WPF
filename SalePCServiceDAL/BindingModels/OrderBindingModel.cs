namespace SalePCServiceDAL.BindingModels
{
    public class OrderBindingModel
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int PCId { get; set; }
        public int Count { get; set; }
        public decimal Sum { get; set; }
    }
}