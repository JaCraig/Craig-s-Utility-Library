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

using System;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;

namespace Utilities.DataTypes.DataMapper.Interfaces.Contracts
{
    /// <summary>
    /// ITypeMapping contract class
    /// </summary>
    [ContractClassFor(typeof(ITypeMapping))]
    internal abstract class ITypeMappingContract : ITypeMapping
    {
        /// <summary>
        /// Automatically maps properties that are named the same thing
        /// </summary>
        /// <returns></returns>
        public ITypeMapping AutoMap()
        {
            Contract.Ensures(Contract.Result<ITypeMapping>() != null);
            return null;
        }

        /// <summary>
        /// Copies from the source to the destination
        /// </summary>
        /// <param name="Source">Source object</param>
        /// <param name="Destination">Destination object</param>
        public void Copy(object Source, object Destination)
        {
            Contract.Requires<ArgumentNullException>(Source != null);
            Contract.Requires<ArgumentNullException>(Destination != null);
        }
    }

    /// <summary>
    /// ITypeMapping contract class
    /// </summary>
    /// <typeparam name="Left">The type of the left.</typeparam>
    /// <typeparam name="Right">The type of the right.</typeparam>
    [ContractClassFor(typeof(ITypeMapping<,>))]
    internal abstract class ITypeMappingContract<Left, Right> : ITypeMapping<Left, Right>
    {
        public ITypeMapping<Left, Right> AddMapping(Expression<Func<Left, object>> LeftExpression, Expression<Func<Right, object>> RightExpression)
        {
            Contract.Ensures(Contract.Result<ITypeMapping<Left, Right>>() != null);
            return null;
        }

        public ITypeMapping<Left, Right> AddMapping(Func<Left, object> LeftGet, Action<Left, object> LeftSet, Expression<Func<Right, object>> RightExpression)
        {
            Contract.Ensures(Contract.Result<ITypeMapping<Left, Right>>() != null);
            return null;
        }

        public ITypeMapping<Left, Right> AddMapping(Expression<Func<Left, object>> LeftExpression, Func<Right, object> RightGet, Action<Right, object> RightSet)
        {
            Contract.Ensures(Contract.Result<ITypeMapping<Left, Right>>() != null);
            return null;
        }

        public ITypeMapping<Left, Right> AddMapping(Func<Left, object> LeftGet, Action<Left, object> LeftSet, Func<Right, object> RightGet, Action<Right, object> RightSet)
        {
            Contract.Ensures(Contract.Result<ITypeMapping<Left, Right>>() != null);
            return null;
        }

        public void Copy(Left Source, Right Destination)
        {
        }

        public void Copy(Right Source, Left Destination)
        {
        }

        public void CopyLeftToRight(Left Source, Right Destination)
        {
        }

        public void CopyRightToLeft(Right Source, Left Destination)
        {
        }

        public ITypeMapping AutoMap()
        {
            return null;
        }

        public void Copy(object Source, object Destination)
        {
        }
    }
}