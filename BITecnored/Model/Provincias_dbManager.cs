using BITecnored.Model.DataContract;
using BITecnored.Model.DB;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Web;

namespace BITecnored.Model
{
    public class Provincias_dbManager
    {
        public List<IdValue> GetAllProvincias()
        {
            List<IdValue> res = new List<IdValue>();

            string query = "SELECT \"Provincias_Id\", \"Provincias_Nombre\" from \"Provincias\"";

            DBAgenda db = new DBAgenda();
            db.Connect();
            OdbcDataReader dr = db.ExecuteSQL(query);
            while (dr.Read())
            {
                if (!dr.IsDBNull(0) && !dr.IsDBNull(1))
                {
                    res.Add(new IdValue(dr.GetInt32(0), dr.GetString(1)));
                }
            }
            return res;
        }

        public List<IdValue> GetAllProvinciasSLA()
        {
            List<IdValue> res = new List<IdValue>();

            string query = "SELECT \"Provincias_Id\", \"Provincias_Nombre\" FROM \"Provincias\""
                +" WHERE \"Provincias_Id\"= 1"
                + " OR \"Provincias_Id\"= 2"
                + " OR \"Provincias_Id\"= 3"
                + " OR \"Provincias_Id\"= 4"
                + " OR \"Provincias_Id\"= 5"
                + " OR \"Provincias_Id\"= 11"
                ;

            DBAgenda db = new DBAgenda();
            db.Connect();
            OdbcDataReader dr = db.ExecuteSQL(query);
            while (dr.Read())
            {
                if (!dr.IsDBNull(0) && !dr.IsDBNull(1))
                {
                    res.Add(new IdValue(dr.GetInt32(0), dr.GetString(1)));
                }
            }
            return res;
        }
    }
}