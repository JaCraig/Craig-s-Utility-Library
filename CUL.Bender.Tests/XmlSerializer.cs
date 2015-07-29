using CUL.Serialization;
using Xunit;

namespace CUL.Bender.Tests
{
    public class XmlSerializerTests
    {
        [Fact]
        public void Creation()
        {
            var Serializer = new XmlSerializer();
            Assert.Equal("text/xml", Serializer.ContentType);
            Assert.Equal(".xml", Serializer.FileType);
            Assert.Equal("Bender.XML", Serializer.Name);
        }

        [Fact]
        public void Deserialize()
        {
            var Serializer = new XmlSerializer();
            var Object = (TestObject)Serializer.Deserialize(typeof(TestObject), "<?xml version=\"1.0\" encoding=\"utf-8\"?><TestObject><A>5</A><B>ASDF</B></TestObject>");
            Assert.Equal(5, Object.A);
            Assert.Equal("ASDF", Object.B);
        }

        [Fact]
        public void NullDeserialize()
        {
            var Temp = new XmlSerializer();
            Assert.Equal(null, Temp.Deserialize(typeof(TestObject), null));
            Assert.Equal(null, Temp.Deserialize(typeof(TestObject), ""));
            Assert.Equal(null, Temp.Deserialize(null, ""));
        }

        [Fact]
        public void NullSerialize()
        {
            var Temp = new XmlSerializer();
            Assert.Equal("", Temp.Serialize(typeof(TestObject), null));
            Assert.Equal("", Temp.Serialize(null, new { A = "" }));
        }

        [Fact]
        public void Serialize()
        {
            var Serializer = new XmlSerializer();
            var Object = new TestObject() { A = 5, B = "ASDF" };
            Assert.Equal("<?xml version=\"1.0\" encoding=\"utf-8\"?><TestObject><A>5</A><B>ASDF</B></TestObject>", Serializer.Serialize(Object.GetType(), Object));
        }

        public class TestObject
        {
            public int A { get; set; }

            public string B { get; set; }
        }
    }
}