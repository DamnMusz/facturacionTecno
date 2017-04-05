using System;
using System.Collections.Generic;
using NHibernate;

namespace BITecnored.Entities.Basic
{
    public class Insp_Provincia : Entity
    {
        public virtual int id_agenda { get; set; }
        public virtual int id_provincia { get; set; }
        public virtual string nombre_provincia { get; set; }
        
        public override IList<Entity> DoRead(ISession session)
        {
            throw new NotImplementedException();
            //IQueryOver<Aseguradora> queryOver = session.QueryOver<Aseguradora>()
            //    .Where(aseguradora => aseguradora.activa == "TRUE")
            //    .OrderBy(aseguradora => aseguradora.id).Desc;
            //return new List<Entity>(queryOver.List<Aseguradora>());
        }

        public override void Write()
        {
            throw new DontWriteOnLugaresException();
        }
    }
}