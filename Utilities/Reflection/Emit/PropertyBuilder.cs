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
using System.Reflection.Emit;
using System.Text;
using Utilities.Reflection.Emit.BaseClasses;
using Utilities.Reflection.Emit.Interfaces;
using Utilities.Reflection.ExtensionMethods;
#endregion

namespace Utilities.Reflection.Emit
{
    /// <summary>
    /// Helper class for defining a property
    /// </summary>
    public class PropertyBuilder : VariableBase, IPropertyBuilder
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="TypeBuilder">Type builder</param>
        /// <param name="Name">Name of the property</param>
        /// <param name="Attributes">Attributes for the property (public, private, etc.)</param>
        /// <param name="GetMethodAttributes">Get method attributes</param>
        /// <param name="SetMethodAttributes">Set method attributes</param>
        /// <param name="PropertyType">Property type for the property</param>
        /// <param name="Parameters">Parameter types for the property</param>
        public PropertyBuilder(TypeBuilder TypeBuilder, string Name,
            PropertyAttributes Attributes, MethodAttributes GetMethodAttributes,
            MethodAttributes SetMethodAttributes,
            Type PropertyType, List<Type> Parameters)
            : base()
        {
            if (TypeBuilder == null)
                throw new ArgumentNullException("TypeBuilder");
            if (string.IsNullOrEmpty(Name))
                throw new ArgumentNullException("Name");
            this.Name = Name;
            this.Type = TypeBuilder;
            this.Attributes = Attributes;
            this.GetMethodAttributes = GetMethodAttributes;
            this.SetMethodAttributes = SetMethodAttributes;
            this.DataType = PropertyType;
            this.Parameters = new List<ParameterBuilder>();
            if (Parameters != null)
            {
                int x = 1;
                foreach (Type Parameter in Parameters)
                {
                    this.Parameters.Add(new ParameterBuilder(Parameter, x));
                    ++x;
                }
            }
            Builder = Type.Builder.DefineProperty(Name, Attributes, PropertyType,
                (Parameters != null && Parameters.Count > 0) ? Parameters.ToArray() : System.Type.EmptyTypes);
            GetMethod = new MethodBuilder(Type, "get_" + Name, GetMethodAttributes, Parameters, PropertyType);
            List<Type> SetParameters = new List<System.Type>();
            if (Parameters != null)
            {
                SetParameters.AddRange(Parameters);
            }
            SetParameters.Add(PropertyType);
            SetMethod = new MethodBuilder(Type, "set_" + Name, SetMethodAttributes, SetParameters, typeof(void));
            Builder.SetGetMethod(GetMethod.Builder);
            Builder.SetSetMethod(SetMethod.Builder);
        }

        #endregion

        #region Functions

        public override void Load(ILGenerator Generator)
        {
            if (GetMethod.Builder.IsVirtual)
                Generator.EmitCall(OpCodes.Callvirt, GetMethod.Builder, null);
            else
                Generator.EmitCall(OpCodes.Call, GetMethod.Builder, null);
        }

        public override void Save(ILGenerator Generator)
        {
            if(SetMethod.Builder.IsVirtual)
                Generator.EmitCall(OpCodes.Callvirt, SetMethod.Builder, null);
            else
                Generator.EmitCall(OpCodes.Call, SetMethod.Builder, null);
        }

        public override string GetDefinition()
        {
            StringBuilder Output = new StringBuilder();

            Output.Append("\n");
            if ((GetMethodAttributes & MethodAttributes.Public) > 0)
                Output.Append("public ");
            else if ((GetMethodAttributes & MethodAttributes.Private) > 0)
                Output.Append("private ");
            if ((GetMethodAttributes & MethodAttributes.Static) > 0)
                Output.Append("static ");
            if ((GetMethodAttributes & MethodAttributes.Virtual) > 0)
                Output.Append("virtual ");
            else if ((GetMethodAttributes & MethodAttributes.Abstract) > 0)
                Output.Append("abstract ");
            else if ((GetMethodAttributes & MethodAttributes.HideBySig) > 0)
                Output.Append("override ");

            Output.Append(DataType.GetName());
            Output.Append(" ").Append(Name);

            string Splitter = "";
            if (Parameters != null && Parameters.Count > 0)
            {
                Output.Append("[");
                foreach (ParameterBuilder Parameter in Parameters)
                {
                    Output.Append(Splitter).Append(Parameter.GetDefinition());
                    Splitter = ",";
                }
                Output.Append("]");
            }
            Output.Append(" {\nget\n{\n");
            foreach (ICommand Command in GetMethod.Commands)
            {
                Output.Append(Command.ToString());
            }
            Output.Append("}\n\n");
            if ((SetMethodAttributes & GetMethodAttributes) != SetMethodAttributes)
            {
                if ((SetMethodAttributes & MethodAttributes.Public) > 0)
                    Output.Append("public ");
                else if ((SetMethodAttributes & MethodAttributes.Private) > 0)
                    Output.Append("private ");
            }
            Output.Append("set\n{\n");
            foreach (ICommand Command in SetMethod.Commands)
            {
                Output.Append(Command.ToString());
            }
            Output.Append("}\n}\n");

            return Output.ToString();
        }

        #endregion

        #region Properties

        public virtual List<ParameterBuilder> Parameters { get; protected set; }
        public virtual System.Reflection.Emit.PropertyBuilder Builder { get; protected set; }
        public virtual System.Reflection.PropertyAttributes Attributes { get; protected set; }
        public virtual System.Reflection.MethodAttributes GetMethodAttributes { get; protected set; }
        public virtual System.Reflection.MethodAttributes SetMethodAttributes { get; protected set; }
        public virtual MethodBuilder GetMethod { get; protected set; }
        public virtual MethodBuilder SetMethod { get; protected set; }

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

        #region Operator Functions

        public static PropertyBuilder operator ++(PropertyBuilder Left)
        {
            if (Utilities.Reflection.Emit.BaseClasses.MethodBase.CurrentMethod == null)
                throw new NullReferenceException("Unsure which method is the current method");
            Left.Assign(Utilities.Reflection.Emit.BaseClasses.MethodBase.CurrentMethod.Add(Left, 1));
            return Left;
        }

        public static PropertyBuilder operator --(PropertyBuilder Left)
        {
            if (Utilities.Reflection.Emit.BaseClasses.MethodBase.CurrentMethod == null)
                throw new NullReferenceException("Unsure which method is the current method");
            Left.Assign(Utilities.Reflection.Emit.BaseClasses.MethodBase.CurrentMethod.Subtract(Left, 1));
            return Left;
        }

        #endregion
    }
}