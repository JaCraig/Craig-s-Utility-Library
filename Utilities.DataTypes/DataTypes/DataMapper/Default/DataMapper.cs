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

using System;
using System.Linq;
using Utilities.DataTypes.DataMapper.BaseClasses;
using Utilities.DataTypes.DataMapper.Interfaces;

#endregion Usings

namespace Utilities.DataTypes.DataMapper.Default
{
    /// <summary>
    /// Default data mapper
    /// </summary>
    public class DataMapper : DataMapperBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DataMapper()
            : base()
        {
        }

        /// <summary>
        /// Used internally to create type mappings
        /// </summary>
        /// <typeparam name="Left">Left type</typeparam>
        /// <typeparam name="Right">Right type</typeparam>
        /// <returns>A mapping object for the two types specified</returns>
        protected override ITypeMapping<Left, Right> CreateTypeMapping<Left, Right>()
        {
            return new TypeMapping<Left, Right>();
        }

        /// <summary>
        /// Used internally to create type mappings
        /// </summary>
        /// <param name="Left">Left type</param>
        /// <param name="Right">Right type</param>
        /// <returns>A mapping object for the two types specified</returns>
        protected override ITypeMapping CreateTypeMapping(Type Left, Type Right)
        {
            return (ITypeMapping)AppDomain.CurrentDomain.GetAssemblies().Types<ITypeMapping>().First().MakeGenericType(Left, Right).Create();
        }
    }
}