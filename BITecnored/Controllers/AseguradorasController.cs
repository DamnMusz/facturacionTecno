using System.Collections.Generic;
using System.Web.Http;
using System;
using BITecnored.Model.DataContract;
using BITecnored.Model.DB;
using BITecnored.Model;
using System.Data.Odbc;
using System.Linq;

namespace BITecnored.Controllers
{
    [Authorize]
    public class AseguradorasController : ApiController
    {
        Aseguradoras_dbManager manager = new Aseguradoras_dbManager();

        [HttpGet]
        [Route("api/Aseguradoras")]
        public string Get()
        {
            try
            {
                List<Aseguradora> res = new List<Aseguradora>();
                string aseguradoras = this.getManager().GetAseguradoras();
                return aseguradoras;
            }
            catch (Exception e) { return e.StackTrace; }
        }

        [HttpGet]
        [Route("api/AseguradorasList")]
        public IList<Entities.Basic.Aseguradora> GetAseList()
        {
            return new Entities.Basic.Aseguradora().Read().Cast<Entities.Basic.Aseguradora>().ToList();
        }

        public Aseguradoras_dbManager getManager()
        {
            return manager;
        }

        public void setManager(Aseguradoras_dbManager manager)
        {
            this.manager = manager;
        }
    }
}
