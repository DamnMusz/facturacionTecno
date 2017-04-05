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
    public class Centros_dbManager
    {
        private static string criterios_centro_incompleto = ""
            + "(	(activo_facturacion = TRUE and ("
                + " propio IS NULL or "
                + " id_persona_juridica IS NULL or "
                + " nombre_fantasia IS NULL or "
                + " cuit IS NULL or"
                + " razon_social IS NULL or"
                + " id_provincia_legal IS NULL or"
                + " id_localidad_legal IS NULL or"
                + " direccion_legal_calle IS NULL or"
                + " direccion_legal_numero IS NULL or"
                + " tipo_factura IS NULL "
                + " )) or activo_facturacion IS NULL"
                + " ) ";

        private static string criterios_centro_tarifa_incompleta = " ( activo_facturacion = TRUE and tc.periodo IS NULL ) ";

        private static string from_centros_join_persona_juridica_afinidad_tarifaria_tarifas_centros = ""
            + " from \"Centros\" centros"
            + " left join persona_juridica pj on centros.id_persona_juridica = pj.cuit "
            + " left join afinidad_tarifaria at on centros.id_afinidad_tarifaria = at.id ";

        private static string from_centros_join_persona_juridica_afinidad_tarifaria_provincias_lugares_tarifas_centros = ""
            + " from \"Centros\" centros"
            + " left join persona_juridica pj on centros.id_persona_juridica = pj.cuit "
            + " left join afinidad_tarifaria at on centros.id_afinidad_tarifaria = at.id"
            + " left join \"Provincias\" prov on pj.id_provincia_legal = prov.\"Provincias_Id\""
            + " left join lugares lug on pj.id_localidad_legal = lug.id ";

        //                                                0               1           2           3           4              5                6   
        private static string basic_query = "SELECT \"Centros_Id\", nombre_fantasia, cuit, razon_social, propio, id_provincia_legal, id_localidad_legal, "
            //              7                       8                   9               10          11              12                  13           14         15
            + "direccion_legal_calle, direccion_legal_numero, activo_facturacion, at.id, at.descripcion, \"Centros_Nombre\", \"Provincias_Nombre\", lugar, tipo_factura "

            + from_centros_join_persona_juridica_afinidad_tarifaria_provincias_lugares_tarifas_centros;

        public List<CentroFacturable> GetAllCentros(int pagina, int resultados_por_pagina, bool activos)
        {
            string query = basic_query

                + ((activos)? " where estado = true ":" where true ")
                
                +"ORDER BY \"Centros_Id\" DESC limit " + resultados_por_pagina

                + " offset " + (pagina - 1) * resultados_por_pagina;
            List<CentroFacturable> res = new List<CentroFacturable>();
            DBAgenda db = new DBAgenda();
            db.Connect();
            OdbcDataReader dr = db.ExecuteSQL(query);
            while (dr.Read())
            {
                if (!dr.IsDBNull(0))
                {
                    CentroFacturable.CentroFacturableBuilder centroBuilder = CentroFacturable.getBuilder()
                        .id(dr.GetInt32(0))
                        .nombre_fantasia((dr.IsDBNull(1) ? "" : dr.GetString(1)))
                        .cuit((dr.IsDBNull(2) ? -1 : dr.GetInt64(2)))
                        .razon_social((dr.IsDBNull(3) ? "" : dr.GetString(3)))
                        .provincia_legal(new IdValue(dr.IsDBNull(5) ? -1 : dr.GetInt32(5), (dr.IsDBNull(13) ? "" : dr.GetString(13))))
                        .localidad_legal(new IdValue(dr.IsDBNull(6) ? -1 : dr.GetInt32(6), (dr.IsDBNull(14) ? "" : dr.GetString(14))))
                        .direccion_legal_calle((dr.IsDBNull(7) ? "" : dr.GetString(7)))
                        .direccion_legal_numero(dr.IsDBNull(8) ? -1 : dr.GetInt32(8))
                        .afinidad_tarifaria(new IdValue(dr.IsDBNull(10) ? -1 : dr.GetInt32(10), (dr.IsDBNull(11) ? "" : dr.GetString(11))))
                        .nombre_centro((dr.IsDBNull(12) ? "" : dr.GetString(12)))
                        .tipo_factura((dr.IsDBNull(15) ? "-" : dr.GetString(15)));
                        

                    if (dr.IsDBNull(4))
                        centroBuilder.propio();
                    else
                        centroBuilder.propio(dr.GetString(4));

                    if (dr.IsDBNull(9))
                        centroBuilder.activo();
                    else
                        centroBuilder.activo(dr.GetString(9));


                    CentroFacturable centro = centroBuilder.build();
                    res.Add(centro);
                }
            }
            db.Disconnect();
            return res;
        }

        public List<CentroFacturable> GetCentrosPorBusqueda(int pagina, int resultados_por_pagina, CentroBusqueda busqueda, bool activos)
        {
            string query = basic_query
                + ((activos) ? " where estado = true " : " where ")
                + busqueda.getCriterios()
                +" ORDER BY \"Centros_Id\" DESC limit " + resultados_por_pagina
                + " offset " + (pagina - 1) * resultados_por_pagina;

            List<CentroFacturable> res = new List<CentroFacturable>();
            DBAgenda db = new DBAgenda();
            db.Connect();
            OdbcDataReader dr = db.ExecuteSQL(query);
            while (dr.Read())
            {
                if (!dr.IsDBNull(0))
                {
                    CentroFacturable.CentroFacturableBuilder centroBuilder = CentroFacturable.getBuilder()
                        .id(dr.GetInt32(0))
                        .nombre_fantasia((dr.IsDBNull(1) ? "" : dr.GetString(1)))
                        .cuit((dr.IsDBNull(2) ? -1 : dr.GetInt64(2)))
                        .razon_social((dr.IsDBNull(3) ? "" : dr.GetString(3)))
                        .provincia_legal(new IdValue(dr.IsDBNull(5) ? -1 : dr.GetInt32(5), (dr.IsDBNull(13) ? "" : dr.GetString(13))))
                        .localidad_legal(new IdValue(dr.IsDBNull(6) ? -1 : dr.GetInt32(6), (dr.IsDBNull(14) ? "" : dr.GetString(14))))
                        .direccion_legal_calle((dr.IsDBNull(7) ? "" : dr.GetString(7)))
                        .direccion_legal_numero(dr.IsDBNull(8) ? -1 : dr.GetInt32(8))
                        .afinidad_tarifaria(new IdValue(dr.IsDBNull(10) ? -1 : dr.GetInt32(10), (dr.IsDBNull(11) ? "" : dr.GetString(11))))
                        .nombre_centro((dr.IsDBNull(12) ? "" : dr.GetString(12)))
                        .tipo_factura((dr.IsDBNull(15) ? "-" : dr.GetString(15)));

                    if (dr.IsDBNull(4))
                        centroBuilder.propio();
                    else
                        centroBuilder.propio(dr.GetString(4));

                    if (dr.IsDBNull(9))
                        centroBuilder.activo();
                    else
                        centroBuilder.activo(dr.GetString(9));


                    CentroFacturable centro = centroBuilder.build();
                    res.Add(centro);
                }
            }
            db.Disconnect();
            return res;
        }

        public List<CentroFacturable> GetCentrosIncompletos(int pagina, int resultados_por_pagina)
        {
            string query = basic_query
                + " where estado = true and "
                + criterios_centro_incompleto
                + " ORDER BY \"Centros_Id\" DESC limit " + resultados_por_pagina
                + " offset " + (pagina - 1) * resultados_por_pagina;

            List<CentroFacturable> res = new List<CentroFacturable>();
            DBAgenda db = new DBAgenda();
            db.Connect();
            OdbcDataReader dr = db.ExecuteSQL(query);
            while (dr.Read())
            {
                if (!dr.IsDBNull(0))
                {
                    CentroFacturable.CentroFacturableBuilder centroBuilder = CentroFacturable.getBuilder()
                        .id(dr.GetInt32(0))
                        .nombre_fantasia((dr.IsDBNull(1) ? "" : dr.GetString(1)))
                        .cuit((dr.IsDBNull(2) ? -1 : dr.GetInt64(2)))
                        .razon_social((dr.IsDBNull(3) ? "" : dr.GetString(3)))
                        .provincia_legal(new IdValue(dr.IsDBNull(5) ? -1 : dr.GetInt32(5), (dr.IsDBNull(13) ? "" : dr.GetString(13))))
                        .localidad_legal(new IdValue(dr.IsDBNull(6) ? -1 : dr.GetInt32(6), (dr.IsDBNull(14) ? "" : dr.GetString(14))))
                        .direccion_legal_calle((dr.IsDBNull(7) ? "" : dr.GetString(7)))
                        .direccion_legal_numero(dr.IsDBNull(8) ? -1 : dr.GetInt32(8))
                        .afinidad_tarifaria(new IdValue(dr.IsDBNull(10) ? -1 : dr.GetInt32(10), (dr.IsDBNull(11) ? "" : dr.GetString(11))))
                        .nombre_centro((dr.IsDBNull(12) ? "" : dr.GetString(12)))
                        .tipo_factura((dr.IsDBNull(15) ? "-" : dr.GetString(15)));

                    if (dr.IsDBNull(4))
                        centroBuilder.propio();
                    else
                        centroBuilder.propio(dr.GetString(4));

                    if (dr.IsDBNull(9))
                        centroBuilder.activo();
                    else
                        centroBuilder.activo(dr.GetString(9));


                    CentroFacturable centro = centroBuilder.build();
                    res.Add(centro);
                }
            }
            db.Disconnect();
            return res;
        }

        public List<CentroFacturable> GetCentrosTarifasIncompletas(int pagina, int resultados_por_pagina)
        {
            string query = basic_query
                + " where estado = true and "
                + criterios_centro_tarifa_incompleta
                + " ORDER BY \"Centros_Id\" DESC limit " + resultados_por_pagina
                + " offset " + (pagina - 1) * resultados_por_pagina;

            List<CentroFacturable> res = new List<CentroFacturable>();
            DBAgenda db = new DBAgenda();
            db.Connect();
            OdbcDataReader dr = db.ExecuteSQL(query);
            while (dr.Read())
            {
                if (!dr.IsDBNull(0))
                {
                    CentroFacturable.CentroFacturableBuilder centroBuilder = CentroFacturable.getBuilder()
                        .id(dr.GetInt32(0))
                        .nombre_fantasia((dr.IsDBNull(1) ? "" : dr.GetString(1)))
                        .cuit((dr.IsDBNull(2) ? -1 : dr.GetInt64(2)))
                        .razon_social((dr.IsDBNull(3) ? "" : dr.GetString(3)))
                        .provincia_legal(new IdValue(dr.IsDBNull(5) ? -1 : dr.GetInt32(5), (dr.IsDBNull(13) ? "" : dr.GetString(13))))
                        .localidad_legal(new IdValue(dr.IsDBNull(6) ? -1 : dr.GetInt32(6), (dr.IsDBNull(14) ? "" : dr.GetString(14))))
                        .direccion_legal_calle((dr.IsDBNull(7) ? "" : dr.GetString(7)))
                        .direccion_legal_numero(dr.IsDBNull(8) ? -1 : dr.GetInt32(8))
                        .afinidad_tarifaria(new IdValue(dr.IsDBNull(10) ? -1 : dr.GetInt32(10), (dr.IsDBNull(11) ? "" : dr.GetString(11))))
                        .nombre_centro((dr.IsDBNull(12) ? "" : dr.GetString(12)))
                        .tipo_factura((dr.IsDBNull(15) ? "-" : dr.GetString(15)));

                    if (dr.IsDBNull(4))
                        centroBuilder.propio();
                    else
                        centroBuilder.propio(dr.GetString(4));

                    if (dr.IsDBNull(9))
                        centroBuilder.activo();
                    else
                        centroBuilder.activo(dr.GetString(9));


                    CentroFacturable centro = centroBuilder.build();
                    res.Add(centro);
                }
            }
            db.Disconnect();
            return res;
        }

        public long GetCantidadCentros(CentroBusqueda busqueda, bool activos)
        {
            long cantidad = 0;
            string query = "SELECT count(*) "+ from_centros_join_persona_juridica_afinidad_tarifaria_tarifas_centros
                + ((activos) ? " where estado = true " : " where true ")
                + busqueda.getCriterios();

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

        public long GetCantidadCentrosIncompletos()
        {
            long cantidad = 0;
            string query = "SELECT count(*) " + from_centros_join_persona_juridica_afinidad_tarifaria_tarifas_centros + " where estado = true and "
                + criterios_centro_incompleto
                ;

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

        public long GetCantidadCentrosTarifasIncompletas()
        {
            long cantidad = 0;
            string query = "SELECT count(*) " + from_centros_join_persona_juridica_afinidad_tarifaria_tarifas_centros + " where estado = true and "
                + criterios_centro_tarifa_incompleta
                ;

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

        public void Update(CentroFacturable centro)
        {
            List<string> attrNames = centro.GetAttrNamesCentro();
            List<string> attrValues = centro.GetAttrValuesCentro();
            string sql_centros = "UPDATE \"Centros\" SET ";
            if(attrNames.Count == attrValues.Count)
                for(int i = 0; i < attrNames.Count; ++i)
                {
                    sql_centros += attrNames[i] + "=" + attrValues[i];
                    if (i == (attrValues.Count - 1))
                        sql_centros += " ";
                    else
                        sql_centros += ", ";
                }
            sql_centros += "WHERE \"Centros_Id\" = " + centro.id;

            PersonaJuridica_dbManager pj_db = new PersonaJuridica_dbManager();
            if (pj_db.Existe(centro.cuit))
                pj_db.Update(centro);
            else
                pj_db.Crear(centro);

            DBAgenda db = new DBAgenda();
            db.Connect();

            OdbcDataReader dr = db.ExecuteSQL(sql_centros);
            db.Disconnect();
        }
    }
}