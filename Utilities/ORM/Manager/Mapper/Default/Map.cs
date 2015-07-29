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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using Utilities.DataTypes;
using Utilities.ORM.Manager.Aspect.Interfaces;
using Utilities.ORM.Manager.Mapper.BaseClasses;
using Utilities.ORM.Manager.Mapper.Interfaces;
using Utilities.ORM.Manager.QueryProvider.Interfaces;
using Utilities.ORM.Manager.SourceProvider.Interfaces;

namespace Utilities.ORM.Manager.Mapper.Default
{
    /// <summary>
    /// Map class
    /// </summary>
    /// <typeparam name="ClassType">Class type</typeparam>
    /// <typeparam name="DataType">Data type</typeparam>
    public class Map<ClassType, DataType> : PropertyBase<ClassType, DataType, Map<ClassType, DataType>>, IMap
        where ClassType : class,new()
        where DataType : class,new()
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Expression">Expression pointing to the Map</param>
        /// <param name="Mapping">Mapping the StringID is added to</param>
        public Map(Expression<Func<ClassType, DataType>> Expression, IMapping Mapping)
            : base(Expression, Mapping)
        {
            Contract.Requires<ArgumentNullException>(Expression != null, "Expression");
            Contract.Requires<ArgumentNullException>(Mapping != null, "Mapping");
            if (typeof(DataType).Is(typeof(IEnumerable)))
                throw new ArgumentException("Expression is an IEnumerable, use ManyToOne or ManyToMany instead");
            SetTableName(Mapping.TableName);
            SetFieldName(typeof(DataType).Name + "_" + Name + "_ID");
        }

        /// <summary>
        /// Gets the name of the type.
        /// </summary>
        /// <value>
        /// The name of the type.
        /// </value>
        public override string TypeName
        {
            get { return Type.GetName(); }
        }

        /// <summary>
        /// Does a cascade delete of an object for this property
        /// </summary>
        /// <param name="Object">Object</param>
        /// <param name="Source">Source info</param>
        /// <param name="ObjectsSeen">Objects seen thus far</param>
        /// <returns>Batch object with the appropriate commands</returns>
        public override IBatch CascadeDelete(ClassType Object, ISourceInfo Source, IList<object> ObjectsSeen)
        {
            QueryProvider.Manager Provider = IoC.Manager.Bootstrapper.Resolve<QueryProvider.Manager>();
            Mapper.Manager MappingProvider = IoC.Manager.Bootstrapper.Resolve<Mapper.Manager>();
            IMapping PropertyMapping = MappingProvider[typeof(DataType), Source];
            IBatch Batch = Provider.Batch(Source);
            var AspectObject = Object as IORMObject;
            if (Object == null || (AspectObject != null && ObjectsSeen.Contains(Mapping.IDProperties.FirstOrDefault().GetValue(Object))))
                return Provider.Batch(Source);
            if (AspectObject != null)
                ObjectsSeen.Add(Mapping.IDProperties.FirstOrDefault().GetValue(Object));
            DataType Item = CompiledExpression(Object);
            if (Item == null)
                return Batch;
            foreach (IProperty<DataType> Property in PropertyMapping.Properties.Where(x => x.Cascade))
            {
                Batch.AddCommand(Property.CascadeDelete(Item, Source, ObjectsSeen.ToList()));
            }
            Batch.AddCommand(Provider.Generate<DataType>(Source, PropertyMapping).Delete(Item));
            Utilities.IoC.Manager.Bootstrapper.Resolve<DataTypes.Caching.Manager>().Cache().RemoveByTag(typeof(DataType).GetName());
            return Batch;
        }

        /// <summary>
        /// Called to create a batch that deletes items from the joining tables
        /// </summary>
        /// <param name="Object">Object</param>
        /// <param name="Source">Source info</param>
        /// <param name="ObjectsSeen">Objects seen thus far</param>
        /// <returns>Batch object with the appropriate commands</returns>
        public override IBatch CascadeJoinsDelete(ClassType Object, ISourceInfo Source, IList<object> ObjectsSeen)
        {
            QueryProvider.Manager Provider = IoC.Manager.Bootstrapper.Resolve<QueryProvider.Manager>();
            Mapper.Manager MappingProvider = IoC.Manager.Bootstrapper.Resolve<Mapper.Manager>();
            IMapping PropertyMapping = MappingProvider[typeof(DataType), Source];
            IBatch Batch = Provider.Batch(Source);
            var AspectObject = Object as IORMObject;
            if (Object == null || (AspectObject != null && ObjectsSeen.Contains(Mapping.IDProperties.FirstOrDefault().GetValue(Object))))
                return Provider.Batch(Source);
            if (AspectObject != null)
                ObjectsSeen.Add(Mapping.IDProperties.FirstOrDefault().GetValue(Object));
            DataType Item = CompiledExpression(Object);
            if (Item == null)
                return Batch;
            foreach (IProperty<DataType> Property in PropertyMapping.Properties)
            {
                if (!Property.Cascade
                    && (Property is ISingleMapping
                        || Property is IMultiMapping))
                {
                    Batch.AddCommand(Property.JoinsDelete(Item, Source, ObjectsSeen.ToList()));
                }
                else if (Property.Cascade)
                {
                    Batch.AddCommand(Property.CascadeJoinsDelete(Item, Source, ObjectsSeen.ToList()));
                }
            }
            return Batch;
        }

