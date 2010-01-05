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
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
#endregion

namespace Utilities.FileFormats.FOAF
{
    /// <summary>
    /// Container of an individual's information
    /// </summary>
    public class Person
    {
        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        public Person()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Element">Element containing the proper information</param>
        public Person(XmlElement Element)
        {
            if (Element.Name.Equals("foaf:Person", StringComparison.CurrentCultureIgnoreCase))
            {
                foreach (XmlNode Child in Element.ChildNodes)
                {
                    if (Child.Name.Equals("foaf:name", StringComparison.CurrentCultureIgnoreCase))
                    {
                        Name = Child.InnerText;
                    }
                    else if (Child.Name.Equals("foaf:title", StringComparison.CurrentCultureIgnoreCase))
                    {
                        Title = Child.InnerText;
                    }
                    else if (Child.Name.Equals("foaf:givenname", StringComparison.CurrentCultureIgnoreCase))
                    {
                        GivenName = Child.InnerText;
                    }
                    else if (Child.Name.Equals("foaf:family_name", StringComparison.CurrentCultureIgnoreCase))
                    {
                        FamilyName = Child.InnerText;
                    }
                    else if (Child.Name.Equals("foaf:mbox_sha1sum", StringComparison.CurrentCultureIgnoreCase) || Child.Name.Equals("foaf:mbox", StringComparison.CurrentCultureIgnoreCase))
                    {
                        Email.Add(Child.InnerText);
                    }
                    else if (Child.Name.Equals("foaf:homepage", StringComparison.CurrentCultureIgnoreCase))
                    {
                        if (Child.Attributes["rdf:resource"] != null)
                        {
                            Homepage.Add(Child.Attributes["rdf:resource"].Value);
                        }
                    }
                    else if (Child.Name.Equals("foaf:depiction", StringComparison.CurrentCultureIgnoreCase))
                    {
                        if (Child.Attributes["rdf:resource"] != null)
                        {
                            Depiction.Add(Child.Attributes["rdf:resource"].Value);
                        }
                    }
                    else if (Child.Name.Equals("foaf:phone", StringComparison.CurrentCultureIgnoreCase))
                    {
                        if (Child.Attributes["rdf:resource"] != null)
                        {
                            Phone.Add(Child.Attributes["rdf:resource"].Value);
                        }
                    }
                    else if (Child.Name.Equals("foaf:workplacehomepage", StringComparison.CurrentCultureIgnoreCase))
                    {
                        if (Child.Attributes["rdf:resource"] != null)
                        {
                            WorkplaceHomepage = Child.Attributes["rdf:resource"].Value;
                        }
                    }
                    else if (Child.Name.Equals("foaf:workinfohomepage", StringComparison.CurrentCultureIgnoreCase))
                    {
                        if (Child.Attributes["rdf:resource"] != null)
                        {
                            WorkInfoHomepage = Child.Attributes["rdf:resource"].Value;
                        }
                    }
                    else if (Child.Name.Equals("foaf:schoolhomepage", StringComparison.CurrentCultureIgnoreCase))
                    {
                        if (Child.Attributes["rdf:resource"] != null)
                        {
                            SchoolHomepage = Child.Attributes["rdf:resource"].Value;
                        }
                    }
                    else if (Child.Name.Equals("foaf:knows", StringComparison.CurrentCultureIgnoreCase))
                    {
                        foreach (XmlNode Child2 in Child.ChildNodes)
                        {
                            PeopleKnown.Add(new Person((XmlElement)Child2));
                        }
                    }
                    else if (Child.Name.Equals("rdfs:seeAlso", StringComparison.CurrentCultureIgnoreCase))
                    {
                        if (Child.Attributes["rdf:resource"] != null)
                        {
                            SeeAlso = Child.Attributes["rdf:resource"].Value;
                        }
                    }
                }
            }
        }
        #endregion

        #region Public Properties
        private string _SeeAlso = "";
        private string _Name = "";
        private string _Title = "";
        private string _GivenName = "";
        private string _FamilyName = "";
        private string _NickName = "";
        private List<string> _Email = new List<string>();
        private List<string> _Homepage = new List<string>();
        private List<string> _Depiction = new List<string>();
        private List<string> _Phone = new List<string>();
        private string _WorkplaceHomepage = "";
        private string _WorkInfoHomepage = "";
        private string _SchoolHomepage = "";
        private List<Person> _PeopleKnown = new List<Person>();

        /// <summary>
        /// Points to a person's FOAF file
        /// </summary>
        public string SeeAlso
        {
            get { return _SeeAlso; }
            set { _SeeAlso = value; }
        }

        /// <summary>
        /// Name of the individual
        /// </summary>
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        /// <summary>
        /// Title (such as Mr, Ms., etc.)
        /// </summary>
        public string Title
        {
            get { return _Title; }
            set { _Title = value; }
        }

        /// <summary>
        /// Their given name
        /// </summary>
        public string GivenName
        {
            get { return _GivenName; }
            set { _GivenName = value; }
        }

