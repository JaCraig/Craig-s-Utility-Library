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
using Utilities.Validation.BaseClasses;
using Utilities.Validation.Interfaces;
using Utilities.Validation.Rules;
using Utilities.Validation.Exceptions;
using System.Linq.Expressions;
using System.Reflection;
#endregion

namespace Utilities.Validation
{
    /// <summary>
    /// Holds rules for a specific type
    /// </summary>
    public class Validator<Type>:IValidator
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public Validator()
        {
            Rules = new List<IRule<Type>>();
            System.Type ObjectType = typeof(Type);
            System.Type[] Interfaces = ObjectType.GetInterfaces();
            PropertyInfo[] Properties = ObjectType.GetProperties();
            foreach (PropertyInfo Property in Properties)
            {
                object[] Attributes = Property.GetCustomAttributes(typeof(BaseAttribute), true);
                foreach (BaseAttribute Attribute in Attributes)
                {
                    if (Attribute is Required)
                    {
                        Expression<Func<Type, object>> PropertyGetter = Utilities.Reflection.Reflection.GetPropertyGetter<Type>(Property);
                        this.Required(PropertyGetter, ((Required)Attribute).DefaultValue, Attribute.ErrorMessage);
                    }
                    else if (Attribute is Regex)
                    {
                        Expression<Func<Type, string>> PropertyGetter = Utilities.Reflection.Reflection.GetPropertyGetter<Type, string>(Property);
                        this.Regex(PropertyGetter, ((Regex)Attribute).RegexString, Attribute.ErrorMessage);
                    }
                    else if (Attribute is MaxLength)
                    {
                        Expression<Func<Type, string>> PropertyGetter = Utilities.Reflection.Reflection.GetPropertyGetter<Type, string>(Property);
                        this.MaxLength(PropertyGetter, ((MaxLength)Attribute).MaxLengthAllowed, Attribute.ErrorMessage);
                    }
                    else if (Attribute is LessThanOrEqual)
                    {
                        Expression<Func<Type, IComparable>> PropertyGetter = Utilities.Reflection.Reflection.GetPropertyGetter<Type, IComparable>(Property);
                        this.LessThanOrEqual(PropertyGetter, ((LessThanOrEqual)Attribute).Value, Attribute.ErrorMessage);
                    }
                    else if (Attribute is LessThan)
                    {
                        Expression<Func<Type, IComparable>> PropertyGetter = Utilities.Reflection.Reflection.GetPropertyGetter<Type, IComparable>(Property);
                        this.LessThan(PropertyGetter, ((LessThan)Attribute).Value, Attribute.ErrorMessage);
                    }
                    else if (Attribute is GreaterThanOrEqual)
                    {
                        Expression<Func<Type, IComparable>> PropertyGetter = Utilities.Reflection.Reflection.GetPropertyGetter<Type, IComparable>(Property);
                        this.GreaterThanOrEqual(PropertyGetter, ((GreaterThanOrEqual)Attribute).Value, Attribute.ErrorMessage);
                    }
                    else if (Attribute is GreaterThan)
                    {
                        Expression<Func<Type, IComparable>> PropertyGetter = Utilities.Reflection.Reflection.GetPropertyGetter<Type, IComparable>(Property);
                        this.GreaterThan(PropertyGetter, ((GreaterThan)Attribute).Value, Attribute.ErrorMessage);
                    }
                    else if (Attribute is Equal)
                    {
                        Expression<Func<Type, IComparable>> PropertyGetter = Utilities.Reflection.Reflection.GetPropertyGetter<Type, IComparable>(Property);
                        this.Equal(PropertyGetter, ((Equal)Attribute).Value, Attribute.ErrorMessage);
                    }
                    else if (Attribute is Cascade)
                    {
                        Expression<Func<Type, object>> PropertyGetter = Utilities.Reflection.Reflection.GetPropertyGetter<Type>(Property);
                        this.Cascade(PropertyGetter, Attribute.ErrorMessage);
                    }
                    else if (Attribute is Between)
                    {
                        Expression<Func<Type, IComparable>> PropertyGetter = Utilities.Reflection.Reflection.GetPropertyGetter<Type, IComparable>(Property);
                        this.Between(PropertyGetter, ((Between)Attribute).MinValue, ((Between)Attribute).MaxValue, Attribute.ErrorMessage);
                    }

                }
                foreach (System.Type Interface in Interfaces)
                {
                    try
                    {
                        PropertyInfo TempProperty = Interface.GetProperty(Property.Name);
                        if (TempProperty != null)
                        {
                            object[] InterfaceAttributes = TempProperty.GetCustomAttributes(typeof(BaseAttribute), true);
                            foreach (object Attribute in InterfaceAttributes)
                            {
                                if (Attribute is Required)
                                    this.Required(Utilities.Reflection.Reflection.GetPropertyGetter<Type>(Property), ((Required)Attribute).DefaultValue, ((Required)Attribute).ErrorMessage);
                            }
                        }
                    }
                    catch { }
                }
            }
        }

