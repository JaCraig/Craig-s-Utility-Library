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
using Utilities.Reflection.Emit.Interfaces;
using System.Reflection.Emit;
using Utilities.Reflection.Emit.BaseClasses;
#endregion

namespace Utilities.Reflection.Emit
{
    /// <summary>
    /// Helper class for defining a field within a type
    /// </summary>
    public class FieldBuilder : VariableBase
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="TypeBuilder">Type builder</param>
        /// <param name="Name">Name of the method</param>
        /// <param name="Attributes">Attributes for the field (public, private, etc.)</param>
        /// <param name="FieldType">Type for the field</param>
        public FieldBuilder(TypeBuilder TypeBuilder, string Name, Type FieldType, FieldAttributes Attributes)
            : base()
        {
            if (TypeBuilder == null)
                throw new ArgumentNullException("TypeBuilder");
            if (string.IsNullOrEmpty(Name))
                throw new ArgumentNullException("Name");
            this.Name = Name;
            this.Type = TypeBuilder;
            this.DataType = FieldType;
            this.Attributes = Attributes;
            Builder = Type.Builder.DefineField(Name, FieldType, Attributes);
        }

        #endregion

        #region Functions

        public override void Load(System.Reflection.Emit.ILGenerator Generator)
        {
            Generator.Emit(OpCodes.Ldfld, Builder);
        }

        public override void Save(System.Reflection.Emit.ILGenerator Generator)
        {
            Generator.Emit(OpCodes.Stfld, Builder);
        }

        public override string GetDefinition()
        {
            StringBuilder Output = new StringBuilder();

            Output.Append("\n");
            if ((Attributes & FieldAttributes.Public) > 0)
                Output.Append("public ");
            else if ((Attributes & FieldAttributes.Private) > 0)
                Output.Append("private ");
            if ((Attributes & FieldAttributes.Static) > 0)
                Output.Append("static ");
            Output.Append(Reflection.GetTypeName(DataType));
            Output.Append(" ").Append(Name).Append(";");

            return Output.ToString();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Field builder
        /// </summary>
        public virtual System.Reflection.Emit.FieldBuilder Builder { get; protected set; }

        /// <summary>
        /// Attributes for the field (private, public, etc.)
        /// </summary>
        public virtual FieldAttributes Attributes { get; protected set; }

        /// <summary>
        /// Type builder
        /// </summary>
        protected virtual TypeBuilder Type { get; set; }

        #endregion

        #region Overridden Functions

        public override string ToString()
        {
            return Name;
        }

        #endregion
    }
}