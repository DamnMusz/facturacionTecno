using BITecnored.Entities;
using BITecnored.Entities.Inspeccion;
using BITecnored.Entities.SLA;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace BITecnored.Model.SLA
{
    public class BaseSLAGenerator
    {

        public enum Estado { IDLE, RUNNING }

        public Estado estado = Estado.IDLE;
        public DateTime startTime;
        public DateTime periodo_corriendo;
        BaseSLATriki baseTriki = null;
        BaseSLAAgenda baseAgenda = null;
        private static BaseSLAGenerator _instance = null;

        private BaseSLAGenerator() { }

        public static BaseSLAGenerator GetInstance()
        {
            if (_instance == null)
                _instance = new BaseSLAGenerator();
            return _instance;
        }

        private void CreateBases()
        {
            if (baseTriki == null)
                baseTriki = new BaseSLATriki();
            if (baseAgenda == null)
                baseAgenda = new BaseSLAAgenda();
        }

        public void UpdateBase(DateTime periodo, string usuario)
        {
            CreateBases();
            List<InspeccionTriki> inspecciones = null;
            Start(periodo);
            try
            {
                if (baseTriki != null)
                    inspecciones = baseTriki.GetInspecciones(periodo);
                if (baseAgenda != null && inspecciones != null)
                {
                    baseAgenda.Insert(inspecciones, periodo, usuario);
                    baseAgenda.Complete(periodo);
                }
            } catch(Exception e)
            {
                Stop();
                throw e;
            }
            Stop();
        }

        public void GenerateNoRealizadas(DateTime periodo)
        {
            CreateBases();
            List<NoRealizadasAgenda> no_realizadas = new NoRealizadasAgenda().SetPeriodoBusqueda(periodo).Read().Cast<NoRealizadasAgenda>().ToList();
            List<Entity> insert_list = new List<Entity>();
            foreach(NoRealizadasAgenda una_no_realizada in no_realizadas)
                insert_list.Add(new SLA_Agenda(una_no_realizada, periodo));
            SLA_Agenda.WriteAll(insert_list);
        }

        public bool ExistsPrevious(DateTime periodo)
        {
            return new BaseSLAAgenda().ExistsPrevious(periodo);
        }

        public bool ExistsPrevious(DateTime periodo, string aseguradora_alias)
        {
            return new BaseSLAAgenda().ExistsPrevious(periodo, aseguradora_alias);
        }

        public void RemoveBase(DateTime periodo)
        {
            new BaseSLAAgenda().Remove(periodo);
        }

        private void Start(DateTime periodo_a_ejecutar)
        {
            SetEstado(Estado.RUNNING);
            startTime = DateTime.Now;
            periodo_corriendo = periodo_a_ejecutar;
        }

        private void Stop()
        {
            SetEstado(Estado.IDLE);
            startTime = new DateTime();
            periodo_corriendo = new DateTime();
        }

        public void Cancel()
        {
            if (baseTriki != null)
            {
                baseTriki.Cancel();
                baseTriki = null;
            }
            if (baseAgenda != null)
            {
                baseAgenda.Cancel();
                baseAgenda = null;
            }
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