        #endregion

        #region Properties

        public virtual List<IRule<Type>> Rules { get; set; }

        #endregion

        #region Functions

        #region Between

        /// <summary>
        /// Adds a between rule
        /// </summary>
        /// <typeparam name="DataType">Data type that the property/function should return</typeparam>
        /// <param name="ItemToValidate">Property/Function to validate</param>
        /// <param name="ErrorMessage">Error message to throw if not valid</param>
        /// <param name="MinValue">Value that the property/function should be greater than or equal to</param>
        /// <param name="MaxValue">Value that the property/function should be less than or equal to</param>
        /// <returns>This</returns>
        public virtual Validator<Type> Between<DataType>(Expression<Func<Type, DataType>> ItemToValidate,
            DataType MinValue, DataType MaxValue, string ErrorMessage = "") where DataType : IComparable
        {
            if (string.IsNullOrEmpty(ErrorMessage))
                ErrorMessage = Utilities.Reflection.Reflection.GetPropertyName(ItemToValidate) + " is not between " + MinValue.ToString() + " and " + MaxValue.ToString();
            Rules.Add(new Between<Type, DataType>(ItemToValidate.Compile(), MinValue, MaxValue, ErrorMessage));
            return this;
        }

        #endregion

        #region Cascade

        /// <summary>
        /// Adds a cascade rule
        /// </summary>
        /// <typeparam name="DataType">Data type that the property/function should return</typeparam>
        /// <param name="ItemToValidate">Property/Function to validate</param>
        /// <param name="ErrorMessage">Error message to throw if not valid</param>
        /// <returns>This</returns>
        public virtual Validator<Type> Cascade<DataType>(Expression<Func<Type, DataType>> ItemToValidate,
            string ErrorMessage="")
        {
            if (string.IsNullOrEmpty(ErrorMessage))
                ErrorMessage = Utilities.Reflection.Reflection.GetPropertyName(ItemToValidate) + " is not valid:\n";
            Rules.Add(new Cascade<Type, DataType>(ItemToValidate.Compile(), ErrorMessage));
            return this;
        }

        #endregion

        #region Custom

        /// <summary>
        /// Adds a custom rule
        /// </summary>
        /// <typeparam name="DataType">Data type that the property/function should return</typeparam>
        /// <param name="ItemToValidate">Property/Function to validate</param>
        /// <param name="CustomRule">Custom rule to call</param>
        /// <returns>This</returns>
        public virtual Validator<Type> Custom<DataType>(Expression<Func<Type, DataType>> ItemToValidate,
            Action<DataType> CustomRule)
        {
            Rules.Add(new Custom<Type, DataType>(ItemToValidate.Compile(), CustomRule));
            return this;
        }

        #endregion

        #region Equal

        /// <summary>
        /// Adds an equal rule
        /// </summary>
        /// <typeparam name="DataType">Data type that the property/function should return</typeparam>
        /// <param name="ItemToValidate">Property/Function to validate</param>
        /// <param name="ErrorMessage">Error message to throw if not valid</param>
        /// <param name="Value">Value that the property/function should be equal to</param>
        /// <returns>This</returns>
        public virtual Validator<Type> Equal<DataType>(Expression<Func<Type, DataType>> ItemToValidate,
            DataType Value, string ErrorMessage = "")
        {
            if (string.IsNullOrEmpty(ErrorMessage))
                ErrorMessage = Utilities.Reflection.Reflection.GetPropertyName(ItemToValidate) + " is not equal to " + Value.ToString();
            Rules.Add(new Equal<Type, DataType>(ItemToValidate.Compile(), Value, ErrorMessage));
            return this;
        }

