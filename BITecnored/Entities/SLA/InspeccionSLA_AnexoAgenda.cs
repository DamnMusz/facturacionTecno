using System.Collections.Generic;
using NHibernate;
using NHibernate.Criterion;
using System;
using BITecnored.Model;
using BITecnored.Model.DB.Querys;
using System.Diagnostics;

namespace BITecnored.Entities.SLA
{
    public class InspeccionSLA_AnexoAgenda : Entity
    {
        public virtual int id { get; set; }

        public virtual int? id_centro { get; set; }
        public virtual int? id_inspector { get; set; }
        public virtual DateTime? fecha_solicitud { get; set; }

        public InspeccionSLA_AnexoAgenda() { }

        public InspeccionSLA_AnexoAgenda(InspeccionSLA_DatosAgenda thisInsp)
        {
            id = thisInsp.id_triki;
            if (thisInsp.datos_agenda != null)
            {
                id_centro = thisInsp.datos_agenda.id_centro;
                fecha_solicitud = thisInsp.datos_agenda.fecha_solicitud;
                id_inspector = thisInsp.datos_agenda.id_inspector;
            }
        }

        public override IList<Entity> DoRead(ISession session)
        {
            ICriteria query = session.CreateCriteria<InspeccionSLA>()
                .AddOrder(Order.Asc("id"));


            List<Entity> res = new List<Entity>(query.List<InspeccionSLA>());
            return res;
        }

        public virtual void CompleteMarried(DateTime periodo)
        {
            string query = "UPDATE sla s"
            + " SET fecha_solicitud = i.\"Fecha\", id_centro_agenda = i.\"Id_Centro\", inspector = i.\"Inspector\" "
            + " FROM \"Inspecciones\" i "
            + " WHERE s.id_agenda = i.\"Id\" "
            + " AND s.periodo = " + Int32.Parse(Utils.MesAnioToNumberFormat(periodo))
            ;

            ISession session = new Hibernate().ConfigureSession(this);
            session.CreateSQLQuery(query)
                .List();
            session.Close();
        }

        public virtual void CompleteUnmarried(DateTime periodo)
        {
            string query = "UPDATE sla s"
            + " SET fecha_solicitud = fecha_inspeccion " // + INTERVAL '-1 day' " /* Descomentar para poner un día antes */
            + " WHERE s.periodo = " + Int32.Parse(Utils.MesAnioToNumberFormat(periodo))
            + " AND fecha_solicitud IS NULL";
            ;

            ISession session = new Hibernate().ConfigureSession(this);
            session.CreateSQLQuery(query)
                .List();
            session.Close();
        }
    }
}