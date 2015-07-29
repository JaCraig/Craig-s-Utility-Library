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

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using Utilities.DataTypes;
using Utilities.DataTypes.Comparison;

namespace Utilities.Validation
{
    /// <summary>
    /// CompareTo attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class CompareToAttribute : ValidationAttribute, IClientValidatable
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="PropertyName">Property to compare to</param>
        /// <param name="Type">Comparison type to use</param>
        /// <param name="ErrorMessage">Error message</param>
        public CompareToAttribute(string PropertyName, ComparisonType Type, string ErrorMessage = "")
            : base(string.IsNullOrEmpty(ErrorMessage) ? "{0} is not {1} {2}" : ErrorMessage)
        {
            this.PropertyName = PropertyName;
            this.Type = Type;
        }

        /// <summary>
        /// Property to compare to
        /// </summary>
        public string PropertyName { get; private set; }

        /// <summary>
        /// Comparison type
        /// </summary>
        public ComparisonType Type { get; private set; }

        /// <summary>
        /// Formats the error message
        /// </summary>
        /// <param name="name">Property name</param>
        /// <returns>The formatted string</returns>
        public override string FormatErrorMessage(string name)
        {
            string ComparisonTypeString = "";
            switch (Type)
            {
                case ComparisonType.Equal:
                    ComparisonTypeString = "equal";
                    break;
                case ComparisonType.GreaterThan:
                    ComparisonTypeString = "greater than";
                    break;
                case ComparisonType.GreaterThanOrEqual:
                    ComparisonTypeString = "greater than or equal";
                    break;
                case ComparisonType.LessThan:
                    ComparisonTypeString = "less than";
                    break;
                case ComparisonType.LessThanOrEqual:
                    ComparisonTypeString = "less than or equal";
                    break;
                case ComparisonType.NotEqual:
                    ComparisonTypeString = "not equal";
                    break;
            }

            return string.Format(CultureInfo.InvariantCulture, ErrorMessageString, name, ComparisonTypeString, PropertyName);
        }

        /// <summary>
        /// Gets the client side validation rules
        /// </summary>
        /// <param name="metadata">Model meta data</param>
        /// <param name="context">Controller context</param>
        /// <returns>The list of client side validation rules</returns>
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var Rule = new ModelClientValidationRule();
            Rule.ErrorMessage = FormatErrorMessage(metadata.GetDisplayName());
            Rule.ValidationParameters.Add("Type", Type);
            Rule.ValidationParameters.Add("PropertyName", PropertyName);
            Rule.ValidationType = "CompareTo";
            return new ModelClientValidationRule[] { Rule };
        }

        /// <summary>
        /// Determines if the property is valid
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="validationContext">Validation context</param>
        /// <returns>The validation result</returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var Tempvalue = value as IComparable;
            var Comparer = new GenericComparer<IComparable>();
            var ComparisonValue = (IComparable)validationContext.ObjectType.GetProperty(PropertyName).GetValue(validationContext.ObjectInstance, null).To<object>(value.GetType());
            switch (Type)
            {
                case ComparisonType.Equal:
                    return Comparer.Compare(Tempvalue, ComparisonValue) == 0 ? ValidationResult.Success : new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
                case ComparisonType.NotEqual:
                    return Comparer.Compare(Tempvalue, ComparisonValue) != 0 ? ValidationResult.Success : new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
                case ComparisonType.GreaterThan:
                    return Comparer.Compare(Tempvalue, ComparisonValue) > 0 ? ValidationResult.Success : new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
                case ComparisonType.GreaterThanOrEqual:
                    return Comparer.Compare(Tempvalue, ComparisonValue) >= 0 ? ValidationResult.Success : new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
                case ComparisonType.LessThan:
                    return Comparer.Compare(Tempvalue, ComparisonValue) < 0 ? ValidationResult.Success : new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
                case ComparisonType.LessThanOrEqual:
                    return Comparer.Compare(Tempvalue, ComparisonValue) <= 0 ? ValidationResult.Success : new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
                default:
                    return ValidationResult.Success;
            }
        }
    }
}