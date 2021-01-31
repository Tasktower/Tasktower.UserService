using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tasktower.UserService.BusinessService
{
    public interface ICryptoService
    {
        byte[] GeneratePasswordSalt();
        byte[] PasswordHash(string password, byte[] passwordSalt);
        bool VerifyPasswordHash(string expectedPwd, byte[] givenPasswordHash, byte[] passwordSalt);
    }
}
