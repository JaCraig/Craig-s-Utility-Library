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
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using Utilities.DataTypes.Patterns.BaseClasses;

namespace Utilities.Media
{
    /// <summary>
    /// Bitmap wrapper. Helps make bitmap access faster and a bit simpler.
    /// </summary>
    public class SwiftBitmap : SafeDisposableBaseClass, ICloneable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SwiftBitmap"/> class.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        public SwiftBitmap(string fileName)
            : this(new Bitmap(fileName))
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(fileName));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SwiftBitmap"/> class.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public SwiftBitmap(int width, int height)
            : this(new Bitmap(width, height))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SwiftBitmap"/> class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public SwiftBitmap(Stream stream)
            : this(new Bitmap(stream))
        {
            Contract.Requires<ArgumentNullException>(stream != null);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SwiftBitmap"/> class.
        /// </summary>
        /// <param name="image">The image.</param>
        public SwiftBitmap(Image image)
            : this(new Bitmap(image))
        {
            Contract.Requires<ArgumentNullException>(image != null);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SwiftBitmap"/> class.
        /// </summary>
        /// <param name="bitmap">The bitmap.</param>
        public SwiftBitmap(Bitmap bitmap)
            : base()
        {
            Contract.Requires<ArgumentNullException>(bitmap != null);
            InternalBitmap = bitmap;
            Height = InternalBitmap.Height;
            Width = InternalBitmap.Width;
        }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        /// <value>The height.</value>
        public int Height { get; private set; }

        /// <summary>
        /// Gets the internal bitmap.
        /// </summary>
        /// <value>The internal bitmap.</value>
        public Bitmap InternalBitmap { get; private set; }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>The width.</value>
        public int Width { get; private set; }

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <value>The data.</value>
        protected BitmapData Data { get; private set; }

        /// <summary>
        /// Gets the data pointer.
        /// </summary>
        /// <value>The data pointer.</value>
        [CLSCompliant(false)]
        protected unsafe byte* DataPointer { get; private set; }

        /// <summary>
        /// Gets the pixel size (in bytes)
        /// </summary>
        /// <value>The size of the pixel.</value>
        protected int PixelSize { get; private set; }

        /// <summary>
        /// Applies the matrix.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <returns>This</returns>
        public SwiftBitmap ApplyMatrix(System.Drawing.Imaging.ColorMatrix matrix)
        {
            Contract.Requires<ArgumentNullException>(matrix != null);
            Unlock();
            using (Graphics NewGraphics = Graphics.FromImage(InternalBitmap))
            {
                using (ImageAttributes Attributes = new ImageAttributes())
                {
                    Attributes.SetColorMatrix(matrix);
                    NewGraphics.DrawImage(InternalBitmap,
                        new System.Drawing.Rectangle(0, 0, Width, Height),
                        0, 0, Width, Height,
                        GraphicsUnit.Pixel,
                        Attributes);
                }
            }
            return this;
        }

        /// <summary>
        /// Applies the matrix.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <returns>This</returns>
        public SwiftBitmap ApplyMatrix(float[][] matrix)
        {
            Contract.Requires<ArgumentNullException>(matrix != null);
            return ApplyMatrix(new System.Drawing.Imaging.ColorMatrix(matrix));
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        public object Clone()
        {
            Unlock();
            return new SwiftBitmap((Bitmap)InternalBitmap.Clone());
        }

        /// <summary>
        /// Gets the pixel.
        /// </summary>
        /// <param name="x">The x position</param>
        /// <param name="y">The y position</param>
        /// <returns>This</returns>
        public unsafe Color GetPixel(int x, int y)
        {
            Contract.Requires<NullReferenceException>(Data != null);
            byte* TempPointer = DataPointer + (y * Data.Stride) + (x * PixelSize);
            return (PixelSize == 3) ?
                Color.FromArgb(TempPointer[2], TempPointer[1], TempPointer[0]) :
                Color.FromArgb(TempPointer[3], TempPointer[2], TempPointer[1], TempPointer[0]);
        }

        /// <summary>
        /// Gets the pixel.
        /// </summary>
        /// <param name="position">The position in the image</param>
        /// <returns>This</returns>
        public unsafe Color GetPixel(int position)
        {
            Contract.Requires<NullReferenceException>(Data != null);
            byte* TempPointer = DataPointer + (position * PixelSize);
            return (PixelSize == 3) ?
                Color.FromArgb(TempPointer[2], TempPointer[1], TempPointer[0]) :
                Color.FromArgb(TempPointer[3], TempPointer[2], TempPointer[1], TempPointer[0]);
        }

        /// <summary>
        /// Locks this instance.
        /// </summary>
        /// <returns>This</returns>
        public unsafe SwiftBitmap Lock()
        {
            Contract.Requires<NullReferenceException>(InternalBitmap != null);
            if (Data != null)
                return this;
            Data = InternalBitmap.LockImage();
            PixelSize = Data.GetPixelSize();
            DataPointer = (byte*)Data.Scan0;
            return this;
        }

        /// <summary>
        /// Saves to the specified file name.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>This</returns>
        public SwiftBitmap Save(string fileName)
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(fileName));
            Contract.Requires<NullReferenceException>(InternalBitmap != null);
            Unlock();
            InternalBitmap.Save(fileName, GetImageFormat(fileName));
            return this;
        }

        /// <summary>
        /// Sets the pixel.
        /// </summary>
        /// <param name="x">The x position</param>
        /// <param name="y">The y position</param>
        /// <param name="pixelColor">Color of the pixel.</param>
        /// <returns>This</returns>
        public unsafe SwiftBitmap SetPixel(int x, int y, Color pixelColor)
        {
            Contract.Requires<NullReferenceException>(Data != null, "Data");
            byte* TempPointer = DataPointer + (y * Data.Stride) + (x * PixelSize);
            if (PixelSize == 3)
            {
                TempPointer[2] = pixelColor.R;
                TempPointer[1] = pixelColor.G;
                TempPointer[0] = pixelColor.B;
                return this;
            }
            TempPointer[3] = pixelColor.A;
            TempPointer[2] = pixelColor.R;
            TempPointer[1] = pixelColor.G;
            TempPointer[0] = pixelColor.B;
            return this;
        }

        /// <summary>
        /// Sets the pixels starting at the x and y coordinate specified.
        /// </summary>
        /// <param name="x">The beginning x coordinate</param>
        /// <param name="y">The beginning y coordinate</param>
        /// <param name="pixels">The pixels to set</param>
        /// <returns>This</returns>
        public unsafe SwiftBitmap SetPixels(int x, int y, Color[] pixels)
        {
            Contract.Requires<NullReferenceException>(Data != null, "Data");
            if (pixels == null)
                return this;
            byte* TempPointer = DataPointer + (y * Data.Stride) + (x * PixelSize);
            for (int z = 0; z < pixels.Length; ++z)
            {
                if (PixelSize == 3)
                {
                    TempPointer[2] = pixels[z].R;
                    TempPointer[1] = pixels[z].G;
                    TempPointer[0] = pixels[z].B;
                }
                else
                {
                    TempPointer[3] = pixels[z].A;
                    TempPointer[2] = pixels[z].R;
                    TempPointer[1] = pixels[z].G;
                    TempPointer[0] = pixels[z].B;
                }
                TempPointer += PixelSize;
            }
            return this;
        }

        /// <summary>
        /// Copies the image from one image to this one.
        /// </summary>
        /// <param name="bitmap">The bitmap to copy from.</param>
        /// <returns>
        /// This
        /// </returns>
        public unsafe SwiftBitmap Copy(SwiftBitmap bitmap)
        {
            if (bitmap == null)
                return this;
            Unlock();
            if (InternalBitmap != null)
            {
                InternalBitmap.Dispose();
            }
            InternalBitmap = (Bitmap)bitmap.InternalBitmap.Clone();
            return this;
        }

        /// <summary>
        /// Unlocks this bitmap
        /// </summary>
        /// <returns>This</returns>
        public unsafe SwiftBitmap Unlock()
        {
            Contract.Requires<NullReferenceException>(InternalBitmap != null);
            if (Data == null)
                return this;
            InternalBitmap.UnlockImage(Data);
            Data = null;
            DataPointer = null;
            return this;
        }

        /// <summary>
        /// Function to override in order to dispose objects
        /// </summary>
        /// <param name="Managed">
        /// If true, managed and unmanaged objects should be disposed. Otherwise unmanaged objects only.
        /// </param>
        protected override void Dispose(bool Managed)
        {
            if (Data != null)
            {
                Unlock();
            }
            if (InternalBitmap != null)
            {
                InternalBitmap.Dispose();
                InternalBitmap = null;
            }
        }

        /// <summary>
        /// Crops the image by the specified width/height
        /// </summary>
        /// <param name="Width">The width.</param>
        /// <param name="Height">The height.</param>
        /// <param name="VAlignment">The v alignment.</param>
        /// <param name="HAlignment">The h alignment.</param>
        /// <returns></returns>
        public SwiftBitmap Crop(int Width, int Height, Align VAlignment, Align HAlignment)
        {
            Unlock();
            System.Drawing.Rectangle TempRectangle = new System.Drawing.Rectangle();
            TempRectangle.Height = Height;
            TempRectangle.Width = Width;
            TempRectangle.Y = VAlignment == Align.Top ? 0 : this.Height - Height;
            if (TempRectangle.Y < 0)
                TempRectangle.Y = 0;
            TempRectangle.X = HAlignment == Align.Left ? 0 : this.Width - Width;
            if (TempRectangle.X < 0)
                TempRectangle.X = 0;
            var TempHolder = InternalBitmap.Clone(TempRectangle, InternalBitmap.PixelFormat);
            InternalBitmap.Dispose();
            InternalBitmap = TempHolder;
            return this;
        }

        /// <summary>
        /// Implements the operator &amp;.
        /// </summary>
        /// <param name="Image1">The first image.</param>
        /// <param name="Image2">The second image</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static SwiftBitmap operator &(SwiftBitmap Image1, SwiftBitmap Image2)
        {
            Contract.Requires<ArgumentNullException>(Image1 != null, "Image1");
            Contract.Requires<ArgumentNullException>(Image2 != null, "Image2");
            Image1.Lock();
            Image2.Lock();
            SwiftBitmap Result = new SwiftBitmap(Image1.Width, Image1.Height);
            Result.Lock();
            Parallel.For(0, Result.Width, x =>
            {
                for (int y = 0; y < Result.Height; ++y)
                {
                    Color Pixel1 = Image1.GetPixel(x, y);
                    Color Pixel2 = Image2.GetPixel(x, y);
                    Result.SetPixel(x, y,
                        Color.FromArgb(Pixel1.R & Pixel2.R,
                            Pixel1.G & Pixel2.G,
                            Pixel1.B & Pixel2.B));
                }
            });
            Image2.Unlock();
            Image1.Unlock();
            return Result.Unlock();
        }

        /// <summary>
        /// Draws the path specified
        /// </summary>
        /// <param name="pen">The pen to use.</param>
        /// <param name="path">The path to draw</param>
        /// <returns>This</returns>
        public SwiftBitmap DrawPath(Pen pen, GraphicsPath path)
        {
            Contract.Requires<ArgumentNullException>(pen != null);
            Contract.Requires<ArgumentNullException>(path != null);
            Unlock();
            using (Graphics NewGraphics = Graphics.FromImage(InternalBitmap))
            {
                NewGraphics.DrawPath(pen, path);
            }
            return this;
        }

        /// <summary>
        /// Draws the text specified
        /// </summary>
        /// <param name="TextToDraw">The text to draw.</param>
        /// <param name="FontToUse">The font to use.</param>
        /// <param name="BrushUsing">The brush to use.</param>
        /// <param name="BoxToDrawWithin">The box to draw within.</param>
        /// <returns>This</returns>
        public SwiftBitmap DrawText(string TextToDraw,
            Font FontToUse, Brush BrushUsing, RectangleF BoxToDrawWithin)
        {
            Contract.Requires<ArgumentNullException>(FontToUse != null, "FontToUse");
            Contract.Requires<ArgumentNullException>(BrushUsing != null, "BrushUsing");
            Contract.Requires<ArgumentNullException>(BoxToDrawWithin != null, "BoxToDrawWithin");
            Unlock();
            using (Graphics TempGraphics = Graphics.FromImage(InternalBitmap))
            {
                TempGraphics.DrawString(TextToDraw, FontToUse, BrushUsing, BoxToDrawWithin);
            }
            return this;
        }

        /// <summary>
        /// Returns the Bitmap format this file is using
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>The image format</returns>
        private static ImageFormat GetImageFormat(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return ImageFormat.Bmp;
            if (fileName.EndsWith("jpg", StringComparison.InvariantCultureIgnoreCase) || fileName.EndsWith("jpeg", StringComparison.InvariantCultureIgnoreCase))
                return ImageFormat.Jpeg;
            if (fileName.EndsWith("png", StringComparison.InvariantCultureIgnoreCase))
                return ImageFormat.Png;
            if (fileName.EndsWith("tiff", StringComparison.InvariantCultureIgnoreCase))
                return ImageFormat.Tiff;
            if (fileName.EndsWith("ico", StringComparison.InvariantCultureIgnoreCase))
                return ImageFormat.Icon;
            if (fileName.EndsWith("gif", StringComparison.InvariantCultureIgnoreCase))
                return ImageFormat.Gif;
            return ImageFormat.Bmp;
        }
    }
}