using BITecnored.Model.DataContract;
using BITecnored.Model.DB;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Web;

namespace BITecnored.Model
{
    public class Localidades_dbManager
    {
        public List<IdValue> GetAllLocalidades(int provincia)
        {
            List<IdValue> res = new List<IdValue>();

            string query = "SELECT id, lugar from lugares where cod_provincia = "+provincia;

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