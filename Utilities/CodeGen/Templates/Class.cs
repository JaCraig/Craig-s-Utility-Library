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
using System.Linq;
using System.Text;
using Utilities.CodeGen.Interfaces;
using Utilities.CodeGen.Templates.Enums;
using Utilities.CodeGen.Templates.Interfaces;
using Utilities.CodeGen.Templates.BaseClasses;
#endregion

namespace Utilities.CodeGen.Templates
{
    /// <summary>
    /// Basic class template
    /// </summary>
    public class Class:ObjectBase
    {
        #region Constructor

        public Class(AccessModifier AccessModifier, Modifiers Modifier, string ClassName,
            string Namespace)
            : base()
        {
            Properties = new List<IProperty>();
            Usings = new List<Using>();
            Constructors = new List<IFunction>();
            Functions = new List<IFunction>();
            this.ClassName = ClassName;
            this.AccessModifier = AccessModifier;
            this.Modifier = Modifier;
            this.Namespace = Namespace;
        }

        #endregion

        #region Functions

        protected override void SetupTemplate()
        {
            Template = new DefaultTemplate(@"/* Code generated using Craig's Utility Library
http://cul.codeplex.com */
@Usings

namespace @Namespace
{
    /// <summary>
    /// @ClassName class
    /// </summary>
    @AccessModifier @Modifier class @ClassName
    {
        #region Constructor
        
        @Constructor

        #endregion

        #region Properties

        @Properties

        #endregion

        #region Functions

        @Functions

        #endregion
    }
}");
        }

        protected override void SetupInput()
        {
            string TempString = "";
            foreach (IProperty Property in Properties)
            {
                TempString += Property.Transform() + "\n\n";
            }
            Input.Values.Add("Properties", TempString);
            TempString="";
            foreach (Using Using in Usings)
            {
                TempString += Using.Transform() + "\n";
            }
            Input.Values.Add("Usings", TempString);
            TempString = "";
            foreach (IFunction Constructor in Constructors)
            {
                TempString += Constructor.Transform() + "\n";
            }
            Input.Values.Add("Constructor", TempString);
            TempString = "";
            foreach (IFunction Function in Functions)
            {
                TempString += Function.Transform() + "\n";
            }
            Input.Values.Add("Functions", TempString);
            Input.Values.Add("ClassName", ClassName);
            Input.Values.Add("AccessModifier", AccessModifier.ToString().ToLower());
            if (Modifier == Modifiers.None)
                Input.Values.Add("Modifier", "");
            else
                Input.Values.Add("Modifier", Modifier.ToString().ToLower());
            Input.Values.Add("Namespace", Namespace);
        }
        
        /// <summary>
        /// Adds a property to the class
        /// </summary>
        /// <param name="AccessModifier">Access modifier</param>
        /// <param name="Modifier">Modifier</param>
        /// <param name="PropertyType">Property type</param>
        /// <param name="Name">Property name</param>
        /// <param name="GetFunction">Get function</param>
        /// <param name="SetFunction">Set function</param>
        public virtual void AddProperty(AccessModifier AccessModifier, Modifiers Modifier, string PropertyType,
            string Name, string GetFunction, string SetFunction)
        {
            Property Property = new Property(Parser);
            Property.AccessModifier = AccessModifier;
            Property.Modifier = Modifier;
            Property.PropertyType = PropertyType;
            Property.Name = Name;
            Property.GetFunction = GetFunction;
            Property.SetFunction = SetFunction;
            Properties.Add(Property);
        }

        /// <summary>
        /// Adds a property to the class
        /// </summary>
        /// <param name="AccessModifier">Access modifier</param>
        /// <param name="Modifier">Modifier</param>
        /// <param name="PropertyType">Property type</param>
        /// <param name="Name">Property name</param>
        public virtual void AddProperty(AccessModifier AccessModifier, Modifiers Modifier,
            string PropertyType, string Name)
        {
            DefaultProperty Property = new DefaultProperty(Parser);
            Property.AccessModifier = AccessModifier;
            Property.Modifier = Modifier;
            Property.PropertyType = PropertyType;
            Property.Name = Name;
            Properties.Add(Property);
        }

        /// <summary>
        /// Adds a using to the class's header
        /// </summary>
        /// <param name="Namespace">Namespace to add</param>
        public virtual void AddUsing(string Namespace)
        {
            Usings.Add(new Using(Namespace, Parser));
        }

        /// <summary>
        /// Adds a constructor to the class
        /// </summary>
        /// <param name="AccessModifier">Access Modifier</param>
        /// <param name="Body">Constructor body</param>
        /// <param name="ParameterList">Parameter list</param>
        public virtual void AddConstructor(AccessModifier AccessModifier,string Body,IParameter[] ParameterList)
        {
            Constructors.Add(new Constructor(AccessModifier, ClassName, ParameterList, Body, Parser));
        }

        /// <summary>
        /// Adds a function the the class
        /// </summary>
        /// <param name="AccessModifier">Access modifier</param>
        /// <param name="Modifier">Modifier</param>
        /// <param name="Type">Type returned by the function</param>
        /// <param name="Name">Name of the function</param>
        /// <param name="Body">Body of the function</param>
        /// <param name="ParameterList">Parameter list</param>
        public virtual void AddFunction(AccessModifier AccessModifier, Modifiers Modifier, string Type,
            string Name, string Body, IParameter[] ParameterList)
        {
            Functions.Add(new Function(AccessModifier, Modifier, Type, Name, ParameterList, Body, Parser));
        }

        #endregion

        #region Properties

        protected virtual List<IProperty> Properties { get; set; }
        protected virtual List<Using> Usings { get; set; }
        protected virtual List<IFunction> Constructors { get; set; }
        protected virtual List<IFunction> Functions { get; set; }
        protected virtual string ClassName { get; set; }
        protected virtual AccessModifier AccessModifier { get; set; }
        protected virtual Modifiers Modifier { get; set; }
        protected virtual string Namespace { get; set; }

        #endregion
    }
}
