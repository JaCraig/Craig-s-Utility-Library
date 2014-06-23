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

#region Usings

using Utilities.IoC.Interfaces;
using Utilities.ORM.Manager.Mapper.Interfaces;
using Utilities.ORM.Manager.QueryProvider.BaseClasses;
using Utilities.ORM.Manager.QueryProvider.Interfaces;
using Utilities.ORM.Manager.SourceProvider.Interfaces;

#endregion Usings

namespace Utilities.ORM.Manager.QueryProvider.Default.FileSystem
{
    /// <summary>
    /// File system query provider
    /// </summary>
    public class FileSystemQueryProvider : IQueryProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileSystemQueryProvider" /> class.
        /// </summary>
        public FileSystemQueryProvider()
            : base()
        {
        }

        /// <summary>
        /// Provider name associated with the query provider
        /// </summary>
        public override string ProviderName { get { return "Local"; } }

        /// <summary>
        /// Parameter prefix
        /// </summary>
        /// <value>The parameter prefix.</value>
        protected override string ParameterPrefix { get { return "@"; } }

        /// <summary>
        /// Creates a generator class for the appropriate provider
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="Source">Source information</param>
        /// <param name="Mapping">Mapping information</param>
        /// <returns>A generator class</returns>
        public override IGenerator<T> Generate<T>(ISourceInfo Source, IMapping Mapping)
        {
            return new FileSystemGenerator<T>(this, Source, Mapping);
        }
    }
}