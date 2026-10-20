using RepositoryLayer.Repositories.Product;
using RepositoryLayer.Repositories.Product.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceLayer.Product
{
    public class ProductService
    {
        private readonly IProductRepository _rep;
        public ProductService(IProductRepository rep)
        {
            _rep = rep;
        }

        public async Task<List<ProductEntity>> GetAll()
        {
            return await _rep.GetAll();
        }
        public async Task<ProductEntity> GetByBarCode(string barCode)
        {
            return await _rep.GetByBarCode(barCode);
        }
        public async Task<List<ProductEntity>> FindByText(string text, int page, int sizePage)
        {
            return await _rep.FindByText(text, page, sizePage);
        }
    }
}
