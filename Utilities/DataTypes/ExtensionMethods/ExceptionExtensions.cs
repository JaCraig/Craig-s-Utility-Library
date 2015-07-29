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
using System.ComponentModel;
using System.Text;

namespace Utilities.DataTypes
{
    /// <summary>
    /// Class for housing exception specific extensions
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class ExceptionExtensions
    {
        /// <summary>
        /// Converts the exception to a string and appends the specified prefix/suffix (used for logging)
        /// </summary>
        /// <param name="Exception">Exception to convert</param>
        /// <param name="Prefix">Prefix</param>
        /// <param name="Suffix">Suffix</param>
        /// <returns>The exception as a string</returns>
        public static string ToString(this Exception Exception, string Prefix, string Suffix = "")
        {
            if (Exception == null)
                return "";
            var Builder = new StringBuilder();
            Builder.AppendLine(Prefix);
            try
            {
                Builder.AppendLineFormat("Exception: {0}", Exception.Message);
                Builder.AppendLineFormat("Exception Type: {0}", Exception.GetType().FullName);
                if (Exception.Data != null)
                {
                    foreach (object Object in Exception.Data)
                        Builder.AppendLineFormat("Data: {0}:{1}", Object, Exception.Data[Object]);
                }
                Builder.AppendLineFormat("StackTrace: {0}", Exception.StackTrace);
                Builder.AppendLineFormat("Source: {0}", Exception.Source);
                Builder.AppendLineFormat("TargetSite: {0}", Exception.TargetSite);
                Builder.Append(Exception.InnerException.ToString(Prefix, Suffix));
            }
            catch { }
            Builder.AppendLine(Suffix);
            return Builder.ToString();
        }
    }
}