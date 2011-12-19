/*
Copyright (c) 2009 <a href="http://www.gutgames.com">James Craig</a>

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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using System.Drawing.Imaging;
#endregion

namespace Utilities.Media
{
    /// <summary>
    /// Used when applying convolution filters to an image
    /// </summary>
    public class Filter
    {
        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public Filter()
        {
            _MyFilter = new int[3, 3];
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Width">Width</param>
        /// <param name="Height">Height</param>
        public Filter(int Width, int Height)
        {
            _MyFilter = new int[Width, Height];
            this.Width = Width;
            this.Height = Height;
        }
        #endregion

        #region Public Properties
        private int[,] _MyFilter = null;
        private int _Width = 3;
        private int _Height = 3;
        private int _Offset = 0;

        /// <summary>
        /// The actual filter array
        /// </summary>
        public int[,] MyFilter
        {
            get { return _MyFilter; }
            set { _MyFilter = value; }
        }
        /// <summary>
        /// Width of the filter box
        /// </summary>
        public int Width
        {
            get { return _Width; }
            set { _Width = value; }
        }
        /// <summary>
        /// Height of the filter box
        /// </summary>
        public int Height
        {
            get { return _Height; }
            set { _Height = value; }
        }
        /// <summary>
        /// Amount to add to the red, blue, and green values
        /// </summary>
        public int Offset
        {
            get { return _Offset; }
            set { _Offset = value; }
        }
        #endregion

        #region Public Functions
        public Bitmap ApplyFilter(Bitmap Input)
        {
            System.Drawing.Bitmap TempBitmap = Input;
            System.Drawing.Bitmap NewBitmap = new System.Drawing.Bitmap(TempBitmap.Width, TempBitmap.Height);
            System.Drawing.Graphics NewGraphics = System.Drawing.Graphics.FromImage(NewBitmap);
            NewGraphics.DrawImage(TempBitmap, new System.Drawing.Rectangle(0, 0, TempBitmap.Width, TempBitmap.Height), new System.Drawing.Rectangle(0, 0, TempBitmap.Width, TempBitmap.Height), System.Drawing.GraphicsUnit.Pixel);
            NewGraphics.Dispose();
            for (int x = 0; x < Input.Width; ++x)
            {
                for (int y = 0; y < Input.Height; ++y)
                {
                    int RValue = 0;
                    int GValue = 0;
                    int BValue = 0;
                    int Weight = 0;
                    int XCurrent = -Width / 2;
                    for (int x2 = 0; x2 < Width; ++x2)
                    {
                        if (XCurrent + x < Input.Width && XCurrent + x >= 0)
                        {
                            int YCurrent = -Height / 2;
                            for (int y2 = 0; y2 < Height; ++y2)
                            {
                                if (YCurrent + y < Input.Height && YCurrent + y >= 0)
                                {
                                    RValue += MyFilter[x2, y2] * TempBitmap.GetPixel(XCurrent + x, YCurrent + y).R;
                                    GValue += MyFilter[x2, y2] * TempBitmap.GetPixel(XCurrent + x, YCurrent + y).G;
                                    BValue += MyFilter[x2, y2] * TempBitmap.GetPixel(XCurrent + x, YCurrent + y).B;
                                    Weight += MyFilter[x2, y2];
                                }
                                ++YCurrent;
                            }
                        }
                        ++XCurrent;
                    }
                    Color MeanPixel = TempBitmap.GetPixel(x, y);
                    if (Weight == 0)
                        Weight = 1;
                    if (Weight > 0)
                    {
                        RValue = (RValue / Weight)+Offset;
                        if (RValue < 0)
                            RValue = 0;
                        else if (RValue > 255)
                            RValue = 255;
                        GValue = (GValue / Weight) + Offset;
                        if (GValue < 0)
                            GValue = 0;
                        else if (GValue > 255)
                            GValue = 255;
                        BValue = (BValue / Weight) + Offset;
                        if (BValue < 0)
                            BValue = 0;
                        else if (BValue > 255)
                            BValue = 255;
                        MeanPixel = Color.FromArgb(RValue, GValue, BValue);
                    }
                    NewBitmap.SetPixel(x, y, MeanPixel);
                }
            }
            return NewBitmap;
        }
        #endregion
    }
}
