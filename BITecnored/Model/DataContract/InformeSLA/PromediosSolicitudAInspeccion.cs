using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace BITecnored.Model.DataContract.InformeSLA
{
    [DataContract]
    public class PromediosSolicitudAInspeccion : PromediosGestion
    {
        public PromediosSolicitudAInspeccion(CantidadesGestion cantidades) : base(cantidades) { }
    }
}