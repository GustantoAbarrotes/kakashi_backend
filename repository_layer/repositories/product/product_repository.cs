using Oracle.ManagedDataAccess.Client;
using RepositoryLayer.Connection;
using RepositoryLayer.Repositories.Product.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Repositories.Product
{
    public class ProductRepository : IProductRepository
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
                query.AppendLine("SELECT ");
                query.AppendLine("    A.CODIGO_ARTICULO, ");
                query.AppendLine("    C.CVE_SUBCODIGO_HIJO, C.CVE_SUBCODIGO_PADRE, ");
                query.AppendLine("    D.CODIGO_BARRAS, ");
                query.AppendLine("    CASE ");
                query.AppendLine("        WHEN TRIM(D.FORMAVENTA) = 'W' THEN 'W' ");
                query.AppendLine("        ELSE 'C' ");
                query.AppendLine("    END AS FormaVenta, ");
                query.AppendLine("    A.DESCRIPCION || ' ' || TRIM(B.DES_TABLAS) AS NOMBRE, ");
                query.AppendLine("    ROUND( ");
                query.AppendLine("        CASE ");
                query.AppendLine("            WHEN D.FORMAVENTA = 'W' THEN ");
                query.AppendLine("                (A.PRECIO1 / A.NUMUNIDADES) * CASE WHEN A.CVE_IVA = 2 THEN 1.16 ELSE 1 END ");
                query.AppendLine("            ELSE ");
                query.AppendLine("                A.PRECIO1 * CASE WHEN A.CVE_IVA = 2 THEN 1.16 ELSE 1 END ");
                query.AppendLine("        END, 5 ");
                query.AppendLine("    ) AS PRECIO_FINAL ");
                query.AppendLine("FROM ARTICULOS A ");
                query.AppendLine("INNER JOIN TABLAS B ");
                query.AppendLine("    ON B.TIPO_TABLAS = 5 ");
                query.AppendLine("    AND B.CVE_TABLAS = A.CVE_MEDIDA ");
                query.AppendLine("INNER JOIN SUBCODIGOS_HIJO C ");
                query.AppendLine("    ON C.CVE_SUBCODIGO_PADRE = A.SUBCODIGO_ARTICULO ");
                query.AppendLine("INNER JOIN CODIGOBARRAS D ");
                query.AppendLine("    ON D.CODIGO_ARTICULO = A.CODIGO_ARTICULO ");
                query.AppendLine("    AND D.CVE_SUBCODIGO_PADRE = C.CVE_SUBCODIGO_PADRE ");
                query.AppendLine("    AND D.CVE_SUBCODIGO_HIJO = C.CVE_SUBCODIGO_HIJO ");
                query.AppendLine("WHERE A.ACTIVO = 'A' ");
                #endregion
                using (var oConnection = await _dbManager.GetConnection())
                using (var oCommand = new OracleCommand(query.ToString(), oConnection))
                {
                    if (oConnection.State == ConnectionState.Open)
                        await oConnection.CloseAsync();
                    await oConnection.OpenAsync();
                    using (var reader = await oCommand.ExecuteReaderAsync())
                    {
                        var obj = new ProductEntity();
                        while (await reader.ReadAsync())
                        {
                            var i = 0;
                            obj.CodigoProducto = reader[i] == DBNull.Value ? string.Empty : reader.GetString(i); i++;
                            obj.SubCodigoHijo = reader[i] == DBNull.Value ? string.Empty : reader.GetString(i); i++;
                            obj.SubCodigoPadre = reader[i] == DBNull.Value ? string.Empty : reader.GetString(i); i++;
                            obj.CodigoBarra = reader[i] == DBNull.Value ? string.Empty : reader.GetString(i); i++;
                            obj.FormaVentaCB = reader[i] == DBNull.Value ? string.Empty : reader.GetString(i); i++;
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
                    if (oConnection.State == ConnectionState.Open)
                        await oConnection.CloseAsync();
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
        public async Task<List<ProductEntity>> FindByText(string text, int page = 1, int sizePage = 50)
        {
            var datos = new List<ProductEntity>();
            try
            {
                if (page < 1) page = 1;
                var offset = (page - 1) * sizePage;
                #region querySql
                var query = new StringBuilder();
                query.AppendLine("SELECT A.CODIGO_ARTICULO,");
                query.AppendLine("       C.CVE_SUBCODIGO_HIJO,");
                query.AppendLine("       C.CVE_SUBCODIGO_PADRE,");
                query.AppendLine("       D.CODIGO_BARRAS,");
                query.AppendLine("       CASE WHEN TRIM(D.FORMAVENTA) = 'W' THEN 'W' ELSE 'C' END AS FORMAVENTA,");
                query.AppendLine("       A.DESCRIPCION || ' ' || TRIM(B.DES_TABLAS) AS NOMBRE,");
                query.AppendLine("       ROUND(");
                query.AppendLine("           (CASE WHEN TRIM(D.FORMAVENTA) = 'W'");
                query.AppendLine("                 THEN A.PRECIO1 / NULLIF(A.NUMUNIDADES, 0)");
                query.AppendLine("                 ELSE A.PRECIO1");
                query.AppendLine("            END)");
                query.AppendLine("           * (CASE WHEN A.CVE_IVA = 2 THEN 1.16 ELSE 1 END), 5) AS PRECIO_FINAL");
                query.AppendLine("FROM   ARTICULOS A");
                query.AppendLine("JOIN   SUBCODIGOS_HIJO C");
                query.AppendLine("       ON  C.CVE_SUBCODIGO_PADRE = A.SUBCODIGO_ARTICULO");
                query.AppendLine("JOIN   CODIGOBARRAS D");
                query.AppendLine("       ON  D.CODIGO_ARTICULO     = A.CODIGO_ARTICULO");
                query.AppendLine("       AND D.CVE_SUBCODIGO_PADRE = C.CVE_SUBCODIGO_PADRE");
                query.AppendLine("       AND D.CVE_SUBCODIGO_HIJO  = C.CVE_SUBCODIGO_HIJO");
                query.AppendLine("JOIN   TABLAS B");
                query.AppendLine("       ON  B.TIPO_TABLAS = 5");
                query.AppendLine("       AND B.CVE_TABLAS  = A.CVE_MEDIDA");
                query.AppendLine("WHERE  A.ACTIVO = 'A'");
                query.AppendLine("AND    INSTR(UPPER(A.CODIGO_ARTICULO || '|' || C.CVE_SUBCODIGO_HIJO || '|' ||");
                query.AppendLine("                   C.CVE_SUBCODIGO_PADRE || '|' || A.DESCRIPCION),");
                query.AppendLine("             :TEXT) > 0");
                query.AppendLine("ORDER BY A.CODIGO_ARTICULO, C.CVE_SUBCODIGO_HIJO, C.CVE_SUBCODIGO_PADRE");
                query.AppendLine("OFFSET :P_OFFSET ROWS FETCH NEXT :P_PAGE_SIZE ROWS ONLY");
                #endregion
                using (var oConnection = await _dbManager.GetConnection())
                using (var oCommand = new OracleCommand(query.ToString(), oConnection))
                {
                    oCommand.BindByName = true;
                    oCommand.Parameters.Add(new OracleParameter("TEXT", OracleDbType.Varchar2)).Value = text.Trim().ToUpperInvariant();
                    oCommand.Parameters.Add(new OracleParameter("P_OFFSET", OracleDbType.Int32)).Value = offset;
                    oCommand.Parameters.Add(new OracleParameter("P_PAGE_SIZE", OracleDbType.Int32)).Value = sizePage;
                    if (oConnection.State == ConnectionState.Open)
                        await oConnection.CloseAsync();
                    await oConnection.OpenAsync();
                    using (var reader = await oCommand.ExecuteReaderAsync())
                    {
                        var obj = new ProductEntity();
                        while (await reader.ReadAsync())
                        {
                            obj = new ProductEntity(); var i = 0;
                            obj.CodigoProducto = reader[i] == DBNull.Value ? string.Empty : reader.GetString(i); i++;
                            obj.SubCodigoHijo = reader[i] == DBNull.Value ? string.Empty : reader.GetString(i); i++;
                            obj.SubCodigoPadre = reader[i] == DBNull.Value ? string.Empty : reader.GetString(i); i++;
                            obj.CodigoBarra = reader[i] == DBNull.Value ? string.Empty : reader.GetString(i); i++;
                            obj.FormaVentaCB = reader[i] == DBNull.Value ? string.Empty : reader.GetString(i); i++;
                            obj.Descripcion = reader[i] == DBNull.Value ? string.Empty : reader.GetString(i); i++;
                            obj.Precio = reader[i] == DBNull.Value ? 0 : Convert.ToDouble(reader.GetString(i)); i++;
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
        public async Task<bool> InsertBarcode(BarcodeEntity entity, string usuario)
        {
            try
            {
                var query = new StringBuilder();
                #region querySql
                query.AppendLine("INSERT INTO CODIGOBARRAS (");
                query.AppendLine("    CODIGO_BARRAS,");
                query.AppendLine("    CODIGO_ARTICULO,");
                query.AppendLine("    FORMAVENTA,");
                query.AppendLine("    CVE_SUBCODIGO_PADRE,");
                query.AppendLine("    CVE_SUBCODIGO_HIJO,");
                query.AppendLine("    CREADOPOR,");
                query.AppendLine("    CREADOFECHA,");
                query.AppendLine("    MODIFICADOPOR,");
                query.AppendLine("    MODIFICADOFECHA");
                query.AppendLine(") VALUES (");
                query.AppendLine("    :p_codigo_barras,");
                query.AppendLine("    :p_codigo_articulo,");
                query.AppendLine("    :p_formaventa,");
                query.AppendLine("    :p_cve_subcodigo_padre,");
                query.AppendLine("    :p_cve_subcodigo_hijo,");
                query.AppendLine("    :p_creadopor,");
                query.AppendLine("    SYSDATE,");
                query.AppendLine("    NULL,");
                query.AppendLine("    NULL");
                query.AppendLine(")");
                #endregion
                using (var oConnection = await _dbManager.GetConnection())
                using (var cmd = new OracleCommand(query.ToString(), oConnection))
                {
                    cmd.BindByName = true;
                    #region paremeters sql
                    cmd.Parameters.Add("p_codigo_barras", OracleDbType.Varchar2).Value = entity.CodigoBarra;
                    cmd.Parameters.Add("p_codigo_articulo", OracleDbType.Varchar2).Value = entity.CodigoProducto;
                    cmd.Parameters.Add("p_formaventa", OracleDbType.Varchar2).Value = entity.FormaVentaCB;
                    cmd.Parameters.Add("p_cve_subcodigo_padre", OracleDbType.Varchar2).Value = entity.SubCodigoPadre;
                    cmd.Parameters.Add("p_cve_subcodigo_hijo", OracleDbType.Varchar2).Value = entity.SubCodigoHijo;
                    cmd.Parameters.Add("p_creadopor", OracleDbType.Varchar2).Value = usuario;
                    #endregion
                    if (oConnection.State == ConnectionState.Open)
                        await oConnection.CloseAsync();                    
                    await oConnection.OpenAsync();
                    var res = (await cmd.ExecuteNonQueryAsync()) > 0;
                    cmd.Dispose(); oConnection.Close(); query.Clear();
                    return res;
                }
            }
            catch (OracleException ex) when (ex.Number == 1)
            {
                throw new InvalidOperationException("El código de barras ya existe.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}