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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
#endregion

namespace Utilities.Reflection.Emit
{
    /// <summary>
    /// Helper class for defining a field within a type
    /// </summary>
    public class FieldBuilder
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="TypeBuilder">Type builder</param>
        /// <param name="Name">Name of the method</param>
        /// <param name="Attributes">Attributes for the field (public, private, etc.)</param>
        /// <param name="FieldType">Type for the field</param>
        public FieldBuilder(TypeBuilder TypeBuilder,string Name,Type FieldType,FieldAttributes Attributes)
        {
            if (TypeBuilder == null)
                throw new ArgumentNullException("TypeBuilder");
            if (string.IsNullOrEmpty(Name))
                throw new ArgumentNullException("Name");
            this.Name = Name;
            this.Type = TypeBuilder;
            this.FieldType = FieldType;
            this.Attributes = Attributes;
            Setup();
        }

        #endregion

        #region Functions

        /// <summary>
        /// Sets up the field
        /// </summary>
        private void Setup()
        {
            if (Type == null)
                throw new NullReferenceException("No type is associated with this field");
            Builder = Type.Builder.DefineField(Name, FieldType, Attributes);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Field name
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Field builder
        /// </summary>
        public System.Reflection.Emit.FieldBuilder Builder {get; private set;}

        /// <summary>
        /// Field type
        /// </summary>
        public Type FieldType { get; private set; }

        /// <summary>
        /// Attributes for the field (private, public, etc.)
        /// </summary>
        public FieldAttributes Attributes { get; private set; }

        private TypeBuilder Type { get; set; }

        #endregion
    }
}
