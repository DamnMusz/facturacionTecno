using BITecnored.Model.DataContract;
using BITecnored.Model.DB;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Web;

namespace BITecnored.Model
{
    public class Aseguradoras_dbManager
    {
        string SQL_get_all = "SELECT \"Aseguradoras_Id\",\"Aseguradoras_Nombre\",\"Aseguradoras_Alias\" FROM \"Aseguradoras\" WHERE activa = true ORDER BY \"Aseguradoras_Nombre\"";

        public virtual string GetAseguradoras()
        {
            List<Aseguradora> res = new List<Aseguradora>();
            DBAgenda db = new DBAgenda();
            db.Connect();
            OdbcDataReader dr = db.ExecuteSQL(SQL_get_all);
            while (dr.Read())
            {
                res.Add(new Aseguradora(Int32.Parse(dr.GetString(0)), dr.GetString(1), dr.GetString(2)));
            }
            db.Disconnect();
            return Serializer.ToJSon<Aseguradora>(res);
        }
    }
}