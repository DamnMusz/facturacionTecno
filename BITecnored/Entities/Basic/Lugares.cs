using System;
using System.Collections.Generic;
using NHibernate;

namespace BITecnored.Entities.Basic
{
    public class Lugares : Entity
    {
        public virtual int id { get; set; }
        public virtual int cod_provincia { get; set; }
        
        public override IList<Entity> DoRead(ISession session)
        {
            throw new NotImplementedException();
        }

        public override void Write()
        {
            throw new DontWriteOnLugaresException();
        }

        public override string ToString()
        {
            return "id: " + id + " nombre: " + cod_provincia + "\n";
        }
    }
}