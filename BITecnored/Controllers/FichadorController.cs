using BITecnored.Model.DataContract;
using BITecnored.Model.Fichador;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BITecnored.Controllers
{
    public class FichadorController : ApiController
    {
        [HttpGet]
        [Route("api/Fichador/GetClocks")]
        public IEnumerable<IdValueValue> Get()
        {
            Anviz fichador = new Anviz();
            List<IdValueValue> res = fichador.GetClocks();
            return res;
        }

        // GET: api/Fichador/5
        public string Get([FromUri] int id)
        {
            return "value";
        }

        // POST: api/Fichador
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Fichador/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Fichador/5
        public void Delete(int id)
        {
        }
    }
}
