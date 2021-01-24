using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Microsoft.Extensions.Configuration;
using NHibernate;
using Tasktower.UserService.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NHibernate.Tool.hbm2ddl;

namespace Tasktower.UserService.DataAccess.SessionFactory
{
    public class NHibertnateSessionFactory : INHibernateSessionFactory
    {
        private readonly ISessionFactory _sessionFactory;

        public NHibertnateSessionFactory(IConfiguration configuration)
        {
            var sessionFactory = Fluently.Configure()
                .Database(PostgreSQLConfiguration.Standard
                    .ConnectionString(configuration.GetConnectionString("pgconnection"))
                    .ShowSql())
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<User>().Add<User.UserMap>())
                .ExposeConfiguration(cfg => new SchemaExport(cfg).Create(false, false))
                .BuildSessionFactory();
            _sessionFactory = sessionFactory;
        }

        public ISession OpenNewSession()
        {
            return _sessionFactory.OpenSession();
        }

    }
}
