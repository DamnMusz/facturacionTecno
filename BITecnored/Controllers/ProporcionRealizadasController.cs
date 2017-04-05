using System.Collections.Generic;
using System.Web.Http;
using System;
using System.Globalization;
using BITecnored.Model.DataContract;
using BITecnored.Model;
using BITecnored.Model.DB;
using System.Data.Odbc;
using BITecnored.Entities.SLA.Estadisticas;

namespace BITecnored.Controllers
{
    [Authorize]
    public class ProporcionRealizadasController : ApiController
    {
        public string Get([FromUri] int aseguradoraID, [FromUri] string mesActual, [FromUri] int mesesHaciaAtras, [FromUri] int provincia_id)
        {
            try
            {
                string aseguradoraAlias = Utils.GetAseguradoraAlias(aseguradoraID);
                return Process(aseguradoraAlias, mesActual, mesesHaciaAtras, provincia_id);
            }
            catch (Exception e) { return e.StackTrace; }
        }

        public string Get([FromUri] string mesActual, [FromUri] int mesesHaciaAtras, [FromUri] int provincia_id)
        {
            try
            {
                return Process(null, mesActual, mesesHaciaAtras, provincia_id);
            }
            catch (Exception e) { return e.StackTrace; }
        }

        private string Process(string aseguradoraAlias, string mesActual, int mesesHaciaAtras, int provincia_id)
        {
            List<ProporcionRealizadas> proporciones = new List<ProporcionRealizadas>();

            string inicioMes = "01/" + mesActual;
            DateTime fechaDesde = DateTime.ParseExact(inicioMes, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            string finMes = DateTime.DaysInMonth(fechaDesde.Year, fechaDesde.Month) + "/" + mesActual;
            DateTime fechaHasta = DateTime.ParseExact(finMes, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            DBAgenda db = new DBAgenda();
            db.Connect();

            for (int index = 0; index <= mesesHaciaAtras; ++index)
            {
                int value = -index;
                DateTime auxDate = fechaDesde.AddMonths(value);

                string query = GenerateQuery(auxDate, aseguradoraAlias, provincia_id);

                OdbcDataReader dr = db.ExecuteSQL(query);

                if (dr.HasRows)
                {
                    ProporcionRealizadas proporcion = new ProporcionRealizadas(auxDate.ToString("MM/yyyy"), 0, 0);
                    while (dr.Read())
                    {
                        if (dr.GetInt32(0) == 5)
                            proporcion.realizadas = dr.GetInt32(1);
                        if (dr.GetInt32(0) == 6)
                            proporcion.sin_efecto = dr.GetInt32(1);
                    }

                    proporciones.Add(proporcion);
                }
                else
                {
                    dr.Close();
                    break;
                }
                dr.Close();
            }
            db.Disconnect();
            string data = Serializer.ToJSon<ProporcionRealizadas>(proporciones);
            return data;
        }

        private string GenerateQuery(DateTime auxDate, string aseguradoraAlias, int provincia_id)
        {
            string query =
                "SELECT estado, count(*) FROM sla_agenda"
                    + ((provincia_id != -1) ? " INNER JOIN \"Inspecciones\" ON \"Inspecciones\".\"Id\" = sla_agenda.id_agenda" : "")
                    + " WHERE periodo = " + auxDate.Year.ToString("D" + 4) + auxDate.Month.ToString("D" + 2)
                    + " AND (estado = 5 OR estado = 6)"
                    + ((aseguradoraAlias!=null && aseguradoraAlias != "") ? " AND aseguradora_alias = '" + aseguradoraAlias + "'" : "")
                    + ((provincia_id != -1) ? (" AND \"Inspecciones\".\"Provincia\" = " + provincia_id) : "")
                    + " GROUP BY estado"
                    + " ORDER BY estado"
                    ;
            return query;
        }

        [Route("api/Realizadas_Por_Aseguradora")]
        public Dictionary<string, SolicitudesAgenda> GetSolicitudesPorAseguradora([FromUri] string periodo)
        {
            DateTime periodoObj = Utils.LastDayInMonth(periodo);
            IList<SolicitudesAgenda> res = new SolicitudesAgenda().GetProporcionesPorAseguradora(periodoObj, -1);
            Dictionary<string, SolicitudesAgenda> resumen = new Dictionary<string, SolicitudesAgenda>();
            foreach(SolicitudesAgenda solicitud in res)
                resumen.Add(solicitud.aseguradora_alias, solicitud);
            return resumen;
        }
    }
}
