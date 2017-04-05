using System;
using System.Collections.Generic;
using NHibernate;
using BITecnored.Model.DB.Querys;
using BITecnored.Model;
using System.Collections;
using NHibernate.Transform;
using System.Diagnostics;

namespace BITecnored.Entities.SLA.Estadisticas
{
    public class NoRealizadas : Entity
    {
        public virtual int id { get; set; }
        public virtual string aseguradora_alias { get; set; }
        public virtual long realizadas { get; set; }
        public virtual long sin_efecto { get; set; }

        public override IList<Entity> DoRead(ISession session)
        {
            IQueryOver<NoRealizadas> queryOver = session.QueryOver<NoRealizadas>();
            return new List<Entity>(queryOver.List<NoRealizadas>());
        }

        public virtual IList<NoRealizadas> GetProporcionesPorAseguradora(DateTime periodo)
        {
            string query =
             "select aseguradora_alias as aseguradora_alias, count(case when sla_agenda.estado = 5 then 1 end)*100 / count(*) as realizadas, count(case when sla_agenda.estado = 6 then 1 end)*100 / count(*) as sin_efecto "
            + "from historico "
            +"inner join \"Motivos\" on \"Motivos\".motivos_codigo = historico.motivo "
            +"inner join subestados on historico.motivo = subestados.cod "
            +"inner join sla_agenda on historico.agenda_id = sla_agenda.id_agenda "
            +"where 1 = 1 "
            +"and sla_agenda.periodo = " + Int32.Parse(Utils.MesAnioToNumberFormat(periodo)) + " " 
            +"AND subestados.estado = 4 "
            + "GROUP BY aseguradora_alias"
            ;

            ISession session = new Hibernate().ConfigureSession(this);
            IQuery sqlQuery =
              session.CreateSQLQuery(query).
              AddScalar("aseguradora_alias", NHibernateUtil.String).
              AddScalar("realizadas", NHibernateUtil.Int64).
              AddScalar("sin_efecto", NHibernateUtil.Int64).SetResultTransformer(
              Transformers.AliasToBean(typeof(NoRealizadas)));

            IList<NoRealizadas> solicitudes = sqlQuery.List<NoRealizadas>();
            session.Close();
            return solicitudes;
        }

        public override void Write()
        {
            throw new NotImplementedException();
        }
    }
}