        /// <summary>
        /// Last name/Family name
        /// </summary>
        public string FamilyName
        {
            get { return _FamilyName; }
            set { _FamilyName = value; }
        }

        /// <summary>
        /// Any sort of nick name
        /// </summary>
        public string NickName
        {
            get { return _NickName; }
            set { _NickName = value; }
        }

        /// <summary>
        /// Their home pages
        /// </summary>
        public List<string> Homepage
        {
            get { return _Homepage; }
            set { _Homepage = value; }
        }

        /// <summary>
        /// Image of the person
        /// </summary>
        public List<string> Depiction
        {
            get { return _Depiction; }
            set { _Depiction = value; }
        }

        /// <summary>
        /// Their phone number
        /// </summary>
        public List<string> Phone
        {
            get { return _Phone; }
            set { _Phone = value; }
        }

        /// <summary>
        /// Workplace home page
        /// </summary>
        public string WorkplaceHomepage
        {
            get { return _WorkplaceHomepage; }
            set { _WorkplaceHomepage = value; }
        }

        /// <summary>
        /// Information about what the person does (link to it)
        /// </summary>
        public string WorkInfoHomepage
        {
            get { return _WorkInfoHomepage; }
            set { _WorkInfoHomepage = value; }
        }

        /// <summary>
        /// Link to the school they went/currently going to
        /// </summary>
        public string SchoolHomepage
        {
            get { return _SchoolHomepage; }
            set { _SchoolHomepage = value; }
        }

        /// <summary>
        /// Email addresses associated with the person (may be SHA1 hashes)
        /// </summary>
        public List<string> Email
        {
            get { return _Email; }
            set { _Email = value; }
        }

        /// <summary>
        /// People that this person knows
        /// </summary>
        public List<Person> PeopleKnown
        {
            get { return _PeopleKnown; }
            set { _PeopleKnown = value; }
        }
        #endregion

        #region Public Overridden Functions
        /// <summary>
        /// Outputs the person's information
        /// </summary>
        /// <returns>An rdf/xml formatted string of the person's info</returns>
        public override string ToString()
        {
            StringBuilder Builder = new StringBuilder();
            if(!string.IsNullOrEmpty(Name))
                Builder.Append("<foaf:name>" + Name + "</foaf:name>");
            if(!string.IsNullOrEmpty(Title))
                Builder.Append("<foaf:title>" + Title + "</foaf:title>");
            if(!string.IsNullOrEmpty(GivenName))
                Builder.Append("<foaf:givenname>" + GivenName + "</foaf:givenname>");
            if(!string.IsNullOrEmpty(FamilyName))
                Builder.Append("<foaf:family_name>" + FamilyName + "</foaf:family_name>");
            if(!string.IsNullOrEmpty(NickName))
                Builder.Append("<foaf:nick>" + NickName + "</foaf:nickname>");
            foreach (string CurrentEmail in Email)
            {
                if (!string.IsNullOrEmpty(CurrentEmail))
                {
                    if (CurrentEmail.Contains("@"))
                    {
                        Builder.Append("<foaf:mbox>" + CurrentEmail + "</foaf:mbox>");
                    }
                    else
                    {
                        Builder.Append("<foaf:mbox_sha1sum>" + CurrentEmail + "</foaf:mbox_sha1sum>");
                    }
                }
            }
            foreach (string CurrentHomePage in Homepage)
            {
                if(!string.IsNullOrEmpty(CurrentHomePage))
                    Builder.Append("<foaf:homepage rdf:resource=\"" + CurrentHomePage + "\" />");
            }
            foreach (string CurrentDepiction in Depiction)
            {
                if(!string.IsNullOrEmpty(CurrentDepiction))
                    Builder.Append("<foaf:depiction rdf:resource=\"" + CurrentDepiction + "\" />");
            }
            foreach (string CurrentPhone in Phone)
            {
                if(!string.IsNullOrEmpty(CurrentPhone))
                    Builder.Append("<foaf:phone rdf:resource=\"" + CurrentPhone + "\" />");
            }
            if(!string.IsNullOrEmpty(WorkplaceHomepage))
                Builder.Append("<foaf:workplaceHomepage rdf:resource=\"" + WorkplaceHomepage + "\" />");
            if(!string.IsNullOrEmpty(WorkInfoHomepage))
                Builder.Append("<foaf:workInfoHomepage rdf:resource=\"" + WorkInfoHomepage + "\" />");
            if(!string.IsNullOrEmpty(SchoolHomepage))
                Builder.Append("<foaf:schoolHomepage rdf:resource=\"" + SchoolHomepage + "\" />");
            foreach (Person CurrentPerson in PeopleKnown)
            {
                if (CurrentPerson != null)
                {
                    Builder.Append("<foaf:knows><foaf:Person>");
                    Builder.Append(CurrentPerson.ToString());
                    Builder.Append("</foaf:Person></foaf:knows>");
                }
            }
            if(!string.IsNullOrEmpty(SeeAlso))
                Builder.Append("<rdfs:seeAlso rdf:resource=\"" + SeeAlso + "\"/>");
            return Builder.ToString();
        }
        #endregion
    }
}