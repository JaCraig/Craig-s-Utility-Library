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
using Utilities.ORM.Interfaces;
using Utilities.Events.EventArgs;
using Utilities.ORM.QueryProviders;
using Utilities.ORM.QueryProviders.Interfaces;
#endregion

namespace Utilities.ORM
{
    /// <summary>
    /// Object base class helper. This is not required but automatically
    /// sets up basic functions and properties to simplify things a bit.
    /// </summary>
    public class ObjectBaseClass<ObjectType, IDType> : IComparable, IComparable<ObjectType>, IObject<IDType>
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
        /// Loads the items based on type
        /// </summary>
        /// <param name="Params">Parameters used to specify what to load</param>
        /// <returns>All items that fit the specified query</returns>
        public static IEnumerable<ObjectType> All(params IParameter[] Params)
        {
            return All(ORM.CreateSession(), Params);
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
                instance.OnLoaded(new LoadedEventArgs());
            }
            return instance;
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
        /// Saves a list of objects
        /// </summary>
        /// <param name="Objects">List of objects</param>
        public static void Save(List<ObjectType> Objects)
        {
            ObjectBaseClass<ObjectType, IDType>.Save(Objects, ORM.CreateSession());
        }

        /// <summary>
        /// Saves a list of objects
        /// </summary>
        /// <param name="Objects">List of objects</param>
        /// <param name="Session">ORM session variable</param>
        public static void Save(List<ObjectType> Objects, Session Session)
        {
            foreach (ObjectType Object in Objects)
            {
                Object.SetupObject();
                Validation.ValidationManager.Validate(Object);
                Object.Save(Session);
            }
        }

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
                Validation.ValidationManager.Validate((ObjectType)this);
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
        public static EventHandler<SavedEventArgs> Saved;

        /// <summary>
        /// Called when the item is Saved
        /// </summary>
        /// <param name="e">SavedEventArgs item</param>
        protected virtual void OnSaved(SavedEventArgs e)
        {
            Utilities.Events.EventHelper.Raise<SavedEventArgs>(Saved, this, e);
        }

        /// <summary>
        /// Called when the object is deleted
        /// </summary>
        public static EventHandler<DeletedEventArgs> Deleted;

        /// <summary>
        /// Called when the item is Deleted
        /// </summary>
        /// <param name="e">DeletedEventArgs item</param>
        protected virtual void OnDeleted(DeletedEventArgs e)
        {
            Utilities.Events.EventHelper.Raise<DeletedEventArgs>(Deleted, this, e);
        }

        /// <summary>
        /// Called prior to an object is saving
        /// </summary>
        public static EventHandler<SavingEventArgs> Saving;

        /// <summary>
        /// Called when the item is Saving
        /// </summary>
        /// <param name="e">SavingEventArgs item</param>
        protected virtual void OnSaving(SavingEventArgs e)
        {
            Utilities.Events.EventHelper.Raise<SavingEventArgs>(Saving, this, e);
        }

        /// <summary>
        /// Called prior to an object is deleting
        /// </summary>
        public static EventHandler<DeletingEventArgs> Deleting;

        /// <summary>
        /// Called when the item is Deleting
        /// </summary>
        /// <param name="e">DeletingEventArgs item</param>
        protected virtual void OnDeleting(DeletingEventArgs e)
        {
            Utilities.Events.EventHelper.Raise<DeletingEventArgs>(Deleting, this, e);
        }

        /// <summary>
        /// Called prior to an object is loading
        /// </summary>
        public static EventHandler<LoadingEventArgs> Loading;

        /// <summary>
        /// Called when the item is Loading
        /// </summary>
        /// <param name="e">LoadingEventArgs item</param>
        protected virtual void OnLoading(LoadingEventArgs e)
        {
            Utilities.Events.EventHelper.Raise<LoadingEventArgs>(Loading, this, e);
        }

        /// <summary>
        /// Called when the item is Loading
        /// </summary>
        /// <param name="e">LoadingEventArgs item</param>
        protected static void OnLoading(object sender, LoadingEventArgs e)
        {
            Utilities.Events.EventHelper.Raise<LoadingEventArgs>(Loading, sender, e);
        }

        /// <summary>
        /// Called prior to an object being loaded
        /// </summary>
        public static EventHandler<LoadedEventArgs> Loaded;

        /// <summary>
        /// Called when the item is Loaded
        /// </summary>
        /// <param name="e">LoadedEventArgs item</param>
        protected virtual void OnLoaded(LoadedEventArgs e)
        {
            Utilities.Events.EventHelper.Raise<LoadedEventArgs>(Loaded, this, e);
        }

        /// <summary>
        /// Called when the item is Loaded
        /// </summary>
        /// <param name="e">LoadedEventArgs item</param>
        protected static void OnLoaded(object sender, LoadedEventArgs e)
        {
            Utilities.Events.EventHelper.Raise<LoadedEventArgs>(Loaded, sender, e);
        }

        #endregion

        #region IObject Members

        public virtual IDType ID { get; set; }
        public virtual DateTime DateModified { get; set; }
        public virtual DateTime DateCreated { get; set; }
        public virtual bool Active { get; set; }

        #endregion

        #region IComparable Functions

        public int CompareTo(object obj)
        {
            if (obj is ObjectBaseClass<ObjectType, IDType>)
                return CompareTo((ObjectType)obj);
            return -1;
        }

        public virtual int CompareTo(ObjectType other)
        {
            return other.ID.CompareTo(ID);
        }

        #endregion
    }
}