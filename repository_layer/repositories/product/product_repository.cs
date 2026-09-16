using Oracle.ManagedDataAccess.Client;
using RepositoryLayer.Connection;
using RepositoryLayer.Repositories.Product.Entity;
using System;
using System.Collections.Generic;
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
                            obj.Precio = reader[i] == DBNull.Value ? 0 : Convert.ToDouble(reader.GetString(i));

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

        public async Task<ProductEntity> GetByBarCode(string barCode)
        {
            ProductEntity obj = null;
            try
            {
                #region querySql
                var query = new StringBuilder();
                //query.AppendLine("SELECT ART.*");
                //query.AppendLine("FROM ARTICULOS ART ");
                //query.AppendLine("WHERE ART.CODIGOBARRA=:ID");
                query.AppendLine("SELECT A.CODIGO_ARTICULO, D.CODIGO_BARRAS");
                query.AppendLine(",A.DESCRIPCION || ' ' || TRIM(B.DES_TABLAS) AS NOMBRE");
                query.AppendLine(",ROUND(CASE");
                query.AppendLine("       WHEN D.FORMAVENTA = 'W' AND A.CVE_IVA = 2");
                query.AppendLine("           THEN (A.PRECIO1 / A.NUMUNIDADES) * 1.16");
                query.AppendLine("       WHEN D.FORMAVENTA = 'W' AND A.CVE_IVA < 2");
                query.AppendLine("           THEN A.PRECIO1 / A.NUMUNIDADES");
                query.AppendLine("       WHEN (D.FORMAVENTA <> 'W' OR D.FORMAVENTA IS NULL) AND A.CVE_IVA = 2");
                query.AppendLine("           THEN (A.PRECIO1) * 1.16");
                query.AppendLine("       ELSE A.PRECIO1");
                query.AppendLine("    END, 5) AS Precio_Final");
                query.AppendLine("FROM ARTICULOS A");
                query.AppendLine("INNER JOIN TABLAS B");
                query.AppendLine("    ON B.TIPO_TABLAS=5");
                query.AppendLine("    AND B.CVE_TABLAS=A.CVE_MEDIDA");
                query.AppendLine("INNER JOIN SUBCODIGOS_HIJO C");
                query.AppendLine("    ON C.CVE_SUBCODIGO_PADRE = A.SUBCODIGO_ARTICULO");
                query.AppendLine("INNER JOIN CODIGOBARRAS D");
                query.AppendLine("    ON D.CODIGO_ARTICULO = A.CODIGO_ARTICULO");
                query.AppendLine("    AND D.CVE_SUBCODIGO_PADRE = C.CVE_SUBCODIGO_PADRE");
                query.AppendLine("    AND D.CVE_SUBCODIGO_HIJO = C.CVE_SUBCODIGO_HIJO");
                query.AppendLine("WHERE A.ACTIVO = 'A' AND D.CODIGO_BARRAS=:CODIGO_BARRAS");
                #endregion
                using (var oConnection = await _dbManager.GetConnection())
                using (var oCommand = new OracleCommand(query.ToString(), oConnection))
                {
                    oCommand.Parameters.Add(new OracleParameter(":CODIGO_BARRAS", OracleDbType.Varchar2)).Value = barCode;
                    await oConnection.OpenAsync();
                    using (var reader = await oCommand.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            obj = new ProductEntity(); var i = 0;
                            obj.CodigoProducto = reader[i] == DBNull.Value ? string.Empty : reader.GetString(i); i++;
                            obj.CodigoBarra = reader[i] == DBNull.Value ? string.Empty : reader.GetString(i); i++;
                            //obj.FormaVentaCB = reader[i] == DBNull.Value ? string.Empty : reader.GetString(i); i++;
                           // obj.Caja = reader[i] == DBNull.Value ? string.Empty : reader.GetString(i); i++;
                            //obj.Pieza = reader[i] == DBNull.Value ? string.Empty : reader.GetString(i); i++;
                            obj.Descripcion = reader[i] == DBNull.Value ? string.Empty : reader.GetString(i); i++;
                            obj.Precio = reader[i] == DBNull.Value ? 0 : Convert.ToDouble(reader.GetString(i)); i++;
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