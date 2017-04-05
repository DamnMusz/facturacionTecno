using BITecnored.Model.DB.Querys;
using NHibernate;
using System.Collections.Generic;

namespace BITecnored.Entities
{
    public abstract class Entity
    {
        public static string TIPO_PREVIA_AUTO = "PREAUT";
        public static string BONIFICADO_TECNORED = "BT";
        public static int ASEGURADORA_PRUEBA = 11;
        public abstract IList<Entity> DoRead(ISession session);
        protected ISession runningSession = null;
        protected ReaderStrategy reader = null;
        
        public abstract class ReaderStrategy
        {
            public abstract IList<Entity> DoRead(Entity caller, ISession session);
        }

        public virtual IList<Entity> ToList()
        {
            IList<Entity> list = new List<Entity>();
            list.Add(this);
            return list;
        }

        public virtual void ResetReader()
        {
            reader = null;
        }

        public virtual IList<Entity> Read()
        {
            return new Hibernate().Read(this);
        }

        public virtual IList<Entity> Read(ISession session)
        {
            return new Hibernate().Read_no_close(this, session);
        }

        public virtual ISession StartSession()
        {
            return new Hibernate().ConfigureSession(this);
        }

        public virtual void CloseSession(ISession session)
        {
            session.Close();
        }

        public virtual void Write()
        {
            Write(ToList());
        }

        public virtual void Write(IList<Entity> elements)
        {
            new Hibernate().Write(this, elements);
        }

        public static void WriteAll(IList<Entity> list)
        {
            if (list.Count > 0)
                list[0].Write(list);
        }

        public virtual void Update()
        {
            Update(ToList());
        }

        public virtual void Update(IList<Entity> elements)
        {
            new Hibernate().Update(this, elements);
        }

        public static void UpdateAll(IList<Entity> list)
        {
            if (list.Count > 0)
                list[0].Update(list);
        }

        public virtual void WriteOrUpdate()
        {
            WriteOrUpdate(ToList());
        }

        public virtual void WriteOrUpdate(IList<Entity> elements)
        {
            new Hibernate().WriteOrUpdate(this, elements);
        }

        public static void WriteOrUpdateAll(IList<Entity> list)
        {
            if (list.Count > 0)
                list[0].Update(list);
        }

        public virtual string GetSessionConfigFile()
        {
            return "BITecnored.HibernateAgenda.cfg.xml";
        }

        public virtual bool IsRunning()
        {
            return runningSession != null;
        }

        public virtual void Cancel()
        {
            if (IsRunning())
            {
                runningSession.CancelQuery();
                runningSession = null;
            }
        }

        public virtual void SetRunningSession(ISession session)
        {
            this.runningSession = session;
        }

        public virtual ISession GetRunningSession()
        {
            return this.runningSession;
        }
    }
}