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
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using Utilities.ORM.Manager.Mapper.BaseClasses;
using Utilities.ORM.Manager.Mapper.Interfaces;
using Utilities.ORM.Manager.SourceProvider.Interfaces;

#endregion Usings

namespace Utilities.ORM.Manager.Mapper.Default
{
    /// <summary>
    /// Many to one class
    /// </summary>
    /// <typeparam name="ClassType">Class type</typeparam>
    /// <typeparam name="DataType">Data type</typeparam>
    public class IEnumerableManyToOne<ClassType, DataType> : PropertyBase<ClassType, IEnumerable<DataType>, IEnumerableManyToOne<ClassType, DataType>>, IIEnumerableManyToOne
        where ClassType : class,new()
        where DataType : class,new()
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Expression">Expression pointing to the many to one</param>
        /// <param name="Mapping">Mapping the StringID is added to</param>
        public IEnumerableManyToOne(Expression<Func<ClassType, IEnumerable<DataType>>> Expression, IMapping Mapping)
            : base(Expression, Mapping)
        {
            Contract.Requires<ArgumentNullException>(Expression != null, "Expression");
            Type = typeof(DataType);
            SetDefaultValue(() => new List<DataType>());
            string Class1 = typeof(ClassType).Name;
            string Class2 = typeof(DataType).Name;
            if (string.Compare(Class1, Class2, StringComparison.Ordinal) < 0)
                SetTableName(Class1 + "_" + Class2);
            else
                SetTableName(Class2 + "_" + Class1);
        }

        /// <summary>
        /// Sets up the property
        /// </summary>
        /// <param name="MappingProvider">Mapping provider</param>
        /// <param name="QueryProvider">Query provider</param>
        /// <param name="Source">Source info</param>
        public override void Setup(ISourceInfo Source, Mapper.Manager MappingProvider, QueryProvider.Manager QueryProvider)
        {
            ForeignMapping = MappingProvider[Type, Source];
            QueryProvider.Generate<ClassType>(Source, Mapping).SetupLoadCommands(this);
        }
    }
}