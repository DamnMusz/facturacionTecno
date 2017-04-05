using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace BITecnored.Model.DataContract
{
    [DataContract]
    public class RangoFotos
    {
        [DataMember]
        protected string texto { get; set; }
        [DataMember]
        protected int cantidad { get; set; } = 0;
        [DataMember]
        protected double promedio { get; set; } = 0.0d;

        protected int rango_desde;
        protected int rango_hasta;
        protected bool sin_fin = false;
        protected double sumatoriaValores = 0.0d;

        public RangoFotos(int rango_desde, int rango_hasta)
        {
            texto = rango_desde + " a "+rango_hasta+" fotos";
            this.rango_desde = rango_desde;
            this.rango_hasta = rango_hasta;
        }

        public RangoFotos(int rango_desde)
        {
            texto = "mas de " + rango_desde + " fotos";
            this.rango_desde = rango_desde;
            sin_fin = true;
        }

        public void Procesar(int value)
        {
            if (value >= rango_desde
                && (sin_fin || value <= rango_hasta))
            {
                ++cantidad;
                sumatoriaValores += value;
                promedio = sumatoriaValores / cantidad;
            }
        }
    }
}