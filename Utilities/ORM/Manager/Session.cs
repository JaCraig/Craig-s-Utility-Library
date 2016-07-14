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
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using Utilities.DataTypes;
using Utilities.DataTypes.Caching.Interfaces;
using Utilities.ORM.Manager.Aspect.Interfaces;
using Utilities.ORM.Manager.Mapper.Interfaces;
using Utilities.ORM.Manager.QueryProvider.Interfaces;
using Utilities.ORM.Manager.SourceProvider.Interfaces;
using Utilities.ORM.Parameters;

namespace Utilities.ORM.Manager
{
    /// <summary>
    /// Session object
    /// </summary>
    public class Session
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Session()
        {
            QueryProvider = IoC.Manager.Bootstrapper.Resolve<QueryProvider.Manager>();
            SourceProvider = IoC.Manager.Bootstrapper.Resolve<SourceProvider.Manager>();
            MapperProvider = IoC.Manager.Bootstrapper.Resolve<Mapper.Manager>();
            Cache = IoC.Manager.Bootstrapper.Resolve<DataTypes.Caching.Manager>().Cache();
        }

        /// <summary>
        /// Cache that is used
        /// </summary>
        private ICache Cache { get; set; }

        /// <summary>
        /// Mapper provider
        /// </summary>
        private Mapper.Manager MapperProvider { get; set; }

        /// <summary>
        /// Query provider
        /// </summary>
        private QueryProvider.Manager QueryProvider { get; set; }

        /// <summary>
        /// Source provider
        /// </summary>
        private SourceProvider.Manager SourceProvider { get; set; }

        /// <summary>
        /// Returns all items that match the criteria
        /// </summary>
        /// <typeparam name="ObjectType">Type of the object</typeparam>
        /// <param name="Parameters">Parameters used in the where clause</param>
        /// <returns>All items that match the criteria</returns>
        public IEnumerable<ObjectType> All<ObjectType>(params IParameter[] Parameters)
            where ObjectType : class
        {
            Parameters = Parameters.Check(new IParameter[] { });
            var ReturnValue = new List<Dynamo>();
            string KeyName = typeof(ObjectType).GetName() + "_All_" + Parameters.ToString(x => x.ToString(), "_");
            Parameters.ForEach(x => { KeyName = x.AddParameter(KeyName); });
            if (Cache.ContainsKey(KeyName))
            {
                return GetListCached<ObjectType>(ref ReturnValue, KeyName);
            }
            foreach (ISourceInfo Source in SourceProvider.Where(x => x.Readable).OrderBy(x => x.Order))
            {
                IMapping Mapping = MapperProvider[typeof(ObjectType), Source];
                if (Mapping != null)
                {
                    foreach (Dynamo Item in QueryProvider.Generate<ObjectType>(Source, Mapping, MapperProvider.GetStructure(Mapping.DatabaseConfigType)).All(Parameters).Execute()[0])
                    {
                        var IDProperty = Mapping.IDProperties.FirstOrDefault();
                        CopyOrAdd(ReturnValue, IDProperty, Item);
                    }
                }
            }
            Cache.Add(KeyName, ReturnValue, new string[] { typeof(ObjectType).GetName() });
            return ConvertValues<ObjectType>(ReturnValue);
        }

        /// <summary>
        /// Returns a single item matching the criteria
        /// </summary>
        /// <typeparam name="ObjectType">Type of the object</typeparam>
        /// <param name="Parameters">Parameters used in the where clause</param>
        /// <returns>A single object matching the criteria</returns>
        public ObjectType Any<ObjectType>(params IParameter[] Parameters)
            where ObjectType : class
        {
            Parameters = Parameters.Check(new IParameter[] { });
            Dynamo ReturnValue = null;
            string KeyName = typeof(ObjectType).GetName() + "_Any_" + Parameters.ToString(x => x.ToString(), "_");
            Parameters.ForEach(x => { KeyName = x.AddParameter(KeyName); });
            if (Cache.ContainsKey(KeyName))
            {
                return GetCached<ObjectType>(ref ReturnValue, KeyName);
            }
            foreach (ISourceInfo Source in SourceProvider.Where(x => x.Readable).OrderBy(x => x.Order))
            {
                IMapping Mapping = MapperProvider[typeof(ObjectType), Source];
                if (Mapping != null)
                {
                    Dynamo Value = QueryProvider.Generate<ObjectType>(Source, Mapping, MapperProvider.GetStructure(Mapping.DatabaseConfigType)).Any(Parameters).Execute()[0].FirstOrDefault();
                    ReturnValue = CopyOrAssign(ReturnValue, Value);
                }
            }
            Cache.Add(KeyName, ReturnValue, new string[] { typeof(ObjectType).GetName() });
            return ConvertValue<ObjectType>(ReturnValue);
        }

