/*
Copyright (c) 2011 <a href="http://www.gutgames.com">James Craig</a>

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
using System.Drawing;
using System.Drawing.Imaging;
using Utilities.Math.ExtensionMethods;
#endregion

namespace Utilities.Media.Image.Procedural
{
    /// <summary>
    /// Helper class for doing fault formations
    /// </summary>
    public static class FaultFormation
    {
        #region Functions
        
        /// <summary>
        /// Generates a number of faults, returning an image
        /// </summary>
        /// <param name="Width">Width of the resulting image</param>
        /// <param name="Height">Height of the resulting image</param>
        /// <param name="NumberFaults">Number of faults</param>
        /// <param name="Seed">Random seed</param>
        /// <returns>An image from the resulting faults</returns>
        public static Bitmap Generate(int Width,int Height,int NumberFaults,int Seed)
        {
            float[,] Heights = new float[Width, Height];
            float IncreaseVal = 0.1f;
            System.Random Generator = new System.Random(Seed);
            for (int x = 0; x < NumberFaults; ++x)
            {
                IncreaseVal = GenerateFault(Width, Height, NumberFaults, Heights, IncreaseVal, Generator);
            }
            Bitmap ReturnValue = new Bitmap(Width, Height);
            BitmapData ImageData = Image.LockImage(ReturnValue);
            int ImagePixelSize = Image.GetPixelSize(ImageData);
            for (int x = 0; x < Width; ++x)
            {
                for (int y = 0; y < Height; ++y)
                {
                    float Value = Heights[x, y];
                    Value = (Value * 0.5f) + 0.5f;
                    Value *= 255;
                    int RGBValue = ((int)Value).Clamp(255, 0);
                    Image.SetPixel(ImageData, x, y, Color.FromArgb(RGBValue, RGBValue, RGBValue), ImagePixelSize);
                }
            }
            Image.UnlockImage(ReturnValue, ImageData);
            return ReturnValue;
        }

        private static float GenerateFault(int Width, int Height, int NumberFaults, float[,] Heights, float IncreaseVal, System.Random Generator)
        {
            int Wall = 0;
            int Wall2 = 0;
            while (Wall == Wall2)
            {
                Wall = Generator.Next(4);
                Wall2 = Generator.Next(4);
            }
            int X1 = 0;
            int Y1 = 0;
            int X2 = 0;
            int Y2 = 0;
            while (X1 == X2 || Y1 == Y2)
            {
                if (Wall == 0)
                {
                    X1 = Generator.Next(Width);
                    Y1 = 0;
                }
                else if (Wall == 1)
                {
                    Y1 = Generator.Next(Height);
                    X1 = Width;
                }
                else if (Wall == 2)
                {
                    X1 = Generator.Next(Width);
                    Y1 = Height;
                }
                else
                {
                    X1 = 0;
                    Y1 = Generator.Next(Height);
                }

                if (Wall2 == 0)
                {
                    X2 = Generator.Next(Width);
                    Y2 = 0;
                }
                else if (Wall2 == 1)
                {
                    Y2 = Generator.Next(Height);
                    X2 = Width;
                }
                else if (Wall2 == 2)
                {
                    X2 = Generator.Next(Width);
                    Y2 = Height;
                }
                else
                {
                    X2 = 0;
                    Y2 = Generator.Next(Height);
                }
            }
            int M = (Y1 - Y2) / (X1 - X2);
            int B = Y1 - (M * X1);
            int Side = Generator.Next(2);
            int Direction = 0;
            while (Direction == 0)
                Direction = Generator.Next(-1, 2);
            float TempIncreaseVal = (float)Generator.NextDouble() * IncreaseVal * (float)Direction;
            if (Side == 0)
            {
                for (int y = 0; y < Width; ++y)
                {
                    int LastY = (M * y) + B;
                    for (int z = 0; z < LastY; ++z)
                    {
                        if (z < Height)
                        {
                            Heights[y, z] += TempIncreaseVal;
                            if (Heights[y, z] > 1.0f)
                                Heights[y, z] = 1.0f;
                            else if (Heights[y, z] < -1.0f)
                                Heights[y, z] = -1.0f;
                        }
                    }
                }
            }
            else
            {
                for (int y = 0; y < Width; ++y)
                {
                    int LastY = (M * y) + B;
                    if (LastY < 0)
                        LastY = 0;
                    for (int z = LastY; z < Height; ++z)
                    {
                        Heights[y, z] += TempIncreaseVal;
                        if (Heights[y, z] > 1.0f)
                            Heights[y, z] = 1.0f;
                        else if (Heights[y, z] < -1.0f)
                            Heights[y, z] = -1.0f;
                    }
                }
            }
            IncreaseVal -= (0.1f / (float)NumberFaults);
            return IncreaseVal;
        }

        #endregion
    }
}