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

#endregion

namespace Utilities.FileFormats.FixedLength.Interfaces
{
    /// <summary>
    /// Field interface
    /// </summary>
    public interface IField<T>
    {
        #region Functions

        /// <summary>
        /// Parses the string into fields
        /// </summary>
        /// <param name="Value">The string value</param>
        void Parse(string Value);

        /// <summary>
        /// Parses the string into fields
        /// </summary>
        /// <param name="Value">The string value</param>
        /// <param name="Length">Max length of the value</param>
        void Parse(string Value,int Length);

        /// <summary>
        /// Parses the string into fields
        /// </summary>
        /// <param name="Value">The string value</param>
        /// <param name="Length">Max length of the value</param>
        /// <param name="FillerCharacter">Filler character used to fill
        /// the rest of the field</param>
        void Parse(string Value, int Length, string FillerCharacter);

        #endregion

        #region Properties

        /// <summary>
        /// Field length
        /// </summary>
        int Length { get; set; }

        /// <summary>
        /// Field value
        /// </summary>
        T Value { get; set; }

        #endregion
    }
}
