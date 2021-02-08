using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tasktower.UserService.Security.Auth
{
    static class RoleGroups
    {
        public static ISet<Role> AdminRoles()
        {
            return new HashSet<Role> { Role.SUPERUSER, Role.ADMINISTRATOR };
        }

        public static ISet<Role> UserSensitiveReaderRoles()
        {
            return new HashSet<Role> { Role.USER_SENSITIVE_READER, Role.USER_SENSITIVE_WRITER };
        }

        public static ISet<Role> UserSensitiveWriterRoles()
        {
            return new HashSet<Role> {Role.USER_SENSITIVE_WRITER };
        }


        public static ISet<Role> StandardRoles()
        {
            return new HashSet<Role> { Role.STANDARD };
        }
    }
}
