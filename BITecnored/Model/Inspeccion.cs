using System;
using System.Collections.Generic;

namespace BITecnored.Model
{
    public class Inspeccion : IComparable
    {
        public string id_triki { get; set; } = "";
        public string id_agenda { get; set; } = "";
        public string aseguradora_alias { get; set; } = "";
        public int inspector { get; set; }
        public string inspector_triki { get; set; } = "";
        public string dominio { get; set; } = "";
        public string taller_triki { get; set; } = "";
        public int id_centro_agenda { get; set; }
        public int fotos_cantidad { get; set; }
        public int cierre_facturacion { get; set; }
        public int cierre_prestador { get; set; }
        public DateTime fecha_solicitud { get; set; }
        public DateTime fecha_inspeccion { get; set; }
        public DateTime fecha_envio { get; set; }
        public string tipo_prestador { get; set; } = "";
        public string tipo_inspeccion { get; set; } = "";
        public int id_lugares { get; set; }
        public int estado_ultimo { get; set; }
        public int subestado_ultimo { get; set; }

        public bool domicilio { get; set; }
        public bool completa { get; set; }

        public static string sqlColumns()
        {
            //          0           1           2
            return "id_triki, id_agenda, aseguradora_alias," +
            //     3           4           5                6           7               8
               " inspector, dominio, taller_triki, id_centro_agenda, fotos_cantidad, periodo," +
            //          9               10              11              12
               " fecha_solicitud, fecha_inspeccion, fecha_envio, tipo_prestador," +
            //      13           14                 15                  16                17
               " id_lugares, inspector_triki, tipo_inspeccion, cierre_aseguradora, cierre_prestador";
        }

        public string GetId_triki() {  return ((id_triki != "" && id_triki != null) ? id_triki : "0");  }
        public string GetId_agenda() {  return ((id_agenda != "" && id_agenda != null) ? id_agenda : "0"); }
        public string GetAseguradora_alias() {  return "'" + aseguradora_alias + "'"; }
        public string GetInspector_triki() {  return "'" + inspector_triki + "'"; }
        public string GetDominio() {  return "'" + dominio + "'"; }
        public string GetTaller_triki() {  return "'" + taller_triki + "'"; }
        public string GetId_centro_agenda() {  return ""+id_centro_agenda; }
        public string GetFotos_cantidad() {  return ""+fotos_cantidad; }
        public string GetPeriodo() { return GetCierre_facturacion(); }
        public string GetCierre_aseguradora() { return GetCierre_facturacion(); }
        public string GetCierre_facturacion() {  return ""+cierre_facturacion; }
        public string GetCierre_prestador() {  return ""+cierre_prestador; }
        public string GetFecha_solicitud() {  return "'" + fecha_solicitud.ToString("yyyy-MM-dd hh:mm:ss") + "'"; }
        public string GetFecha_inspeccion() {  return "'" + fecha_inspeccion.ToString("yyyy-MM-dd hh:mm:ss") + "'"; }
        public string GetFecha_envio() {  return "'" + fecha_envio.ToString("yyyy-MM-dd hh:mm:ss") + "'"; }
        public string GetTipo_prestador() {  return "'" + tipo_prestador + "'"; }
        public string GetTipo_inspeccion() {  return "'" + tipo_inspeccion + "'"; }
        public string GetId_lugares() {  return ""+id_lugares; }

        public string GetInspector() { return "" + inspector; }

        public string sqlValues()
        {
            return ""
            + ((id_triki!="" && id_triki != null) ?id_triki:"0") + ", "         //0
            + ((id_agenda!="" && id_agenda != null) ?id_agenda:"0") + ", "      //1
            + "'" + aseguradora_alias + "', "                                   //2
            + inspector + ", "                                                  //3
            + "'" + dominio + "', "                                             //4
            + "'" + taller_triki + "', "                                        //5
            + id_centro_agenda + ", "                                           //6
            + fotos_cantidad + ", "                                             //7
            + cierre_facturacion + ", "                                         //8
            + "'" + fecha_solicitud.ToString("yyyy-MM-dd hh:mm:ss") + "', "     //9
            + "'" + fecha_inspeccion.ToString("yyyy-MM-dd hh:mm:ss") + "', "    //10
            + "'" + fecha_envio.ToString("yyyy-MM-dd hh:mm:ss") + "', "         //11
            + "'" + tipo_prestador + "', "                                      //12
            + id_lugares + ", "                                                 //13
            + "'" + inspector_triki + "', "                                     //14
            + "'" + tipo_inspeccion + "',"                                      //15
            + cierre_facturacion + ", "                                         //16
            + cierre_prestador                                                  //17
            + "";
        }

