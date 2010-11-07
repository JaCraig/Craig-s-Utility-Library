/*
Copyright (c) 2010 <a href="http://www.gutgames.com">James Craig</a>

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
using Utilities.Reflection.Emit.Interfaces;
using System.Reflection;
using System.Reflection.Emit;
#endregion

namespace Utilities.Reflection.Emit.Commands
{
    /// <summary>
    /// Command for creating a new object
    /// </summary>
    public class NewObj:ICommand
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Method">Method created within</param>
        /// <param name="Constructor">Constructor to use</param>
        /// <param name="Variables">Variables sent to the constructor</param>
        /// <param name="ObjectCount">Object count</param>
        /// <param name="Generator">IL generator</param>
        public NewObj(IMethodBuilder Method,ConstructorInfo Constructor, List<IVariable> Variables,
            int ObjectCount,ILGenerator Generator)
        {
            this.Constructor = Constructor;
            this.Variables = Variables;
            foreach (IVariable Variable in Variables)
            {
                Variable.Load(Generator);
            }
            Generator.Emit(OpCodes.Newobj, Constructor);
            TempObject = new ObjectBuilder(Method, Constructor.DeclaringType, ObjectCount);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Constructor used
        /// </summary>
        protected ConstructorInfo Constructor { get; set; }

        /// <summary>
        /// Method object is created within
        /// </summary>
        protected IMethodBuilder Method { get; set; }

        /// <summary>
        /// Variables sent to the Constructor
        /// </summary>
        protected List<IVariable> Variables { get; set; }

        /// <summary>
        /// Temp object
        /// </summary>
        protected IVariable TempObject{get;set;}

        /// <summary>
        /// Object count
        /// </summary>
        protected int ObjectCount { get; set; }

        #endregion

        #region Functions

        /// <summary>
        /// Gets the object that temporarily stores the new object
        /// </summary>
        /// <returns>The new object</returns>
        public virtual IVariable GetObject()
        {
            return TempObject;
        }

        #endregion

        #region Overridden Functions

        public override string ToString()
        {
            StringBuilder Output = new StringBuilder();
            Output.Append(Reflection.GetTypeName(Constructor.DeclaringType))
                .Append(" ").Append(TempObject).Append(" = new ")
                .Append(Reflection.GetTypeName(Constructor.DeclaringType))
                .Append("(");
            string Seperator = "";
            foreach (IVariable Variable in Variables)
            {
                Output.Append(Seperator).Append(Variable.ToString());
                Seperator = ",";
            }
            Output.Append(");\n");
            return Output.ToString();
        }

        #endregion
    }
}
