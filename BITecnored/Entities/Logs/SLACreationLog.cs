using System.Collections.Generic;
using NHibernate;
using System;

namespace BITecnored.Entities.Logs
{
    public class SLACreationLog : Entity
    {
        public virtual int cierre_prestador { get; set; }
        public virtual DateTime fecha_calculo_sla { get; set; }
        public virtual string usuario_calculo_sla { get; set; }

        public SLACreationLog() { }

        public SLACreationLog(int cierre_prestador, DateTime fecha_calculo_sla, string usuario_calculo_sla)
        {
            this.cierre_prestador = cierre_prestador;
            this.fecha_calculo_sla = fecha_calculo_sla;
            this.usuario_calculo_sla = usuario_calculo_sla;
        }

        public override IList<Entity> DoRead(ISession session)
        {
            IQueryOver<SLACreationLog> queryOver = session.QueryOver<SLACreationLog>()
                .OrderBy(inspeccion => inspeccion.cierre_prestador).Desc
                .Take(1);
            return new List<Entity>(queryOver.List<SLACreationLog>());
        }
    }
}