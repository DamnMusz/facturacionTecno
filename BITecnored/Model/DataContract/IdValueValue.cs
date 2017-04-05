using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace BITecnored.Model.DataContract
{
    [DataContract]
    public class IdValueValue
    {
        [DataMember]
        public Int32 id { get; set; }
        [DataMember]
        public string value1 { get; set; }
        [DataMember]
        public string value2 { get; set; }
        public IdValueValue(Int32 id, string value1, string value2)
        {
            this.id = id;
            this.value1 = value1;
            this.value2 = value2;
        }
    }
}