        #endregion

        #region GreaterThan

        /// <summary>
        /// Adds a greater than rule
        /// </summary>
        /// <typeparam name="DataType">Data type that the property/function should return</typeparam>
        /// <param name="ItemToValidate">Property/Function to validate</param>
        /// <param name="ErrorMessage">Error message to throw if not valid</param>
        /// <param name="MinValue">Value that the property/function should be greater than</param>
        /// <returns>This</returns>
        public virtual Validator<Type> GreaterThan<DataType>(Expression<Func<Type, DataType>> ItemToValidate,
            DataType MinValue, string ErrorMessage = "") where DataType : IComparable
        {
            if (string.IsNullOrEmpty(ErrorMessage))
                ErrorMessage = Utilities.Reflection.Reflection.GetPropertyName(ItemToValidate) + " is less than or equal to " + MinValue.ToString();
            Rules.Add(new GreaterThan<Type, DataType>(ItemToValidate.Compile(), MinValue, ErrorMessage));
            return this;
        }

        #endregion

        #region GreaterThanOrEqual

        /// <summary>
        /// Adds a greater than or equal rule
        /// </summary>
        /// <typeparam name="DataType">Data type that the property/function should return</typeparam>
        /// <param name="ItemToValidate">Property/Function to validate</param>
        /// <param name="ErrorMessage">Error message to throw if not valid</param>
        /// <param name="MinValue">Value that the property/function should be greater than or equal to</param>
        /// <returns>This</returns>
        public virtual Validator<Type> GreaterThanOrEqual<DataType>(Expression<Func<Type, DataType>> ItemToValidate,
            DataType MinValue, string ErrorMessage = "") where DataType : IComparable
        {
            if (string.IsNullOrEmpty(ErrorMessage))
                ErrorMessage = Utilities.Reflection.Reflection.GetPropertyName(ItemToValidate) + " is less than " + MinValue.ToString();
            Rules.Add(new GreaterThanOrEqual<Type, DataType>(ItemToValidate.Compile(), MinValue, ErrorMessage));
            return this;
        }

        #endregion

        #region LessThan

        /// <summary>
        /// Adds a less than rule
        /// </summary>
        /// <typeparam name="DataType">Data type that the property/function should return</typeparam>
        /// <param name="ItemToValidate">Property/Function to validate</param>
        /// <param name="ErrorMessage">Error message to throw if not valid</param>
        /// <param name="MaxValue">Value that the property/function should be less than</param>
        /// <returns>This</returns>
        public virtual Validator<Type> LessThan<DataType>(Expression<Func<Type, DataType>> ItemToValidate,
            DataType MaxValue, string ErrorMessage = "") where DataType : IComparable
        {
            if (string.IsNullOrEmpty(ErrorMessage))
                ErrorMessage = Utilities.Reflection.Reflection.GetPropertyName(ItemToValidate) + " is greater than or equal to " + MaxValue.ToString();
            Rules.Add(new LessThan<Type, DataType>(ItemToValidate.Compile(), MaxValue, ErrorMessage));
            return this;
        }

        #endregion

        #region LessThanOrEqual

        /// <summary>
        /// Adds a less than or equal rule
        /// </summary>
        /// <typeparam name="DataType">Data type that the property/function should return</typeparam>
        /// <param name="ItemToValidate">Property/Function to validate</param>
        /// <param name="ErrorMessage">Error message to throw if not valid</param>
        /// <param name="MaxValue">Value that the property/function should be less than or equal to</param>
        /// <returns>This</returns>
        public virtual Validator<Type> LessThanOrEqual<DataType>(Expression<Func<Type, DataType>> ItemToValidate,
            DataType MaxValue, string ErrorMessage = "") where DataType : IComparable
        {
            if (string.IsNullOrEmpty(ErrorMessage))
                ErrorMessage = Utilities.Reflection.Reflection.GetPropertyName(ItemToValidate) + " is greater than " + MaxValue.ToString();
            Rules.Add(new LessThanOrEqual<Type, DataType>(ItemToValidate.Compile(), MaxValue, ErrorMessage));
            return this;
        }

