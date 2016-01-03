/*
Copyright (c) 2014 <a href="http://www.gutgames.com">James Craig</a>

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

using System;
using System.Runtime.Serialization;
using Utilities.IO;
using Xunit;

namespace UnitTests.IO.ExtensionMethods
{
    public class SerializationExtensions
    {
        [Fact]
        public void SerializeDeserialize()
        {
            var TestObj = new Temp() { A = 100 };
            string Value = TestObj.Serialize<string, Temp>();
            Temp TestObj2 = Value.Deserialize<Temp, string>();
            Assert.Equal("{\"A\":100}", Value);
            Assert.Equal(TestObj.A, TestObj2.A);
            Value = TestObj.Serialize<string, Temp>("application/soap+xml");
            TestObj2 = Value.Deserialize<Temp, string>("application/soap+xml");
            Assert.Equal(@"<SOAP-ENV:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:SOAP-ENC=""http://schemas.xmlsoap.org/soap/encoding/"" xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:clr=""http://schemas.microsoft.com/soap/encoding/clr/1.0"" SOAP-ENV:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/"">
<SOAP-ENV:Body>
<a1:SerializationExtensions_x002B_Temp id=""ref-1"" xmlns:a1=""http://schemas.microsoft.com/clr/nsassem/UnitTests.IO.ExtensionMethods/UnitTests%2C%20Version%3D1.0.0.0%2C%20Culture%3Dneutral%2C%20PublicKeyToken%3Dnull"">
<_x003C_A_x003E_k__BackingField>100</_x003C_A_x003E_k__BackingField>
</a1:SerializationExtensions_x002B_Temp>
</SOAP-ENV:Body>
</SOAP-ENV:Envelope>
", Value);
            Assert.Equal(TestObj.A, TestObj2.A);
            Value = TestObj.Serialize<string, Temp>("text/xml");
            TestObj2 = Value.Deserialize<Temp, string>("text/xml");
            Assert.Equal(@"<?xml version=""1.0""?>
<Temp xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
  <A>100</A>
</Temp>", Value);
            Assert.Equal(TestObj.A, TestObj2.A);
            byte[] Value2 = TestObj.Serialize<byte[], Temp>("application/octet-stream");
            TestObj2 = Value2.Deserialize<Temp, byte[]>("application/octet-stream");
            Assert.Equal(new byte[] { 0, 1, 0, 0, 0, 255, 255, 255, 255, 1, 0, 0, 0, 0, 0, 0, 0, 12, 2, 0, 0, 0, 64, 85, 110, 105, 116, 84, 101, 115, 116, 115, 44, 32, 86, 101, 114, 115, 105, 111, 110, 61, 49, 46, 48, 46, 48, 46, 48, 44, 32, 67, 117, 108, 116, 117, 114, 101, 61, 110, 101, 117, 116, 114, 97, 108, 44, 32, 80, 117, 98, 108, 105, 99, 75, 101, 121, 84, 111, 107, 101, 110, 61, 110, 117, 108, 108, 5, 1, 0, 0, 0, 58, 85, 110, 105, 116, 84, 101, 115, 116, 115, 46, 73, 79, 46, 69, 120, 116, 101, 110, 115, 105, 111, 110, 77, 101, 116, 104, 111, 100, 115, 46, 83, 101, 114, 105, 97, 108, 105, 122, 97, 116, 105, 111, 110, 69, 120, 116, 101, 110, 115, 105, 111, 110, 115, 43, 84, 101, 109, 112, 1, 0, 0, 0, 18, 60, 65, 62, 107, 95, 95, 66, 97, 99, 107, 105, 110, 103, 70, 105, 101, 108, 100, 0, 8, 2, 0, 0, 0, 100, 0, 0, 0, 11 }, Value2);
            Assert.Equal(TestObj.A, TestObj2.A);
        }

        [Fact]
        public void SerializeDeserialize2()
        {
            var TestObj = new Temp() { A = 100 };
            string Value = TestObj.Serialize<string, Temp>(SerializationType.JSON);
            Temp TestObj2 = Value.Deserialize<Temp, string>(SerializationType.JSON);
            Assert.Equal("{\"A\":100}", Value);
            Assert.Equal(TestObj.A, TestObj2.A);
            Value = TestObj.Serialize<string, Temp>(SerializationType.SOAP);
            TestObj2 = Value.Deserialize<Temp, string>(SerializationType.SOAP);
            Assert.Equal(@"<SOAP-ENV:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:SOAP-ENC=""http://schemas.xmlsoap.org/soap/encoding/"" xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:clr=""http://schemas.microsoft.com/soap/encoding/clr/1.0"" SOAP-ENV:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/"">
<SOAP-ENV:Body>
<a1:SerializationExtensions_x002B_Temp id=""ref-1"" xmlns:a1=""http://schemas.microsoft.com/clr/nsassem/UnitTests.IO.ExtensionMethods/UnitTests%2C%20Version%3D1.0.0.0%2C%20Culture%3Dneutral%2C%20PublicKeyToken%3Dnull"">
<_x003C_A_x003E_k__BackingField>100</_x003C_A_x003E_k__BackingField>
</a1:SerializationExtensions_x002B_Temp>
</SOAP-ENV:Body>
</SOAP-ENV:Envelope>
", Value);
            Assert.Equal(TestObj.A, TestObj2.A);
            Value = TestObj.Serialize<string, Temp>(SerializationType.XML);
            TestObj2 = Value.Deserialize<Temp, string>(SerializationType.XML);
            Assert.Equal(@"<?xml version=""1.0""?>
<Temp xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
  <A>100</A>
</Temp>", Value);
            Assert.Equal(TestObj.A, TestObj2.A);
            byte[] Value2 = TestObj.Serialize<byte[], Temp>(SerializationType.Binary);
            TestObj2 = Value2.Deserialize<Temp, byte[]>(SerializationType.Binary);
            Assert.Equal(new byte[] { 0, 1, 0, 0, 0, 255, 255, 255, 255, 1, 0, 0, 0, 0, 0, 0, 0, 12, 2, 0, 0, 0, 64, 85, 110, 105, 116, 84, 101, 115, 116, 115, 44, 32, 86, 101, 114, 115, 105, 111, 110, 61, 49, 46, 48, 46, 48, 46, 48, 44, 32, 67, 117, 108, 116, 117, 114, 101, 61, 110, 101, 117, 116, 114, 97, 108, 44, 32, 80, 117, 98, 108, 105, 99, 75, 101, 121, 84, 111, 107, 101, 110, 61, 110, 117, 108, 108, 5, 1, 0, 0, 0, 58, 85, 110, 105, 116, 84, 101, 115, 116, 115, 46, 73, 79, 46, 69, 120, 116, 101, 110, 115, 105, 111, 110, 77, 101, 116, 104, 111, 100, 115, 46, 83, 101, 114, 105, 97, 108, 105, 122, 97, 116, 105, 111, 110, 69, 120, 116, 101, 110, 115, 105, 111, 110, 115, 43, 84, 101, 109, 112, 1, 0, 0, 0, 18, 60, 65, 62, 107, 95, 95, 66, 97, 99, 107, 105, 110, 103, 70, 105, 101, 108, 100, 0, 8, 2, 0, 0, 0, 100, 0, 0, 0, 11 }, Value2);
            Assert.Equal(TestObj.A, TestObj2.A);
        }

        [Serializable]
        [DataContract]
        public class Temp
        {
            [DataMember(Name = "A", Order = 1)]
            public int A { get; set; }
        }
    }
}