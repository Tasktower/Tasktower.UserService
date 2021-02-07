using System;

namespace Tasktower.UserService.Domain.CacheOnly
{
    public class RefreshTokenData
    {
        public string RefreshTokenSaltBase64 { get; set; }

        public Guid UserID { get; set; }
    }
}
