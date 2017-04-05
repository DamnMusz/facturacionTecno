using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;
using BITecnored.Model.DB;
using System.Data.Odbc;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Diagnostics;

namespace BITecnored.Model
{
    public class Utils
    {

        public static DateTime FirstDayInMonth(DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1);
        }

        public static DateTime LastDayInMonth(DateTime date)
        {
            return new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));
        }

        public static DateTime FirstDayInMonth(string fecha)
        {
            DateTime date = DateTime.ParseExact(fecha, "MM/yyyy", CultureInfo.InvariantCulture);
            return new DateTime(date.Year, date.Month, 1);
        }

        public static DateTime LastDayInMonth(string fecha)
        {
            DateTime date = DateTime.ParseExact(fecha, "MM/yyyy", CultureInfo.InvariantCulture);
            return new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));
        }

        public static string DateToDBFormat(string fecha)
        {
            return DateTime.ParseExact(fecha, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy/MM/dd");
        }

        public static int DistanciaEntreDias(DateTime fechaInicio, DateTime fechaFin, string feriados)
        {
            fechaInicio = new DateTime(fechaInicio.Year, fechaInicio.Month, fechaInicio.Day);
            fechaFin = new DateTime(fechaFin.Year, fechaFin.Month, fechaFin.Day);

            int diferenciaDias = (fechaFin - fechaInicio).Days;
            
            for (int anio = fechaInicio.Year; anio <= fechaFin.Year; ++anio)
                for (int mes = fechaInicio.Month; mes <= fechaFin.Month; ++mes)
                    for (int dia = fechaInicio.Day + 1; dia < fechaFin.Day; ++dia)
                    {
                        DateTime fecha = new DateTime(anio, mes, dia);
                        if (
                            fecha.DayOfWeek == DayOfWeek.Sunday ||
                            fecha.DayOfWeek == DayOfWeek.Saturday ||
                            feriados.Contains(fecha.ToString("yyyy-MM-dd")))
                            --diferenciaDias;
                    }
            if (diferenciaDias < 0)
                return 0;
            else
                return diferenciaDias;
        }

        public static IEnumerable<DateTime> MesesEnRango(DateTime desde, DateTime hasta)
        {
            for (DateTime date = desde; date <= hasta; date = date.AddMonths(1))
            {
                yield return date;
            }
        }

        public static bool ExistPrevious(string aseguradora, DateTime periodo)
        {
            bool existsPrevious = false;
            string existCheckQuery = "SELECT periodo FROM sla WHERE periodo = " + periodo.Year.ToString("D" + 4) + periodo.Month.ToString("D" + 2)
                + " AND aseguradora_alias = '" + aseguradora + "'";
            DBAgenda db = new DBAgenda();
            db.Connect();
            OdbcDataReader dr = db.ExecuteSQL(existCheckQuery);
            if (dr.HasRows)
                existsPrevious = true;
            dr.Close();
            db.Disconnect();
            return existsPrevious;
        }

        public static bool ExistPrevious(DateTime periodo)
        {
            bool existsPrevious = false;
            string existCheckQuery = "SELECT periodo FROM sla WHERE periodo = " + periodo.Year.ToString("D" + 4) + periodo.Month.ToString("D" + 2);
            DBAgenda db = new DBAgenda();
            db.Connect();
            OdbcDataReader dr = db.ExecuteSQL(existCheckQuery);
            if (dr.HasRows)
                existsPrevious = true;
            dr.Close();
            db.Disconnect();
            return existsPrevious;
        }

        public static string GetAseguradoraAlias(int? aseguradoraID)
        {
            if (aseguradoraID == null)
                return null;
            else
            {
                string query = "SELECT \"Aseguradoras_Alias\" FROM \"Aseguradoras\" WHERE \"Aseguradoras_Id\" = " + aseguradoraID;
                string res = "";
                DBAgenda db = new DBAgenda();
                db.Connect();
                OdbcDataReader dr = db.ExecuteSQL(query);
                if (dr.HasRows)
                    while (dr.Read())
                        res = dr.GetString(0);
                dr.Close();
                db.Disconnect();
                return res;
            }
        }

        public static string GetUsuario(HttpRequestMessage request)
        {
            if (request != null)
            {
                HttpRequestHeaders headers = request.Headers;
                string authHeader = null;
                if (headers.Contains("Authorization"))
                    authHeader = headers.GetValues("Authorization").First();

                if (authHeader != null && authHeader.StartsWith("Basic"))
                {
                    string encodedUsernamePassword = authHeader.Substring("Basic ".Length).Trim();
                    // the coding should be iso or you could use ASCII and UTF - 8 decoder
                    Encoding encoding = Encoding.GetEncoding("iso-8859-1");
                    string usernamePassword = encoding.GetString(Convert.FromBase64String(encodedUsernamePassword));
                    int seperatorIndex = usernamePassword.IndexOf(':');
                    string username = usernamePassword.Substring(0, seperatorIndex);

                    return username;   
                }
                else {
                    throw new Exception("The authorization header is either empty or isn't Basic.");
                }
            }
            return "";
        }

        public static string GetPeriodoActual()
        {
            DateTime ahora = DateTime.Now;
            return ahora.Year.ToString("D"+4) + ahora.Month.ToString("D" + 2);
        }

        public static string MesAnioToNumberFormat(DateTime fecha)
        {
            return (fecha.Year.ToString("D" + 4) + fecha.Month.ToString("D" + 2));
        }

        public static DateTime NumberToFirstDayFormat(int periodo)
        {
            int anio = periodo / 100;
            int mes = periodo % 100;
            return FirstDayInMonth(mes + "/" + anio);
        }

        public static DateTime NumberToLastDayFormat(int periodo)
        {
            int anio = periodo / 100;
            int mes = periodo % 100;
            return LastDayInMonth(mes + "/" + anio);
        }

        public static string GetFeriados()
        {
            string res = "";
            try
            {
                string query = "SELECT * FROM feriados LIMIT 1";
                DBAgenda db = new DBAgenda();
                db.Connect();
                OdbcDataReader dr = db.ExecuteSQL(query);
                if (dr.Read())
                    res = dr.GetString(0);
                dr.Close();
                db.Disconnect();
            }
            catch (Exception e) { }
            return res;
        }
    }
}