namespace BITecnored.Entities.Inspeccion
{
    public class InspeccionTrikiOtros
    {
        public virtual int id { get; set; }
        public virtual int id_agenda { get; set; }
        public virtual int qfotos { get; set; }
        public virtual int cierre_fact { get; set; }
        public virtual int cierre_liq_prestador { get; set; }
        public virtual string tipo_prestador { get; set; }
        public virtual int id_lugares { get; set; }

        public override string ToString()
        {
            return "id_agenda: " + id_agenda + "\n" + "qfotos: " + qfotos + "\n" + "cierre_fact: " + cierre_fact + "\n"
                + "cierre_liq_prestador: " + cierre_liq_prestador + "\n"
                 + "tipo_prestador: " + tipo_prestador + "\n" + "id_lugares: " + id_lugares + "\n";
        }
    }
}