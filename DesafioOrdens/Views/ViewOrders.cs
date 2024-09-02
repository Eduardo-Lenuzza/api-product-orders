namespace ApiProductOrders.Views
{
    public class ViewOrders
    {
        public int orderCount { get; set; }

        public ViewOrders(int orderCount)
        {
            this.orderCount = orderCount;
        }

        public override string ToString()
        {
            return $"Total de objetos importados {this.orderCount}";
        }
    }
}
