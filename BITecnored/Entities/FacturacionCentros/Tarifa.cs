using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate;
using BITecnored.Model;
using System.Diagnostics;

namespace BITecnored.Entities.FacturacionCentros
{
    public class Tarifa : Entity
    {
        public virtual long id { get; set; }
        public virtual string nombre { get; set; }
        public virtual float tarifa_por_inspeccion { get; set; }
        public virtual int? periodo_desde { get; set; }
        public virtual int? periodo_hasta { get; set; }
        public virtual string color_vista { get; set; }
        public virtual string observacion { get; set; }

        public virtual DateTime fecha_creacion { get; set; }
        public virtual string usuario_creacion { get; set; }

        public Tarifa() { }
        
        public override IList<Entity> DoRead(ISession session)
        {
            IQueryOver<Tarifa> queryOver = session.QueryOver<Tarifa>()
                .Where(tarifa => tarifa.periodo_hasta >= int.Parse(Utils.MesAnioToNumberFormat(DateTime.Today)));
            return new List<Entity>(queryOver.List<Tarifa>());
        }

        public override void Write()
        {
            throw new NotImplementedException();
        }
    }
}