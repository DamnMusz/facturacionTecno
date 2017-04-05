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
using BITecnored.Entities.SLA;
using BITecnored.Entities.SLA.Estadisticas;

namespace BITecnored.Controllers
{
    [Authorize]
    public class CantidadInspeccionesController : ApiController
    {
        [HttpGet]
        [Route("api/nro_insp_centro")]
        public HttpResponseMessage GetCentro([FromUri] int aseguradora_id, [FromUri] string periodo, [FromUri] int provincia_id)
        {
            DateTime periodoObj = Utils.FirstDayInMonth(periodo);
            string aseguradora_alias;
            if (aseguradora_id != -1)
                aseguradora_alias = Utils.GetAseguradoraAlias(aseguradora_id);
            else
                aseguradora_alias = null;
            Int64 res = new InspeccionSLA().NroInspCentro(periodoObj, aseguradora_alias, provincia_id);
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StringContent(""+res, Encoding.UTF8, "application/text");
            return response;
        }

        [Route("api/nro_insp_domicilio")]
        public HttpResponseMessage GetDomicilio([FromUri] int aseguradora_id, [FromUri] string periodo, [FromUri] int provincia_id)
        {
            DateTime periodoObj = Utils.FirstDayInMonth(periodo);
            string aseguradora_alias;
            if (aseguradora_id != -1)
                aseguradora_alias = Utils.GetAseguradoraAlias(aseguradora_id);
            else
                aseguradora_alias = null;
            Int64 res = new InspeccionSLA().NroInspDomicilio(periodoObj, aseguradora_alias, provincia_id);

            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StringContent(""+res, Encoding.UTF8, "application/text");
            return response;
        }

        [Route("api/resumen_semestral")]
        public IList<CantidadRealizadas> GetSemestral([FromUri] string periodo)
        {
            DateTime periodoObj = Utils.FirstDayInMonth(periodo);
            IList<CantidadRealizadas> resumen = new CantidadRealizadas().GetResumenSemestralPorAseguradora(periodoObj);
            return resumen;
        }

        [Route("api/resumen_semestral_centros")]
        public IList<CantidadRealizadas> GetSemestralCentros([FromUri] string periodo)
        {
            DateTime periodoObj = Utils.FirstDayInMonth(periodo);
            IList<CantidadRealizadas> resumen = new CantidadRealizadas().GetResumenSemestralPorCentro(periodoObj);
            return resumen;
        }

        [Route("api/resumen_semestral_inspectores")]
        public IList<CantidadRealizadas> GetSemestralInspectores([FromUri] string periodo)
        {
            DateTime periodoObj = Utils.FirstDayInMonth(periodo);
            IList<CantidadRealizadas> resumen = new CantidadRealizadas().GetResumenSemestralPorInspector(periodoObj);
            return resumen;
        }
    }
}
