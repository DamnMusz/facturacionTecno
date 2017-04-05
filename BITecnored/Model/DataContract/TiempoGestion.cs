using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace BITecnored.Model.DataContract
{
    [DataContract]
    public class TiempoGestion
    {
        [DataMember]
        TiempoDeInspeccionAInforme tiempoDeInspeccionAInforme { get; set; }
        [DataMember]
        TiempoDeSolicitudAInspeccion tiempoDeSolicitudAInspeccion { get; set; }
        [DataMember]
        public double promedio { get; set; }

        public TiempoGestion(TiempoDeInspeccionAInforme tiempoDeInspeccionAInforme, TiempoDeSolicitudAInspeccion tiempoDeSolicitudAInspeccion, double tiempoPromedio)
        {
            this.tiempoDeInspeccionAInforme = tiempoDeInspeccionAInforme;
            this.tiempoDeSolicitudAInspeccion = tiempoDeSolicitudAInspeccion;
            this.promedio = Math.Round(tiempoPromedio,2);
        }
    }
}