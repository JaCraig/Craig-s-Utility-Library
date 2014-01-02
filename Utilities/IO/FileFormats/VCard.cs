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

#region Usings
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using Utilities.IO.FileFormats.BaseClasses;

#endregion

namespace Utilities.IO.FileFormats
{
    /// <summary>
    /// VCard class
    /// </summary>
    public class VCard : StringFormatBase<VCard>
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public VCard()
            : base()
        {
            Relationships = new List<Relationship>();
        }

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
        public ICollection<Relationship> Relationships { get; private set; }

        /// <summary>
        /// Url to the person's site
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Full name
        /// </summary>
        protected string FullName
        {
            get
            {
                StringBuilder Builder = new StringBuilder();
                if (!string.IsNullOrEmpty(Prefix))
                {
                    Builder.AppendFormat("{0} ", Prefix);
                }
                Builder.AppendFormat("{0} ", FirstName);
                if (!string.IsNullOrEmpty(MiddleName))
                {
                    Builder.AppendFormat("{0} ", MiddleName);
                }
                Builder.Append(LastName);
                if (!string.IsNullOrEmpty(Suffix))
                {
                    Builder.AppendFormat(" {0}", Suffix);
                }
                return Builder.ToString();
            }
        }

        /// <summary>
        /// Name
        /// </summary>
        protected string Name
        {
            get
            {
                return new StringBuilder().AppendFormat(CultureInfo.CurrentCulture,"{0};{1};{2};{3};{4}", LastName, FirstName, MiddleName, Prefix, Suffix).ToString();
            }
        }

        #endregion

        #region Public Functions

        /// <summary>
        /// Gets the hCard version of the vCard
        /// </summary>
        /// <returns>A hCard in string format</returns>
        public string HCard()
        {
            StringBuilder Builder = new StringBuilder();
            Builder.Append("<div class=\"vcard\">");
            if (string.IsNullOrEmpty(Url))
            {
                Builder.AppendFormat("<div class=\"fn\">{0}</div>", FullName);
            }
            else
            {
                Builder.AppendFormat("<a class=\"fn url\" href=\"{0}\"", Url);
                if (Relationships.Count > 0)
                {
                    Builder.Append(" rel=\"");
                    foreach (Relationship Relationship in Relationships)
                    {
                        Builder.Append(Relationship.ToString()).Append(" ");
                    }
                    Builder.Append("\"");
                }
                Builder.AppendFormat(">{0}</a>", FullName);
            }
            Builder.AppendFormat("<span class=\"n\" style=\"display:none;\"><span class=\"family-name\">{0}</span><span class=\"given-name\">{1}</span></span>", LastName, FirstName);
            if (!string.IsNullOrEmpty(DirectDial))
            {
                Builder.AppendFormat("<div class=\"tel\"><span class=\"type\">Work</span> {0}</div>", DirectDial);
            }
            if (!string.IsNullOrEmpty(Email))
            {
                Builder.AppendFormat("<div>Email: <a class=\"email\" href=\"mailto:{0}\">{0}</a></div>", StripHTML(Email));
            }
            if (!string.IsNullOrEmpty(Organization))
            {
                Builder.AppendFormat("<div>Organization: <span class=\"org\">{0}</span></div>", Organization);
            }
            if (!string.IsNullOrEmpty(Title))
            {
                Builder.AppendFormat("<div>Title: <span class=\"title\">{0}</span></div>", Title);
            }
            Builder.Append("</div>");
            return Builder.ToString();
        }

        /// <summary>
        /// Gets the VCard as a string
        /// </summary>
        /// <returns>VCard as a string</returns>
        public override string ToString()
        {
            return new StringBuilder().Append("BEGIN:VCARD\r\nVERSION:2.1\r\n")
                .AppendFormat(CultureInfo.CurrentCulture, "FN:{0}\r\n", FullName)
                .AppendFormat(CultureInfo.CurrentCulture, "N:{0}\r\n", Name)
                .AppendFormat(CultureInfo.CurrentCulture, "TEL;WORK:{0}\r\n", DirectDial)
                .AppendFormat(CultureInfo.CurrentCulture, "EMAIL;INTERNET:{0}\r\n", StripHTML(Email))
                .AppendFormat(CultureInfo.CurrentCulture, "TITLE:{0}\r\n", Title)
                .AppendFormat(CultureInfo.CurrentCulture, "ORG:{0}\r\n", Organization)
                .AppendFormat(CultureInfo.CurrentCulture, "END:VCARD\r\n")
                .ToString();
        }

