using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tasktower.UserService.DataAccess.SessionFactory
{
    public interface INHibernateSessionFactory
    {
        ISession OpenNewSession();
    }
}
