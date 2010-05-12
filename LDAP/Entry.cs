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
using System.DirectoryServices;
#endregion

namespace Utilities.LDAP
{
    /// <summary>
    /// Directory entry class
    /// </summary>
    public class Entry:IDisposable
    {
        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="DirectoryEntry">Directory entry for the item</param>
        public Entry(DirectoryEntry DirectoryEntry)
        {
            this._DirectoryEntry = DirectoryEntry;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Actual base directory entry
        /// </summary>
        public DirectoryEntry DirectoryEntry
        {
            get { return _DirectoryEntry; }
            set { _DirectoryEntry = value; }
        }

        /// <summary>
        /// Email property for this entry
        /// </summary>
        public string Email
        {
            get { return (string)GetValue("mail"); }
            set { SetValue("mail", value); }
        }

        /// <summary>
        /// distinguished name property for this entry
        /// </summary>
        public string DistinguishedName
        {
            get { return (string)GetValue("distinguishedname"); }
            set { SetValue("distinguishedname", value); }
        }

        /// <summary>
        /// country code property for this entry
        /// </summary>
        public string CountryCode
        {
            get { return (string)GetValue("countrycode"); }
            set { SetValue("countrycode", value); }
        }

        /// <summary>
        /// company property for this entry
        /// </summary>
        public string Company
        {
            get { return (string)GetValue("company"); }
            set { SetValue("company", value); }
        }

        /// <summary>
        /// MemberOf property for this entry
        /// </summary>
        public List<string> MemberOf
        {
            get 
            {
                List<string> Values = new List<string>();
                PropertyValueCollection Collection = DirectoryEntry.Properties["memberof"];
                foreach (object Item in Collection)
                {
                    Values.Add((string)Item);
                }
                return Values;
            }
        }

        /// <summary>
        /// display name property for this entry
        /// </summary>
        public string DisplayName
        {
            get { return (string)GetValue("displayname"); }
            set { SetValue("displayname", value); }
        }

        /// <summary>
        /// initials property for this entry
        /// </summary>
        public string Initials
        {
            get { return (string)GetValue("initials"); }
            set { SetValue("initials", value); }
        }

        /// <summary>
        /// title property for this entry
        /// </summary>
        public string Title
        {
            get { return (string)GetValue("title"); }
            set { SetValue("title", value); }
        }

        /// <summary>
        /// samaccountname property for this entry
        /// </summary>
        public string SamAccountName
        {
            get { return (string)GetValue("samaccountname"); }
            set { SetValue("samaccountname", value); }
        }

        /// <summary>
        /// givenname property for this entry
        /// </summary>
        public string GivenName
        {
            get { return (string)GetValue("givenname"); }
            set { SetValue("givenname", value); }
        }

        /// <summary>
        /// cn property for this entry
        /// </summary>
        public string CN
        {
            get { return (string)GetValue("cn"); }
            set { SetValue("cn", value); }
        }

        /// <summary>
        /// name property for this entry
        /// </summary>
        public string Name
        {
            get { return (string)GetValue("name"); }
            set { SetValue("name", value); }
        }

        /// <summary>
        /// office property for this entry
        /// </summary>
        public string Office
        {
            get { return (string)GetValue("physicaldeliveryofficename"); }
            set { SetValue("physicaldeliveryofficename", value); }
        }

        /// <summary>
        /// telephone number property for this entry
        /// </summary>
        public string TelephoneNumber
        {
            get { return (string)GetValue("telephonenumber"); }
            set { SetValue("telephonenumber", value); }
        }
        #endregion

        #region Public Functions
        /// <summary>
        /// Saves any changes that have been made
        /// </summary>
        public void Save()
        {
            try
            {
                _DirectoryEntry.CommitChanges();
            }
            catch { throw; }
        }

        /// <summary>
        /// Gets a value from the entry
        /// </summary>
        /// <param name="Property">Property you want the information about</param>
        /// <returns>an object containing the property's information</returns>
        public object GetValue(string Property)
        {
            try
            {
                PropertyValueCollection Collection = DirectoryEntry.Properties[Property];
                if (Collection != null)
                {
                    return Collection.Value;
                }
                return null;
            }
            catch { throw; }
        }

        /// <summary>
        /// Sets a property of the entry to a specific value
        /// </summary>
        /// <param name="Property">Property of the entry to set</param>
        /// <param name="Value">Value to set the property to</param>
        public void SetValue(string Property,object Value)
        {
            try
            {
                PropertyValueCollection Collection = DirectoryEntry.Properties[Property];
                if (Collection != null)
                {
                    Collection.Value = Value;
                }
            }
            catch { throw; }
        }
        #endregion

        #region Private Variables
        private DirectoryEntry _DirectoryEntry;
        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            if (_DirectoryEntry != null)
            {
                _DirectoryEntry.Dispose();
                _DirectoryEntry = null;
            }
        }

        #endregion
    }
}