        private static string StripHTML(string HTML)
        {
            if (string.IsNullOrEmpty(HTML))
                return string.Empty;

            HTML = STRIP_HTML_REGEX.Replace(HTML, string.Empty);
            HTML = HTML.Replace("&nbsp;", " ");
            return HTML.Replace("&#160;", string.Empty);
        }

        private static readonly Regex STRIP_HTML_REGEX = new Regex("<[^>]*>", RegexOptions.Compiled);

        /// <summary>
        /// Loads the object from the data specified
        /// </summary>
        /// <param name="Data">Data to load into the object</param>
        protected override void LoadFromData(string Data)
        {
            string Content = Data;
            foreach (Match TempMatch in Regex.Matches(Content, "(?<Title>[^:]+):(?<Value>.*)"))
            {
                if (TempMatch.Groups["Title"].Value.ToUpperInvariant() == "N")
                {
                    string[] Name = TempMatch.Groups["Value"].Value.Split(';');
                    if (Name.Length > 0)
                    {
                        LastName = Name[0];
                        if (Name.Length > 1)
                            FirstName = Name[1];
                        if (Name.Length > 2)
                            MiddleName = Name[2];
                        if (Name.Length > 3)
                            Prefix = Name[3];
                        if (Name.Length > 4)
                            Suffix = Name[4];
                    }
                }
                else if (TempMatch.Groups["Title"].Value.ToUpperInvariant() == "TEL;WORK")
                {
                    DirectDial = TempMatch.Groups["Value"].Value;
                }
                else if (TempMatch.Groups["Title"].Value.ToUpperInvariant() == "EMAIL;INTERNET")
                {
                    Email = TempMatch.Groups["Value"].Value;
                }
                else if (TempMatch.Groups["Title"].Value.ToUpperInvariant() == "TITLE")
                {
                    Title = TempMatch.Groups["Value"].Value;
                }
                else if (TempMatch.Groups["Title"].Value.ToUpperInvariant() == "ORG")
                {
                    Organization = TempMatch.Groups["Value"].Value;
                }
            }
        }

        #endregion
    }

    #region Enums

    /// <summary>
    /// Enum defining relationships (used for XFN markup)
    /// </summary>
    public enum Relationship
    {
        /// <summary>
        /// Friend
        /// </summary>
        Friend,
        /// <summary>
        /// Acquaintance
        /// </summary>
        Acquaintance,
        /// <summary>
        /// Contact
        /// </summary>
        Contact,
        /// <summary>
        /// Met
        /// </summary>
        Met,
        /// <summary>
        /// Coworker
        /// </summary>
        CoWorker,
        /// <summary>
        /// Colleague
        /// </summary>
        Colleague,
        /// <summary>
        /// Coresident
        /// </summary>
        CoResident,
        /// <summary>
        /// Neighbor
        /// </summary>
        Neighbor,
        /// <summary>
        /// Child
        /// </summary>
        Child,
        /// <summary>
        /// Parent
        /// </summary>
        Parent,
        /// <summary>
        /// Sibling
        /// </summary>
        Sibling,
        /// <summary>
        /// Spouse
        /// </summary>
        Spouse,
        /// <summary>
        /// Kin
        /// </summary>
        Kin,
        /// <summary>
        /// Muse
        /// </summary>
        Muse,
        /// <summary>
        /// Crush
        /// </summary>
        Crush,
        /// <summary>
        /// Date
        /// </summary>
        Date,
        /// <summary>
        /// Sweetheart
        /// </summary>
        Sweetheart,
        /// <summary>
        /// Me
        /// </summary>
        Me
    }

    #endregion
}