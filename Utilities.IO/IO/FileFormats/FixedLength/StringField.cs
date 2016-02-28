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

using System.Text;
using Utilities.DataTypes;
using Utilities.IO.FileFormats.FixedLength.BaseClasses;

namespace Utilities.IO.FileFormats.FixedLength
{
    /// <summary>
    /// Field containing string info (used in text based files)
    /// </summary>
    public class StringField : FieldBase<string>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Value">Value to place in the field</param>
        /// <param name="Length">Max length of the value</param>
        public StringField(string Value = "", int Length = -1)
        {
            Parse(Value, Length);
        }

        /// <summary>
        /// Parses the string field
        /// </summary>
        /// <param name="Value">Value</param>
        /// <param name="Length">Max length allowed</param>
        /// <param name="FillerCharacter">Filler character</param>
        public override void Parse(string Value, int Length = -1, string FillerCharacter = " ")
        {
            this.Value = Value;
            this.Length = Length >= 0 ? Length : Value.Length;
            if (Value.Length > this.Length)
            {
                this.Value = Value.Left(Length);
                return;
            }
            var Builder = new StringBuilder();
            Builder.Append(Value);
            while (Builder.Length < this.Length)
                Builder.Append(FillerCharacter);
            this.Value = Builder.ToString();
        }
    }
}