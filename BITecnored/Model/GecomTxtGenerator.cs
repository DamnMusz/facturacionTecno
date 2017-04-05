using BITecnored.Model.DataContract;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Web;

namespace BITecnored.Model
{
    public class GecomTxtGenerator
    {
        public string Generate(List<SiniestroFacturable> siniestros)
        {
            string result = "";
            foreach(SiniestroFacturable siniestro in siniestros)
            {
                result += CodigoCia(siniestro) + Importe(siniestro) + CantidadAFacturar(siniestro)
                    + CodigoImputacion(siniestro) + IVA(siniestro) + NroPedido(siniestro) + Detalle(siniestro)
                    + Environment.NewLine;
            }
            return result;
        }

        private bool CheckSize(long value, long maxValue, string message)
        {
            if (value > maxValue || value < 0)
                throw new Exception(message);
            else
                return true;
        }

        private string FillWithSpaces(string src, int ammount)
        {
            string result = src;
            while (result.Length < ammount)
                result = " " + result;
            return result;
        }

        private string CodigoCia(SiniestroFacturable siniestro)
        {
            int codigoInt = siniestro.cod_ase_gecom;
            CheckSize(codigoInt, 99, "Siniestro " + siniestro.nro_siniestro + ": Codigo Aseguradora Fuera de Rango.");

            string aux = codigoInt.ToString();
            string result = "  ";

            if (aux.Length == 1)
                result = " " + aux[0];
            if (aux.Length == 2)
                result = "" + aux[0] + aux[1];

            return result;
        }

        private string Importe(SiniestroFacturable siniestro)
        {
            float importe = siniestro.importe;

            string s = importe.ToString("0.00", CultureInfo.InvariantCulture);
            string[] parts = s.Split('.');
            string entero = parts[0];
            string decimales = parts[1];
            int i1 = int.Parse(entero);
            int i2 = int.Parse(decimales);

            CheckSize(i1, 99999999, "Siniestro " + siniestro.nro_siniestro + ": Importe Fuera de Rango.");

            string result = entero;
            result = FillWithSpaces(result, 7);
            result = result + ",";

            if (decimales.Length == 0)
                result = result + "00";
            if (decimales.Length == 1)
                result = result + "0" + decimales[0];
            if (decimales.Length > 1)
                result = result + decimales[0] + decimales[1];
            return result;
        }

        private string CodigoImputacion(SiniestroFacturable siniestro)
        {
            int codigoInt = siniestro.cod_imputacion;
            CheckSize(codigoInt, 999999999, "Siniestro " + siniestro.nro_siniestro + ": Codigo Imputacion Fuera de Rango.");
            string result = codigoInt.ToString();
            result = FillWithSpaces(result, 9);
            return result;
        }

        private string CantidadAFacturar(SiniestroFacturable siniestro)
        {
            int cantAFacturar = siniestro.cant_siniestros;
            CheckSize(cantAFacturar, 9999, "Siniestro " + siniestro.nro_siniestro + ": Cantidad de Siniestros Fuera de Rango.");
            string result = cantAFacturar.ToString();
            result = FillWithSpaces(result, 4);
            return result;
        }

        private string IVA(SiniestroFacturable siniestro)
        {
            float iva = siniestro.iva;

            string s = iva.ToString("0.00", CultureInfo.InvariantCulture);
            string[] parts = s.Split('.');
            string entero = parts[0];
            string decimales = parts[1];
            int i1 = int.Parse(entero);
            int i2 = int.Parse(decimales);

            CheckSize(i1, 99999, "Siniestro " + siniestro.nro_siniestro + ": IVA Fuera de Rango.");

            string result = entero;
            result = FillWithSpaces(result, 5);
            result = result + ",";

            if (decimales.Length == 0)
                result = result + "00";
            if (decimales.Length == 1)
                result = result + "0" + decimales[0];
            if (decimales.Length > 1)
                result = result + decimales[0] + decimales[1];
            return result;
        }

        private string NroPedido(SiniestroFacturable siniestro)
        {
            int codigoInt = siniestro.nro_pedido;
            CheckSize(codigoInt, 9999999999, "Siniestro " + siniestro.nro_siniestro + ": Numero de Pedido Fuera de Rango.");
            string result = codigoInt.ToString();
            result = FillWithSpaces(result, 10);
            return result;
        }

        private string Detalle(SiniestroFacturable siniestro)
        {
            string result = "STRO: " + siniestro.nro_siniestro + " | " + siniestro.dominio + " | " + siniestro.nombre;
            if(siniestro.observacion != null && siniestro.observacion.Length > 0)
                result += " | " + siniestro.observacion;

            if (result.Length > 120)
                result = result.Substring(0, 120);

            return result;
        }
    }
}