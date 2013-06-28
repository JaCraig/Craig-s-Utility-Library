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
using System.Diagnostics.CodeAnalysis;
using Utilities.DataTypes.EventArgs;
using Utilities.DataTypes.ExtensionMethods;
using Utilities.ORM.Interfaces;
using Utilities.SQL.Interfaces;
using Utilities.Validation.ExtensionMethods;
#endregion

namespace Utilities.ORM
{
    /// <summary>
    /// Object base class helper. This is not required but automatically
    /// sets up basic functions and properties to simplify things a bit.
    /// </summary>
    /// <typeparam name="IDType">ID type</typeparam>
    /// <typeparam name="ObjectType">Object type (must be the child object type)</typeparam>
    public abstract class ObjectBaseClass<ObjectType, IDType> : IComparable, IComparable<ObjectType>, IObject<IDType>
        where ObjectType : ObjectBaseClass<ObjectType, IDType>, new()
        where IDType : IComparable
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        protected ObjectBaseClass()
        {
            this.Active = true;
            this.DateCreated = DateTime.Now;
            this.DateModified = DateTime.Now;
        }

        #endregion

        #region Static Functions

        #region Any

        /// <summary>
        /// Loads the item based on the ID
        /// </summary>
        /// <param name="Params">Parameters used to specify what to load</param>
        /// <returns>The specified item</returns>
        public static ObjectType Any(params IParameter[] Params)
        {
            return Any(ORM.CreateSession(), Params);
        }

        /// <summary>
        /// Loads the item based on the ID
        /// </summary>
        /// <param name="Params">Parameters used to specify what to load</param>
        /// <param name="Session">ORM session variable</param>
        /// <returns>The specified item</returns>
        public static ObjectType Any(Session Session, params IParameter[] Params)
        {
            ObjectType instance = new ObjectType();
            LoadingEventArgs E = new LoadingEventArgs();
            E.Content = Params;
            instance.OnLoading(E);
            if (!E.Stop)
            {
                instance = Session.Any<ObjectType>(Params);
                if (instance != null)
                    instance.OnLoaded(new LoadedEventArgs());
            }
            return instance;
        }

        /// <summary>
        /// Loads the item based on the ID
        /// </summary>
        /// <param name="Command">Command</param>
        /// <param name="CommandType">Command type</param>
        /// <param name="Params">Parameters used to specify what to load</param>
        /// <returns>The specified item</returns>
        public static ObjectType Any(string Command, CommandType CommandType, params IParameter[] Params)
        {
            return Any(ORM.CreateSession(), Command, CommandType, Params);
        }

        /// <summary>
        /// Loads the item based on the ID
        /// </summary>
        /// <param name="Command">Command</param>
        /// <param name="CommandType">Command type</param>
        /// <param name="Params">Parameters used to specify what to load</param>
        /// <param name="Session">ORM session variable</param>
        /// <returns>The specified item</returns>
        public static ObjectType Any(Session Session, string Command, CommandType CommandType, params IParameter[] Params)
        {
            ObjectType instance = new ObjectType();
            LoadingEventArgs E = new LoadingEventArgs();
            E.Content = Params;
            instance.OnLoading(E);
            if (!E.Stop)
            {
                instance = Session.Any<ObjectType>(Command, CommandType, Params);
                if (instance != null)
                    instance.OnLoaded(new LoadedEventArgs());
            }
            return instance;
        }

        #endregion
        
        #region All

        /// <summary>
        /// Loads the items based on type
        /// </summary>
        /// <param name="Params">Parameters used to specify what to load</param>
        /// <returns>All items that fit the specified query</returns>
        public static IEnumerable<ObjectType> All(params IParameter[] Params)
        {
            return All(ORM.CreateSession(), Params);
        }

        /// <summary>
        /// Loads the items based on type
        /// </summary>
        /// <param name="Params">Parameters used to specify what to load</param>
        /// <param name="Session">ORM session variable</param>
        /// <returns>All items that fit the specified query</returns>
        public static IEnumerable<ObjectType> All(Session Session, params IParameter[] Params)
        {
            IEnumerable<ObjectType> instance = new List<ObjectType>();
            LoadingEventArgs E = new LoadingEventArgs();
            ObjectBaseClass<ObjectType, IDType>.OnLoading(null, E);
            if (!E.Stop)
            {
                instance = Session.All<ObjectType>(Params);
                foreach (ObjectType Item in instance)
                {
                    Item.OnLoaded(new LoadedEventArgs());
                }
            }
            return instance;
        }

