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

using Utilities.ORM.Manager.Mapper.Interfaces;
using Utilities.ORM.Manager.SourceProvider.Interfaces;

namespace Utilities.ORM.Manager.QueryProvider.Interfaces
{
    /// <summary>
    /// Query provider
    /// </summary>
    public interface IQueryProvider
    {
        /// <summary>
        /// Provider name associated with the query provider
        /// </summary>
        string ProviderName { get; }

        /// <summary>
        /// Creates a batch for running commands
        /// </summary>
        /// <param name="Source">Source info</param>
        /// <returns>A batch object</returns>
        IBatch Batch(ISourceInfo Source);

        /// <summary>
        /// Creates a generator object
        /// </summary>
        /// <typeparam name="T">Class type to create the generator for</typeparam>
        /// <param name="Source">Source info</param>
        /// <param name="Mapping">Mapping info</param>
        /// <returns>Generator object</returns>
        IGenerator<T> Generate<T>(ISourceInfo Source, IMapping Mapping)
            where T : class,new();
    }
}