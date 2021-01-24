using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tasktower.UserService.DataAccess.SessionFactory;

namespace Tasktower.UserService.DataAccess
{
    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        private readonly INHibernateSessionFactory _nhibernateSessionFactory;
        public UnitOfWorkFactory(INHibernateSessionFactory nHibertnateSessionFactory)
        {
            _nhibernateSessionFactory = nHibertnateSessionFactory;
        }

        public IUnitOfWork Create(bool useDatabase = true, bool useMemStore = true, bool useKeyVault = true)
        {
            NHibernate.ISession dbSession = useDatabase? _nhibernateSessionFactory.OpenNewSession(): null;
            return new UnitOfWork(dbSession);
        }
    }
}
