using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tasktower.UserService.DataAccess
{
    public interface IUnitOfWorkFactory
    {
        IUnitOfWork Create(bool useDatabase = true, bool useMemStore = true, bool useKeyVault = true);
    }
}
