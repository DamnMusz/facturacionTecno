using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate;
using BITecnored.Model;
using System.Diagnostics;
using NHibernate.Transform;

namespace BITecnored.Entities.FacturacionCentros
{
    public class TarifarioReducido
    {
        public class Tarifa
        {
            public virtual long tarifa_id { get; set; }
            public virtual string tarifa_nombre { get; set; }
            public virtual float monto { get; set; }
            public virtual string descripcion { get; set; }
            public virtual string color { get; set; }
            public virtual int? periodo_desde { get; set; }
            public virtual int? periodo_hasta { get; set; }
        }

        public class Afinidad
        {
            public virtual long id { get; set; }
            public virtual string value { get; set; }
        }

        public virtual long id { get; set; }
        public virtual string nombre { get; set; }
        public virtual Afinidad afinidad { get; set; }
        public virtual List<Tarifa> tarifas { get; set; }

        public TarifarioReducido(Tarifario prototype, List<Tarifario> t) {
            id = prototype.id;
            nombre = prototype.nombre;
            afinidad.id = prototype.afinidad_id;
            afinidad.value = prototype.afinidad_desc;
            
        }
    }
}