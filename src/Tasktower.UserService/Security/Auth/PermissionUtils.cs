using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tasktower.UserService.Security.Auth
{
    public static class PermissionUtils
    {
        public static ISet<Role> GetPermissionRoles(Permissions roleGroup)
        {
            return roleGroup switch
            {
                Permissions.ADMIN => MergeRoleGroups(RoleGroups.AdminRoles()),

                Permissions.STANDARD => MergeRoleGroups(RoleGroups.StandardRoles()),

                Permissions.READ_USER_SENSITVE => MergeRoleGroups(
                    RoleGroups.AdminRoles(),
                    RoleGroups.UserSensitiveReaderRoles(),
                    RoleGroups.UserSensitiveWriterRoles()),

                Permissions.WRITE_USER_SENSITVE => MergeRoleGroups(
                    RoleGroups.AdminRoles(),
                    RoleGroups.UserSensitiveWriterRoles()),

                _ => new HashSet<Role> { }
            };
        }

        private static ISet<Role> MergeRoleGroups(params ISet<Role>[] roleGroups) {
            return roleGroups.SelectMany(r => r).ToHashSet();
        }
    }
}
