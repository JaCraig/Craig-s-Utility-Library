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
using System.Drawing;
using System.Drawing.Imaging;
using Utilities.Math;

#endregion

namespace Utilities.Media.Image
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
            MyFilter = new int[3, 3];
            Width = 3;
            Height = 3;
            Offset = 0;
            Absolute = false;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Width">Width</param>
        /// <param name="Height">Height</param>
        public Filter(int Width, int Height)
        {
            MyFilter = new int[Width, Height];
            this.Width = Width;
            this.Height = Height;
            Offset = 0;
            Absolute = false;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// The actual filter array
        /// </summary>
        public int[,] MyFilter { get; set; }

        /// <summary>
        /// Width of the filter box
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Height of the filter box
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Amount to add to the red, blue, and green values
        /// </summary>
        public int Offset { get; set; }

        /// <summary>
        /// Determines if we should take the absolute value prior to clamping
        /// </summary>
        public bool Absolute { get; set; }

        #endregion

        #region Public Functions

        /// <summary>
        /// Applies the filter to the input image
        /// </summary>
        /// <param name="Input">input image</param>
        /// <returns>Returns a separate image with the filter applied</returns>
        public Bitmap ApplyFilter(Bitmap Input)
        {
            Bitmap NewBitmap = new Bitmap(Input.Width, Input.Height);
            BitmapData NewData = Image.LockImage(NewBitmap);
            BitmapData OldData = Image.LockImage(Input);
            int NewPixelSize = Image.GetPixelSize(NewData);
            int OldPixelSize = Image.GetPixelSize(OldData);
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
                                    Color Pixel = Image.GetPixel(OldData, XCurrent + x, YCurrent + y, OldPixelSize);
                                    RValue += MyFilter[x2, y2] * Pixel.R;
                                    GValue += MyFilter[x2, y2] * Pixel.G;
                                    BValue += MyFilter[x2, y2] * Pixel.B;
                                    Weight += MyFilter[x2, y2];
                                }
                                ++YCurrent;
                            }
                        }
                        ++XCurrent;
                    }
                    Color MeanPixel = Image.GetPixel(OldData, x, y, OldPixelSize);
                    if (Weight == 0)
                        Weight = 1;
                    if (Weight > 0)
                    {
                        if (Absolute)
                        {
                            RValue = System.Math.Abs(RValue);
                            GValue = System.Math.Abs(GValue);
                            BValue = System.Math.Abs(BValue);
                        }
                        RValue = (RValue / Weight) + Offset;
                        RValue = MathHelper.Clamp(RValue, 255, 0);
                        GValue = (GValue / Weight) + Offset;
                        GValue = MathHelper.Clamp(GValue, 255, 0);
                        BValue = (BValue / Weight) + Offset;
                        BValue = MathHelper.Clamp(BValue, 255, 0);
                        MeanPixel = Color.FromArgb(RValue, GValue, BValue);
                    }
                    Image.SetPixel(NewData, x, y, MeanPixel, NewPixelSize);
                }
            }
            Image.UnlockImage(NewBitmap, NewData);
            Image.UnlockImage(Input, OldData);
            return NewBitmap;
        }

        #endregion
    }
}