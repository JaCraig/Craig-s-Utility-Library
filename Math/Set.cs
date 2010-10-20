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
using System.Text;
using Utilities.DataTypes;
#endregion

namespace Utilities.Math
{
    /// <summary>
    /// Class to be used for sets of data
    /// </summary>
    /// <typeparam name="T">Type that the set holds</typeparam>
    public class Set<T> : Vector<T>
    {
        #region Constructors

        public Set()
            : base()
        {

        }

        public Set(int InitialSize)
            : base(InitialSize)
        {
        }

        #endregion

        #region Public Functions

        /// <summary>
        /// Used to tell if this set contains the other
        /// </summary>
        /// <param name="Set">Set to check against</param>
        /// <returns>True if it is, false otherwise</returns>
        public bool Contains(Set<T> Set)
        {
            return Set.IsSubset(this);
        }

        /// <summary>
        /// Used to tell if this is a subset of the other
        /// </summary>
        /// <param name="Set">Set to check against</param>
        /// <returns>True if it is, false otherwise</returns>
        public bool IsSubset(Set<T> Set)
        {
            if (Set == null || this.NumberItems > Set.NumberItems)
                return false;

            for (int x = 0; x < this.NumberItems; ++x)
            {
                if (!Set.Contains(this.Items[x]))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Determines if the sets intersect
        /// </summary>
        /// <param name="Set">Set to check against</param>
        /// <returns>True if they do, false otherwise</returns>
        public bool Intersect(Set<T> Set)
        {
            if (Set == null)
                return false;
            for (int x = 0; x < this.NumberItems; ++x)
            {
                if (Set.Contains(this.Items[x]))
                    return true;
            }
            return false;
        }

        #endregion

        #region Public Static Functions

        /// <summary>
        /// Gets the intersection of set 1 and set 2
        /// </summary>
        /// <param name="Set1">Set 1</param>
        /// <param name="Set2">Set 2</param>
        /// <returns>The intersection of the two sets</returns>
        public static Set<T> GetIntersection(Set<T> Set1, Set<T> Set2)
        {
            if (Set1 == null || Set2 == null || !Set1.Intersect(Set2))
                return null;
            Set<T> ReturnValue = new Set<T>();
            for (int x = 0; x < Set1.NumberItems; ++x)
            {
                if (Set2.Contains(Set1.Items[x]))
                    ReturnValue.Add(Set1.Items[x]);
            }

            for (int x = 0; x < Set2.NumberItems; ++x)
            {
                if (Set1.Contains(Set2.Items[x]))
                    ReturnValue.Add(Set2.Items[x]);
            }

            return ReturnValue;
        }

        /// <summary>
        /// Adds two sets together
        /// </summary>
        /// <param name="Set1">Set 1</param>
        /// <param name="Set2">Set 2</param>
        /// <returns>The joined sets</returns>
        public static Set<T> operator +(Set<T> Set1, Set<T> Set2)
        {
            if (Set1 == null || Set2 == null)
                throw new ArgumentNullException();

            Set<T> ReturnValue = new Set<T>();
            for (int x = 0; x < Set1.NumberItems; ++x)
            {
                ReturnValue.Add(Set1.Items[x]); ;
            }
            for (int x = 0; x < Set2.NumberItems; ++x)
            {
                ReturnValue.Add(Set2.Items[x]); ;
            }
            return ReturnValue;
        }

        /// <summary>
        /// Removes items from set 2 from set 1
        /// </summary>
        /// <param name="Set1">Set 1</param>
        /// <param name="Set2">Set 2</param>
        /// <returns>The resulting set</returns>
        public static Set<T> operator -(Set<T> Set1, Set<T> Set2)
        {
            if (Set1 == null || Set2 == null)
                throw new ArgumentNullException();

            Set<T> ReturnValue = new Set<T>();
            for (int x = 0; x < Set1.NumberItems; ++x)
            {
                if (!Set2.Contains(Set1.Items[x]))
                    ReturnValue.Add(Set1.Items[x]);
            }
            return ReturnValue;
        }

        #endregion

        #region Public Overridden Functions

        public override string ToString()
        {
            StringBuilder Builder = new StringBuilder();
            Builder.Append("{ ");
            string Splitter = "";
            for (int x = 0; x < this.NumberItems; ++x)
            {
                Builder.Append(Splitter);
                Builder.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, "{0}", this.Items[x]);
                Splitter = ",  ";
            }
            Builder.Append(" }");
            return Builder.ToString();
        }

        #endregion
    }
}