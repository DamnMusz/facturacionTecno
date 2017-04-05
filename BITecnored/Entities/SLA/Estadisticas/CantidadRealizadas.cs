using System;
using System.Collections.Generic;
using NHibernate;
using BITecnored.Model.DB.Querys;
using BITecnored.Model;
using NHibernate.Transform;
using System.Diagnostics;

namespace BITecnored.Entities.SLA.Estadisticas
{
    public class CantidadRealizadas : Entity
    {
        public virtual int id { get; set; }
        public virtual string aseguradora_alias { get; set; }
        public virtual string inspector_triki { get; set; }
        public virtual string taller_triki { get; set; }
        public virtual long mes1 { get; set; }
        public virtual long mes2 { get; set; }
        public virtual long mes3 { get; set; }
        public virtual long mes4 { get; set; }
        public virtual long mes5 { get; set; }
        public virtual long mes6 { get; set; }
        public virtual double proporcion_trimestre_1 { get; set; }
        public virtual double proporcion_trimestre_2 { get; set; }
        public virtual double proporcion_semestre { get; set; }

        public override IList<Entity> DoRead(ISession session)
        {
            IQueryOver<CantidadRealizadas> queryOver = session.QueryOver<CantidadRealizadas>();
            return new List<Entity>(queryOver.List<CantidadRealizadas>());
        }

        public override void Write()
        {
            throw new NotImplementedException();
        }
        public virtual IList<CantidadRealizadas> GetResumenSemestralPorAseguradora(DateTime periodo)
        {
            DateTime periodo_1 = periodo.AddMonths(-5);
            DateTime periodo_2 = periodo.AddMonths(-4);
            DateTime periodo_3 = periodo.AddMonths(-3);
            DateTime periodo_4 = periodo.AddMonths(-2);
            DateTime periodo_5 = periodo.AddMonths(-1);
            DateTime periodo_6 = periodo;
            string query =
            "select aseguradora_alias, " +
            "sum(case when periodo = "+ Int32.Parse(Utils.MesAnioToNumberFormat(periodo_1)) + " then 1 else 0 end) as mes1, " +
            "sum(case when periodo = "+ Int32.Parse(Utils.MesAnioToNumberFormat(periodo_2)) + " then 1 else 0 end) as mes2, " +
            "sum(case when periodo = "+ Int32.Parse(Utils.MesAnioToNumberFormat(periodo_3)) + " then 1 else 0 end) as mes3, " +
            "sum(case when periodo = "+ Int32.Parse(Utils.MesAnioToNumberFormat(periodo_4)) + " then 1 else 0 end) as mes4, " +
            "sum(case when periodo = "+ Int32.Parse(Utils.MesAnioToNumberFormat(periodo_5)) + " then 1 else 0 end) as mes5, " +
            "sum(case when periodo = "+ Int32.Parse(Utils.MesAnioToNumberFormat(periodo_6)) + " then 1 else 0 end) as mes6, " +

            "(sum(case when periodo = " + Int32.Parse(Utils.MesAnioToNumberFormat(periodo_1)) + " then 1 else 0 end) +" +
            "sum(case when periodo = " + Int32.Parse(Utils.MesAnioToNumberFormat(periodo_2)) + " then 1 else 0 end) +" +
            "sum(case when periodo = " + Int32.Parse(Utils.MesAnioToNumberFormat(periodo_3)) + " then 1 else 0 end)) as proporcion_trimestre_1, " +

            "(sum(case when periodo = " + Int32.Parse(Utils.MesAnioToNumberFormat(periodo_4)) + " then 1 else 0 end) +" +
            "sum(case when periodo = " + Int32.Parse(Utils.MesAnioToNumberFormat(periodo_5)) + " then 1 else 0 end) +" +
            "sum(case when periodo = " + Int32.Parse(Utils.MesAnioToNumberFormat(periodo_6)) + " then 1 else 0 end)) as proporcion_trimestre_2 " +
            
            "from sla " +
            "where aseguradora_alias != 'ZZZ' " +
            "group by aseguradora_alias " +
            "order by aseguradora_alias";

            ISession session = new Hibernate().ConfigureSession(this);
            IQuery sqlQuery =
              session.CreateSQLQuery(query).
              AddScalar("aseguradora_alias", NHibernateUtil.String).
              AddScalar("mes1", NHibernateUtil.Int64).
              AddScalar("mes2", NHibernateUtil.Int64).
              AddScalar("mes3", NHibernateUtil.Int64).
              AddScalar("mes4", NHibernateUtil.Int64).
              AddScalar("mes5", NHibernateUtil.Int64).
              AddScalar("mes6", NHibernateUtil.Int64).
              AddScalar("proporcion_trimestre_1", NHibernateUtil.Double).
              AddScalar("proporcion_trimestre_2", NHibernateUtil.Double).
              SetResultTransformer(Transformers.AliasToBean(typeof(CantidadRealizadas)));
            
            IList<CantidadRealizadas> res = sqlQuery.List<CantidadRealizadas>();
            session.Close();

            foreach (CantidadRealizadas row in res)
                if (row.proporcion_trimestre_1 == 0)
                    row.proporcion_semestre = Math.Round(row.proporcion_trimestre_2 * 100,2);
                else
                    row.proporcion_semestre = (Math.Round((row.proporcion_trimestre_2 / row.proporcion_trimestre_1 * 100) - 100, 2));

            return res;
        }

