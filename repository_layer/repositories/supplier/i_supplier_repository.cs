using RepositoryLayer.Repositories.Supplier.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RepositoryLayer.Repositories.Supplier
{
    public interface ISupplierRepository
    {
        public Task<List<SupplierEntity>> GetAll(int cveProv, string rfc, string tradeName
            , string supplierName, string email);
        public Task<List<string>> GetComboBox(int cveProv, string rfc, string tradeName
            , string supplierName, string email);
        public Task<List<SupplierEntity>> ApproveBalance(int keyBranch, int keyCustomer, int cveProv
           , string filtersString, string tradeName, string supplierName, string email);
    }
}
