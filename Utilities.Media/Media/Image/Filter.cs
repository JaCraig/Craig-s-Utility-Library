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
using System.Drawing;
using System.Drawing.Imaging;
using Utilities.Math.ExtensionMethods;
using Utilities.Media.Image.ExtensionMethods;
using System;
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
        /// <param name="Width">Width</param>
        /// <param name="Height">Height</param>
        public Filter(int Width = 3, int Height = 3)
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
        public virtual int[,] MyFilter { get; set; }

        /// <summary>
        /// Width of the filter box
        /// </summary>
        public virtual int Width { get; set; }

        /// <summary>
        /// Height of the filter box
        /// </summary>
        public virtual int Height { get; set; }

        /// <summary>
        /// Amount to add to the red, blue, and green values
        /// </summary>
        public virtual int Offset { get; set; }

        /// <summary>
        /// Determines if we should take the absolute value prior to clamping
        /// </summary>
        public virtual bool Absolute { get; set; }

        #endregion

        #region Public Functions

        /// <summary>
        /// Applies the filter to the input image
        /// </summary>
        /// <param name="Input">input image</param>
        /// <returns>Returns a separate image with the filter applied</returns>
        public virtual Bitmap ApplyFilter(Bitmap Input)
        {
            if (Input == null)
                throw new ArgumentNullException("Input");
            Bitmap NewBitmap = new Bitmap(Input.Width, Input.Height);
            BitmapData NewData = NewBitmap.LockImage();
            BitmapData OldData = Input.LockImage();
            int NewPixelSize = NewData.GetPixelSize();
            int OldPixelSize = OldData.GetPixelSize();
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
                                    Color Pixel = OldData.GetPixel(XCurrent + x, YCurrent + y, OldPixelSize);
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
                    Color MeanPixel = OldData.GetPixel(x, y, OldPixelSize);
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
                        RValue = RValue.Clamp(255, 0);
                        GValue = (GValue / Weight) + Offset;
                        GValue = GValue.Clamp(255, 0);
                        BValue = (BValue / Weight) + Offset;
                        BValue = BValue.Clamp(255, 0);
                        MeanPixel = Color.FromArgb(RValue, GValue, BValue);
                    }
                    NewData.SetPixel(x, y, MeanPixel, NewPixelSize);
                }
            }
            NewBitmap.UnlockImage(NewData);
            Input.UnlockImage(OldData);
            return NewBitmap;
        }

        #endregion
    }
}