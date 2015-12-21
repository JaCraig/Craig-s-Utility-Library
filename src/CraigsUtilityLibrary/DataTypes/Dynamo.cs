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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security;
using System.Text;
using Utilities.DataTypes.DataMapper;

namespace Utilities.DataTypes
{
    /// <summary>
    /// Change class
    /// </summary>
    public class Change
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="originalValue">Original value</param>
        /// <param name="newValue">New value</param>
        public Change(object originalValue, object newValue)
        {
            OriginalValue = originalValue;
            NewValue = newValue;
        }

        /// <summary>
        /// New value
        /// </summary>
        public object NewValue { get; private set; }

        /// <summary>
        /// Original value
        /// </summary>
        public object OriginalValue { get; private set; }
    }

    /// <summary>
    /// Dynamic object implementation (used when inheriting)
    /// </summary>
    /// <typeparam name="T">Child object type</typeparam>
    public abstract class Dynamo<T> : Dynamo
        where T : Dynamo<T>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        protected Dynamo()
            : this(new Dictionary<string, object>())
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="item">Item to copy values from</param>
        protected Dynamo(object item)
            : base(item)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dictionary">Dictionary to copy</param>
        protected Dynamo(IDictionary<string, object> dictionary)
            : base(dictionary)
        {
        }

        /// <summary>
        /// Keys to the dynamic type
        /// </summary>
        public override ICollection<string> Keys
        {
            get
            {
                var Temp = new List<string>();
                Temp.Add(base.Keys);
                Type ObjectType = GetType();
                foreach (PropertyInfo Property in ObjectType.GetProperties().Where(x => x.DeclaringType != typeof(Dynamo<T>) && x.DeclaringType != typeof(Dynamo)))
                {
                    Temp.Add(Property.Name);
                }
                return Temp;
            }
        }

        /// <summary>
        /// Gets the Values
        /// </summary>
        public override ICollection<object> Values
        {
            get
            {
                var Temp = new List<object>();
                foreach (string Key in Keys)
                {
                    Temp.Add(GetValue(Key, typeof(object)));
                }
                return Temp;
            }
        }

        /// <summary>
        /// Gets a value
        /// </summary>
        /// <param name="name">Name of the item</param>
        /// <param name="returnType">Return value type</param>
        /// <returns>The returned value</returns>
        protected override object GetValue(string name, Type returnType)
        {
            if (ContainsKey(name))
                return InternalValues[name].To(returnType, null);
            if (!ChildValues.ContainsKey(name))
            {
                Type ObjectType = GetType();
                PropertyInfo Property = ObjectType.GetProperty(name);
                if (Property != null)
                {
                    Func<T, object> Temp = Property.PropertyGetter<T>().Compile();
                    ChildValues.AddOrUpdate(name, x => () => Temp((T)this), (x, y) => () => Temp((T)this));
                }
                else
                    ChildValues.AddOrUpdate(name, x => () => null, (x, y) => null);
            }
            return ChildValues[name]().To(returnType, null);
        }

        /// <summary>
        /// Sets a value
        /// </summary>
        /// <param name="key">Name of the item</param>
        /// <param name="value">Value associated with the key</param>
        protected override void SetValue(string key, object value)
        {
            Type ObjectType = GetType();
            PropertyInfo Property = ObjectType.GetProperty(key);
            if (Property != null && Property.CanWrite)
            {
                RaisePropertyChanged(key, value);
                Property.SetValue(this, value);
            }
            else if (Property == null)
                base.SetValue(key, value);
        }
    }

    /// <summary>
    /// Dynamic object implementation
    /// </summary>
    public class Dynamo : DynamicObject, IDictionary<string, object>, INotifyPropertyChanged
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Dynamo()
            : this((object)null)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="item">Item to copy values from</param>
        public Dynamo(object item)
        {
            InternalValues = new ConcurrentDictionary<string, object>(StringComparer.OrdinalIgnoreCase);
            ChildValues = new ConcurrentDictionary<string, Func<object>>(StringComparer.OrdinalIgnoreCase);
            ChangeLog = new ConcurrentDictionary<string, Change>(StringComparer.OrdinalIgnoreCase);
            var DictItem = item as IDictionary<string, object>;
            if (item == null)
                return;
            if (item is string || item.GetType().IsValueType)
                SetValue("Value", item);
            else if (DictItem != null)
                InternalValues = new ConcurrentDictionary<string, object>(DictItem, StringComparer.OrdinalIgnoreCase);
            else if (item is IEnumerable)
                SetValue("Items", item);
            else
                DataMapper.Map(item.GetType(), GetType())
                          .AutoMap()
                          .Copy(item, this);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dictionary">Dictionary to copy</param>
        public Dynamo(IDictionary<string, object> dictionary)
        {
            InternalValues = new ConcurrentDictionary<string, object>(dictionary, StringComparer.OrdinalIgnoreCase);
            ChildValues = new ConcurrentDictionary<string, Func<object>>(StringComparer.OrdinalIgnoreCase);
            ChangeLog = new ConcurrentDictionary<string, Change>(StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="info">Serialization info</param>
        /// <param name="context">Streaming context</param>
        protected Dynamo(SerializationInfo info, StreamingContext context)
        {
            Contract.Requires<ArgumentNullException>(info != null, "info");
            InternalValues = new ConcurrentDictionary<string, object>(StringComparer.OrdinalIgnoreCase);
            ChildValues = new ConcurrentDictionary<string, Func<object>>(StringComparer.OrdinalIgnoreCase);
            ChangeLog = new ConcurrentDictionary<string, Change>(StringComparer.OrdinalIgnoreCase);
            foreach (SerializationEntry Item in info)
            {
                SetValue(Item.Name, Item.Value);
            }
        }

        /// <summary>
        /// Change log
        /// </summary>
        public ConcurrentDictionary<string, Change> ChangeLog { get; private set; }

        /// <summary>
        /// Number of items
        /// </summary>
        public int Count => InternalValues.Count;

        /// <summary>
        /// Is this read only?
        /// </summary>
        public bool IsReadOnly => false;

        /// <summary>
        /// Keys
        /// </summary>
        public virtual ICollection<string> Keys => InternalValues.Keys;

        /// <summary>
        /// Values
        /// </summary>
        public virtual ICollection<object> Values => InternalValues.Values;

        /// <summary>
        /// Child class key/value dictionary
        /// </summary>
        internal ConcurrentDictionary<string, Func<object>> ChildValues { get; set; }

        /// <summary>
        /// Internal key/value dictionary
        /// </summary>
        internal ConcurrentDictionary<string, object> InternalValues { get; set; }

        /// <summary>
        /// Gets or sets the aop manager.
        /// </summary>
        /// <value>The aop manager.</value>
        private static AOP.Manager AOPManager => IoC.Manager.Bootstrapper.Resolve<AOP.Manager>();

        /// <summary>
        /// Gets or sets the data mapper.
        /// </summary>
        /// <value>The data mapper.</value>
        private static Manager DataMapper => IoC.Manager.Bootstrapper.Resolve<Manager>();

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

        /// <summary>
        /// The get value end_
        /// </summary>
        private Action<Dynamo, string, EventArgs.OnEndEventArgs> getValueEnd_;

        /// <summary>
        /// The get value start_
        /// </summary>
        private Action<Dynamo, EventArgs.OnStartEventArgs> getValueStart_;

        /// <summary>
        /// The property changed_
        /// </summary>
        private PropertyChangedEventHandler propertyChanged_;

        /// <summary>
        /// Called when the value/property is found but before it is returned to the caller Sends
        /// (this, PropertyName, EventArgs) to items attached to the event
        /// </summary>
        public event Action<Dynamo, string, EventArgs.OnEndEventArgs> GetValueEnd
        {
            add
            {
                getValueEnd_ -= value;
                getValueEnd_ += value;
            }
            remove
            {
                getValueEnd_ -= value;
            }
        }

        /// <summary>
        /// Called when beginning to get a value/property Sends (this, EventArgs) to items attached
        /// to the event
        /// </summary>
        public event Action<Dynamo, EventArgs.OnStartEventArgs> GetValueStart
        {
            add
            {
                getValueStart_ -= value;
                getValueStart_ += value;
            }
            remove
            {
                getValueStart_ -= value;
            }
        }

        /// <summary>
        /// Property changed event
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged
        {
            add
            {
                propertyChanged_ -= value;
                propertyChanged_ += value;
            }
            remove
            {
                propertyChanged_ -= value;
            }
        }

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
            RaisePropertyChanged("", null);
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
        /// Determines if the object contains a key
        /// </summary>
        /// <param name="key">Key to check</param>
        /// <returns>True if it is found, false otherwise</returns>
        public bool ContainsKey(string key)
        {
            return InternalValues.ContainsKey(key);
        }

        /// <summary>
        /// Copies the properties from an item
        /// </summary>
        /// <param name="item">Item to copy from</param>
        public void Copy(object item)
        {
            if (item == null)
                return;
            var DictItem = item as IDictionary<string, object>;
            if (item is string || item.GetType().IsValueType)
                SetValue("Value", item);
            else if (DictItem != null)
            {
                foreach (string Key in DictItem.Keys)
                {
                    InternalValues.AddOrUpdate(Key, x => DictItem[Key], (x, y) => DictItem[Key]);
                }
            }
            else if (item is IEnumerable)
                SetValue("Items", item);
            else
                DataMapper.Map(item.GetType(), GetType())
                          .AutoMap()
                          .Copy(item, this);
        }

        /// <summary>
        /// Copies the key/value pairs to an array
        /// </summary>
        /// <param name="array">Array to copy to</param>
        /// <param name="arrayIndex">Array index</param>
        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            InternalValues.ToArray().CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Copies data from here to another object
        /// </summary>
        /// <param name="result">Result</param>
        public void CopyTo(object result)
        {
            Contract.Requires<ArgumentNullException>(result != null, "result");
            DataMapper.Map(GetType(), result.GetType())
                      .AutoMap()
                      .Copy(this, result);
        }

        /// <summary>
        /// Determines if two objects are equal
        /// </summary>
        /// <param name="obj">Object to compare to</param>
        /// <returns>True if they're equal, false otherwise</returns>
        public override bool Equals(object obj)
        {
            var TempObj = obj as Dynamo;
            if (TempObj == null)
                return false;
            return TempObj.GetHashCode() == GetHashCode();
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
        /// Gets the enumerator for the object
        /// </summary>
        /// <returns>The enumerator</returns>
        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            foreach (string Key in Keys)
            {
                yield return new KeyValuePair<string, object>(Key, this[Key]);
            }
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>The hash code</returns>
        public override int GetHashCode()
        {
            int Value = 1;
            foreach (string Key in Keys)
            {
                unchecked
                {
                    object TempValue = GetValue(Key, typeof(object));
                    if (TempValue != null && !TempValue.GetType().Is<Delegate>())
                    {
                        Value = (Value * TempValue.GetHashCode()) % int.MaxValue;
                    }
                }
            }
            return Value;
        }

        /// <summary>
        /// Gets the object data and serializes it
        /// </summary>
        /// <param name="info">Serialization info object</param>
        /// <param name="context">Streaming context object</param>
        [SecurityCritical]
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            foreach (string Key in Keys)
            {
                info.AddValue(Key, GetValue(Key, typeof(object)));
            }
        }

        /// <summary>
        /// Not used
        /// </summary>
        /// <returns>Null</returns>
        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        /// <summary>
        /// Gets the enumerator for the object
        /// </summary>
        /// <returns>The enumerator</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return InternalValues.GetEnumerator();
        }

        /// <summary>
        /// Reads the data from an XML doc
        /// </summary>
        /// <param name="reader">XML reader</param>
        public virtual void ReadXml(System.Xml.XmlReader reader)
        {
            SetValue(reader.Name, reader.Value);
            while (reader.Read())
            {
                SetValue(reader.Name, reader.Value);
            }
        }

        /// <summary>
        /// Removes the value associated with the key
        /// </summary>
        /// <param name="key">Key to remove</param>
        /// <returns>True if it is removed, false otherwise</returns>
        public bool Remove(string key)
        {
            RaisePropertyChanged(key, null);
            object TempObject = null;
            return InternalValues.TryRemove(key, out TempObject);
        }

        /// <summary>
        /// Removes a key/value pair
        /// </summary>
        /// <param name="item">Item to remove</param>
        /// <returns>True if it is removed, false otherwise</returns>
        public bool Remove(KeyValuePair<string, object> item)
        {
            RaisePropertyChanged(item.Key, null);
            object TempObject = null;
            return InternalValues.TryRemove(item.Key, out TempObject);
        }

        /// <summary>
        /// Returns a subset of the current Dynamo object
        /// </summary>
        /// <param name="keys">Property keys to return</param>
        /// <returns>A new Dynamo object containing only the keys specified</returns>
        public dynamic SubSet(params string[] keys)
        {
            if (keys == null)
                return new Dynamo();
            var ReturnValue = new Dynamo();
            ReturnValue.Clear();
            foreach (string Key in keys)
            {
                ReturnValue.Add(Key, this[Key]);
            }
            return ReturnValue;
        }

        /// <summary>
        /// Converts the object to the type specified
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <returns>The object converted to the type specified</returns>
        public T To<T>()
        {
            return (T)To(typeof(T));
        }

        /// <summary>
        /// Converts the object to the type specified
        /// </summary>
        /// <param name="ObjectType">Object type</param>
        /// <returns>The object converted to the type specified</returns>
        public object To(Type ObjectType)
        {
            object Result = AOPManager.Create(ObjectType);
            DataMapper.Map(GetType(), ObjectType)
                      .AutoMap()
                      .Copy(this, Result);
            return Result;
        }

        /// <summary>
        /// Outputs the object graph
        /// </summary>
        /// <returns>The string version of the object</returns>
        public override string ToString()
        {
            var Builder = new StringBuilder();
            Builder.AppendLineFormat("{0} this", GetType().Name);
            foreach (string Key in Keys.OrderBy(x => x))
            {
                object Item = GetValue(Key, typeof(object));
                if (Item != null)
                    Builder.AppendLineFormat("\t{0} {1} = {2}", Item.GetType().GetName(), Key, Item.ToString());
                else
                    Builder.AppendLineFormat("\t{0} {1} = {2}", "object", Key, "null");
            }
            return Builder.ToString();
        }

        /// <summary>
        /// Attempts to convert the object
        /// </summary>
        /// <param name="binder">Convert binder</param>
        /// <param name="result">Result</param>
        /// <returns>True if it is converted, false otherwise</returns>
        public override bool TryConvert(ConvertBinder binder, out object result)
        {
            result = To(binder.Type);
            return true;
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
        /// Writes the data to an XML doc
        /// </summary>
        /// <param name="writer">XML writer</param>
        public virtual void WriteXml(System.Xml.XmlWriter writer)
        {
            foreach (string Key in Keys)
            {
                writer.WriteElementString(Key, (string)GetValue(Key, typeof(string)));
            }
        }

        /// <summary>
        /// Gets a value
        /// </summary>
        /// <param name="name">Name of the item</param>
        /// <param name="returnType">Return value type</param>
        /// <returns>The returned value</returns>
        protected virtual object GetValue(string name, Type returnType)
        {
            object Value = RaiseGetValueStart(name);
            if (Value != null)
                return Value;
            if (ContainsKey(name))
            {
                if (InternalValues.TryGetValue(name, out Value))
                    return Value.To(returnType, null);
            }
            if (!ChildValues.ContainsKey(name))
            {
                Type ObjectType = GetType();
                PropertyInfo Property = ObjectType.GetProperty(name);
                if (Property != null)
                {
                    Func<Dynamo, object> Temp = Property.PropertyGetter<Dynamo>().Compile();
                    ChildValues.AddOrUpdate(name, x => () => Temp(this), (x, y) => () => Temp(this));
                }
                else
                    ChildValues.AddOrUpdate(name, x => () => null, (x, y) => null);
            }
            object ReturnValue = ChildValues[name]().To(returnType, null);
            Value = RaiseGetValueEnd(name, ReturnValue);
            return Value ?? ReturnValue;
        }

        /// <summary>
        /// Raises the get value end event
        /// </summary>
        /// <param name="propertyName">Property name</param>
        /// <param name="value">Value initially being returned</param>
        /// <returns>
        /// Returns null if the function should continue, any other value should be immediately
        /// returned to the user
        /// </returns>
        protected object RaiseGetValueEnd(string propertyName, object value)
        {
            var End = new EventArgs.OnEndEventArgs() { Content = value };
            var Handler = getValueEnd_;
            if (Handler != null)
                Handler(this, propertyName, End);
            return End.Stop ? End.Content : null;
        }

        /// <summary>
        /// Raises the get value start event
        /// </summary>
        /// <param name="propertyName">Property name</param>
        /// <returns>
        /// Returns null if the function should continue, any other value should be immediately
        /// returned to the user
        /// </returns>
        protected object RaiseGetValueStart(string propertyName)
        {
            var Start = new EventArgs.OnStartEventArgs() { Content = propertyName };
            var Handler = getValueStart_;
            if (Handler != null)
                Handler(this, Start);
            return Start.Stop ? Start.Content : null;
        }

        /// <summary>
        /// Raises the property changed event
        /// </summary>
        /// <param name="propertyName">Property name</param>
        /// <param name="newValue">New value for the property</param>
        protected void RaisePropertyChanged(string propertyName, object newValue)
        {
            Contract.Requires<NullReferenceException>(ChangeLog != null, "ChangeLog");
            if (ChangeLog.ContainsKey(propertyName))
                ChangeLog.SetValue(propertyName, new Change(this[propertyName], newValue));
            else
                ChangeLog.SetValue(propertyName, new Change(newValue, newValue));
            var Handler = propertyChanged_;
            if (Handler != null)
                Handler(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Sets a value
        /// </summary>
        /// <param name="key">Name of the item</param>
        /// <param name="value">Value to set</param>
        protected virtual void SetValue(string key, object value)
        {
            RaisePropertyChanged(key, value);
            if (InternalValues.ContainsKey(key))
                InternalValues[key] = value;
            else
                InternalValues.AddOrUpdate(key, value, (x, y) => value);
        }
    }
}