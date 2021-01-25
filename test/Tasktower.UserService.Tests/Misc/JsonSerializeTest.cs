using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace Tasktower.UserService.Tests.Misc
{
    public class JsonSerializeTest
    {
        [Fact]
        public void SerializeString_StringSerializable_FromNonJsonString() {
            var someStr = "Hello world!!!";
            var json = JsonSerializer.Serialize(someStr);
            var deserialized = JsonSerializer.Deserialize<string>(json);
            Assert.Equal(someStr, deserialized);
            Assert.Equal($"\"{someStr}\"", json);
        }
    }
}
