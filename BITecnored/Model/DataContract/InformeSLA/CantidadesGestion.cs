using BITecnored.Entities.SLA.Informe;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace BITecnored.Model.DataContract.InformeSLA
{
    [DataContract]
    public abstract class CantidadesGestion
    {
        [DataMember]
        public virtual decimal mismo_dia { get; set; } = 0;
        [DataMember]
        public virtual decimal en_24hs { get; set; } = 0;
        [DataMember]
        public virtual decimal en_48hs { get; set; } = 0;
        [DataMember]
        public virtual decimal en_72hs { get; set; } = 0;
        [DataMember]
        public virtual decimal mas_de_72hs { get; set; } = 0;
        [DataMember]
        public virtual decimal promedio { get; set; } = 0;

        public CantidadesGestion(List<FechasGestionInspeccion> fechas, string feriados)
        {
            if (fechas.Count == 0)
                return;
            decimal contador_dias = 0;
            foreach (FechasGestionInspeccion fecha in fechas)
            {
                int distancia = Utils.DistanciaEntreDias(FechaDesde(fecha), FechaHasta(fecha), feriados);
                if (distancia < 1)
                    mismo_dia++;
                else if (distancia < 2)
                    en_24hs++;
                else if (distancia < 3)
                    en_48hs++;
                else if (distancia < 4)
                    en_72hs++;
                else
                    mas_de_72hs++;
                contador_dias += distancia;
            }
            promedio = Math.Round(contador_dias / fechas.Count,2);
        }

        public abstract DateTime FechaDesde(FechasGestionInspeccion inspeccion);
        public abstract DateTime FechaHasta(FechasGestionInspeccion inspeccion);
    }    
}