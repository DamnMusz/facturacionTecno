using System;
using System.Runtime.Serialization;

namespace BITecnored.Model.DataContract
{
    [DataContract]
    public abstract class TiempoPorServicio
    {
        [DataMember]
        RangoTiempos centro;
        [DataMember]
        RangoTiempos domicilio;
        [DataMember]
        public double promedio { get; set; }

        public TiempoPorServicio(RangoTiempos fechasCentro, RangoTiempos fechasDomicilio, double tiempoPromedio)
        {
            this.centro = fechasCentro;
            this.domicilio = fechasDomicilio;
            this.promedio = Math.Round(tiempoPromedio,2);
        }
    }
}