using BITecnored.Entities.Basic;
using BITecnored.Model;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace BITecnored.Entities.SLA
{
    public class NoRealizadasAgenda : Entity
    {
        public virtual int id_agenda { get; set; }
        public virtual int estado { get; set; }
        public virtual int subestado { get; set; }
        public virtual DateTime fecha_solicitud { get; set; }
        public virtual Aseguradora aseguradora { get; set; }
        public virtual string motivo { get; set; }

        public virtual DateTime? periodoBusqueda { get; set; }

        public virtual NoRealizadasAgenda SetPeriodoBusqueda(DateTime date)
        {
            periodoBusqueda = date;
            return this;
        }

        public override IList<Entity> DoRead(ISession session)
        {
            Conjunction restriction = Restrictions.Conjunction();
            if (periodoBusqueda != null)
            {
                DateTime firstDay = Utils.FirstDayInMonth(periodoBusqueda.GetValueOrDefault());
                DateTime lastDay = Utils.LastDayInMonth(periodoBusqueda.GetValueOrDefault());
                restriction.Add<NoRealizadasAgenda>(insp => insp.fecha_solicitud >= firstDay);
                restriction.Add<NoRealizadasAgenda>(insp => insp.fecha_solicitud <= lastDay);
            }
            Disjunction restriction_or = Restrictions.Disjunction();
            restriction_or.Add<NoRealizadasAgenda>(insp => insp.estado == 5);
            restriction_or.Add<NoRealizadasAgenda>(insp => insp.estado == 6);
            restriction.Add(restriction_or);
            restriction.Add<NoRealizadasAgenda>(insp => insp.aseguradora.id != ASEGURADORA_PRUEBA);

            IQueryOver<NoRealizadasAgenda> queryOver = session.QueryOver<NoRealizadasAgenda>()
                .Where(restriction)
                .JoinQueryOver(no_realizada => no_realizada.aseguradora);
            IList<NoRealizadasAgenda> res = queryOver.List<NoRealizadasAgenda>();
            foreach (NoRealizadasAgenda one in res)
                NHibernateUtil.Initialize(one);
            return new List<Entity>(res);
        }

        public override void Write(IList<Entity> elements)
        {
            throw new DontWriteOnInspeccionesException();
        }

        public override void Write()
        {
            throw new DontWriteOnInspeccionesException();
        }
    }
}