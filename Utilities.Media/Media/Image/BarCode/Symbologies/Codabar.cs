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
using Utilities.Media.Image.BarCode.Interfaces;
#endregion

namespace Utilities.Media.Image.BarCode.Symbologies
{
    /// <summary>
    /// Codabar
    /// </summary>
    public class Codabar : ISymbology
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Data">Data to encode</param>
        public Codabar(string Data)
        {
            Init();
            this.Input = Data.Trim();
        }

        #endregion

        #region Variables
        private System.Collections.Hashtable Table = new System.Collections.Hashtable();
        #endregion

        #region Properties

        public string Input { get; set; }

        #endregion

        #region Functions

        public string Encode()
        {
            if (Input.Length < 2)
                throw new Exception("Input too short");

            switch (Input[0].ToString().ToUpper())
            {
                case "A": break;
                case "B": break;
                case "C": break;
                case "D": break;
                default: throw new Exception("Invalid starting character");
            }

            switch (Input[Input.Trim().Length - 1].ToString().ToUpper())
            {
                case "A": break;
                case "B": break;
                case "C": break;
                case "D": break;
                default: throw new Exception("Invalid ending character");
            }

            string Result = "";
            foreach (char Char in Input)
            {
                Result += Table[Char];
                Result += "0";
            }
            return Result.Remove(Result.Length - 1);
        }

        private void Init()
        {
            Table.Clear();
            Table.Add('0', "101010011");
            Table.Add('1', "101011001");
            Table.Add('2', "101001011");
            Table.Add('3', "110010101");
            Table.Add('4', "101101001");
            Table.Add('5', "110101001");
            Table.Add('6', "100101011");
            Table.Add('7', "100101101");
            Table.Add('8', "100110101");
            Table.Add('9', "110100101");
            Table.Add('-', "101001101");
            Table.Add('$', "101100101");
            Table.Add(':', "1101011011");
            Table.Add('/', "1101101011");
            Table.Add('.', "1101101101");
            Table.Add('+', "101100110011");
            Table.Add('A', "1011001001");
            Table.Add('B', "1010010011");
            Table.Add('C', "1001001011");
            Table.Add('D', "1010011001");
            Table.Add('a', "1011001001");
            Table.Add('b', "1010010011");
            Table.Add('c', "1001001011");
            Table.Add('d', "1010011001");
        }

        #endregion
    }
}