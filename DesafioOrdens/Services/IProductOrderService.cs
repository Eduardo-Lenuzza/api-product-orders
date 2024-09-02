using ApiProductOrders.Views;
using DesafioOrdens.Models;

namespace ApiProductOrders.Services
{
    public interface IProductOrderService
    {
        public ViewOrders ImportOrders();
        public ViewNotes ImportNotes();
        public List<ProductOrderModel> ImportOrdersById(int orderId);
        public void ClearMemo();
    }
}
