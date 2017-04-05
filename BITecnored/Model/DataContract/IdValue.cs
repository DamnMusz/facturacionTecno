using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace BITecnored.Model.DataContract
{
    [DataContract]
    public class IdValue
    {
        [DataMember]
        public Int32 id { get; set; }
        [DataMember]
        public string value { get; set; }
        public IdValue(Int32 id, string value)
        {
            this.id = id;
            this.value = value;
        }
    }
}