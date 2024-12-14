using DTOLayer.Supplier;
using RepositoryLayer.Repositories.Supplier;
using RepositoryLayer.Repositories.Supplier.Entity;
using ServiceLayer.Supplier.Mappers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceLayer.Supplier
{
    public class SupplierService
    {
        private readonly ISupplierRepository _rep;
        public SupplierService(ISupplierRepository rep)
        {
            _rep = rep;
        }

        public async Task<List<SupplierEntity>> GetAll(int cveProv, string rfc, string tradeName
           , string supplierName, string email)
        {
            return await _rep.GetAll(cveProv, rfc, tradeName, supplierName, email);
        }

        public async Task<List<SupplierDTO>> ApproveBalance(int keyBranch, int keyCustomer, int cveProv
          , string filtersString, string tradeName, string supplierName, string email)
        {
            var res = await _rep.ApproveBalance(keyBranch, keyCustomer, cveProv
          , filtersString, tradeName, supplierName, email);
            return res.ConvertAll<SupplierDTO>(x => x.ToDTO());
        }
    }
}
