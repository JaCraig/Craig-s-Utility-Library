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
using System.Collections.Generic;
using System.Linq;
using Utilities.CodeGen.Interfaces;
using Utilities.CodeGen.Templates.BaseClasses;
using Utilities.CodeGen.Templates.Enums;
using Utilities.CodeGen.Templates.Interfaces;
#endregion

namespace Utilities.CodeGen.Templates
{
    /// <summary>
    /// Function class
    /// </summary>
    public class Function : ObjectBase, IFunction
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="AccessModifier">Access modifier</param>
        /// <param name="Modifier">Modifier</param>
        /// <param name="Type">Type</param>
        /// <param name="FunctionName">Function name</param>
        /// <param name="ParameterList">Parameter list</param>
        /// <param name="Body">Body of the function</param>
        /// <param name="Parser">Parser to use</param>
        public Function(AccessModifier AccessModifier, Modifiers Modifier, string Type,
            string FunctionName, IParameter[] ParameterList, string Body, IParser Parser)
            : base(Parser)
        {
            this.AccessModifier = AccessModifier;
            this.Modifier = Modifier;
            this.Type = Type;
            this.Name = FunctionName;
            this.Parameters = ParameterList.ToList();
            this.Body = Body;
        }

        #endregion

        #region Functions

        /// <summary>
        /// Sets up the template
        /// </summary>
        protected override void SetupTemplate()
        {
            Template = new DefaultTemplate(@"@AccessModifier @Modifier @Type @FunctionName(@ParameterList)
{
@Body
}");
        }

        /// <summary>
        /// Sets up the input
        /// </summary>
        protected override void SetupInput()
        {
            Input.Values.Add("AccessModifier", AccessModifier.ToString().ToLower());
            if (Modifier != Modifiers.None)
                Input.Values.Add("Modifier", Modifier.ToString().ToLower());
            else
                Input.Values.Add("Modifier", "");
            Input.Values.Add("Type", Type);
            Input.Values.Add("FunctionName", Name);
            Input.Values.Add("Body", Body);
            string TempString = "";
            string Splitter = "";
            foreach (IParameter Parameter in Parameters)
            {
                TempString += Splitter + Parameter.Transform();
                Splitter = ",";
            }
            Input.Values.Add("ParameterList", TempString);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Access modifier
        /// </summary>
        protected virtual AccessModifier AccessModifier { get; set; }

        /// <summary>
        /// Modifier
        /// </summary>
        protected virtual Modifiers Modifier { get; set; }

        /// <summary>
        /// Function name
        /// </summary>
        protected virtual string Name { get; set; }

        /// <summary>
        /// Function return type
        /// </summary>
        protected virtual string Type { get; set; }

        /// <summary>
        /// Parameters for the function
        /// </summary>
        protected virtual List<IParameter> Parameters { get; set; }

        /// <summary>
        /// Body of the function
        /// </summary>
        protected virtual string Body { get; set; }

        #endregion
    }
}