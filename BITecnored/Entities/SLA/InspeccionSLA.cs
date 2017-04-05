using System.Collections.Generic;
using NHibernate;
using NHibernate.Criterion;
using System;
using BITecnored.Entities.Inspeccion;
using BITecnored.Model.DB.Querys;
using BITecnored.Model;
using System.Collections;
using System.Diagnostics;

namespace BITecnored.Entities.SLA
{
    public class InspeccionSLA : Entity
    {
        public virtual int id { get; set; }
        public virtual string aseguradora { get; set; }
        public virtual string inspector { get; set; }
        public virtual string dominio { get; set; }
        public virtual DateTime? fec_envio { get; set; }
        public virtual string tipo_inspeccion { get; set; }

        public virtual string taller { get; set; }
        public virtual DateTime? fecha_inspeccion { get; set; }

        public virtual int id_agenda { get; set; }
        public virtual int qfotos { get; set; }
        public virtual int periodo { get; set; }
        public virtual int cierre_fact { get; set; }
        public virtual int cierre_liq_prestador { get; set; }
        public virtual string tipo_prestador { get; set; }
        public virtual int id_lugares { get; set; }

        public InspeccionSLA() { }

        public InspeccionSLA(InspeccionTriki thisInsp)
        {
            id = thisInsp.id;
            aseguradora = thisInsp.aseguradora;
            inspector = thisInsp.inspector;
            dominio = thisInsp.dominio;
            fec_envio = thisInsp.fec_envio;
            tipo_inspeccion = thisInsp.tipo_inspeccion;
            id_agenda = thisInsp.otros.id_agenda;
            qfotos = thisInsp.otros.qfotos;
            periodo = thisInsp.otros.cierre_liq_prestador;
            cierre_fact = thisInsp.otros.cierre_fact;
            cierre_liq_prestador = thisInsp.otros.cierre_liq_prestador;
            tipo_prestador = thisInsp.otros.tipo_prestador;
            id_lugares = thisInsp.otros.id_lugares;
            taller = thisInsp.data.taller;
            fecha_inspeccion = thisInsp.data.GetFechaInspeccion();
        }

        public override IList<Entity> DoRead(ISession session)
        {
            ICriteria query = session.CreateCriteria<InspeccionSLA>()
                .AddOrder(Order.Asc("id"));
            List<Entity> res = new List<Entity>(query.List<InspeccionSLA>());
            return res;
        }

        public virtual bool CheckExistence(DateTime periodo)
        {
            string query = "select * from sla where periodo="+Utils.MesAnioToNumberFormat(periodo)+" limit 1";
            ISession session = new Hibernate().ConfigureSession(this);
            IList res = session.CreateSQLQuery(query).List();
            session.Close();
            return (res.Count > 0);
        }

        public virtual bool CheckExistence(DateTime periodo, string aseguradora_alias)
        {
            string query = "SELECT * FROM sla WHERE periodo=" + Utils.MesAnioToNumberFormat(periodo) +
                " "+ ((aseguradora_alias!=null)?("AND aseguradora_alias = '"+aseguradora_alias)+"'":"") + " limit 1";
            ISession session = new Hibernate().ConfigureSession(this);
            IList res = session.CreateSQLQuery(query).List();
            session.Close();
            return (res.Count > 0);
        }

        public virtual Int64 NroInspCentro(DateTime periodo, string aseguradora_alias, int provincia_id)
        {
            string query =
            "select count(*)"
            +" from sla"
            + ((provincia_id != -1) ? (" INNER JOIN lugares ON sla.id_lugares = lugares.id") : "")
            + " where periodo = " + Utils.MesAnioToNumberFormat(periodo)
            + ((aseguradora_alias != null) ? (" AND aseguradora_alias = '" + aseguradora_alias) + "'" : "")
            + " and taller_triki != 'DOMICILIO'"
            + ((provincia_id != -1) ? (" AND cod_provincia = " + provincia_id) : "")
            + " and tipo_inspeccion = 'PREAUT'"
            + " AND tipo_prestador <> 'BT' ";
            ISession session = new Hibernate().ConfigureSession(this);
            Int64 res = session.CreateSQLQuery(query).UniqueResult<Int64>();
            session.Close();
            return (res);
        }

        public virtual Int64 NroInspDomicilio(DateTime periodo, string aseguradora_alias, int provincia_id)
        {
            string query =
            "select count(*)"
            + " from sla"
            + ((provincia_id != -1) ? (" INNER JOIN lugares ON sla.id_lugares = lugares.id") : "")
            + " where periodo = " + Utils.MesAnioToNumberFormat(periodo)
            + ((aseguradora_alias != null) ? (" AND aseguradora_alias = '" + aseguradora_alias) + "'" : "")
            + " and taller_triki = 'DOMICILIO'"
            + ((provincia_id != -1) ? (" AND cod_provincia = " + provincia_id) : "")
            + " and tipo_inspeccion = 'PREAUT'"
            + " AND tipo_prestador <> 'BT' ";
            ISession session = new Hibernate().ConfigureSession(this);
            Int64 res = session.CreateSQLQuery(query).UniqueResult<Int64>();
            session.Close();
            return (res);
        }
    }
}