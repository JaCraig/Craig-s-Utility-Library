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
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
#endregion

namespace Utilities.LDAP
{
    /// <summary>
    /// Class for helping with AD
    /// </summary>
    public class Directory : IDisposable
    {
        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="UserName">User name used to log in</param>
        /// <param name="Password">Password used to log in</param>
        /// <param name="Path">Path of the LDAP server</param>
        /// <param name="Query">Query to use in the search</param>
        public Directory(string Query, string UserName, string Password, string Path)
        {
            Entry = new DirectoryEntry(Path, UserName, Password, AuthenticationTypes.Secure);
            this.Path = Path;
            this.UserName = UserName;
            this.Password = Password;
            this.Query = Query;
            Searcher = new DirectorySearcher(Entry);
            Searcher.Filter = Query;
            Searcher.PageSize = 1000;
        }
        #endregion

        #region Public Functions

        #region Authenticate

        /// <summary>
        /// Checks to see if the person was authenticated
        /// </summary>
        /// <returns>true if they were authenticated properly, false otherwise</returns>
        public virtual bool Authenticate()
        {
            try
            {
                if (!Entry.Guid.ToString().ToLower().Trim().Equals(""))
                    return true;
            }
            catch { }
            return false;
        }
        #endregion

        #region Close

        /// <summary>
        /// Closes the directory
        /// </summary>
        public virtual void Close()
        {
            Entry.Close();
        }

        #endregion

        #region FindActiveGroupMembers

        /// <summary>
        /// Returns a group's list of members who are active
        /// </summary>
        /// <param name="GroupName">The group's name</param>
        /// <returns>A list of the members</returns>
        public virtual List<Entry> FindActiveGroupMembers(string GroupName)
        {
            try
            {
                List<Entry> Entries = this.FindGroups("cn=" + GroupName);
                return (Entries.Count < 1) ? new List<Entry>() : this.FindActiveUsersAndGroups("memberOf=" + Entries[0].DistinguishedName);
            }
            catch
            {
                return new List<Entry>();
            }
        }

        #endregion

        #region FindActiveGroups

        /// <summary>
        /// Finds all active groups
        /// </summary>
        /// <param name="Filter">Filter used to modify the query</param>
        /// <param name="args">Additional arguments (used in string formatting</param>
        /// <returns>A list of all active groups' entries</returns>
        public virtual List<Entry> FindActiveGroups(string Filter, params object[] args)
        {
            Filter = string.Format(Filter, args);
            Filter = string.Format("(&((userAccountControl:1.2.840.113556.1.4.803:=512)(!(userAccountControl:1.2.840.113556.1.4.803:=2))(!(cn=*$)))({0}))", Filter);
            return FindGroups(Filter);
        }

        #endregion

        #region FindActiveUsers

        /// <summary>
        /// Finds all active users
        /// </summary>
        /// <param name="Filter">Filter used to modify the query</param>
        /// <param name="args">Additional arguments (used in string formatting</param>
        /// <returns>A list of all active users' entries</returns>
        public virtual List<Entry> FindActiveUsers(string Filter, params object[] args)
        {
            Filter = string.Format(Filter, args);
            Filter = string.Format("(&((userAccountControl:1.2.840.113556.1.4.803:=512)(!(userAccountControl:1.2.840.113556.1.4.803:=2))(!(cn=*$)))({0}))", Filter);
            return FindUsers(Filter);
        }

        #endregion

        #region FindActiveUsersAndGroups

        /// <summary>
        /// Finds all active users and groups
        /// </summary>
        /// <param name="Filter">Filter used to modify the query</param>
        /// <param name="args">Additional arguments (used in string formatting</param>
        /// <returns>A list of all active groups' entries</returns>
        public virtual List<Entry> FindActiveUsersAndGroups(string Filter, params object[] args)
        {
            Filter = string.Format(Filter, args);
            Filter = string.Format("(&((userAccountControl:1.2.840.113556.1.4.803:=512)(!(userAccountControl:1.2.840.113556.1.4.803:=2))(!(cn=*$)))({0}))", Filter);
            return FindUsersAndGroups(Filter);
        }

        #endregion

        #region FindAll

        /// <summary>
        /// Finds all entries that match the query
        /// </summary>
        /// <returns>A list of all entries that match the query</returns>
        public virtual List<Entry> FindAll()
        {
            List<Entry> ReturnedResults = new List<Entry>();
            using (SearchResultCollection Results = Searcher.FindAll())
            {
                foreach (SearchResult Result in Results)
                    ReturnedResults.Add(new Entry(Result.GetDirectoryEntry()));
            }
            return ReturnedResults;
        }

        #endregion

        #region FindComputers

        /// <summary>
        /// Finds all computers
        /// </summary>
        /// <param name="Filter">Filter used to modify the query</param>
        /// <param name="args">Additional arguments (used in string formatting</param>
        /// <returns>A list of all computers meeting the specified Filter</returns>
        public virtual List<Entry> FindComputers(string Filter, params object[] args)
        {
            Filter = string.Format(Filter, args);
            Filter = string.Format("(&(objectClass=computer)({0}))", Filter);
            Searcher.Filter = Filter;
            return FindAll();
        }

        #endregion

        #region FindGroupMembers

        /// <summary>
        /// Returns a group's list of members
        /// </summary>
        /// <param name="GroupName">The group's name</param>
        /// <returns>A list of the members</returns>
        public virtual List<Entry> FindGroupMembers(string GroupName)
        {
            try
            {
                List<Entry> Entries = this.FindGroups("cn=" + GroupName);
                return (Entries.Count < 1) ? new List<Entry>() : this.FindUsersAndGroups("memberOf=" + Entries[0].DistinguishedName);
            }
            catch
            {
                return new List<Entry>();
            }
        }

