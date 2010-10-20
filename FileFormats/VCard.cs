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
using System.Text.RegularExpressions;
#endregion

namespace Utilities.FileFormats
{
    /// <summary>
    /// Class for creating vCards
    /// </summary>
    public class VCard
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public VCard()
        {
            Relationships = new List<Relationship>();
        }

        #endregion

        #region Public Functions

        /// <summary>
        /// Gets the vCard
        /// </summary>
        /// <returns>A vCard in string format</returns>
        public string GetVCard()
        {
            StringBuilder Builder = new StringBuilder();
            Builder.Append("BEGIN:VCARD\r\nVERSION:2.1\r\n");
            Builder.Append("FN:");
            if (!string.IsNullOrEmpty(Prefix))
            {
                Builder.Append(Prefix).Append(" ");
            }
            Builder.Append(FirstName + " ");
            if (!string.IsNullOrEmpty(MiddleName))
            {
                Builder.Append(MiddleName).Append(" ");
            }
            Builder.Append(LastName);
            if (!string.IsNullOrEmpty(Suffix))
            {
                Builder.Append(" ").Append(Suffix);
            }
            Builder.Append("\r\n");

            Builder.Append("N:");
            Builder.Append(LastName).Append(";").Append(FirstName).Append(";")
                .Append(MiddleName).Append(";").Append(Prefix).Append(";")
                .Append(Suffix).Append("\r\n");
            Builder.Append("TEL;WORK:").Append(DirectDial).Append("\r\n");
            Builder.Append("EMAIL;INTERNET:").Append(StripHTML(Email)).Append("\r\n");
            Builder.Append("TITLE:").Append(Title).Append("\r\n");
            Builder.Append("ORG:").Append(Organization).Append("\r\n");
            Builder.Append("END:VCARD\r\n");
            return Builder.ToString();
        }

        /// <summary>
        /// Gets the hCard version of the vCard
        /// </summary>
        /// <returns>A hCard in string format</returns>
        public string GetHCard()
        {
            StringBuilder Builder = new StringBuilder();
            Builder.Append("<div class=\"vcard\">");
            if (string.IsNullOrEmpty(Url))
            {
                Builder.Append("<div class=\"fn\">");
                if (!string.IsNullOrEmpty(Prefix))
                {
                    Builder.Append(Prefix).Append(" ");
                }
                Builder.Append(FirstName).Append(" ");
                if (!string.IsNullOrEmpty(MiddleName))
                {
                    Builder.Append(MiddleName).Append(" ");
                }
                Builder.Append(LastName);
                if (!string.IsNullOrEmpty(Suffix))
                {
                    Builder.Append(" ").Append(Suffix);
                }
                Builder.Append("</div>");
            }
            else
            {
                Builder.Append("<a class=\"fn url\" href=\"").Append(Url).Append("\"");
                if (Relationships.Count > 0)
                {
                    Builder.Append(" rel=\"");
                    foreach (Relationship Relationship in Relationships)
                    {
                        Builder.Append(Relationship.ToString()).Append(" ");
                    }
                    Builder.Append("\"");
                }
                Builder.Append(">");
                if (!string.IsNullOrEmpty(Prefix))
                {
                    Builder.Append(Prefix).Append(" ");
                }
                Builder.Append(FirstName).Append(" ");
                if (!string.IsNullOrEmpty(MiddleName))
                {
                    Builder.Append(MiddleName).Append(" ");
                }
                Builder.Append(LastName);
                if (!string.IsNullOrEmpty(Suffix))
                {
                    Builder.Append(" ").Append(Suffix);
                }
                Builder.Append("</a>");
            }
            Builder.Append("<span class=\"n\" style=\"display:none;\"><span class=\"family-name\">").Append(LastName).Append("</span><span class=\"given-name\">").Append(FirstName).Append("</span></span>");
            if (!string.IsNullOrEmpty(DirectDial))
            {
                Builder.Append("<div class=\"tel\"><span class=\"type\">Work</span> ").Append(DirectDial).Append("</div>");
            }
            if (!string.IsNullOrEmpty(Email))
            {
                Builder.Append("<div>Email: <a class=\"email\" href=\"mailto:").Append(StripHTML(Email)).Append("\">").Append(StripHTML(Email)).Append("</a></div>");
            }
            if (!string.IsNullOrEmpty(Organization))
            {
                Builder.Append("<div>Organization: <span class=\"org\">").Append(Organization).Append("</span></div>");
            }
            if (!string.IsNullOrEmpty(Title))
            {
                Builder.Append("<div>Title: <span class=\"title\">").Append(Title).Append("</span></div>");
            }
            Builder.Append("</div>");
            return Builder.ToString();
        }
        #endregion

        #region Private Functions

        private static string StripHTML(string HTML)
        {
            if (string.IsNullOrEmpty(HTML))
                return string.Empty;

            HTML = STRIP_HTML_REGEX.Replace(HTML, string.Empty);
            HTML = HTML.Replace("&nbsp;", " ");
            return HTML.Replace("&#160;", string.Empty);
        }

        private static readonly Regex STRIP_HTML_REGEX = new Regex("<[^>]*>", RegexOptions.Compiled);

        #endregion

        #region Properties
        /// <summary>
        /// First name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Last name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Middle name
        /// </summary>
        public string MiddleName { get; set; }

        /// <summary>
        /// Prefix
        /// </summary>
        public string Prefix { get; set; }

        /// <summary>
        /// Suffix
        /// </summary>
        public string Suffix { get; set; }

        /// <summary>
        /// Work phone number of the individual
        /// </summary>
        public string DirectDial { get; set; }

        /// <summary>
        /// Email of the individual
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Title of the person
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Organization the person belongs to
        /// </summary>
        public string Organization { get; set; }

        /// <summary>
        /// Relationship to the person (uses XFN)
        /// </summary>
        public List<Relationship> Relationships { get; set; }

        /// <summary>
        /// Url to the person's site
        /// </summary>
        public string Url { get; set; }

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