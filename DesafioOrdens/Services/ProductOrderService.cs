using ApiProductOrders.Models;
using ApiProductOrders.Repositories;
using ApiProductOrders.Views;
using DesafioOrdens.Models;
using Microsoft.Extensions.Caching.Memory;
using System.Runtime.Caching;

namespace ApiProductOrders.Services
{
    public class ProductOrderService : IProductOrderService
    {
        private readonly IProductOrderRepository _productOrderRepository;
        private readonly IApontamentosRepository _apontamentosRepository;
        private readonly IMemoryCache _memoryCache;

        public ProductOrderService(IProductOrderRepository productOrderRepository,
            IApontamentosRepository apontamentosRepository, IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
            _productOrderRepository = productOrderRepository;
            _apontamentosRepository = apontamentosRepository;
        }


        public ViewOrders ImportOrders()
        {
            List<ProductOrderModel> productOrdersList;
            ObjectCache cache = System.Runtime.Caching.MemoryCache.Default;

            var cacheData = cache[Constants.ProductOrderCache] as List<ProductOrderModel>;
            if (cacheData == null)
            {
                productOrdersList = _productOrderRepository.ImportProductOrders();
            }
            else
            {
                productOrdersList = cacheData;
            }

            return new ViewOrders(productOrdersList.Count);
        }

        public ViewNotes ImportNotes()
        {
            ViewNotes viewNotes;
            ObjectCache cache = System.Runtime.Caching.MemoryCache.Default;

            var cacheProductOrder = cache[Constants.ProductOrderCache] as List<ProductOrderModel>;
            var cacheViewNotes = cache[Constants.ViewNotesCache] as ViewNotes;
            if (cacheProductOrder == null)
            {
                this.ImportOrders();
            }

            if (cacheViewNotes == null)
            {
                viewNotes = _apontamentosRepository.ImportNotes();
                AddViewNotesToCache(viewNotes);
            }
            else
            {
                viewNotes = cacheViewNotes;
            }

            viewNotes.uniqueNotesOrderIdCount = this.GetUniqueIdNotesCount();

            return viewNotes;
        }

        public int GetUniqueIdNotesCount()
        {
            ObjectCache cache = System.Runtime.Caching.MemoryCache.Default;
            var cacheNotesList = cache[Constants.NotesCache] as List<ApontamentosModel>;
            return cacheNotesList!.Select(obj => obj.orderId).Distinct().Count();
        }

        public List<ProductOrderModel> ImportOrdersById(int orderId)
        {
            ObjectCache cache = System.Runtime.Caching.MemoryCache.Default;

            var orderList = cache[Constants.ProductOrderCache] as List<ProductOrderModel>;
            if (orderList == null)
            {
                this.ImportOrders();
                orderList = cache[Constants.ProductOrderCache] as List<ProductOrderModel>;
            }
            List<ProductOrderModel> productOrderWithId = orderList!.Where(p => p.orderId == orderId).ToList();
            return productOrderWithId;
        }

        public void ClearMemo()
        {
            ObjectCache cache = System.Runtime.Caching.MemoryCache.Default;
            if (cache != null)
            {
                foreach (var cacheData in cache)
                {
                    cache.Remove(cacheData.Key);
                }
            }

            // Limpar o cache de memória
            if (_memoryCache is Microsoft.Extensions.Caching.Memory.MemoryCache memoryCache)
            {
                memoryCache.Compact(1.0); // Remove todos os itens no cache
            }
            // Força a coleta de lixo em todas as gerações
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

        public void AddViewOrdersToCache(ViewOrders viewOrder)
        {
            ObjectCache cache = System.Runtime.Caching.MemoryCache.Default;

            var cachedData = cache[Constants.ViewOrdersCache] as ViewOrders;

            if (cachedData == null)
            {
                CacheItemPolicy policy = new CacheItemPolicy();

                cache.Set(Constants.ViewOrdersCache, viewOrder, policy);
                cachedData = viewOrder;
            }
        }

        public void AddViewNotesToCache(ViewNotes viewNotes)
        {
            ObjectCache cache = System.Runtime.Caching.MemoryCache.Default;

            var cachedData = cache[Constants.ViewNotesCache] as ViewNotes;

            if (cachedData == null)
            {
                CacheItemPolicy policy = new CacheItemPolicy();

                cache.Set(Constants.ViewNotesCache, viewNotes, policy);
                cachedData = viewNotes;
            }
        }
    }
}
