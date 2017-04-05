using System;
using System.Collections.Generic;
using NHibernate;
using BITecnored.Model.DB.Querys;
using BITecnored.Model;
using NHibernate.Transform;

namespace BITecnored.Entities.SLA.Estadisticas
{
    public class Fotos : Entity
    {
        public virtual int id { get; set; }
        public virtual string aseguradora_alias { get; set; }
        public virtual double prom_fotos { get; set; }
        public virtual long mas_de_4_fotos { get; set; }
        
        public override IList<Entity> DoRead(ISession session)
        {
            IQueryOver<Fotos> queryOver = session.QueryOver<Fotos>();
            return new List<Entity>(queryOver.List<Fotos>());
        }

        public override void Write()
        {
            throw new NotImplementedException();
        }
        public virtual IList<Fotos> GetResumenPorAseguradora(DateTime periodo)
        {
            string query =
            "SELECT aseguradora_alias as aseguradora_alias, ROUND(AVG(fotos_cantidad), 1) as prom_fotos, count(case when fotos_cantidad > 4 then 1 end) * 100 / count(*) as mas_de_4_fotos "
            + "FROM sla "
            + " WHERE periodo = " + Int32.Parse(Utils.MesAnioToNumberFormat(periodo))
            + " GROUP BY aseguradora_alias "
            + " ORDER BY aseguradora_alias "
            ;
            
            ISession session = new Hibernate().ConfigureSession(this);
            IQuery sqlQuery =
              session.CreateSQLQuery(query).
              AddScalar("aseguradora_alias", NHibernateUtil.String).
              AddScalar("prom_fotos", NHibernateUtil.Double).
              AddScalar("mas_de_4_fotos", NHibernateUtil.Int64).SetResultTransformer(
              Transformers.AliasToBean(typeof(Fotos)));

            IList<Fotos> fotos = sqlQuery.List<Fotos>();
            session.Close();
            return fotos;
        }
    }
}