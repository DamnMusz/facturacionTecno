using BITecnored.Model;
using BITecnored.Model.DataContract;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace BITecnored.Controllers
{
    [Authorize]
    public class FacturacionCentrosController : ApiController
    {
        [HttpGet]
        [Route("api/get_all_centros")]
        public async Task<HttpResponseMessage> Get(HttpRequestMessage request, [FromUri]int page, [FromUri]int view_size, [FromUri] bool activos)
        {
            Centros_dbManager dbManager = new Centros_dbManager();
            List<CentroFacturable> centros = dbManager.GetAllCentros(page, view_size, activos);

            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(Serializer.ToJSon<CentroFacturable>(centros), System.Text.Encoding.UTF8, "application/json");
            return response;
        }

        [HttpPost]
        [Route("api/buscar_centros")]
        public async Task<HttpResponseMessage> Buscar(HttpRequestMessage request, [FromUri]int page, [FromUri]int view_size, [FromUri] bool activos, [FromBody] CentroBusqueda centro)
        {
            Centros_dbManager dbManager = new Centros_dbManager();
            List<CentroFacturable> centros = dbManager.GetCentrosPorBusqueda(page, view_size, centro, activos);

            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(Serializer.ToJSon<CentroFacturable>(centros), System.Text.Encoding.UTF8, "application/json");
            return response;
        }

        [HttpGet]
        [Route("api/buscar_centros_incompletos")]
        public async Task<HttpResponseMessage> BuscarIncompletos(HttpRequestMessage request, [FromUri]int page, [FromUri]int view_size)
        {
            Centros_dbManager dbManager = new Centros_dbManager();
            List<CentroFacturable> centros = dbManager.GetCentrosIncompletos(page, view_size);

            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(Serializer.ToJSon<CentroFacturable>(centros), System.Text.Encoding.UTF8, "application/json");
            return response;
        }

        [HttpGet]
        [Route("api/buscar_centros_tarifas_incompletas")]
        public async Task<HttpResponseMessage> BuscarTarifasIncompletas(HttpRequestMessage request, [FromUri]int page, [FromUri]int view_size)
        {
            Centros_dbManager dbManager = new Centros_dbManager();
            List<CentroFacturable> centros = dbManager.GetCentrosTarifasIncompletas(page, view_size);

            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(Serializer.ToJSon<CentroFacturable>(centros), System.Text.Encoding.UTF8, "application/json");
            return response;
        }

        [HttpPost]
        [Route("api/get_amount_centros")]
        public async Task<HttpResponseMessage> GetAmount(HttpRequestMessage request, [FromUri] bool activos, [FromBody] CentroBusqueda criterios)
        {
            Centros_dbManager dbManager = new Centros_dbManager();
            long cantidad = dbManager.GetCantidadCentros(criterios, activos);

            var response = Request.CreateResponse(HttpStatusCode.OK);
            string json = "{ \"cantidad\": " + cantidad + " }";
            response.Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            return response;
        }

        [HttpGet]
        [Route("api/get_amount_centros_incompletos")]
        public async Task<HttpResponseMessage> GetAmountIncompletos(HttpRequestMessage request)
        {
            Centros_dbManager dbManager = new Centros_dbManager();
            long cantidad = dbManager.GetCantidadCentrosIncompletos();

            var response = Request.CreateResponse(HttpStatusCode.OK);
            string json = "{ \"cantidad\": " + cantidad + " }";
            response.Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            return response;
        }

        [HttpGet]
        [Route("api/get_amount_centros_tarifas_incompletas")]
        public async Task<HttpResponseMessage> GetAmountTarifasIncompletas(HttpRequestMessage request)
        {
            Centros_dbManager dbManager = new Centros_dbManager();
            long cantidad = dbManager.GetCantidadCentrosTarifasIncompletas();

            var response = Request.CreateResponse(HttpStatusCode.OK);
            string json = "{ \"cantidad\": " + cantidad + " }";
            response.Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            return response;
        }

        [HttpGet]
        [Route("api/centros_unlock")]
        public async Task<HttpResponseMessage> Unlock(HttpRequestMessage request)
        {
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }
        
        [HttpPost]
        [Route("api/update_centro")]
        public async Task<HttpResponseMessage> Update(HttpRequestMessage request, [FromBody] CentroFacturable centro)
        {
            Centros_dbManager dbManager = new Centros_dbManager();
            dbManager.Update(centro);

            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }
    }
}
