using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate;
using NHibernate.Criterion;
using BITecnored.Model;
using BITecnored.Model.DB.Querys;
using BITecnored.Entities.Basic;
using System.Diagnostics;

namespace BITecnored.Entities.SLA.Informe
{
    public class FechasGestionInspeccion : Entity
    {
        public virtual int id { get; set; }
        public virtual DateTime fecha_solicitud { get; set; }
        public virtual DateTime fecha_inspeccion { get; set; }
        public virtual DateTime fecha_publicacion { get; set; }
        public virtual int periodo { get; set; }
        public virtual string tipo_inspeccion { get; set; }
        public virtual string tipo_prestador { get; set; }
        public virtual string aseguradora_alias { get; set; }
        public virtual Lugares lugar { get; set; }

        private DateTime? periodo_busqueda = null;
        private string aseguradora_busqueda = null;
        private int? provincia_busqueda = null;

        public virtual FechasGestionInspeccion SetPeriodoBusqueda(DateTime periodo)
        {
            periodo_busqueda = periodo;
            return this;
        }

        public virtual FechasGestionInspeccion SetAseguradoraBusqueda(string aseguradora)
        {
            aseguradora_busqueda = aseguradora;
            return this;
        }

        public virtual FechasGestionInspeccion SetProvinciaBusqueda(int provincia_id)
        {
            provincia_busqueda = provincia_id;
            return this;
        }

        public override IList<Entity> DoRead(ISession session)
        {
            if (reader == null)
            {
                Conjunction restrictions = Restrictions.Conjunction();
                if (periodo_busqueda != null)
                    restrictions.Add<FechasGestionInspeccion>(fechas => fechas.periodo == Int32.Parse(Utils.MesAnioToNumberFormat((periodo_busqueda.GetValueOrDefault()))));
                if (aseguradora_busqueda != null)
                    restrictions.Add<FechasGestionInspeccion>(fechas => fechas.aseguradora_alias == aseguradora_busqueda);

                restrictions.Add<FechasGestionInspeccion>(fechas => fechas.tipo_prestador != BONIFICADO_TECNORED);

                IQueryOver<FechasGestionInspeccion> queryOver;

                Conjunction restrictions2 = Restrictions.Conjunction();
                if (provincia_busqueda != null)
                {
                    restrictions2.Add<Lugares>(lugar => lugar.cod_provincia == provincia_busqueda);
                    queryOver = session.QueryOver<FechasGestionInspeccion>().Where(restrictions).JoinQueryOver(fechas => fechas.lugar).And(restrictions2);
                } else
                {
                    queryOver = session.QueryOver<FechasGestionInspeccion>().Where(restrictions);
                }
                
                List<Entity> res = new List<Entity>(queryOver.List<FechasGestionInspeccion>());
                return res;
            } else
            {
                return reader.DoRead(this, session);
            }
        }

        public class ReaderResumenAseguradoras : ReaderStrategy
        {
            public override IList<Entity> DoRead(Entity caller, ISession session)
            {
                Conjunction restrictions = Restrictions.Conjunction();
                if (((FechasGestionInspeccion)caller).periodo_busqueda != null)
                    restrictions.Add<FechasGestionInspeccion>(insp => insp.periodo == Int32.Parse(Utils.MesAnioToNumberFormat((((FechasGestionInspeccion)caller).periodo_busqueda.GetValueOrDefault()))));
                if (((FechasGestionInspeccion)caller).aseguradora_busqueda != null)
                    restrictions.Add<FechasGestionInspeccion>(insp => insp.aseguradora_alias == ((FechasGestionInspeccion)caller).aseguradora_busqueda);
                restrictions.Add<FechasGestionInspeccion>(insp => insp.tipo_inspeccion == TIPO_PREVIA_AUTO);

                IQueryOver<FechasGestionInspeccion> queryOver = session.QueryOver<FechasGestionInspeccion>()
                    .Where(restrictions);

                List<Entity> res = new List<Entity>(queryOver.List<FechasGestionInspeccion>());
                return res;
            }
        }

        public virtual FechasGestionInspeccion SetReaderResumenPorAseguradora()
        {
            reader = new ReaderResumenAseguradoras();
            return this;
        }
    }
}