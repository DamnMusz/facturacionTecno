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
    public class Tarifario : Entity
    {
        public virtual long id { get; set; }
        public virtual string nombre { get; set; }

        public virtual long afinidad_id { get; set; }
        public virtual string afinidad_desc { get; set; }

        public virtual long tarifa_id { get; set; }
        public virtual string tarifa_nombre { get; set; }
        public virtual float monto { get; set; }

        public virtual string descripcion { get; set; }
        public virtual string color { get; set; }
        public virtual int? periodo_desde { get; set; }
        public virtual int? periodo_hasta { get; set; }

        public Tarifario() { }
        
        public override IList<Entity> DoRead(ISession session)
        {
            string query =
            "select \"Centros\".\"Centros_Id\" as id, \"Centros\".nombre_fantasia as nombre, afinidad_tarifaria.id as afinidad_id,"
            + " afinidad_tarifaria.descripcion as afinidad_desc, tarifas_centros.id as tarifa_id, tarifas_centros.nombre as tarifa_nombre, tarifas_centros.tarifa_por_inspeccion as monto,"
            +" tarifas_centros.observacion as descripcion, tarifas_centros.color_vista as color, tarifas_centros.periodo_desde, tarifas_centros.periodo_hasta"
            + " from \"Centros\""
            + " inner join rela_tarifa_centro on \"Centros\".\"Centros_Id\" = rela_tarifa_centro.id_centro"
            + " inner join afinidad_tarifaria on \"Centros\".id_afinidad_tarifaria = afinidad_tarifaria.id"
            + " inner join tarifas_centros on rela_tarifa_centro.id_tarifa = tarifas_centros.id"
            + " order by \"Centros\".\"Centros_Id\"";

            IQuery sqlQuery =
              session.CreateSQLQuery(query)
              .AddScalar("id", NHibernateUtil.Int64)
              .AddScalar("nombre", NHibernateUtil.String)
              .AddScalar("afinidad_id", NHibernateUtil.Int64)
              .AddScalar("afinidad_desc", NHibernateUtil.String)
              .AddScalar("tarifa_id", NHibernateUtil.Int64)
              .AddScalar("tarifa_nombre", NHibernateUtil.String)
              .AddScalar("monto", NHibernateUtil.Single)
              .AddScalar("descripcion", NHibernateUtil.String)
              .AddScalar("color", NHibernateUtil.String)
              .AddScalar("periodo_desde", NHibernateUtil.Int32)
              .AddScalar("periodo_hasta", NHibernateUtil.Int32)
              .SetResultTransformer(Transformers.AliasToBean(typeof(Tarifario)));
            IList<Entity> res = new List<Entity>(sqlQuery.List<Tarifario>());
            return res;
        }

        public override void Write()
        {
            throw new NotImplementedException();
        }
    }
}