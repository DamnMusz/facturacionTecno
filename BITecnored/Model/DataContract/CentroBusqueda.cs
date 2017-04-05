using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Diagnostics;

namespace BITecnored.Model.DataContract
{
    [DataContract]
    public class CentroBusqueda
    {
        [DataMember]
        public string id { get; set; } = "";
        [DataMember]
        public String nombre { get; set; } = "";
        [DataMember]
        public String nombre_fantasia { get; set; } = "";
        [DataMember]
        public String calle { get; set; } = "";
        [DataMember]
        public String razon_social { get; set; } = "";
        [DataMember]
        public string cuit { get; set; } = "";
        [DataMember]
        public string propio { get; set; } = "";
        [DataMember]
        public string activo { get; set; } = "";
        [DataMember]
        public string numero { get; set; } = "";
        [DataMember]
        public string tipo_factura { get; set; } = "";
        [DataMember]
        public IdValue provincia { get; set; } = null;
        [DataMember]
        public IdValue localidad { get; set; } = null;
        [DataMember]
        public IdValue afinidad_tarifaria { get; set; } = null;

        public string getCriterios()
        {
            string res = "";
            if(id != null)
                res = AddCriterio(res, "\"Centros_Id\"", id);
            res = AddCriterioLike(res, "\"Centros_Nombre\"", nombre);
            res = AddCriterioLike(res, "nombre_fantasia", nombre_fantasia);
            res = AddCriterio(res, "cuit", cuit.Replace("-",""));
            res = AddCriterioLike(res, "razon_social", razon_social);
            res = AddCriterio(res, "propio", propio);
            res = AddCriterio(res, "activo_facturacion", activo);
            res = AddCriterio(res, "tipo_factura", "'"+tipo_factura.Replace("-", "")+ "'");
            if (provincia!=null)
                res = AddCriterio(res, "id_provincia_legal", ""+provincia.id);
            if (localidad!=null)
                res = AddCriterio(res, "id_localidad_legal", ""+localidad.id);
            res = AddCriterioLike(res, "direccion_legal_calle", calle);
            res = AddCriterio(res, "direccion_legal_numero", numero);
            if (afinidad_tarifaria!=null)
                res = AddCriterio(res, "at.id", ""+afinidad_tarifaria.id);

            return res;
        }

        private string AddCriterio(string res, string key, string value)
        {
            string aux = res;
            if(value != "" && value != "''") { 
                aux += " and "+key+" = " + value + " ";
            }
            return aux;
        }

        private string AddCriterioLike(string res, string key, string value)
        {
            string aux = res;
            if (value != "" && value != "''")
            {
                aux += " and LOWER(" + key + ") like " + "LOWER('%" + value + "%') ";
            }
            return aux;
        }
    }
}