using BITecnored.Model;
using BITecnored.Model.DataContract;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace BITecnored.Controllers
{
    [Authorize]
    public class AfinidadTarifariaController : ApiController
    {
        AfinidadTarifaria_dbManager manager = new AfinidadTarifaria_dbManager();

        [HttpGet]
        [Route("api/afinidades_tarfiarias")]
        public HttpResponseMessage Get()
        {
            try {
                List<IdValue> afinidades = (getManager()).Buscar();
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StringContent(Serializer.ToJSon<List<IdValue>>(afinidades), System.Text.Encoding.UTF8, "application/json");
                return response;
            }
            catch (Exception e)
            {
                var response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                response.Content = new StringContent(e.StackTrace, System.Text.Encoding.UTF8, "application/text");
                return response;
            }
        }

        [HttpPost]
        [Route("api/afinidades_tarfiarias")]
        public HttpResponseMessage Post([FromUri] string nombre)
        {
            try
            {
                (getManager()).Crear(nombre);
                List<IdValue> afinidades = (getManager()).Buscar();
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StringContent(Serializer.ToJSon<List<IdValue>>(afinidades), System.Text.Encoding.UTF8, "application/json");
                return response;
            }
            catch (Exception e)
            {
                var response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                response.Content = new StringContent(e.StackTrace, System.Text.Encoding.UTF8, "application/text");
                return response;
            }
        }

        [HttpDelete]
        [Route("api/afinidades_tarfiarias")]
        public HttpResponseMessage Delete([FromUri] int id)
        {
            try
            {
                bool eliminada = (getManager()).Eliminar(id);
                List<IdValue> afinidades = (getManager()).Buscar();
                HttpResponseMessage response;

                if (eliminada)
                    response = new HttpResponseMessage(HttpStatusCode.OK);
                else
                    response = new HttpResponseMessage(HttpStatusCode.PreconditionFailed);

                response.Content = new StringContent(Serializer.ToJSon<List<IdValue>>(afinidades), System.Text.Encoding.UTF8, "application/json");
                return response;
            }
            catch (Exception e)
            {
                var response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                response.Content = new StringContent(e.StackTrace, System.Text.Encoding.UTF8, "application/text");
                return response;
            }
        }

        public AfinidadTarifaria_dbManager getManager()
        {
            return manager;
        }

        public void setManager(AfinidadTarifaria_dbManager manager)
        {
            this.manager = manager;
        }
    }
}
