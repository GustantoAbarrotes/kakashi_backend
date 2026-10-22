using RepositoryLayer.Repositories.Product.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RepositoryLayer.Repositories.Product
{
    public interface IProductRepository
    {
        public Task<List<ProductEntity>> GetAll();
        public Task<ProductEntity> GetByBarCode(string id);
        public Task<List<ProductEntity>> FindByText(string text, int page, int sizePage);
        Task<bool> InsertBarcode(BarcodeEntity entity, string usuario);
    }
}
