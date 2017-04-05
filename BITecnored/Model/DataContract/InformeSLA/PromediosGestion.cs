using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace BITecnored.Model.DataContract.InformeSLA
{
    [DataContract]
    public class PromediosGestion
    {
        [DataMember]
        public virtual decimal mismo_dia { get; set; } = 0;
        [DataMember]
        public virtual decimal en_24hs { get; set; } = 0;
        [DataMember]
        public virtual decimal en_48hs { get; set; } = 0;
        [DataMember]
        public virtual decimal en_72hs { get; set; } = 0;
        [DataMember]
        public virtual decimal mas_de_72hs { get; set; } = 0;

        public PromediosGestion(CantidadesGestion cantidades)
        {
            decimal cantidad_total = cantidades.mismo_dia + cantidades.en_24hs + cantidades.en_48hs + cantidades.en_72hs + cantidades.mas_de_72hs;
            if (cantidad_total == 0)
                return;
            mismo_dia = Math.Round((cantidades.mismo_dia / cantidad_total), 2);
            en_24hs = Math.Round((cantidades.en_24hs / cantidad_total), 2);
            en_48hs = Math.Round((cantidades.en_48hs / cantidad_total), 2);
            en_72hs = Math.Round((cantidades.en_72hs / cantidad_total), 2);
            mas_de_72hs = Math.Round((cantidades.mas_de_72hs / cantidad_total), 2);
        }
    }
}