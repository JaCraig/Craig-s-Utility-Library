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

using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Utilities.Validation
{
    /// <summary>
    /// Object extensions
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class ObjectExtensions
    {
        /// <summary>
        /// Determines if the object is valid
        /// </summary>
        /// <typeparam name="ObjectType">Object type</typeparam>
        /// <param name="Object">Object to validate</param>
        /// <param name="Results">Results list</param>
        /// <returns>True if valid, false otherwise</returns>
        public static bool TryValidate<ObjectType>(this ObjectType Object, ICollection<ValidationResult> Results)
        {
            if (Object == null)
                return true;
            return Validator.TryValidateObject(Object, new ValidationContext(Object, null, null), Results, true);
        }

        /// <summary>
        /// Determines if the object is valid
        /// </summary>
        /// <typeparam name="ObjectType">Object type</typeparam>
        /// <param name="Object">Object to validate</param>
        /// <exception cref="System.ComponentModel.DataAnnotations.ValidationException"/>
        public static void Validate<ObjectType>(this ObjectType Object)
        {
            if (Object == null)
                return;
            Validator.ValidateObject(Object, new ValidationContext(Object, null, null), true);
        }
    }
}