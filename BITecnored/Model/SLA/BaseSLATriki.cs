using System;
using System.Collections.Generic;
using BITecnored.Entities;
using BITecnored.Entities.Inspeccion;
using System.Linq;

namespace BITecnored.Model.SLA
{
    public class BaseSLATriki
    {
        public enum Estado { IDLE, RUNNING }
        public static Estado estado = Estado.IDLE;
        private InspeccionTriki runningInspeccion = null;

        public List<InspeccionTriki> GetInspecciones(DateTime periodo)
        {
            InspeccionTriki inspeccion = new InspeccionTriki();
            Start(inspeccion);
            IList<Entity> aux = inspeccion.SetPeriodoFacturacion(periodo).Read();
            List<InspeccionTriki> res = new List<Entity>(aux).Cast<InspeccionTriki>().ToList();
            Stop();
            return (res);
        }

        private void Start(InspeccionTriki runningInspeccion)
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