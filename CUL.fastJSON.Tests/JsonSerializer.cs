using CUL.Serialization;
using Xunit;

namespace CUL.fastJSON.Tests
{
    public class JsonSerializerTests
    {
        [Fact]
        public void Creation()
        {
            JsonSerializer Serializer = new JsonSerializer();
            Assert.Equal("application/json", Serializer.ContentType);
            Assert.Equal(".json", Serializer.FileType);
            Assert.Equal("fastJSON", Serializer.Name);
            Assert.Equal(typeof(string), Serializer.ReturnType);
        }

        [Fact]
        public void Deserialize()
        {
            JsonSerializer Serializer = new JsonSerializer();
            dynamic Object = (TestObject)Serializer.Deserialize(typeof(TestObject), "{\"A\":5,\"B\":\"ASDF\"}");
            Assert.Equal(5, Object.A);
            Assert.Equal("ASDF", Object.B);
        }

        [Fact]
        public void Serialize()
        {
            JsonSerializer Serializer = new JsonSerializer();
            dynamic Object = new { A = 5, B = "ASDF" };
            Assert.Equal("{\"A\":5,\"B\":\"ASDF\"}", Serializer.Serialize(Object.GetType(), Object));
            Object = new TestObject { A = 5, B = "ASDF" };
            Assert.Equal("{\"A\":5,\"B\":\"ASDF\"}", Serializer.Serialize(Object.GetType(), Object));
        }

        [Fact]
        public void NullSerialize()
        {
            JsonSerializer Serializer = new JsonSerializer();
            Assert.Equal("", Serializer.Serialize(typeof(object), null));
            Assert.Equal("", Serializer.Serialize(null, new { A = "" }));
            Assert.Equal("", Serializer.Serialize(null, null));
        }

        [Fact]
        public void NullDeserialize()
        {
            JsonSerializer Serializer = new JsonSerializer();
            Assert.Equal(null, Serializer.Deserialize(typeof(object), null));
            Assert.Equal(null, Serializer.Deserialize(typeof(object), ""));
            Assert.Equal(null, Serializer.Deserialize(null, "ASDF"));
            Assert.Equal(null, Serializer.Deserialize(null, null));
            Assert.Equal(null, Serializer.Deserialize(null, ""));
        }

        public class TestObject
        {
            public int A { get; set; }

            public string B { get; set; }
        }
    }
}