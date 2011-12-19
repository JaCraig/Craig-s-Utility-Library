/*
Copyright (c) 2011 <a href="http://www.gutgames.com">James Craig</a>

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
    /// Is Domain
    /// </summary>
    public class IsDomain<ObjectType> : Rule<ObjectType,string>
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ItemToValidate">Item to validate</param>
        /// <param name="ErrorMessage">Error message</param>
        public IsDomain(Func<ObjectType, string> ItemToValidate, string ErrorMessage)
            : base(ItemToValidate, ErrorMessage)
        {
        }

        #endregion

        #region Functions

        public override void Validate(ObjectType Object)
        {
            string Value = this.ItemToValidate(Object);
            if (string.IsNullOrEmpty(Value))
                return;
            if(!new System.Text.RegularExpressions.Regex(@"^(http|https|ftp)://([a-zA-Z0-9_-]*(?:\.[a-zA-Z0-9_-]*)+):?([0-9]+)?/?").IsMatch(Value))
                throw new NotValid(ErrorMessage);
        }

        #endregion
    }

    /// <summary>
    /// IsDomain attribute
    /// </summary>
    public class IsDomain : BaseAttribute
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ErrorMessage">Error message</param>
        public IsDomain(string ErrorMessage = "")
            : base(ErrorMessage)
        {
        }

        #endregion
    }
}
