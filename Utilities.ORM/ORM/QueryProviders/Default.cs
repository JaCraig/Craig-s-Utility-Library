/*
Copyright (c) 2011 <a href="http://www.gutgames.com">James Craig</a>

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
using System.Linq;
using System.Text;
using System.Reflection;
using Utilities.ORM.QueryProviders.Interfaces;
using Utilities.ORM.Mapping.Interfaces;
using Utilities.DataTypes;
using Utilities.SQL.MicroORM;
using System.Data;
using Utilities.ORM.Aspect.Interfaces;
using Utilities.Reflection.ExtensionMethods;
#endregion

namespace Utilities.ORM.QueryProviders
{
    /// <summary>
    /// Default query provider
    /// </summary>
    public class Default
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="AssemblyUsing">Assembly using</param>
        public Default(Assembly AssemblyUsing)
        {
            Setup(AssemblyUsing);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="AssembliesUsing">Assemblies using</param>
        public Default(Assembly[] AssembliesUsing)
        {
            foreach (Assembly Assembly in AssembliesUsing)
            {
                Setup(Assembly);
            }
        }

        #endregion

        #region Functions

        #region Setup

        /// <summary>
        /// Sets up the system
        /// </summary>
        /// <param name="AssemblyUsing">Assembly to set up</param>
        private void Setup(Assembly AssemblyUsing)
        {
            if (Databases == null)
                Databases = new System.Collections.Generic.List<IDatabase>();
            IEnumerable<Type> Types = AssemblyUsing.GetTypes(typeof(IDatabase));
            foreach (Type Type in Types)
            {
                Type BaseType = Type.BaseType;
                IDatabase TempObject = (IDatabase)Activator.CreateInstance(Type);
                if (!string.IsNullOrEmpty(TempObject.ConnectionString))
                {
                    MicroORM.Database(TempObject.ConnectionString, TempObject.Name);
                    Databases.Add(TempObject);
                }
            }
            Manager = new Reflection.AOP.AOPManager();
        }

        #endregion

        #region AddMapping

        /// <summary>
        /// Adds a mapping
        /// </summary>
        /// <param name="Mapping">Mapping to add</param>
        public virtual void AddMapping(IMapping Mapping)
        {
            if (Mappings == null)
                Mappings = new ListMapping<IDatabase, IMapping>();
            IEnumerable<IDatabase> Databases = this.Databases.Where(x => x.IsOfType(Mapping.DatabaseConfigType)
                                                        && !string.IsNullOrEmpty(x.ConnectionString));
            foreach (IDatabase Database in Databases)
            {
                Mapping.AddToQueryProvider(Database);
                Mappings.Add(Database, Mapping);
            }
        }

        #endregion

        #region Any

        /// <summary>
        /// Returns any item that matches the criteria
        /// </summary>
        /// <typeparam name="ObjectType">Object type</typeparam>
        /// <param name="Parameters">Parameters used in the where clause</param>
        /// <param name="CurrentSession">Current session</param>
        /// <param name="ReturnValue">Return value</param>
        /// <returns>First item matching the criteria</returns>
        public virtual ObjectType Any<ObjectType>(Session CurrentSession, ObjectType ReturnValue = null, params IParameter[] Parameters) where ObjectType : class,new()
        {
            foreach (IDatabase Database in Mappings.Keys.OrderBy(x => x.Order))
            {
                IMapping Mapping = Mappings[Database].FirstOrDefault(x => x.ObjectType == typeof(ObjectType));
                if (Mapping != null)
                {
                    using (MicroORM ORMObject = new MicroORM(Database.Name))
                    {
                        if (Mapping.AnyCommand == null)
                            ReturnValue = ORMObject.Map<ObjectType>().Any("*", ReturnValue, () => Manager.Create<ObjectType>(), Parameters);
                        else
                            ReturnValue = ORMObject.Map<ObjectType>().Any(Mapping.AnyCommand.CommandToRun, Mapping.AnyCommand.CommandType, ReturnValue, () => Manager.Create<ObjectType>(), Parameters);
                    }
                }
            }
            if (ReturnValue is IORMObject)
                ((IORMObject)ReturnValue).Session0 = CurrentSession;
            return ReturnValue;
        }

        /// <summary>
        /// Returns any item that matches the criteria
        /// </summary>
        /// <typeparam name="ObjectType">Object type</typeparam>
        /// <param name="Columns">Columns to load</param>
        /// <param name="Parameters">Parameters used in the where clause</param>
        /// <param name="CurrentSession">Current session</param>
        /// <param name="ReturnValue">Return value</param>
        /// <returns>First item that matches the criteria</returns>
        public virtual ObjectType Any<ObjectType>(Session CurrentSession, string Columns, ObjectType ReturnValue = null, params IParameter[] Parameters) where ObjectType : class,new()
        {
            foreach (IDatabase Database in Mappings.Keys.OrderBy(x => x.Order))
            {
                if (Mappings[Database].FirstOrDefault(x => x.ObjectType == typeof(ObjectType)) != null)
                {
                    using (MicroORM ORMObject = new MicroORM(Database.Name))
                    {
                        ReturnValue = ORMObject.Map<ObjectType>().Any(Columns, ReturnValue, () => Manager.Create<ObjectType>(), Parameters);
                    }
                }
            }
            if (ReturnValue is IORMObject)
                ((IORMObject)ReturnValue).Session0 = CurrentSession;
            return ReturnValue;
        }

        /// <summary>
        /// Returns any item that matches the criteria
        /// </summary>
        /// <typeparam name="ObjectType">Object type</typeparam>
        /// <param name="Command">Command to run</param>
        /// <param name="CommandType">Command type</param>
        /// <param name="Parameters">Parameters used in the where clause</param>
        /// <param name="CurrentSession">Current session</param>
        /// <param name="ReturnValue">Return value</param>
        /// <returns>First item that matches the criteria</returns>
        public virtual ObjectType Any<ObjectType>(Session CurrentSession, string Command, CommandType CommandType, ObjectType ReturnValue = null, params IParameter[] Parameters) where ObjectType : class,new()
        {
            foreach (IDatabase Database in Mappings.Keys.OrderBy(x => x.Order))
            {
                if (Mappings[Database].FirstOrDefault(x => x.ObjectType == typeof(ObjectType)) != null)
                {
                    using (MicroORM ORMObject = new MicroORM(Database.Name))
                    {
                        ReturnValue = ORMObject.Map<ObjectType>().Any(Command, CommandType, ReturnValue, () => Manager.Create<ObjectType>(), Parameters);
                    }
                }
            }
            if (ReturnValue is IORMObject)
                ((IORMObject)ReturnValue).Session0 = CurrentSession;
            return ReturnValue;
        }

        #endregion

        #region All

        /// <summary>
        /// Returns a list of objects that meet the criteria
        /// </summary>
        /// <typeparam name="ObjectType">Object type</typeparam>
        /// <param name="CurrentSession">Current session</param>
        /// <param name="Columns">Columns to load</param>
        /// <param name="Limit">Limit on the number of items to return</param>
        /// <param name="OrderBy">Order by clause (minus the ORDER BY)</param>
        /// <param name="Parameters">Parameters used in the where clause</param>
        /// <returns>A list of objects that meet the criteria</returns>
        public virtual IEnumerable<ObjectType> All<ObjectType>(Session CurrentSession, string Columns, int Limit, string OrderBy, params IParameter[] Parameters) where ObjectType : class,new()
        {
            System.Collections.Generic.List<ObjectType> ReturnValues = new System.Collections.Generic.List<ObjectType>();
            foreach (IDatabase Database in Mappings.Keys.OrderBy(x => x.Order))
            {
                if (Mappings[Database].FirstOrDefault(x => x.ObjectType == typeof(ObjectType)) != null)
                {
                    using (MicroORM ORMObject = new MicroORM(Database.Name))
                    {
                        ReturnValues = (System.Collections.Generic.List<ObjectType>)ORMObject.Map<ObjectType>().All(Columns, Limit, OrderBy, ReturnValues, () => Manager.Create<ObjectType>(), Parameters);
                    }
                }
            }
            foreach (ObjectType ReturnValue in ReturnValues)
            {
                if (ReturnValue is IORMObject)
                    ((IORMObject)ReturnValue).Session0 = CurrentSession;
            }
            return ReturnValues;
        }

        /// <summary>
        /// Returns a list of objects that meet the criteria
        /// </summary>
        /// <typeparam name="ObjectType">Object type</typeparam>
        /// <param name="CurrentSession">Current session</param>
        /// <param name="Command">Command to run</param>
        /// <param name="CommandType">Command type</param>
        /// <param name="Parameters">Parameters used in the where clause</param>
        /// <returns>A list of objects that meet the criteria</returns>
        public virtual IEnumerable<ObjectType> All<ObjectType>(Session CurrentSession, string Command, CommandType CommandType, params IParameter[] Parameters) where ObjectType : class,new()
        {
            System.Collections.Generic.List<ObjectType> ReturnValues = new System.Collections.Generic.List<ObjectType>();
            foreach (IDatabase Database in Mappings.Keys.OrderBy(x => x.Order))
            {
                if (Mappings[Database].FirstOrDefault(x => x.ObjectType == typeof(ObjectType)) != null)
                {
                    using (MicroORM ORMObject = new MicroORM(Database.Name))
                    {
                        ReturnValues = (System.Collections.Generic.List<ObjectType>)ORMObject.Map<ObjectType>().All(Command, CommandType, ReturnValues, () => Manager.Create<ObjectType>(), Parameters);
                    }
                }
            }
            foreach (ObjectType ReturnValue in ReturnValues)
            {
                if (ReturnValue is IORMObject)
                    ((IORMObject)ReturnValue).Session0 = CurrentSession;
            }
            return ReturnValues;
        }

        /// <summary>
        /// Returns a list of objects that meet the criteria
        /// </summary>
        /// <typeparam name="ObjectType">Object type</typeparam>
        /// <param name="CurrentSession">Current session</param>
        /// <param name="Parameters">Parameters used in the where clause</param>
        /// <returns>A list of objects that meet the criteria</returns>
        public virtual IEnumerable<ObjectType> All<ObjectType>(Session CurrentSession, params IParameter[] Parameters) where ObjectType : class,new()
        {
            System.Collections.Generic.List<ObjectType> ReturnValues = new System.Collections.Generic.List<ObjectType>();
            foreach (IDatabase Database in Mappings.Keys.OrderBy(x => x.Order))
            {
                IMapping Mapping = Mappings[Database].FirstOrDefault(x => x.ObjectType == typeof(ObjectType));
                if (Mapping != null)
                {
                    using (MicroORM ORMObject = new MicroORM(Database.Name))
                    {
                        if (Mapping.AllCommand == null)
                        {
                            ReturnValues = (System.Collections.Generic.List<ObjectType>)ORMObject.Map<ObjectType>().All("*", 0, "", ReturnValues, () => Manager.Create<ObjectType>(), Parameters);
                        }
                        else
                        {
                            ReturnValues = (System.Collections.Generic.List<ObjectType>)ORMObject.Map<ObjectType>().All(Mapping.AllCommand.CommandToRun, Mapping.AllCommand.CommandType, ReturnValues, () => Manager.Create<ObjectType>(), Parameters);
                        }
                    }
                }
            }
            foreach (ObjectType ReturnValue in ReturnValues)
            {
                if (ReturnValue is IORMObject)
                    ((IORMObject)ReturnValue).Session0 = CurrentSession;
            }
            return ReturnValues;
        }

        #endregion

        #region Delete

        /// <summary>
        /// Deletes the specified object from the database
        /// </summary>
        /// <typeparam name="ObjectType">Object type</typeparam>
        /// <param name="Object">Object to delete</param>
        public virtual void Delete<ObjectType>(ObjectType Object) where ObjectType : class,new()
        {
            foreach (IDatabase Database in Mappings.Keys.OrderBy(x => x.Order))
            {
                IMapping Mapping = Mappings[Database].FirstOrDefault(x => x.ObjectType == typeof(ObjectType));
                if (Mapping != null)
                {
                    using (MicroORM ORMObject = new MicroORM(Database.Name))
                    {
                        foreach (IProperty Property in Mapping.Properties)
                        {
                            if (Property.Cascade)
                            {
                                ((IProperty<ObjectType>)Property).CascadeDelete(Object, ORMObject);
                            }
                        }
                        ORMObject.Map<ObjectType>().Delete(Object);
                    }
                }
            }
        }

        #endregion

        #region LoadProperties

        /// <summary>
        /// Loads a property
        /// </summary>
        /// <typeparam name="ObjectType">Object type</typeparam>
        /// <typeparam name="DataType">Property type</typeparam>
        /// <param name="CurrentSession">Current Session</param>
        /// <param name="Object">Object</param>
        /// <param name="PropertyName">Property name</param>
        /// <param name="Parameters">Extra parameters</param>
        /// <returns>The appropriate property value</returns>
        public virtual IEnumerable<DataType> LoadProperties<ObjectType, DataType>(Session CurrentSession, ObjectType Object, string PropertyName, params IParameter[] Parameters)
            where ObjectType : class,new()
            where DataType : class,new()
        {
            System.Collections.Generic.List<DataType> ReturnValue = new System.Collections.Generic.List<DataType>();
            foreach (IDatabase Database in Mappings.Keys.OrderBy(x => x.Order))
            {
                IMapping Mapping = Mappings[Database].FirstOrDefault(x => x.ObjectType == typeof(ObjectType));
                if (Mapping != null)
                {
                    IProperty Property = Mapping.Properties.FirstOrDefault(x => x.Type == typeof(DataType)
                        && x.Name == PropertyName);
                    if (Property != null)
                    {
                        using (MicroORM ORMObject = new MicroORM(Database.Name))
                        {
                            if (Property.CommandToLoad == null)
                                ReturnValue = (System.Collections.Generic.List<DataType>)ORMObject.Map<DataType>().All("*", 0, "", ReturnValue, () => Manager.Create<DataType>(), Parameters);
                            else
                                ReturnValue = (System.Collections.Generic.List<DataType>)ORMObject.Map<DataType>().All(Property.CommandToLoad.CommandToRun, Property.CommandToLoad.CommandType, ReturnValue, () => Manager.Create<DataType>(), Parameters);
                        }
                    }
                }
            }
            foreach (DataType Item in ReturnValue)
            {
                if (Item is IORMObject)
                    ((IORMObject)Item).Session0 = CurrentSession;
            }
            return ReturnValue;
        }

        #endregion

        #region LoadProperty

        /// <summary>
        /// Loads a property
        /// </summary>
        /// <typeparam name="ObjectType">Object type</typeparam>
        /// <typeparam name="DataType">Property type</typeparam>
        /// <param name="CurrentSession">Current session</param>
        /// <param name="Object">Object</param>
        /// <param name="PropertyName">Property name</param>
        /// <param name="Parameters">Extra parameters</param>
        /// <returns>The appropriate property value</returns>
        public virtual DataType LoadProperty<ObjectType, DataType>(Session CurrentSession, ObjectType Object, string PropertyName, params IParameter[] Parameters)
            where ObjectType : class,new()
            where DataType : class,new()
        {
            DataType ReturnValue = null;
            foreach (IDatabase Database in Mappings.Keys.OrderBy(x => x.Order))
            {
                IMapping Mapping = Mappings[Database].FirstOrDefault(x => x.ObjectType == typeof(ObjectType));
                if (Mapping != null)
                {
                    IProperty Property = Mapping.Properties.FirstOrDefault(x => x.Type == typeof(DataType) && x.Name == PropertyName);
                    if (Property != null)
                    {
                        using (MicroORM ORMObject = new MicroORM(Database.Name))
                        {
                            if (Property.CommandToLoad == null)
                                ReturnValue = ORMObject.Map<DataType>().Any("*", ReturnValue, () => Manager.Create<DataType>(), Parameters);
                            else
                                ReturnValue = ORMObject.Map<DataType>().Any(Property.CommandToLoad.CommandToRun, Property.CommandToLoad.CommandType, ReturnValue, () => Manager.Create<DataType>(), Parameters);
                        }
                    }
                }
            }
            if (ReturnValue is IORMObject)
                ((IORMObject)ReturnValue).Session0 = CurrentSession;
            return ReturnValue;
        }

        #endregion

        #region Paged

        /// <summary>
        /// Returns a paged list of items
        /// </summary>
        /// <typeparam name="ObjectType">Object type</typeparam>
        /// <param name="Columns">Columns to load</param>
        /// <param name="OrderBy">Order by clause (minus the ORDER BY part)</param>
        /// <param name="PageSize">Page size</param>
        /// <param name="CurrentPage">Current page (starting with 0)</param>
        /// <param name="Parameters">Parameters used in the where clause</param>
        /// <param name="CurrentSession">Current session to use in the query</param>
        /// <returns>A paged list of items that match the criteria</returns>
        public virtual IEnumerable<ObjectType> Paged<ObjectType>(Session CurrentSession, string Columns = "*", string OrderBy = "", int PageSize = 25, int CurrentPage = 0, params IParameter[] Parameters) where ObjectType : class,new()
        {
            System.Collections.Generic.List<ObjectType> ReturnValues = new System.Collections.Generic.List<ObjectType>();
            foreach (IDatabase Database in Mappings.Keys.OrderBy(x => x.Order))
            {
                IMapping Mapping = Mappings[Database].FirstOrDefault(x => x.ObjectType == typeof(ObjectType));
                if (Mapping != null)
                {
                    using (MicroORM ORMObject = new MicroORM(Database.Name))
                    {
                        ReturnValues = (System.Collections.Generic.List<ObjectType>)ORMObject.Map<ObjectType>().Paged(Columns, OrderBy, PageSize, CurrentPage, ReturnValues, () => Manager.Create<ObjectType>(), Parameters);
                    }
                }
            }
            foreach (ObjectType ReturnValue in ReturnValues)
            {
                if (ReturnValue is IORMObject)
                    ((IORMObject)ReturnValue).Session0 = CurrentSession;
            }
            return ReturnValues;
        }

        #endregion

        #region Save

        /// <summary>
        /// Saves an object to the database
        /// </summary>
        /// <typeparam name="ObjectType">Object type</typeparam>
        /// <typeparam name="PrimaryKeyType">Primary key type</typeparam>
        /// <param name="Object">Object to save</param>
        /// <param name="Parameters">Extra parameters used in saving the object</param>
        public virtual void Save<ObjectType, PrimaryKeyType>(ObjectType Object, params IParameter[] Parameters) where ObjectType : class,new()
        {
            foreach (IDatabase Database in Mappings.Keys.OrderBy(x => x.Order))
            {
                IMapping Mapping = Mappings[Database].FirstOrDefault(x => x.ObjectType == typeof(ObjectType));
                if (Mapping != null)
                {
                    using (MicroORM ORMObject = new MicroORM(Database.Name))
                    {
                        foreach (IProperty Property in Mapping.Properties)
                        {
                            if (Property.Cascade)
                            {
                                ((IProperty<ObjectType>)Property).CascadeSave(Object, ORMObject);
                            }
                        }
                        System.Collections.Generic.List<IParameter> Params = Parameters.ToList();
                        foreach (IProperty Property in Mapping.Properties)
                        {
                            IParameter Parameter = ((IProperty<ObjectType>)Property).GetAsParameter(Object);
                            if (Parameter != null)
                                Params.Add(Parameter);
                        }
                        ORMObject.Map<ObjectType>().Save<PrimaryKeyType>(Object, Params.ToArray());
                        foreach (IProperty Property in Mapping.Properties)
                        {
                            if (!Property.Cascade && (Property is IManyToMany || Property is IManyToOne || Property is IIEnumerableManyToOne))
                            {
                                ((IProperty<ObjectType>)Property).JoinsDelete(Object, ORMObject);
                                ((IProperty<ObjectType>)Property).JoinsSave(Object, ORMObject);
                            }
                            if (Property.Cascade)
                            {
                                ((IProperty<ObjectType>)Property).CascadeJoinsDelete(Object, ORMObject);
                                ((IProperty<ObjectType>)Property).CascadeJoinsSave(Object, ORMObject);
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #endregion

        #region Properties

        /// <summary>
        /// Mappings associated to databases
        /// </summary>
        public virtual ListMapping<IDatabase, IMapping> Mappings { get; set; }

        /// <summary>
        /// List of database configurations
        /// </summary>
        public virtual System.Collections.Generic.List<IDatabase> Databases { get; set; }

        /// <summary>
        /// AOP manager (used to create objects)
        /// </summary>
        public virtual Reflection.AOP.AOPManager Manager { get; set; }

        #endregion
    }
}