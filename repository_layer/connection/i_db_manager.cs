using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Threading.Tasks;
using static RepositoryLayer.Utils.Consts;

namespace RepositoryLayer.Connection
{
    public interface IDBManager
    {
        public Task<OracleConnection> GetConnection();
        public Task<OracleConnection> GetConnectionSec();
        public void BuildDataConnection(TypeConn tipo);
        public void BuildDataConnectionSec(TypeConn tipo);
        public Dictionary<string, string> GetInfo();
    }
}
