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
    /// List used for displaying XFN data
    /// </summary>
    public class XFNList
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public XFNList()
        {
            People = new List<People>();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// List of people
        /// </summary>
        public List<People> People { get; set; }

        #endregion

        #region Public Overridden Function

        /// <summary>
        /// Returns an HTML formatted string containing the information
        /// </summary>
        /// <returns>An HTML formatted string containing the information</returns>
        public override string ToString()
        {
            StringBuilder Builder = new StringBuilder();
            foreach (People CurrentPerson in People)
            {
                Builder.Append(CurrentPerson.ToString());
            }
            return Builder.ToString();
        }

        #endregion
    }

    #region Enums
    /// <summary>
    /// Enum defining relationships (used for XFN markup)
    /// </summary>
    public enum Relationship
    {
        Friend,
        Acquaintance,
        Contact,
        Met,
        CoWorker,
        Colleague,
        CoResident,
        Neighbor,
        Child,
        Parent,
        Sibling,
        Spouse,
        Kin,
        Muse,
        Crush,
        Date,
        Sweetheart,
        Me
    }
    #endregion
}