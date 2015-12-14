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
        /// <param name="exception">Exception to convert</param>
        /// <param name="prefix">Prefix</param>
        /// <param name="suffix">Suffix</param>
        /// <returns>The exception as a string</returns>
        public static string ToString(this Exception exception, string prefix, string suffix = "")
        {
            if (exception == null)
                return "";
            var Builder = new StringBuilder();
            Builder.AppendLine(prefix);
            Builder.AppendLineFormat("Exception: {0}", exception.Message)
                   .AppendLineFormat("Exception Type: {0}", exception.GetType().FullName);
            if (exception.Data != null)
            {
                foreach (object Object in exception.Data)
                    Builder.AppendLineFormat("Data: {0}:{1}", Object, exception.Data[Object]);
            }
            Builder.AppendLineFormat("StackTrace: {0}", exception.StackTrace)
                   .AppendLineFormat("Source: {0}", exception.Source);
            if (exception.InnerException != null)
                Builder.Append(exception.InnerException.ToString(prefix, suffix));
            Builder.AppendLine(suffix);
            return Builder.ToString();
        }
    }
}