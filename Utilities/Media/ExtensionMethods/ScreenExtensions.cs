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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace Utilities.Media
{
    /// <summary>
    /// Screen extensions
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class ScreenExtensions
    {
        /// <summary>
        /// Takes a screenshot of the screen as a whole (if multiple screens are attached, it takes
        /// an image containing them all)
        /// </summary>
        /// <param name="Screen">Screen to get the screenshot from</param>
        /// <param name="FileName">Name of the file to save the screenshot (optional)</param>
        /// <returns>Returns a SwiftBitmap containing the screen shot</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public static Bitmap TakeScreenShot(this Screen Screen, string FileName = "")
        {
            Contract.Requires<ArgumentNullException>(Screen != null, "Screen");
            Bitmap TempSwiftBitmap = new Bitmap(Screen.Bounds.Width > 1 ? Screen.Bounds.Width : 1, Screen.Bounds.Height > 1 ? Screen.Bounds.Height : 1, PixelFormat.Format32bppArgb);
            try
            {
                if (Screen.Bounds.Width > 1 && Screen.Bounds.Height > 1)
                {
                    using (Graphics TempGraphics = Graphics.FromImage(TempSwiftBitmap))
                    {
                        TempGraphics.CopyFromScreen(Screen.Bounds.X, Screen.Bounds.Y, 0, 0, Screen.Bounds.Size, CopyPixelOperation.SourceCopy);
                    }
                }
            }
            catch { }
            if (!string.IsNullOrEmpty(FileName))
                TempSwiftBitmap.Save(FileName);
            return TempSwiftBitmap;
        }

        /// <summary>
        /// Takes a screenshot of the screen as a whole (if multiple screens are attached, it takes
        /// an image containing them all)
        /// </summary>
        /// <param name="Screens">Screens to get the screenshot from</param>
        /// <param name="FileName">Name of the file to save the screenshot (optional)</param>
        /// <returns>Returns a SwiftBitmap containing the screen shot</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public static Bitmap TakeScreenShot(this IEnumerable<Screen> Screens, string FileName = "")
        {
            Contract.Requires<ArgumentNullException>(Screens != null, "Screens");
            Rectangle TotalScreenRect = Rectangle.Empty;
            foreach (Screen CurrentScreen in Screen.AllScreens)
                TotalScreenRect = Rectangle.Union(TotalScreenRect, CurrentScreen.Bounds);
            Bitmap TempSwiftBitmap = new Bitmap(TotalScreenRect.Width > 1 ? TotalScreenRect.Width : 1, TotalScreenRect.Height > 1 ? TotalScreenRect.Width : 1, PixelFormat.Format32bppArgb);
            try
            {
                if (TotalScreenRect.Width > 1 && TotalScreenRect.Height > 1)
                {
                    using (Graphics TempGraphics = Graphics.FromImage(TempSwiftBitmap))
                    {
                        TempGraphics.CopyFromScreen(TotalScreenRect.X, TotalScreenRect.Y, 0, 0, TotalScreenRect.Size, CopyPixelOperation.SourceCopy);
                    }
                }
            }
            catch { }
            if (!string.IsNullOrEmpty(FileName))
                TempSwiftBitmap.Save(FileName);
            return TempSwiftBitmap;
        }
    }
}