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
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using Utilities.DataTypes;
using Utilities.DataTypes.Comparison;
using Utilities.ORM.Manager.Mapper.Interfaces;
using Utilities.ORM.Manager.QueryProvider.Interfaces;
using Utilities.ORM.Manager.SourceProvider.Interfaces;
using Utilities.ORM.Parameters;

namespace Utilities.ORM.Manager.QueryProvider.Default.LDAP
{
    /// <summary>
    /// SQL Server generator
    /// </summary>
    /// <typeparam name="T">Class type</typeparam>
    public class LDAPGenerator<T> : IGenerator<T>
        where T : class,new()
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="QueryProvider">Query provider</param>
        /// <param name="Source">Source info</param>
        /// <param name="Mapping">Mapping info</param>
        public LDAPGenerator(LDAPQueryProvider QueryProvider, ISourceInfo Source, IMapping Mapping)
        {
            this.QueryProvider = QueryProvider;
            this.Source = Source;
            this.Mapping = Mapping;
        }

        public IBatch All(params IParameter[] Parameters)
        {
            throw new NotImplementedException();
        }

        public IBatch All(int Limit, params IParameter[] Parameters)
        {
            throw new NotImplementedException();
        }

        public IBatch Any(params IParameter[] Parameters)
        {
            throw new NotImplementedException();
        }

        public IBatch Delete(T Object)
        {
            throw new NotImplementedException();
        }

        public IBatch Delete(IEnumerable<T> Objects)
        {
            throw new NotImplementedException();
        }

        public IBatch Insert(T Object)
        {
            throw new NotImplementedException();
        }

        public IBatch Insert(IEnumerable<T> Objects)
        {
            throw new NotImplementedException();
        }

        public IBatch JoinsDelete<P>(IProperty<T, P> Property, T Object)
        {
            throw new NotImplementedException();
        }

        public IBatch JoinsSave<P, ItemType>(IProperty<T, P> Property, T Object)
        {
            throw new NotImplementedException();
        }

        public IBatch LoadProperty<P>(T Object, IProperty Property)
        {
            throw new NotImplementedException();
        }

        public IBatch PageCount(int PageSize, params IParameter[] Parameters)
        {
            throw new NotImplementedException();
        }

        public IBatch Paged(int PageSize, int CurrentPage, params IParameter[] Parameters)
        {
            throw new NotImplementedException();
        }

        public IBatch Save<PrimaryKeyType>(T Object)
        {
            throw new NotImplementedException();
        }

        public void SetupCommands(IMapping<T> Mapping)
        {
            throw new NotImplementedException();
        }

        public void SetupLoadCommands<D>(Mapper.Default.Map<T, D> Property) where D : class, new()
        {
            throw new NotImplementedException();
        }

        public void SetupLoadCommands<D>(Mapper.Default.IEnumerableManyToOne<T, D> Property) where D : class, new()
        {
            throw new NotImplementedException();
        }

        public void SetupLoadCommands<D>(Mapper.Default.ListManyToOne<T, D> Property) where D : class, new()
        {
            throw new NotImplementedException();
        }

        public void SetupLoadCommands<D>(Mapper.Default.ListManyToMany<T, D> Property) where D : class, new()
        {
            throw new NotImplementedException();
        }

        public void SetupLoadCommands<D>(Mapper.Default.ManyToMany<T, D> Property) where D : class, new()
        {
            throw new NotImplementedException();
        }

        public void SetupLoadCommands<D>(Mapper.Default.ManyToOne<T, D> Property) where D : class, new()
        {
            throw new NotImplementedException();
        }

        public IBatch Update(T Object)
        {
            throw new NotImplementedException();
        }

        public IBatch Update(IEnumerable<T> Objects)
        {
            throw new NotImplementedException();
        }
    }
}