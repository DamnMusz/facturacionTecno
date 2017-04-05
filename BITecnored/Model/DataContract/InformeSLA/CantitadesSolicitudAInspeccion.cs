using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BITecnored.Entities.SLA.Informe;
using System.Runtime.Serialization;

namespace BITecnored.Model.DataContract.InformeSLA
{
    [DataContract]
    public class CantitadesSolicitudAInspeccion : CantidadesGestion
    {
        public CantitadesSolicitudAInspeccion(List<FechasGestionInspeccion> fechas, string feriados) : base(fechas, feriados) {}

        public override DateTime FechaDesde(FechasGestionInspeccion inspeccion)
        {
            return inspeccion.fecha_solicitud;
        }

        public override DateTime FechaHasta(FechasGestionInspeccion inspeccion)
        {
            return inspeccion.fecha_inspeccion;
        }
    }
}