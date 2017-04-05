using System;
using System.Collections.Generic;
using NHibernate;

namespace BITecnored.Entities.Basic
{
    public class Aseguradora : Entity
    {
        public virtual int id { get; set; }
        public virtual string nombre { get; set; }
        public virtual string alias { get; set; }
        public virtual string activa { get; set; }
        
        public override IList<Entity> DoRead(ISession session)
        {
            IQueryOver<Aseguradora> queryOver = session.QueryOver<Aseguradora>()
                .Where(aseguradora => aseguradora.activa == "TRUE")
                .OrderBy(aseguradora => aseguradora.id).Desc;
            return new List<Entity>(queryOver.List<Aseguradora>());
        }

        public override void Write()
        {
            throw new DontWriteOnAseguradorasException();
        }

        public override string ToString()
        {
            return "id: " + id + " nombre: " + nombre + " alias: " + alias + "\n";
        }
    }
}