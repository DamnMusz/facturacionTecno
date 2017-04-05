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
    public class Tarifa : TarifaNueva
    {
        [DataMember]
        public string id { get; set; } = "";
        [DataMember]
        public new string fecha_creacion { get; set; } = "";
        [DataMember]
        public new string usuario_creacion { get; set; } = "";

        public static TarifaBuilder GetBuilder()
        {
            return new TarifaBuilder();
        }

        public class TarifaBuilder
        {
            Tarifa tarifa;
            public TarifaBuilder()
            {
                tarifa = new Tarifa();
            }

            public TarifaBuilder id(string id)
            {
                tarifa.id = id;
                return this;
            }

            public TarifaBuilder id_centro(string id_centro)
            {
                tarifa.id_centro = id_centro;
                return this;
            }

            public TarifaBuilder periodo_desde(string periodo_desde)
            {
                tarifa.periodo_desde = periodo_desde;
                return this;
            }

            public TarifaBuilder periodo_hasta(string periodo_hasta)
            {
                tarifa.periodo_hasta = periodo_hasta;
                return this;
            }

            public TarifaBuilder monto_por_ip(string monto_por_ip)
            {
                tarifa.monto_por_ip = monto_por_ip;
                return this;
            }

            public TarifaBuilder observacion(string observacion)
            {
                tarifa.observacion = observacion;
                return this;
            }

            public TarifaBuilder fecha_creacion(string fecha_creacion)
            {
                tarifa.fecha_creacion = fecha_creacion;
                return this;
            }

            public TarifaBuilder usuario_creacion(string usuario_creacion)
            {
                tarifa.usuario_creacion = usuario_creacion;
                return this;
            }
            
            public Tarifa build()
            {
                return tarifa;
            }
        }
    }
}