/*
Copyright (c) 2012 <a href="http://www.gutgames.com">James Craig</a>

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.*/

#region Usings

using System;
using System.IO;
using System.Xml;
using UnitTests.Fixtures;
using Utilities.IO.ExtensionMethods;
using Utilities.IO.Serializers;
using Xunit;

#endregion Usings

namespace UnitTests.IO.ExtensionMethods
{
    public class Serialization : TestingDirectoryFixture
    {
        public Serialization()
        {
            TestItem = new TestClass(); TestItem.ID = 123; TestItem.Content = "This is test content";
        }

        public TestClass TestItem { get; set; }

        public void SetFixture(TestingDirectoryFixture data)
        {
        }

        [Fact]
        public void ToBinary()
        {
            byte[] Content = TestItem.SerializeBinary(FileLocation: @".\Testing\Test.dat");
            Assert.NotNull(Content);
            Assert.NotEmpty(Content);
            TestClass Temp = Content.Deserialize<TestClass>();
            Assert.Equal(123, Temp.ID);
            Assert.Equal("This is test content", Temp.Content);
        }

        [Fact]
        public void ToJSON()
        {
            Assert.Equal(@"{""<Content>k__BackingField"":""This is test content"",""<ID>k__BackingField"":123}", TestItem.Serialize(FileLocation: @".\Testing\Test.dat"));
            TestClass Temp = new FileInfo(@".\Testing\Test.dat").Read().Deserialize<TestClass>();
            Assert.Equal(123, Temp.ID);
            Assert.Equal("This is test content", Temp.Content);
        }

        [Fact]
        public void ToJSON2()
        {
            TestItem.Serialize(FileLocation: @".\Testing\Test.dat");
            TestClass Temp = new FileInfo(@".\Testing\Test.dat").Deserialize<TestClass>();
            Assert.Equal(123, Temp.ID);
            Assert.Equal("This is test content", Temp.Content);
        }

        [Fact]
        public void ToSOAP()
        {
            Assert.Equal(@"<SOAP-ENV:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:SOAP-ENC=""http://schemas.xmlsoap.org/soap/encoding/"" xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:clr=""http://schemas.microsoft.com/soap/encoding/clr/1.0"" SOAP-ENV:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/"">
<SOAP-ENV:Body>
<a1:TestClass id=""ref-1"" xmlns:a1=""http://schemas.microsoft.com/clr/nsassem/UnitTests.IO.ExtensionMethods/UnitTests%2C%20Version%3D1.0.0.0%2C%20Culture%3Dneutral%2C%20PublicKeyToken%3Dc774ddc815643583"">
<_x003C_ID_x003E_k__BackingField>123</_x003C_ID_x003E_k__BackingField>
<_x003C_Content_x003E_k__BackingField id=""ref-3"">This is test content</_x003C_Content_x003E_k__BackingField>
</a1:TestClass>
</SOAP-ENV:Body>
</SOAP-ENV:Envelope>
", TestItem.Serialize(new SOAPSerializer(), FileLocation: @".\Testing\Test.xml"));
            TestClass Temp = new FileInfo(@".\Testing\Test.xml").Read().Deserialize<TestClass>(new SOAPSerializer());
            Assert.Equal(123, Temp.ID);
            Assert.Equal("This is test content", Temp.Content);
        }

        [Fact]
        public void ToSOAP2()
        {
            TestItem.Serialize(new SOAPSerializer(), FileLocation: @".\Testing\Test.xml");
            TestClass Temp = new FileInfo(@".\Testing\Test.xml").Deserialize<TestClass>(new SOAPSerializer());
            Assert.Equal(123, Temp.ID);
            Assert.Equal("This is test content", Temp.Content);
        }

        [Fact]
        public void ToXML()
        {
            Assert.NotNull(TestItem.Serialize(new XMLSerializer(), FileLocation: @".\Testing\Test.xml"));
            TestClass Temp = new FileInfo(@".\Testing\Test.xml").Read().Deserialize<TestClass>(new XMLSerializer());
            Assert.Equal(123, Temp.ID);
            Assert.Equal("This is test content", Temp.Content);
        }

        [Fact]
        public void ToXML2()
        {
            Assert.NotNull(TestItem.Serialize(new XMLSerializer(), FileLocation: @".\Testing\Test.xml"));
            XmlDocument Document = new XmlDocument();
            Document.LoadXml(new FileInfo(@".\Testing\Test.xml").Read());
            TestClass Temp = Document.Deserialize<TestClass>(new XMLSerializer());
            Assert.Equal(123, Temp.ID);
            Assert.Equal("This is test content", Temp.Content);
        }

        [Fact]
        public void ToXML3()
        {
            Assert.NotNull(TestItem.Serialize(new XMLSerializer(), FileLocation: @".\Testing\Test.xml"));
            TestClass Temp = new FileInfo(@".\Testing\Test.xml").Deserialize<TestClass>(new XMLSerializer());
            Assert.Equal(123, Temp.ID);
            Assert.Equal("This is test content", Temp.Content);
        }
    }

    [Serializable]
    public class TestClass
    {
        public string Content { get; set; }

        public int ID { get; set; }
    }
}