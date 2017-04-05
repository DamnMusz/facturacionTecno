using BITecnored.Entities.FacturacionCentros;
using BITecnored.Model;
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
    public class TarifasController : ApiController
    {
        // GET: api/Tarifas
        public IList<Tarifa> Get(HttpRequestMessage request)
        {
            return new Tarifa().Read().Cast<Tarifa>().ToList();
        }


        // POST: api/Tarifas
        public async Task<HttpResponseMessage> Post(HttpRequestMessage request, [FromBody]Tarifa_write value)
        {
            value.usuario_creacion = Utils.GetUsuario(request);
            value.fecha_creacion = DateTime.Today;
            value.Write();

            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }
    }
}
