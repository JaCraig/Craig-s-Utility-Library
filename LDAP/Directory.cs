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
    /// Class for helping with AD
    /// </summary>
    public class Directory:IDisposable
    {
        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="UserName">User name used to log in</param>
        /// <param name="Password">Password used to log in</param>
        /// <param name="Path">Path of the LDAP server</param>
        /// <param name="Query">Query to use in the search</param>
        public Directory(string Query,string UserName, string Password, string Path)
        {
            try
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
            catch { throw; }
        }
        #endregion

        #region Public Functions
        /// <summary>
        /// Finds a user by his user name
        /// </summary>
        /// <param name="UserName">User name to search by</param>
        /// <returns>The user's entry</returns>
        public Entry FindUserByUserName(string UserName)
        {
            try
            {
                List<Entry> Entries = FindUsers("samAccountName=" + UserName);
                if (Entries.Count > 0)
                {
                    return Entries[0];
                }
                return null;
            }
            catch { throw; }
        }

        /// <summary>
        /// Finds all active users
        /// </summary>
        /// <param name="Filter">Filter used to modify the query</param>
        /// <param name="args">Additional arguments (used in string formatting</param>
        /// <returns>A list of all active users' entries</returns>
        public List<Entry> FindActiveUsers(string Filter, params object[] args)
        {
            try
            {
                Filter = string.Format(Filter, args);
                Filter = string.Format("(&((userAccountControl:1.2.840.113556.1.4.803:=512)(!(userAccountControl:1.2.840.113556.1.4.803:=2))(!(cn=*$)))({0}))", Filter);
                return FindUsers(Filter);
            }
            catch { throw; }
        }

        /// <summary>
        /// Finds all users
        /// </summary>
        /// <param name="Filter">Filter used to modify the query</param>
        /// <param name="args">Additional arguments (used in string formatting</param>
        /// <returns>A list of all users meeting the specified Filter</returns>
        public List<Entry> FindUsers(string Filter, params object[] args)
        {
            try
            {
                Filter = string.Format(Filter, args);
                Filter = string.Format("(&(objectClass=User)(objectCategory=Person)({0}))", Filter);
                Searcher.Filter = Filter;
                return FindAll();
            }
            catch { throw; }
        }

        /// <summary>
        /// Finds all computers
        /// </summary>
        /// <param name="Filter">Filter used to modify the query</param>
        /// <param name="args">Additional arguments (used in string formatting</param>
        /// <returns>A list of all computers meeting the specified Filter</returns>
        public List<Entry> FindComputers(string Filter, params object[] args)
        {
            try
            {
                Filter = string.Format(Filter, args);
                Filter = string.Format("(&(objectClass=computer)({0}))", Filter);
                Searcher.Filter = Filter;
                return FindAll();
            }
            catch { throw; }
        }

        /// <summary>
        /// Finds all active users and groups
        /// </summary>
        /// <param name="Filter">Filter used to modify the query</param>
        /// <param name="args">Additional arguments (used in string formatting</param>
        /// <returns>A list of all active groups' entries</returns>
        public List<Entry> FindActiveUsersAndGroups(string Filter, params object[] args)
        {
            try
            {
                Filter = string.Format(Filter, args);
                Filter = string.Format("(&((userAccountControl:1.2.840.113556.1.4.803:=512)(!(userAccountControl:1.2.840.113556.1.4.803:=2))(!(cn=*$)))({0}))", Filter);
                return FindUsersAndGroups(Filter);
            }
            catch { throw; }
        }

        /// <summary>
        /// Finds all users and groups
        /// </summary>
        /// <param name="Filter">Filter used to modify the query</param>
        /// <param name="args">Additional arguments (used in string formatting</param>
        /// <returns>A list of all users and groups meeting the specified Filter</returns>
        public List<Entry> FindUsersAndGroups(string Filter, params object[] args)
        {
            try
            {
                Filter = string.Format(Filter, args);
                Filter = string.Format("(&(|(&(objectClass=Group)(objectCategory=Group))(&(objectClass=User)(objectCategory=Person)))({0}))", Filter);
                Searcher.Filter = Filter;
                return FindAll();
            }
            catch { throw; }
        }

        /// <summary>
        /// Finds all active groups
        /// </summary>
        /// <param name="Filter">Filter used to modify the query</param>
        /// <param name="args">Additional arguments (used in string formatting</param>
        /// <returns>A list of all active groups' entries</returns>
        public List<Entry> FindActiveGroups(string Filter, params object[] args)
        {
            try
            {
                Filter = string.Format(Filter, args);
                Filter = string.Format("(&((userAccountControl:1.2.840.113556.1.4.803:=512)(!(userAccountControl:1.2.840.113556.1.4.803:=2))(!(cn=*$)))({0}))", Filter);
                return FindGroups(Filter);
            }
            catch { throw; }
        }

        /// <summary>
        /// Finds all groups
        /// </summary>
        /// <param name="Filter">Filter used to modify the query</param>
        /// <param name="args">Additional arguments (used in string formatting</param>
        /// <returns>A list of all groups meeting the specified Filter</returns>
        public List<Entry> FindGroups(string Filter, params object[] args)
        {
            try
            {
                Filter = string.Format(Filter, args);
                Filter = string.Format("(&(objectClass=Group)(objectCategory=Group)({0}))", Filter);
                Searcher.Filter = Filter;
                return FindAll();
            }
            catch { throw; }
        }

        /// <summary>
        /// Returns a group's list of members
        /// </summary>
        /// <param name="GroupName">The group's name</param>
        /// <returns>A list of the members</returns>
        public List<Utilities.LDAP.Entry> FindActiveGroupMembers(string GroupName)
        {
            try
            {
                List<Utilities.LDAP.Entry> Entries = this.FindGroups("cn=" + GroupName);
                if (Entries.Count < 1)
                    return new List<Utilities.LDAP.Entry>();

                return this.FindActiveUsersAndGroups("memberOf=" + Entries[0].DistinguishedName);
            }
            catch
            {
                return new List<Utilities.LDAP.Entry>();
            }
        }

        /// <summary>
        /// Finds all entries that match the query
        /// </summary>
        /// <returns>A list of all entries that match the query</returns>
        public List<Entry> FindAll()
        {
            try
            {
                List<Entry> ReturnedResults = new List<Entry>();
                SearchResultCollection Results = Searcher.FindAll();
                foreach (SearchResult Result in Results)
                {
                    ReturnedResults.Add(new Entry(Result.GetDirectoryEntry()));
                }
                Results.Dispose();
                return ReturnedResults;
            }
            catch { throw; }
        }

        /// <summary>
        /// Finds one entry that matches the query
        /// </summary>
        /// <returns>A single entry matching the query</returns>
        public Entry FindOne()
        {
            try
            {
                SearchResult Result = Searcher.FindOne();
                return new Entry(Result.GetDirectoryEntry());
            }
            catch { throw; }
        }

        /// <summary>
        /// Closes the directory
        /// </summary>
        public void Close()
        {
            try
            {
                Entry.Close();
            }
            catch { throw; }
        }

        /// <summary>
        /// Checks to see if the person was authenticated
        /// </summary>
        /// <returns>true if they were authenticated properly, false otherwise</returns>
        public bool Authenticate()
        {
            try
            {
                if (!Entry.Guid.ToString().ToLower().Trim().Equals(""))
                {
                    return true;
                }
            }
            catch
            {
            }
            return false;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Path of the server
        /// </summary>
        public string Path
        {
            get { return _Path; }
            set
            {
                _Path = value;
                try
                {
                    if (Entry != null)
                    {
                        Entry.Close();
                    }
                    Entry = new DirectoryEntry(_Path, _UserName, _Password, AuthenticationTypes.Secure);
                    Searcher = new DirectorySearcher(Entry);
                    Searcher.Filter = Query;
                    Searcher.PageSize = 1000;
                }
                catch { throw; }
            }
        }

        /// <summary>
        /// User name used to log in
        /// </summary>
        public string UserName
        {
            get { return _UserName; }
            set
            {
                _UserName = value;
                try
                {
                    if (Entry != null)
                    {
                        Entry.Close();
                    }
                    Entry = new DirectoryEntry(_Path, _UserName, _Password, AuthenticationTypes.Secure);
                    Searcher = new DirectorySearcher(Entry);
                    Searcher.Filter = Query;
                    Searcher.PageSize = 1000;
                }
                catch { throw; }
            }
        }

        /// <summary>
        /// Password used to log in
        /// </summary>
        public string Password
        {
            get { return _Password; }
            set
            {
                _Password = value;
                try
                {
                    if (Entry != null)
                    {
                        Entry.Close();
                    }
                    Entry = new DirectoryEntry(_Path, _UserName, _Password, AuthenticationTypes.Secure);
                    Searcher = new DirectorySearcher(Entry);
                    Searcher.Filter = Query;
                    Searcher.PageSize = 1000;
                }
                catch { throw; }
            }
        }

        /// <summary>
        /// The query that is being made
        /// </summary>
        public string Query
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
        public string SortBy
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

        public void Dispose()
        {
            if (Entry != null)
            {
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
