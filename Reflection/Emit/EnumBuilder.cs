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
#endregion

namespace Utilities.Reflection.Emit
{
    /// <summary>
    /// Helper class for defining enums
    /// </summary>
    public class EnumBuilder:IType
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Assembly">Assembly builder</param>
        /// <param name="Name">Name of the enum</param>
        /// <param name="Attributes">Attributes for the enum (public, private, etc.)</param>
        /// <param name="FieldType">Type for the enum</param>
        public EnumBuilder(Assembly Assembly,string Name,Type EnumType,TypeAttributes Attributes)
        {
            if (Assembly == null)
                throw new ArgumentNullException("Assembly");
            if (string.IsNullOrEmpty(Name))
                throw new ArgumentNullException("Name");
            this.Name = Name;
            this.Assembly = Assembly;
            this.EnumType = EnumType;
            this.Attributes = Attributes;
            this.Literals = new List<System.Reflection.Emit.FieldBuilder>();
            Setup();
        }

        #endregion

        #region Functions

        /// <summary>
        /// Sets up the enum
        /// </summary>
        private void Setup()
        {
            if (Assembly == null)
                throw new NullReferenceException("No assembly is associated with this enum");
            Builder = Assembly.Module.DefineEnum(Assembly.Name + "." + Name, TypeAttributes.Public, EnumType);
        }

        /// <summary>
        /// Adds a literal to the enum (an entry)
        /// </summary>
        /// <param name="Name">Name of the entry</param>
        /// <param name="Value">Value associated with it</param>
        public void AddLiteral(string Name, object Value)
        {
            Literals.Add(Builder.DefineLiteral(Name, Value));
        }

        /// <summary>
        /// Creates the enum
        /// </summary>
        /// <returns>The type defined by this EnumBuilder</returns>
        public Type Create()
        {
            if (Builder == null)
                throw new NullReferenceException("The builder object has not been defined. Ensure that Setup is called prior to Create");
            DefinedType = Builder.CreateType();
            return DefinedType;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Field name
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Literals defined within the enum
        /// </summary>
        public List<System.Reflection.Emit.FieldBuilder> Literals { get; private set; }

        /// <summary>
        /// Field builder
        /// </summary>
        public System.Reflection.Emit.EnumBuilder Builder {get; private set;}

        /// <summary>
        /// Base enum type (int32, etc.)
        /// </summary>
        public Type EnumType { get; private set; }

        /// <summary>
        /// Type defined by this enum
        /// </summary>
        public Type DefinedType { get; private set; }

        /// <summary>
        /// Attributes for the enum (private, public, etc.)
        /// </summary>
        public TypeAttributes Attributes { get; private set; }

        private Assembly Assembly { get; set; }

        #endregion
    }
}
