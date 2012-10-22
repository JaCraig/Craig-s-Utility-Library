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
using System.Reflection.Emit;
using Utilities.Reflection.Emit.BaseClasses;
#endregion

namespace Utilities.Reflection.Emit.Commands
{
    /// <summary>
    /// End While command
    /// </summary>
    public class EndWhile : CommandBase
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="WhileCommand">While command</param>
        public EndWhile(While WhileCommand)
            : base()
        {
            this.WhileCommand = WhileCommand;
        }

        #endregion

        #region Properties

        /// <summary>
        /// While command
        /// </summary>
        protected virtual While WhileCommand { get; set; }

        #endregion

        #region Functions

        /// <summary>
        /// Sets up the command
        /// </summary>
        public override void Setup()
        {
            Utilities.Reflection.Emit.BaseClasses.MethodBase.CurrentMethod.Generator.Emit(OpCodes.Br, WhileCommand.StartWhileLabel);
            Utilities.Reflection.Emit.BaseClasses.MethodBase.CurrentMethod.Generator.MarkLabel(WhileCommand.EndWhileLabel);
        }

        /// <summary>
        /// Converts the command to a string
        /// </summary>
        /// <returns>The string version of the command</returns>
        public override string ToString()
        {
            return "}\n";
        }

        #endregion
    }
}