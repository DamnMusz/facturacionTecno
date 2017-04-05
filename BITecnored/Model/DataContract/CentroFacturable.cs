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
    public class CentroFacturable
    {
        [DataMember]
        string nombre_centro { get; set; }

        [DataMember]
        public Int32 id { get; set; }

        [DataMember]
        string propio { get; set; }
        //cuit
        [DataMember]
        string activo { get; set; }
        [DataMember]
        string nombre_fantasia { get; set; }
        [DataMember]
        IdValue afinidad_tarifaria { get; set; }

        [DataMember]
        public long cuit { get; set; }
        [DataMember]
        string razon_social { get; set; }
        [DataMember]
        IdValue provincia_legal { get; set; }
        [DataMember]
        IdValue localidad_legal { get; set; }
        [DataMember]
        string direccion_legal_calle { get; set; }
        [DataMember]
        Int32 direccion_legal_numero { get; set; }
        [DataMember]
        string tipo_factura { get; set; }
        [DataMember]
        public string tarifa_actual { get; set; } = "";

        public List<string> GetAttrNamesPersonaJuridica()
        {
            List<string> res = new List<string>();
            res.Add("cuit");
            res.Add("razon_social");
            res.Add("id_provincia_legal");
            res.Add("id_localidad_legal");
            res.Add("direccion_legal_calle");
            res.Add("direccion_legal_numero");
            res.Add("tipo_factura");
            return res;
        }

        public List<string> GetAttrValuesPersonaJuridica()
        {
            List<string> res = new List<string>();

            if (cuit > 0)
                res.Add("" + cuit);
            else
                res.Add("NULL");

            if (razon_social != null)
                res.Add("'"+razon_social+"'");
            else
                res.Add("NULL");

            if (provincia_legal != null && provincia_legal.id != 0 && provincia_legal.id != -1)
                res.Add(""+provincia_legal.id);
            else
                res.Add("NULL");

            if (localidad_legal != null && localidad_legal.id != 0 && localidad_legal.id != -1)
                res.Add("" + localidad_legal.id);
            else
                res.Add("NULL");
            
            if (direccion_legal_calle != null && direccion_legal_calle.Length > 0)
                res.Add("'"+direccion_legal_calle+"'");
            else
                res.Add("NULL");

            if (direccion_legal_numero != -1 && direccion_legal_numero != 0)
                res.Add("" + direccion_legal_numero);
            else
                res.Add("NULL");

            if (tipo_factura != null && tipo_factura.Length > 0 && tipo_factura != "-")
                res.Add("'" + tipo_factura + "'");
            else
                res.Add("NULL");
            return res;
        }

        public List<string> GetAttrNamesCentro()
        {
            List<string> res = new List<string>();
            res.Add("propio");
            res.Add("id_persona_juridica");
            res.Add("activo_facturacion");
            res.Add("nombre_fantasia");
            res.Add("id_afinidad_tarifaria");
            return res;
        }

        public List<string> GetAttrValuesCentro()
        {
            List<string> res = new List<string>();
            if (propio == null || propio == "")
                res.Add("NULL");
            else if (propio == "true")
                    res.Add("true");
                else if (propio == "false")
                        res.Add("false");
            
            if(cuit > 0)
                res.Add(""+cuit);
            else
                res.Add("NULL");

            if (activo == null || activo == "")
                res.Add("NULL");
            else if (activo == "true")
                res.Add("true");
            else if (activo == "false")
                res.Add("false");

            if(nombre_fantasia != null && nombre_fantasia.Length > 0)
                res.Add("'"+nombre_fantasia+"'");
            else
                res.Add("NULL");
            
            if (afinidad_tarifaria != null && afinidad_tarifaria.id != -1)
                res.Add("" + afinidad_tarifaria.id);
            else
                res.Add("NULL");

            return res;
        }

        public static CentroFacturableBuilder getBuilder()
        {
            return new CentroFacturableBuilder();
        }

        public class CentroFacturableBuilder
        {
            CentroFacturable centro;

            public CentroFacturableBuilder() { centro = new CentroFacturable(); }

            public CentroFacturableBuilder id(Int32 id)
            {
                centro.id = id;
                return this;
            }

            public CentroFacturableBuilder nombre_fantasia(string nombre)
            {
                centro.nombre_fantasia = nombre;
                return this;
            }

            public CentroFacturableBuilder nombre_centro(string nombre_centro)
            {
                centro.nombre_centro = nombre_centro;
                return this;
            }

            public CentroFacturableBuilder cuit(long cuit)
            {
                centro.cuit = cuit;
                return this;
            }

            public CentroFacturableBuilder razon_social(string razon_social)
            {
                centro.razon_social = razon_social;
                return this;
            }

            public CentroFacturableBuilder tarifa_actual(string tarifa_actual)
            {
                centro.tarifa_actual = tarifa_actual;
                return this;
            }

            public CentroFacturableBuilder propio(bool propio)
            {
                centro.propio = propio?"true":"false";
                return this;
            }

            public CentroFacturableBuilder propio()
            {
                centro.propio = "";
                return this;
            }

            public CentroFacturableBuilder propio(string _propio)
            {
                string propio = null;
                if(_propio != null)
                    propio = _propio.ToLower();
                if (propio == null || (propio != "true" && propio != "false"))
                    centro.propio = "";
                if (propio == "true" || propio == "1")
                    centro.propio = "true";
                if (propio == "false" || propio == "0")
                    centro.propio = "false";
                return this;
            }

            public CentroFacturableBuilder provincia_legal(IdValue provincia_legal)
            {
                centro.provincia_legal = provincia_legal;
                return this;
            }

            public CentroFacturableBuilder localidad_legal(IdValue localidad_legal)
            {
                centro.localidad_legal = localidad_legal;
                return this;
            }

            public CentroFacturableBuilder direccion_legal_calle(string direccion_legal_calle)
            {
                centro.direccion_legal_calle = direccion_legal_calle;
                return this;
            }

            public CentroFacturableBuilder tipo_factura(string tipo_factura)
            {
                centro.tipo_factura = tipo_factura;
                return this;
            }
            
            public CentroFacturableBuilder direccion_legal_numero(Int32 direccion_legal_numero)
            {
                centro.direccion_legal_numero = direccion_legal_numero;
                return this;
            }

            public CentroFacturableBuilder activo(bool activo)
            {
                centro.activo = activo?"true":"false";
                return this;
            }

            public CentroFacturableBuilder activo(string _activo)
            {
                string activo = null;
                if (_activo != null)
                    activo = _activo.ToLower();
                if (activo==null || (activo != "true" && activo != "false"))
                    centro.activo = "";
                if (activo == "true" || activo == "1")
                    centro.activo = "true";
                if (activo == "false" || activo == "0")
                    centro.activo = "false";
                return this;
            }

            public CentroFacturableBuilder activo()
            {
                centro.activo = "";
                return this;
            }

            public CentroFacturableBuilder afinidad_tarifaria(IdValue afinidad_tarifaria)
            {
                centro.afinidad_tarifaria = afinidad_tarifaria;
                return this;
            }

            public CentroFacturable build()
            {
                return centro;
            }
        }
    }
}