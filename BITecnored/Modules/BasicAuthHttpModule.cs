using BITecnored.Model.DB;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web;

namespace AngularWebAPI.WebAPI.Modules
{
    public class BasicAuthHttpModule : IHttpModule
    {
        private const string Realm = "AngularWebAPI";
        public static string status_ok = "OK";
        public static string status_error_user = "Usuario inexistente";
        public static string status_error_pass = "Password incorrecto";
        public static string status_error_inactivo = "Usuario inactivo";

        public void Init(HttpApplication context)
        {
            // Register event handlers
            context.AuthenticateRequest += OnApplicationAuthenticateRequest;
            context.EndRequest += OnApplicationEndRequest;
        }
        
        private static void SetPrincipal(IPrincipal principal)
        {
            Thread.CurrentPrincipal = principal;
            if (HttpContext.Current != null)
            {
                HttpContext.Current.User = principal;
            }
        }

        private static bool AuthenticateUser(string credentials)
        {
            var encoding = Encoding.GetEncoding("iso-8859-1");
            credentials = encoding.GetString(Convert.FromBase64String(credentials));

            var credentialsArray = credentials.Split(':');
            var username = credentialsArray[0];
            var password = credentialsArray[1];
            
            UserLogin result = CheckUser(username, password);

            if (!(result.status == status_ok))
            {
                return false;
            }

            var identity = new GenericIdentity(username);
            SetPrincipal(new GenericPrincipal(identity, null));

            return true;
        }

        public class UserLogin
        {
            public string nickname, nombre, apellido, status;
            public UserLogin(string status)
            {
                this.status = status;
            }
            public UserLogin(string nickname, string nombre, string apellido)
            {
                this.nickname = nickname;
                this.nombre = nombre;
                this.apellido = apellido;
                this.status = status_ok;
            }
        }

        public static UserLogin CheckUser(string username, string password)
        {
            DBAgenda db = new DBAgenda();
            db.Connect();
            OdbcDataReader dr = db.ExecuteSQL("SELECT \"Usuario\", \"Password\", \"Activo\", \"Nombre\", \"Apellido\" FROM \"Usuarios\" WHERE \"Usuario\" = '"+username+"'");
            if (dr.Read())
            {
                string user = dr.GetString(0);
                string pass = dr.GetString(1);
                bool activo = dr.GetBoolean(2);
                string nombre = dr.IsDBNull(3)?"":dr.GetString(3);
                string apellido = dr.IsDBNull(4) ? "" : dr.GetString(4);

                UserLogin res = new UserLogin(status_ok);

                if (user != username)
                    res.status = status_error_user;

                if (pass != password)
                    res.status = status_error_pass;

                if (!activo)
                    res.status = status_error_inactivo;

                res.nickname = user;
                res.nombre = nombre;
                res.apellido = apellido;

                db.Disconnect();
                return res;
            }
            db.Disconnect();
            return new UserLogin(status_error_user);
        }

        private static void OnApplicationAuthenticateRequest(object sender, EventArgs e)
        {
            var request = HttpContext.Current.Request;
            var authHeader = request.Headers["Authorization"];
            if (authHeader != null)
            {
                var authHeaderVal = AuthenticationHeaderValue.Parse(authHeader);

                // RFC 2617 sec 1.2, "scheme" name is case-insensitive
                if (authHeaderVal.Scheme.Equals("basic", StringComparison.OrdinalIgnoreCase) && authHeaderVal.Parameter != null)
                {
                    AuthenticateUser(authHeaderVal.Parameter);
                }
            }
        }

        // If the request was unauthorized, add the WWW-Authenticate header 
        // to the response.
        private static void OnApplicationEndRequest(object sender, EventArgs e)
        {
            var response = HttpContext.Current.Response;
            if (response.StatusCode == 401)
            {
                response.Headers.Add("WWW-Authenticate", string.Format("Basic realm=\"{0}\"", Realm));
            }
        }

        public void Dispose()
        {
        }
    }
}