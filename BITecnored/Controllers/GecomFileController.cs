using BITecnored.Model;
using BITecnored.Model.DataContract;
using BITecnored.Model.DB;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace BITecnored.Controllers
{
    [Authorize]
    public class GecomFileController : ApiController
    {
        [HttpPost]
        public HttpResponse Post(IEnumerable<SiniestroFacturable> incoming)
        {
            try
            {
                int seq = GetLastCode();

                foreach (SiniestroFacturable item in incoming)
                {
                    item.nro_pedido = seq++;
                }

                GecomTxtGenerator gecom = new GecomTxtGenerator();
                string text = gecom.Generate(incoming.ToList());

                ConfigResponse(text);

                SetLastCode(seq);
                return HttpContext.Current.Response;
            }
            catch (Exception e)
            {
                HttpContext.Current.Response.StatusCode = 500;
                return HttpContext.Current.Response;
            }
        }

        private void SetLastCode(int seq)
        {
            DBAgenda db = new DBAgenda();
            db.Connect();
            string select = "UPDATE \"Maximos\" SET last_gecom_id = " + seq;
            OdbcDataReader dr = db.ExecuteSQL(select);
            dr.Close();
            db.Disconnect();
        }

        private int GetLastCode()
        {
            int code = 0;
            DBAgenda db = new DBAgenda();
            db.Connect();

            string select = "SELECT last_gecom_id FROM \"Maximos\" LIMIT 1";
            
            OdbcDataReader dr = db.ExecuteSQL(select);
            if (dr.HasRows)
                while (dr.Read())
                {
                    code = dr.GetInt32(0);
                }
            dr.Close();
            db.Disconnect();
            
            return code;
        }

        private void ConfigResponse(string text)
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ClearHeaders();

            HttpContext.Current.Response.AddHeader("Content-Length", text.Length.ToString());
            HttpContext.Current.Response.ContentType = "text/plain";
            HttpContext.Current.Response.AppendHeader("content-disposition", "attachment;filename=\"output.txt\"");

            HttpContext.Current.Response.Write(text);
            HttpContext.Current.Response.End();
        }
        
    }
}
