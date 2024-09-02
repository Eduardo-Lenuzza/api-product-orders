namespace DesafioOrdens.Models
{
    public class ProductOrderModel
    {
        public int orderId { get; set; }
        public int orderNumber { get; set; }
        public int operationNumber { get; set; }
        public int productNumber { get; set; }
        public double quantity { get; set; }
        public DateTime dueDate { get; set; }
        public string product {  get; set; }
    }
}