        #endregion

        #region MaxLength

        /// <summary>
        /// Adds a max length rule
        /// </summary>
        /// <param name="MaxLength">Maximum length of the string</param>
        /// <param name="ItemToValidate">Property/Function to validate</param>
        /// <param name="ErrorMessage">Error message to throw if not valid</param>
        /// <returns>This</returns>
        public virtual Validator<Type> MaxLength(Expression<Func<Type, string>> ItemToValidate, int MaxLength, string ErrorMessage = "")
        {
            if (string.IsNullOrEmpty(ErrorMessage))
                ErrorMessage = Utilities.Reflection.Reflection.GetPropertyName(ItemToValidate) + "'s length is greater than " + MaxLength.ToString() + " characters";
            Rules.Add(new MaxLength<Type>(ItemToValidate.Compile(), MaxLength, ErrorMessage));
            return this;
        }

        #endregion

        #region Regex

        /// <summary>
        /// Adds a regex rule
        /// </summary>
        /// <param name="RegexString">Regex string used for validation</param>
        /// <param name="ItemToValidate">Property/Function to validate</param>
        /// <param name="ErrorMessage">Error message to throw if not valid</param>
        /// <returns>This</returns>
        public virtual Validator<Type> Regex(Expression<Func<Type, string>> ItemToValidate, string RegexString, string ErrorMessage = "")
        {
            if (string.IsNullOrEmpty(ErrorMessage))
                ErrorMessage = Utilities.Reflection.Reflection.GetPropertyName(ItemToValidate) + " does not match";
            Rules.Add(new Regex<Type>(ItemToValidate.Compile(), RegexString, ErrorMessage));
            return this;
        }

        #endregion

        #region Required

        /// <summary>
        /// Adds a required rule
        /// </summary>
        /// <typeparam name="DataType">Data type that the property/function should return</typeparam>
        /// <param name="ItemToValidate">Property/Function to validate</param>
        /// <param name="ErrorMessage">Error message to throw if not valid</param>
        /// <param name="DefaultValue">Default value</param>
        /// <returns>This</returns>
        public virtual Validator<Type> Required<DataType>(Expression<Func<Type, DataType>> ItemToValidate,
            DataType DefaultValue=default(DataType), string ErrorMessage = "")
        {
            if (string.IsNullOrEmpty(ErrorMessage))
                ErrorMessage = Utilities.Reflection.Reflection.GetPropertyName(ItemToValidate) + " is required";
            Rules.Add(new Required<Type, DataType>(ItemToValidate.Compile(), DefaultValue, ErrorMessage));
            return this;
        }

        /// <summary>
        /// Adds a required rule
        /// </summary>
        /// <param name="ItemToValidate">Property/Function to validate</param>
        /// <param name="ErrorMessage">Error message to throw if not valid</param>
        /// <returns>This</returns>
        public virtual Validator<Type> Required(Expression<Func<Type, string>> ItemToValidate, 
            string DefaultValue = null, string ErrorMessage = "")
        {
            if (string.IsNullOrEmpty(ErrorMessage))
                ErrorMessage = Utilities.Reflection.Reflection.GetPropertyName(ItemToValidate) + " is required";
            Rules.Add(new RequiredString<Type>(ItemToValidate.Compile(), DefaultValue, ErrorMessage));
            return this;
        }

        #endregion

        #region Validate

        public virtual void Validate(object Object)
        {
            Validate((Type)Object);
        }

        /// <summary>
        /// Validates an object
        /// </summary>
        /// <param name="Object">Object to validate</param>
        public virtual void Validate(Type Object)
        {
            StringBuilder Builder=new StringBuilder();
            foreach (IRule<Type> Rule in Rules)
            {
                try
                {
                    Rule.Validate(Object);
                }
                catch (Exception e) { Builder.AppendLine(e.Message); }
            }
            if (Builder.Length > 0)
                throw new NotValid(Builder.ToString());
        }

        #endregion

        #endregion
    }
}