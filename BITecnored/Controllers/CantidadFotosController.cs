using BITecnored.Entities.SLA.Estadisticas;
using BITecnored.Model;
using BITecnored.Model.DataContract;
using BITecnored.Model.DB;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace BITecnored.Controllers
{
    [Authorize]
    public class CantidadFotosController : ApiController
    {
        [HttpGet]
        public async Task<HttpResponseMessage> Get([FromUri] int aseguradora_id, [FromUri] string periodo, [FromUri] int provincia_id)
        {
            try {
                CantidadFotos fotos = Ejecutar(aseguradora_id, periodo, provincia_id);
                var response = Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(Serializer.ToJSon<CantidadFotos>(fotos), System.Text.Encoding.UTF8, "application/json");
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
                CantidadFotos fotos = Ejecutar(null, periodo, provincia_id);
                var response = Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(Serializer.ToJSon<CantidadFotos>(fotos), System.Text.Encoding.UTF8, "application/json");
                return response;
            }
            catch (Exception e)
            {
                var response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                response.Content = new StringContent(e.StackTrace, System.Text.Encoding.UTF8, "application/text");
                return response;
            }
        }

        private CantidadFotos Ejecutar(int? aseguradora_id, string periodo, int provincia_id)
        {
            string inicioMes = "01/" + periodo;
            DateTime fechaDesde = DateTime.ParseExact(inicioMes, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            string mes = fechaDesde.Year.ToString("D" + 4) + fechaDesde.Month.ToString("D" + 2);

            string aseguradora_alias = Utils.GetAseguradoraAlias(aseguradora_id);

            CantidadFotos fotos = new CantidadFotos();
            fotos.AddRango(0, 4);
            fotos.AddRango(5, 8);
            fotos.AddRango(9, 12);
            fotos.AddRango(13);

            string query = "";

            if(provincia_id == -1)
                query = "SELECT fotos_cantidad FROM sla WHERE periodo = " + mes + ((aseguradora_alias!= null)?" AND aseguradora_alias = '" + aseguradora_alias + "'":"");
            else
                query = "SELECT fotos_cantidad FROM sla INNER JOIN lugares ON sla.id_lugares = lugares.id"
                    + " WHERE periodo = " + mes + ((aseguradora_alias != null) ? " AND aseguradora_alias = '" + aseguradora_alias + "'" : "")
                    + " AND tipo_prestador <> 'BT' "
                    + " AND cod_provincia = " + provincia_id;
            DBAgenda db = new DBAgenda();
            db.Connect();
            OdbcDataReader dr = db.ExecuteSQL(query);
            if (dr.HasRows)
                while (dr.Read())
                {
                    fotos.Procesar(dr.GetInt32(0));
                }
            dr.Close();
            db.Disconnect();
            return fotos;
        }

        [Route("api/Fotos_Por_Aseguradora")]
        public IList<Fotos> GetPorAseguradora([FromUri] string periodo)
        {
            DateTime periodoObj = Utils.LastDayInMonth(periodo);
            IList<Fotos> res = new Fotos().GetResumenPorAseguradora(periodoObj);
            return res;
        }
    }
}
