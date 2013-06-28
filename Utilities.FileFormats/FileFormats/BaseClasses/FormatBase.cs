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
using System;

#endregion

namespace Utilities.FileFormats.BaseClasses
{
    /// <summary>
    /// Format base class
    /// </summary>
    public abstract class FormatBase<FormatType, ContentType> : IComparable, IComparable<FormatType>, IEquatable<FormatType>,ICloneable
        where FormatType : FormatBase<FormatType, ContentType>, new()
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        protected FormatBase() { }

        #endregion

        #region Functions

        /// <summary>
        /// Compares the object to another object
        /// </summary>
        /// <param name="obj">Object to compare to</param>
        /// <returns>0 if they are equal, -1 if this is smaller, 1 if it is larger</returns>
        public int CompareTo(object obj)
        {
            if (obj is FormatBase<FormatType, ContentType>)
                return CompareTo((FormatType)obj);
            return -1;
        }

        /// <summary>
        /// Compares the object to another object
        /// </summary>
        /// <param name="other">Object to compare to</param>
        /// <returns>0 if they are equal, -1 if this is smaller, 1 if it is larger</returns>
        public abstract int CompareTo(FormatType other);

        /// <summary>
        /// Determines if the objects are equal
        /// </summary>
        /// <param name="other">Other object to compare to</param>
        /// <returns>True if they are equal, false otherwise</returns>
        public abstract bool Equals(FormatType other);

        /// <summary>
        /// Loads the object from the location specified
        /// </summary>
        /// <param name="Location">Location of the file to load</param>
        /// <returns>The object specified in the location</returns>
        public static FormatType Load(string Location)
        {
            return new FormatType().InternalLoad(Location);
        }

        /// <summary>
        /// Loads the object from the location specified
        /// </summary>
        /// <param name="Location">Location of the file to load</param>
        /// <returns>This</returns>
        protected abstract FormatType InternalLoad(string Location);

        /// <summary>
        /// Saves the object
        /// </summary>
        /// <param name="Location">Location to save it to</param>
        /// <returns>This</returns>
        public abstract FormatType Save(string Location);

        /// <summary>
        /// Clones the object
        /// </summary>
        /// <returns>A newly cloned object</returns>
        public abstract object Clone();

        /// <summary>
        /// Determines if the objects are equal
        /// </summary>
        /// <param name="obj">Other object to compare to</param>
        /// <returns>True if they are equal, false otherwise</returns>
        public override bool Equals(object obj)
        {
            FormatType TempItem = obj as FormatType;
            if (TempItem == null)
                return false;
            return Equals(TempItem);
        }

        /// <summary>
        /// Gets the hash code for the object
        /// </summary>
        /// <returns>The hash code for the object</returns>
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        #endregion
    }
}