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

using System.Security.Cryptography;
using System.Text;
using Utilities.DataTypes;
using Utilities.IO;
using Xunit;

namespace UnitTests.Encryption.ExtensionMethods
{
    public class EncryptionExtensions
    {
        [Fact]
        public void BasicTest()
        {
            string Data = "This is my test.";
            Assert.Equal("This is my test.", Data.ToByteArray().Encrypt("Babysfirstpassword".ToByteArray()).Decrypt("Babysfirstpassword".ToByteArray()).ToString(new UTF8Encoding()));
        }

        [Fact]
        public void GenerateSalt()
        {
            Assert.Equal(100, new System.Random().GenerateSalt(100).Length);
        }

        [Fact]
        public void HashTest()
        {
            string Data = "This is a test";
            Assert.Equal("A54D88E06612D820BC3BE72877C74F257B561B19", Data.Hash());
            Assert.Equal("CE114E4501D2F4E2DCEA3E17B546F339", Data.Hash("MD5"));
        }

        [Fact]
        public void SaltTest()
        {
            string Data = "This is a test";
            Assert.Equal("This is a test", Data.ToByteArray().Concat(new System.Random().GenerateSalt(100)).ToString(x => ((char)x).ToString(), "").Left(14));
            byte[] Data2 = Data.ToByteArray();
            Assert.Equal(1014, Data2.Concat(new System.Random().GenerateSalt(1000)).Length);
        }

        [Fact]
        public void SymmetricBasicTest()
        {
            string Data = "This is a test of the system.";
            Assert.NotEqual("This is a test of the system.", Data.Encrypt(new PasswordDeriveBytes("Babysfirstkey", "Kosher".ToByteArray(), "SHA1", 2)));
            Assert.Equal("This is a test of the system.", Data.Encrypt(new PasswordDeriveBytes("Babysfirstkey", "Kosher".ToByteArray(), "SHA1", 2), null, "Rijndael").Decrypt(new PasswordDeriveBytes("Babysfirstkey", "Kosher".ToByteArray(), "SHA1", 2), null, "Rijndael"));
            Assert.Equal("This is a test of the system.", Data.Encrypt(new PasswordDeriveBytes("Babysfirstkey", "Kosher".ToByteArray(), "SHA1", 2), null, "Rijndael").Decrypt(new PasswordDeriveBytes("Babysfirstkey", "Kosher".ToByteArray(), "SHA1", 2), null, "Rijndael"));
            Assert.Equal("This is a test of the system.", Data.Encrypt(new PasswordDeriveBytes("Babysfirstkey", "Kosher".ToByteArray(), "SHA1", 2), AlgorithmUsing: "DES", KeySize: 64).Decrypt(new PasswordDeriveBytes("Babysfirstkey", "Kosher".ToByteArray(), "SHA1", 2), AlgorithmUsing: "DES", KeySize: 64));
            Assert.Equal("This is a test of the system.", Data.Encrypt(new PasswordDeriveBytes("Babysfirstkey", "Kosher".ToByteArray(), "SHA1", 2), AlgorithmUsing: "TripleDES", KeySize: 192).Decrypt(new PasswordDeriveBytes("Babysfirstkey", "Kosher".ToByteArray(), "SHA1", 2), AlgorithmUsing: "TripleDES", KeySize: 192));
        }

        [Fact]
        public void SymmetricBasicTest2()
        {
            string Data = "This is a test of the system.";
            Assert.NotEqual("This is a test of the system.", Data.Encrypt(new PasswordDeriveBytes("Babysfirstkey".ToByteArray(), "Kosher123".ToByteArray(), "SHA1", 2)));
            Assert.Equal("This is a test of the system.", Data.Encrypt(new PasswordDeriveBytes("Babysfirstkey".ToByteArray(), "Kosher123".ToByteArray(), "SHA1", 2)).Decrypt(new PasswordDeriveBytes("Babysfirstkey".ToByteArray(), "Kosher123".ToByteArray(), "SHA1", 2)));
            Assert.Equal("This is a test of the system.", Data.Encrypt(new PasswordDeriveBytes("Babysfirstkey".ToByteArray(), "Kosher123".ToByteArray(), "SHA1", 2), AlgorithmUsing: "DES", KeySize: 64).Decrypt(new PasswordDeriveBytes("Babysfirstkey".ToByteArray(), "Kosher123".ToByteArray(), "SHA1", 2), AlgorithmUsing: "DES", KeySize: 64));
            Assert.Equal("This is a test of the system.", Data.Encrypt(new PasswordDeriveBytes("Babysfirstkey".ToByteArray(), "Kosher123".ToByteArray(), "SHA1", 2), AlgorithmUsing: "TripleDES", KeySize: 192).Decrypt(new PasswordDeriveBytes("Babysfirstkey".ToByteArray(), "Kosher123".ToByteArray(), "SHA1", 2), AlgorithmUsing: "TripleDES", KeySize: 192));
        }
    }
}