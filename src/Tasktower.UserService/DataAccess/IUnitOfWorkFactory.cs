using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tasktower.UserService.DataAccess
{
    public interface IUnitOfWorkFactory
    {
        /// <summary>
        /// Creates a unit of work. 
        /// Cache is seperated from the database as it should be used in the business logic. 
        /// This allows for more flexible caching.
        /// </summary>
        /// <param name="useDatabase">Main database is usable if true</param>
        /// <param name="useKeyVault">Key vault is usable if true</param>
        /// <param name="useCache">Cache repositories are usable if true</param>
        /// <returns></returns>
        IUnitOfWork Create(bool useDatabase = true, bool useCache = true);
    }
}
