using BITecnored.Entities.SLA.Estadisticas;
using BITecnored.Model;
using BITecnored.Model.DataContract;
using BITecnored.Model.DB;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace BITecnored.Controllers
{
    [Authorize]
    public class MotivosDiaPactadoController : ApiController
    {
        public static int ESTADO_EN_GESTION = 2;
        public static int ESTADO_A_RECOMBINAR = 4;
        public static int ESTADO_REALIZADA = 5;
        public static int ESTADO_SIN_EFECTO = 6;
        [HttpGet]
        public async Task<HttpResponseMessage> Get([FromUri] int aseguradora_id, [FromUri] string periodo, [FromUri] int provincia_id)
        {
            try
            {
                int index = periodo.IndexOf('/');
                string anio = periodo.Substring(index+1);
                string mes = periodo.Substring(0, index);
                List<MotivoDiaPactado> motivos = Ejecutar(aseguradora_id, anio+mes, provincia_id);
                var response = Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(Serializer.ToJSon<MotivoDiaPactado>(motivos), System.Text.Encoding.UTF8, "application/json");
                return response;
            }
            catch (Exception e)
            {
                var response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                response.Content = new StringContent(e.StackTrace, System.Text.Encoding.UTF8, "application/text");
                return response;
            }
        }

        [HttpGet]
        public async Task<HttpResponseMessage> Get([FromUri] string periodo, [FromUri] int provincia_id)
        {
            try
            {
                int index = periodo.IndexOf('/');
                string anio = periodo.Substring(index + 1);
                string mes = periodo.Substring(0, index);
                List<MotivoDiaPactado> motivos = Ejecutar(null, anio + mes, provincia_id);
                var response = Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(Serializer.ToJSon<MotivoDiaPactado>(motivos), System.Text.Encoding.UTF8, "application/json");
                return response;
            }
            catch (Exception e)
            {
                var response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                response.Content = new StringContent(e.StackTrace, System.Text.Encoding.UTF8, "application/text");
                return response;
            }
        }

        private List<MotivoDiaPactado> Ejecutar(int? aseguradora_id, string periodo, int provincia_id)
        {
            Dictionary<string, MotivoDiaPactado> motivos = new Dictionary<string, MotivoDiaPactado>();
            string aseguradora_alias = Utils.GetAseguradoraAlias(aseguradora_id);
            ObtenerRealizadas(motivos, aseguradora_alias, periodo, provincia_id);
            ObtenerEnGestion(motivos, aseguradora_alias, periodo, provincia_id);
            ObtenerSinEfecto(motivos, aseguradora_alias, periodo, provincia_id);
            return motivos.Values.ToList();
        }

        private void ObtenerEstado_with_motivos(Dictionary<string, MotivoDiaPactado> motivos, string aseguradora_alias, string periodo, int estado, int provincia_id)
        {
            string query = "SELECT historico.motivo, \"Motivos\".motivos_texto, count(*) " +
            "FROM historico " +
            "INNER JOIN \"Motivos\" ON \"Motivos\".motivos_codigo = historico.motivo " +
            "INNER JOIN subestados ON historico.motivo = subestados.cod " +
            "INNER JOIN sla_agenda ON historico.agenda_id = sla_agenda.id_agenda " +
            ((provincia_id != -1) ? ("INNER JOIN \"Inspecciones\" ON sla_agenda.id_agenda = \"Inspecciones\".\"Id\" ") : " ") +

            "WHERE 1=1 " +
            "AND sla_agenda.estado = " + estado + " " +
            "AND sla_agenda.periodo = " + periodo + " " +
            "AND subestados.estado = " + ESTADO_A_RECOMBINAR + " " +
            ((aseguradora_alias != null) ?
            "AND sla_agenda.aseguradora_alias = '" + aseguradora_alias + "' "
            : " ") +
            ((provincia_id != -1) ? ("AND \"Provincia\" = " + provincia_id + " ") : "") +
            "GROUP BY historico.motivo, \"Motivos\".motivos_texto " +
            "ORDER BY \"Motivos\".motivos_texto";


            DBAgenda db = new DBAgenda();
            db.Connect();
            OdbcDataReader dr = db.ExecuteSQL(query);
            if (dr.HasRows)
                while (dr.Read())
                {
                    string key = dr.GetString(0);
                    MotivoDiaPactado motivo;
                    if (motivos.ContainsKey(key))
                    {
                        motivo = motivos[key];
                    }
                    else
                    {
                        motivo = new MotivoDiaPactado(dr.GetString(1));
                        motivos.Add(key, motivo);
                    }
                    motivo.setByEstado(estado, dr.GetInt32(2));
                }
            dr.Close();
            db.Disconnect();
        }

        private void ObtenerEstado(Dictionary<string, MotivoDiaPactado> motivos, string aseguradora_alias, string periodo, int estado, int provincia_id)
        {
            ObtenerEstado_with_motivos(motivos, aseguradora_alias, periodo, estado, provincia_id);
        }

        private void ObtenerRealizadas(Dictionary<string, MotivoDiaPactado> motivos, string aseguradora_alias, string periodo, int provincia_id)
        {
            ObtenerEstado(motivos, aseguradora_alias, periodo, ESTADO_REALIZADA, provincia_id);
        }

        private void ObtenerEnGestion(Dictionary<string, MotivoDiaPactado> motivos, string aseguradora_alias, string periodo, int provincia_id)
        {
            ObtenerEstado(motivos, aseguradora_alias, periodo, ESTADO_EN_GESTION, provincia_id);
        }

        private void ObtenerSinEfecto(Dictionary<string, MotivoDiaPactado> motivos, string aseguradora_alias, string periodo, int provincia_id)
        {
            ObtenerEstado(motivos, aseguradora_alias, periodo, ESTADO_SIN_EFECTO, provincia_id);
        }

        [Route("api/No_Realizadas_Previas_Por_Aseguradora")]
        public IList<NoRealizadas> GetPorAseguradora([FromUri] string periodo)
        {
            DateTime periodoObj = Utils.FirstDayInMonth(periodo);
            IList<NoRealizadas> resumen = new NoRealizadas().GetProporcionesPorAseguradora(periodoObj);
            return resumen;
        }
    }
}
