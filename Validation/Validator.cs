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
#endregion

namespace Utilities.Validation
{
    /// <summary>
    /// Holds rules for a specific type
    /// </summary>
    public class Validator<Type>
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public Validator()
        {
            Rules = new List<IRule<Type>>();
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
        public virtual Validator<Type> Between<DataType>(Func<Type, DataType> ItemToValidate,
            DataType MinValue, DataType MaxValue, string ErrorMessage = "Object is not valid") where DataType : IComparable
        {
            Rules.Add(new Between<Type, DataType>(ItemToValidate, MinValue, MaxValue, ErrorMessage));
            return this;
        }

        #endregion

        #region Custom

        /// <summary>
        /// Adds a custom rule
        /// </summary>
        /// <typeparam name="DataType">Data type that the property/function should return</typeparam>
        /// <param name="ItemToValidate">Property/Function to validate</param>
        /// <param name="ErrorMessage">Error message to throw if not valid</param>
        /// <param name="CustomRule">Custom rule to call</param>
        /// <returns>This</returns>
        public virtual Validator<Type> Custom<DataType>(Func<Type, DataType> ItemToValidate,
            Action<DataType> CustomRule, string ErrorMessage = "Object is not valid")
        {
            Rules.Add(new Custom<Type, DataType>(ItemToValidate, CustomRule, ErrorMessage));
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
        public virtual Validator<Type> Equal<DataType>(Func<Type, DataType> ItemToValidate,
            DataType Value, string ErrorMessage = "Object is not valid")
        {
            Rules.Add(new Equal<Type, DataType>(ItemToValidate, Value, ErrorMessage));
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
        public virtual Validator<Type> GreaterThan<DataType>(Func<Type, DataType> ItemToValidate,
            DataType MinValue, string ErrorMessage = "Object is not valid") where DataType : IComparable
        {
            Rules.Add(new GreaterThan<Type, DataType>(ItemToValidate, MinValue, ErrorMessage));
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
        public virtual Validator<Type> GreaterThanOrEqual<DataType>(Func<Type, DataType> ItemToValidate,
            DataType MinValue, string ErrorMessage = "Object is not valid") where DataType : IComparable
        {
            Rules.Add(new GreaterThanOrEqual<Type, DataType>(ItemToValidate, MinValue, ErrorMessage));
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
        public virtual Validator<Type> LessThan<DataType>(Func<Type, DataType> ItemToValidate,
            DataType MaxValue, string ErrorMessage = "Object is not valid") where DataType : IComparable
        {
            Rules.Add(new LessThan<Type, DataType>(ItemToValidate, MaxValue, ErrorMessage));
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
        public virtual Validator<Type> LessThanOrEqual<DataType>(Func<Type, DataType> ItemToValidate,
            DataType MaxValue, string ErrorMessage = "Object is not valid") where DataType : IComparable
        {
            Rules.Add(new LessThanOrEqual<Type, DataType>(ItemToValidate, MaxValue, ErrorMessage));
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
        public virtual Validator<Type> MaxLength(Func<Type, string> ItemToValidate, int MaxLength, string ErrorMessage = "Object is not valid")
        {
            Rules.Add(new MaxLength<Type>(ItemToValidate, MaxLength, ErrorMessage));
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
        public virtual Validator<Type> Regex(Func<Type, string> ItemToValidate, string RegexString, string ErrorMessage = "Object is not valid")
        {
            Rules.Add(new Regex<Type>(ItemToValidate, RegexString, ErrorMessage));
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
        public virtual Validator<Type> Required<DataType>(Func<Type, DataType> ItemToValidate,
            DataType DefaultValue=default(DataType), string ErrorMessage = "Object is not valid")
        {
            Rules.Add(new Required<Type, DataType>(ItemToValidate, DefaultValue, ErrorMessage));
            return this;
        }

        /// <summary>
        /// Adds a required rule
        /// </summary>
        /// <param name="ItemToValidate">Property/Function to validate</param>
        /// <param name="ErrorMessage">Error message to throw if not valid</param>
        /// <returns>This</returns>
        public virtual Validator<Type> Required(Func<Type, string> ItemToValidate, 
            string DefaultValue = null, string ErrorMessage = "Object is not valid")
        {
            Rules.Add(new RequiredString<Type>(ItemToValidate, DefaultValue, ErrorMessage));
            return this;
        }

        #endregion

        #region Validate

        public virtual void Validate(Type Object)
        {
            foreach (IRule<Type> Rule in Rules)
                Rule.Validate(Object);
        }

        #endregion

        #endregion
    }
}