        /// <summary>
        /// Returns a single item matching the criteria specified
        /// </summary>
        /// <typeparam name="ObjectType">Type of the object</typeparam>
        /// <typeparam name="IDType">ID type for the object</typeparam>
        /// <param name="ID">ID of the object to load</param>
        /// <returns>A single object matching the ID</returns>
        public ObjectType Any<ObjectType, IDType>(IDType ID)
            where ObjectType : class
            where IDType : IComparable
        {
            Dynamo ReturnValue = null;
            string KeyName = typeof(ObjectType).GetName() + "_Any_" + ID.ToString();
            if (Cache.ContainsKey(KeyName))
            {
                return GetCached<ObjectType>(ref ReturnValue, KeyName);
            }
            var StringID = ID.ToString();
            foreach (ISourceInfo Source in SourceProvider.Where(x => x.Readable).OrderBy(x => x.Order))
            {
                IMapping Mapping = MapperProvider[typeof(ObjectType), Source];
                if (Mapping != null)
                {
                    var IDProperty = Mapping.IDProperties.FirstOrDefault();
                    if (IDProperty != null)
                    {
                        Dynamo Value = typeof(IDType) == typeof(string) ?
                            QueryProvider.Generate<ObjectType>(Source, Mapping, MapperProvider.GetStructure(Mapping.DatabaseConfigType)).Any(new StringEqualParameter(StringID, IDProperty.FieldName, StringID.Length, IDProperty.FieldName, Source.ParameterPrefix)).Execute()[0].FirstOrDefault() :
                            QueryProvider.Generate<ObjectType>(Source, Mapping, MapperProvider.GetStructure(Mapping.DatabaseConfigType)).Any(new EqualParameter<IDType>(ID, IDProperty.FieldName, IDProperty.FieldName, Source.ParameterPrefix)).Execute()[0].FirstOrDefault();
                        ReturnValue = CopyOrAssign(ReturnValue, Value);
                    }
                }
            }
            Cache.Add(KeyName, ReturnValue, new string[] { typeof(ObjectType).GetName() });
            return ConvertValue<ObjectType>(ReturnValue);
        }

        /// <summary>
        /// Deletes an object from the database
        /// </summary>
        /// <typeparam name="ObjectType">Object type</typeparam>
        /// <param name="Object">Object to delete</param>
        public void Delete<ObjectType>(ObjectType Object)
            where ObjectType : class
        {
            Cache.RemoveByTag(typeof(ObjectType).GetName());
            foreach (ISourceInfo Source in SourceProvider.Where(x => x.Writable).OrderBy(x => x.Order))
            {
                IMapping Mapping = MapperProvider[typeof(ObjectType), Source];
                if (Mapping != null)
                {
                    var Generator = QueryProvider.Generate<ObjectType>(Source, MapperProvider[typeof(ObjectType), Source], MapperProvider.GetStructure(Mapping.DatabaseConfigType));
                    var TempBatch = QueryProvider.Batch(Source);
                    CascadeDelete<ObjectType>(Object, Source, Mapping, TempBatch, new List<object>());
                    TempBatch.AddCommand(Generator.Delete(Object));
                    TempBatch.Execute();
                }
            }
        }

