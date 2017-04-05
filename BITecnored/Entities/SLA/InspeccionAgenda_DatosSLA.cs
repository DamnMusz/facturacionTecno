using System;

namespace BITecnored.Entities.SLA
{
    public class InspeccionAgenda_DatosSLA
    {
        public virtual int id { get; set; }
        public virtual int? id_centro { get; set; }
        public virtual int? id_inspector { get; set; }
        public virtual DateTime? fecha_solicitud { get; set; }

        public InspeccionAgenda_DatosSLA() { }

        public override string ToString()
        {
            return 
                 "Id: " + id + "\n"
                + "id_centro: " + id_centro + "\n"
                + "fecha_solicitud: " + fecha_solicitud + "\n"
                ;
        }
    }
}