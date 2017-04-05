using System.Collections.Generic;
using System.Web.Http;
using System;
using System.Globalization;
using BITecnored.Model.DataContract;
using BITecnored.Model;
using BITecnored.Model.DB;
using System.Data.Odbc;
using System.Diagnostics;

namespace BITecnored.Controllers
{
    [Authorize]
    public class MotivosNoRealizadasController : ApiController
    {
        private static int NO_REALIZADA = 6;
        public string Get([FromUri] int aseguradoraID, [FromUri] string mesActual, [FromUri] int provincia_id)
        {
            try
            {
                string aseguradoraAlias = Utils.GetAseguradoraAlias(aseguradoraID);
                return Process(aseguradoraAlias, mesActual, provincia_id);
            }
            catch (Exception e) { return e.StackTrace; }
        }

        public string Get([FromUri] string mesActual, [FromUri] int provincia_id)
        {
            try
            {
                return Process(null, mesActual, provincia_id);
            }
            catch (Exception e) { return e.StackTrace; }
        }

        public string Process(string aseguradoraAlias, string mesActual, int provincia_id)
        {
            List<MotivoNoRealizada> motivos = new List<MotivoNoRealizada>();

            string inicioMes = "01/" + mesActual;
            DateTime fechaDesde = DateTime.ParseExact(inicioMes, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            DBAgenda db = new DBAgenda();
            db.Connect();



            string query = GenerateQuery(fechaDesde, aseguradoraAlias, provincia_id);

            OdbcDataReader dr = db.ExecuteSQL(query);
            if (dr.HasRows)
                while (dr.Read())
                    motivos.Add(new MotivoNoRealizada(dr.GetString(0), dr.GetInt32(1)));
            dr.Close();

            db.Disconnect();
            string data = Serializer.ToJSon<MotivoNoRealizada>(motivos);
            return data;
        }

        private string GenerateQuery(DateTime periodo, string aseguradoraAlias, int provincia_id)
        {
            string query = "";
            if (aseguradoraAlias!=null && aseguradoraAlias!="")
                query = "SELECT \"Motivos\".motivos_texto, count(*)"
                    + " FROM sla_agenda"
                    + " INNER JOIN \"Motivos\" ON sla_agenda.motivo = \"Motivos\".motivos_codigo"
                    + " INNER JOIN \"Aseguradoras\" ON aseguradora_id = \"Aseguradoras\".\"Aseguradoras_Id\""
                    + ((provincia_id != -1) ? " INNER JOIN \"Inspecciones\" ON \"Inspecciones\".\"Id\" = sla_agenda.id_agenda" : "")
                    + " WHERE periodo = " + periodo.Year.ToString("D" + 4) + periodo.Month.ToString("D" + 2)
                    + " AND estado = " + NO_REALIZADA
                    + " AND \"Aseguradoras\".\"Aseguradoras_Alias\" = '" + aseguradoraAlias + "'"
                    + ((provincia_id != -1) ? (" AND \"Inspecciones\".\"Provincia\" = " + provincia_id) : "")
                    + " GROUP BY \"Motivos\".motivos_texto"
                    + " ORDER BY \"Motivos\".motivos_texto";
            else
                query = "SELECT \"Motivos\".motivos_texto, count(*)"
                    + " FROM sla_agenda"
                    + " INNER JOIN \"Motivos\" ON sla_agenda.motivo = \"Motivos\".motivos_codigo"
                    + ((provincia_id != -1) ? " INNER JOIN \"Inspecciones\" ON \"Inspecciones\".\"Id\" = sla_agenda.id_agenda" : "")
                    + " WHERE periodo = " + periodo.Year.ToString("D" + 4) + periodo.Month.ToString("D" + 2)
                    + " AND estado = " + NO_REALIZADA
                    + ((provincia_id != -1) ? (" AND \"Inspecciones\".\"Provincia\" = " + provincia_id) : "")
                    + " GROUP BY \"Motivos\".motivos_texto"
                    + " ORDER BY \"Motivos\".motivos_texto";

            return query;
        }
    }
}