        public virtual IList<CantidadRealizadas> GetResumenSemestralPorCentro(DateTime periodo)
        {
            DateTime periodo_1 = periodo.AddMonths(-5);
            DateTime periodo_2 = periodo.AddMonths(-4);
            DateTime periodo_3 = periodo.AddMonths(-3);
            DateTime periodo_4 = periodo.AddMonths(-2);
            DateTime periodo_5 = periodo.AddMonths(-1);
            DateTime periodo_6 = periodo;
            string query =
            "select taller_triki, " +
            "sum(case when periodo = " + Int32.Parse(Utils.MesAnioToNumberFormat(periodo_1)) + " then 1 else 0 end) as mes1, " +
            "sum(case when periodo = " + Int32.Parse(Utils.MesAnioToNumberFormat(periodo_2)) + " then 1 else 0 end) as mes2, " +
            "sum(case when periodo = " + Int32.Parse(Utils.MesAnioToNumberFormat(periodo_3)) + " then 1 else 0 end) as mes3, " +
            "sum(case when periodo = " + Int32.Parse(Utils.MesAnioToNumberFormat(periodo_4)) + " then 1 else 0 end) as mes4, " +
            "sum(case when periodo = " + Int32.Parse(Utils.MesAnioToNumberFormat(periodo_5)) + " then 1 else 0 end) as mes5, " +
            "sum(case when periodo = " + Int32.Parse(Utils.MesAnioToNumberFormat(periodo_6)) + " then 1 else 0 end) as mes6, " +
                        "(sum(case when periodo = " + Int32.Parse(Utils.MesAnioToNumberFormat(periodo_1)) + " then 1 else 0 end) +" +
            "sum(case when periodo = " + Int32.Parse(Utils.MesAnioToNumberFormat(periodo_2)) + " then 1 else 0 end) +" +
            "sum(case when periodo = " + Int32.Parse(Utils.MesAnioToNumberFormat(periodo_3)) + " then 1 else 0 end)) as proporcion_trimestre_1, " +

            "(sum(case when periodo = " + Int32.Parse(Utils.MesAnioToNumberFormat(periodo_4)) + " then 1 else 0 end) +" +
            "sum(case when periodo = " + Int32.Parse(Utils.MesAnioToNumberFormat(periodo_5)) + " then 1 else 0 end) +" +
            "sum(case when periodo = " + Int32.Parse(Utils.MesAnioToNumberFormat(periodo_6)) + " then 1 else 0 end)) as proporcion_trimestre_2 " +
            "from sla " +
            "where taller_triki != 'DOMICILIO' " +
            "and taller_triki IS NOT NULL " +
            "and taller_triki != '' " +
            "group by taller_triki " +
            "having count(case when periodo >= "+ Int32.Parse(Utils.MesAnioToNumberFormat(periodo_1)) + " and periodo <= "+ Int32.Parse(Utils.MesAnioToNumberFormat(periodo_6)) + " then 1 end) > 0 " +
            "order by taller_triki";

            Debug.WriteLine(query);

            ISession session = new Hibernate().ConfigureSession(this);
            IQuery sqlQuery =
              session.CreateSQLQuery(query).
              AddScalar("taller_triki", NHibernateUtil.String).
              AddScalar("mes1", NHibernateUtil.Int64).
              AddScalar("mes2", NHibernateUtil.Int64).
              AddScalar("mes3", NHibernateUtil.Int64).
              AddScalar("mes4", NHibernateUtil.Int64).
              AddScalar("mes5", NHibernateUtil.Int64).
              AddScalar("mes6", NHibernateUtil.Int64).
              AddScalar("proporcion_trimestre_1", NHibernateUtil.Double).
              AddScalar("proporcion_trimestre_2", NHibernateUtil.Double).
              SetResultTransformer(Transformers.AliasToBean(typeof(CantidadRealizadas)));

            IList<CantidadRealizadas> res = sqlQuery.List<CantidadRealizadas>();
            session.Close();

            foreach (CantidadRealizadas row in res)
                if (row.proporcion_trimestre_1 == 0)
                    row.proporcion_semestre = Math.Round(row.proporcion_trimestre_2 * 100, 2);
                else
                    row.proporcion_semestre = (Math.Round((row.proporcion_trimestre_2 / row.proporcion_trimestre_1 * 100) - 100, 2));
            return res;
        }

