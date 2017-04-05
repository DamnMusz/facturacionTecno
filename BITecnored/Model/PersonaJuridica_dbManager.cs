using BITecnored.Model.DataContract;
using BITecnored.Model.DB;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace BITecnored.Model
{
    public class PersonaJuridica_dbManager
    {
        public bool Existe(long cuit)
        {
            long cantidad = -1;
            DBAgenda db = new DBAgenda();
            db.Connect();
            OdbcDataReader dr = db.ExecuteSQL("select count(*) from persona_juridica where cuit = " + cuit + "");
            while (dr.Read())
            {
                cantidad = dr.GetInt64(0);
            }
            db.Disconnect();
            return (cantidad > 0);
        }
        
        public void Crear(CentroFacturable centro)
        {
            if (!Existe(centro.id))
            {
                List<string> attrNames = centro.GetAttrNamesPersonaJuridica();
                List<string> attrValues = centro.GetAttrValuesPersonaJuridica();
                string sql_persona_juridica = "INSERT INTO persona_juridica (";
                if (attrNames.Count == attrValues.Count)
                {
                    for (int i = 0; i < attrNames.Count; ++i)
                    {
                        sql_persona_juridica += attrNames[i];
                        if (i == (attrNames.Count - 1))
                            sql_persona_juridica += ")";
                        else
                            sql_persona_juridica += ", ";
                    }
                   
                    sql_persona_juridica += " VALUES (";

                    for (int i = 0; i < attrValues.Count; ++i)
                    {
                        sql_persona_juridica += attrValues[i];
                        if (i == (attrValues.Count - 1))
                            sql_persona_juridica += ")";
                        else
                            sql_persona_juridica += ", ";
                    }
                }

                DBAgenda db = new DBAgenda();
                db.Connect();

                OdbcDataReader dr = db.ExecuteSQL(sql_persona_juridica);
                db.Disconnect();
            }
        }

        public void Update(CentroFacturable centro)
        {
            List<string> attrNames = centro.GetAttrNamesPersonaJuridica();
            List<string> attrValues = centro.GetAttrValuesPersonaJuridica();
            string sql_persona_juridica = "UPDATE persona_juridica SET ";
            if (attrNames.Count == attrValues.Count)
                for (int i = 0; i < attrNames.Count; ++i)
                {
                    sql_persona_juridica += attrNames[i] + "=" + attrValues[i];
                    if (i == (attrValues.Count - 1))
                        sql_persona_juridica += " ";
                    else
                        sql_persona_juridica += ", ";
                }
            sql_persona_juridica += "WHERE cuit = " + centro.cuit;

            DBAgenda db = new DBAgenda();
            db.Connect();

            OdbcDataReader dr = db.ExecuteSQL(sql_persona_juridica);
            db.Disconnect();
        }
    }
}