        /// <summary>
        /// Loads the items based on type
        /// </summary>
        /// <param name="Command">Command</param>
        /// <param name="CommandType">Command type</param>
        /// <param name="Params">Parameters used to specify what to load</param>
        /// <returns>All items that fit the specified query</returns>
        public static IEnumerable<ObjectType> All(string Command, CommandType CommandType, params IParameter[] Params)
        {
            return All(ORM.CreateSession(), Command, CommandType, Params);
        }

        /// <summary>
        /// Loads the items based on type
        /// </summary>
        /// <param name="Command">Command</param>
        /// <param name="CommandType">Command type</param>
        /// <param name="Params">Parameters used to specify what to load</param>
        /// <param name="Session">ORM session variable</param>
        /// <returns>All items that fit the specified query</returns>
        public static IEnumerable<ObjectType> All(Session Session, string Command, CommandType CommandType, params IParameter[] Params)
        {
            IEnumerable<ObjectType> instance = new List<ObjectType>();
            LoadingEventArgs E = new LoadingEventArgs();
            ObjectBaseClass<ObjectType, IDType>.OnLoading(null, E);
            if (!E.Stop)
            {
                instance = Session.All<ObjectType>(Command, CommandType, Params);
                foreach (ObjectType Item in instance)
                {
                    Item.OnLoaded(new LoadedEventArgs());
                }
            }
            return instance;
        }

        #endregion

        #region Paged

        /// <summary>
        /// Loads the items based on type
        /// </summary>
        /// <param name="OrderBy">What the data is ordered by</param>
        /// <param name="PageSize">Page size</param>
        /// <param name="CurrentPage">Current page (0 based)</param>
        /// <param name="Params">Parameters used to specify what to load</param>
        /// <returns>All items that fit the specified query</returns>
        public static IEnumerable<ObjectType> Paged(string OrderBy = "", int PageSize = 25, int CurrentPage = 0, params IParameter[] Params)
        {
            return Paged(ORM.CreateSession(), OrderBy, PageSize, CurrentPage, Params);
        }

        /// <summary>
        /// Loads the items based on type
        /// </summary>
        /// <param name="OrderBy">What the data is ordered by</param>
        /// <param name="PageSize">Page size</param>
        /// <param name="CurrentPage">Current page (0 based)</param>
        /// <param name="Params">Parameters used to specify what to load</param>
        /// <param name="Session">ORM session variable</param>
        /// <returns>All items that fit the specified query</returns>
        public static IEnumerable<ObjectType> Paged(Session Session, string OrderBy = "", int PageSize = 25, int CurrentPage = 0, params IParameter[] Params)
        {
            IEnumerable<ObjectType> instance = new List<ObjectType>();
            LoadingEventArgs E = new LoadingEventArgs();
            ObjectBaseClass<ObjectType, IDType>.OnLoading(null, E);
            if (!E.Stop)
            {
                instance = Session.Paged<ObjectType>("*", OrderBy, PageSize, CurrentPage, Params);
                foreach (ObjectType Item in instance)
                {
                    Item.OnLoaded(new LoadedEventArgs());
                }
            }
            return instance;
        }

        #endregion

        #region PagedCommand

        /// <summary>
        /// Loads the items based on type
        /// </summary>
        /// <param name="OrderBy">What the data is ordered by</param>
        /// <param name="PageSize">Page size</param>
        /// <param name="CurrentPage">Current page (0 based)</param>
        /// <param name="Params">Parameters used to specify what to load</param>
        /// <param name="Command">Command to run</param>
        /// <returns>All items that fit the specified query</returns>
        public static IEnumerable<ObjectType> PagedCommand(string Command, string OrderBy = "", int PageSize = 25, int CurrentPage = 0, params IParameter[] Params)
        {
            return PagedCommand(ORM.CreateSession(), Command, OrderBy, PageSize, CurrentPage, Params);
        }

