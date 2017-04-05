using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace BITecnored.Model.DataContract.InformeSLA
{
    [DataContract]
    public class PromediosInspeccionAInforme : PromediosGestion
    {
        public PromediosInspeccionAInforme(CantidadesGestion cantidades) : base(cantidades) { }
    }
}