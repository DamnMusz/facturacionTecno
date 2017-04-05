using System;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using NReco.PdfGenerator;
using System.Net;
using System.Net.Http.Headers;
using System.Diagnostics;
using System.IO;
using BITecnored.Model.DataContract;
using System.Text;

namespace BITecnored.Controllers
{
    [Authorize]
    public class PDFController : ApiController
    {
        [HttpPost]
        [Route("api/generate_pdf")]
        public HttpResponseMessage Post()//[FromUri] int periodo, [FromUri] string aseguradora)
        {
            string strHtml = Request.Content.ReadAsStringAsync().Result;


            HtmlToPdfConverter pdfConverter = new HtmlToPdfConverter();
            var pdfBytes = pdfConverter.GeneratePdf(strHtml);
            var response = Request.CreateResponse(HttpStatusCode.OK);
            string fileName = Path.GetFullPath(HttpContext.Current.Server.MapPath("~") + "\\sla_pdf\\" + "pruebas.pdf");
            WriteToFile(pdfBytes, fileName);

            string fileNameHTML = Path.GetFullPath(HttpContext.Current.Server.MapPath("~") + "\\sla_pdf\\" + "pruebas.html");
            WriteToFile(Encoding.ASCII.GetBytes(strHtml), fileNameHTML);

            return response;
        }

        protected bool WriteToFile(byte[] data, string filename)
        {
            try
            {
                FileStream _FileStream = new FileStream(filename, FileMode.Create,
                                            FileAccess.Write);
                _FileStream.Write(data, 0, data.Length);
                _FileStream.Close();

                return true;
            }
            catch (Exception _Exception)
            {
                Debug.WriteLine("Exception caught in process: {0}",
                                  _Exception.ToString());
            }
            return false;
        }
    }
}
