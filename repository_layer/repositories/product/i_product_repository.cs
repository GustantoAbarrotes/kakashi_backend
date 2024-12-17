using RepositoryLayer.Repositories.Product.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RepositoryLayer.Repositories.Product
{
    public interface IProductRepository
    {
        public Task<List<ProductEntity>> GetAll();
        public Task<ProductEntity> GetById(string id);
    }
}
