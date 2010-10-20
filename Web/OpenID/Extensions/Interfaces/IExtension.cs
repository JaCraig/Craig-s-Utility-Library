/*
Copyright (c) 2010 <a href="http://www.gutgames.com">James Craig</a>

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
using Utilities.DataTypes;

#endregion

namespace Utilities.Web.OpenID.Extensions.Interfaces
{
    /// <summary>
    /// Extension interface
    /// </summary>
    public interface IExtension
    {
        #region Functions

        /// <summary>
        /// Generates the attributes in a list of pairs
        /// </summary>
        /// <param name="Required">Required attributes</param>
        /// <returns>A list of attribute pairs</returns>
        System.Collections.Generic.List<Pair<string, string>> GenerateURLAttributes();

        /// <summary>
        /// Parses the URL and gets any attribute values passed back
        /// </summary>
        /// <param name="URL">URL</param>
        /// <param name="Pairs">Query string broken down into attribute pairs</param>
        /// <returns>True if it's valid, false otherwise</returns>
        bool Verify(string URL,System.Collections.Generic.List<Pair<string, string>> Pairs);

        #endregion
    }
}
 