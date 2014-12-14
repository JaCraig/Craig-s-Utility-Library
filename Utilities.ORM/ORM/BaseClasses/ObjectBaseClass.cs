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

using System;
using System.Collections.Generic;
using System.Data;
using Utilities.DataTypes;
using Utilities.DataTypes.EventArgs;
using Utilities.ORM.Interfaces;
using Utilities.ORM.Manager.QueryProvider.Interfaces;
using Utilities.Random.DefaultClasses;
using Utilities.Validation;

namespace Utilities.ORM
{
    /// <summary>
    /// Object base class helper. This is not required but automatically sets up basic functions and
    /// properties to simplify things a bit.
    /// </summary>
    /// <typeparam name="IDType">ID type</typeparam>
    /// <typeparam name="ObjectType">Object type (must be the child object type)</typeparam>
    public abstract class ObjectBaseClass<ObjectType, IDType> : IComparable, IComparable<ObjectType>, IObject<IDType>
        where ObjectType : ObjectBaseClass<ObjectType, IDType>, new()
        where IDType : IComparable
    {
        /// <summary>
        /// Constructor
        /// </summary>
        protected ObjectBaseClass()
        {
            this.Active = true;
            this.DateCreated = DateTime.Now;
            this.DateModified = DateTime.Now;
        }

        /// <summary>
        /// Called prior to an object is loading
        /// </summary>
        public static EventHandler<LoadingEventArgs> Loading;

        /// <summary>
        /// Is the object active?
        /// </summary>
        [BoolGenerator]
        public virtual bool Active { get; set; }

        /// <summary>
        /// Date object was created
        /// </summary>
        [Between("1/1/1900", "1/1/2100")]
        [DateTimeGenerator("1/1/1900", "1/1/2100")]
        public virtual DateTime DateCreated { get; set; }

        /// <summary>
        /// Date last modified
        /// </summary>
        [Between("1/1/1900", "1/1/2100")]
        [DateTimeGenerator("1/1/1900", "1/1/2100")]
        public virtual DateTime DateModified { get; set; }

        /// <summary>
        /// Called when the object is deleted
        /// </summary>
        public EventHandler<DeletedEventArgs> Deleted { get; set; }

        /// <summary>
        /// Called prior to an object is deleting
        /// </summary>
        public EventHandler<DeletingEventArgs> Deleting { get; set; }

        /// <summary>
        /// ID for the object
        /// </summary>
        public virtual IDType ID { get; set; }

        /// <summary>
        /// Called prior to an object being loaded
        /// </summary>
        public EventHandler<LoadedEventArgs> Loaded { get; set; }

        /// <summary>
        /// Called when the object is saved
        /// </summary>
        public EventHandler<SavedEventArgs> Saved { get; set; }

        /// <summary>
        /// Called prior to an object is saving
        /// </summary>
        public EventHandler<SavingEventArgs> Saving { get; set; }

        /// <summary>
        /// Loads the items based on type
        /// </summary>
        /// <param name="Params">Parameters used to specify what to load</param>
        /// <returns>All items that fit the specified query</returns>
        public static IEnumerable<ObjectType> All(params IParameter[] Params)
        {
            IEnumerable<ObjectType> instance = new List<ObjectType>();
            LoadingEventArgs E = new LoadingEventArgs();
            ObjectBaseClass<ObjectType, IDType>.OnLoading(null, E);
            if (!E.Stop)
            {
                instance = QueryProvider.All<ObjectType>(Params);
                foreach (ObjectType Item in instance)
                {
                    Item.OnLoaded(new LoadedEventArgs());
                }
            }
            return instance;
        }

        /// <summary>
        /// Loads the items based on the criteria specified
        /// </summary>
        /// <param name="Command">Command to run</param>
        /// <param name="Type">Command type</param>
        /// <param name="ConnectionString">Connection string name</param>
        /// <param name="Params">Parameters used to specify what to load</param>
        /// <returns>The specified items</returns>
        public static IEnumerable<ObjectType> All(string Command, CommandType Type, string ConnectionString, params IParameter[] Params)
        {
            IEnumerable<ObjectType> instance = new List<ObjectType>();
            LoadingEventArgs E = new LoadingEventArgs();
            ObjectBaseClass<ObjectType, IDType>.OnLoading(null, E);
            if (!E.Stop)
            {
                instance = QueryProvider.All<ObjectType>(Command, Type, ConnectionString, Params);
                foreach (ObjectType Item in instance)
                {
                    Item.OnLoaded(new LoadedEventArgs());
                }
            }
            return instance;
        }

        /// <summary>
        /// Loads the items based on the criteria specified
        /// </summary>
        /// <param name="Command">Command to run</param>
        /// <param name="Type">Command type</param>
        /// <param name="ConnectionString">Connection string name</param>
        /// <param name="Params">Parameters used to specify what to load</param>
        /// <returns>The specified items</returns>
        public static IEnumerable<ObjectType> All(string Command, CommandType Type, string ConnectionString, params object[] Params)
        {
            IEnumerable<ObjectType> instance = new List<ObjectType>();
            LoadingEventArgs E = new LoadingEventArgs();
            ObjectBaseClass<ObjectType, IDType>.OnLoading(null, E);
            if (!E.Stop)
            {
                instance = QueryProvider.All<ObjectType>(Command, Type, ConnectionString, Params);
                foreach (ObjectType Item in instance)
                {
                    Item.OnLoaded(new LoadedEventArgs());
                }
            }
            return instance;
        }

        /// <summary>
        /// Loads the item based on the criteria specified
        /// </summary>
        /// <param name="Params">Parameters used to specify what to load</param>
        /// <returns>The specified item</returns>
        public static ObjectType Any(params IParameter[] Params)
        {
            ObjectType instance = new ObjectType();
            LoadingEventArgs E = new LoadingEventArgs();
            E.Content = Params;
            instance.OnLoading(E);
            if (!E.Stop)
            {
                instance = QueryProvider.Any<ObjectType>(Params);
                if (instance != null)
                    instance.OnLoaded(new LoadedEventArgs());
            }
            return instance;
        }

        /// <summary>
        /// Loads the item based on the criteria specified
        /// </summary>
        /// <param name="Command">Command to run</param>
        /// <param name="Type">Command type</param>
        /// <param name="ConnectionString">Connection string name</param>
        /// <param name="Params">Parameters used to specify what to load</param>
        /// <returns>The specified item</returns>
        public static ObjectType Any(string Command, CommandType Type, string ConnectionString, params IParameter[] Params)
        {
            ObjectType instance = new ObjectType();
            LoadingEventArgs E = new LoadingEventArgs();
            E.Content = Params;
            instance.OnLoading(E);
            if (!E.Stop)
            {
                instance = QueryProvider.Any<ObjectType>(Command, Type, ConnectionString, Params);
                if (instance != null)
                    instance.OnLoaded(new LoadedEventArgs());
            }
            return instance;
        }

        /// <summary>
        /// Loads the item based on the criteria specified
        /// </summary>
        /// <param name="Command">Command to run</param>
        /// <param name="Type">Command type</param>
        /// <param name="ConnectionString">Connection string name</param>
        /// <param name="Params">Parameters used to specify what to load</param>
        /// <returns>The specified item</returns>
        public static ObjectType Any(string Command, CommandType Type, string ConnectionString, params object[] Params)
        {
            ObjectType instance = new ObjectType();
            LoadingEventArgs E = new LoadingEventArgs();
            E.Content = Params;
            instance.OnLoading(E);
            if (!E.Stop)
            {
                instance = QueryProvider.Any<ObjectType>(Command, Type, ConnectionString, Params);
                if (instance != null)
                    instance.OnLoaded(new LoadedEventArgs());
            }
            return instance;
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
        /// Gets the page count based on page size
        /// </summary>
        /// <param name="PageSize">Page size</param>
        /// <param name="Params">Parameters used to specify what to load</param>
        /// <returns>All items that fit the specified query</returns>
        public static int PageCount(int PageSize = 25, params IParameter[] Params)
        {
            return QueryProvider.PageCount<ObjectType>(PageSize, Params);
        }

        /// <summary>
        /// Loads the items based on type
        /// </summary>
        /// <param name="PageSize">Page size</param>
        /// <param name="CurrentPage">Current page (0 based)</param>
        /// <param name="OrderBy">The order by portion of the query</param>
        /// <param name="Params">Parameters used to specify what to load</param>
        /// <returns>All items that fit the specified query</returns>
        public static IEnumerable<ObjectType> Paged(int PageSize = 25, int CurrentPage = 0, string OrderBy = "", params IParameter[] Params)
        {
            IEnumerable<ObjectType> instance = new List<ObjectType>();
            LoadingEventArgs E = new LoadingEventArgs();
            ObjectBaseClass<ObjectType, IDType>.OnLoading(null, E);
            if (!E.Stop)
            {
                instance = QueryProvider.Paged<ObjectType>(PageSize, CurrentPage, OrderBy, Params);
                foreach (ObjectType Item in instance)
                {
                    Item.OnLoaded(new LoadedEventArgs());
                }
            }
            return instance;
        }

        /// <summary>
        /// Saves a list of objects
        /// </summary>
        /// <param name="Objects">List of objects</param>
        public static void Save(IEnumerable<ObjectType> Objects)
        {
            if (Objects == null)
                return;
            Objects.ForEach(x => x.Save());
        }

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

        /// <summary>
        /// Deletes the item
        /// </summary>
        public virtual void Delete()
        {
            DeletingEventArgs E = new DeletingEventArgs();
            OnDeleting(E);
            if (!E.Stop)
            {
                QueryProvider.Delete<ObjectType>((ObjectType)this);
                DeletedEventArgs X = new DeletedEventArgs();
                OnDeleted(X);
            }
        }

        /// <summary>
        /// Determines if two items are equal
        /// </summary>
        /// <param name="obj">The object to compare this to</param>
        /// <returns>true if they are the same, false otherwise</returns>
        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != this.GetType())
                return false;
            return obj.GetHashCode() == this.GetHashCode();
        }

