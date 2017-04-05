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
    public class AfinidadTarifaria_dbManager
    {
        public virtual List<IdValue> Buscar()
        {
            List<IdValue> afinidades = new List<IdValue>();

            DBAgenda db = new DBAgenda();
            db.Connect();
            OdbcDataReader dr = db.ExecuteSQL("select id, descripcion from afinidad_tarifaria where activa = true");
            while (dr.Read())
            {
                afinidades.Add(new IdValue(Int32.Parse(dr.GetString(0)), dr.GetString(1)));
            }
            db.Disconnect();
            return afinidades;
        }

        public long CantidadTotal()
        {
            long cantidad = -1;
            DBAgenda db = new DBAgenda();
            db.Connect();
            OdbcDataReader dr = db.ExecuteSQL("select count(*) from afinidad_tarifaria");
            while (dr.Read())
            {
                cantidad = dr.GetInt64(0);
            }
            db.Disconnect();
            return cantidad;
        }

        public bool Existe(string afinidad)
        {
            long cantidad = -1;
            DBAgenda db = new DBAgenda();
            db.Connect();
            OdbcDataReader dr = db.ExecuteSQL("select count(*) from afinidad_tarifaria where descripcion = '" + afinidad + "'");
            while (dr.Read())
            {
                cantidad = dr.GetInt64(0);
            }
            db.Disconnect();
            return (cantidad > 0);
        }

        public bool EsUsada(int id_afinidad)
        {
            long cantidad = -1;
            DBAgenda db = new DBAgenda();
            db.Connect();
            OdbcDataReader dr = db.ExecuteSQL("select count(*) from \"Centros\" where id_afinidad_tarifaria =" + id_afinidad);
            while (dr.Read())
            {
                cantidad = dr.GetInt64(0);
            }
            db.Disconnect();
            return (cantidad > 0);
        }

        public virtual bool Eliminar(int id)
        {
            if (!EsUsada(id))
            {
                long cantidad = CantidadTotal();

                DBAgenda db = new DBAgenda();
                db.Connect();
                string sql = "delete from afinidad_tarifaria where id = " + id;
                OdbcDataReader dr = db.ExecuteSQL(sql);
                db.Disconnect();
                return true;
            }
            else
            {
                return false;
            }
        }

        public virtual void Crear(string nombre)
        {
            if (!Existe(nombre))
            {
                long cantidad = CantidadTotal();

                DBAgenda db = new DBAgenda();
                db.Connect();
                string sql = "insert into afinidad_tarifaria (id, descripcion, activa) values (" + cantidad + ",\'" + nombre + "\',true)";
                OdbcDataReader dr = db.ExecuteSQL(sql);
                db.Disconnect();
            }
        }
    }
}