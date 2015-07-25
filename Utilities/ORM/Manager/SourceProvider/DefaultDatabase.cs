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
using Utilities.ORM.Interfaces;

namespace Utilities.ORM.Manager.SourceProvider
{
    /// <summary>
    /// Default database object
    /// </summary>
    public class DefaultDatabase : IDatabase
    {
        /// <summary>
        /// Should the database be auditted
        /// </summary>
        public bool Audit
        {
            get { return false; }
        }

        /// <summary>
        /// The name of the connection string
        /// </summary>
        public string Name
        {
            get { return ConfigurationManager.ConnectionStrings[0].Name; }
        }

        /// <summary>
        /// Order of the database (used when running commands to save/select objects)
        /// </summary>
        public int Order
        {
            get { return 0; }
        }

        /// <summary>
        /// Is this readable?
        /// </summary>
        public bool Readable
        {
            get { return true; }
        }

        /// <summary>
        /// Should we update the database
        /// </summary>
        public bool Update
        {
            get { return false; }
        }

        /// <summary>
        /// Is this writable?
        /// </summary>
        public bool Writable
        {
            get { return true; }
        }
    }
}