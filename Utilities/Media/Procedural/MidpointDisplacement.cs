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

#region Usings

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Drawing;

#endregion Usings

namespace Utilities.Media.Procedural
{
    /// <summary>
    /// Helper class for generating cracks by midpoint displacement
    /// </summary>
    public static class MidpointDisplacement
    {
        #region Functions

        /// <summary>
        /// Generates an image that contains cracks
        /// </summary>
        /// <param name="Width">Image width</param>
        /// <param name="Height">Image height</param>
        /// <param name="NumberOfCracks">Number of cracks</param>
        /// <param name="Iterations">Iterations</param>
        /// <param name="MaxChange">Maximum height change of the cracks</param>
        /// <param name="MaxLength">Maximum length of the cracks</param>
        /// <param name="Seed">Random seed</param>
        /// <returns>An image containing "cracks"</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        public static Bitmap Generate(int Width, int Height, int NumberOfCracks, int Iterations,
            int MaxChange, int MaxLength, int Seed)
        {
            Contract.Requires<ArgumentException>(NumberOfCracks >= 0, "Number of cracks should be greater than 0");
            Bitmap ReturnValue = new Bitmap(Width, Height);
            List<Line> Lines = GenerateLines(Width, Height, NumberOfCracks, Iterations, MaxChange, MaxLength, Seed);
            using (Graphics ReturnGraphic = Graphics.FromImage(ReturnValue))
            {
                foreach (Line Line in Lines)
                {
                    foreach (Line SubLine in Line.SubLines)
                    {
                        ReturnGraphic.DrawLine(Pens.White, SubLine.X1, SubLine.Y1, SubLine.X2, SubLine.Y2);
                    }
                }
            }
            return ReturnValue;
        }

        private static List<Line> GenerateLines(int Width, int Height, int NumberOfCracks, int Iterations, int MaxChange, int MaxLength, int Seed)
        {
            Contract.Requires<ArgumentException>(NumberOfCracks >= 0 && Width >= 0);
            List<Line> Lines = new List<Line>();
            System.Random Generator = new System.Random(Seed);
            for (int x = 0; x < NumberOfCracks; ++x)
            {
                Line TempLine = null;
                int LineLength = 0;
                do
                {
                    TempLine = new Line(Generator.Next(0, Width), Generator.Next(0, Width),
                        Generator.Next(0, Height), Generator.Next(0, Height));
                    LineLength = (int)System.Math.Sqrt((double)((TempLine.X1 - TempLine.X2) * (TempLine.X1 - TempLine.X2))
                        + ((TempLine.Y1 - TempLine.Y2) * (TempLine.Y1 - TempLine.Y2)));
                } while (LineLength > MaxLength && LineLength <= 0);
                Lines.Add(TempLine);
                List<Line> TempLineList = new List<Line>();
                TempLineList.Add(TempLine);
                for (int y = 0; y < Iterations; ++y)
                {
                    Line LineUsing = TempLineList[Generator.Next(0, TempLineList.Count)];
                    int XBreak = Generator.Next(LineUsing.X1, LineUsing.X2) + Generator.Next(-MaxChange, MaxChange);
                    int YBreak = 0;
                    if (LineUsing.Y1 > LineUsing.Y2)
                    {
                        YBreak = Generator.Next(LineUsing.Y2, LineUsing.Y1) + Generator.Next(-MaxChange, MaxChange);
                    }
                    else
                    {
                        YBreak = Generator.Next(LineUsing.Y1, LineUsing.Y2) + Generator.Next(-MaxChange, MaxChange);
                    }
                    Line LineA = new Line(LineUsing.X1, XBreak, LineUsing.Y1, YBreak);
                    Line LineB = new Line(XBreak, LineUsing.X2, YBreak, LineUsing.Y2);
                    TempLineList.Remove(LineUsing);
                    TempLineList.Add(LineA);
                    TempLineList.Add(LineB);
                }
                TempLine.SubLines = TempLineList;
            }
            return Lines;
        }

        #endregion Functions
    }

    #region Internal classes

    internal class Line
    {
        public Line()
        {
        }

        public Line(int X1, int X2, int Y1, int Y2)
        {
            if (X1 > X2)
            {
                int Holder = X1;
                X1 = X2;
                X2 = Holder;
                Holder = Y1;
                Y1 = Y2;
                Y2 = Holder;
            }
            this.X1 = X1;
            this.X2 = X2;
            this.Y1 = Y1;
            this.Y2 = Y2;
        }

        public List<Line> SubLines = new List<Line>();
        public int X1;
        public int X2;
        public int Y1;
        public int Y2;
    }

    #endregion Internal classes
}