        /// <summary>
        /// Loads the items based on type
        /// </summary>
        /// <param name="OrderBy">What the data is ordered by</param>
        /// <param name="PageSize">Page size</param>
        /// <param name="CurrentPage">Current page (0 based)</param>
        /// <param name="Params">Parameters used to specify what to load</param>
        /// <param name="Session">ORM session variable</param>
        /// <param name="Command">Command to run</param>
        /// <returns>All items that fit the specified query</returns>
        public static IEnumerable<ObjectType> PagedCommand(Session Session, string Command, string OrderBy = "", int PageSize = 25, int CurrentPage = 0, params IParameter[] Params)
        {
            IEnumerable<ObjectType> instance = new List<ObjectType>();
            LoadingEventArgs E = new LoadingEventArgs();
            ObjectBaseClass<ObjectType, IDType>.OnLoading(null, E);
            if (!E.Stop)
            {
                instance = Session.PagedCommand<ObjectType>(Command, OrderBy, PageSize, CurrentPage, Params);
                foreach (ObjectType Item in instance)
                {
                    Item.OnLoaded(new LoadedEventArgs());
                }
            }
            return instance;
        }

        #endregion

        #region PageCount

        /// <summary>
        /// Gets the page count based on page size
        /// </summary>
        /// <param name="PageSize">Page size</param>
        /// <param name="Params">Parameters used to specify what to load</param>
        /// <returns>All items that fit the specified query</returns>
        public static int PageCount(int PageSize = 25, params IParameter[] Params)
        {
            return PageCount(ORM.CreateSession(), PageSize, Params);
        }

        /// <summary>
        /// Gets the page count based on page size
        /// </summary>
        /// <param name="PageSize">Page size</param>
        /// <param name="Params">Parameters used to specify what to load</param>
        /// <param name="Session">ORM session variable</param>
        /// <returns>All items that fit the specified query</returns>
        public static int PageCount(Session Session, int PageSize = 25, params IParameter[] Params)
        {
            return Session.PageCount<ObjectType>(PageSize, Params);
        }

        /// <summary>
        /// Gets the page count based on page size
        /// </summary>
        /// <param name="PageSize">Page size</param>
        /// <param name="Params">Parameters used to specify what to load</param>
        /// <param name="Command">Command to run</param>
        /// <returns>All items that fit the specified query</returns>
        public static int PageCount(string Command, int PageSize = 25, params IParameter[] Params)
        {
            return PageCount(ORM.CreateSession(), Command, PageSize, Params);
        }

        /// <summary>
        /// Gets the page count based on page size
        /// </summary>
        /// <param name="PageSize">Page size</param>
        /// <param name="Params">Parameters used to specify what to load</param>
        /// <param name="Session">ORM session variable</param>
        /// <param name="Command">Command to run</param>
        /// <returns>All items that fit the specified query</returns>
        public static int PageCount(Session Session, string Command, int PageSize = 25, params IParameter[] Params)
        {
            return Session.PageCount<ObjectType>(Command, PageSize, Params);
        }

        #endregion

        #region Save

        /// <summary>
        /// Saves a list of objects
        /// </summary>
        /// <param name="Objects">List of objects</param>
        public static void Save(IEnumerable<ObjectType> Objects)
        {
            ObjectBaseClass<ObjectType, IDType>.Save(Objects, ORM.CreateSession());
        }

