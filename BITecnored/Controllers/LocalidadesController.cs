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
    public class LocalidadesController : ApiController
    {
        [Authorize]
        [HttpGet]
        [Route("api/get_localidades")]
        public async Task<HttpResponseMessage> GetLocalidades([FromUri]int provincia)
        {
            Localidades_dbManager dbManager = new Localidades_dbManager();
            List<IdValue> provincias = dbManager.GetAllLocalidades(provincia);

            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(Serializer.ToJSon<List<IdValue>>(provincias), System.Text.Encoding.UTF8, "application/json");
            return response;
        }
    }
}
