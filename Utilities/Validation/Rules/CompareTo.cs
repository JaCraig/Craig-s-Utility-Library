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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Utilities.DataTypes.Comparison;
using Utilities.DataTypes.ExtensionMethods;
using Utilities.Validation.Rules.Enums;
using System.Globalization;
#endregion

namespace Utilities.Validation.Rules
{
    /// <summary>
    /// CompareTo attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class CompareToAttribute : ValidationAttribute, IClientValidatable
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="PropertyName">Property to compare to</param>
        /// <param name="Type">Comparison type to use</param>
        /// <param name="ErrorMessage">Error message</param>
        public CompareToAttribute(string PropertyName, ComparisonType Type, string ErrorMessage = "")
            : base(ErrorMessage.IsNullOrEmpty() ? "{0} is not {1} {2}" : ErrorMessage)
        {
            this.PropertyName = PropertyName;
            this.Type = Type;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property to compare to
        /// </summary>
        public string PropertyName { get;private set; }

        /// <summary>
        /// Comparison type
        /// </summary>
        public ComparisonType Type { get;private set; }

        #endregion

        #region Functions

        /// <summary>
        /// Formats the error message
        /// </summary>
        /// <param name="name">Property name</param>
        /// <returns>The formatted string</returns>
        public override string FormatErrorMessage(string name)
        {
            string ComparisonTypeString = "";
            if (Type == ComparisonType.Equal)
                ComparisonTypeString = "equal";
            else if (Type == ComparisonType.GreaterThan)
                ComparisonTypeString = "greater than";
            else if (Type == ComparisonType.GreaterThanOrEqual)
                ComparisonTypeString = "greater than or equal";
            else if (Type == ComparisonType.LessThan)
                ComparisonTypeString = "less than";
            else if (Type == ComparisonType.LessThanOrEqual)
                ComparisonTypeString = "less than or equal";
            else if (Type == ComparisonType.NotEqual)
                ComparisonTypeString = "not equal";
            return string.Format(CultureInfo.InvariantCulture, ErrorMessageString, name, ComparisonTypeString, PropertyName);
        }

        /// <summary>
        /// Determines if the property is valid
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="validationContext">Validation context</param>
        /// <returns>The validation result</returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            IComparable Tempvalue = value as IComparable;
            GenericComparer<IComparable> Comparer = new GenericComparer<IComparable>();
            IComparable ComparisonValue = (IComparable)validationContext.ObjectType.GetProperty(PropertyName).GetValue(validationContext.ObjectInstance, null).TryTo<object>(value.GetType());
            if (Type == ComparisonType.Equal)
                return Comparer.Compare(Tempvalue, ComparisonValue) == 0 ? ValidationResult.Success : new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            else if (Type == ComparisonType.NotEqual)
                return Comparer.Compare(Tempvalue, ComparisonValue) != 0 ? ValidationResult.Success : new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            else if (Type == ComparisonType.GreaterThan)
                return Comparer.Compare(Tempvalue, ComparisonValue) > 0 ? ValidationResult.Success : new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            else if (Type == ComparisonType.GreaterThanOrEqual)
                return Comparer.Compare(Tempvalue, ComparisonValue) >= 0 ? ValidationResult.Success : new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            else if (Type == ComparisonType.LessThan)
                return Comparer.Compare(Tempvalue, ComparisonValue) < 0 ? ValidationResult.Success : new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            else if (Type == ComparisonType.LessThanOrEqual)
                return Comparer.Compare(Tempvalue, ComparisonValue) <= 0 ? ValidationResult.Success : new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            else
                return ValidationResult.Success;
        }


        /// <summary>
        /// Gets the client side validation rules
        /// </summary>
        /// <param name="metadata">Model meta data</param>
        /// <param name="context">Controller context</param>
        /// <returns>The list of client side validation rules</returns>
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            ModelClientValidationRule Rule = new ModelClientValidationRule();
            Rule.ErrorMessage = FormatErrorMessage(metadata.GetDisplayName());
            Rule.ValidationParameters.Add("Type", Type);
            Rule.ValidationParameters.Add("PropertyName", PropertyName);
            Rule.ValidationType = "CompareTo";
            return new ModelClientValidationRule[] { Rule };
        }

        #endregion
    }
}