        /// <summary>
        /// Saves a list of objects
        /// </summary>
        /// <param name="Objects">List of objects</param>
        /// <param name="Session">ORM session variable</param>
        public static void Save(IEnumerable<ObjectType> Objects, Session Session)
        {
            foreach (ObjectType Object in Objects)
            {
                Object.SetupObject();
                Object.Validate();
                Object.Save(Session);
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
        /// <typeparam name="DataType">Data type</typeparam>
        /// <returns>The scalar value returned by the command</returns>
        public static DataType Scalar<DataType>(string Command, CommandType CommandType, params IParameter[] Parameters)
        {
            return Scalar<DataType>(ORM.CreateSession(), Command, CommandType, Parameters);
        }

        /// <summary>
        /// Runs a scalar command using the specified aggregate function
        /// </summary>
        /// <typeparam name="DataType">Data type</typeparam>
        /// <param name="AggregateFunction">Aggregate function</param>
        /// <param name="Parameters">Parameters</param>
        /// <returns>The scalar value returned by the command</returns>
        public static DataType Scalar<DataType>(string AggregateFunction, params IParameter[] Parameters)
        {
            return Scalar<DataType>(ORM.CreateSession(), AggregateFunction, Parameters);
        }

        /// <summary>
        /// Runs a supplied scalar function and returns the result
        /// </summary>
        /// <param name="CommandType">Command type</param>
        /// <param name="Parameters">Parameters to search by</param>
        /// <param name="Command">Command to get the page count of</param>
        /// <param name="Session">ORM session variable</param>
        /// <typeparam name="DataType">Data type</typeparam>
        /// <returns>The scalar value returned by the command</returns>
        public static DataType Scalar<DataType>(Session Session, string Command, CommandType CommandType, params IParameter[] Parameters)
        {
            return Session.Scalar<ObjectType, DataType>(Command, CommandType, Parameters);
        }

        /// <summary>
        /// Runs a scalar command using the specified aggregate function
        /// </summary>
        /// <typeparam name="DataType">Data type</typeparam>
        /// <param name="AggregateFunction">Aggregate function</param>
        /// <param name="Parameters">Parameters</param>
        /// <param name="Session">Session object</param>
        /// <returns>The scalar value returned by the command</returns>
        public static DataType Scalar<DataType>(Session Session, string AggregateFunction, params IParameter[] Parameters)
        {
            return Session.Scalar<ObjectType, DataType>(AggregateFunction, Parameters);
        }

        #endregion

        #endregion

        #region Functions

        /// <summary>
        /// Sets up the object for saving purposes
        /// </summary>
        public virtual void SetupObject()
        {
            DateModified = DateTime.Now;
        }

        /// <summary>
        /// Saves the item (if it already exists, it updates the item.
        /// Otherwise it inserts the item)
        /// </summary>
        public virtual void Save()
        {
            Save(ORM.CreateSession());
        }

        /// <summary>
        /// Deletes the item
        /// </summary>
        public virtual void Delete()
        {
            Delete(ORM.CreateSession());
        }

        /// <summary>
        /// Saves the item (if it already exists, it updates the item.
        /// Otherwise it inserts the item)
        /// </summary>
        /// <param name="Session">ORM session variable</param>
        public virtual void Save(Session Session)
        {
            SavingEventArgs E = new SavingEventArgs();
            OnSaving(E);

            if (!E.Stop)
            {
                SetupObject();
                this.Validate();
                Session.Save<ObjectType, IDType>((ObjectType)this);
                SavedEventArgs X = new SavedEventArgs();
                OnSaved(X);
            }
        }

        /// <summary>
        /// Deletes the item
        /// </summary>
        /// <param name="Session">ORM session variable</param>
        public virtual void Delete(Session Session)
        {
            DeletingEventArgs E = new DeletingEventArgs();
            OnDeleting(E);
            if (!E.Stop)
            {
                Session.Delete((ObjectType)this);
                DeletedEventArgs X = new DeletedEventArgs();
                OnDeleted(X);
            }
        }

        #endregion

        #region Overridden Functions

        /// <summary>
        /// Returns the hash of this item
        /// </summary>
        /// <returns>the int hash of the item</returns>
        public override int GetHashCode()
        {
            return this.ID.GetHashCode();
        }

        /// <summary>
        /// Determines if two items are equal
        /// </summary>
        /// <param name="obj">The object to compare this to</param>
        /// <returns>true if they are the same, false otherwise</returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (obj.GetType() == this.GetType())
            {
                return obj.GetHashCode() == this.GetHashCode();
            }

            return false;
        }

        /// <summary>
        /// The &lt; operator
        /// </summary>
        /// <param name="first">First item</param>
        /// <param name="second">Second item</param>
        /// <returns>True if the first item is less than the second, false otherwise</returns>
        public static bool operator <(ObjectBaseClass<ObjectType, IDType> first, ObjectBaseClass<ObjectType, IDType> second)
        {
            if (Object.ReferenceEquals(first, second))
                return false;
            if ((object)first == null || (object)second == null)
                return false;
            return first.GetHashCode() < second.GetHashCode();
        }

        /// <summary>
        /// The &gt; operator
        /// </summary>
        /// <param name="first">First item</param>
        /// <param name="second">Second item</param>
        /// <returns>True if the first item is greater than the second, false otherwise</returns>
        public static bool operator >(ObjectBaseClass<ObjectType, IDType> first, ObjectBaseClass<ObjectType, IDType> second)
        {
            if (Object.ReferenceEquals(first, second))
                return false;
            if ((object)first == null || (object)second == null)
                return false;
            return first.GetHashCode() > second.GetHashCode();
        }

        /// <summary>
        /// The == operator
        /// </summary>
        /// <param name="first">First item</param>
        /// <param name="second">Second item</param>
        /// <returns>true if the first and second item are the same, false otherwise</returns>
        public static bool operator ==(ObjectBaseClass<ObjectType, IDType> first, ObjectBaseClass<ObjectType, IDType> second)
        {
            if (Object.ReferenceEquals(first, second))
                return true;

            if ((object)first == null || (object)second == null)
                return false;

            return first.GetHashCode() == second.GetHashCode();
        }

        /// <summary>
        /// != operator
        /// </summary>
        /// <param name="first">First item</param>
        /// <param name="second">Second item</param>
        /// <returns>returns true if they are not equal, false otherwise</returns>
        public static bool operator !=(ObjectBaseClass<ObjectType, IDType> first, ObjectBaseClass<ObjectType, IDType> second)
        {
            return !(first == second);
        }

        #endregion

        #region Events

        /// <summary>
        /// Called when the object is saved
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static EventHandler<SavedEventArgs> Saved;

        /// <summary>
        /// Called when the item is Saved
        /// </summary>
        /// <param name="e">SavedEventArgs item</param>
        protected virtual void OnSaved(SavedEventArgs e)
        {
            Saved.Raise(this, e);
        }

        /// <summary>
        /// Called when the object is deleted
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static EventHandler<DeletedEventArgs> Deleted;

        /// <summary>
        /// Called when the item is Deleted
        /// </summary>
        /// <param name="e">DeletedEventArgs item</param>
        protected virtual void OnDeleted(DeletedEventArgs e)
        {
            Deleted.Raise(this, e);
        }

        /// <summary>
        /// Called prior to an object is saving
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static EventHandler<SavingEventArgs> Saving;

        /// <summary>
        /// Called when the item is Saving
        /// </summary>
        /// <param name="e">SavingEventArgs item</param>
        protected virtual void OnSaving(SavingEventArgs e)
        {
            Saving.Raise(this, e);
        }

        /// <summary>
        /// Called prior to an object is deleting
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static EventHandler<DeletingEventArgs> Deleting;

        /// <summary>
        /// Called when the item is Deleting
        /// </summary>
        /// <param name="e">DeletingEventArgs item</param>
        protected virtual void OnDeleting(DeletingEventArgs e)
        {
            Deleting.Raise(this, e);
        }

        /// <summary>
        /// Called prior to an object is loading
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static EventHandler<LoadingEventArgs> Loading;

        /// <summary>
        /// Called when the item is Loading
        /// </summary>
        /// <param name="e">LoadingEventArgs item</param>
        protected virtual void OnLoading(LoadingEventArgs e)
        {
            Loading.Raise(this, e);
        }

        /// <summary>
        /// Called when the item is Loading
        /// </summary>
        /// <param name="e">LoadingEventArgs item</param>
        /// <param name="sender">Sender item</param>
        protected static void OnLoading(object sender, LoadingEventArgs e)
        {
            Loading.Raise(sender, e);
        }

        /// <summary>
        /// Called prior to an object being loaded
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static EventHandler<LoadedEventArgs> Loaded;

        /// <summary>
        /// Called when the item is Loaded
        /// </summary>
        /// <param name="e">LoadedEventArgs item</param>
        protected virtual void OnLoaded(LoadedEventArgs e)
        {
            Loaded.Raise(this, e);
        }

        /// <summary>
        /// Called when the item is Loaded
        /// </summary>
        /// <param name="e">LoadedEventArgs item</param>
        /// <param name="sender">Sender item</param>
        protected static void OnLoaded(object sender, LoadedEventArgs e)
        {
            Loaded.Raise(sender, e);
        }

        #endregion

        #region IObject Members

        /// <summary>
        /// ID for the object
        /// </summary>
        public virtual IDType ID { get; set; }

        /// <summary>
        /// Date last modified
        /// </summary>
        public virtual DateTime DateModified { get; set; }

        /// <summary>
        /// Date object was created
        /// </summary>
        public virtual DateTime DateCreated { get; set; }

        /// <summary>
        /// Is the object active?
        /// </summary>
        public virtual bool Active { get; set; }

        #endregion

        #region IComparable Functions

        /// <summary>
        /// Compares the object to another object
        /// </summary>
        /// <param name="obj">Object to compare to</param>
        /// <returns>0 if they are equal, -1 if this is smaller, 1 if it is larger</returns>
        public int CompareTo(object obj)
        {
            if (obj is ObjectBaseClass<ObjectType, IDType>)
                return CompareTo((ObjectType)obj);
            return -1;
        }

        /// <summary>
        /// Compares the object to another object
        /// </summary>
        /// <param name="other">Object to compare to</param>
        /// <returns>0 if they are equal, -1 if this is smaller, 1 if it is larger</returns>
        public virtual int CompareTo(ObjectType other)
        {
            return other.ID.CompareTo(ID);
        }

        #endregion
    }
}