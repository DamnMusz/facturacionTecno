using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace BITecnored.Model.DataContract
{
    [DataContract]
    public class Aseguradora
    {
        [DataMember]
        Int32 id { get; set; }
        [DataMember]
        string nombre { get; set; }
        [DataMember]
        string codigo { get; set; }
        public Aseguradora(Int32 id, string nombre, string codigo)
        {
            this.id = id;
            this.nombre = nombre;
            this.codigo = codigo;
        }
    }
}