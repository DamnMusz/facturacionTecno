using BITecnored.Model;
using BITecnored.Model.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace BITecnored.Controllers
{
    [Authorize]
    public class ProvinciasController : ApiController
    {
        [HttpGet]
        [Route("api/get_provincias")]
        public async Task<HttpResponseMessage> GetProvincias()
        {
            Provincias_dbManager dbManager = new Provincias_dbManager();
            List<IdValue> provincias = dbManager.GetAllProvincias();

            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(Serializer.ToJSon<List<IdValue>>(provincias), System.Text.Encoding.UTF8, "application/json");
            return response;
        }

        [HttpGet]
        [Route("api/get_provincias_sla")]
        public async Task<HttpResponseMessage> GetProvinciasSLA()
        {
            Provincias_dbManager dbManager = new Provincias_dbManager();
            List<IdValue> provincias = dbManager.GetAllProvinciasSLA();

            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(Serializer.ToJSon<List<IdValue>>(provincias), System.Text.Encoding.UTF8, "application/json");
            return response;
        }
    }
}
