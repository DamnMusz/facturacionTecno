using System;
using System.Collections.Generic;
using NHibernate;
using BITecnored.Model.DB.Querys;
using BITecnored.Model;
using System.Collections;
using NHibernate.Transform;

namespace BITecnored.Entities.SLA.Estadisticas
{
    public class SolicitudesAgenda : Entity
    {
        public virtual int id { get; set; }
        public virtual string aseguradora_alias { get; set; }
        public virtual long total_count { get; set; }
        public virtual long realiz_count { get; set; }

        public override IList<Entity> DoRead(ISession session)
        {
            IQueryOver<SolicitudesAgenda> queryOver = session.QueryOver<SolicitudesAgenda>();
            return new List<Entity>(queryOver.List<SolicitudesAgenda>());
        }

        public virtual IList<SolicitudesAgenda> GetProporcionesPorAseguradora(DateTime periodo, int provincia_id)
        {
            string query = "SELECT aseguradora_alias as aseguradora_alias, count(*) as total_count, count(case when estado = 5 then 1 end) as realiz_count"
            + " FROM sla_agenda "
            + ((provincia_id != -1) ? " INNER JOIN \"Inspecciones\" ON \"Inspecciones\".\"Id\" = sla_agenda.id_agenda" : "")
            + " WHERE periodo = " + Int32.Parse(Utils.MesAnioToNumberFormat(periodo))
            + ((provincia_id != -1) ? (" AND \"Inspecciones\".\"Provincia\" = " + provincia_id) : "")
            + " GROUP BY aseguradora_alias "
            + " ORDER BY aseguradora_alias "
            ;

            ISession session = new Hibernate().ConfigureSession(this);
            IQuery sqlQuery =
              session.CreateSQLQuery(query).
              AddScalar("aseguradora_alias", NHibernateUtil.String).
              AddScalar("total_count", NHibernateUtil.Int64).
              AddScalar("realiz_count", NHibernateUtil.Int64).SetResultTransformer(
              Transformers.AliasToBean(typeof(SolicitudesAgenda)));

            IList<SolicitudesAgenda> solicitudes = sqlQuery.List<SolicitudesAgenda>();
            session.Close();
            return solicitudes;
        }

        public override void Write()
        {
            throw new NotImplementedException();
        }
    }
}