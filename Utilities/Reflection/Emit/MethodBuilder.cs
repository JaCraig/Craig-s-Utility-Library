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
using Utilities.Reflection.Emit.Interfaces;
using Utilities.Reflection.ExtensionMethods;
#endregion

namespace Utilities.Reflection.Emit
{
    /// <summary>
    /// Helper class for defining a method within a type
    /// </summary>
    public class MethodBuilder : Utilities.Reflection.Emit.BaseClasses.MethodBase
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="TypeBuilder">Type builder</param>
        /// <param name="Name">Name of the method</param>
        /// <param name="Attributes">Attributes for the method (public, private, etc.)</param>
        /// <param name="Parameters">Parameter types for the method</param>
        /// <param name="ReturnType">Return type for the method</param>
        public MethodBuilder(TypeBuilder TypeBuilder, string Name,
            MethodAttributes Attributes, List<Type> Parameters, Type ReturnType)
            : base()
        {
            if (TypeBuilder == null)
                throw new ArgumentNullException("TypeBuilder");
            if (string.IsNullOrEmpty(Name))
                throw new ArgumentNullException("Name");
            this.Name = Name;
            this.Type = TypeBuilder;
            this.Attributes = Attributes;
            this.ReturnType = (ReturnType == null) ? typeof(void) : ReturnType;
            this.Parameters = new List<ParameterBuilder>();
            this.Parameters.Add(new ParameterBuilder(null, 0));
            if (Parameters != null)
            {
                int x = 1;
                if (Name.StartsWith("set_"))
                    x = -1;
                foreach (Type ParameterType in Parameters)
                {
                    this.Parameters.Add(new ParameterBuilder(ParameterType, x));
                    ++x;
                }
            }
            Commands = new List<ICommand>();
            Builder = Type.Builder.DefineMethod(Name, Attributes, ReturnType,
                (Parameters != null && Parameters.Count > 0) ? Parameters.ToArray() : System.Type.EmptyTypes);
            Generator = Builder.GetILGenerator();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Method builder
        /// </summary>
        public virtual System.Reflection.Emit.MethodBuilder Builder { get; protected set; }

        /// <summary>
        /// Type builder
        /// </summary>
        protected virtual TypeBuilder Type { get; set; }

        #endregion

        #region Overridden Functions

        /// <summary>
        /// Outputs the method to a string
        /// </summary>
        /// <returns>The string representation of the method</returns>
        public override string ToString()
        {
            StringBuilder Output = new StringBuilder();

            Output.Append("\n");
            if ((Attributes & MethodAttributes.Public) > 0)
                Output.Append("public ");
            else if ((Attributes & MethodAttributes.Private) > 0)
                Output.Append("private ");
            if ((Attributes & MethodAttributes.Static) > 0)
                Output.Append("static ");
            if ((Attributes & MethodAttributes.Abstract) > 0)
                Output.Append("abstract ");
            else if ((Attributes & MethodAttributes.HideBySig) > 0)
                Output.Append("override ");
            else if ((Attributes & MethodAttributes.Virtual) > 0)
                Output.Append("virtual ");
            Output.Append(ReturnType.GetName());
            Output.Append(" ").Append(Name).Append("(");

            string Splitter = "";
            if (Parameters != null)
            {
                foreach (ParameterBuilder Parameter in Parameters)
                {
                    if (Parameter.Number != 0)
                    {
                        Output.Append(Splitter).Append(Parameter.GetDefinition());
                        Splitter = ",";
                    }
                }
            }
            Output.Append(")");
            Output.Append("\n{\n");
            Commands.ForEach(x => Output.Append(x.ToString()));
            Output.Append("}\n\n");

            return Output.ToString();
        }

        #endregion
    }
}