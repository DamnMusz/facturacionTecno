using BITecnored.Model;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;

namespace BITecnored.Entities.SLA
{
    public class InspeccionSLA_DatosAgenda : Entity
    {
        public virtual int id_triki { get; set; }
        public virtual int? id_agenda { get; set; }
        public virtual int cierre_fact { get; set; }
        public virtual DateTime? fecha_inspeccion { get; set; }
        public virtual InspeccionAgenda_DatosSLA datos_agenda { get; set; }
        private bool get_married = false;

        private DateTime periodo = new DateTime();

        public virtual InspeccionSLA_DatosAgenda SetPeriodoFacturacion(DateTime date)
        {
            periodo = date;
            return this;
        }

        public virtual InspeccionSLA_DatosAgenda SelectMarried(bool value)
        {
            get_married = value;
            return this;
        }

        public override IList<Entity> DoRead(ISession session)
        {
            Conjunction restrictions = Restrictions.Conjunction();
            if (periodo != new DateTime())
                restrictions.Add<InspeccionSLA_DatosAgenda>(insp => insp.cierre_fact == Int32.Parse(Utils.MesAnioToNumberFormat(periodo)));
            if(get_married)
            {
                Disjunction restrictions_or = Restrictions.Disjunction();
                restrictions_or.Add<InspeccionSLA_DatosAgenda>(insp => insp.id_agenda != 0);
                restrictions_or.Add<InspeccionSLA_DatosAgenda>(insp => insp.id_agenda != null);
                restrictions.Add(restrictions_or);
            } else
            {
                Disjunction restrictions_or = Restrictions.Disjunction();
                restrictions_or.Add<InspeccionSLA_DatosAgenda>(insp => insp.id_agenda == 0);
                restrictions_or.Add<InspeccionSLA_DatosAgenda>(insp => insp.id_agenda != null);
                restrictions.Add(restrictions_or);
            }

            IQueryOver<InspeccionSLA_DatosAgenda> queryOver = session.QueryOver<InspeccionSLA_DatosAgenda>()
                .Where(restrictions)
            ;
            
            List<Entity> res = new List<Entity>(queryOver.List<InspeccionSLA_DatosAgenda>());
            return res;
        }

        override public string ToString()
        {
            return "id_triki: " + id_triki + "\n"
                + "id_agenda: " + id_agenda + "\n"
                + datos_agenda.ToString()
                ;
        }

        public override void Write(IList<Entity> elementos)
        {
            throw new DontWriteOnInspeccionesException();
        }

        public override void Update(IList<Entity> elementos)
        {
            throw new DontWriteOnInspeccionesException();
        }

        public override void WriteOrUpdate(IList<Entity> elementos)
        {
            throw new DontWriteOnInspeccionesException();
        }
    }
}