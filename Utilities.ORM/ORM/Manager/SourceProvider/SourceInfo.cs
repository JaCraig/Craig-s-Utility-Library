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

using System.Configuration;
using System.Text.RegularExpressions;
using Utilities.DataTypes;
using Utilities.ORM.Interfaces;
using Utilities.ORM.Manager.SourceProvider.Interfaces;

namespace Utilities.ORM.Manager.SourceProvider
{
    /// <summary>
    /// Source info class
    /// </summary>
    public class SourceInfo : ISourceInfo
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public SourceInfo(string Connection, string Name, string SourceType = "System.Data.SqlClient",
                        string ParameterPrefix = "@", bool Writable = true, bool Readable = true,
                        IDatabase Database = null)
        {
            this.Name = string.IsNullOrEmpty(Name) && ConfigurationManager.ConnectionStrings[0] != null ? ConfigurationManager.ConnectionStrings[0].Name : Name;
            if (string.IsNullOrEmpty(Connection) && ConfigurationManager.ConnectionStrings[this.Name] != null)
            {
                this.Connection = ConfigurationManager.ConnectionStrings[this.Name].ConnectionString;
                this.SourceType = ConfigurationManager.ConnectionStrings[this.Name].ProviderName;
            }
            else if (string.IsNullOrEmpty(Connection))
            {
                this.Connection = Name;
                this.SourceType = SourceType;
            }
            else
            {
                this.Connection = Connection;
                this.SourceType = SourceType;
            }
            if (string.IsNullOrEmpty(this.SourceType))
            {
                this.SourceType = "System.Data.SqlClient";
            }
            if (string.IsNullOrEmpty(ParameterPrefix))
            {
                if (SourceType.Contains("MySql"))
                    this.ParameterPrefix = "?";
                else if (SourceType.Contains("Oracle"))
                    this.ParameterPrefix = ":";
                else
                {
                    this.Server = Regex.Match(this.Connection, @"Data Source=([^;]*)").Groups[1].Value;
                    this.DatabaseName = Regex.Match(this.Connection, @"Initial Catalog=([^;]*)").Groups[1].Value;
                    this.UserName = Regex.Match(this.Connection, @"User ID=([^;]*)").Groups[1].Value;
                    this.Password = Regex.Match(this.Connection, @"Password=([^;]*)").Groups[1].Value;
                    this.ParameterPrefix = "@";
                }
            }
            else
                this.ParameterPrefix = ParameterPrefix;
            this.Writable = Writable;
            this.Readable = Readable;
            this.Database = Database.Check(new DefaultDatabase());
            this.Order = this.Database.Order;
        }

        /// <summary>
        /// Connection string
        /// </summary>
        public string Connection { get; protected set; }

        /// <summary>
        /// The database object associated with the source info (if one is associated with it)
        /// </summary>
        public IDatabase Database { get; protected set; }

        /// <summary>
        /// Gets the database.
        /// </summary>
        /// <value>
        /// The database.
        /// </value>
        public string DatabaseName { get; protected set; }

        /// <summary>
        /// Name of the source
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// Order that the source is generally used in
        /// </summary>
        public int Order { get; protected set; }

        /// <summary>
        /// Parameter prefix that the source uses
        /// </summary>
        public string ParameterPrefix { get; protected set; }

        /// <summary>
        /// Gets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        public string Password { get; protected set; }

        /// <summary>
        /// Should this source be used to read data?
        /// </summary>
        public bool Readable { get; protected set; }

        /// <summary>
        /// Gets the data source.
        /// </summary>
        /// <value>
        /// The data source.
        /// </value>
        public string Server { get; protected set; }

        /// <summary>
        /// Source type, based on ADO.Net provider name or identifier used by CUL
        /// </summary>
        public string SourceType { get; protected set; }

        /// <summary>
        /// Gets the name of the user.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        public string UserName { get; protected set; }

        /// <summary>
        /// Should this source be used to write data?
        /// </summary>
        public bool Writable { get; protected set; }
    }
}