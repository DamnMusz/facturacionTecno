using System.Runtime.Serialization;

namespace BITecnored.Model.DataContract
{
    [DataContract]
    public class TiempoDeInspeccionAInforme : TiempoPorServicio
    {
        public TiempoDeInspeccionAInforme(RangoTiempos fechasCentro, RangoTiempos fechasDomicilio, double promedioTiempos)
            : base(fechasCentro, fechasDomicilio, promedioTiempos)
        { }
    }
}