        /// <summary>
        /// Loads a property (primarily used internally for lazy loading)
        /// </summary>
        /// <typeparam name="ObjectType">Object type</typeparam>
        /// <typeparam name="DataType">Data type</typeparam>
        /// <param name="Object">Object</param>
        /// <param name="PropertyName">Property name</param>
        /// <returns>The appropriate property value</returns>
        public IList<DataType> LoadProperties<ObjectType, DataType>(ObjectType Object, string PropertyName)
            where ObjectType : class
            where DataType : class
        {
            var ReturnValue = new System.Collections.Generic.List<Dynamo>();
            foreach (ISourceInfo Source in SourceProvider.Where(x => x.Readable).OrderBy(x => x.Order))
            {
                IMapping Mapping = MapperProvider[typeof(ObjectType), Source];
                if (Mapping != null)
                {
                    var Property = Mapping.Properties.FirstOrDefault(x => x.Name == PropertyName);
                    if (Property != null)
                    {
                        foreach (Dynamo Item in QueryProvider.Generate<ObjectType>(Source, Mapping, MapperProvider.GetStructure(Mapping.DatabaseConfigType))
                            .LoadProperty<DataType>(Object, Property)
                            .Execute()[0])
                        {
                            var IDProperty = Property.ForeignMapping.IDProperties.FirstOrDefault();
                            CopyOrAdd(ReturnValue, IDProperty, Item);
                        }
                    }
                }
            }
            if (ReturnValue.Count == 0)
                return new ObservableList<DataType>();
            foreach (ISourceInfo Source in SourceProvider.Where(x => x.Readable).OrderBy(x => x.Order))
            {
                IMapping ObjectMapping = MapperProvider[typeof(ObjectType), Source];
                IMapping Mapping = MapperProvider[typeof(DataType), Source];
                if (Mapping != null)
                {
                    IProperty ObjectProperty = ObjectMapping == null ? null : ObjectMapping.Properties.FirstOrDefault(x => x.Name == PropertyName);
                    if (ObjectProperty == null)
                    {
                        var IDProperty = Mapping.IDProperties.FirstOrDefault();
                        IParameter Parameter = null;
                        int Counter = 0;
                        foreach (Dynamo Item in ReturnValue)
                        {
                            if (IDProperty.GetParameter(Item) != null)
                            {
                                Parameter = Parameter == null ? (IParameter)new EqualParameter<object>(IDProperty.GetParameter(Item), Counter.ToString(CultureInfo.InvariantCulture), IDProperty.FieldName, Source.ParameterPrefix) : (IParameter)new OrParameter(Parameter, new EqualParameter<object>(IDProperty.GetParameter(Item), Counter.ToString(CultureInfo.InvariantCulture), IDProperty.FieldName, Source.ParameterPrefix));
                                ++Counter;
                            }
                        }
                        if (Parameter != null)
                        {
                            foreach (Dynamo Item in QueryProvider.Generate<DataType>(Source, Mapping, MapperProvider.GetStructure(Mapping.DatabaseConfigType)).All(Parameter).Execute()[0])
                            {
                                CopyOrAdd(ReturnValue, IDProperty, Item);
                            }
                        }
                    }
                }
            }
            return ConvertValues<DataType>(ReturnValue).ToObservableList(x => x);
        }

        /// <summary>
        /// Loads a property (primarily used internally for lazy loading)
        /// </summary>
        /// <typeparam name="ObjectType">Object type</typeparam>
        /// <typeparam name="DataType">Data type</typeparam>
        /// <param name="Object">Object</param>
        /// <param name="PropertyName">Property name</param>
        /// <returns>The appropriate property value</returns>
        public DataType LoadProperty<ObjectType, DataType>(ObjectType Object, string PropertyName)
            where ObjectType : class
            where DataType : class
        {
            return LoadProperties<ObjectType, DataType>(Object, PropertyName).FirstOrDefault();
        }

        /// <summary>
        /// Gets the number of pages based on the specified
        /// </summary>
        /// <param name="PageSize">Page size</param>
        /// <param name="Parameters">Parameters to search by</param>
        /// <typeparam name="ObjectType">Object type to get the page count of</typeparam>
        /// <returns>The number of pages that the table contains for the specified page size</returns>
        public int PageCount<ObjectType>(int PageSize = 25, params IParameter[] Parameters)
            where ObjectType : class
        {
            Parameters = Parameters.Check(new IParameter[] { });
            string KeyName = typeof(ObjectType).GetName() + "_PageCount_" + PageSize.ToString(CultureInfo.InvariantCulture) + "_" + Parameters.ToString(x => x.ToString(), "_");
            Parameters.ForEach(x => { KeyName = x.AddParameter(KeyName); });
            if (Cache.ContainsKey(KeyName))
            {
                return (int)Cache[KeyName];
            }
            foreach (ISourceInfo Source in SourceProvider.Where(x => x.Readable).OrderBy(x => x.Order))
            {
                IMapping Mapping = MapperProvider[typeof(ObjectType), Source];
                if (Mapping != null)
                {
                    int Count = QueryProvider.Generate<ObjectType>(Source, Mapping, MapperProvider.GetStructure(Mapping.DatabaseConfigType))
                        .PageCount(PageSize, Parameters)
                        .Execute()[0]
                        .FirstOrDefault()
                        .Total;
                    if (Count > 0)
                    {
                        int ReturnValue = (Count / PageSize) + (Count % PageSize > 0 ? 1 : 0);
                        Cache.Add(KeyName, ReturnValue, new string[] { typeof(ObjectType).GetName() });
                        return ReturnValue;
                    }
                }
            }
            return 0;
        }

