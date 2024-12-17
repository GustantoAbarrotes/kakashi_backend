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
        public async Task<ProductEntity> GetById(string id)
        {
            return await _rep.GetById(id);
        }
    }
}
