using BITecnored.Entities;
using BITecnored.Entities.Inspeccion;
using BITecnored.Entities.Logs;
using BITecnored.Entities.SLA;
using BITecnored.Model.DB.Querys;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace BITecnored.Model.SLA
{
    public class BaseSLAAgenda
    {
        public enum Estado { IDLE, RUNNING }
        public static Estado estado = Estado.IDLE;
        private InspeccionSLA runningInspeccion = null;

        public void Insert(List<InspeccionTriki> inspecciones, DateTime periodo, string usuario)
        {   
            new SLACreationLog(Int32.Parse(Utils.MesAnioToNumberFormat(periodo)), DateTime.Now, usuario).Write();

            IList<Entity> res_sla = new List<Entity>();

            foreach (InspeccionTriki one in inspecciones)
                res_sla.Add(new InspeccionSLA(one));

            Entity.WriteAll(res_sla);
        }

        public bool ExistsPrevious(DateTime periodo)
        {
            return new InspeccionSLA().CheckExistence(periodo);
        }

        public bool ExistsPrevious(DateTime periodo, string aseguradora_alias)
        {
            return new InspeccionSLA().CheckExistence(periodo, aseguradora_alias);
        }

        public void Complete(DateTime periodo)
        {
            InspeccionSLA_AnexoAgenda anexo = new InspeccionSLA_AnexoAgenda();
            anexo.CompleteMarried(periodo);
            anexo.CompleteUnmarried(periodo);
        }

        public void Remove(DateTime periodo)
        {
            throw new NotImplementedException();
        }

        private void Start(InspeccionSLA runningInspeccion)
        {
            SetEstado(Estado.RUNNING);
            this.runningInspeccion = runningInspeccion;
        }

        private void Stop()
        {
            SetEstado(Estado.IDLE);
            runningInspeccion = null;
        }

        public void Cancel()
        {
            if(runningInspeccion != null)
                runningInspeccion.Cancel();
            Stop();
        }

        public Estado GetEstado()
        {
            return estado;
        }

        public void SetEstado(Estado value)
        {
            estado = value;
        }
    }
}