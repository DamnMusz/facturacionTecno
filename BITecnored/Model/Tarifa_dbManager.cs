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
    public class Tarifa_dbManager
    {
        public long CantidadTotal()
        {
            long cantidad = -1;
            DBAgenda db = new DBAgenda();
            db.Connect();
            OdbcDataReader dr = db.ExecuteSQL("select count(*) from tarifas_centros");
            while (dr.Read())
            {
                cantidad = dr.GetInt64(0);
            }
            db.Disconnect();
            return cantidad;
        }

        public bool Existe(DateTime periodo, string id_centro)
        {
            long cantidad = -1;
            DBAgenda db = new DBAgenda();
            db.Connect();
            OdbcDataReader dr = db.ExecuteSQL("select count(*) from tarifas_centros where periodo = " + periodo.Year.ToString("D"+4)+ periodo.Month.ToString("D" + 2) + " and id_centro = " + id_centro);
            while (dr.Read())
            {
                cantidad = dr.GetInt64(0);
            }
            db.Disconnect();
            return (cantidad > 0);
        }

        public long Contar(int id_centro)
        {
            long cantidad = 0;
            string query = "select count(*) from tarifas_centros where id_centro = " + id_centro;

            DBAgenda db = new DBAgenda();
            db.Connect();
            OdbcDataReader dr = db.ExecuteSQL(query);
            while (dr.Read())
            {
                if (!dr.IsDBNull(0))
                {
                    cantidad = dr.GetInt64(0);
                }
            }
            db.Disconnect();
            return cantidad;
    }

        public List<Tarifa> Buscar(int id_centro, int pagina, int resultados_por_pagina)
        {
            List<Tarifa> tarifas = new List<Tarifa>();

            DBAgenda db = new DBAgenda();
            db.Connect();
            string sql = "select id, periodo, tarifa_por_inspeccion, fecha_creacion, observacion, usuario_creacion from tarifas_centros where id_centro = " + id_centro
                + " ORDER BY periodo DESC limit " + resultados_por_pagina
                + " offset " + (pagina - 1) * resultados_por_pagina;

            Debug.WriteLine(sql);

            OdbcDataReader dr = db.ExecuteSQL(sql);
            
            while (dr.Read())
            {
                Tarifa.TarifaBuilder tarifa =
                    Tarifa.GetBuilder()
                    .id(""+dr.GetInt32(0))
                    .periodo_desde("" + dr.GetInt32(1))
                    .monto_por_ip("" + dr.GetInt32(2))
                    .fecha_creacion("" + dr.GetDateTime(3).ToString("dd/MM/yyyy"))
                    .usuario_creacion("" + dr.GetString(5))
                    ;

                if(!dr.IsDBNull(4))
                {
                    tarifa.observacion("" + dr.GetString(4));
                } else
                {
                    tarifa.observacion("");
                }
                tarifas.Add(tarifa.build());
            }
            db.Disconnect();
            return tarifas;
        }

        private bool existeTarifaEnTabla(TarifaNueva tarifa)
        {
            return Existe(tarifa.getFechaDesde(), tarifa.id_centro);
        }

        private bool anioTarifaMayorAlActual(TarifaNueva tarifa)
        {
            return (tarifa.getFechaDesde().Year > tarifa.fecha_creacion.Year);
        }

        private bool anioTarifaIgualAlActual(TarifaNueva tarifa)
        {
            return (tarifa.getFechaDesde().Year == tarifa.fecha_creacion.Year);
        }

        private bool mesTarifaMayorAlActual(TarifaNueva tarifa)
        {
            return (tarifa.getFechaDesde().Month > tarifa.fecha_creacion.Month);
        }

        public void Crear(TarifaNuevaBase tarifa)
        {
            string cantidad = "(select count(*) from tarifas_centros) ";
            string sql = "insert into tarifas_centros (id, nombre, color_vista, periodo_desde, periodo_hasta, tarifa_por_inspeccion, fecha_creacion, usuario_creacion" + ((tarifa.descripcion != null && tarifa.descripcion.Length > 0) ? (", observacion") : "") + ") "
                        + "values (" + cantidad + ",'" + tarifa.titulo + "','" + tarifa.color + "'," + tarifa.periodo_desde
                         + "," + tarifa.periodo_hasta
                         + "," + tarifa.monto + ",'" + tarifa.fecha_creacion + "','" + tarifa.usuario_creacion + "'" + ((tarifa.descripcion != null && tarifa.descripcion.Length > 0) ? (",'" + tarifa.descripcion + "'") : "") + ")";

            DBAgenda db = new DBAgenda();
            db.Connect();
            OdbcDataReader dr = db.ExecuteSQL(sql);
            db.Disconnect();
        }

        [Obsolete]
        public List<TarifaNueva> Crear(TarifaNueva tarifas_agrupadas)
        {
            List<TarifaNueva> tarifas = tarifas_agrupadas.descomponerEnPeriodos();
            List<TarifaNueva> rechazadas = new List<TarifaNueva>();

            foreach (TarifaNueva tarifa in tarifas)
            {
                if (!existeTarifaEnTabla(tarifa)) 
                {
                    string cantidad = "(select count(*) from tarifas_centros) ";

                    DateTime fecha_desde = tarifa.getFechaDesde();
                    DateTime fecha_hasta = tarifa.getFechaHasta();

                    DBAgenda db = new DBAgenda();
                    db.Connect();
                    string sql = "insert into tarifas_centros (id, id_centro, periodo, tarifa_por_inspeccion, fecha_creacion, usuario_creacion"+ ((tarifa.observacion != null && tarifa.observacion.Length > 0) ? (", observacion") : "") +") "
                        + "values (" + cantidad + "," + tarifa.id_centro + "," + fecha_desde.Year.ToString("D" + 4) + fecha_desde.Month.ToString("D" + 2)
                         + "," + tarifa.monto_por_ip + ",'" + tarifa.fecha_creacion + "','" + tarifa.usuario_creacion + "'"+ ((tarifa.observacion!=null && tarifa.observacion.Length>0) ? (",'" + tarifa.observacion + "'") : "") +")";

                    OdbcDataReader dr = db.ExecuteSQL(sql);
                    db.Disconnect();
                }
                else
                {
                    if (anioTarifaMayorAlActual(tarifa) || (anioTarifaIgualAlActual(tarifa) && mesTarifaMayorAlActual(tarifa)))
                    {
                        DateTime fecha_desde = tarifa.getFechaDesde();
                        DateTime fecha_hasta = tarifa.getFechaHasta();

                        string sql = "update tarifas_centros set "+
                            " "
                            +"tarifa_por_inspeccion = " + tarifa.monto_por_ip
                            + ", fecha_creacion = '" + tarifa.fecha_creacion + "'"
                            + ", usuario_creacion = '" + tarifa.usuario_creacion + "'"
                            + ((tarifa.observacion != null && tarifa.observacion.Length > 0) ? (", observacion = '" + tarifa.observacion + "'") : "") 
                            + " "
                            + " where id_centro = " + tarifa.id_centro
                            + " and periodo = " + fecha_desde.Year.ToString("D" + 4) + fecha_desde.Month.ToString("D" + 2)
                         ;
                        DBAgenda db = new DBAgenda();
                        db.Connect();
                        Debug.WriteLine(sql);
                        OdbcDataReader dr = db.ExecuteSQL(sql);
                        db.Disconnect();
                    }
                    else {
                        rechazadas.Add(tarifa);
                    }
                }
            }
            return rechazadas;
        }
    }
}