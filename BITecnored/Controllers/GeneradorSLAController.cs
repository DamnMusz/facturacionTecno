using BITecnored.Model;
using BITecnored.Model.SLA;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace BITecnored.Controllers
{
    [Authorize]
    public class GeneradorSLAController : ApiController
    {
        [HttpGet]
        [Route("api/generate_sla_base")]
        public HttpResponseMessage Get(HttpRequestMessage request, [FromUri] string mes_anio)
        {
            BaseSLAGenerator sla = BaseSLAGenerator.GetInstance();
            if( sla.GetEstado()==BaseSLAGenerator.Estado.RUNNING )
            {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.MethodNotAllowed);
                response.Content = new StringContent("Ya está corriendo el proceso", Encoding.UTF8, "application/text");
                return response;
            } else
            {
                DateTime periodo = Utils.FirstDayInMonth(mes_anio);

                if(sla.ExistsPrevious(periodo))
                {
                    HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Found);
                    response.Content = new StringContent("Ya existe la base en el sistema", Encoding.UTF8, "application/text");
                    return response;
                } else
                {
                    BackgroundWorker bw = new BackgroundWorker();
                    bw.WorkerReportsProgress = true;

                    // what to do in the background thread
                    bw.DoWork += new DoWorkEventHandler(
                    delegate (object o, DoWorkEventArgs args)
                    {
                        sla.UpdateBase(periodo, Utils.GetUsuario(request));
                    });
                    bw.RunWorkerAsync();

                    BackgroundWorker bw2 = new BackgroundWorker();
                    bw2.WorkerReportsProgress = true;

                    // what to do in the background thread
                    bw2.DoWork += new DoWorkEventHandler(
                    delegate (object o, DoWorkEventArgs args)
                    {
                        sla.GenerateNoRealizadas(periodo);
                    });
                    bw2.RunWorkerAsync();
                    return new HttpResponseMessage(HttpStatusCode.OK);
                }
            }
        }

        [HttpGet]
        [Route("api/check_sla_base_existence")]
        public HttpResponseMessage Check(HttpRequestMessage request, [FromUri] int aseguradora_id, [FromUri] string mes_anio)
        {
            DateTime periodo = Utils.FirstDayInMonth(mes_anio);
            string aseguradora_alias = Utils.GetAseguradoraAlias(aseguradora_id);
            bool exist = BaseSLAGenerator.GetInstance().ExistsPrevious(periodo, aseguradora_alias);
            HttpResponseMessage res = new HttpResponseMessage(HttpStatusCode.OK);
            res.Content = new StringContent("{ \"exist\":"+((exist)?"true":"false")+"}", Encoding.UTF8, "application/json");
            return res;
        }

        [HttpGet]
        [Route("api/check_sla_base_existence")]
        public HttpResponseMessage Check(HttpRequestMessage request, [FromUri] string mes_anio)
        {
            DateTime periodo = Utils.FirstDayInMonth(mes_anio);
            bool exist = BaseSLAGenerator.GetInstance().ExistsPrevious(periodo, null);
            HttpResponseMessage res = new HttpResponseMessage(HttpStatusCode.OK);
            res.Content = new StringContent("{ \"exist\":" + ((exist) ? "true" : "false") + "}", Encoding.UTF8, "application/json");
            return res;
        }

        [HttpGet]
        [Route("api/cancel_sla_base")]
        public HttpResponseMessage Cancel(HttpRequestMessage request)
        {
            BaseSLAGenerator sla = BaseSLAGenerator.GetInstance();
            sla.Cancel();
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpGet]
        [Route("api/get_state_sla_base")]
        public HttpResponseMessage GetState(HttpRequestMessage request)
        {
            BaseSLAGenerator sla = BaseSLAGenerator.GetInstance();
            BaseSLAGenerator.Estado estado = sla.GetEstado();
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            string json = "{ \"estado\": \"" + estado + "\"" + ", \"periodo\": \"" + Utils.MesAnioToNumberFormat(sla.periodo_corriendo)
                + "\" , \"tiempo_inicio\": \"" + sla.startTime + "\" }";
            response.Content = new StringContent(json, Encoding.UTF8, "application/json");
            return response;
        }
    }
}