        /// <summary>
        /// Returns a paged list of items
        /// </summary>
        /// <typeparam name="ObjectType">Object type</typeparam>
        /// <param name="PageSize">Page size</param>
        /// <param name="CurrentPage">Current page (starting with 0)</param>
        /// <param name="OrderBy">The order by portion of the query</param>
        /// <param name="Parameters">Parameters used in the where clause</param>
        /// <returns>A paged list of items that match the criteria</returns>
        public IEnumerable<ObjectType> Paged<ObjectType>(int PageSize = 25, int CurrentPage = 0, string OrderBy = "", params IParameter[] Parameters)
            where ObjectType : class
        {
            Parameters = Parameters.Check(new IParameter[] { });
            string KeyName = typeof(ObjectType).GetName() + "_Paged_" + PageSize.ToString(CultureInfo.InvariantCulture) + "_" + CurrentPage.ToString(CultureInfo.InvariantCulture) + "_" + Parameters.ToString(x => x.ToString(), "_");
            Parameters.ForEach(x => { KeyName = x.AddParameter(KeyName); });
            var ReturnValue = new System.Collections.Generic.List<Dynamo>();
            if (Cache.ContainsKey(KeyName))
            {
                return GetListCached<ObjectType>(ref ReturnValue, KeyName);
            }
            foreach (ISourceInfo Source in SourceProvider.Where(x => x.Readable).OrderBy(x => x.Order))
            {
                IMapping Mapping = MapperProvider[typeof(ObjectType), Source];
                if (Mapping != null)
                {
                    var IDProperty = Mapping.IDProperties.FirstOrDefault();
                    if (IDProperty != null)
                    {
                        foreach (Dynamo Item in QueryProvider.Generate<ObjectType>(Source, Mapping, MapperProvider.GetStructure(Mapping.DatabaseConfigType))
                            .Paged(PageSize, CurrentPage, OrderBy, Parameters)
                            .Execute()[0])
                        {
                            CopyOrAdd(ReturnValue, IDProperty, Item);
                        }
                    }
                }
            }
            Cache.Add(KeyName, ReturnValue, new string[] { typeof(ObjectType).GetName() });
            return ConvertValues<ObjectType>(ReturnValue);
        }

        /// <summary>
        /// Saves an object to the database
        /// </summary>
        /// <typeparam name="ObjectType">Object type</typeparam>
        /// <typeparam name="PrimaryKeyType">Primary key type</typeparam>
        /// <param name="Object">Object to save</param>
        public void Save<ObjectType, PrimaryKeyType>(ObjectType Object)
            where ObjectType : class
        {
            Cache.RemoveByTag(typeof(ObjectType).GetName());
            foreach (ISourceInfo Source in SourceProvider.Where(x => x.Writable).OrderBy(x => x.Order))
            {
                IMapping Mapping = MapperProvider[typeof(ObjectType), Source];
                if (Mapping != null)
                {
                    var Generator = QueryProvider.Generate<ObjectType>(Source, MapperProvider[typeof(ObjectType), Source], MapperProvider.GetStructure(Mapping.DatabaseConfigType));
                    var TempBatch = QueryProvider.Batch(Source);
                    CascadeSave<ObjectType>(Object, Source, Mapping, TempBatch, new List<object>());
                    TempBatch.Execute();
                    TempBatch = QueryProvider.Batch(Source);
                    TempBatch.AddCommand(Generator.Save<PrimaryKeyType>(Object));
                    TempBatch.Execute();
                    TempBatch = QueryProvider.Batch(Source);
                    JoinsDelete<ObjectType>(Object, Source, Mapping, TempBatch, new List<object>());
                    JoinsSave<ObjectType>(Object, Source, Mapping, TempBatch, new List<object>());
                    TempBatch.RemoveDuplicateCommands().Execute();
                }
            }
        }

        private static void CascadeDelete<ObjectType>(ObjectType Object, ISourceInfo Source, IMapping Mapping, IBatch TempBatch, List<object> ObjectsSeen)
            where ObjectType : class
        {
            Contract.Requires<ArgumentNullException>(Mapping != null, "Mapping");
            Contract.Requires<ArgumentNullException>(Mapping.Properties != null, "Mapping.Properties");
            foreach (IProperty<ObjectType> Property in Mapping.Properties.Where(x => x.Cascade))
            {
                TempBatch.AddCommand(Property.CascadeDelete(Object, Source, ObjectsSeen.ToList()));
            }
        }

        private static void CascadeSave<ObjectType>(ObjectType Object, ISourceInfo Source, IMapping Mapping, IBatch TempBatch, List<object> ObjectsSeen)
            where ObjectType : class
        {
            Contract.Requires<ArgumentNullException>(Mapping != null, "Mapping");
            Contract.Requires<ArgumentNullException>(Mapping.Properties != null, "Mapping.Properties");
            var ORMObject = Object as IORMObject;
            foreach (IProperty<ObjectType> Property in Mapping.Properties.Where(x => x.Cascade))
            {
                if (ORMObject == null || ORMObject.PropertiesChanged0.Contains(Property.Name))
                    TempBatch.AddCommand(Property.CascadeSave(Object, Source, ObjectsSeen.ToList()));
            }
        }

