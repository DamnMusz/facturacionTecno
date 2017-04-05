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
    public class TarifaNueva
    {
        [DataMember]
        public string id_centro { get; set; } = "";
        [DataMember]
        public string periodo_desde { get; set; } = "";
        [DataMember]
        public string periodo_hasta { get; set; } = "";
        [DataMember]
        public bool por_rango { get; set; }
        [DataMember]
        public string monto_por_ip { get; set; } = "";
        [DataMember]
        public string observacion { get; set; } = "";

        public DateTime fecha_creacion { get; set; } = DateTime.Now;
        public string usuario_creacion { get; set; } = "";

        public DateTime getFechaDesde()
        {
            return Utils.FirstDayInMonth(periodo_desde);
        }

        public DateTime getFechaHasta()
        {
            if(por_rango)
                return Utils.LastDayInMonth(periodo_hasta);
            else
                return Utils.LastDayInMonth(periodo_desde);
        }

        public List<TarifaNueva> descomponerEnPeriodos()
        {
            IEnumerable<DateTime> rango = Utils.MesesEnRango(getFechaDesde(), getFechaHasta());
            List<TarifaNueva> res = new List<TarifaNueva>();

            foreach(DateTime fecha in rango)
            {
                TarifaNueva nueva = new TarifaNueva();
                nueva.id_centro = this.id_centro;
                nueva.periodo_desde = fecha.Month.ToString("D" + 2) + "/" + fecha.Year.ToString("D" + 4);
                nueva.periodo_hasta = "";
                nueva.por_rango = false;
                nueva.monto_por_ip = this.monto_por_ip;
                nueva.observacion = this.observacion;
                nueva.fecha_creacion = this.fecha_creacion;
                nueva.usuario_creacion = this.usuario_creacion;
                res.Add(nueva);
            }
            return res;
        }
    }
}