        #endregion

        #region FindGroups

        /// <summary>
        /// Finds all groups
        /// </summary>
        /// <param name="Filter">Filter used to modify the query</param>
        /// <param name="args">Additional arguments (used in string formatting</param>
        /// <returns>A list of all groups meeting the specified Filter</returns>
        public virtual List<Entry> FindGroups(string Filter, params object[] args)
        {
            Filter = string.Format(Filter, args);
            Filter = string.Format("(&(objectClass=Group)(objectCategory=Group)({0}))", Filter);
            Searcher.Filter = Filter;
            return FindAll();
        }

        #endregion

        #region FindOne

        /// <summary>
        /// Finds one entry that matches the query
        /// </summary>
        /// <returns>A single entry matching the query</returns>
        public virtual Entry FindOne()
        {
            return new Entry(Searcher.FindOne().GetDirectoryEntry());
        }

        #endregion

        #region FindUsersAndGroups

        /// <summary>
        /// Finds all users and groups
        /// </summary>
        /// <param name="Filter">Filter used to modify the query</param>
        /// <param name="args">Additional arguments (used in string formatting</param>
        /// <returns>A list of all users and groups meeting the specified Filter</returns>
        public virtual List<Entry> FindUsersAndGroups(string Filter, params object[] args)
        {
            Filter = string.Format(Filter, args);
            Filter = string.Format("(&(|(&(objectClass=Group)(objectCategory=Group))(&(objectClass=User)(objectCategory=Person)))({0}))", Filter);
            Searcher.Filter = Filter;
            return FindAll();
        }

        #endregion

        #region FindUserByUserName

        /// <summary>
        /// Finds a user by his user name
        /// </summary>
        /// <param name="UserName">User name to search by</param>
        /// <returns>The user's entry</returns>
        public virtual Entry FindUserByUserName(string UserName)
        {
            if (string.IsNullOrEmpty(UserName))
                throw new ArgumentNullException("UserName");
            return FindUsers("samAccountName=" + UserName).FirstOrDefault();
        }

        #endregion

        #region FindUsers

        /// <summary>
        /// Finds all users
        /// </summary>
        /// <param name="Filter">Filter used to modify the query</param>
        /// <param name="args">Additional arguments (used in string formatting</param>
        /// <returns>A list of all users meeting the specified Filter</returns>
        public virtual List<Entry> FindUsers(string Filter, params object[] args)
        {
            Filter = string.Format(Filter, args);
            Filter = string.Format("(&(objectClass=User)(objectCategory=Person)({0}))", Filter);
            Searcher.Filter = Filter;
            return FindAll();
        }

        #endregion

        #endregion

        #region Properties
        /// <summary>
        /// Path of the server
        /// </summary>
        public virtual string Path
        {
            get { return _Path; }
            set
            {
                _Path = value;
                if (Entry != null)
                {
                    Entry.Close();
                    Entry.Dispose();
                    Entry = null;
                }
                if (Searcher != null)
                {
                    Searcher.Dispose();
                    Searcher = null;
                }
                Entry = new DirectoryEntry(_Path, _UserName, _Password, AuthenticationTypes.Secure);
                Searcher = new DirectorySearcher(Entry);
                Searcher.Filter = Query;
                Searcher.PageSize = 1000;
            }
        }

        /// <summary>
        /// User name used to log in
        /// </summary>
        public virtual string UserName
        {
            get { return _UserName; }
            set
            {
                _UserName = value;
                if (Entry != null)
                {
                    Entry.Close();
                    Entry.Dispose();
                    Entry = null;
                }
                if (Searcher != null)
                {
                    Searcher.Dispose();
                    Searcher = null;
                }
                Entry = new DirectoryEntry(_Path, _UserName, _Password, AuthenticationTypes.Secure);
                Searcher = new DirectorySearcher(Entry);
                Searcher.Filter = Query;
                Searcher.PageSize = 1000;
            }
        }

        /// <summary>
        /// Password used to log in
        /// </summary>
        public virtual string Password
        {
            get { return _Password; }
            set
            {
                _Password = value;
                if (Entry != null)
                {
                    Entry.Close();
                    Entry.Dispose();
                    Entry = null;
                }
                if (Searcher != null)
                {
                    Searcher.Dispose();
                    Searcher = null;
                }
                Entry = new DirectoryEntry(_Path, _UserName, _Password, AuthenticationTypes.Secure);
                Searcher = new DirectorySearcher(Entry);
                Searcher.Filter = Query;
                Searcher.PageSize = 1000;
            }
        }

        /// <summary>
        /// The query that is being made
        /// </summary>
        public virtual string Query
        {
            get { return _Query; }
            set
            {
                _Query = value;
                Searcher.Filter = _Query;
            }
        }

        /// <summary>
        /// Decides what to sort the information by
        /// </summary>
        public virtual string SortBy
        {
            get { return _SortBy; }
            set
            {
                _SortBy = value;
                Searcher.Sort.PropertyName = _SortBy;
                Searcher.Sort.Direction = SortDirection.Ascending;
            }
        }
        #endregion

        #region Private Variables
        private string _Path = "";
        private string _UserName = "";
        private string _Password = "";
        private DirectoryEntry Entry = null;
        private string _Query = "";
        private DirectorySearcher Searcher = null;
        private string _SortBy = "";
        #endregion

        #region IDisposable Members

        /// <summary>
        /// Disposes of the directory object
        /// </summary>
        public void Dispose()
        {
            if (Entry != null)
            {
                Entry.Close();
                Entry.Dispose();
                Entry = null;
            }
            if (Searcher != null)
            {
                Searcher.Dispose();
                Searcher = null;
            }
        }

        #endregion
    }
}