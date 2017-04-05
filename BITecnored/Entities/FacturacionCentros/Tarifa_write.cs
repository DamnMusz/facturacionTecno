using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate;
using BITecnored.Model;
using System.Diagnostics;

namespace BITecnored.Entities.FacturacionCentros
{
    public class Tarifa_write : Entity
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

        public Tarifa_write() { }
        
        public override IList<Entity> DoRead(ISession session)
        {
            throw new NotImplementedException();
        }

        public override void Write()
        {
            base.Write();
        }
    }
}