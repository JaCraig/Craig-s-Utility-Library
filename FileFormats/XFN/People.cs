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
using System.Collections.Generic;
using System.Text;
#endregion

namespace Utilities.FileFormats.XFN
{
    /// <summary>
    /// Contains an individual's information
    /// </summary>
    public class People
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public People()
        {
            Relationships = new List<Relationship>();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Person's name
        /// </summary>
        public string Name{get;set;}

        /// <summary>
        /// Person's URL
        /// </summary>
        public string URL{get;set;}

        /// <summary>
        /// Person's relationships
        /// </summary>
        public List<Relationship> Relationships{get;set;}

        #endregion

        #region Public Overridden Function

        /// <summary>
        /// Returns an HTML formatted string containing the information
        /// </summary>
        /// <returns>An HTML formatted string containing the information</returns>
        public override string ToString()
        {
            try
            {
                StringBuilder Builder = new StringBuilder();
                Builder.Append("<a href=\"" + URL + "\"");
                if (Relationships.Count > 0)
                {
                    Builder.Append(" rel=\"");
                    foreach (Relationship Relationship in Relationships)
                    {
                        Builder.Append(Relationship.ToString() + " ");
                    }
                    Builder.Append("\"");
                }
                Builder.Append(">");
                Builder.Append(Name);
                Builder.Append("</a>");
                return Builder.ToString();
            }
            catch { throw; }
        }

        #endregion
    }
}
