using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using static System.Net.Mime.MediaTypeNames;

namespace BITecnored.Model.DataContract
{
    [DataContract]
    public class TextData
    {
        [DataMember]
        public string text { get; set; }
    }
}