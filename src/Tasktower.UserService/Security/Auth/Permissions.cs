using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tasktower.UserService.Security.Auth
{
    public enum Permissions
    {
        STANDARD, 
        ADMIN, 
        READ_USER_SENSITVE, 
        WRITE_USER_SENSITVE
    }
}
