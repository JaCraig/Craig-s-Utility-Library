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
#endregion

namespace Utilities.Media.Image
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
        public RGBHistogram()
        {
            R = new float[256];
            G = new float[256];
            B = new float[256];
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Image">Image to load</param>
        public RGBHistogram(Bitmap Image)
        {
            R = new float[256];
            G = new float[256];
            B = new float[256];
            LoadImage(Image);
        }

        #endregion

        #region Public Functions

        /// <summary>
        /// Loads an image
        /// </summary>
        /// <param name="ImageUsing">Image to load</param>
        public void LoadImage(Bitmap ImageUsing)
        {
            BitmapData OldData = Image.LockImage(ImageUsing);
            int PixelSize = Image.GetPixelSize(OldData);
            Width = ImageUsing.Width;
            Height = ImageUsing.Height;
            for (int x = 0; x < 256; ++x)
            {
                R[x] = 0;
                G[x] = 0;
                B[x] = 0;
            }
            for (int x = 0; x < ImageUsing.Width; ++x)
            {
                for (int y = 0; y < ImageUsing.Height; ++y)
                {
                    Color TempColor = Image.GetPixel(OldData, x, y, PixelSize);
                    ++R[(int)TempColor.R];
                    ++G[(int)TempColor.G];
                    ++B[(int)TempColor.B];
                }
            }
            Image.UnlockImage(ImageUsing, OldData);
        }

        /// <summary>
        /// Normalizes the histogram
        /// </summary>
        public void Normalize()
        {
            float TotalPixels = Width * Height;
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
        public void Equalize()
        {
            float TotalPixels = Width * Height;

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
                R[x] = PreviousR * 256 / TotalPixels;
                G[x] = PreviousG * 256 / TotalPixels;
                B[x] = PreviousB * 256 / TotalPixels;
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
        public float[] R { get; set; }

        /// <summary>
        /// Green values
        /// </summary>
        public float[] G { get; set; }

        /// <summary>
        /// Blue values
        /// </summary>
        public float[] B { get; set; }

        #endregion
    }
}