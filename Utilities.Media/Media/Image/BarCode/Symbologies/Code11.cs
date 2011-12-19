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
    /// Code 11
    /// </summary>
    public class Code11 : ISymbology
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Data">Input data</param>
        public Code11(string Data)
        {
            Input = Data;
        }

        #endregion

        #region Functions

        public string Encode()
        {
            int Weight = 1;
            int Total = 0;
            string ReturnValue = Input;

            for (int x = Input.Length - 1; x >= 0; --x)
            {
                if (Weight == 10)
                    Weight = 1;
                if (Input[x] != '-')
                    Total += Int32.Parse(Input[x].ToString()) * Weight++;
                else
                    Total += 10 * Weight++;
            }
            int CheckSum = Total % 11;
            ReturnValue += CheckSum.ToString();

            if (Input.Length >= 1)
            {
                Weight = 1;
                Total = 0;
                for (int x = ReturnValue.Length - 1; x >= 0; x--)
                {
                    if (Weight == 9)
                        Weight = 1;
                    if (ReturnValue[x] != '-')
                        Total += Int32.Parse(ReturnValue[x].ToString()) * Weight++;
                    else
                        Total += 10 * Weight++;
                }
                CheckSum = Total % 11;
                ReturnValue += CheckSum.ToString();
            }

            string Space = "0";
            string FinalResult = Codes[11] + Space;

            foreach (char Char in ReturnValue)
            {
                int Index = (Char == '-' ? 10 : Int32.Parse(Char.ToString()));
                FinalResult += Codes[Index];
                FinalResult += Space;
            }
            FinalResult += Codes[11];
            return FinalResult;
        }

        #endregion

        #region Properties

        public string Input { get; set; }

        #endregion

        #region Variables

        private string[] Codes = { "101011", "1101011", "1001011", "1100101", "1011011", "1101101", "1001101", "1010011", "1101001", "110101", "101101", "1011001" };

        #endregion
    }
}