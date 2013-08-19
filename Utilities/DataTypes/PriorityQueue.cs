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
using System.Collections.Generic;
using System.Linq;
#endregion

namespace Utilities.DataTypes
{
    /// <summary>
    /// Helper class that implements a priority queue
    /// </summary>
    /// <typeparam name="T">The type of the values placed in the queue</typeparam>
    public class PriorityQueue<T> : ListMapping<int, T>
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public PriorityQueue()
            : base()
        {
            HighestKey = int.MinValue;
        }

        #endregion

        #region Public Functions

        /// <summary>
        /// Peek at the next thing in the queue
        /// </summary>
        /// <returns>The next item in queue or default(T) if it is empty</returns>
        public virtual T Peek()
        {
            if (Items.ContainsKey(HighestKey))
                return Items[HighestKey].FirstOrDefault();
            return default(T);
        }

        /// <summary>
        /// Adds an item to the queue
        /// </summary>
        /// <param name="Priority">Priority</param>
        /// <param name="Value">Items to add</param>
        public override void Add(int Priority, ICollection<T> Value)
        {
            if (Priority > HighestKey)
                HighestKey = Priority;
            base.Add(Priority, Value);
        }

        /// <summary>
        /// Adds an item to the queue
        /// </summary>
        /// <param name="item">Item to add</param>
        public override void Add(System.Collections.Generic.KeyValuePair<int, ICollection<T>> item)
        {
            if (item.Key > HighestKey)
                HighestKey = item.Key;
            base.Add(item);
        }

        /// <summary>
        /// Adds an item to the queue
        /// </summary>
        /// <param name="Priority">Priority</param>
        /// <param name="Value">Item to add</param>
        public override void Add(int Priority, T Value)
        {
            if (Priority > HighestKey)
                HighestKey = Priority;
            base.Add(Priority, Value);
        }

        /// <summary>
        /// Removes an item from the queue and returns it
        /// </summary>
        /// <returns>The next item in the queue</returns>
        public virtual T Pop()
        {
            T ReturnValue = default(T);
            if (Items.ContainsKey(HighestKey) && Items[HighestKey].Count > 0)
            {
                ReturnValue = Items[HighestKey].FirstOrDefault();
                Remove(HighestKey, ReturnValue);
                if (!ContainsKey(HighestKey))
                {
                    HighestKey = int.MinValue;
                    foreach (int Key in Items.Keys)
                        if (Key > HighestKey)
                            HighestKey = Key;
                }
            }
            return ReturnValue;
        }

        #endregion

        #region Protected Variables

        /// <summary>
        /// Highest value key
        /// </summary>
        protected virtual int HighestKey { get; set; }

        #endregion
    }
}