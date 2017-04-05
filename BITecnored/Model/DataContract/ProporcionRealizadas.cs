using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace BITecnored.Model.DataContract
{
    [DataContract]
    public class ProporcionRealizadas
    {
        [DataMember]
        public string periodo { get; set; }
        [DataMember]
        public Int32 realizadas { get; set; }
        [DataMember]
        public Int32 sin_efecto { get; set; }
        public ProporcionRealizadas(string periodo, Int32 realizadas, Int32 sin_efecto)
        {
            this.periodo = periodo;
            this.realizadas = realizadas;
            this.sin_efecto = sin_efecto;
        }
        override
        public string ToString()
        {
            return "Periodo: " + periodo + " Realizadas: " + realizadas + " Sin Efecto: " + sin_efecto;
        }
    }
}