        public virtual IList<CantidadRealizadas> GetResumenSemestralPorInspector(DateTime periodo)
        {
            DateTime periodo_1 = periodo.AddMonths(-5);
            DateTime periodo_2 = periodo.AddMonths(-4);
            DateTime periodo_3 = periodo.AddMonths(-3);
            DateTime periodo_4 = periodo.AddMonths(-2);
            DateTime periodo_5 = periodo.AddMonths(-1);
            DateTime periodo_6 = periodo;
            string query =
            "select inspector_triki, " +
            "sum(case when periodo = " + Int32.Parse(Utils.MesAnioToNumberFormat(periodo_1)) + " then 1 else 0 end) as mes1, " +
            "sum(case when periodo = " + Int32.Parse(Utils.MesAnioToNumberFormat(periodo_2)) + " then 1 else 0 end) as mes2, " +
            "sum(case when periodo = " + Int32.Parse(Utils.MesAnioToNumberFormat(periodo_3)) + " then 1 else 0 end) as mes3, " +
            "sum(case when periodo = " + Int32.Parse(Utils.MesAnioToNumberFormat(periodo_4)) + " then 1 else 0 end) as mes4, " +
            "sum(case when periodo = " + Int32.Parse(Utils.MesAnioToNumberFormat(periodo_5)) + " then 1 else 0 end) as mes5, " +
            "sum(case when periodo = " + Int32.Parse(Utils.MesAnioToNumberFormat(periodo_6)) + " then 1 else 0 end) as mes6, " +
                        "(sum(case when periodo = " + Int32.Parse(Utils.MesAnioToNumberFormat(periodo_1)) + " then 1 else 0 end) +" +
            "sum(case when periodo = " + Int32.Parse(Utils.MesAnioToNumberFormat(periodo_2)) + " then 1 else 0 end) +" +
            "sum(case when periodo = " + Int32.Parse(Utils.MesAnioToNumberFormat(periodo_3)) + " then 1 else 0 end)) as proporcion_trimestre_1, " +

            "(sum(case when periodo = " + Int32.Parse(Utils.MesAnioToNumberFormat(periodo_4)) + " then 1 else 0 end) +" +
            "sum(case when periodo = " + Int32.Parse(Utils.MesAnioToNumberFormat(periodo_5)) + " then 1 else 0 end) +" +
            "sum(case when periodo = " + Int32.Parse(Utils.MesAnioToNumberFormat(periodo_6)) + " then 1 else 0 end)) as proporcion_trimestre_2 " +
            "from sla " +
            "where taller_triki = 'DOMICILIO' " +
            "and inspector_triki IS NOT NULL " +
            "and inspector_triki != '' " +
            "group by inspector_triki " +
            "having count(case when periodo >= " + Int32.Parse(Utils.MesAnioToNumberFormat(periodo_1)) + " and periodo <= " + Int32.Parse(Utils.MesAnioToNumberFormat(periodo_6)) + " then 1 end) > 0 " +
            "order by inspector_triki";

            ISession session = new Hibernate().ConfigureSession(this);
            IQuery sqlQuery =
              session.CreateSQLQuery(query).
              AddScalar("inspector_triki", NHibernateUtil.String).
              AddScalar("mes1", NHibernateUtil.Int64).
              AddScalar("mes2", NHibernateUtil.Int64).
              AddScalar("mes3", NHibernateUtil.Int64).
              AddScalar("mes4", NHibernateUtil.Int64).
              AddScalar("mes5", NHibernateUtil.Int64).
              AddScalar("mes6", NHibernateUtil.Int64).
              AddScalar("proporcion_trimestre_1", NHibernateUtil.Double).
              AddScalar("proporcion_trimestre_2", NHibernateUtil.Double).
              SetResultTransformer(Transformers.AliasToBean(typeof(CantidadRealizadas)));

            IList<CantidadRealizadas> res = sqlQuery.List<CantidadRealizadas>();
            session.Close();

            foreach (CantidadRealizadas row in res)
                if (row.proporcion_trimestre_1 == 0)
                    row.proporcion_semestre = Math.Round(row.proporcion_trimestre_2 * 100, 2);
                else
                    row.proporcion_semestre = (Math.Round((row.proporcion_trimestre_2 / row.proporcion_trimestre_1 * 100) - 100, 2));
            return res;
        }
    }
}