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
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
#endregion

namespace Utilities.Media
{
    /// <summary>
    /// Class for handling screen capture
    /// </summary>
    public static class Screenshot
    {
        #region Public Static Functions
        /// <summary>
        /// Takes a screenshot of the screen as a whole
        /// (if multiple screens are attached, it takes an image containing them all)
        /// </summary>
        /// <param name="FileName">Name of the file to save the screenshot.</param>
        public static void TakeScreenShot(string FileName)
        {
            try
            {
                Bitmap TempBitmap=Screenshot.TakeScreenShot();
                TempBitmap.Save(FileName);
                TempBitmap.Dispose();
            }
            catch (Exception a)
            {
                throw a;
            }
        }

        /// <summary>
        /// Takes a screenshot of the screen as a whole
        /// (if multiple screens are attached, it takes an image containing them all)
        /// </summary>
        /// <returns>Returns the bitmap object containing the screenshot</returns>
        public static Bitmap TakeScreenShot()
        {
            try
            {
                Rectangle TotalScreenRect = Rectangle.Empty;
                foreach (Screen CurrentScreen in Screen.AllScreens)
                {
                    TotalScreenRect = Rectangle.Union(TotalScreenRect, CurrentScreen.Bounds);
                }
                Bitmap TempBitmap = new Bitmap(TotalScreenRect.Width, TotalScreenRect.Height, PixelFormat.Format32bppArgb);
                Graphics TempGraphics = Graphics.FromImage(TempBitmap);
                TempGraphics.CopyFromScreen(TotalScreenRect.X, TotalScreenRect.Y, 0, 0, TotalScreenRect.Size, CopyPixelOperation.SourceCopy);
                TempGraphics.Dispose();
                return TempBitmap;
            }
            catch (Exception a)
            {
                throw a;
            }
        }
        #endregion
    }
}