        /// <summary>
        /// Called to create a batch that saves items from the joining tables
        /// </summary>
        /// <param name="Object">Object</param>
        /// <param name="Source">Source info</param>
        /// <param name="ObjectsSeen">Objects seen thus far</param>
        /// <returns>Batch object with the appropriate commands</returns>
        public override IBatch CascadeJoinsSave(ClassType Object, ISourceInfo Source, IList<object> ObjectsSeen)
        {
            QueryProvider.Manager Provider = IoC.Manager.Bootstrapper.Resolve<QueryProvider.Manager>();
            Mapper.Manager MappingProvider = IoC.Manager.Bootstrapper.Resolve<Mapper.Manager>();
            IMapping PropertyMapping = MappingProvider[typeof(DataType), Source];
            IBatch Batch = Provider.Batch(Source);
            var AspectObject = Object as IORMObject;
            if (Object == null || (AspectObject != null && ObjectsSeen.Contains(Mapping.IDProperties.FirstOrDefault().GetValue(Object))))
                return Provider.Batch(Source);
            if (AspectObject != null)
                ObjectsSeen.Add(Mapping.IDProperties.FirstOrDefault().GetValue(Object));
            DataType Item = CompiledExpression(Object);
            if (Item == null)
                return Batch;
            foreach (IProperty<DataType> Property in PropertyMapping.Properties)
            {
                if (!Property.Cascade
                    && (Property is IMultiMapping
                        || Property is ISingleMapping))
                {
                    Batch.AddCommand(Property.JoinsSave(Item, Source, ObjectsSeen.ToList()));
                }
                else if (Property.Cascade)
                {
                    Batch.AddCommand(Property.CascadeJoinsSave(Item, Source, ObjectsSeen.ToList()));
                }
            }
            return Batch;
        }

        /// <summary>
        /// Does a cascade save of an object for this property
        /// </summary>
        /// <param name="Object">Object</param>
        /// <param name="Source">Source info</param>
        /// <param name="ObjectsSeen">Objects seen thus far</param>
        /// <returns>Batch object with the appropriate commands</returns>
        public override IBatch CascadeSave(ClassType Object, ISourceInfo Source, IList<object> ObjectsSeen)
        {
            QueryProvider.Manager Provider = IoC.Manager.Bootstrapper.Resolve<QueryProvider.Manager>();
            Mapper.Manager MappingProvider = IoC.Manager.Bootstrapper.Resolve<Mapper.Manager>();
            IMapping PropertyMapping = MappingProvider[typeof(DataType), Source];
            IBatch Batch = Provider.Batch(Source);
            var AspectObject = Object as IORMObject;
            if (Object == null || (AspectObject != null && ObjectsSeen.Contains(Mapping.IDProperties.FirstOrDefault().GetValue(Object))))
                return Provider.Batch(Source);
            if (AspectObject != null)
                ObjectsSeen.Add(Mapping.IDProperties.FirstOrDefault().GetValue(Object));
            DataType Item = CompiledExpression(Object);
            if (Item == null)
                return Batch;
            foreach (IProperty<DataType> Property in PropertyMapping.Properties.Where(x => x.Cascade))
            {
                Batch.AddCommand(Property.CascadeSave(Item, Source, ObjectsSeen.ToList()));
            }
            Batch.AddCommand(((IProperty<DataType>)PropertyMapping.IDProperties.FirstOrDefault()).CascadeSave(Item, Source, ObjectsSeen.ToList()));
            Utilities.IoC.Manager.Bootstrapper.Resolve<DataTypes.Caching.Manager>().Cache().RemoveByTag(typeof(DataType).GetName());
            return Batch;
        }

        /// <summary>
        /// Gets the property as a parameter (for classes, this will return the ID of the property)
        /// </summary>
        /// <param name="Object">Object to get the parameter from</param>
        /// <returns>The parameter version of the property</returns>
        public override object GetParameter(object Object)
        {
            DataType Item = CompiledExpression(Object as ClassType);
            return ForeignMapping.IDProperties.FirstOrDefault().GetValue(Item);
        }

        /// <summary>
        /// Gets the property as a parameter (for classes, this will return the ID of the property)
        /// </summary>
        /// <param name="Object">Object to get the parameter from</param>
        /// <returns>The parameter version of the property</returns>
        public override object GetParameter(Dynamo Object)
        {
            return ForeignMapping.IDProperties.FirstOrDefault().GetValue(GetValue(Object));
        }

        /// <summary>
        /// Called to create a batch that deletes items from the joining table
        /// </summary>
        /// <param name="Object">Object</param>
        /// <param name="Source">Source info</param>
        /// <param name="ObjectsSeen">Objects seen thus far</param>
        /// <returns>Batch object with the appropriate commands</returns>
        public override IBatch JoinsDelete(ClassType Object, ISourceInfo Source, IList<object> ObjectsSeen)
        {
            QueryProvider.Manager Provider = IoC.Manager.Bootstrapper.Resolve<QueryProvider.Manager>();
            return Provider.Batch(Source);
        }

        /// <summary>
        /// Called to create a batch that saves items from the joining table
        /// </summary>
        /// <param name="Object">Object</param>
        /// <param name="Source">Source info</param>
        /// <param name="ObjectsSeen">Objects seen thus far</param>
        /// <returns>Batch object with the appropriate commands</returns>
        public override IBatch JoinsSave(ClassType Object, ISourceInfo Source, IList<object> ObjectsSeen)
        {
            QueryProvider.Manager Provider = IoC.Manager.Bootstrapper.Resolve<QueryProvider.Manager>();
            return Provider.Batch(Source);
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