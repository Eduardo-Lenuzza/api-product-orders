namespace ApiProductOrders.Models
{
    public class ApontamentosModel
    {
        public int orderId { get; set; }
        public int orderNumber { get; set; }
        public int operationNumber { get; set; }
        public double quantity { get; set; }
        public DateTime? productionDateTime { get; set; }

        public override string ToString() => $"orderId= {orderId} " +
            $"\n orderNumber= {orderNumber} " +
            $"\n operationNumber= {operationNumber} " +
            $"\n quantity= {quantity} " +
            $"\n productDateTime= {productionDateTime}";
    }
}
