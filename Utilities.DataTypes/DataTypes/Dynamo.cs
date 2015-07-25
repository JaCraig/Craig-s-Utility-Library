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
using System.Xml.Serialization;
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
        /// <param name="OriginalValue">Original value</param>
        /// <param name="NewValue">New value</param>
        public Change(object OriginalValue, object NewValue)
        {
            this.OriginalValue = OriginalValue;
            this.NewValue = NewValue;
        }

        /// <summary>
        /// New value
        /// </summary>
        public object NewValue { get; set; }

        /// <summary>
        /// Original value
        /// </summary>
        public object OriginalValue { get; set; }
    }

    /// <summary>
    /// Dynamic object implementation (used when inheriting)
    /// </summary>
    /// <typeparam name="T">Child object type</typeparam>
    [Serializable]
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
        /// <param name="Item">Item to copy values from</param>
        protected Dynamo(object Item)
            : base(Item)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Dictionary">Dictionary to copy</param>
        protected Dynamo(IDictionary<string, object> Dictionary)
            : base(Dictionary)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="info">Serialization info</param>
        /// <param name="context">Streaming context</param>
        protected Dynamo(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            Contract.Requires<ArgumentNullException>(info != null, "info");
        }

        /// <summary>
        /// Keys to the dynamic type
        /// </summary>
        public override ICollection<string> Keys
        {
            get
            {
                List<string> Temp = new List<string>();
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
                List<object> Temp = new List<object>();
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
                    ChildValues.AddOrUpdate(Name, x => () => Temp((T)this), (x, y) => () => Temp((T)this));
                }
                else
                    ChildValues.AddOrUpdate(Name, x => () => null, (x, y) => null);
            }
            return ChildValues[Name]().To(ReturnType, null);
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
    [Serializable]
    public class Dynamo : DynamicObject, IDictionary<string, object>, INotifyPropertyChanged, ISerializable, IXmlSerializable
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
        /// <param name="Item">Item to copy values from</param>
        public Dynamo(object Item)
            : base()
        {
            InternalValues = new ConcurrentDictionary<string, object>(StringComparer.OrdinalIgnoreCase);
            ChildValues = new ConcurrentDictionary<string, Func<object>>(StringComparer.OrdinalIgnoreCase);
            ChangeLog = new ConcurrentDictionary<string, Change>(StringComparer.OrdinalIgnoreCase);
            IDictionary<string, object> DictItem = Item as IDictionary<string, object>;
            if (Item == null)
                return;
            if (Item is string || Item.GetType().IsValueType)
                SetValue("Value", Item);
            else if (DictItem != null)
                InternalValues = new ConcurrentDictionary<string, object>(DictItem, StringComparer.OrdinalIgnoreCase);
            else if (Item is IEnumerable)
                SetValue("Items", Item);
            else
                DataMapper.Map(Item.GetType(), this.GetType())
                          .AutoMap()
                          .Copy(Item, this);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Dictionary">Dictionary to copy</param>
        public Dynamo(IDictionary<string, object> Dictionary)
            : base()
        {
            InternalValues = new ConcurrentDictionary<string, object>(Dictionary, StringComparer.OrdinalIgnoreCase);
            ChildValues = new ConcurrentDictionary<string, Func<object>>(StringComparer.OrdinalIgnoreCase);
            ChangeLog = new ConcurrentDictionary<string, Change>(StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="info">Serialization info</param>
        /// <param name="context">Streaming context</param>
        protected Dynamo(SerializationInfo info, StreamingContext context)
            : base()
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
        /// Change log
        /// </summary>
        public ConcurrentDictionary<string, Change> ChangeLog { get; private set; }

        /// <summary>
        /// Number of items
        /// </summary>
        public int Count { get { return InternalValues.Count; } }

        /// <summary>
        /// Is this read only?
        /// </summary>
        public bool IsReadOnly { get { return false; } }

        /// <summary>
        /// Keys
        /// </summary>
        public virtual ICollection<string> Keys { get { return InternalValues.Keys; } }

        /// <summary>
        /// Values
        /// </summary>
        public virtual ICollection<object> Values { get { return InternalValues.Values; } }

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
        private static AOP.Manager AOPManager { get { return IoC.Manager.Bootstrapper.Resolve<AOP.Manager>(); } }

        /// <summary>
        /// Gets or sets the data mapper.
        /// </summary>
        /// <value>The data mapper.</value>
        private static Manager DataMapper { get { return IoC.Manager.Bootstrapper.Resolve<Manager>(); } }

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
        /// <param name="Item">Item to copy from</param>
        public void Copy(object Item)
        {
            if (Item == null)
                return;
            IDictionary<string, object> DictItem = Item as IDictionary<string, object>;
            if (Item is string || Item.GetType().IsValueType)
                SetValue("Value", Item);
            else if (DictItem != null)
            {
                foreach (string Key in DictItem.Keys)
                {
                    InternalValues.AddOrUpdate(Key, x => DictItem[Key], (x, y) => DictItem[Key]);
                }
            }
            else if (Item is IEnumerable)
                SetValue("Items", Item);
            else
                DataMapper.Map(Item.GetType(), this.GetType())
                          .AutoMap()
                          .Copy(Item, this);
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
            DataMapper.Map(this.GetType(), result.GetType())
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
            Dynamo TempObj = obj as Dynamo;
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
        /// <param name="Keys">Property keys to return</param>
        /// <returns>A new Dynamo object containing only the keys specified</returns>
        public dynamic SubSet(params string[] Keys)
        {
            if (Keys == null)
                return new Dynamo();
            Dynamo ReturnValue = new Dynamo();
            ReturnValue.Clear();
            foreach (string Key in Keys)
            {
                ReturnValue.Add(Key, this[Key]);
            }
            return ReturnValue;
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
            DataMapper.Map(this.GetType(), ObjectType)
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
            StringBuilder Builder = new StringBuilder();
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
        /// <param name="Name">Name of the item</param>
        /// <param name="ReturnType">Return value type</param>
        /// <returns>The returned value</returns>
        protected virtual object GetValue(string Name, Type ReturnType)
        {
            object Value = RaiseGetValueStart(Name);
            if (Value != null)
                return Value;
            if (ContainsKey(Name))
            {
                if (InternalValues.TryGetValue(Name, out Value))
                    return Value.To(ReturnType, null);
            }
            if (!ChildValues.ContainsKey(Name))
            {
                Type ObjectType = GetType();
                PropertyInfo Property = ObjectType.GetProperty(Name);
                if (Property != null)
                {
                    Func<Dynamo, object> Temp = Property.PropertyGetter<Dynamo>().Compile();
                    ChildValues.AddOrUpdate(Name, x => () => Temp(this), (x, y) => () => Temp(this));
                }
                else
                    ChildValues.AddOrUpdate(Name, x => () => null, (x, y) => null);
            }
            object ReturnValue = ChildValues[Name]().To(ReturnType, null);
            Value = RaiseGetValueEnd(Name, ReturnValue);
            return (Value != null) ? Value : ReturnValue;
        }

        /// <summary>
        /// Raises the get value end event
        /// </summary>
        /// <param name="PropertyName">Property name</param>
        /// <param name="Value">Value initially being returned</param>
        /// <returns>
        /// Returns null if the function should continue, any other value should be immediately
        /// returned to the user
        /// </returns>
        protected object RaiseGetValueEnd(string PropertyName, object Value)
        {
            EventArgs.OnEndEventArgs End = new EventArgs.OnEndEventArgs() { Content = Value };
            var Handler = getValueEnd_;
            if (Handler != null)
                Handler(this, PropertyName, End);
            return End.Stop ? End.Content : null;
        }

        /// <summary>
        /// Raises the get value start event
        /// </summary>
        /// <param name="PropertyName">Property name</param>
        /// <returns>
        /// Returns null if the function should continue, any other value should be immediately
        /// returned to the user
        /// </returns>
        protected object RaiseGetValueStart(string PropertyName)
        {
            EventArgs.OnStartEventArgs Start = new EventArgs.OnStartEventArgs() { Content = PropertyName };
            var Handler = getValueStart_;
            if (Handler != null)
                Handler(this, Start);
            return Start.Stop ? Start.Content : null;
        }

        /// <summary>
        /// Raises the property changed event
        /// </summary>
        /// <param name="PropertyName">Property name</param>
        /// <param name="NewValue">New value for the property</param>
        protected void RaisePropertyChanged(string PropertyName, object NewValue)
        {
            Contract.Requires<NullReferenceException>(ChangeLog != null, "ChangeLog");
            if (ChangeLog.ContainsKey(PropertyName))
                ChangeLog.SetValue(PropertyName, new Change(this[PropertyName], NewValue));
            else
                ChangeLog.SetValue(PropertyName, new Change(NewValue, NewValue));
            var Handler = propertyChanged_;
            if (Handler != null)
                Handler(this, new PropertyChangedEventArgs(PropertyName));
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