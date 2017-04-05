using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Web.Script.Serialization;
using System.Diagnostics;

namespace BITecnored.Model
{
    public class Serializer
    {
        public static string ToJSon<T>(List<T> list)
        {
            MemoryStream resStream = new MemoryStream();
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(List<T>));
            ser.WriteObject(resStream, list);

            resStream.Position = 0;
            StreamReader sr = new StreamReader(resStream);
            resStream.Position = 0;
            return sr.ReadToEnd();
        }

        public static string ToJSon<T>(T data)
        {
            MemoryStream resStream = new MemoryStream();
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            try {
                ser.WriteObject(resStream, data);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                throw e;
            }

            resStream.Position = 0;
            StreamReader sr = new StreamReader(resStream);
            resStream.Position = 0;
            return sr.ReadToEnd();
        }

        public static T FromJSon<T>(string json)
        {
            JavaScriptSerializer oJS = new JavaScriptSerializer();
            return oJS.Deserialize<T>(json);
        }
    }
}