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
using System.IO;
using Utilities.IO.ExtensionMethods;
#endregion

namespace Utilities.FileFormats.BaseClasses
{
    /// <summary>
    /// Format base class for objects that are string based
    /// </summary>
    public abstract class StringFormatBase<FormatType> : FormatBase<FormatType, string>
        where FormatType : StringFormatBase<FormatType>, new()
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        protected StringFormatBase()
            : base()
        {
        }

        #endregion

        #region Functions

        /// <summary>
        /// Compares the object to another object
        /// </summary>
        /// <param name="other">Object to compare to</param>
        /// <returns>0 if they are equal, -1 if this is smaller, 1 if it is larger</returns>
        public override int CompareTo(FormatType other)
        {
            return other.ToString().CompareTo(ToString());
        }
        
        /// <summary>
        /// Saves the object
        /// </summary>
        /// <param name="Location">Location to save it to</param>
        /// <returns>This</returns>
        public override FormatType Save(string Location)
        {
            new FileInfo(Location).Save(ToString());
            return (FormatType)this;
        }

        /// <summary>
        /// Determines if the objects are equal
        /// </summary>
        /// <param name="other">Other object to compare to</param>
        /// <returns>True if they are equal, false otherwise</returns>
        public override bool Equals(FormatType other)
        {
            return ToString().Equals(other.ToString(), StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Loads the object from the location specified
        /// </summary>
        /// <param name="Location">Location of the file to load</param>
        /// <returns>This</returns>
        protected override FormatType InternalLoad(string Location)
        {
            LoadFromData(new FileInfo(Location).Read());
            return (FormatType)this;
        }

        /// <summary>
        /// Loads the object from the data specified
        /// </summary>
        /// <param name="Data">Data to load into the object</param>
        protected abstract void LoadFromData(string Data);

        /// <summary>
        /// Clones the object
        /// </summary>
        /// <returns>A newly cloned object</returns>
        public override object Clone()
        {
            return (FormatType)this.ToString();
        }

        #endregion

        #region Operators

        /// <summary>
        /// Converts the format to a string
        /// </summary>
        /// <param name="Value">Value to convert</param>
        /// <returns>The value as a string</returns>
        public static implicit operator string(StringFormatBase<FormatType> Value)
        {
            return Value.ToString();
        }

        /// <summary>
        /// Converts the string to the format specified
        /// </summary>
        /// <param name="Value">Value to convert</param>
        /// <returns>The string as an object</returns>
        public static implicit operator StringFormatBase<FormatType>(string Value)
        {
            FormatType ReturnValue=new FormatType();
            ReturnValue.LoadFromData(Value);
            return ReturnValue;
        }

        #endregion
    }
}