        private static void CopyOrAdd(List<Dynamo> ReturnValue, IProperty IDProperty, Dynamo Item)
        {
            Contract.Requires<ArgumentNullException>(IDProperty != null);
            if (Item == null)
                return;
            if (ReturnValue == null)
                ReturnValue = new List<Dynamo>();
            var IDValue = IDProperty.GetValue(Item);
            var Value = ReturnValue.FirstOrDefault(x => IDProperty.GetValue(x).Equals(IDValue));
            if (Value == null)
                ReturnValue.Add(Item);
            else
                Item.CopyTo(Value);
        }

        private static Dynamo CopyOrAssign(Dynamo ReturnValue, Dynamo Value)
        {
            if (Value == null)
                return ReturnValue;
            if (ReturnValue == null)
                return Value;
            Value.CopyTo(ReturnValue);
            return ReturnValue;
        }

        private static void JoinsDelete<ObjectType>(ObjectType Object, ISourceInfo Source, IMapping Mapping, IBatch TempBatch, List<object> ObjectsSeen)
            where ObjectType : class
        {
            Contract.Requires<ArgumentNullException>(Mapping != null, "Mapping");
            Contract.Requires<ArgumentNullException>(Mapping.Properties != null, "Mapping.Properties");
            var ORMObject = Object as IORMObject;
            foreach (IProperty<ObjectType> Property in Mapping.Properties)
            {
                if (ORMObject == null || ORMObject.PropertiesChanged0.Contains(Property.Name))
                {
                    if (!Property.Cascade &&
                        (Property is IMultiMapping
                            || Property is ISingleMapping))
                    {
                        TempBatch.AddCommand(Property.JoinsDelete(Object, Source, ObjectsSeen.ToList()));
                    }
                    else if (Property.Cascade)
                    {
                        TempBatch.AddCommand(Property.CascadeJoinsDelete(Object, Source, ObjectsSeen.ToList()));
                    }
                }
            }
        }

        private static void JoinsSave<ObjectType>(ObjectType Object, ISourceInfo Source, IMapping Mapping, IBatch TempBatch, List<object> ObjectsSeen)
            where ObjectType : class
        {
            Contract.Requires<ArgumentNullException>(Mapping != null, "Mapping");
            Contract.Requires<ArgumentNullException>(Mapping.Properties != null, "Mapping.Properties");
            var ORMObject = Object as IORMObject;
            foreach (IProperty<ObjectType> Property in Mapping.Properties)
            {
                if (ORMObject == null || ORMObject.PropertiesChanged0.Contains(Property.Name))
                {
                    if (!Property.Cascade &&
                        (Property is IMultiMapping
                            || Property is ISingleMapping))
                    {
                        TempBatch.AddCommand(Property.JoinsSave(Object, Source, ObjectsSeen.ToList()));
                    }
                    else if (Property.Cascade)
                    {
                        TempBatch.AddCommand(Property.CascadeJoinsSave(Object, Source, ObjectsSeen.ToList()));
                    }
                }
            }
        }

        private ObjectType ConvertValue<ObjectType>(Dynamo ReturnValue) where ObjectType : class
        {
            if (ReturnValue == null)
                return default(ObjectType);
            return ReturnValue.To<ObjectType>().Chain(x => { ((IORMObject)x).Session0 = this; });
        }

        private IEnumerable<ObjectType> ConvertValues<ObjectType>(List<Dynamo> ReturnValue) where ObjectType : class
        {
            if (ReturnValue == null)
                ReturnValue = new List<Dynamo>();
            return ReturnValue.ForEachParallel(x => ConvertValue<ObjectType>(x));
        }

        private ObjectType GetCached<ObjectType>(ref Dynamo ReturnValue, string KeyName) where ObjectType : class
        {
            Contract.Requires(this.Cache != null);
            ReturnValue = (Dynamo)Cache[KeyName];
            return ConvertValue<ObjectType>(ReturnValue);
        }

        private IEnumerable<ObjectType> GetListCached<ObjectType>(ref List<Dynamo> ReturnValue, string KeyName) where ObjectType : class
        {
            Contract.Requires(this.Cache != null);
            ReturnValue = (List<Dynamo>)Cache[KeyName];
            return ConvertValues<ObjectType>(ReturnValue);
        }
    }
}