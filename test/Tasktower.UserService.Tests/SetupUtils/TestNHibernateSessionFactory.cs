using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tasktower.UserService.DataAccess.SessionFactory;
using Tasktower.UserService.Domain;

namespace Tasktower.UserService.Tests.SetupUtils
{
    class TestNHibernateSessionFactory : INHibernateSessionFactory
    {
        private readonly ISessionFactory _sessionFactory;

        public TestNHibernateSessionFactory()
        {
            var sessionFactory = Fluently.Configure()
                .Database(SQLiteConfiguration.Standard.InMemory().ShowSql())
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<User>().Add<User.UserMap>())
                .ExposeConfiguration(cfg => new SchemaExport(cfg).Create(true, true))
                .BuildSessionFactory();
            _sessionFactory = sessionFactory;
        }
        public ISession OpenNewSession()
        {
            return _sessionFactory.OpenSession();
        }
    }
}
