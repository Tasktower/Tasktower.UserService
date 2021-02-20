using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Tasktower.UserService.Domain;
using Xunit;

namespace Tasktower.UserService.Tests.Misc
{
    public class DefaultTest
    {
        [Fact]
        public void DefaultUser_null_fromUserType() {
            UserProfile user = default(UserProfile);
            Assert.Null(user);
        }

        [Fact]
        public void DefaultString_null_fromStringType()
        {
            string str = default(string);
            Assert.Null(str);
        }
    }
}
