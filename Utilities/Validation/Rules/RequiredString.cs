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
using Utilities.DataTypes.Comparison;
using Utilities.Validation.BaseClasses;
using Utilities.Validation.Exceptions;
#endregion

namespace Utilities.Validation.Rules
{
    /// <summary>
    /// This item is required
    /// </summary>
    /// <typeparam name="ObjectType">Object type that the rule applies to</typeparam>
    public class RequiredString<ObjectType> : Rule<ObjectType, string>
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ItemToValidate">Item to validate</param>
        /// <param name="ErrorMessage">Error message</param>
        /// <param name="DefaultValue">Default value</param>
        public RequiredString(Func<ObjectType,string> ItemToValidate,string DefaultValue,string ErrorMessage)
            : base(ItemToValidate,ErrorMessage)
        {
            this.DefaultValue = DefaultValue;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Default value
        /// </summary>
        protected virtual string DefaultValue { get; set; }

        #endregion

        #region Functions

        /// <summary>
        /// Validates an object
        /// </summary>
        /// <param name="Object">Object to validate</param>
        public override void Validate(ObjectType Object)
        {
            GenericEqualityComparer<string> Comparer = new GenericEqualityComparer<string>();
            if (Comparer.Equals(ItemToValidate(Object), DefaultValue))
                throw new NotValid(ErrorMessage);
        }

        #endregion
    }
}