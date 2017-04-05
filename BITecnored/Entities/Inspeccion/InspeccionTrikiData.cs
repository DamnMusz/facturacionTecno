using System;
using System.Diagnostics;

namespace BITecnored.Entities.Inspeccion
{
    public class InspeccionTrikiData
    {
        public virtual int id { get; set; }
        public virtual string taller { get; set; }
        public virtual string inspeccionado_fecha { get; set; }
        public virtual string inspeccionado_hora { get; set; }

        public override string ToString()
        {
            return "id: " + id + "\n" + "taller: " + taller + "\n" + "inspeccionado_fecha: " + inspeccionado_fecha + "\n"
                + "inspeccionado_hora: " + inspeccionado_hora + "\n";
        }

        public virtual DateTime GetFechaInspeccion()
        {
            if (inspeccionado_fecha == null || inspeccionado_fecha == "")
                inspeccionado_fecha = "0000-00-00";
            if (inspeccionado_hora == null || inspeccionado_hora == "")
                inspeccionado_hora = "00:00:00";
            else if (inspeccionado_hora.Length == 5)
                inspeccionado_hora += ":00";
            string date_text = inspeccionado_fecha + " " + inspeccionado_hora;

            try {
                return DateTime.ParseExact(date_text, "yyyy-MM-dd HH:mm:ss", null);
            } catch (Exception e)
            {
                try
                {
                    return DateTime.ParseExact(date_text.Substring(0, 10)+" "+ date_text.Substring(11, 5) + ":00", "yyyy-MM-dd", null);
                } catch (Exception)
                {
                    try
                    {
                        return DateTime.ParseExact(date_text.Substring(0, 10), "yyyy-MM-dd", null);
                    }
                    catch (Exception e2)
                    {
                        return new DateTime();
                    }
                }
            }
        }
    }
}