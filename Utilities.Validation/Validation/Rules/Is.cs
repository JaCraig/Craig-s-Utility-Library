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
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Utilities.DataTypes.ExtensionMethods;
using Utilities.Validation.Rules.Enums;
using System.Globalization;
#endregion

namespace Utilities.Validation.Rules
{
    /// <summary>
    /// Is attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class IsAttribute : ValidationAttribute, IClientValidatable
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Type">Validation type enum</param>
        /// <param name="ErrorMessage">Error message</param>
        public IsAttribute(IsValid Type, string ErrorMessage = "")
            : base(string.IsNullOrEmpty(ErrorMessage) ? "{0} is not {1}" : ErrorMessage)
        {
            this.Type = Type;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Type of validation to do
        /// </summary>
        public IsValid Type { get;private set; }

        #endregion

        #region Functions

        /// <summary>
        /// Formats the error message
        /// </summary>
        /// <param name="name">Property name</param>
        /// <returns>The formatted string</returns>
        public override string FormatErrorMessage(string name)
        {
            string ComparisonString = "";
            if (Type == Enums.IsValid.CreditCard)
                ComparisonString = "a credit card";
            else if (Type == Enums.IsValid.Decimal)
                ComparisonString = "a decimal";
            else if (Type == Enums.IsValid.Domain)
                ComparisonString = "a domain";
            else if (Type == Enums.IsValid.Integer)
                ComparisonString = "an integer";
            return string.Format(CultureInfo.InvariantCulture, ErrorMessageString, name, ComparisonString);
        }

        /// <summary>
        /// Determines if the property is valid
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="validationContext">Validation context</param>
        /// <returns>The validation result</returns>
        protected override System.ComponentModel.DataAnnotations.ValidationResult IsValid(object value, System.ComponentModel.DataAnnotations.ValidationContext validationContext)
        {
            string Tempvalue = value as string;
            if(Type==Enums.IsValid.CreditCard)
                return Tempvalue.Is(StringCompare.CreditCard) ? ValidationResult.Success : new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            else if(Type==Enums.IsValid.Decimal)
                return Regex.IsMatch(Tempvalue, @"^(\d+)+(\.\d+)?$|^(\d+)?(\.\d+)+$") ? ValidationResult.Success : new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            else if (Type == Enums.IsValid.Domain)
                return Regex.IsMatch(Tempvalue, @"^(http|https|ftp)://([a-zA-Z0-9_-]*(?:\.[a-zA-Z0-9_-]*)+):?([0-9]+)?/?") ? ValidationResult.Success : new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            else if (Type == Enums.IsValid.Integer)
                return Regex.IsMatch(Tempvalue, @"^\d+$") ? ValidationResult.Success : new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
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
            Rule.ValidationParameters.Add("Type", Type.ToString());
            Rule.ValidationType = "Is";
            return new ModelClientValidationRule[] { Rule };
        }

        #endregion
    }
}