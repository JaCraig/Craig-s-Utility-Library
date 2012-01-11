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
using System.Text.RegularExpressions;
#endregion

namespace Utilities.Validation.Rules
{
    /// <summary>
    /// This item is equal to the value
    /// </summary>
    /// <typeparam name="ObjectType">Object type that the rule applies to</typeparam>
    public class NotNaN<ObjectType> : Rule<ObjectType, double>
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ItemToValidate">Item to validate</param>
        /// <param name="ErrorMessage">Error message</param>
        public NotNaN(Func<ObjectType, double> ItemToValidate, string ErrorMessage)
            : base(ItemToValidate, ErrorMessage)
        {
        }

        #endregion

        #region Functions

        public override void Validate(ObjectType Object)
        {
            if (double.IsNaN(ItemToValidate(Object)))
                throw new NotValid(ErrorMessage);
        }

        #endregion
    }

    /// <summary>
    /// NotNaN attribute
    /// </summary>
    public class NotNaN : BaseAttribute
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ErrorMessage">Error message</param>
        public NotNaN(string ErrorMessage = "")
            : base(ErrorMessage)
        {
        }

        #endregion
    }
}
