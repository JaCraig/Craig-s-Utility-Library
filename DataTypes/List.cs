/*
Copyright (c) 2010 <a href="http://www.gutgames.com">James Craig</a>

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
using Utilities.Events;
using Utilities.Events.EventArgs;
#endregion

namespace Utilities.DataTypes
{
    /// <summary>
    /// Class designed to replace List. Contains events so that we can tell
    /// when the list has been changed.
    /// </summary>
    public class List<T> : System.Collections.Generic.List<T>
    {
        #region Events
        public EventHandler<ChangedEventArgs> Changed;
        #endregion

        #region Public Functions

        public new void Add(T value)
        {
            base.Add(value);
            ChangedEventArgs TempArgs = new ChangedEventArgs();
            TempArgs.Content = PropertyName;
            EventHelper.Raise<ChangedEventArgs>(Changed, this, TempArgs);
        }

        public new void AddRange(System.Collections.Generic.IEnumerable<T> value)
        {
            base.AddRange(value);
            ChangedEventArgs TempArgs = new ChangedEventArgs();
            TempArgs.Content = PropertyName;
            EventHelper.Raise<ChangedEventArgs>(Changed, this, TempArgs);
        }

        public new bool Remove(T obj)
        {
            bool ReturnValue = base.Remove(obj);
            ChangedEventArgs TempArgs = new ChangedEventArgs();
            TempArgs.Content = PropertyName;
            EventHelper.Raise<ChangedEventArgs>(Changed, this, TempArgs);
            return ReturnValue;
        }

        public new void RemoveAt(int index)
        {
            base.RemoveAt(index);
            ChangedEventArgs TempArgs = new ChangedEventArgs();
            TempArgs.Content = PropertyName;
            EventHelper.Raise<ChangedEventArgs>(Changed, this, TempArgs);
        }

        public new int RemoveAll(Predicate<T> match)
        {
            int ReturnValue = base.RemoveAll(match);
            ChangedEventArgs TempArgs = new ChangedEventArgs();
            TempArgs.Content = PropertyName;
            EventHelper.Raise<ChangedEventArgs>(Changed, this, TempArgs);
            return ReturnValue;
        }

        public new void RemoveRange(int index, int count)
        {
            base.RemoveRange(index, count);
            ChangedEventArgs TempArgs = new ChangedEventArgs();
            TempArgs.Content = PropertyName;
            EventHelper.Raise<ChangedEventArgs>(Changed, this, TempArgs);
        }

        public new void Insert(int index, T value)
        {
            base.Insert(index, value);
            ChangedEventArgs TempArgs = new ChangedEventArgs();
            TempArgs.Content = PropertyName;
            EventHelper.Raise<ChangedEventArgs>(Changed, this, TempArgs);
        }

        public new void InsertRange(int index, System.Collections.Generic.IEnumerable<T> collection)
        {
            base.InsertRange(index, collection);
            ChangedEventArgs TempArgs = new ChangedEventArgs();
            TempArgs.Content = PropertyName;
            EventHelper.Raise<ChangedEventArgs>(Changed, this, TempArgs);
        }

        public new void Clear()
        {
            base.Clear();
            ChangedEventArgs TempArgs = new ChangedEventArgs();
            TempArgs.Content = PropertyName;
            EventHelper.Raise<ChangedEventArgs>(Changed, this, TempArgs);
        }

        #endregion

        #region Properties

        public new T this[int index]
        {
            get { return base[index]; }
            set
            {
                base[index] = value;
                EventHelper.Raise<ChangedEventArgs>(Changed, this, new ChangedEventArgs());
            }
        }

        public string PropertyName { get; set; }

        #endregion
    }
}