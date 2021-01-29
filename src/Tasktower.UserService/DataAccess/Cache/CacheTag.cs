using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tasktower.UserService.DataAccess.Cache
{
    public enum CacheTag
    {
        PASSWORD_RESET_TOKEN,
        REFRESH_TOKEN,
        EMAIL_VERIFY_TOKEN,
        USER
    }
}
