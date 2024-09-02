using ApiProductOrders.Services;
using ApiProductOrders.Views;
using DesafioOrdens.Models;
using Microsoft.AspNetCore.Mvc;

namespace DesafioOrdens.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductOrderController : ControllerBase
    {
        private readonly IProductOrderService _productOrderService;

        public ProductOrderController(IProductOrderService productOrderService)
        {
            _productOrderService = productOrderService;
        }

        [HttpGet("importOrders")]
        public ActionResult<string> ImportOrders()
        {
            ViewOrders productOrderCount = _productOrderService.ImportOrders();
            return Ok(productOrderCount.ToString());
        }

        [HttpGet("importNotes")]
        public ActionResult<String> ImportNotes()
        {
            ViewNotes viewNotes = _productOrderService.ImportNotes();
            return Ok(viewNotes.ToString());
        }

        [HttpGet("importOrdersById")]
        public ActionResult<List<ProductOrderModel>> ImportOrdersById(int orderId)
        {
            List<ProductOrderModel> ordersList = _productOrderService.ImportOrdersById(orderId);
            return Ok(ordersList);
        }

        [HttpGet("memoclear")]
        public void MemoClear()
        {
            _productOrderService.ClearMemo();
        }
    }
}
