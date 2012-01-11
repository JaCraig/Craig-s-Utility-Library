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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities.Validation.BaseClasses;
using Utilities.Validation.Exceptions;
using Utilities.DataTypes.Comparison;
#endregion

namespace Utilities.Validation.Rules
{
    /// <summary>
    /// This item is equal to the value
    /// </summary>
    /// <typeparam name="ObjectType">Object type that the rule applies to</typeparam>
    /// <typeparam name="DataType">Data type of the object validating</typeparam>
    public class Equal<ObjectType, DataType> : Rule<ObjectType, DataType>
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ItemToValidate">Item to validate</param>
        /// <param name="Value">value</param>
        /// <param name="ErrorMessage">Error message</param>
        public Equal(Func<ObjectType, DataType> ItemToValidate, DataType Value, string ErrorMessage)
            : base(ItemToValidate, ErrorMessage)
        {
            this.Value = Value;
        }

        #endregion

        #region Properties

        /// <summary>
        /// value
        /// </summary>
        protected virtual DataType Value { get; set; }

        #endregion

        #region Functions

        public override void Validate(ObjectType Object)
        {
            GenericEqualityComparer<DataType> Comparer = new GenericEqualityComparer<DataType>();
            if (!Comparer.Equals(ItemToValidate(Object), Value))
                throw new NotValid(ErrorMessage);
        }

        #endregion
    }

    /// <summary>
    /// Equal attribute
    /// </summary>
    public class Equal : BaseAttribute
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ErrorMessage">Error message</param>
        /// <param name="Value">Value to compare to</param>
        public Equal(object Value, string ErrorMessage = "")
            : base(ErrorMessage)
        {
            this.Value = (IComparable)Value;
        }

        #endregion

        #region Properties

        /// <summary>
        /// value to compare to
        /// </summary>
        public IComparable Value { get; set; }

        #endregion
    }
}