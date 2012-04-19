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
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
using Utilities.Validation.BaseClasses;
using Utilities.Validation.Exceptions;
#endregion

namespace Utilities.Validation.Rules
{
    /// <summary>
    /// Email address
    /// </summary>
    public class IsEmailAddress<ObjectType> : Rule<ObjectType, string>
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ItemToValidate">Item to validate</param>
        /// <param name="ErrorMessage">Error message</param>
        public IsEmailAddress(Func<ObjectType, string> ItemToValidate, string ErrorMessage)
            : base(ItemToValidate, ErrorMessage)
        {
        }

        #endregion

        #region Functions

        /// <summary>
        /// Validates an object
        /// </summary>
        /// <param name="Object">Object to validate</param>
        public override void Validate(ObjectType Object)
        {
            string Value = this.ItemToValidate(Object);
            if (string.IsNullOrEmpty(Value))
                return;
            System.Text.RegularExpressions.Regex TempReg = new System.Text.RegularExpressions.Regex(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                  @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                  @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
            if(!TempReg.IsMatch(Value))
                throw new NotValid(ErrorMessage);
        }

        #endregion
    }

    /// <summary>
    /// IsEmailAddress attribute
    /// </summary>
    public class IsEmailAddress : BaseAttribute
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ErrorMessage">Error message</param>
        public IsEmailAddress(string ErrorMessage = "")
            : base(ErrorMessage)
        {
        }

        #endregion
    }
}