using BITecnored.Entities.Basic;
using BITecnored.Entities.SLA.Informe;
using BITecnored.Model.DataContract.InformeSLA;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace BITecnored.Model
{
    public class TiemposGestionPrevias
    {
        private static TiemposGestionPrevias _instance;

        public static TiemposGestionPrevias GetInstance()
        {
            if (_instance == null)
                _instance = new TiemposGestionPrevias();
            return _instance;
        }

        public ResumenGestion GetResumen(int aseguradora_id, string periodo, int provincia_id)
        {
            DateTime periodoObj = Utils.LastDayInMonth(periodo);
            string aseguradora_alias = Utils.GetAseguradoraAlias(aseguradora_id);
            string feriados = Utils.GetFeriados();

            List<FechasGestionInspeccion> tiempos = null;

            if(provincia_id == -1)
                tiempos = new FechasGestionInspeccion().SetPeriodoBusqueda(periodoObj).SetAseguradoraBusqueda(aseguradora_alias).Read().Cast<FechasGestionInspeccion>().ToList();
            else
                tiempos = new FechasGestionInspeccion().SetPeriodoBusqueda(periodoObj).SetAseguradoraBusqueda(aseguradora_alias).SetProvinciaBusqueda(provincia_id).Read().Cast<FechasGestionInspeccion>().ToList();
            return new ResumenGestion(tiempos, feriados);
        }

        public ResumenGestion GetResumenTotal(string periodo, int provincia_id)
        {
            DateTime periodoObj = Utils.LastDayInMonth(periodo);
            string feriados = Utils.GetFeriados();

            List<FechasGestionInspeccion> tiempos = null;
            if (provincia_id == -1)
                tiempos = new FechasGestionInspeccion().SetPeriodoBusqueda(periodoObj).Read().Cast<FechasGestionInspeccion>().ToList();
            else
                tiempos = new FechasGestionInspeccion().SetPeriodoBusqueda(periodoObj).SetProvinciaBusqueda(provincia_id).Read().Cast<FechasGestionInspeccion>().ToList();
            
            ResumenGestion resumen = new ResumenGestion(tiempos, feriados);
            return resumen;
        }

        public Dictionary<string, ResumenGestion> GetResumenPorAseguradora(string periodo)
        {
            DateTime periodoObj = Utils.LastDayInMonth(periodo);
            string feriados = Utils.GetFeriados();

            List<FechasGestionInspeccion> tiempos = new FechasGestionInspeccion().SetPeriodoBusqueda(periodoObj).Read().Cast<FechasGestionInspeccion>().ToList();
            Dictionary<string, ResumenGestion> resumen = ResumenGestion.GetByAseguradora(tiempos, feriados);

            return resumen;
        }
    }
}