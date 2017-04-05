using System.Data.Odbc;
using System.Diagnostics;

namespace BITecnored.Model.DB
{
    public class AbstractDB
    {
        protected OdbcConnection conn;
        protected string dns = "";

        public void Connect()
        {
            conn = new OdbcConnection();
            conn.ConnectionString = "DSN=" + GetDSN() + ";";
            conn.Open();
        }

        protected virtual string GetDSN() { return dns; }

        public OdbcDataReader ExecuteSQL(string sqlQuery)
        {
            OdbcCommand sql = new OdbcCommand(sqlQuery, conn);
            OdbcDataReader dato = sql.ExecuteReader();
            return dato;
        }

        public void Disconnect() { conn.Close(); }
    }
}