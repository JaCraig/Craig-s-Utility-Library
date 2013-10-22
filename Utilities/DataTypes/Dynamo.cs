/*
Copyright (c) 2013 <a href="http://www.gutgames.com">James Craig</a>

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
using System.ComponentModel;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Utilities.DataTypes;
using Utilities.DataTypes.DataMapper;
#endregion

namespace Utilities.DataTypes
{
    /// <summary>
    /// Dynamic object implementation (used when inheriting
    /// </summary>
    /// <typeparam name="T">Child object type</typeparam>
    public class Dynamo<T> : Dynamo
        where T : Dynamo<T>
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public Dynamo()
            : base()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Dictionary">Dictionary to copy</param>
        public Dynamo(IDictionary<string, object> Dictionary)
            : base(Dictionary)
        {
        }

        #endregion

        #region Functions

        /// <summary>
        /// Gets a value
        /// </summary>
        /// <param name="Name">Name of the item</param>
        /// <param name="ReturnType">Return value type</param>
        /// <returns>The returned value</returns>
        protected override object GetValue(string Name, Type ReturnType)
        {
            if (ContainsKey(Name))
                return InternalValues[Name].To(ReturnType, null);
            if (!ChildValues.ContainsKey(Name))
            {
                Type ObjectType = GetType();
                PropertyInfo Property = ObjectType.GetProperty(Name);
                if (Property != null)
                {
                    Func<T, object> Temp = Property.PropertyGetter<T>().Compile();
                    ChildValues.Add(Name, () => Temp((T)this));
                }
                else
                    ChildValues.Add(Name, () => null);
            }
            return ChildValues[Name]().To(ReturnType, null);
        }

        #endregion
    }

    /// <summary>
    /// Dynamic object implementation
    /// </summary>
    public class Dynamo : DynamicObject, IDictionary<string, object>, INotifyPropertyChanged
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public Dynamo()
            : base()
        {
            InternalValues = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
            ChildValues = new Dictionary<string, Func<object>>(StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Dictionary">Dictionary to copy</param>
        public Dynamo(IDictionary<string, object> Dictionary)
            : base()
        {
            InternalValues = new Dictionary<string, object>(Dictionary, StringComparer.OrdinalIgnoreCase);
            ChildValues = new Dictionary<string, Func<object>>(StringComparer.OrdinalIgnoreCase);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Internal key/value dictionary
        /// </summary>
        internal IDictionary<string, object> InternalValues { get; set; }

        /// <summary>
        /// Child class key/value dictionary
        /// </summary>
        internal IDictionary<string, Func<object>> ChildValues { get; set; }

        /// <summary>
        /// Keys
        /// </summary>
        public ICollection<string> Keys { get { return InternalValues.Keys; } }

        /// <summary>
        /// Values
        /// </summary>
        public ICollection<object> Values { get { return InternalValues.Values; } }

        /// <summary>
        /// Number of items
        /// </summary>
        public int Count { get { return InternalValues.Count; } }

        /// <summary>
        /// Is this read only?
        /// </summary>
        public bool IsReadOnly { get { return InternalValues.IsReadOnly; } }

        /// <summary>
        /// Gets the value associated with the key specified
        /// </summary>
        /// <param name="key">Key to get</param>
        /// <returns>The object associated with the key</returns>
        public object this[string key]
        {
            get
            {
                return GetValue(key, typeof(object));
            }
            set
            {
                SetValue(key, value);
            }
        }

        #endregion

        #region Functions

        /// <summary>
        /// Adds a key/value pair to the object
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">value</param>
        public void Add(string key, object value)
        {
            SetValue(key, value);
        }

        /// <summary>
        /// Determines if the object contains a key
        /// </summary>
        /// <param name="key">Key to check</param>
        /// <returns>True if it is found, false otherwise</returns>
        public bool ContainsKey(string key)
        {
            return InternalValues.ContainsKey(key);
        }

        /// <summary>
        /// Removes the value associated with the key
        /// </summary>
        /// <param name="key">Key to remove</param>
        /// <returns>True if it is removed, false otherwise</returns>
        public bool Remove(string key)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(key));
            return InternalValues.Remove(key);
        }

        /// <summary>
        /// Attempts to get a value
        /// </summary>
        /// <param name="key">Key to get</param>
        /// <param name="value">Value object</param>
        /// <returns>True if it the key is found, false otherwise</returns>
        public bool TryGetValue(string key, out object value)
        {
            return InternalValues.TryGetValue(key, out value);
        }

        /// <summary>
        /// Adds a key/value pair
        /// </summary>
        /// <param name="item">Item to add</param>
        public void Add(KeyValuePair<string, object> item)
        {
            SetValue(item.Key, item.Value);
        }

        /// <summary>
        /// Clears the key/value pairs
        /// </summary>
        public void Clear()
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(""));
            InternalValues.Clear();
        }

        /// <summary>
        /// Does the object contain the key/value pair
        /// </summary>
        /// <param name="item">Item to check</param>
        /// <returns>True if it is found, false otherwise</returns>
        public bool Contains(KeyValuePair<string, object> item)
        {
            return InternalValues.Contains(item);
        }

        /// <summary>
        /// Copies the key/value pairs to an array
        /// </summary>
        /// <param name="array">Array to copy to</param>
        /// <param name="arrayIndex">Array index</param>
        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            InternalValues.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Removes a key/value pair
        /// </summary>
        /// <param name="item">Item to remove</param>
        /// <returns>True if it is removed, false otherwise</returns>
        public bool Remove(KeyValuePair<string, object> item)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(item.Key));
            return InternalValues.Remove(item);
        }

        /// <summary>
        /// Gets the enumerator for the object
        /// </summary>
        /// <returns>The enumerator</returns>
        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return InternalValues.GetEnumerator();
        }

        /// <summary>
        /// Gets the enumerator for the object
        /// </summary>
        /// <returns>The enumerator</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return InternalValues.GetEnumerator();
        }

        /// <summary>
        /// Gets the dynamic member names
        /// </summary>
        /// <returns>The keys used internally</returns>
        public override IEnumerable<string> GetDynamicMemberNames()
        {
            return Keys;
        }

        /// <summary>
        /// Attempts to get a member
        /// </summary>
        /// <param name="binder">GetMemberBinder object</param>
        /// <param name="result">Result</param>
        /// <returns>True if it gets the member, false otherwise</returns>
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = GetValue(binder.Name, binder.ReturnType);
            return true;
        }

        /// <summary>
        /// Attempts to invoke a function
        /// </summary>
        /// <param name="binder">Invoke binder</param>
        /// <param name="args">Function args</param>
        /// <param name="result">Result</param>
        /// <returns>True if it invokes, false otherwise</returns>
        public override bool TryInvoke(InvokeBinder binder, object[] args, out object result)
        {
            return base.TryInvoke(binder, args, out result);
        }

        /// <summary>
        /// Attempts to invoke a member
        /// </summary>
        /// <param name="binder">Invoke binder</param>
        /// <param name="args">Function args</param>
        /// <param name="result">Result</param>
        /// <returns>True if it invokes, false otherwise</returns>
        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            return base.TryInvokeMember(binder, args, out result);
        }

        /// <summary>
        /// Attempts to set the member
        /// </summary>
        /// <param name="binder">Member binder</param>
        /// <param name="value">Value</param>
        /// <returns>True if it is set, false otherwise</returns>
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            SetValue(binder.Name, value);
            return true;
        }

        /// <summary>
        /// Attempts to convert the object
        /// </summary>
        /// <param name="binder">Convert binder</param>
        /// <param name="result">Result</param>
        /// <returns>True if it is converted, false otherwise</returns>
        public override bool TryConvert(ConvertBinder binder, out object result)
        {
            result = Activator.CreateInstance(binder.Type);
            IoC.Manager.Bootstrapper.Resolve<Manager>().Map(this.GetType(), binder.Type)
                .AutoMap()
                .Copy(this, result);
            return true;
        }

        /// <summary>
        /// Gets a value
        /// </summary>
        /// <param name="Name">Name of the item</param>
        /// <param name="ReturnType">Return value type</param>
        /// <returns>The returned value</returns>
        protected virtual object GetValue(string Name, Type ReturnType)
        {
            if (ContainsKey(Name))
                return InternalValues[Name].To(ReturnType, null);
            if (!ChildValues.ContainsKey(Name))
            {
                Type ObjectType = GetType();
                PropertyInfo Property = ObjectType.GetProperty(Name);
                if (Property != null)
                {
                    Func<Dynamo, object> Temp = Property.PropertyGetter<Dynamo>().Compile();
                    ChildValues.Add(Name, () => Temp(this));
                }
                else
                    ChildValues.Add(Name, () => null);
            }
            return ChildValues[Name]().To(ReturnType, null);
        }

        /// <summary>
        /// Sets a value
        /// </summary>
        /// <param name="key">Name of the item</param>
        /// <param name="value">Value to set</param>
        protected virtual void SetValue(string key, object value)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(key));
            if (InternalValues.ContainsKey(key))
                InternalValues[key] = value;
            else
                InternalValues.Add(key, value);
        }

        #endregion

        #region Events

        /// <summary>
        /// Property changed event
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}