        /// <summary>
        /// Returns the hash of this item
        /// </summary>
        /// <returns>the int hash of the item</returns>
        public override int GetHashCode()
        {
            return this.ID.GetHashCode();
        }

        /// <summary>
        /// Saves the item (if it already exists, it updates the item. Otherwise it inserts the item)
        /// </summary>
        public virtual void Save()
        {
            SavingEventArgs E = new SavingEventArgs();
            OnSaving(E);

            if (!E.Stop)
            {
                SetupObject();
                this.Validate();
                QueryProvider.Save<ObjectType, IDType>((ObjectType)this);
                SavedEventArgs X = new SavedEventArgs();
                OnSaved(X);
            }
        }

        /// <summary>
        /// Sets up the object for saving purposes
        /// </summary>
        public virtual void SetupObject()
        {
            DateModified = DateTime.Now;
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
        /// Called when the item is Deleted
        /// </summary>
        /// <param name="e">DeletedEventArgs item</param>
        protected virtual void OnDeleted(DeletedEventArgs e)
        {
            Deleted.Raise(this, e);
        }

        /// <summary>
        /// Called when the item is Deleting
        /// </summary>
        /// <param name="e">DeletingEventArgs item</param>
        protected virtual void OnDeleting(DeletingEventArgs e)
        {
            Deleting.Raise(this, e);
        }

        /// <summary>
        /// Called when the item is Loaded
        /// </summary>
        /// <param name="e">LoadedEventArgs item</param>
        protected virtual void OnLoaded(LoadedEventArgs e)
        {
            Loaded.Raise(this, e);
        }

        /// <summary>
        /// Called when the item is Loading
        /// </summary>
        /// <param name="e">LoadingEventArgs item</param>
        protected virtual void OnLoading(LoadingEventArgs e)
        {
            Loading.Raise(this, e);
        }

        /// <summary>
        /// Called when the item is Saved
        /// </summary>
        /// <param name="e">SavedEventArgs item</param>
        protected virtual void OnSaved(SavedEventArgs e)
        {
            Saved.Raise(this, e);
        }

        /// <summary>
        /// Called when the item is Saving
        /// </summary>
        /// <param name="e">SavingEventArgs item</param>
        protected virtual void OnSaving(SavingEventArgs e)
        {
            Saving.Raise(this, e);
        }
    }
}