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

using Ironman.Core.API.Manager.Interfaces;
using Ironman.Core.API.Manager.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Utilities.DataTypes;
using Utilities.ORM;
using Utilities.Web;

namespace Ironman.Core.API.Manager.BaseClasses
{
    /// <summary>
    /// API mapping base class
    /// </summary>
    /// <typeparam name="ClassType">Class type to map</typeparam>
    /// <typeparam name="IDType">ID type</typeparam>
    public abstract class APIMappingBaseClass<ClassType, IDType> : IAPIMapping<ClassType>, IAPIMapping
        where ClassType : class,new()
        where IDType : IComparable
    {
        /// <summary>
        /// Constructor
        /// </summary>
        protected APIMappingBaseClass(params int[] Versions)
            : base()
        {
            ObjectType = typeof(ClassType);
            Name = ObjectType.Name;
            Properties = new List<IAPIProperty>();
            SetCanDelete(x => true);
            SetCanGet(x => true);
            SetCanSave(x => true);
            SetAll(() => QueryProvider.All<ClassType>());
            SetAny(x => QueryProvider.Any<ClassType, IDType>(x.To(default(IDType))));
            SetDelete(x => { QueryProvider.Delete(x); return true; });
            SetSave(x => { QueryProvider.Save<ClassType, IDType>(x); return true; });
            this.Versions = Versions.Check(x => x.Length > 0, new int[] { 1 }).ToList();
        }

        /// <summary>
        /// Object type name
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Object type
        /// </summary>
        public Type ObjectType { get; private set; }

        /// <summary>
        /// Properties in the mapping
        /// </summary>
        public ICollection<IAPIProperty> Properties { get; private set; }

        /// <summary>
        /// List of versions this API mapping is used for
        /// </summary>
        public IEnumerable<int> Versions { get; private set; }

        private static Manager APIManager { get { return Utilities.IoC.Manager.Bootstrapper.Resolve<Manager>(); } }

        private Func<IEnumerable<ClassType>> AllFunc { get; set; }

        private Func<string, ClassType> AnyFunc { get; set; }

        private Func<ClassType, bool> CanDeleteFunc { get; set; }

        private Func<ClassType, bool> CanGetFunc { get; set; }

        private Func<ClassType, bool> CanSaveFunc { get; set; }

        private Func<ClassType, bool> DeleteFunc { get; set; }

        private Func<ClassType, bool> SaveFunc { get; set; }

        /// <summary>
        /// Gets all items of the mapped type
        /// </summary>
        /// <param name="EmbeddedProperties">Properties to embed</param>
        /// <param name="Mappings">Mappings</param>
        /// <returns>All items of the mapped type</returns>
        public IEnumerable<Dynamo> All(MappingHolder Mappings, params string[] EmbeddedProperties)
        {
            IEnumerable<ClassType> Objects = AllFunc();
            List<Dynamo> ReturnValue = new List<Dynamo>();
            foreach (ClassType Object in Objects)
            {
                if (CanGetFunc(Object))
                {
                    Dynamo TempItem = new Dynamo(Object);
                    Dynamo ReturnItem = TempItem.SubSet(Properties.Where(x => x is IReference || x is IID)
                                                                   .Select(x => x.Name)
                                                                   .ToArray());
                    string AbsoluteUri = HttpContext.Current != null ? HttpContext.Current.Request.Url.AbsoluteUri.Left(HttpContext.Current.Request.Url.AbsoluteUri.IndexOf('?')) : "";
                    if (!AbsoluteUri.EndsWith("/"))
                        AbsoluteUri += "/";
                    AbsoluteUri += Properties.FirstOrDefault(x => x is IID).GetValue(Mappings, Object).ToString() + "/";
                    foreach (IAPIProperty Property in Properties.Where(x => x is IMap))
                    {
                        ReturnItem[Property.Name] = EmbeddedProperties.Contains(Property.Name) ?
                                                        (object)Property.GetValue(Mappings, TempItem) :
                                                        (object)(AbsoluteUri + Property.Name);
                    }
                    ReturnValue.Add(ReturnItem);
                }
            }
            return ReturnValue;
        }

        /// <summary>
        /// Gets the item specified of the mapped type
        /// </summary>
        /// <param name="ID">ID of the item</param>
        /// <param name="Mappings">Mappings</param>
        /// <param name="EmbeddedProperties">Properties to embed</param>
        /// <returns>The item specified</returns>
        public Dynamo Any(string ID, MappingHolder Mappings, params string[] EmbeddedProperties)
        {
            if (string.IsNullOrEmpty(ID))
                throw new ArgumentNullException("ID");
            ClassType Object = AnyFunc(ID);
            if (!CanGetFunc(Object))
                return null;
            Dynamo TempItem = new Dynamo(Object);
            Dynamo ReturnValue = TempItem.SubSet(Properties.Where(x => x is IReference || x is IID)
                                                           .Select(x => x.Name)
                                                           .ToArray());
            string AbsoluteUri = HttpContext.Current != null ? HttpContext.Current.Request.Url.AbsoluteUri.Left(HttpContext.Current.Request.Url.AbsoluteUri.IndexOf('?')) : "";
            if (!AbsoluteUri.EndsWith("/"))
                AbsoluteUri += "/";
            foreach (IAPIProperty Property in Properties.Where(x => x is IMap))
            {
                ReturnValue[Property.Name] = EmbeddedProperties.Contains(Property.Name) ?
                                                (object)Property.GetValue(Mappings, TempItem) :
                                                (object)(AbsoluteUri + Property.Name);
            }
            return ReturnValue;
        }

        /// <summary>
        /// Used to determine if an object can be viewed
        /// </summary>
        /// <param name="Value">Value</param>
        /// <returns>True if it can, false otherwise</returns>
        public bool CanDelete(ClassType Value)
        {
            return CanDeleteFunc(Value);
        }

        /// <summary>
        /// Used to determine if an object can be viewed
        /// </summary>
        /// <param name="Value">Value</param>
        /// <returns>True if it can, false otherwise</returns>
        public bool CanGet(ClassType Value)
        {
            return CanGetFunc(Value);
        }

        /// <summary>
        /// Used to determine if an object can be saved
        /// </summary>
        /// <param name="Value">Value</param>
        /// <returns>True if it can, false otherwise</returns>
        public bool CanSave(ClassType Value)
        {
            return CanSaveFunc(Value);
        }

        /// <summary>
        /// Deletes the specified object
        /// </summary>
        /// <param name="ID">ID of the object to delete</param>
        /// <returns>True if it is deleted, false otherwise</returns>
        public bool Delete(string ID)
        {
            if (string.IsNullOrEmpty(ID))
                throw new ArgumentNullException("ID");
            ClassType DeleteValue = AnyFunc(ID);
            if (CanDeleteFunc(DeleteValue))
                return DeleteFunc(DeleteValue);
            return false;
        }

        /// <summary>
        /// Gets a specific property from an object
        /// </summary>
        /// <param name="ID">ID of the item</param>
        /// <param name="EmbeddedProperties">Properties to embed</param>
        /// <param name="Mappings">Mappings</param>
        /// <param name="Property">Property name</param>
        /// <returns>The property specified</returns>
        public dynamic GetProperty(string ID, MappingHolder Mappings, string Property, params string[] EmbeddedProperties)
        {
            if (string.IsNullOrEmpty(ID))
                throw new ArgumentNullException("ID");
            IAPIProperty PropertyObject = Properties.FirstOrDefault(x => string.Equals(x.Name, Property, StringComparison.InvariantCultureIgnoreCase));
            if (PropertyObject == null)
                return null;
            ClassType Object = AnyFunc(ID);
            if (!CanGetFunc(Object))
                return null;
            return PropertyObject.GetValue(Mappings, Object);
        }

        /// <summary>
        /// Sets an ID property for the mapping
        /// </summary>
        /// <typeparam name="DataType">Data type</typeparam>
        /// <param name="Expression">Expression pointing to the property</param>
        /// <returns>ID property object</returns>
        public IAPIProperty<ClassType, DataType> ID<DataType>(Expression<Func<ClassType, DataType>> Expression)
        {
            return (IAPIProperty<ClassType, DataType>)Properties.AddAndReturn(new ID<ClassType, DataType>(Expression));
        }

        /// <summary>
        /// Sets an Map property for the mapping
        /// </summary>
        /// <typeparam name="DataType">Data type</typeparam>
        /// <param name="Expression">Expression pointing to the property</param>
        /// <returns>Map property object</returns>
        public IAPIProperty<ClassType, DataType> Map<DataType>(Expression<Func<ClassType, DataType>> Expression)
            where DataType : class,new()
        {
            return (IAPIProperty<ClassType, DataType>)Properties.AddAndReturn(new Map<ClassType, DataType>(Expression));
        }

        /// <summary>
        /// Sets an Map property that is an IEnumerable for the mapping
        /// </summary>
        /// <typeparam name="DataType">Data type</typeparam>
        /// <param name="Expression">Expression pointing to the property</param>
        /// <returns>List Map property object</returns>
        public IAPIProperty<ClassType, IEnumerable<DataType>> MapList<DataType>(Expression<Func<ClassType, IEnumerable<DataType>>> Expression)
            where DataType : class,new()
        {
            return (IAPIProperty<ClassType, IEnumerable<DataType>>)Properties.AddAndReturn(new MapList<ClassType, DataType>(Expression));
        }

        /// <summary>
        /// Sets a Reference property for the mapping
        /// </summary>
        /// <typeparam name="DataType">Data type</typeparam>
        /// <param name="Expression">Expression pointing to the property</param>
        /// <returns>Reference property object</returns>
        public IAPIProperty<ClassType, DataType> Reference<DataType>(Expression<Func<ClassType, DataType>> Expression)
        {
            return (IAPIProperty<ClassType, DataType>)Properties.AddAndReturn(new Reference<ClassType, DataType>(Expression));
        }

        /// <summary>
        /// Saves the specified object
        /// </summary>
        /// <param name="Object">Object to save</param>
        /// <returns>True if it is saved, false otherwise</returns>
        public bool Save(Dynamo Object)
        {
            if (Object == null)
                return false;
            IAPIProperty IDProperty = Properties.FirstOrDefault(x => x is IID);
            if (IDProperty == null)
                return false;
            object IDValue = Object[IDProperty.Name];
            ClassType Item = IDValue == null ? new ClassType() : AnyFunc(IDValue.ToString()).Check(new ClassType());
            if (!CanSaveFunc(Item))
                return false;
            Dynamo SubSet = Object.SubSet(Properties.Where(x => x is IReference)
                                                                   .Select(x => x.Name)
                                                                   .ToArray());
            SubSet.CopyTo(Item);
            return SaveFunc(Item);
        }

        /// <summary>
        /// Sets the function to get all items of the specified type
        /// </summary>
        /// <param name="Value">Function used to get all items of the specified type</param>
        /// <returns>This</returns>
        public IAPIMapping<ClassType> SetAll(Func<IEnumerable<ClassType>> Value)
        {
            AllFunc = Value;
            return this;
        }

        /// <summary>
        /// Sets the function to get a specific item of the specified type
        /// </summary>
        /// <param name="Value">Function used to get a specific item of the specified type</param>
        /// <returns>This</returns>
        public IAPIMapping<ClassType> SetAny(Func<string, ClassType> Value)
        {
            AnyFunc = Value;
            return this;
        }

        /// <summary>
        /// Sets a function that determines if the object can be deleted
        /// </summary>
        /// <param name="Value">
        /// Function to use to determine if an object can be deleted (takes the object and returns
        /// true if it can be deleted, false otherwise)
        /// </param>
        /// <returns>This</returns>
        public IAPIMapping<ClassType> SetCanDelete(Func<ClassType, bool> Value)
        {
            CanDeleteFunc = Value;
            return this;
        }

        /// <summary>
        /// Sets a function that determines if the object can be seen
        /// </summary>
        /// <param name="Value">
        /// Function to use to determine if an object can be seen (takes the object and returns true
        /// if it can be seen, false otherwise)
        /// </param>
        /// <returns>This</returns>
        public IAPIMapping<ClassType> SetCanGet(Func<ClassType, bool> Value)
        {
            CanGetFunc = Value;
            return this;
        }

        /// <summary>
        /// Sets a function that determines if the object can be saved
        /// </summary>
        /// <param name="Value">
        /// Function to use to determine if an object can be saved (takes the object and returns
        /// true if it can be saved, false otherwise)
        /// </param>
        /// <returns>This</returns>
        public IAPIMapping<ClassType> SetCanSave(Func<ClassType, bool> Value)
        {
            CanSaveFunc = Value;
            return this;
        }

        /// <summary>
        /// Sets the delete function that is used by the mapping
        /// </summary>
        /// <param name="Value">
        /// Function to use to delete an object (takes the object and returns true if it is deleted,
        /// false otherwise)
        /// </param>
        /// <returns>This</returns>
        public IAPIMapping<ClassType> SetDelete(Func<ClassType, bool> Value)
        {
            DeleteFunc = Value;
            return this;
        }

        /// <summary>
        /// Sets the save function that is used by the mapping
        /// </summary>
        /// <param name="Value">
        /// Function to use to save an object (takes the object and returns true if it is saved,
        /// false otherwise)
        /// </param>
        /// <returns>This</returns>
        public IAPIMapping<ClassType> SetSave(Func<ClassType, bool> Value)
        {
            SaveFunc = Value;
            return this;
        }
    }
}