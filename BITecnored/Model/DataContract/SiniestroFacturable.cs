using System.Runtime.Serialization;

namespace BITecnored.Model.DataContract
{
    [DataContract]
    public class SiniestroFacturable
    {
        public static int DEFAULT_CODIGO_IMPUTACION = 515000;
        public static int DEFAULT_CANTIDAD_SINIESTROS = 1;
        public static float DEFAULT_IVA = 21.00f;

        [DataMember]
        public int cod_ase_gecom;
        [DataMember]
        public string nombre_ase;
        [DataMember]
        public float importe;
        [DataMember]
        public int cant_siniestros;
        [DataMember]
        public int cod_imputacion;
        [DataMember]
        public float iva;
        [DataMember]
        public int nro_pedido = 0;
        [DataMember]
        public int nro_siniestro;
        [DataMember]
        public string dominio;
        [DataMember]
        public string nombre;
        [DataMember]
        public string observacion;
        [DataMember]
        public string concepto;
        [DataMember]
        public string analista;


        public static SiniestroFacturableBuilder getBuilder()
        {
            return new SiniestroFacturableBuilder();
        }

        public class SiniestroFacturableBuilder
        {
            SiniestroFacturable siniestro;

            public SiniestroFacturableBuilder() { siniestro = new SiniestroFacturable(); }

            public SiniestroFacturableBuilder cod_ase_gecom(int cod_ase_gecom)
            {
                siniestro.cod_ase_gecom = cod_ase_gecom;
                return this;
            }

            public SiniestroFacturableBuilder nombre_ase(string nombre_ase)
            {
                siniestro.nombre_ase = nombre_ase.ToUpper();
                if (nombre_ase.ToUpper().Contains("ZURICH"))
                    siniestro.cod_ase_gecom = 28;
                if (nombre_ase.ToUpper().Contains("BERKLEY"))
                    siniestro.cod_ase_gecom = 35;
                return this;
            }

            public SiniestroFacturableBuilder importe(float importe)
            {
                siniestro.importe = importe;
                return this;
            }

            public SiniestroFacturableBuilder cant_siniestros(int cant_siniestros)
            {
                siniestro.cant_siniestros = cant_siniestros;
                return this;
            }

            public SiniestroFacturableBuilder cod_imputacion(int cod_imputacion)
            {
                siniestro.cod_imputacion = cod_imputacion;
                return this;
            }

            public SiniestroFacturableBuilder iva(float iva)
            {
                siniestro.iva = iva;
                return this;
            }

            public SiniestroFacturableBuilder nro_pedido(int nro_pedido)
            {
                siniestro.nro_pedido = nro_pedido;
                return this;
            }

            public SiniestroFacturableBuilder nro_siniestro(int nro_siniestro)
            {
                siniestro.nro_siniestro = nro_siniestro;
                return this;
            }

            public SiniestroFacturableBuilder dominio(string dominio)
            {
                siniestro.dominio = dominio;
                return this;
            }

            public SiniestroFacturableBuilder nombre(string nombre)
            {
                siniestro.nombre = formatString(nombre);
                return this;
            }

            public SiniestroFacturableBuilder concepto(string concepto)
            {
                siniestro.concepto = formatString(concepto);
                return this;
            }

            public SiniestroFacturableBuilder analista(string analista)
            {
                siniestro.analista = formatString(analista);
                return this;
            }

            public SiniestroFacturableBuilder observacion(string observacion)
            {
                siniestro.observacion = formatString(observacion);
                return this;
            }

            private string formatString(string input)
            {
                return input
                    .ToUpper()
                    .Replace("ñ", "n")
                    .Replace("Ñ", "N")
                    .Replace("Á", "A")
                    .Replace("É", "E")
                    .Replace("Í", "I")
                    .Replace("Ó", "O")
                    .Replace("Ú", "U");
            } 

            public SiniestroFacturable build()
            {
                return siniestro;
            }
        }
    }
}