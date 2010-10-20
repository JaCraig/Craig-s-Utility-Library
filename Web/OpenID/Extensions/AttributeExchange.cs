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
using System.Web;
using Utilities.DataTypes;
using Utilities.Web.OpenID.Extensions.Enums;
using Utilities.Web.OpenID.Extensions.Interfaces;
#endregion

namespace Utilities.Web.OpenID.Extensions
{
    /// <summary>
    /// Attribute exchange extension
    /// </summary>
    public class AttributeExchange : IExtension
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public AttributeExchange()
        {
            Required = Attributes.None;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Required attributes
        /// </summary>
        public Attributes Required { get; set; }

        #endregion

        #region Functions

        /// <summary>
        /// Gets a dictionary with the requested values
        /// </summary>
        /// <param name="Pairs">Returned attribute pairs</param>
        /// <returns>A dictionary with the requested values</returns>
        public Dictionary<Attributes, string> GetValues(System.Collections.Generic.List<Pair<string, string>> Pairs)
        {
            Dictionary<Attributes, string> ReturnValues = new Dictionary<Attributes, string>();
            Pair<string, string> Pair = Pairs.Find(x => x.Right == "http://openid.net/srv/ax/1.0");
            string[] Splitter = { "." };
            string Extension = Pair.Left.Split(Splitter, StringSplitOptions.None)[2];

            if ((Required & Attributes.Address) == Attributes.Address)
            {
                ReturnValues.Add(Attributes.Address, Pairs.Find(x => x.Left == "openid." + Extension + ".value.address").Right);
            }
            if ((Required & Attributes.BirthDate) == Attributes.BirthDate)
            {
                ReturnValues.Add(Attributes.BirthDate, Pairs.Find(x => x.Left == "openid." + Extension + ".value.birthdate").Right);
            }
            if ((Required & Attributes.CompanyName) == Attributes.CompanyName)
            {
                ReturnValues.Add(Attributes.CompanyName, Pairs.Find(x => x.Left == "openid." + Extension + ".value.companyname").Right);
            }
            if ((Required & Attributes.Country) == Attributes.Country)
            {
                ReturnValues.Add(Attributes.Country, Pairs.Find(x => x.Left == "openid." + Extension + ".value.country").Right);
            }
            if ((Required & Attributes.Email) == Attributes.Email)
            {
                ReturnValues.Add(Attributes.Email, Pairs.Find(x => x.Left == "openid." + Extension + ".value.email").Right);
            }
            if ((Required & Attributes.FirstName) == Attributes.FirstName)
            {
                ReturnValues.Add(Attributes.FirstName, Pairs.Find(x => x.Left == "openid." + Extension + ".value.firstname").Right);
            }
            if ((Required & Attributes.FullName) == Attributes.FullName)
            {
                ReturnValues.Add(Attributes.FullName, Pairs.Find(x => x.Left == "openid." + Extension + ".value.fullname").Right);
            }
            if ((Required & Attributes.Gender) == Attributes.Gender)
            {
                ReturnValues.Add(Attributes.Gender, Pairs.Find(x => x.Left == "openid." + Extension + ".value.gender").Right);
            }
            if ((Required & Attributes.JobTitle) == Attributes.JobTitle)
            {
                ReturnValues.Add(Attributes.JobTitle, Pairs.Find(x => x.Left == "openid." + Extension + ".value.jobtitle").Right);
            }
            if ((Required & Attributes.Language) == Attributes.Language)
            {
                ReturnValues.Add(Attributes.Language, Pairs.Find(x => x.Left == "openid." + Extension + ".value.language").Right);
            }
            if ((Required & Attributes.LastName) == Attributes.LastName)
            {
                ReturnValues.Add(Attributes.LastName, Pairs.Find(x => x.Left == "openid." + Extension + ".value.lastname").Right);
            }
            if ((Required & Attributes.Phone) == Attributes.Phone)
            {
                ReturnValues.Add(Attributes.Phone, Pairs.Find(x => x.Left == "openid." + Extension + ".value.phone").Right);
            }
            if ((Required & Attributes.PostalCode) == Attributes.PostalCode)
            {
                ReturnValues.Add(Attributes.PostalCode, Pairs.Find(x => x.Left == "openid." + Extension + ".value.postalcode").Right);
            }
            if ((Required & Attributes.TimeZone) == Attributes.TimeZone)
            {
                ReturnValues.Add(Attributes.TimeZone, Pairs.Find(x => x.Left == "openid." + Extension + ".value.timezone").Right);
            }
            if ((Required & Attributes.UserName) == Attributes.UserName)
            {
                ReturnValues.Add(Attributes.UserName, Pairs.Find(x => x.Left == "openid." + Extension + ".value.username").Right);
            }
            return ReturnValues;
        }

        public bool Verify(string URL, System.Collections.Generic.List<Pair<string, string>> Pairs)
        {
            Pair<string, string> Pair = Pairs.Find(x => x.Right == "http://openid.net/srv/ax/1.0");
            if (Pair == null && Required != Attributes.None)
                return false;
            else if (Pair == null)
                return true;
            string[] Splitter = { "." };
            string Extension = Pair.Left.Split(Splitter, StringSplitOptions.None)[2];

            if ((Required & Attributes.Address) == Attributes.Address)
            {
                if (Pairs.Find(x => x.Left == "openid." + Extension + ".value.address") == null)
                    return false;
            }
            if ((Required & Attributes.BirthDate) == Attributes.BirthDate)
            {
                if (Pairs.Find(x => x.Left == "openid." + Extension + ".value.birthdate") == null)
                    return false;
            }
            if ((Required & Attributes.CompanyName) == Attributes.CompanyName)
            {
                if (Pairs.Find(x => x.Left == "openid." + Extension + ".value.companyname") == null)
                    return false;
            }
            if ((Required & Attributes.Country) == Attributes.Country)
            {
                if (Pairs.Find(x => x.Left == "openid." + Extension + ".value.country") == null)
                    return false;
            }
            if ((Required & Attributes.Email) == Attributes.Email)
            {
                if (Pairs.Find(x => x.Left == "openid." + Extension + ".value.email") == null)
                    return false;
            }
            if ((Required & Attributes.FirstName) == Attributes.FirstName)
            {
                if (Pairs.Find(x => x.Left == "openid." + Extension + ".value.firstname") == null)
                    return false;
            }
            if ((Required & Attributes.FullName) == Attributes.FullName)
            {
                if (Pairs.Find(x => x.Left == "openid." + Extension + ".value.fullname") == null)
                    return false;
            }
            if ((Required & Attributes.Gender) == Attributes.Gender)
            {
                if (Pairs.Find(x => x.Left == "openid." + Extension + ".value.gender") == null)
                    return false;
            }
            if ((Required & Attributes.JobTitle) == Attributes.JobTitle)
            {
                if (Pairs.Find(x => x.Left == "openid." + Extension + ".value.jobtitle") == null)
                    return false;
            }
            if ((Required & Attributes.Language) == Attributes.Language)
            {
                if (Pairs.Find(x => x.Left == "openid." + Extension + ".value.language") == null)
                    return false;
            }
            if ((Required & Attributes.LastName) == Attributes.LastName)
            {
                if (Pairs.Find(x => x.Left == "openid." + Extension + ".value.lastname") == null)
                    return false;
            }
            if ((Required & Attributes.Phone) == Attributes.Phone)
            {
                if (Pairs.Find(x => x.Left == "openid." + Extension + ".value.phone") == null)
                    return false;
            }
            if ((Required & Attributes.PostalCode) == Attributes.PostalCode)
            {
                if (Pairs.Find(x => x.Left == "openid." + Extension + ".value.postalcode") == null)
                    return false;
            }
            if ((Required & Attributes.TimeZone) == Attributes.TimeZone)
            {
                if (Pairs.Find(x => x.Left == "openid." + Extension + ".value.timezone") == null)
                    return false;
            }
            if ((Required & Attributes.UserName) == Attributes.UserName)
            {
                if (Pairs.Find(x => x.Left == "openid." + Extension + ".value.username") == null)
                    return false;
            }
            return true;
        }

        public System.Collections.Generic.List<Pair<string, string>> GenerateURLAttributes()
        {
            System.Collections.Generic.List<Pair<string, string>> ReturnValues = new System.Collections.Generic.List<Pair<string, string>>();
            if (Required == Attributes.None)
                return ReturnValues;
            ReturnValues.Add(new Pair<string, string>("openid.ns.ax", HttpUtility.UrlEncode("http://openid.net/srv/ax/1.0")));
            ReturnValues.Add(new Pair<string, string>("openid.ax.mode", "fetch_request"));
            if ((Required & Attributes.Address) == Attributes.Address)
            {
                ReturnValues.Add(new Pair<string, string>("openid.ax.type.address", HttpUtility.UrlEncode("http://axschema.org/contact/postalAddress/home")));
            }
            if ((Required & Attributes.BirthDate) == Attributes.BirthDate)
            {
                ReturnValues.Add(new Pair<string, string>("openid.ax.type.birthdate", HttpUtility.UrlEncode("http://axschema.org/birthDate")));
            }
            if ((Required & Attributes.CompanyName) == Attributes.CompanyName)
            {
                ReturnValues.Add(new Pair<string, string>("openid.ax.type.companyname", HttpUtility.UrlEncode("http://axschema.org/company/name")));
            }
            if ((Required & Attributes.Country) == Attributes.Country)
            {
                ReturnValues.Add(new Pair<string, string>("openid.ax.type.country", HttpUtility.UrlEncode("http://axschema.org/contact/country/home")));
            }
            if ((Required & Attributes.Email) == Attributes.Email)
            {
                ReturnValues.Add(new Pair<string, string>("openid.ax.type.email", HttpUtility.UrlEncode("http://axschema.org/contact/email")));
            }
            if ((Required & Attributes.FirstName) == Attributes.FirstName)
            {
                ReturnValues.Add(new Pair<string, string>("openid.ax.type.firstname", HttpUtility.UrlEncode("http://axschema.org/namePerson/first")));
            }
            if ((Required & Attributes.FullName) == Attributes.FullName)
            {
                ReturnValues.Add(new Pair<string, string>("openid.ax.type.fullname", HttpUtility.UrlEncode("http://axschema.org/namePerson")));
            }
            if ((Required & Attributes.Gender) == Attributes.Gender)
            {
                ReturnValues.Add(new Pair<string, string>("openid.ax.type.gender", HttpUtility.UrlEncode("http://axschema.org/person/gender")));
            }
            if ((Required & Attributes.JobTitle) == Attributes.JobTitle)
            {
                ReturnValues.Add(new Pair<string, string>("openid.ax.type.jobtitle", HttpUtility.UrlEncode("http://axschema.org/company/title")));
            }
            if ((Required & Attributes.Language) == Attributes.Language)
            {
                ReturnValues.Add(new Pair<string, string>("openid.ax.type.language", HttpUtility.UrlEncode("http://axschema.org/pref/language")));
            }
            if ((Required & Attributes.LastName) == Attributes.LastName)
            {
                ReturnValues.Add(new Pair<string, string>("openid.ax.type.lastname", HttpUtility.UrlEncode("http://axschema.org/namePerson/last")));
            }
            if ((Required & Attributes.Phone) == Attributes.Phone)
            {
                ReturnValues.Add(new Pair<string, string>("openid.ax.type.phone", HttpUtility.UrlEncode("http://axschema.org/contact/phone/default")));
            }
            if ((Required & Attributes.PostalCode) == Attributes.PostalCode)
            {
                ReturnValues.Add(new Pair<string, string>("openid.ax.type.postalcode", HttpUtility.UrlEncode("http://axschema.org/contact/postalCode/home")));
            }
            if ((Required & Attributes.TimeZone) == Attributes.TimeZone)
            {
                ReturnValues.Add(new Pair<string, string>("openid.ax.type.timezone", HttpUtility.UrlEncode("http://axschema.org/pref/timezone")));
            }
            if ((Required & Attributes.UserName) == Attributes.UserName)
            {
                ReturnValues.Add(new Pair<string, string>("openid.ax.type.username", HttpUtility.UrlEncode("http://axschema.org/namePerson/friendly")));
            }
            ReturnValues.Add(new Pair<string, string>("openid.ax.required", Required.ToString().ToLower().Replace(" ", "")));
            return ReturnValues;
        }

        #endregion
    }
}