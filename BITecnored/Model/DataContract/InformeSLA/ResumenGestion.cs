using BITecnored.Entities;
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
    public class ResumenGestion
    {
        [DataMember]
        public virtual CantitadesSolicitudAInspeccion cant_sol_insp { get; set; }
        [DataMember]
        public virtual PromediosSolicitudAInspeccion prom_sol_insp { get; set; }
        [DataMember]
        public virtual CantitadesInspeccionAInforme cant_insp_pub { get; set; }
        [DataMember]
        public virtual PromediosInspeccionAInforme prom_insp_pub { get; set; }

        public ResumenGestion(List<FechasGestionInspeccion> fechas, string feriados)
        {
            cant_sol_insp = new CantitadesSolicitudAInspeccion(fechas, feriados);
            prom_sol_insp = new PromediosSolicitudAInspeccion(cant_sol_insp);
            cant_insp_pub = new CantitadesInspeccionAInforme(fechas, feriados);
            prom_insp_pub = new PromediosInspeccionAInforme(cant_insp_pub);
        }

        public static Dictionary<string, ResumenGestion> GetByAseguradora(List<FechasGestionInspeccion> fechas, string feriados)
        {
            Dictionary<string, ResumenGestion> res = new Dictionary<string, ResumenGestion>();
            foreach (IGrouping<string, FechasGestionInspeccion> fechas_para_aseguradora in fechas.GroupBy(x => x.aseguradora_alias)) {
                try
                {
                    if(fechas_para_aseguradora.Key != "ZZZ")
                        res.Add(fechas_para_aseguradora.Key, new ResumenGestion(fechas_para_aseguradora.ToList(), feriados));
                }
                catch (Exception) {}
            }
            return res;
        }
    }
}