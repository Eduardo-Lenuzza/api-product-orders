using DesafioOrdens.Models;
using OfficeOpenXml;
using System.Runtime.Caching;

namespace ApiProductOrders.Repositories
{
    public class ProductOrderRepository : IProductOrderRepository
    {
        public List<ProductOrderModel> ImportProductOrders()
        {
            int objetosImportados;

            List<ProductOrderModel> productOrdersList = new List<ProductOrderModel>();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            FileInfo arquivoExcel = new FileInfo(Constants.PathProductOrders);
            using (ExcelPackage pacote = new ExcelPackage(arquivoExcel))
            {
                ExcelWorksheet planilha = pacote.Workbook.Worksheets[0];

                int colunaOrderNumber = 1;
                int colunaOperationNumber = 2;

                int linhaFinal = planilha.Dimension.End.Row;

                Dictionary<string, int> combinacoesUnicas = new Dictionary<string, int>();
                int orderIdAtual = 1;

                for (int linha = 2; linha <= linhaFinal; linha++)
                {
                    var orderNumber = planilha.Cells[linha, colunaOrderNumber].Text;
                    var operationNumber = planilha.Cells[linha, colunaOperationNumber].Text;
                    var chaveUnica = $"{orderNumber}{operationNumber}";

                    if (!combinacoesUnicas.ContainsKey(chaveUnica))
                    {
                        ProductOrderModel productOrderModel = new ProductOrderModel();

                        combinacoesUnicas[chaveUnica] = orderIdAtual;

                        orderIdAtual++;

                        productOrderModel.orderId = orderIdAtual;
                        productOrderModel.orderNumber = Int32.Parse(planilha.Cells[linha, 1].Text);
                        productOrderModel.operationNumber = Int32.Parse(planilha.Cells[linha, 2].Text);
                        productOrderModel.quantity = Double.Parse(planilha.Cells[linha, 3].Text);
                        productOrderModel.dueDate = DateTime.Parse(planilha.Cells[linha, 4].Text);
                        productOrderModel.productNumber = Int32.Parse(planilha.Cells[linha, 5].Text);
                        productOrderModel.product = planilha.Cells[linha, 6].Text;

                        productOrdersList.Add(productOrderModel);
                    }
                }
                objetosImportados = combinacoesUnicas.Count;
            }
            AddProductOrderListToCache(productOrdersList);
            return productOrdersList;
        }
        public void AddProductOrderListToCache(List<ProductOrderModel> productOrdersList)
        {
            ObjectCache cache = MemoryCache.Default;

            var cachedData = cache[Constants.ProductOrderCache] as List<ProductOrderModel>;

            if (cachedData == null)
            {
                CacheItemPolicy policy = new CacheItemPolicy();

                cache.Set(Constants.ProductOrderCache, productOrdersList, policy);
                cachedData = productOrdersList;
            }
        }
    }
}
