using DTOLayer.Product;
using RepositoryLayer.Repositories.Product;
using RepositoryLayer.Repositories.Product.Entity;
using ServiceLayer.Product.Mappers;
using System.Collections.Generic;
using System.Linq;
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
        public async Task<List<ProductResponseDTO>> FindByText(string text, int page, int sizePage)
        {
            var res = await _rep.FindByText(text, page, sizePage);
            return res.Select(p => p.ToResponseDTO()).ToList();
        }
        public async Task<string> InsertBarcode(BarcodeCreateRequestDTO dto, string usuario) 
        {
            try
            {
                var entity = dto.ToEntity();
                var res = await _rep.InsertBarcode(entity, usuario);
                return res ? "ok" : "rows0";
            }
            catch (System.Exception ex)
            {
                return ex.Message;
            }
         
        }
    }
}
