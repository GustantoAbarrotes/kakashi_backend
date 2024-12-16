using Oracle.ManagedDataAccess.Client;
using RepositoryLayer.Connection;
using RepositoryLayer.Repositories.Supplier.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Repositories.Supplier
{
    public class SupplierRepository: ISupplierRepository
    {
        private readonly IDBManager _dbManager;
        public SupplierRepository(IDBManager dbManager)
        {
            _dbManager = dbManager;
        }
        public async Task<List<SupplierEntity>> GetAll(int cveProv, string rfc, string tradeName
            , string supplierName, string email)
        {
            var data = new List<SupplierEntity>();
            try
            {
                var query = new StringBuilder("SELECT * FROM PROVEDORES ");
                query.AppendLine("WHERE ACTIVO='A' AND (LENGTH(RFC) BETWEEN 12 AND 13) ");
                var filtersString = rfc + tradeName + supplierName + email;
                if (cveProv > 0 || !string.IsNullOrWhiteSpace(filtersString))
                    query.AppendLine("AND (");
                if (cveProv > 0)
                    query.AppendLine(" CVE_PROVEDOR=:CVE_PROVEDOR");
                if (!string.IsNullOrWhiteSpace(filtersString))
                {
                    if (!string.IsNullOrWhiteSpace(rfc))
                    {
                        if (cveProv > 0) query.Append(" OR");
                        query.AppendLine(" UPPER(RFC) IN(:RFC)");
                        var listRfc = rfc.Split(',');
                        var cadenaConcatenada = string.Join(",", listRfc.Select(e => $"\'{e.ToUpper()}\'"));
                        query.Replace(":RFC", cadenaConcatenada);
                    }
                    if (!string.IsNullOrWhiteSpace(tradeName))
                    {
                        if (cveProv > 0 || !string.IsNullOrWhiteSpace(rfc)) query.Append(" OR");
                        query.AppendLine("  UPPER(NOMBRECOMERCIAL) LIKE :NOMBRECOMERCIAL");
                    }
                    if (!string.IsNullOrWhiteSpace(supplierName))
                    {
                        if (cveProv > 0 || !string.IsNullOrWhiteSpace(rfc) || !string.IsNullOrWhiteSpace(tradeName))
                            query.Append(" OR");
                        query.AppendLine("  UPPER(NOMBRE) LIKE :NOMBRE");
                    }
                    if (!string.IsNullOrWhiteSpace(email))
                    {
                        if (cveProv > 0 || !string.IsNullOrWhiteSpace(rfc) || !string.IsNullOrWhiteSpace(tradeName) || !string.IsNullOrWhiteSpace(supplierName))
                            query.Append(" OR");
                        query.AppendLine(" UPPER(EMAIL) LIKE :EMAIL");
                    }
                }
                if (cveProv > 0 || !string.IsNullOrWhiteSpace(filtersString))
                    query.AppendLine(")");
                using (var oConn = await _dbManager.GetConnection())
                using (var cmd = new OracleCommand(query.ToString(), oConn))
                {
                    #region Oracle parameters
                    if (cveProv > 0)
                        cmd.Parameters.Add(new OracleParameter(":CVE_PROVEDOR", OracleDbType.Decimal)).Value = cveProv;
                    if (!string.IsNullOrWhiteSpace(filtersString))
                    {
                        if (!string.IsNullOrWhiteSpace(tradeName))
                            cmd.Parameters.Add(new OracleParameter(":NOMBRECOMERCIAL", OracleDbType.Varchar2)).Value = $"{tradeName.ToUpper()}%";
                        if (!string.IsNullOrWhiteSpace(supplierName))
                            cmd.Parameters.Add(new OracleParameter(":NOMBRE", OracleDbType.Varchar2)).Value = $"{supplierName.ToUpper()}%";
                        if (!string.IsNullOrWhiteSpace(email))
                            cmd.Parameters.Add(new OracleParameter(":EMAIL", OracleDbType.Varchar2)).Value = $"%{email.ToUpper()}%";
                    }
                    #endregion

                    await oConn.OpenAsync();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        var supplier = new SupplierEntity();
                        var i = 0;
                        while (await reader.ReadAsync())
                        {
                            supplier.cveProveedor = reader[i] == DBNull.Value ? 0 : (long)reader[i]; i++;
                            supplier.nombre = reader[i] == DBNull.Value ? string.Empty : (string)reader[i]; i++;
                            supplier.colonia = reader[i] == DBNull.Value ? string.Empty : (string)reader[i]; i++;
                            supplier.telefono = reader[i] == DBNull.Value ? string.Empty : (string)reader[i]; i++;
                            supplier.rfc = reader[i] == DBNull.Value ? string.Empty : (string)reader[i]; i++;

                            supplier.ciudad = reader[i] == DBNull.Value ? string.Empty : (string)reader[i]; i++;
                            supplier.diasCred = reader[i] == DBNull.Value ? 0 : (long)reader[i]; i++;
                            supplier.diasEnt = reader[i] == DBNull.Value ? 0 : (long)reader[i]; i++;
                            supplier.estado = reader[i] == DBNull.Value ? string.Empty : (string)reader[i]; i++;
                            supplier.nombreComercial = reader[i] == DBNull.Value ? string.Empty : (string)reader[i]; i++;

                            supplier.calle = reader[i] == DBNull.Value ? string.Empty : (string)reader[i]; i++;
                            supplier.numeroExterior = reader[i] == DBNull.Value ? string.Empty : (string)reader[i]; i++;
                            supplier.numeroInterior = reader[i] == DBNull.Value ? string.Empty : (string)reader[i]; i++;
                            supplier.referencia = reader[i] == DBNull.Value ? string.Empty : (string)reader[i]; i++;
                            supplier.municipio = reader[i] == DBNull.Value ? string.Empty : (string)reader[i]; i++;

                            supplier.pais = reader[i] == DBNull.Value ? string.Empty : (string)reader[i]; i++;
                            supplier.activo = reader[i] == DBNull.Value ? string.Empty : (string)reader[i]; i++;
                            supplier.codigoPostal = reader[i] == DBNull.Value ? 0 : (long)reader[i]; i++;
                            supplier.email = reader[i] == DBNull.Value ? string.Empty : (string)reader[i]; i++;
                            supplier.telefonoContacto = reader[i] == DBNull.Value ? string.Empty : (string)reader[i]; i++;
                            supplier.emailContacto = reader[i] == DBNull.Value ? string.Empty : (string)reader[i]; i++;

                            supplier.creadoPor = reader[i] == DBNull.Value ? string.Empty : (string)reader[i]; i++;
                            supplier.creadoFecha = reader[i] == DBNull.Value ? DateTime.MinValue : (DateTime)reader[i]; i++;
                            supplier.modificadoPor = reader[i] == DBNull.Value ? string.Empty : (string)reader[i]; i++;
                            supplier.modificadoFecha = reader[i] == DBNull.Value ? DateTime.MinValue : (DateTime)reader[i]; i++;

                            supplier.informacionAdicional = reader[i] == DBNull.Value ? string.Empty : (string)reader[i]; i++;
                            supplier.calcFechaVenc = reader[i] == DBNull.Value ? 0 : (int)reader[i]; i++;
                            supplier.nombreContacto = reader[i] == DBNull.Value ? string.Empty : (string)reader[i]; i++;
                            supplier.tipoProv = reader[i] == DBNull.Value ? 0 : (short)reader[i];
                            data.Add(supplier); supplier = new SupplierEntity(); i = 0;
                        }
                        await oConn.CloseAsync(); await reader.CloseAsync(); query.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                data = null;
                throw ex;
            }
            return data;
        }
        public async Task<List<string>> GetComboBox(int cveProv, string rfc, string tradeName
            , string supplierName, string email)
        {
            var data = new List<string>();
            try
            {
                #region Query SQL
                var query = new StringBuilder();
                query.AppendLine("SELECT CVE_PROVEDOR, NOMBRE, NOMBRECOMERCIAL,RFC, EMAIL ");
                query.AppendLine("FROM PROVEDORES ");
                query.AppendLine("WHERE ROWNUM <= 20 AND ACTIVO='A' AND (LENGTH(RFC) BETWEEN 12 AND 13) ");
                var filtersString = rfc + tradeName + supplierName + email;
                if (cveProv > 0 || !string.IsNullOrWhiteSpace(filtersString))
                    query.AppendLine("AND (");
                if (cveProv > 0)
                    query.AppendLine(" CVE_PROVEDOR LIKE :CVE_PROVEDOR");
                if (!string.IsNullOrWhiteSpace(filtersString))
                {
                    if (!string.IsNullOrWhiteSpace(rfc))
                    {
                        if (cveProv > 0) query.Append(" OR");
                        query.AppendLine(" UPPER(RFC) LIKE :RFC");
                    }
                    if (!string.IsNullOrWhiteSpace(tradeName))
                    {
                        if (cveProv > 0 || !string.IsNullOrWhiteSpace(rfc)) query.Append(" OR");
                        query.AppendLine(" UPPER(NOMBRECOMERCIAL) LIKE :NOMBRECOMERCIAL");
                    }
                    if (!string.IsNullOrWhiteSpace(supplierName))
                    {
                        if (cveProv > 0 || !string.IsNullOrWhiteSpace(rfc) || !string.IsNullOrWhiteSpace(tradeName))
                            query.Append(" OR");
                        query.AppendLine(" UPPER(NOMBRE) LIKE :NOMBRE");
                    }
                    if (!string.IsNullOrWhiteSpace(email))
                    {
                        if (cveProv > 0 || !string.IsNullOrWhiteSpace(rfc) || !string.IsNullOrWhiteSpace(tradeName)
                            || !string.IsNullOrWhiteSpace(supplierName))
                            query.Append(" OR");
                        query.AppendLine(" UPPER(EMAIL) LIKE :EMAIL");
                    }
                }
                if (cveProv > 0 || !string.IsNullOrWhiteSpace(filtersString))
                    query.AppendLine(")");
                #endregion

                using (var oConn = await _dbManager.GetConnection())
                using (var cmd = new OracleCommand(query.ToString(), oConn))
                {
                    #region Oracle parameters
                    if (cveProv > 0)
                        cmd.Parameters.Add(new OracleParameter(":CVE_PROVEDOR", OracleDbType.Varchar2)).Value = $"%{cveProv}%";
                    if (!string.IsNullOrWhiteSpace(filtersString))
                    {
                        if (!string.IsNullOrWhiteSpace(rfc))
                            cmd.Parameters.Add(new OracleParameter(":RFC", OracleDbType.Varchar2)).Value = $"%{rfc.ToUpper()}%";
                        if (!string.IsNullOrWhiteSpace(tradeName))
                            cmd.Parameters.Add(new OracleParameter(":NOMBRECOMERCIAL", OracleDbType.Varchar2)).Value = $"%{tradeName.ToUpper()}%";
                        if (!string.IsNullOrWhiteSpace(supplierName))
                            cmd.Parameters.Add(new OracleParameter(":NOMBRE", OracleDbType.Varchar2)).Value = $"%{supplierName.ToUpper()}%";
                        if (!string.IsNullOrWhiteSpace(email))
                            cmd.Parameters.Add(new OracleParameter(":EMAIL", OracleDbType.Varchar2)).Value = $"%{email.ToUpper()}%";
                    }
                    #endregion

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        var suppliers = new List<SupplierEntity>();
                        var supplier = new SupplierEntity();
                        var i = 0;
                        while (await reader.ReadAsync())
                        {
                            supplier.cveProveedor = reader[i] == DBNull.Value ? 0 : (long)reader[i]; i++;
                            supplier.nombre = reader[i] == DBNull.Value ? string.Empty : (string)reader[i]; i++;
                            supplier.nombreComercial = reader[i] == DBNull.Value ? string.Empty : (string)reader[i]; i++;
                            supplier.rfc = reader[i] == DBNull.Value ? string.Empty : (string)reader[i]; i++;
                            supplier.email = reader[i] == DBNull.Value ? string.Empty : (string)reader[i]; i++;

                            suppliers.Add(supplier); supplier = new SupplierEntity(); i = 0;
                        }
                        data = suppliers.Select(e =>
                        {

                            if (cveProv > 0)
                                return $"{e.cveProveedor}";
                            if (!string.IsNullOrWhiteSpace(rfc))
                                return e.rfc;
                            if (!string.IsNullOrWhiteSpace(tradeName))
                                return e.nombreComercial;
                            if (!string.IsNullOrWhiteSpace(supplierName))
                                return e.nombre;
                            if (!string.IsNullOrWhiteSpace(email))
                                return e.email;
                            return null;
                        }).OrderBy(e => e).ToList();
                        data = data.FirstOrDefault(e => !string.IsNullOrWhiteSpace(e)) == null ? new List<string>() : data;
                        await oConn.CloseAsync(); await reader.CloseAsync(); query.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                data = null;
                throw ex;
            }
            return data;
        }
        public async Task<List<SupplierEntity>> ApproveBalance(int keyBranch, int keyCustomer, int cveProv
           , string filtersString, string tradeName, string supplierName, string email)
        {
            var data = new List<SupplierEntity>();
            try
            {
                var query = new StringBuilder("SELECT * FROM PROVEDORES ");
                query.AppendLine("WHERE ACTIVO='A' AND (LENGTH(RFC) BETWEEN 12 AND 13) ");

                using (var oConn = await _dbManager.GetConnection())
                using (var cmd = new OracleCommand(query.ToString(), oConn))
                {
                    #region Oracle parameters
                    if (cveProv > 0)
                        cmd.Parameters.Add(new OracleParameter(":CVE_PROVEDOR", OracleDbType.Decimal)).Value = cveProv;
                    if (!string.IsNullOrWhiteSpace(filtersString))
                    {
                        if (!string.IsNullOrWhiteSpace(tradeName))
                            cmd.Parameters.Add(new OracleParameter(":NOMBRECOMERCIAL", OracleDbType.Varchar2)).Value = $"{tradeName.ToUpper()}%";
                        if (!string.IsNullOrWhiteSpace(supplierName))
                            cmd.Parameters.Add(new OracleParameter(":NOMBRE", OracleDbType.Varchar2)).Value = $"{supplierName.ToUpper()}%";
                        if (!string.IsNullOrWhiteSpace(email))
                            cmd.Parameters.Add(new OracleParameter(":EMAIL", OracleDbType.Varchar2)).Value = $"%{email.ToUpper()}%";
                    }
                    #endregion
                    await oConn.OpenAsync();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        var supplier = new SupplierEntity();
                        var i = 0;
                        while (await reader.ReadAsync())
                        {
                            supplier.cveProveedor = reader[i] == DBNull.Value ? 0 : (Int64)reader[i]; i++;
                            supplier.nombre = reader[i] == DBNull.Value ? string.Empty : (string)reader[i]; i++;
                            supplier.colonia = reader[i] == DBNull.Value ? string.Empty : (string)reader[i]; i++;
                            supplier.telefono = reader[i] == DBNull.Value ? string.Empty : (string)reader[i]; i++;
                            supplier.rfc = reader[i] == DBNull.Value ? string.Empty : (string)reader[i]; i++;

                            supplier.ciudad = reader[i] == DBNull.Value ? string.Empty : (string)reader[i]; i++;
                            supplier.diasCred = reader[i] == DBNull.Value ? 0 : (Int64)reader[i]; i++;
                            supplier.diasEnt = reader[i] == DBNull.Value ? 0 : (Int64)reader[i]; i++;
                            supplier.estado = reader[i] == DBNull.Value ? string.Empty : (string)reader[i]; i++;
                            supplier.nombreComercial = reader[i] == DBNull.Value ? string.Empty : (string)reader[i]; i++;

                            supplier.calle = reader[i] == DBNull.Value ? string.Empty : (string)reader[i]; i++;
                            supplier.numeroExterior = reader[i] == DBNull.Value ? string.Empty : (string)reader[i]; i++;
                            supplier.numeroInterior = reader[i] == DBNull.Value ? string.Empty : (string)reader[i]; i++;
                            supplier.referencia = reader[i] == DBNull.Value ? string.Empty : (string)reader[i]; i++;
                            supplier.municipio = reader[i] == DBNull.Value ? string.Empty : (string)reader[i]; i++;

                            supplier.pais = reader[i] == DBNull.Value ? string.Empty : (string)reader[i]; i++;
                            supplier.activo = reader[i] == DBNull.Value ? string.Empty : (string)reader[i]; i++;
                            supplier.codigoPostal = reader[i] == DBNull.Value ? 0 : (Int64)reader[i]; i++;
                            supplier.email = reader[i] == DBNull.Value ? string.Empty : (string)reader[i]; i++;
                            supplier.telefonoContacto = reader[i] == DBNull.Value ? string.Empty : (string)reader[i]; i++;
                            supplier.emailContacto = reader[i] == DBNull.Value ? string.Empty : (string)reader[i]; i++;

                            supplier.creadoPor = reader[i] == DBNull.Value ? string.Empty : (string)reader[i]; i++;
                            supplier.creadoFecha = reader[i] == DBNull.Value ? DateTime.MinValue : (DateTime)reader[i]; i++;
                            supplier.modificadoPor = reader[i] == DBNull.Value ? string.Empty : (string)reader[i]; i++;
                            supplier.modificadoFecha = reader[i] == DBNull.Value ? DateTime.MinValue : (DateTime)reader[i]; i++;

                            supplier.informacionAdicional = reader[i] == DBNull.Value ? string.Empty : (string)reader[i]; i++;
                            supplier.calcFechaVenc = reader[i] == DBNull.Value ? 0 : (Int32)reader[i]; i++;
                            supplier.nombreContacto = reader[i] == DBNull.Value ? string.Empty : (string)reader[i]; i++;
                            supplier.tipoProv = reader[i] == DBNull.Value ? 0 : (Int16)reader[i];
                            data.Add(supplier); supplier = new SupplierEntity(); i = 0;
                        }
                        await oConn.CloseAsync(); await reader.CloseAsync(); query.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                data = null;
                throw ex;
            }
            return data;
        }

    }
}