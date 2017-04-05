using System.Runtime.Serialization;

namespace BITecnored.Model.DataContract
{
    [DataContract]
    public class TiempoDeSolicitudAInspeccion : TiempoPorServicio
    {
        public TiempoDeSolicitudAInspeccion(RangoTiempos fechasCentro, RangoTiempos fechasDomicilio, double promedioTiempos)
            : base(fechasCentro, fechasDomicilio, promedioTiempos)
        { }
    }
}