        private Inspeccion() {
            this.id_agenda = null;
            this.id_triki = null;
            this.domicilio = false;
            this.fecha_solicitud = new DateTime();
            this.fecha_inspeccion = new DateTime();
            this.fecha_envio = new DateTime();
            this.completa = false;
        }
        private Inspeccion(string idAgenda, bool domicilio, string fechaSolicitud, string fechaInspeccion, string fechaPublicacion)
        {
            completa = true;
            this.id_agenda = (idAgenda!= null)?idAgenda:"";
            this.domicilio = domicilio;
            if (fechaSolicitud != null) this.fecha_solicitud = DateTime.Parse(fechaSolicitud);
            else completa = false;
            if (fechaInspeccion != null) this.fecha_inspeccion = DateTime.Parse(fechaInspeccion);
            else completa = false;
            if (fechaPublicacion != null) this.fecha_envio = DateTime.Parse(fechaPublicacion);
            else completa = false;
        }

        public int CompareTo(object obj)
        {
            Inspeccion otra = (Inspeccion)obj;
            return this.id_agenda.CompareTo(otra.id_agenda);
        }

        public void CompletarSoltera()
        {
            this.fecha_solicitud = this.fecha_inspeccion;
            this.completa = true;
        }

        public void Completar(Inspeccion otra)
        {
            this.fecha_solicitud = otra.fecha_solicitud;
            this.inspector = otra.inspector;
            this.id_centro_agenda = otra.id_centro_agenda;
            this.estado_ultimo = otra.estado_ultimo;
            this.subestado_ultimo = otra.subestado_ultimo;
            this.aseguradora_alias = otra.aseguradora_alias;
            this.completa = true;
        }

        public static InspeccionBuilder getBuilder()
        {
            return new InspeccionBuilder();
        }

        public class InspeccionBuilder {
            Inspeccion inspeccion;

            public InspeccionBuilder() { inspeccion = new Inspeccion(); }

            public InspeccionBuilder IdAgenda(string idAgenda)
            {
                inspeccion.id_agenda = idAgenda;
                return this;
            }
            public InspeccionBuilder IdTriki(string idTriki)
            {
                inspeccion.id_triki = idTriki;
                return this;
            }
            public InspeccionBuilder Domicilio(bool domicilio)
            {
                inspeccion.domicilio = domicilio;
                return this;
            }
            public InspeccionBuilder AseguradoraAlias(string aseguradoraAlias)
            {
                inspeccion.aseguradora_alias = aseguradoraAlias;
                return this;
            }
            public InspeccionBuilder IdInspector(int idInspector)
            {
                inspeccion.inspector = idInspector;
                return this;
            }
            public InspeccionBuilder InspectorTriki(string nombreInspector)
            {
                inspeccion.inspector_triki = nombreInspector;
                return this;
            }
            public InspeccionBuilder TipoInspeccion(string tipoInspeccion)
            {
                inspeccion.tipo_inspeccion = tipoInspeccion;
                return this;
            }
            public InspeccionBuilder TallerTriki(string tallerTriki)
            {
                inspeccion.taller_triki = tallerTriki;
                return this;
            }
            public InspeccionBuilder Dominio(string dominio)
            {
                inspeccion.dominio = dominio;
                return this;
            }
            public InspeccionBuilder IdCentroAgenda(int idCentroAgenda)
            {
                inspeccion.id_centro_agenda = idCentroAgenda;
                return this;
            }
            public InspeccionBuilder CantFotos(int cantFotos)
            {
                inspeccion.fotos_cantidad = cantFotos;
                return this;
            }
            public InspeccionBuilder TipoPrestador(string tipoPrestador)
            {
                inspeccion.tipo_prestador = tipoPrestador;
                return this;
            }
            public InspeccionBuilder CierreFact(int cierreFact)
            {
                inspeccion.cierre_facturacion = cierreFact;
                return this;
            }
            public InspeccionBuilder CierreAseguradora(int cierreAseguradora)
            {
                return CierreFact(cierreAseguradora);
            }
            public InspeccionBuilder CierrePrestador(int cierrePrestador)
            {
                inspeccion.cierre_prestador = cierrePrestador;
                return this;
            }
            public InspeccionBuilder IdLugares(int idLugares)
            {
                inspeccion.id_lugares = idLugares;
                return this;
            }
            public InspeccionBuilder FechaSolicitud(DateTime fechaSolicitud)
            {
                inspeccion.fecha_solicitud = fechaSolicitud;
                return this;
            }
            public InspeccionBuilder FechaInspeccion(DateTime fechaInspeccion)
            {
                inspeccion.fecha_inspeccion = fechaInspeccion;
                return this;
            }
            public InspeccionBuilder FechaPublicacion(DateTime fechaPublicacion)
            {
                inspeccion.fecha_envio = fechaPublicacion;
                return this;
            }
            public InspeccionBuilder Completa(bool completa)
            {
                inspeccion.completa = completa;
                return this;
            }
            public InspeccionBuilder EstadoUltimo(int estado_ultimo)
            {
                inspeccion.estado_ultimo = estado_ultimo;
                return this;
            }
            public InspeccionBuilder SubestadoUltimo(int subestado_ultimo)
            {
                inspeccion.subestado_ultimo = subestado_ultimo;
                return this;
            }

            public Inspeccion build()
            {
                return inspeccion;
            }
        }
    }
}