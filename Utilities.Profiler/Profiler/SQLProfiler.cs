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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities.DataTypes.ExtensionMethods;
using System.Globalization;

#endregion

namespace Utilities.Profiler
{
    /// <summary>
    /// Profiler designed to hold SQL related information
    /// </summary>
    public class SQLProfiler : Profiler
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="FunctionName">Function name/Identifier</param>
        /// <param name="Query">Query used</param>
        /// <param name="Parameters">Parameters used</param>
        public SQLProfiler(string FunctionName, string Query, params object[] Parameters)
            : base(FunctionName)
        {
            this.Query = Query;
            this.Parameters = Parameters;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Query that was called
        /// </summary>
        public string Query { get; protected set; }

        /// <summary>
        /// Parameters used by the call
        /// </summary>
        public IEnumerable<object> Parameters { get; protected set; }

        #endregion

        #region Functions

        /// <summary>
        /// Outputs the profiler information as an HTML table
        /// </summary>
        /// <returns>Table containing profiler information</returns>
        public override string ToHTML()
        {
            CompileData();
            StringBuilder Builder = new StringBuilder();
            Builder.AppendFormat(CultureInfo.InvariantCulture, "<tr><td>{0}</td><td>", CalledFrom);
            Builder.AppendFormat(CultureInfo.InvariantCulture, "{0}</td><td>{1}ms</td><td>{2}ms</td><td>{3}ms</td><td>{4}ms</td><td>{5}</td></tr>", Query, Times.Sum(), Times.Max(), Times.Min(), string.Format(CultureInfo.InvariantCulture, "{0:0.##}", Times.Average()), Times.Count);
            foreach (Profiler Child in Children)
            {
                Builder.AppendLineFormat(Child.ToHTML());
            }
            return Builder.ToString();
        }

        /// <summary>
        /// Equals
        /// </summary>
        /// <param name="obj">Object to compare to</param>
        /// <returns>True if they are equal, false otherwise</returns>
        public override bool Equals(object obj)
        {
            SQLProfiler Temp = obj as SQLProfiler;
            if (Temp == null)
                return false;
            return Temp == this;
        }

        /// <summary>
        /// Compares the profilers and determines if they are equal
        /// </summary>
        /// <param name="First">First</param>
        /// <param name="Second">Second</param>
        /// <returns>True if they are equal, false otherwise</returns>
        public static bool operator ==(SQLProfiler First, SQLProfiler Second)
        {
            if ((object)First == null && (object)Second == null)
                return true;
            if ((object)First == null)
                return false;
            if ((object)Second == null)
                return false;
            return First.Function == Second.Function
                && First.Query == Second.Query
                && First.Parameters.All(x => Second.Parameters.Contains(x))
                && Second.Parameters.All(x => First.Parameters.Contains(x));
        }

        /// <summary>
        /// Compares the profilers and determines if they are not equal
        /// </summary>
        /// <param name="First">First</param>
        /// <param name="Second">Second</param>
        /// <returns>True if they are equal, false otherwise</returns>
        public static bool operator !=(SQLProfiler First, SQLProfiler Second)
        {
            return !(First == Second);
        }

        /// <summary>
        /// Gets the hash code for the profiler
        /// </summary>
        /// <returns>The hash code</returns>
        public override int GetHashCode()
        {
            return Function.GetHashCode();
        }

        #endregion
    }
}