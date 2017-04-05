using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace BITecnored.Model.DataContract
{
    [DataContract]
    public class MotivoNoRealizada
    {
        [DataMember]
        public string nombre { get; set; }
        [DataMember]
        public Int32 cantidad { get; set; }
        public MotivoNoRealizada(string nombre, Int32 cantidad)
        {
            this.nombre = nombre;
            this.cantidad = cantidad;
        }
    }
}