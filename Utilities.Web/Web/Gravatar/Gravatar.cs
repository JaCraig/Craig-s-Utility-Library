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
using System.Security.Cryptography;
using Utilities.Encryption.ExtensionMethods;
using System.Globalization;
#endregion

namespace Utilities.Web.Gravatar
{
    /// <summary>
    /// Helper for getting Gravatar images
    /// </summary>
    public static class Gravatar
    {
        #region Public Static Functions

        /// <summary>
        /// Gets a Gravatar image link
        /// </summary>
        /// <param name="Email">Email identifier</param>
        /// <param name="AppendJPG">Should jpg be appended to the link?</param>
        /// <returns>The full path to the Gravatar image link</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        public static string GetImageLink(string Email, bool AppendJPG = false)
        {
            string Ending = AppendJPG ? ".jpg" : "";
            return "http://www.gravatar.com/avatar/" + Email.Trim().ToLower(CultureInfo.InvariantCulture).Hash(new MD5CryptoServiceProvider()).ToLower(CultureInfo.InvariantCulture) + Ending;
        }

        #endregion
    }
}