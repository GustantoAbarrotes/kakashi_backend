using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using static RepositoryLayer.Utils.Consts;

namespace RepositoryLayer.Connection
{
    internal class DBManager : IDBManager
    {
        private string connectionStr = new string("Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=@PROTOCOL) (HOST=@HOST) (PORT=@PORT))(CONNECT_DATA=(SERVICE_NAME=@SERVICENAME)));Persist Security Info=@SECURITY; User ID=@USERID;Password=@PASSWORD;");
        private Dictionary<string, string> dataConn = new Dictionary<string, string>() {
                { "PROTOCOL", ""}, { "HOST", ""},{ "PORT", ""},{ "SERVICENAME_SID", ""},
                { "SERVICENAME_SID2", ""},{ "SECURITY", ""},{ "USERNAME", ""}, { "PASSWORD", ""}};
        private Dictionary<string, string> dataConnSecondary = new Dictionary<string, string>() {
                { "PROTOCOL", ""}, { "HOST", ""},{ "PORT", ""},{ "SERVICENAME_SID", ""},
                { "SERVICENAME_SID2", ""},{ "SECURITY", ""},{ "USERNAME", ""}, { "PASSWORD", ""}};
        private InfoConnection info;
        private readonly OracleConnection _connection;
        private readonly OracleConnection _connectionSec;
        public DBManager()
        {
            _connection = new OracleConnection();
            _connectionSec = new OracleConnection();
            BuildDataConnection(TypeConn.Local);
        }

        public async Task<OracleConnection> GetConnection()
        {
            if (string.IsNullOrEmpty(_connection.ConnectionString))
                _connection.ConnectionString = GetConnectionString();
            if (_connection.State == ConnectionState.Open) await _connection.CloseAsync();

            return _connection;
        }
        public async Task<OracleConnection> GetConnectionSec()
        {
            if (string.IsNullOrEmpty(_connectionSec.ConnectionString))
                _connectionSec.ConnectionString = GetConnectionStringSec();
            if (_connectionSec.State == ConnectionState.Open) await _connectionSec.CloseAsync();

            return _connectionSec;
        }
        public void BuildDataConnection(TypeConn tipo)
        {
            try
            {
                var xDoc = GetDocumentConnection(tipo);
                using (var tagPadre = xDoc.GetElementsByTagName("DatosConexion"))
                    if (tagPadre != null && tagPadre.Count > 0)
                    {
                        XmlNode nodos = tagPadre[0];
                        dataConn["PROTOCOL"] = nodos["Protocol"].InnerText;
                        dataConn["HOST"] = nodos["Host"].InnerText;
                        dataConn["PORT"] = nodos["Port"].InnerText;
                        dataConn["SERVICENAME_SID"] = nodos["ServiceName_SID"].InnerText;
                        //dataConn["SERVICENAME_SID2"] = nodos["ServiceName_SID2"].InnerText;
                        dataConn["SECURITY"] = nodos["Security"].InnerText;
                        dataConn["USERNAME"] = nodos["UserName"].InnerText;
                        dataConn["PASSWORD"] = nodos["Password"].InnerText;
                    }
                info = new InfoConnection(dataConn["HOST"], tipo);
            }
            catch (Exception exc)
            {
                string msgE = "Error al obtener cadena de coneccion" + exc.Message.ToString();
                throw exc;
            }
        }
        public void BuildDataConnectionSec(TypeConn tipo)
        {
            try
            {
                var xDoc = GetDocumentConnection(tipo);
                using (var tagPadre = xDoc.GetElementsByTagName("DatosConexion"))
                    if (tagPadre != null && tagPadre.Count > 0)
                    {
                        XmlNode nodos = tagPadre[0];
                        dataConnSecondary["PROTOCOL"] = nodos["Protocol"].InnerText;
                        dataConnSecondary["HOST"] = nodos["Host"].InnerText;
                        dataConnSecondary["PORT"] = nodos["Port"].InnerText;
                        dataConnSecondary["SERVICENAME_SID"] = nodos["ServiceName_SID"].InnerText;
                        //dataConn["SERVICENAME_SID2"] = nodos["ServiceName_SID2"].InnerText;
                        dataConnSecondary["SECURITY"] = nodos["Security"].InnerText;
                        dataConnSecondary["USERNAME"] = nodos["UserName"].InnerText;
                        dataConnSecondary["PASSWORD"] = nodos["Password"].InnerText;
                    }
            }
            catch (Exception exc)
            {
                string msgE = "Error al obtener cadena de coneccion" + exc.Message.ToString();
                throw exc;
            }
        }
        private string GetConnectionString()
        {
            var connString = new StringBuilder(connectionStr);
            connString.Replace("@PROTOCOL", dataConn["PROTOCOL"]);
            connString.Replace("@HOST", dataConn["HOST"]);
            connString.Replace("@PORT", dataConn["PORT"]);
            connString.Replace("@SERVICENAME", dataConn["SERVICENAME_SID"]);
            connString.Replace("@SECURITY", dataConn["SECURITY"]);
            connString.Replace("@USERID", dataConn["USERNAME"]);
            connString.Replace("@PASSWORD", dataConn["PASSWORD"].DesEncriptar());
            return connString.ToString();
        }
        public Dictionary<string, string> GetInfo() => new Dictionary<string, string>() { { "host", info.host }, { "type", info.type.ToString() } };
        private string GetConnectionStringSec()
        {
            var connString = new StringBuilder(connectionStr);
            connString.Replace("@PROTOCOL", dataConnSecondary["PROTOCOL"]);
            connString.Replace("@HOST", dataConnSecondary["HOST"]);
            connString.Replace("@PORT", dataConnSecondary["PORT"]);
            connString.Replace("@SERVICENAME", dataConnSecondary["SERVICENAME_SID"]);
            connString.Replace("@SECURITY", dataConnSecondary["SECURITY"]);
            connString.Replace("@USERID", dataConnSecondary["USERNAME"]);
            connString.Replace("@PASSWORD", dataConnSecondary["PASSWORD"].DesEncriptar());
            return connString.ToString();
        }
        private XmlDocument GetDocumentConnection(TypeConn tipo)
        {
            var xDoc = new XmlDocument();
            try
            {
                switch (tipo)
                {
                    case TypeConn.Local:
                        xDoc.Load(@"C:\SiacWin2\configIni.xml");
                        break;
                    case TypeConn.Dss:
                        xDoc.Load(@"C:\SiacWin2\configIni2.8.xml");
                        break;
                    case TypeConn.Central:
                        xDoc.Load(@"C:\SiacWin2\ConfigIni2.5.xml");
                        break;
                    case TypeConn.Pruebas:
                        xDoc.Load(@"C:\SiacWin2\ConfigIni2.34.xml");
                        break;
                    case TypeConn.Cedis:
                        xDoc.Load(@"C:\SiacWin2\ConfigIni2.6.xml");
                        break;
                    default:
                        break;
                }
            }
            catch { }

            return xDoc;
        }
        private class InfoConnection
        {
            public InfoConnection(string host, TypeConn type)
            {
                this.host = host;
                this.type = type;
            }

            public string host { get; set; }
            public TypeConn type { get; set; }
        }
    }
}