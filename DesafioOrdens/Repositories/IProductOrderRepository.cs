using DesafioOrdens.Models;

namespace ApiProductOrders.Repositories
{
    public interface IProductOrderRepository
    {
        public List<ProductOrderModel> ImportProductOrders();
    }
}
