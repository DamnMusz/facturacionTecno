using System.Collections.Generic;
using NHibernate;
using NHibernate.Criterion;
using System;
using BITecnored.Model;

namespace BITecnored.Entities.SLA
{
    public class InspeccionSLA_AnexoNoCasadas : Entity
    {
        public virtual int id { get; set; }
        public virtual int? id_agenda { get; set; }
        public virtual DateTime? fecha_solicitud { get; set; }
        public virtual DateTime? fecha_inspeccion { get; set; }
        public virtual int cierre_fact { get; set; }

        private DateTime? periodo = null;

        public InspeccionSLA_AnexoNoCasadas() { }
        
        public virtual InspeccionSLA_AnexoNoCasadas Completar()
        {
            if(fecha_inspeccion == null)
            {
                fecha_solicitud = null;
            }
            else
            {
                DateTime aux = ((DateTime)fecha_inspeccion).AddDays(-1);
                fecha_solicitud = aux;
            }
            return this;
        }

        public virtual InspeccionSLA_AnexoNoCasadas SetPeriodoFacturacion(DateTime date)
        {
            periodo = date;
            return this;
        }

        public override IList<Entity> DoRead(ISession session)
        {
            Conjunction restrictions = Restrictions.Conjunction();
            if (periodo != null)
                restrictions.Add<InspeccionSLA_AnexoNoCasadas>(insp => insp.cierre_fact == Int32.Parse(Utils.MesAnioToNumberFormat(periodo.GetValueOrDefault())));

            Disjunction restrictions_or = Restrictions.Disjunction();
            restrictions_or.Add<InspeccionSLA_AnexoNoCasadas>(insp => insp.id_agenda == 0);
            restrictions_or.Add<InspeccionSLA_AnexoNoCasadas>(insp => insp.id_agenda != null);
            restrictions.Add(restrictions_or);

            IQueryOver<InspeccionSLA_AnexoNoCasadas> queryOver = session.QueryOver<InspeccionSLA_AnexoNoCasadas>()
                .Where(restrictions)
            ;

            List<Entity> res = new List<Entity>(queryOver.List<InspeccionSLA_AnexoNoCasadas>());
            return res;
        }
    }
}