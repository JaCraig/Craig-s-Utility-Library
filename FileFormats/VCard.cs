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
                Builder.Append(Prefix + " ");
            }
            Builder.Append(FirstName + " ");
            if (!string.IsNullOrEmpty(MiddleName))
            {
                Builder.Append(MiddleName + " ");
            }
            Builder.Append(LastName);
            if (!string.IsNullOrEmpty(Suffix))
            {
                Builder.Append(" " + Suffix);
            }
            Builder.Append("\r\n");

            Builder.Append("N:");
            Builder.Append(LastName + ";" + FirstName + ";" + MiddleName + ";" + Prefix + ";" + Suffix + "\r\n");
            Builder.Append("TEL;WORK:" + DirectDial + "\r\n");
            Builder.Append("EMAIL;INTERNET:" + Email + "\r\n");
            Builder.Append("TITLE:" + Title + "\r\n");
            Builder.Append("ORG:" + Organization + "\r\n");
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
                    Builder.Append(Prefix + " ");
                }
                Builder.Append(FirstName + " ");
                if (!string.IsNullOrEmpty(MiddleName))
                {
                    Builder.Append(MiddleName + " ");
                }
                Builder.Append(LastName);
                if (!string.IsNullOrEmpty(Suffix))
                {
                    Builder.Append(" " + Suffix);
                }
                Builder.Append("</div>");
            }
            else
            {
                Builder.Append("<a class=\"fn url\" href=\""+Url+"\"");
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
                if (!string.IsNullOrEmpty(Prefix))
                {
                    Builder.Append(Prefix + " ");
                }
                Builder.Append(FirstName + " ");
                if (!string.IsNullOrEmpty(MiddleName))
                {
                    Builder.Append(MiddleName + " ");
                }
                Builder.Append(LastName);
                if (!string.IsNullOrEmpty(Suffix))
                {
                    Builder.Append(" " + Suffix);
                }
                Builder.Append("</a>");
            }
            Builder.Append("<span class=\"n\" style=\"display:none;\"><span class=\"family-name\">" + LastName + "</span><span class=\"given-name\">" + FirstName + "</span></span>");
            if (!string.IsNullOrEmpty(DirectDial))
            {
                Builder.Append("<div class=\"tel\"><span class=\"type\">Work</span> " + DirectDial + "</div>");
            }
            if (!string.IsNullOrEmpty(Email))
            {
                Builder.Append("<div>Email: <a class=\"email\" href=\"mailto:" + Email + "\">" + Email + "</a></div>");
            }
            if (!string.IsNullOrEmpty(Organization))
            {
                Builder.Append("<div>Organization: <span class=\"org\">" + Organization + "</span></div>");
            }
            if (!string.IsNullOrEmpty(Title))
            {
                Builder.Append("<div>Title: <span class=\"title\">" + Title + "</span></div>");
            }
            Builder.Append("</div>");
            return Builder.ToString();
        }
        #endregion

        #region Private Variables
        private string _FirstName;
        private string _LastName;
        private string _MiddleName;
        private string _Prefix;
        private string _Suffix;
        private string _DirectDial;
        private string _Email;
        private string _Title;
        private string _Organization;
        private List<Relationship> _Relationships=new List<Relationship>();
        private string _Url;
        #endregion

        #region Properties
        /// <summary>
        /// First name
        /// </summary>
        public string FirstName
        {
            get { return _FirstName; }
            set { _FirstName = value; }
        }

        /// <summary>
        /// Last name
        /// </summary>
        public string LastName
        {
            get { return _LastName; }
            set { _LastName = value; }
        }

        /// <summary>
        /// Middle name
        /// </summary>
        public string MiddleName
        {
            get { return _MiddleName; }
            set { _MiddleName = value; }
        }

        /// <summary>
        /// Prefix
        /// </summary>
        public string Prefix
        {
            get { return _Prefix; }
            set { _Prefix = value; }
        }

        /// <summary>
        /// Suffix
        /// </summary>
        public string Suffix
        {
            get { return _Suffix; }
            set { _Suffix = value; }
        }

        /// <summary>
        /// Work phone number of the individual
        /// </summary>
        public string DirectDial
        {
            get { return _DirectDial; }
            set { _DirectDial = value; }
        }

        /// <summary>
        /// Email of the individual
        /// </summary>
        public string Email
        {
            get { return _Email; }
            set 
            { 
                _Email = value;
                _Email = Utilities.Web.HTML.StripHTML(_Email);
            }
        }

        /// <summary>
        /// Title of the person
        /// </summary>
        public string Title
        {
            get { return _Title; }
            set { _Title = value; }
        }

        /// <summary>
        /// Organization the person belongs to
        /// </summary>
        public string Organization
        {
            get { return _Organization; }
            set { _Organization = value; }
        }

        /// <summary>
        /// Relationship to the person (uses XFN)
        /// </summary>
        public List<Relationship> Relationships
        {
            get { return _Relationships; }
            set { _Relationships = value; }
        }

        /// <summary>
        /// Url to the person's site
        /// </summary>
        public string Url
        {
            get { return _Url; }
            set { _Url = value; }
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
