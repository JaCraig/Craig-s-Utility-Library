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
    /// Constructor class
    /// </summary>
    public class Constructor:ObjectBase,IFunction
    {
        #region Constructor

        public Constructor(AccessModifier AccessModifier, string ClassName,IParameter[] ParameterList,string Body, IParser Parser)
            : base(Parser)
        {
            this.AccessModifier = AccessModifier;
            this.ClassName = ClassName;
            this.Parameters = ParameterList.ToList();
            this.Body = Body;
        }

        #endregion

        #region Functions

        protected override void SetupTemplate()
        {
            Template = new DefaultTemplate(@"@AccessModifier @ClassName(@ParameterList)
{
@Body
}");
        }

        protected override void SetupInput()
        {
            Input.Values.Add("AccessModifier", AccessModifier.ToString());
            Input.Values.Add("ClassName", ClassName);
            Input.Values.Add("Body", Body);
            string TempString = "";
            string Splitter="";
            foreach (IParameter Parameter in Parameters)
            {
                TempString += Splitter + Parameter.Transform();
                Splitter = ",";
            }
            Input.Values.Add("ParameterList", TempString);
        }

        #endregion

        #region Properties

        protected virtual AccessModifier AccessModifier { get; set; }
        protected virtual string ClassName { get; set; }
        protected virtual List<IParameter> Parameters{get;set;}
        protected virtual string Body { get; set; }

        #endregion
    }
}
