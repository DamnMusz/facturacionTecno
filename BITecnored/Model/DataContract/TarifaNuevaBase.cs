using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Diagnostics;

namespace BITecnored.Model.DataContract
{
    [DataContract]
    public class TarifaNuevaBase
    {
        [DataMember]
        public string id { get; set; } = "";
        [DataMember]
        public string titulo { get; set; } = "";
        [DataMember]
        public string monto { get; set; } = "";
        [DataMember]
        public string descripcion { get; set; } = "";
        [DataMember]
        public string color { get; set; } = "";
        [DataMember]
        public string periodo_desde { get; set; } = "";
        [DataMember]
        public string periodo_hasta { get; set; } = "";

        public DateTime fecha_creacion { get; set; } = DateTime.Now;
        public string usuario_creacion { get; set; } = "";

        public DateTime getFechaDesde()
        {
            return Utils.FirstDayInMonth(periodo_desde);
        }

        public DateTime getFechaHasta()
        {
            return Utils.LastDayInMonth(periodo_hasta);
        }
    }
}