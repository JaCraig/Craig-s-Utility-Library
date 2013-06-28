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


using System.Security.Cryptography;
using Utilities.DataTypes.ExtensionMethods;
using Utilities.Encryption.ExtensionMethods;
using Xunit;

namespace UnitTests.Encryption.ExtensionMethods
{
    public class SymmetricEncryption
    {
        [Fact]
        public void BasicTest()
        {
            string Data = "This is a test of the system.";
            Assert.NotEqual("This is a test of the system.", Data.Encrypt("Babysfirstkey"));
            Assert.Equal("This is a test of the system.", Data.Encrypt("Babysfirstkey").Decrypt("Babysfirstkey"));
            Assert.Equal("This is a test of the system.", Data.Encrypt("Babysfirstkey", AlgorithmUsing: new DESCryptoServiceProvider(), KeySize: 64).Decrypt("Babysfirstkey", AlgorithmUsing: new DESCryptoServiceProvider(), KeySize: 64));
            Assert.Equal("This is a test of the system.", Data.Encrypt("Babysfirstkey", AlgorithmUsing: new TripleDESCryptoServiceProvider(), KeySize: 192).Decrypt("Babysfirstkey", AlgorithmUsing: new TripleDESCryptoServiceProvider(), KeySize: 192));
        }

        [Fact]
        public void BasicTest2()
        {
            string Data = "This is a test of the system.";
            Assert.NotEqual("This is a test of the system.", Data.Encrypt(new Rfc2898DeriveBytes("Babysfirstkey", "Kosher123".ToByteArray(), 2)));
            Assert.Equal("This is a test of the system.", Data.Encrypt(new Rfc2898DeriveBytes("Babysfirstkey", "Kosher123".ToByteArray(), 2)).Decrypt(new Rfc2898DeriveBytes("Babysfirstkey", "Kosher123".ToByteArray(), 2)));
            Assert.Equal("This is a test of the system.", Data.Encrypt(new Rfc2898DeriveBytes("Babysfirstkey", "Kosher123".ToByteArray(), 2), AlgorithmUsing: new DESCryptoServiceProvider(), KeySize: 64).Decrypt(new Rfc2898DeriveBytes("Babysfirstkey", "Kosher123".ToByteArray(), 2), AlgorithmUsing: new DESCryptoServiceProvider(), KeySize: 64));
            Assert.Equal("This is a test of the system.", Data.Encrypt(new Rfc2898DeriveBytes("Babysfirstkey", "Kosher123".ToByteArray(), 2), AlgorithmUsing: new TripleDESCryptoServiceProvider(), KeySize: 192).Decrypt(new Rfc2898DeriveBytes("Babysfirstkey", "Kosher123".ToByteArray(), 2), AlgorithmUsing: new TripleDESCryptoServiceProvider(), KeySize: 192));
        }
    }
}