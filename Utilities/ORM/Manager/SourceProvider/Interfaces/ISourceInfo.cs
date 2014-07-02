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

using Utilities.ORM.Interfaces;

namespace Utilities.ORM.Manager.SourceProvider.Interfaces
{
    /// <summary>
    /// Source information
    /// </summary>
    public interface ISourceInfo
    {
        /// <summary>
        /// Connection string
        /// </summary>
        string Connection { get; }

        /// <summary>
        /// The database object associated with the source info (if one is associated with it)
        /// </summary>
        IDatabase Database { get; }

        /// <summary>
        /// Name of the source
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Order that the source should be used in
        /// </summary>
        int Order { get; }

        /// <summary>
        /// Parameter prefix that the source uses
        /// </summary>
        string ParameterPrefix { get; }

        /// <summary>
        /// Should this source be used to read data?
        /// </summary>
        bool Readable { get; }

        /// <summary>
        /// Source type, based on ADO.Net provider name
        /// </summary>
        string SourceType { get; }

        /// <summary>
        /// Should this source be used to write data?
        /// </summary>
        bool Writable { get; }
    }
}