using System;
using System.Collections.Generic;
using NHibernate;
using NHibernate.Criterion;
using BITecnored.Model;

namespace BITecnored.Entities.Inspeccion
{
    public class InspeccionTriki : Entity
    {
        public virtual int id { get; set; }
        public virtual string aseguradora { get; set; }
        public virtual string inspector { get; set; }
        public virtual string dominio { get; set; }
        public virtual DateTime? fec_envio { get; set; }
        public virtual string tipo_inspeccion { get; set; }
        public virtual InspeccionTrikiOtros otros { get; set; }
        public virtual InspeccionTrikiData data { get; set; }

        private DateTime periodo = new DateTime();

        public InspeccionTriki()
        {
            otros = new InspeccionTrikiOtros();
            data = new InspeccionTrikiData();
        }

        public virtual InspeccionTriki SetPeriodoFacturacion(DateTime date)
        {
            periodo = date;
            return this;
        }

        public override IList<Entity> DoRead(ISession session)
        {
            Conjunction restrictions = Restrictions.Conjunction();
            if (periodo != new DateTime())
                restrictions.Add<InspeccionTrikiOtros>(otros => otros.cierre_liq_prestador == Int32.Parse(Utils.MesAnioToNumberFormat(periodo)));

            IQueryOver<InspeccionTriki> queryOver = session.QueryOver<InspeccionTriki>()
                .JoinQueryOver(insp => insp.otros)
                .Where(restrictions)
                .OrderBy(inspeccion => inspeccion.id).Asc;

            List<Entity> res = new List<Entity>(queryOver.List<InspeccionTriki>());
            return res;
        }
        

        public override string ToString()
        {
            return "id: " + id + "\n" + "aseguradora: " + aseguradora + "\n" + "inspector: " + inspector + "\n"
                + "dominio: " + dominio + "\n" + "fec_envio: " + fec_envio + "\n" + "tipo_inspeccion: " + tipo_inspeccion + "\n"
                + ((data != null) ? data.ToString() : "")
                + ((otros != null) ? otros.ToString() : "");
        }

        public override string GetSessionConfigFile()
        {
            return "BITecnored.HibernateTriki.cfg.xml";
        }

        public override void Write()
        {
            throw new DontWriteOnTrikiException();
        }

        public override void WriteOrUpdate()
        {
            throw new DontWriteOnTrikiException();
        }

        public override void Update()
        {
            throw new DontWriteOnTrikiException();
        }

        public override void Write(IList<Entity> elements)
        {
            throw new DontWriteOnTrikiException();
        }

        public override void WriteOrUpdate(IList<Entity> elements)
        {
            throw new DontWriteOnTrikiException();
        }

        public override void Update(IList<Entity> elements)
        {
            throw new DontWriteOnTrikiException();
        }
    }
}