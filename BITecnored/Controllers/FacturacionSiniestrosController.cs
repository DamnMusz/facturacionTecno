using BITecnored.Model;
using BITecnored.Model.DataContract;
using Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace BITecnored.Controllers
{
    [Authorize]
    public class FacturacionSiniestrosController : ApiController
    {
        [HttpPost]
        public async Task<HttpResponseMessage> Post(HttpRequestMessage request)
        {
            List<string> fileNameList = new List<string>();
            try
            {
                var context = new HttpContextWrapper(HttpContext.Current);

                List<SiniestroFacturable> siniestros = new List<SiniestroFacturable>();
                for(int i = 0; i < context.Request.Files.Count; ++i)
                {
                    HttpPostedFileBase file = context.Request.Files[i];
                    string fileName = Path.GetFullPath(System.Web.HttpContext.Current.Server.MapPath("~") + "\\tmp\\" + file.FileName);
                    SaveFile(file, fileName);
                    fileNameList.Add(fileName);
                    List<SiniestroFacturable> partial = ProcessFile(fileName);
                    siniestros.AddRange(partial);
                }               

                var response = Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(Serializer.ToJSon<SiniestroFacturable>(siniestros), System.Text.Encoding.UTF8, "application/json");
                return response;
            }
            catch (System.Exception e)
            {
                var response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
                //response.Content = new StringContent("{ error: '"+e.Message+"'}", System.Text.Encoding.UTF8, "application/json");
                response.Content = new StringContent(e.Message, System.Text.Encoding.UTF8, "application/json");
                return response;
            }
            finally
            {
                foreach(string fileName in fileNameList)
                    DeleteFile(fileName);
            }
        }

        private void SaveFile(HttpPostedFileBase file, string fileName)
        {
            file.SaveAs(fileName);
        }

        private void DeleteFile(string fileName)
        {
            if (File.Exists(fileName))
                File.Delete(fileName);
        }

        private List<SiniestroFacturable> ProcessFile(string fileName)
        {
            FileStream stream = File.Open(fileName, FileMode.Open, FileAccess.Read);

            //2. Reading from a OpenXml Excel file (2007 format; *.xlsx)
            IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);

            //3. DataSet - The result of each spreadsheet will be created in the result.Tables
            //4. DataSet - Create column names from first row
            excelReader.IsFirstRowAsColumnNames = true;
            DataSet result = excelReader.AsDataSet();

            //Salteo la primer fila con nombres de columnas
            excelReader.Read();

            int rowIndx = 1;
            List<SiniestroFacturable> siniestros = new List<SiniestroFacturable>();
            //5. Data Reader methods
            while (excelReader.Read())
            {
                ++rowIndx;
                int colError = -1;

                if (excelReader.IsDBNull(7)) { colError = 6; }
                if (excelReader.IsDBNull(6)) { colError = 5; }
                if (excelReader.IsDBNull(4)) { colError = 4; }
                if (excelReader.IsDBNull(3)) { colError = 3; }
                if (excelReader.IsDBNull(2)) { colError = 2; }
                if (excelReader.IsDBNull(1)) { colError = 1; }
                if (excelReader.IsDBNull(0)) { colError = 0; }

                if (colError != -1)
                    throw new Exception("El archivo contiene campos nulos en fila "+ rowIndx+ " y columna " + colError);

                string aseguradora = excelReader.GetString(0);
                float importe = excelReader.GetFloat(1);
                int nro_siniestro = excelReader.GetInt32(2);
                string dominio = excelReader.GetString(3);
                string nombre = excelReader.GetString(4);
                string observacion = excelReader.IsDBNull(5) ? "" : excelReader.GetString(5);
                string concepto = excelReader.GetString(6);
                string analista = excelReader.GetString(7);

                SiniestroFacturable siniestro = SiniestroFacturable.getBuilder()
                    .nombre_ase(aseguradora)
                    .importe(importe)
                    .nro_siniestro(nro_siniestro)
                    .dominio(dominio)
                    .nombre(nombre)
                    .observacion(observacion)
                    .concepto(concepto)
                    .analista(analista)
                    .cant_siniestros(SiniestroFacturable.DEFAULT_CANTIDAD_SINIESTROS)
                    .cod_imputacion(SiniestroFacturable.DEFAULT_CODIGO_IMPUTACION)
                    .iva(SiniestroFacturable.DEFAULT_IVA)
                    .build();
                siniestros.Add(siniestro);
            }
            //6. Free resources (IExcelDataReader is IDisposable)
            excelReader.Close();
            return siniestros;
        }

    }
}

