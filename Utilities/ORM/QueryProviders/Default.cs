/*
Copyright (c) 2012 <a href="http://www.gutgames.com">James Craig</a>

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
using System.Data;
using System.Linq;
using System.Reflection;
using Utilities.DataTypes;
using Utilities.DataTypes.ExtensionMethods;
using Utilities.ORM.Aspect.Interfaces;
using Utilities.ORM.Mapping.Interfaces;
using Utilities.ORM.QueryProviders.Interfaces;
using Utilities.Reflection.ExtensionMethods;
using Utilities.SQL;
using Utilities.SQL.Interfaces;
using Utilities.SQL.MicroORM;
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
        /// <param name="Profile">Should profiling be done</param>
        /// <param name="AssembliesUsing">Assemblies using</param>
        public Default(bool Profile, params Assembly[] AssembliesUsing)
        {
            this.Profile = Profile;
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
                    SQLHelper.Database(TempObject.ConnectionString, TempObject.Name);
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
            foreach (IDatabase Database in Mappings.Keys.Where(x => x.Readable).OrderBy(x => x.Order))
            {
                IMapping Mapping = Mappings[Database].FirstOrDefault(x => x.ObjectType == typeof(ObjectType));
                if (Mapping != null)
                {
                    using (SQLHelper ORMObject = new SQLHelper(Database.Name))
                    {
                        if (Mapping.AnyCommand == null)
                            ReturnValue = ORMObject.Any<ObjectType>("*", ReturnValue, () => Manager.Create<ObjectType>(),false, Parameters);
                        else
                            ReturnValue = ORMObject.Any<ObjectType>(Mapping.AnyCommand.SQLCommand, Mapping.AnyCommand.CommandType, ReturnValue, () => Manager.Create<ObjectType>(),false, Parameters);
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
            foreach (IDatabase Database in Mappings.Keys.Where(x => x.Readable).OrderBy(x => x.Order))
            {
                if (Mappings[Database].FirstOrDefault(x => x.ObjectType == typeof(ObjectType)) != null)
                {
                    using (SQLHelper ORMObject = new SQLHelper(Database.Name))
                    {
                        ReturnValue = ORMObject.Any<ObjectType>(Columns, ReturnValue, () => Manager.Create<ObjectType>(),false, Parameters);
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
            foreach (IDatabase Database in Mappings.Keys.Where(x => x.Readable).OrderBy(x => x.Order))
            {
                if (Mappings[Database].FirstOrDefault(x => x.ObjectType == typeof(ObjectType)) != null)
                {
                    using (SQLHelper ORMObject = new SQLHelper(Database.Name))
                    {
                        ReturnValue = ORMObject.Any<ObjectType>(Command, CommandType, ReturnValue, () => Manager.Create<ObjectType>(),false, Parameters);
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
            foreach (IDatabase Database in Mappings.Keys.Where(x => x.Readable).OrderBy(x => x.Order))
            {
                if (Mappings[Database].FirstOrDefault(x => x.ObjectType == typeof(ObjectType)) != null)
                {
                    using (SQLHelper ORMObject = new SQLHelper(Database.Name))
                    {
                        ReturnValues = (System.Collections.Generic.List<ObjectType>)ORMObject.All<ObjectType>(Columns, Limit, OrderBy, ReturnValues, () => Manager.Create<ObjectType>(),false, Parameters);
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
            foreach (IDatabase Database in Mappings.Keys.Where(x => x.Readable).OrderBy(x => x.Order))
            {
                if (Mappings[Database].FirstOrDefault(x => x.ObjectType == typeof(ObjectType)) != null)
                {
                    using (SQLHelper ORMObject = new SQLHelper(Database.Name))
                    {
                        ReturnValues = (System.Collections.Generic.List<ObjectType>)ORMObject.All<ObjectType>(Command, CommandType, ReturnValues, () => Manager.Create<ObjectType>(),false, Parameters);
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
            foreach (IDatabase Database in Mappings.Keys.Where(x => x.Readable).OrderBy(x => x.Order))
            {
                IMapping Mapping = Mappings[Database].FirstOrDefault(x => x.ObjectType == typeof(ObjectType));
                if (Mapping != null)
                {
                    using (SQLHelper ORMObject = new SQLHelper(Database.Name))
                    {
                        if (Mapping.AllCommand == null)
                        {
                            ReturnValues = (System.Collections.Generic.List<ObjectType>)ORMObject.All<ObjectType>("*", 0, "", ReturnValues, () => Manager.Create<ObjectType>(),false, Parameters);
                        }
                        else
                        {
                            ReturnValues = (System.Collections.Generic.List<ObjectType>)ORMObject.All<ObjectType>(Mapping.AllCommand.SQLCommand, Mapping.AllCommand.CommandType, ReturnValues, () => Manager.Create<ObjectType>(),false, Parameters);
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
            foreach (IDatabase Database in Mappings.Keys.Where(x => x.Writable).OrderBy(x => x.Order))
            {
                IMapping Mapping = Mappings[Database].FirstOrDefault(x => x.ObjectType == typeof(ObjectType));
                if (Mapping != null)
                {
                    using (SQLHelper ORMObject = new SQLHelper(Database.Name))
                    {
                        System.Collections.Generic.List<Command> JoinCommands = new System.Collections.Generic.List<Command>();
                        foreach (IProperty Property in Mapping.Properties)
                        {
                            if (!Property.Cascade && Mapping.ObjectType == Property.Type)
                            {
                                JoinCommands.AddIfUnique(((IProperty<ObjectType>)Property).JoinsDelete(Object, ORMObject));
                            }
                            if (Property.Cascade)
                            {
                                JoinCommands.AddIfUnique(((IProperty<ObjectType>)Property).CascadeJoinsDelete(Object, ORMObject));
                            }
                        }
                        if (JoinCommands.Count > 0)
                        {
                            ORMObject.Batch().AddCommands(JoinCommands.ToArray());
                            ORMObject.ExecuteNonQuery();
                        }
                        foreach (IProperty Property in Mapping.Properties)
                        {
                            if (Property.Cascade)
                            {
                                ((IProperty<ObjectType>)Property).CascadeDelete(Object, ORMObject);
                            }
                        }
                        ORMObject.Delete<ObjectType>(Object);
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
            foreach (IDatabase Database in Mappings.Keys.Where(x => x.Readable).OrderBy(x => x.Order))
            {
                IMapping Mapping = Mappings[Database].FirstOrDefault(x => x.ObjectType == typeof(ObjectType));
                if (Mapping != null)
                {
                    IProperty Property = Mapping.Properties.FirstOrDefault(x => x.Type == typeof(DataType)
                        && x.Name == PropertyName);
                    if (Property != null)
                    {
                        using (SQLHelper ORMObject = new SQLHelper(Database.Name))
                        {
                            if (Property.CommandToLoad == null)
                                ReturnValue = (System.Collections.Generic.List<DataType>)ORMObject.All<DataType>("*", 0, "", ReturnValue, () => Manager.Create<DataType>(), false, Parameters);
                            else
                                ReturnValue = (System.Collections.Generic.List<DataType>)ORMObject.All<DataType>(Property.CommandToLoad.SQLCommand, Property.CommandToLoad.CommandType, ReturnValue, () => Manager.Create<DataType>(), false, Parameters);
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

        #region LoadListProperties

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
        public virtual System.Collections.Generic.List<DataType> LoadListProperties<ObjectType, DataType>(Session CurrentSession, ObjectType Object, string PropertyName, params IParameter[] Parameters)
            where ObjectType : class,new()
            where DataType : class,new()
        {
            System.Collections.Generic.List<DataType> ReturnValue = new System.Collections.Generic.List<DataType>();
            foreach (IDatabase Database in Mappings.Keys.Where(x => x.Readable).OrderBy(x => x.Order))
            {
                IMapping Mapping = Mappings[Database].FirstOrDefault(x => x.ObjectType == typeof(ObjectType));
                if (Mapping != null)
                {
                    IProperty Property = Mapping.Properties.FirstOrDefault(x => x.Type == typeof(DataType)
                        && x.Name == PropertyName);
                    if (Property != null)
                    {
                        using (SQLHelper ORMObject = new SQLHelper(Database.Name))
                        {
                            if (Property.CommandToLoad == null)
                                ReturnValue = (System.Collections.Generic.List<DataType>)ORMObject.All<DataType>("*", 0, "", ReturnValue, () => Manager.Create<DataType>(),false, Parameters);
                            else
                                ReturnValue = (System.Collections.Generic.List<DataType>)ORMObject.All<DataType>(Property.CommandToLoad.SQLCommand, Property.CommandToLoad.CommandType, ReturnValue, () => Manager.Create<DataType>(),false, Parameters);
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
            foreach (IDatabase Database in Mappings.Keys.Where(x => x.Readable).OrderBy(x => x.Order))
            {
                IMapping Mapping = Mappings[Database].FirstOrDefault(x => x.ObjectType == typeof(ObjectType));
                if (Mapping != null)
                {
                    IProperty Property = Mapping.Properties.FirstOrDefault(x => x.Type == typeof(DataType) && x.Name == PropertyName);
                    if (Property != null)
                    {
                        using (SQLHelper ORMObject = new SQLHelper(Database.Name))
                        {
                            if (Property.CommandToLoad == null)
                                ReturnValue = ORMObject.Any<DataType>("*", ReturnValue, () => Manager.Create<DataType>(), false, Parameters);
                            else
                                ReturnValue = ORMObject.Any<DataType>(Property.CommandToLoad.SQLCommand, Property.CommandToLoad.CommandType, ReturnValue, () => Manager.Create<DataType>(), false, Parameters);
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
            foreach (IDatabase Database in Mappings.Keys.Where(x => x.Readable).OrderBy(x => x.Order))
            {
                IMapping Mapping = Mappings[Database].FirstOrDefault(x => x.ObjectType == typeof(ObjectType));
                if (Mapping != null)
                {
                    using (SQLHelper ORMObject = new SQLHelper(Database.Name))
                    {
                        ReturnValues = (System.Collections.Generic.List<ObjectType>)ORMObject.Paged<ObjectType>(Columns, OrderBy, PageSize, CurrentPage, ReturnValues, () => Manager.Create<ObjectType>(),false, Parameters);
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

        #region PagedCommand

        /// <summary>
        /// Returns a paged list of items
        /// </summary>
        /// <typeparam name="ObjectType">Object type</typeparam>
        /// <param name="Command">Command to use</param>
        /// <param name="OrderBy">Order by clause (minus the ORDER BY part)</param>
        /// <param name="PageSize">Page size</param>
        /// <param name="CurrentPage">Current page (starting with 0)</param>
        /// <param name="Parameters">Parameters used in the where clause</param>
        /// <param name="CurrentSession">Current session to use in the query</param>
        /// <returns>A paged list of items that match the criteria</returns>
        public virtual IEnumerable<ObjectType> PagedCommand<ObjectType>(Session CurrentSession, string Command, string OrderBy = "", int PageSize = 25, int CurrentPage = 0, params IParameter[] Parameters) where ObjectType : class,new()
        {
            System.Collections.Generic.List<ObjectType> ReturnValues = new System.Collections.Generic.List<ObjectType>();
            foreach (IDatabase Database in Mappings.Keys.Where(x => x.Readable).OrderBy(x => x.Order))
            {
                IMapping Mapping = Mappings[Database].FirstOrDefault(x => x.ObjectType == typeof(ObjectType));
                if (Mapping != null)
                {
                    using (SQLHelper ORMObject = new SQLHelper(Database.Name))
                    {
                        ReturnValues = (System.Collections.Generic.List<ObjectType>)ORMObject.PagedCommand<ObjectType>(Command, OrderBy, PageSize, CurrentPage, ReturnValues, () => Manager.Create<ObjectType>(),false, Parameters);
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

        #region PageCount

        /// <summary>
        /// Gets the number of pages based on the specified info
        /// </summary>
        /// <param name="CurrentSession">Current session</param>
        /// <param name="PageSize">Page size</param>
        /// <param name="Parameters">Parameters to search by</param>
        /// <returns>The number of pages that the table contains for the specified page size</returns>
        public virtual int PageCount<ObjectType>(Session CurrentSession, int PageSize = 25, params IParameter[] Parameters) where ObjectType : class,new()
        {
            foreach (IDatabase Database in Mappings.Keys.Where(x => x.Readable).OrderBy(x => x.Order))
            {
                IMapping Mapping = Mappings[Database].FirstOrDefault(x => x.ObjectType == typeof(ObjectType));
                if (Mapping != null)
                {
                    using (SQLHelper ORMObject = new SQLHelper(Database.Name))
                    {
                        return ORMObject.PageCount<ObjectType>(PageSize,false, Parameters);
                    }
                }
            }
            return 0;
        }

        /// <summary>
        /// Gets the number of pages based on the specified info
        /// </summary>
        /// <param name="CurrentSession">Current session</param>
        /// <param name="PageSize">Page size</param>
        /// <param name="Parameters">Parameters to search by</param>
        /// <param name="Command">Command to get the page count of</param>
        /// <returns>The number of pages that the table contains for the specified page size</returns>
        public virtual int PageCount<ObjectType>(Session CurrentSession, string Command, int PageSize = 25, params IParameter[] Parameters) where ObjectType : class,new()
        {
            foreach (IDatabase Database in Mappings.Keys.Where(x => x.Readable).OrderBy(x => x.Order))
            {
                IMapping Mapping = Mappings[Database].FirstOrDefault(x => x.ObjectType == typeof(ObjectType));
                if (Mapping != null)
                {
                    using (SQLHelper ORMObject = new SQLHelper(Database.Name))
                    {
                        return ORMObject.PageCount<ObjectType>(Command, PageSize,false, Parameters);
                    }
                }
            }
            return 0;
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
            foreach (IDatabase Database in Mappings.Keys.Where(x => x.Writable).OrderBy(x => x.Order))
            {
                IMapping Mapping = Mappings[Database].FirstOrDefault(x => x.ObjectType == typeof(ObjectType));
                if (Mapping != null)
                {
                    using (SQLHelper ORMObject = new SQLHelper(Database.Name))
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
                        ORMObject.Save<ObjectType, PrimaryKeyType>(Object, Params.ToArray());
                        System.Collections.Generic.List<Command> JoinCommands = new System.Collections.Generic.List<Command>();
                        foreach (IProperty Property in Mapping.Properties)
                        {
                            if (!Property.Cascade &&
                                (Property is IManyToMany
                                    || Property is IManyToOne
                                    || Property is IIEnumerableManyToOne
                                    || Property is IListManyToMany
                                    || Property is IListManyToOne))
                            {
                                JoinCommands.AddIfUnique(((IProperty<ObjectType>)Property).JoinsDelete(Object, ORMObject));
                            }
                            if (Property.Cascade)
                            {
                                JoinCommands.AddIfUnique(((IProperty<ObjectType>)Property).CascadeJoinsDelete(Object, ORMObject));
                            }
                        }
                        if (JoinCommands.Count > 0)
                        {
                            ORMObject.Batch().AddCommands(JoinCommands.ToArray());
                            ORMObject.ExecuteNonQuery();
                        }
                        JoinCommands = new System.Collections.Generic.List<Command>();
                        foreach (IProperty Property in Mapping.Properties)
                        {
                            if (!Property.Cascade &&
                                (Property is IManyToMany
                                    || Property is IManyToOne
                                    || Property is IIEnumerableManyToOne
                                    || Property is IListManyToMany
                                    || Property is IListManyToOne))
                            {
                                JoinCommands.AddIfUnique(((IProperty<ObjectType>)Property).JoinsSave(Object, ORMObject));
                            }
                            if (Property.Cascade)
                            {
                                JoinCommands.AddIfUnique(((IProperty<ObjectType>)Property).CascadeJoinsSave(Object, ORMObject));
                            }
                        }
                        if (JoinCommands.Count > 0)
                        {
                            ORMObject.Batch().AddCommands(JoinCommands.ToArray());
                            ORMObject.ExecuteNonQuery();
                        }
                    }
                }
            }
        }

        #endregion

        #region Scalar

        /// <summary>
        /// Runs a supplied scalar function and returns the result
        /// </summary>
        /// <param name="CommandType">Command type</param>
        /// <param name="Parameters">Parameters to search by</param>
        /// <param name="Command">Command to get the page count of</param>
        /// <param name="CurrentSession">Current session</param>
        /// <typeparam name="DataType">Data type</typeparam>
        /// <typeparam name="ObjectType">Object type</typeparam>
        /// <returns>The scalar value returned by the command</returns>
        public virtual DataType Scalar<ObjectType, DataType>(Session CurrentSession, string Command, CommandType CommandType, params IParameter[] Parameters)
            where ObjectType : class,new()
        {
            foreach (IDatabase Database in Mappings.Keys.Where(x => x.Readable).OrderBy(x => x.Order))
            {
                IMapping Mapping = Mappings[Database].FirstOrDefault(x => x.ObjectType == typeof(ObjectType));
                if (Mapping != null)
                {
                    using (SQLHelper ORMObject = new SQLHelper(Database.Name))
                    {
                        return ORMObject.Scalar<ObjectType, DataType>(Command, CommandType, false, Parameters);
                    }
                }
            }
            return default(DataType);
        }

        /// <summary>
        /// Runs a scalar command using the specified aggregate function
        /// </summary>
        /// <typeparam name="DataType">Data type</typeparam>
        /// <typeparam name="ObjectType">Object type</typeparam>
        /// <param name="CurrentSession">Current session</param>
        /// <param name="AggregateFunction">Aggregate function</param>
        /// <param name="Parameters">Parameters</param>
        /// <returns>The scalar value returned by the command</returns>
        public virtual DataType Scalar<ObjectType, DataType>(Session CurrentSession, string AggregateFunction, params IParameter[] Parameters)
            where ObjectType : class,new()
        {
            foreach (IDatabase Database in Mappings.Keys.Where(x => x.Readable).OrderBy(x => x.Order))
            {
                IMapping Mapping = Mappings[Database].FirstOrDefault(x => x.ObjectType == typeof(ObjectType));
                if (Mapping != null)
                {
                    using (SQLHelper ORMObject = new SQLHelper(Database.Name))
                    {
                        return ORMObject.Scalar<ObjectType, DataType>(AggregateFunction, false, Parameters);
                    }
                }
            }
            return default(DataType);
        }

        #endregion

        #endregion

        #region Properties

        /// <summary>
        /// Mappings associated to databases
        /// </summary>
        public ListMapping<IDatabase, IMapping> Mappings { get;private set; }

        /// <summary>
        /// List of database configurations
        /// </summary>
        public System.Collections.Generic.List<IDatabase> Databases { get;private set; }

        /// <summary>
        /// AOP manager (used to create objects)
        /// </summary>
        public Reflection.AOP.AOPManager Manager { get; set; }

        /// <summary>
        /// Should the queries be profiled?
        /// </summary>
        public bool Profile { get; set; }

        #endregion
    }
}