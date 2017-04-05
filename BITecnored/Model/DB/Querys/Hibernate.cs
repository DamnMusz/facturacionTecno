using BITecnored.Entities;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using System.Collections.Generic;
using System.Diagnostics;

namespace BITecnored.Model.DB.Querys
{
    public class Hibernate
    {
        public ISession ConfigureSession(Entity caller)
        {
            Configuration hibernateConfiguration = new Configuration().Configure(GetType().Assembly, caller.GetSessionConfigFile());
            SchemaMetadataUpdater.QuoteTableAndColumns(hibernateConfiguration);
            ISessionFactory sessionFactory = hibernateConfiguration.BuildSessionFactory();
            ISession session = sessionFactory.OpenSession();
            return session;
        }

        public void Write(Entity caller, IList<Entity> elements)
        {
            ISession session = ConfigureSession(caller);
            ITransaction tx = session.BeginTransaction();
            foreach (Entity element in elements)
                session.Save(element);
            tx.Commit();
            session.Flush();
            session.Close();
        }

        public void Update(Entity caller, IList<Entity> elements)
        {
            ISession session = ConfigureSession(caller);
            ITransaction tx = session.BeginTransaction();
            foreach (Entity element in elements)
                session.Update(element);
            tx.Commit();
            session.Close();
        }

        public void WriteOrUpdate(Entity caller, IList<Entity> elements)
        {
            ISession session = ConfigureSession(caller);
            ITransaction tx = session.BeginTransaction();
            foreach (Entity element in elements)
                session.SaveOrUpdate(element);
            tx.Commit();
            session.Close();
        }

        public IList<Entity> Read(Entity caller)
        {
            ISession session = ConfigureSession(caller);
            caller.SetRunningSession(session);
            IList<Entity> registers = caller.DoRead(session);
            caller.SetRunningSession(null);
            session.Close();
            return registers;
        }

        public IList<Entity> Read_no_close(Entity caller, ISession session)
        {
            caller.SetRunningSession(session);
            IList<Entity> registers = caller.DoRead(session);
            caller.SetRunningSession(null);
            return registers;
        }
    }
}