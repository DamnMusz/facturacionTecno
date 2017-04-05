using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate;
using BITecnored.Model;
using System.Diagnostics;

namespace BITecnored.Entities.SLA
{
    public class SLA_Agenda : Entity
    {
        public virtual int id_agenda { get; set; }
        public virtual int estado { get; set; }
        public virtual int subestado { get; set; }
        public virtual int periodo { get; set; }
        public virtual int aseguradora_id { get; set; }
        public virtual string aseguradora_alias { get; set; }
        public virtual string motivo { get; set; }

        public SLA_Agenda() { }

        public SLA_Agenda(NoRealizadasAgenda no_realizada, DateTime periodo_fact)
        {
            id_agenda = no_realizada.id_agenda;
            estado = no_realizada.estado;
            subestado = no_realizada.subestado;
            periodo = int.Parse(Utils.MesAnioToNumberFormat(periodo_fact));
            aseguradora_id = no_realizada.aseguradora.id;
            aseguradora_alias = no_realizada.aseguradora.alias;
            motivo = no_realizada.motivo;
        }

        public override IList<Entity> DoRead(ISession session)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return "id_agenda: "+id_agenda;
        }
    }
}