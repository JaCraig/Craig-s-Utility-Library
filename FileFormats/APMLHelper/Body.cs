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

namespace Utilities.FileFormats.APMLHelper
{
    /// <summary>
    /// Body class
    /// </summary>
    public class Body
    {
        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        public Body()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Element">XmlElement containing the body information</param>
        public Body(XmlElement Element)
        {
            if (Element.Name.Equals("body", StringComparison.CurrentCultureIgnoreCase))
            {
                if (Element.Attributes["defaultprofile"] != null)
                {
                    DefaultProfile = Element.Attributes["defaultprofile"].Value;
                }
                foreach (XmlNode Child in Element.ChildNodes)
                {
                    try
                    {
                        if (Child.Name.Equals("profile", StringComparison.CurrentCultureIgnoreCase))
                        {
                            Profiles.Add(new Profile((XmlElement)Child));
                        }
                        else if (Child.Name.Equals("Applications", StringComparison.CurrentCultureIgnoreCase))
                        {
                            Applications = new Applications((XmlElement)Child);
                        }
                    }
                    catch { }
                }
            }
        }
        #endregion

        #region Properties
        /// <summary>
        /// Default Profile
        /// </summary>
        public string DefaultProfile
        {
            get { return _DefaultProfile; }
            set { _DefaultProfile = value; }
        }
        private string _DefaultProfile="";

        /// <summary>
        /// The list of profiles
        /// </summary>
        public List<Profile> Profiles
        {
            get { return _Profiles; }
            set { _Profiles = value; }
        }
        private List<Profile> _Profiles=new List<Profile>();

        /// <summary>
        /// The list of applications
        /// </summary>
        public Applications Applications
        {
            get { return _Applications; }
            set { _Applications = value; }
        }
        private Applications _Applications=null;
        #endregion

        #region Overriden Functions
        public override string ToString()
        {
            StringBuilder Builder = new StringBuilder();
            Builder.Append("<Body defaultprofile=\""+DefaultProfile+"\">");
            foreach (Profile Profile in Profiles)
            {
                Builder.Append(Profile.ToString());
            }
            Builder.Append(Applications.ToString());
            Builder.Append("</Body>");
            return Builder.ToString();
        }
        #endregion
    }
}
