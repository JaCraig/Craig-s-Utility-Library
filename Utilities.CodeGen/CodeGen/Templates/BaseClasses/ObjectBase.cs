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
using Utilities.CodeGen.Interfaces;
using Utilities.CodeGen.Templates.Interfaces;
#endregion

namespace Utilities.CodeGen.Templates.BaseClasses
{
    /// <summary>
    /// Object base class
    /// </summary>
    public abstract class ObjectBase:IObject
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public ObjectBase()
        {
            Input = new DefaultInput();
            Parser = new DefaultParser();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Parser">Parser to use</param>
        public ObjectBase(IParser Parser)
        {
            Input = new DefaultInput();
            this.Parser = Parser;
        }

        #endregion

        #region Functions

        /// <summary>
        /// Transforms the template
        /// </summary>
        /// <returns>The transformed template</returns>
        public virtual string Transform()
        {
            if(Template==null)
                SetupTemplate();
            SetupInput();
            return Parser.Transform(Template, Input);
        }

        /// <summary>
        /// Sets up the template
        /// </summary>
        protected abstract void SetupTemplate();

        /// <summary>
        /// Sets up the input
        /// </summary>
        protected abstract void SetupInput();

        #endregion

        #region Properties

        /// <summary>
        /// Input
        /// </summary>
        public virtual IInput Input { get; set; }

        /// <summary>
        /// Template being used
        /// </summary>
        public virtual ITemplate Template { get; set; }

        /// <summary>
        /// Template parser
        /// </summary>
        public virtual IParser Parser { get; set; }

        #endregion
    }
}
