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
using System;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Drawing.Imaging;
using Utilities.DataTypes;


#endregion

namespace Utilities.Media
{
    /// <summary>
    /// Class used to create an RGB Histogram
    /// </summary>
    public class RGBHistogram
    {
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Image">Image to load</param>
        public RGBHistogram(Bitmap Image = null)
        {
            R = new float[256];
            G = new float[256];
            B = new float[256];
            if (Image != null)
                LoadImage(Image);
        }

        #endregion

        #region Public Functions

        /// <summary>
        /// Loads an image
        /// </summary>
        /// <param name="ImageUsing">Image to load</param>
        public virtual void LoadImage(Bitmap ImageUsing)
        {
            Contract.Requires<ArgumentNullException>(ImageUsing != null, "ImageUsing");
            BitmapData OldData = ImageUsing.LockImage();
            int PixelSize = OldData.GetPixelSize();
            Width = ImageUsing.Width;
            Height = ImageUsing.Height;
            R.Clear();
            G.Clear();
            B.Clear();
            for (int x = 0; x < Width; ++x)
            {
                for (int y = 0; y < Height; ++y)
                {
                    Color TempColor = OldData.GetPixel(x, y, PixelSize);
                    ++R[(int)TempColor.R];
                    ++G[(int)TempColor.G];
                    ++B[(int)TempColor.B];
                }
            }
            ImageUsing.UnlockImage(OldData);
        }

        /// <summary>
        /// Normalizes the histogram
        /// </summary>
        public virtual void Normalize()
        {
            float TotalPixels = Width * Height;
            if (TotalPixels <= 0)
                return;
            for (int x = 0; x < 256; ++x)
            {
                R[x] /= TotalPixels;
                G[x] /= TotalPixels;
                B[x] /= TotalPixels;
            }
        }

        /// <summary>
        /// Equalizes the histogram
        /// </summary>
        public virtual void Equalize()
        {
            float TotalPixels = Width * Height;
            int RMax = int.MinValue;
            int RMin = int.MaxValue;
            int GMax = int.MinValue;
            int GMin = int.MaxValue;
            int BMax = int.MinValue;
            int BMin = int.MaxValue;
            for (int x = 0; x < 256; ++x)
            {
                if (R[x] > 0f)
                {
                    if (RMax < x)
                        RMax = x;
                    if (RMin > x)
                        RMin = x;
                }
                if (G[x] > 0f)
                {
                    if (GMax < x)
                        GMax = x;
                    if (GMin > x)
                        GMin = x;
                }
                if (B[x] > 0f)
                {
                    if (BMax < x)
                        BMax = x;
                    if (BMin > x)
                        BMin = x;
                }
            }

            float PreviousR = R[0];
            R[0] = R[0] * 256 / TotalPixels;
            float PreviousG = G[0];
            G[0] = G[0] * 256 / TotalPixels;
            float PreviousB = B[0];
            B[0] = B[0] * 256 / TotalPixels;
            for (int x = 1; x < 256; ++x)
            {
                PreviousR += R[x];
                PreviousG += G[x];
                PreviousB += B[x];
                R[x] = ((PreviousR - R[RMin]) / (TotalPixels - R[RMin])) * 255;
                G[x] = ((PreviousG - G[GMin]) / (TotalPixels - G[GMin])) * 255;
                B[x] = ((PreviousB - B[BMin]) / (TotalPixels - B[BMin])) * 255;
            }
            Width = 256;
            Height = 1;
        }

        #endregion

        #region Private Values
        private int Width;
        private int Height;
        #endregion

        #region Public Properties

        /// <summary>
        /// Red values
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
        public virtual float[] R { get; set; }

        /// <summary>
        /// Green values
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
        public virtual float[] G { get; set; }

        /// <summary>
        /// Blue values
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
        public virtual float[] B { get; set; }

        #endregion
    }
}