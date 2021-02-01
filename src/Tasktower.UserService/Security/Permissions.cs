using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tasktower.UserService.Security
{
    public static class Permissions
    {
        public static IEnumerable<Role> CanSeeOtherUsers() 
        {
            return RoleGroups.AdminRoles();
        }
    }
}
