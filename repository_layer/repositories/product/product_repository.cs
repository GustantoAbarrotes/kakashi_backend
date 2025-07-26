using Oracle.ManagedDataAccess.Client;
using RepositoryLayer.Connection;
using RepositoryLayer.Repositories.Product.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Repositories.Product
{
    public class ProductRepository: IProductRepository
    {
        private readonly IDBManager _dbManager;
        public ProductRepository(IDBManager dbManager)
        {
            _dbManager = dbManager;
        }
        public async Task<List<ProductEntity>> GetAll()
        {
            var datos = new List<ProductEntity>();
            try
            {
                #region querySql
                var query = new StringBuilder();
                query.AppendLine("SELECT ART.*");
                query.AppendLine("FROM ARTICULOS ART ");
                #endregion
                using (var oConnection = await _dbManager.GetConnection())
                using (var oCommand = new OracleCommand(query.ToString(), oConnection))
                {
                    await oConnection.OpenAsync();
                    using (var reader = await oCommand.ExecuteReaderAsync())
                    {
                        var obj = new ProductEntity();
                        while (await reader.ReadAsync())
                        {
                            var i = 0;
                            obj.CodigoProducto = reader[i] == DBNull.Value ? string.Empty : reader.GetString(i); i++;
                            obj.CodigoBarra = reader[i] == DBNull.Value ? string.Empty : reader.GetString(i); i++;
                            obj.FormaVentaCB = reader[i] == DBNull.Value ? string.Empty : reader.GetString(i); i++;
                            obj.Caja = reader[i] == DBNull.Value ? string.Empty : reader.GetString(i); i++;
                            obj.Pieza = reader[i] == DBNull.Value ? string.Empty : reader.GetString(i); i++;
                            obj.Descripcion = reader[i] == DBNull.Value ? string.Empty : reader.GetString(i); i++;
                            obj.Precio = reader[i] == DBNull.Value ? 0 : Convert.ToDecimal(reader.GetString(i));

                            datos.Add(obj);
                            obj = new ProductEntity();
                        }
                       obj = null; reader.Close(); oConnection.Close(); query.Clear();
                    }
                    return datos;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ProductEntity> GetById(string id)
        {
            ProductEntity obj = null;
            try
            {
                #region querySql
                var query = new StringBuilder();
                query.AppendLine("SELECT ART.*");
                query.AppendLine("FROM ARTICULOS ART ");
                query.AppendLine("WHERE ART.CODIGOBARRA=:ID");
                #endregion
                using (var oConnection = await _dbManager.GetConnection())
                using (var oCommand = new OracleCommand(query.ToString(), oConnection))
                {
                    oCommand.Parameters.Add(new OracleParameter(":ID", OracleDbType.Varchar2)).Value = id;
                    await oConnection.OpenAsync();
                    using (var reader = await oCommand.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            obj = new ProductEntity(); var i = 0;
                            obj.CodigoProducto = reader[i] == DBNull.Value ? string.Empty : reader.GetString(i); i++;
                            obj.CodigoBarra = reader[i] == DBNull.Value ? string.Empty : reader.GetString(i); i++;
                            obj.FormaVentaCB = reader[i] == DBNull.Value ? string.Empty : reader.GetString(i); i++;
                            obj.Caja = reader[i] == DBNull.Value ? string.Empty : reader.GetString(i); i++;
                            obj.Pieza = reader[i] == DBNull.Value ? string.Empty : reader.GetString(i); i++;
                            obj.Descripcion = reader[i] == DBNull.Value ? string.Empty : reader.GetString(i); i++;
                            obj.Precio = reader[i] == DBNull.Value ? 0 : Convert.ToDecimal(reader.GetString(i)); i++;
                        }
                        reader.Close(); await oConnection.CloseAsync(); query.Clear();
                    }
                    return obj;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}