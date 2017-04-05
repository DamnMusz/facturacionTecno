using System.Collections.Generic;
using System.Web.Http;
using System;
using System.Globalization;
using BITecnored.Model.DataContract;
using BITecnored.Model;
using BITecnored.Model.DB;
using System.Data.Odbc;
using System.Net.Http;
using System.Net;
using BITecnored.Entities.SLA.Informe;
using System.Linq;
using BITecnored.Model.DataContract.InformeSLA;
using System.Text;
using System.Diagnostics;
using BITecnored.Entities.SLA.Estadisticas;

namespace BITecnored.Controllers
{
    [Authorize]
    public class TiempoGestionController : ApiController
    {
        [HttpGet]
        [Route("api/Tiempos_Gestion_Previas")]
        public HttpResponseMessage Get([FromUri] int aseguradora_id, [FromUri] string periodo, [FromUri] int provincia_id)
        {
            ResumenGestion resumen = TiemposGestionPrevias.GetInstance().GetResumen(aseguradora_id, periodo, provincia_id);
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StringContent(Serializer.ToJSon(resumen), Encoding.UTF8, "application/json");
            return response;
        }

        [Route("api/Tiempos_Gestion_Previas")]
        public HttpResponseMessage Get([FromUri] string periodo, [FromUri] int provincia_id)
        {
            ResumenGestion resumen = TiemposGestionPrevias.GetInstance().GetResumenTotal(periodo, provincia_id);

            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StringContent(Serializer.ToJSon(resumen), Encoding.UTF8, "application/json");
            return response;
        }

        [Route("api/Tiempos_Gestion_Previas_Por_Aseguradora")]
        public Dictionary<string, ResumenGestion> GetPorAseguradora([FromUri] string periodo)
        {
            Dictionary<string, ResumenGestion> resumen = TiemposGestionPrevias.GetInstance().GetResumenPorAseguradora(periodo);
            return resumen;
        }
    }
}
