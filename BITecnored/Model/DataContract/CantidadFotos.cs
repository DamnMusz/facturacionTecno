using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace BITecnored.Model.DataContract
{
    [DataContract]
    public class CantidadFotos
    {
        [DataMember]
        List<RangoFotos> valores = new List<RangoFotos>();
        [DataMember]
        double promedio_total = 0.0d;

        double cantidad = 0.0d;
        double sumatoriaValores = 0.0d;

        public CantidadFotos() { }
        public void AddRango(int rango_desde, int rango_hasta)
        {
            valores.Add(new RangoFotos(rango_desde, rango_hasta));
        }
        public void AddRango(int rango_desde)
        {
            valores.Add(new RangoFotos(rango_desde));
        }
        public void Procesar(int valor)
        {
            foreach (RangoFotos rango in valores)
                rango.Procesar(valor);
            cantidad++;
            sumatoriaValores += valor;
            promedio_total = sumatoriaValores / cantidad;
        }
    }
}