using ApiProductOrders.Models;
using ApiProductOrders.Views;
using DesafioOrdens.Models;
using OfficeOpenXml;
using System.Runtime.Caching;

namespace ApiProductOrders.Repositories
{
    public class ApontamentosRepository : IApontamentosRepository
    {
        public ViewNotes ImportNotes()
        {
            int totalNotesCount = 0;
            int deletedOrdersCount = 0;
            int updatedOrders = 0;
            int notesFailureCount = 0;
            bool isMatch;
            List<ApontamentosModel> apontamentosList = new List<ApontamentosModel>();

            ObjectCache cache = MemoryCache.Default;
            var productOrdersList = cache[Constants.ProductOrderCache] as List<ProductOrderModel>;

            ViewNotes viewNotes = new ViewNotes();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            FileInfo arquivoExcel = new FileInfo(Constants.PathNotes);
            using (ExcelPackage pacote = new ExcelPackage(arquivoExcel))
            {
                ExcelWorksheet planilha = pacote.Workbook.Worksheets[0];

                int linhaFinal = planilha.Dimension.End.Row;
                totalNotesCount = linhaFinal - 1;

                for (int linha = 2; linha <= linhaFinal; linha++)
                {
                    isMatch = false;

                    int orderNumber = Int32.Parse(planilha.Cells[linha, 1].Text);
                    int operationNumber = Int32.Parse(planilha.Cells[linha, 2].Text);
                    double quantity = Double.Parse(planilha.Cells[linha, 3].Text);
                    string productionDateTime = planilha.Cells[linha, 4].Text;

                    foreach (var order in productOrdersList)
                    {
                        //Encontro o apontamento correspondente a ordem original
                        if (orderNumber == order.orderNumber && operationNumber == order.operationNumber)
                        {
                            isMatch = true;

                            //Deleto apontamentos com quantidade maior ou igual a ordem original
                            if (quantity >= order.quantity)
                            {
                                planilha.DeleteRow(linha);
                                deletedOrdersCount++;
                                linhaFinal--;
                            }
                            else
                            {
                                ApontamentosModel apontamento = new ApontamentosModel();
                                apontamento.orderId = order.orderId;
                                apontamento.orderNumber = orderNumber;
                                apontamento.operationNumber = operationNumber;
                                apontamento.quantity = quantity;
                                apontamento.productionDateTime = !productionDateTime.ToLower().Equals("null") ? DateTime.Parse(productionDateTime) : null;

                                apontamentosList.Add(apontamento);
                            }
                            //Atualizo ordens apontadas com quantidade menor do que a ordem original
                            if (quantity < order.quantity)
                            {
                                planilha.Cells[linha, 3].Value = order.quantity;
                                planilha.Cells[linha, 5].Value = Constants.UpdatedQuantity;
                                updatedOrders++;
                            }
                        }
                    }

                    //Adiciono a ordem que não consta no excel de ordens a uma lista de Falha de Apontamento
                    if (!isMatch)
                    {
                        planilha.Cells[linha, 5].Value = Constants.PointingFailure;
                        notesFailureCount++;
                    }
                }
                pacote.Save();
                pacote.Dispose();
            }
            AddNotesListToCache(apontamentosList);

            viewNotes.totalNotesCount = totalNotesCount;
            viewNotes.deletedOrdersCount = deletedOrdersCount;
            viewNotes.remainingOrders = totalNotesCount - deletedOrdersCount;
            viewNotes.updatedOrders = updatedOrders;
            viewNotes.notesFailureCount = notesFailureCount;
            return viewNotes;
        }

        public void AddNotesListToCache(List<ApontamentosModel> apontamentosList)
        {
            ObjectCache cache = MemoryCache.Default;

            var cachedData = cache[Constants.NotesCache] as List<ApontamentosModel>;

            if (cachedData == null)
            {
                CacheItemPolicy policy = new CacheItemPolicy();

                cache.Set(Constants.NotesCache, apontamentosList, policy);
                cachedData = apontamentosList;
            }
        }
    }
}
