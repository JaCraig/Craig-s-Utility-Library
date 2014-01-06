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

#region Usings

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;

#endregion Usings

namespace Utilities.DataTypes
{
    /// <summary>
    /// Class to be used for sets of data
    /// </summary>
    /// <typeparam name="T">Type that the set holds</typeparam>
    public class Set<T> : List<T>
    {
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public Set()
            : base()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="InitialSize">Initial size</param>
        public Set(int InitialSize)
            : base(InitialSize)
        {
        }

        #endregion Constructors

        #region Public Functions

        /// <summary>
        /// Used to tell if this set contains the other
        /// </summary>
        /// <param name="Set">Set to check against</param>
        /// <returns>True if it is, false otherwise</returns>
        public virtual bool Contains(Set<T> Set)
        {
            Contract.Requires<ArgumentNullException>(Set != null, "Set");
            return Set.IsSubset(this);
        }

        /// <summary>
        /// Determines if the sets intersect
        /// </summary>
        /// <param name="Set">Set to check against</param>
        /// <returns>True if they do, false otherwise</returns>
        public virtual bool Intersect(Set<T> Set)
        {
            if (Set == null)
                return false;
            for (int x = 0; x < this.Count; ++x)
                if (Set.Contains(this[x]))
                    return true;
            return false;
        }

        /// <summary>
        /// Used to tell if this is a subset of the other
        /// </summary>
        /// <param name="Set">Set to check against</param>
        /// <returns>True if it is, false otherwise</returns>
        public virtual bool IsSubset(Set<T> Set)
        {
            if (Set == null || this.Count > Set.Count)
                return false;

            for (int x = 0; x < this.Count; ++x)
                if (!Set.Contains(this[x]))
                    return false;
            return true;
        }

        #endregion Public Functions

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
            for (int x = 0; x < Set1.Count; ++x)
                if (Set2.Contains(Set1[x]))
                    ReturnValue.Add(Set1[x]);

            for (int x = 0; x < Set2.Count; ++x)
                if (Set1.Contains(Set2[x]))
                    ReturnValue.Add(Set2[x]);

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
            Contract.Requires<ArgumentNullException>(Set1 != null, "Set1");
            Contract.Requires<ArgumentNullException>(Set2 != null, "Set2");

            Set<T> ReturnValue = new Set<T>();
            for (int x = 0; x < Set1.Count; ++x)
                if (!Set2.Contains(Set1[x]))
                    ReturnValue.Add(Set1[x]);
            return ReturnValue;
        }

        /// <summary>
        /// Determines if the two sets are not equivalent
        /// </summary>
        /// <param name="Set1">Set 1</param>
        /// <param name="Set2">Set 2</param>
        /// <returns>False if they are, true otherwise</returns>
        public static bool operator !=(Set<T> Set1, Set<T> Set2)
        {
            return !(Set1 == Set2);
        }

        /// <summary>
        /// Adds two sets together
        /// </summary>
        /// <param name="Set1">Set 1</param>
        /// <param name="Set2">Set 2</param>
        /// <returns>The joined sets</returns>
        public static Set<T> operator +(Set<T> Set1, Set<T> Set2)
        {
            Contract.Requires<ArgumentNullException>(Set1 != null, "Set1");
            Contract.Requires<ArgumentNullException>(Set2 != null, "Set2");

            Set<T> ReturnValue = new Set<T>();
            for (int x = 0; x < Set1.Count; ++x)
                ReturnValue.Add(Set1[x]); ;
            for (int x = 0; x < Set2.Count; ++x)
                ReturnValue.Add(Set2[x]); ;
            return ReturnValue;
        }

        /// <summary>
        /// Determines if the two sets are equivalent
        /// </summary>
        /// <param name="Set1">Set 1</param>
        /// <param name="Set2">Set 2</param>
        /// <returns>True if they are, false otherwise</returns>
        public static bool operator ==(Set<T> Set1, Set<T> Set2)
        {
            if (((object)Set1) == null && ((object)Set2) == null)
                return true;
            if (((object)Set1) == null || ((object)Set2) == null)
                return false;
            return Set1.Contains(Set2) && Set2.Contains(Set1);
        }

        #endregion Public Static Functions

        #region Public Overridden Functions

        /// <summary>
        /// Determines if the two sets are equivalent
        /// </summary>
        /// <param name="obj">The object to compare to</param>
        /// <returns>True if they are, false otherwise</returns>
        public override bool Equals(object obj)
        {
            return this == (obj as Set<T>);
        }

        /// <summary>
        /// Returns the hash code for the object
        /// </summary>
        /// <returns>The hash code for the object</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Returns the set as a string
        /// </summary>
        /// <returns>The set as a string</returns>
        public override string ToString()
        {
            StringBuilder Builder = new StringBuilder();
            Builder.Append("{ ");
            string Splitter = "";
            for (int x = 0; x < this.Count; ++x)
            {
                Builder.Append(Splitter);
                Builder.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, "{0}", this[x]);
                Splitter = ",  ";
            }
            Builder.Append(" }");
            return Builder.ToString();
        }

        #endregion Public Overridden Functions
    }
}