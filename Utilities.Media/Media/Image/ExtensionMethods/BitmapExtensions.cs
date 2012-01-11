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
using System.Windows.Forms;
using System;
using System.Collections.Generic;
using Utilities.Math.ExtensionMethods;
using Utilities.DataTypes.ExtensionMethods;
using System.Drawing.Drawing2D;
using Utilities.Media.Image.Procedural;
using System.IO;
#endregion

namespace Utilities.Media.Image.ExtensionMethods
{
    /// <summary>
    /// Image extensions
    /// </summary>
    public static class BitmapExtensions
    {
        #region Functions

        #region AddNoise

        /// <summary>
        /// adds noise to the image
        /// </summary>
        /// <param name="OriginalImage">Image to add noise to</param>
        /// <param name="FileName">Location to save the image to (optional)</param>
        /// <param name="Amount">Amount of noise to add (defaults to 10)</param>
        /// <returns>New image object with the noise added</returns>
        public static Bitmap AddNoise(this Bitmap OriginalImage, int Amount = 10, string FileName = "")
        {
            if (OriginalImage == null)
                throw new ArgumentNullException("OriginalImage");
            ImageFormat FormatUsing = FileName.GetImageFormat();
            Bitmap NewBitmap = new Bitmap(OriginalImage.Width, OriginalImage.Height);
            BitmapData NewData = NewBitmap.LockImage();
            BitmapData OldData = OriginalImage.LockImage();
            int NewPixelSize = NewData.GetPixelSize();
            int OldPixelSize = OldData.GetPixelSize();
            Random.Random TempRandom = new Random.Random();
            for (int x = 0; x < NewBitmap.Width; ++x)
            {
                for (int y = 0; y < NewBitmap.Height; ++y)
                {
                    Color CurrentPixel = OldData.GetPixel(x, y, OldPixelSize);
                    int R = CurrentPixel.R + TempRandom.Next(-Amount, Amount + 1);
                    int G = CurrentPixel.G + TempRandom.Next(-Amount, Amount + 1);
                    int B = CurrentPixel.B + TempRandom.Next(-Amount, Amount + 1);
                    R = R > 255 ? 255 : R;
                    R = R < 0 ? 0 : R;
                    G = G > 255 ? 255 : G;
                    G = G < 0 ? 0 : G;
                    B = B > 255 ? 255 : B;
                    B = B < 0 ? 0 : B;
                    Color TempValue = Color.FromArgb(R, G, B);
                    NewData.SetPixel(x, y, TempValue, NewPixelSize);
                }
            }
            NewBitmap.UnlockImage(NewData);
            OriginalImage.UnlockImage(OldData);
            if (!string.IsNullOrEmpty(FileName))
                NewBitmap.Save(FileName, FormatUsing);
            return NewBitmap;
        }

        #endregion

        #region AdjustBrightness

        /// <summary>
        /// Adjusts the brightness
        /// </summary>
        /// <param name="Image">Image to change</param>
        /// <param name="FileName">File to save to</param>
        /// <param name="Value">-255 to 255</param>
        /// <returns>A bitmap object</returns>
        public static Bitmap AdjustBrightness(this Bitmap Image, int Value = 0, string FileName = "")
        {
            if (Image == null)
                throw new ArgumentNullException("Image");
            ImageFormat FormatUsing = FileName.GetImageFormat();
            float FinalValue = (float)Value / 255.0f;
            ColorMatrix TempMatrix = new ColorMatrix();
            TempMatrix.Matrix = new float[][]{
                            new float[] {1, 0, 0, 0, 0},
                            new float[] {0, 1, 0, 0, 0},
                            new float[] {0, 0, 1, 0, 0},
                            new float[] {0, 0, 0, 1, 0},
                            new float[] {FinalValue, FinalValue, FinalValue, 1, 1}
                        };
            Bitmap NewBitmap = TempMatrix.Apply(Image);
            if (!string.IsNullOrEmpty(FileName))
                NewBitmap.Save(FileName, FormatUsing);
            return NewBitmap;
        }

        #endregion

        #region AdjustContrast

        /// <summary>
        /// Adjusts the Contrast
        /// </summary>
        /// <param name="OriginalImage">Image to change</param>
        /// <param name="Value">Used to set the contrast (-100 to 100)</param>
        /// <param name="FileName">File to save to</param>
        /// <returns>A bitmap object</returns>
        public static Bitmap AdjustContrast(this Bitmap OriginalImage, float Value = 0, string FileName = "")
        {
            if (OriginalImage == null)
                throw new ArgumentNullException("OriginalImage");
            ImageFormat FormatUsing = FileName.GetImageFormat();
            Bitmap NewBitmap = new Bitmap(OriginalImage.Width, OriginalImage.Height);
            BitmapData NewData = NewBitmap.LockImage();
            BitmapData OldData = OriginalImage.LockImage();
            int NewPixelSize = NewData.GetPixelSize();
            int OldPixelSize = OldData.GetPixelSize();
            Value = (100.0f + Value) / 100.0f;
            Value *= Value;

            for (int x = 0; x < NewBitmap.Width; ++x)
            {
                for (int y = 0; y < NewBitmap.Height; ++y)
                {
                    Color Pixel = OldData.GetPixel(x, y, OldPixelSize);
                    float Red = Pixel.R / 255.0f;
                    float Green = Pixel.G / 255.0f;
                    float Blue = Pixel.B / 255.0f;
                    Red = (((Red - 0.5f) * Value) + 0.5f) * 255.0f;
                    Green = (((Green - 0.5f) * Value) + 0.5f) * 255.0f;
                    Blue = (((Blue - 0.5f) * Value) + 0.5f) * 255.0f;
                    NewData.SetPixel(x, y,
                        Color.FromArgb(((int)Red).Clamp(255, 0),
                        ((int)Green).Clamp(255, 0),
                        ((int)Blue).Clamp(255, 0)),
                        NewPixelSize);
                }
            }
            NewBitmap.UnlockImage(NewData);
            OriginalImage.UnlockImage(OldData);
            if (!string.IsNullOrEmpty(FileName))
                NewBitmap.Save(FileName, FormatUsing);
            return NewBitmap;
        }

        #endregion

        #region AdjustGamma

        /// <summary>
        /// Adjusts the Gamma
        /// </summary>
        /// <param name="OriginalImage">Image to change</param>
        /// <param name="Value">Used to build the gamma ramp (usually .2 to 5)</param>
        /// <param name="FileName">File to save to</param>
        /// <returns>A bitmap object</returns>
        public static Bitmap AdjustGamma(this Bitmap OriginalImage, float Value = 1.0f, string FileName = "")
        {
            if (OriginalImage == null)
                throw new ArgumentNullException("OriginalImage");
            ImageFormat FormatUsing = FileName.GetImageFormat();
            Bitmap NewBitmap = new Bitmap(OriginalImage.Width, OriginalImage.Height);
            BitmapData NewData = NewBitmap.LockImage();
            BitmapData OldData = OriginalImage.LockImage();
            int NewPixelSize = NewData.GetPixelSize();
            int OldPixelSize = OldData.GetPixelSize();

            int[] RedRamp = new int[256];
            int[] GreenRamp = new int[256];
            int[] BlueRamp = new int[256];
            for (int x = 0; x < 256; ++x)
            {
                RedRamp[x] = ((int)((255.0 * System.Math.Pow(x / 255.0, 1.0 / Value)) + 0.5)).Clamp(255, 0);
                GreenRamp[x] = ((int)((255.0 * System.Math.Pow(x / 255.0, 1.0 / Value)) + 0.5)).Clamp(255, 0);
                BlueRamp[x] = ((int)((255.0 * System.Math.Pow(x / 255.0, 1.0 / Value)) + 0.5)).Clamp(255, 0);
            }

            for (int x = 0; x < NewBitmap.Width; ++x)
            {
                for (int y = 0; y < NewBitmap.Height; ++y)
                {
                    Color Pixel = OldData.GetPixel(x, y, OldPixelSize);
                    int Red = RedRamp[Pixel.R];
                    int Green = GreenRamp[Pixel.G];
                    int Blue = BlueRamp[Pixel.B];
                    NewData.SetPixel(x, y, Color.FromArgb(Red, Green, Blue), NewPixelSize);
                }
            }

            NewBitmap.UnlockImage(NewData);
            OriginalImage.UnlockImage(OldData);
            if (!string.IsNullOrEmpty(FileName))
                NewBitmap.Save(FileName, FormatUsing);
            return NewBitmap;
        }

        #endregion

        #region And

        /// <summary>
        /// ands two images
        /// </summary>
        /// <param name="Image1">Image to manipulate</param>
        /// <param name="Image2">Image to manipulate</param>
        /// <param name="FileName">File to save to</param>
        /// <returns>A bitmap image</returns>
        public static Bitmap And(this Bitmap Image1, Bitmap Image2, string FileName = "")
        {
            if (Image1 == null)
                throw new ArgumentNullException("Image1");
            if (Image2 == null)
                throw new ArgumentNullException("Image2");
            ImageFormat FormatUsing = FileName.GetImageFormat();
            Bitmap NewBitmap = new Bitmap(Image1.Width, Image1.Height);
            BitmapData NewData = NewBitmap.LockImage();
            BitmapData OldData1 = Image1.LockImage();
            BitmapData OldData2 = Image2.LockImage();
            int NewPixelSize = NewData.GetPixelSize();
            int OldPixelSize1 = OldData1.GetPixelSize();
            int OldPixelSize2 = OldData2.GetPixelSize();
            for (int x = 0; x < NewBitmap.Width; ++x)
            {
                for (int y = 0; y < NewBitmap.Height; ++y)
                {
                    Color Pixel1 = OldData1.GetPixel(x, y, OldPixelSize1);
                    Color Pixel2 = OldData2.GetPixel(x, y, OldPixelSize2);
                    NewData.SetPixel(x, y,
                        Color.FromArgb(Pixel1.R & Pixel2.R,
                            Pixel1.G & Pixel2.G,
                            Pixel1.B & Pixel2.B),
                        NewPixelSize);
                }
            }
            NewBitmap.UnlockImage(NewData);
            Image1.UnlockImage(OldData1);
            Image2.UnlockImage(OldData2);
            if (!string.IsNullOrEmpty(FileName))
                NewBitmap.Save(FileName, FormatUsing);
            return NewBitmap;
        }

        #endregion

        #region BlackAndWhite

        /// <summary>
        /// Converts an image to black and white
        /// </summary>
        /// <param name="Image">Image to change</param>
        /// <param name="FileName">File to save to</param>
        /// <returns>A bitmap object of the black and white image</returns>
        public static Bitmap BlackAndWhite(this Bitmap Image, string FileName = "")
        {
            if (Image == null)
                throw new ArgumentNullException("Image");
            ImageFormat FormatUsing = FileName.GetImageFormat();
            ColorMatrix TempMatrix = new ColorMatrix();
            TempMatrix.Matrix = new float[][]{
                            new float[] {.3f, .3f, .3f, 0, 0},
                            new float[] {.59f, .59f, .59f, 0, 0},
                            new float[] {.11f, .11f, .11f, 0, 0},
                            new float[] {0, 0, 0, 1, 0},
                            new float[] {0, 0, 0, 0, 1}
                        };
            Bitmap NewBitmap = TempMatrix.Apply(Image);
            if (!string.IsNullOrEmpty(FileName))
                NewBitmap.Save(FileName, FormatUsing);
            return NewBitmap;
        }

        #endregion

        #region BlueFilter

        /// <summary>
        /// Gets the blue filter for an image
        /// </summary>
        /// <param name="Image">Image to change</param>
        /// <param name="FileName">File to save to</param>
        /// <returns>A bitmap object</returns>
        public static Bitmap BlueFilter(this Bitmap Image, string FileName = "")
        {
            if (Image == null)
                throw new ArgumentNullException("Image");
            ImageFormat FormatUsing = FileName.GetImageFormat();
            ColorMatrix TempMatrix = new ColorMatrix();
            TempMatrix.Matrix = new float[][]{
                            new float[] {0, 0, 0, 0, 0},
                            new float[] {0, 0, 0, 0, 0},
                            new float[] {0, 0, 1, 0, 0},
                            new float[] {0, 0, 0, 1, 0},
                            new float[] {0, 0, 0, 0, 1}
                        };
            Bitmap NewBitmap = TempMatrix.Apply(Image);
            if (!string.IsNullOrEmpty(FileName))
                NewBitmap.Save(FileName, FormatUsing);
            return NewBitmap;
        }

        #endregion

        #region BoxBlur

        /// <summary>
        /// Does smoothing using a box blur
        /// </summary>
        /// <param name="Image">Image to manipulate</param>
        /// <param name="Size">Size of the aperture</param>
        /// <param name="FileName">File to save to</param>
        /// <returns>A bitmap object</returns>
        public static Bitmap BoxBlur(this Bitmap Image, int Size = 3, string FileName = "")
        {
            if (Image == null)
                throw new ArgumentNullException("Image");
            ImageFormat FormatUsing = FileName.GetImageFormat();
            Filter TempFilter = new Filter(Size, Size);
            for (int x = 0; x < Size; ++x)
                for (int y = 0; y < Size; ++y)
                    TempFilter.MyFilter[x, y] = 1;
            Bitmap NewBitmap = TempFilter.ApplyFilter(Image);
            if (!string.IsNullOrEmpty(FileName))
                NewBitmap.Save(FileName, FormatUsing);
            return NewBitmap;
        }

        #endregion

        #region Colorize

        /// <summary>
        /// Colorizes a black and white image
        /// </summary>
        /// <param name="OriginalImage">Black and white image</param>
        /// <param name="Colors">Color array to use for the image</param>
        /// <param name="FileName">File to save to</param>
        /// <returns>The colorized image</returns>
        public static Bitmap Colorize(this Bitmap OriginalImage, Color[] Colors, string FileName = "")
        {
            if (OriginalImage == null)
                throw new ArgumentNullException("OriginalImage");
            if (Colors.Length < 256)
                return new Bitmap(1, 1);
            ImageFormat FormatUsing = FileName.GetImageFormat();
            Bitmap NewBitmap = new Bitmap(OriginalImage.Width, OriginalImage.Height);
            BitmapData NewData = NewBitmap.LockImage();
            BitmapData OldData = OriginalImage.LockImage();
            int NewPixelSize = NewData.GetPixelSize();
            int OldPixelSize = OldData.GetPixelSize();
            for (int x = 0; x < OriginalImage.Width; ++x)
            {
                for (int y = 0; y < OriginalImage.Height; ++y)
                {
                    int ColorUsing = OldData.GetPixel(x, y, OldPixelSize).R;
                    NewData.SetPixel(x, y, Colors[ColorUsing], NewPixelSize);
                }
            }
            NewBitmap.UnlockImage(NewData);
            OriginalImage.UnlockImage(OldData);
            if (!string.IsNullOrEmpty(FileName))
                NewBitmap.Save(FileName, FormatUsing);
            return NewBitmap;
        }

        #endregion

        #region Crop

        /// <summary>
        /// Crops an image
        /// </summary>
        /// <param name="ImageUsing">Image to crop</param>
        /// <param name="Width">Width of the cropped image</param>
        /// <param name="Height">Height of the cropped image</param>
        /// <param name="VAlignment">The verticle alignment of the cropping (top or bottom)</param>
        /// <param name="HAlignment">The horizontal alignment of the cropping (left or right)</param>
        /// <param name="FileName">File to save to</param>
        /// <returns>A Bitmap object of the cropped image</returns>
        public static Bitmap Crop(this Bitmap ImageUsing, int Width, int Height, Align VAlignment, Align HAlignment, string FileName = "")
        {
            if (ImageUsing == null)
                throw new ArgumentNullException("ImageUsing");
            ImageFormat FormatUsing = FileName.GetImageFormat();
            Bitmap TempBitmap = ImageUsing;
            System.Drawing.Rectangle TempRectangle = new System.Drawing.Rectangle();
            TempRectangle.Height = Height;
            TempRectangle.Width = Width;
            if (VAlignment == Align.Top)
            {
                TempRectangle.Y = 0;
            }
            else
            {
                TempRectangle.Y = TempBitmap.Height - Height;
                if (TempRectangle.Y < 0)
                    TempRectangle.Y = 0;
            }
            if (HAlignment == Align.Left)
            {
                TempRectangle.X = 0;
            }
            else
            {
                TempRectangle.X = TempBitmap.Width - Width;
                if (TempRectangle.X < 0)
                    TempRectangle.X = 0;
            }
            Bitmap NewBitmap = TempBitmap.Clone(TempRectangle, TempBitmap.PixelFormat);
            if (!string.IsNullOrEmpty(FileName))
                NewBitmap.Save(FileName, FormatUsing);
            return NewBitmap;
        }

        #endregion

        #region Dilate

        /// <summary>
        /// Does dilation
        /// </summary>
        /// <param name="OriginalImage">Image to manipulate</param>
        /// <param name="Size">Size of the aperture</param>
        /// <param name="FileName">File to save to</param>
        /// <returns>A Bitmap object of the resulting image</returns>
        public static Bitmap Dilate(this Bitmap OriginalImage, int Size, string FileName = "")
        {
            if (OriginalImage == null)
                throw new ArgumentNullException("OriginalImage");
            ImageFormat FormatUsing = FileName.GetImageFormat();
            Bitmap NewBitmap = new Bitmap(OriginalImage.Width, OriginalImage.Height);
            BitmapData NewData = NewBitmap.LockImage();
            BitmapData OldData = OriginalImage.LockImage();
            int NewPixelSize = NewData.GetPixelSize();
            int OldPixelSize = OldData.GetPixelSize();
            int ApetureMin = -(Size / 2);
            int ApetureMax = (Size / 2);
            for (int x = 0; x < NewBitmap.Width; ++x)
            {
                for (int y = 0; y < NewBitmap.Height; ++y)
                {
                    int RValue = 0;
                    int GValue = 0;
                    int BValue = 0;
                    for (int x2 = ApetureMin; x2 < ApetureMax; ++x2)
                    {
                        int TempX = x + x2;
                        if (TempX >= 0 && TempX < NewBitmap.Width)
                        {
                            for (int y2 = ApetureMin; y2 < ApetureMax; ++y2)
                            {
                                int TempY = y + y2;
                                if (TempY >= 0 && TempY < NewBitmap.Height)
                                {
                                    Color TempColor = OldData.GetPixel(TempX, TempY, OldPixelSize);
                                    if (TempColor.R > RValue)
                                        RValue = TempColor.R;
                                    if (TempColor.G > GValue)
                                        GValue = TempColor.G;
                                    if (TempColor.B > BValue)
                                        BValue = TempColor.B;
                                }
                            }
                        }
                    }
                    Color TempPixel = Color.FromArgb(RValue, GValue, BValue);
                    NewData.SetPixel(x, y, TempPixel, NewPixelSize);
                }
            }
            NewBitmap.UnlockImage(NewData);
            OriginalImage.UnlockImage(OldData);
            if (!string.IsNullOrEmpty(FileName))
                NewBitmap.Save(FileName, FormatUsing);
            return NewBitmap;
        }

        #endregion

        #region Distance

        private static double Distance(int R1, int R2, int G1, int G2, int B1, int B2)
        {
            return ((double)(((R1 - R2) * (R1 - R2)) + ((G1 - G2) * (G1 - G2)) + ((B1 - B2) * (B1 - B2)))).Sqrt();
        }

        #endregion

        #region DrawRoundedRectangle

        /// <summary>
        /// Draws a rounded rectangle on a bitmap
        /// </summary>
        /// <param name="Image">Image to draw on</param>
        /// <param name="BoxColor">The color that the box should be</param>
        /// <param name="XPosition">The upper right corner's x position</param>
        /// <param name="YPosition">The upper right corner's y position</param>
        /// <param name="Height">Height of the box</param>
        /// <param name="Width">Width of the box</param>
        /// <param name="CornerRadius">Radius of the corners</param>
        /// <param name="FileName">File to save to</param>
        /// <returns>The bitmap with the rounded box on it</returns>
        public static Bitmap DrawRoundedRectangle(this Bitmap Image, Color BoxColor, int XPosition, int YPosition,
            int Height, int Width, int CornerRadius, string FileName = "")
        {
            if (Image == null)
                throw new ArgumentNullException("Image");
            if (BoxColor == null)
                throw new ArgumentNullException("BoxColor");
            ImageFormat FormatUsing = FileName.GetImageFormat();
            Bitmap NewBitmap = new Bitmap(Image, Image.Width, Image.Height);
            using (Graphics NewGraphics = Graphics.FromImage(NewBitmap))
            {
                using (Pen BoxPen = new Pen(BoxColor))
                {
                    using (GraphicsPath Path = new GraphicsPath())
                    {
                        Path.AddLine(XPosition + CornerRadius, YPosition, XPosition + Width - (CornerRadius * 2), YPosition);
                        Path.AddArc(XPosition + Width - (CornerRadius * 2), YPosition, CornerRadius * 2, CornerRadius * 2, 270, 90);
                        Path.AddLine(XPosition + Width, YPosition + CornerRadius, XPosition + Width, YPosition + Height - (CornerRadius * 2));
                        Path.AddArc(XPosition + Width - (CornerRadius * 2), YPosition + Height - (CornerRadius * 2), CornerRadius * 2, CornerRadius * 2, 0, 90);
                        Path.AddLine(XPosition + Width - (CornerRadius * 2), YPosition + Height, XPosition + CornerRadius, YPosition + Height);
                        Path.AddArc(XPosition, YPosition + Height - (CornerRadius * 2), CornerRadius * 2, CornerRadius * 2, 90, 90);
                        Path.AddLine(XPosition, YPosition + Height - (CornerRadius * 2), XPosition, YPosition + CornerRadius);
                        Path.AddArc(XPosition, YPosition, CornerRadius * 2, CornerRadius * 2, 180, 90);
                        Path.CloseFigure();
                        NewGraphics.DrawPath(BoxPen, Path);
                    }
                }
            }
            if (!string.IsNullOrEmpty(FileName))
                NewBitmap.Save(FileName, FormatUsing);
            return NewBitmap;
        }

        #endregion

        #region DrawText

        /// <summary>
        /// Draws text on an image within the bounding box specified.
        /// </summary>
        /// <param name="Image">Image to draw on</param>
        /// <param name="TextToDraw">The text to draw on the image</param>
        /// <param name="FontToUse">Font in which to draw the text</param>
        /// <param name="BrushUsing">Defines the brush using</param>
        /// <param name="BoxToDrawWithin">Rectangle to draw the image within</param>
        /// <param name="FileName">File to save to</param>
        /// <returns>A bitmap object with the text drawn on it</returns>
        public static Bitmap DrawText(this Bitmap Image, string TextToDraw,
            Font FontToUse, Brush BrushUsing, RectangleF BoxToDrawWithin,
            string FileName = "")
        {
            if (Image == null)
                throw new ArgumentNullException("Image");
            if (FontToUse == null)
                throw new ArgumentNullException("FontToUse");
            if (BrushUsing == null)
                throw new ArgumentNullException("BrushUsing");
            if (BoxToDrawWithin == null)
                throw new ArgumentNullException("BoxToDrawWithin");
            ImageFormat FormatUsing = FileName.GetImageFormat();
            Bitmap NewBitmap = new Bitmap(Image, Image.Width, Image.Height);
            using (Graphics TempGraphics = Graphics.FromImage(NewBitmap))
            {
                TempGraphics.DrawString(TextToDraw, FontToUse, BrushUsing, BoxToDrawWithin);
            }
            if (!string.IsNullOrEmpty(FileName))
                NewBitmap.Save(FileName, FormatUsing);
            return NewBitmap;
        }

        #endregion

        #region EdgeDetection

        /// <summary>
        /// Does basic edge detection on an image
        /// </summary>
        /// <param name="OriginalImage">Image to do edge detection on</param>
        /// <param name="Threshold">Decides what is considered an edge</param>
        /// <param name="EdgeColor">Color of the edge</param>
        /// <param name="FileName">File to save to</param>
        /// <returns>A bitmap which has the edges drawn on it</returns>
        public static Bitmap EdgeDetection(this Bitmap OriginalImage, float Threshold, Color EdgeColor, string FileName = "")
        {
            if (OriginalImage == null)
                throw new ArgumentNullException("OriginalImage");
            if (EdgeColor == null)
                throw new ArgumentNullException("EdgeColor");
            ImageFormat FormatUsing = FileName.GetImageFormat();
            Bitmap NewBitmap = new Bitmap(OriginalImage, OriginalImage.Width, OriginalImage.Height);
            BitmapData NewData = NewBitmap.LockImage();
            BitmapData OldData = OriginalImage.LockImage();
            int NewPixelSize = NewData.GetPixelSize();
            int OldPixelSize = OldData.GetPixelSize();
            for (int x = 0; x < NewBitmap.Width; ++x)
            {
                for (int y = 0; y < NewBitmap.Height; ++y)
                {
                    Color CurrentColor = OldData.GetPixel(x, y, OldPixelSize);
                    if (y < NewBitmap.Height - 1 && x < NewBitmap.Width - 1)
                    {
                        Color TempColor = OldData.GetPixel(x + 1, y + 1, OldPixelSize);
                        if (Distance(CurrentColor.R, TempColor.R, CurrentColor.G, TempColor.G, CurrentColor.B, TempColor.B) > Threshold)
                        {
                            NewData.SetPixel(x, y, EdgeColor, NewPixelSize);
                        }
                    }
                    else if (y < NewBitmap.Height - 1)
                    {
                        Color TempColor = OldData.GetPixel(x, y + 1, OldPixelSize);
                        if (Distance(CurrentColor.R, TempColor.R, CurrentColor.G, TempColor.G, CurrentColor.B, TempColor.B) > Threshold)
                        {
                            NewData.SetPixel(x, y, EdgeColor, NewPixelSize);
                        }
                    }
                    else if (x < NewBitmap.Width - 1)
                    {
                        Color TempColor = OldData.GetPixel(x + 1, y, OldPixelSize);
                        if (Distance(CurrentColor.R, TempColor.R, CurrentColor.G, TempColor.G, CurrentColor.B, TempColor.B) > Threshold)
                        {
                            NewData.SetPixel(x, y, EdgeColor, NewPixelSize);
                        }
                    }
                }
            }
            NewBitmap.UnlockImage(NewData);
            OriginalImage.UnlockImage(OldData);
            if (!string.IsNullOrEmpty(FileName))
                NewBitmap.Save(FileName, FormatUsing);
            return NewBitmap;
        }

        #endregion

        #region Emboss

        /// <summary>
        /// Emboss function
        /// </summary>
        /// <param name="Image">Image to manipulate</param>
        /// <param name="FileName">File to save to</param>
        /// <returns>A bitmap image</returns>
        public static Bitmap Emboss(this Bitmap Image, string FileName = "")
        {
            if (Image == null)
                throw new ArgumentNullException("Image");
            ImageFormat FormatUsing = FileName.GetImageFormat();
            Filter TempFilter = new Filter(3, 3);
            TempFilter.MyFilter[0, 0] = -2;
            TempFilter.MyFilter[0, 1] = -1;
            TempFilter.MyFilter[1, 0] = -1;
            TempFilter.MyFilter[1, 1] = 1;
            TempFilter.MyFilter[2, 1] = 1;
            TempFilter.MyFilter[1, 2] = 1;
            TempFilter.MyFilter[2, 2] = 2;
            TempFilter.MyFilter[0, 2] = 0;
            TempFilter.MyFilter[2, 0] = 0;
            Bitmap NewBitmap = TempFilter.ApplyFilter(Image);
            if (!string.IsNullOrEmpty(FileName))
                NewBitmap.Save(FileName, FormatUsing);
            return NewBitmap;
        }

        #endregion

        #region Equalize

        /// <summary>
        /// Uses an RGB histogram to equalize the image
        /// </summary>
        /// <param name="OriginalImage">Image to manipulate</param>
        /// <param name="FileName">File to save to</param>
        /// <returns>The resulting bitmap image</returns>
        public static Bitmap Equalize(this Bitmap OriginalImage, string FileName = "")
        {
            if (OriginalImage == null)
                throw new ArgumentNullException("OriginalImage");
            ImageFormat FormatUsing = FileName.GetImageFormat();
            Bitmap NewBitmap = new Bitmap(OriginalImage.Width, OriginalImage.Height);
            RGBHistogram TempHistogram = new RGBHistogram(OriginalImage);
            TempHistogram.Equalize();
            BitmapData NewData = NewBitmap.LockImage();
            BitmapData OldData = OriginalImage.LockImage();
            int NewPixelSize = NewData.GetPixelSize();
            int OldPixelSize = OldData.GetPixelSize();
            for (int x = 0; x < NewBitmap.Width; ++x)
            {
                for (int y = 0; y < NewBitmap.Height; ++y)
                {
                    Color Current = OldData.GetPixel(x, y, OldPixelSize);
                    int NewR = (int)TempHistogram.R[Current.R];
                    int NewG = (int)TempHistogram.G[Current.G];
                    int NewB = (int)TempHistogram.B[Current.B];
                    NewR = NewR.Clamp(255, 0);
                    NewG = NewG.Clamp(255, 0);
                    NewB = NewB.Clamp(255, 0);
                    NewData.SetPixel(x, y, Color.FromArgb(NewR, NewG, NewB), NewPixelSize);
                }
            }
            NewBitmap.UnlockImage(NewData);
            OriginalImage.UnlockImage(OldData);
            if (!string.IsNullOrEmpty(FileName))
                NewBitmap.Save(FileName, FormatUsing);
            return NewBitmap;
        }

        #endregion

        #region Flip

        /// <summary>
        /// Flips an image
        /// </summary>
        /// <param name="Image">Image to flip</param>
        /// <param name="FlipX">Flips an image along the X axis</param>
        /// <param name="FlipY">Flips an image along the Y axis</param>
        /// <param name="FileName">File to save to</param>
        /// <returns>A bitmap which is flipped</returns>
        public static Bitmap Flip(this Bitmap Image, bool FlipX, bool FlipY, string FileName = "")
        {
            if (Image == null)
                throw new ArgumentNullException("Image");
            ImageFormat FormatUsing = FileName.GetImageFormat();
            Bitmap NewBitmap = new Bitmap(Image, Image.Width, Image.Height);
            if (FlipX && !FlipY)
                NewBitmap.RotateFlip(RotateFlipType.RotateNoneFlipX);
            else if (!FlipX && FlipY)
                NewBitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);
            else if (FlipX && FlipY)
                NewBitmap.RotateFlip(RotateFlipType.RotateNoneFlipXY);

            if (!string.IsNullOrEmpty(FileName))
                NewBitmap.Save(FileName, FormatUsing);
            return NewBitmap;
        }

        #endregion

        #region GaussianBlur

        /// <summary>
        /// Does smoothing using a gaussian blur
        /// </summary>
        /// <param name="Image">Image to manipulate</param>
        /// <param name="FileName">File to save to</param>
        /// <param name="Size">Size of the aperture</param>
        /// <returns>The resulting bitmap</returns>
        public static Bitmap GaussianBlur(this Bitmap Image, int Size = 3, string FileName = "")
        {
            if (Image == null)
                throw new ArgumentNullException("Image");
            ImageFormat FormatUsing = FileName.GetImageFormat();
            using (Bitmap ReturnBitmap = Image.BoxBlur(Size))
            {
                using (Bitmap ReturnBitmap2 = ReturnBitmap.BoxBlur(Size))
                {
                    Bitmap ReturnBitmap3 = ReturnBitmap2.BoxBlur(Size);
                    if (!string.IsNullOrEmpty(FileName))
                        ReturnBitmap3.Save(FileName, FormatUsing);
                    return ReturnBitmap3;
                }
            }
        }

        #endregion

        #region GetHTMLPalette

        /// <summary>
        /// Gets a palette listing in HTML string format
        /// </summary>
        /// <param name="OriginalImage">Image to get the palette of</param>
        /// <returns>A list containing HTML color values (ex: #041845)</returns>
        public static List<string> GetHTMLPalette(this Bitmap OriginalImage)
        {
            if (OriginalImage == null)
                throw new ArgumentNullException("OriginalImage");
            List<string> ReturnArray = new List<string>();
            if (OriginalImage.Palette != null && OriginalImage.Palette.Entries.Length > 0)
            {
                OriginalImage.Palette.Entries.ForEach(x => ReturnArray.AddIfUnique(ColorTranslator.ToHtml(x)));
                return ReturnArray;
            }
            BitmapData ImageData = OriginalImage.LockImage();
            int PixelSize = ImageData.GetPixelSize();
            for (int x = 0; x < OriginalImage.Width; ++x)
            {
                for (int y = 0; y < OriginalImage.Height; ++y)
                {
                    ReturnArray.AddIfUnique(ColorTranslator.ToHtml(ImageData.GetPixel(x, y, PixelSize)));
                }
            }
            OriginalImage.UnlockImage(ImageData);
            return ReturnArray;
        }

        #endregion

        #region GreenFilter

        /// <summary>
        /// Gets the Green filter for an image
        /// </summary>
        /// <param name="Image">Image to change</param>
        /// <param name="FileName">File to save to</param>
        /// <returns>A bitmap object</returns>
        public static Bitmap GreenFilter(this Bitmap Image, string FileName = "")
        {
            if (Image == null)
                throw new ArgumentNullException("Image");
            ImageFormat FormatUsing = FileName.GetImageFormat();
            ColorMatrix TempMatrix = new ColorMatrix();
            TempMatrix.Matrix = new float[][]{
                            new float[] {0, 0, 0, 0, 0},
                            new float[] {0, 1, 0, 0, 0},
                            new float[] {0, 0, 0, 0, 0},
                            new float[] {0, 0, 0, 1, 0},
                            new float[] {0, 0, 0, 0, 1}
                        };
            Bitmap NewBitmap = TempMatrix.Apply(Image);
            if (!string.IsNullOrEmpty(FileName))
                NewBitmap.Save(FileName, FormatUsing);
            return NewBitmap;
        }

        #endregion

        #region GetHeight

        private static float GetHeight(int x, int y, BitmapData BlackAndWhiteData, int BlackAndWhitePixelSize)
        {
            if (BlackAndWhiteData == null)
                throw new ArgumentNullException("BlackAndWhiteData");
            Color TempColor = BlackAndWhiteData.GetPixel(x, y, BlackAndWhitePixelSize);
            return GetHeight(TempColor);
        }

        private static float GetHeight(Color Color)
        {
            if (Color == null)
                throw new ArgumentNullException("Color");
            return (float)Color.R / 255.0f;
        }

        #endregion

        #region GetImageFormat

        /// <summary>
        /// Returns the image format this file is using
        /// </summary>
        /// <param name="FileName"></param>
        /// <returns></returns>
        public static ImageFormat GetImageFormat(this string FileName)
        {
            if (string.IsNullOrEmpty(FileName))
                return ImageFormat.Bmp;
            if (FileName.EndsWith("jpg", StringComparison.InvariantCultureIgnoreCase) || FileName.EndsWith("jpeg", StringComparison.InvariantCultureIgnoreCase))
                return ImageFormat.Jpeg;
            if (FileName.EndsWith("png", StringComparison.InvariantCultureIgnoreCase))
                return ImageFormat.Png;
            if (FileName.EndsWith("tiff", StringComparison.InvariantCultureIgnoreCase))
                return ImageFormat.Tiff;
            if (FileName.EndsWith("ico", StringComparison.InvariantCultureIgnoreCase))
                return ImageFormat.Icon;
            if (FileName.EndsWith("gif", StringComparison.InvariantCultureIgnoreCase))
                return ImageFormat.Gif;
            return ImageFormat.Bmp;
        }

        #endregion

        #region GetMinMaxPixel

        private static void GetMinMaxPixel(out Color Min, out Color Max, BitmapData ImageData, int PixelSize)
        {
            if (ImageData == null)
                throw new ArgumentNullException("ImageData");
            int MinR = 255, MinG = 255, MinB = 255;
            int MaxR = 0, MaxG = 0, MaxB = 0;
            for (int x = 0; x < ImageData.Width; ++x)
            {
                for (int y = 0; y < ImageData.Height; ++y)
                {
                    Color TempImage = ImageData.GetPixel(x, y, PixelSize);
                    if (MinR > TempImage.R)
                        MinR = TempImage.R;
                    if (MaxR < TempImage.R)
                        MaxR = TempImage.R;

                    if (MinG > TempImage.G)
                        MinG = TempImage.G;
                    if (MaxG < TempImage.G)
                        MaxG = TempImage.G;

                    if (MinB > TempImage.B)
                        MinB = TempImage.B;
                    if (MaxB < TempImage.B)
                        MaxB = TempImage.B;
                }
            }
            Min = Color.FromArgb(MinR, MinG, MinB);
            Max = Color.FromArgb(MaxR, MaxG, MaxB);
        }

        #endregion

        #region GetPixelSize

        /// <summary>
        /// Gets the pixel size (in bytes)
        /// </summary>
        /// <param name="Data">Bitmap data</param>
        /// <returns>The pixel size (in bytes)</returns>
        public static int GetPixelSize(this BitmapData Data)
        {
            if (Data == null)
                throw new ArgumentNullException("Data");
            if (Data.PixelFormat == PixelFormat.Format24bppRgb)
                return 3;
            else if (Data.PixelFormat == PixelFormat.Format32bppArgb
                || Data.PixelFormat == PixelFormat.Format32bppPArgb
                || Data.PixelFormat == PixelFormat.Format32bppRgb)
                return 4;
            return 0;
        }

        #endregion

        #region GetPixel

        /// <summary>
        /// Gets a pixel from an x,y coordinate
        /// </summary>
        /// <param name="Data">Bitmap data</param>
        /// <param name="x">X coord</param>
        /// <param name="y">Y coord</param>
        /// <param name="PixelSizeInBytes">Pixel size in bytes</param>
        /// <returns>The pixel at the x,y coords</returns>
        public static unsafe Color GetPixel(this BitmapData Data, int x, int y, int PixelSizeInBytes)
        {
            if (Data == null)
                throw new ArgumentNullException("Data");
            byte* DataPointer = (byte*)Data.Scan0;
            DataPointer = DataPointer + (y * Data.Stride) + (x * PixelSizeInBytes);
            return (PixelSizeInBytes == 3) ?
                Color.FromArgb(DataPointer[2], DataPointer[1], DataPointer[0]) :
                Color.FromArgb(DataPointer[3], DataPointer[2], DataPointer[1], DataPointer[0]);
        }

        #endregion

        #region Jitter

        /// <summary>
        /// Causes a "Jitter" effect
        /// </summary>
        /// <param name="OriginalImage">Image to manipulate</param>
        /// <param name="MaxJitter">Maximum number of pixels the item can move</param>
        /// <param name="FileName">File to save to</param>
        /// <returns>A bitmap object</returns>
        public static Bitmap Jitter(this Bitmap OriginalImage, int MaxJitter = 5, string FileName = "")
        {
            if (OriginalImage == null)
                throw new ArgumentNullException("OriginalImage");
            ImageFormat FormatUsing = FileName.GetImageFormat();
            Bitmap NewBitmap = new Bitmap(OriginalImage, OriginalImage.Width, OriginalImage.Height);
            BitmapData NewData = NewBitmap.LockImage();
            BitmapData OldData = OriginalImage.LockImage();
            int NewPixelSize = NewData.GetPixelSize();
            int OldPixelSize = OldData.GetPixelSize();
            Random.Random TempRandom = new Random.Random();
            for (int x = 0; x < NewBitmap.Width; ++x)
            {
                for (int y = 0; y < NewBitmap.Height; ++y)
                {
                    int NewX = TempRandom.Next(-MaxJitter, MaxJitter);
                    int NewY = TempRandom.Next(-MaxJitter, MaxJitter);
                    NewX += x;
                    NewY += y;
                    NewX = NewX.Clamp(NewBitmap.Width - 1, 0);
                    NewY = NewY.Clamp(NewBitmap.Height - 1, 0);

                    NewData.SetPixel(x, y, OldData.GetPixel(NewX, NewY, OldPixelSize), NewPixelSize);
                }
            }
            NewBitmap.UnlockImage(NewData);
            OriginalImage.UnlockImage(OldData);
            if (!string.IsNullOrEmpty(FileName))
                NewBitmap.Save(FileName, FormatUsing);
            return NewBitmap;
        }

        #endregion

        #region KuwaharaBlur

        /// <summary>
        /// Does smoothing using a kuwahara blur
        /// </summary>
        /// <param name="OriginalImage">Image to manipulate</param>
        /// <param name="Size">Size of the aperture</param>
        /// <param name="FileName">File to save to</param>
        /// <returns>A bitmap object</returns>
        public static Bitmap KuwaharaBlur(this Bitmap OriginalImage, int Size = 3, string FileName = "")
        {
            if (OriginalImage == null)
                throw new ArgumentNullException("OriginalImage");
            ImageFormat FormatUsing = FileName.GetImageFormat();
            Bitmap NewBitmap = new Bitmap(OriginalImage.Width, OriginalImage.Height);
            BitmapData NewData = NewBitmap.LockImage();
            BitmapData OldData = OriginalImage.LockImage();
            int NewPixelSize = NewData.GetPixelSize();
            int OldPixelSize = OldData.GetPixelSize();
            int[] ApetureMinX = { -(Size / 2), 0, -(Size / 2), 0 };
            int[] ApetureMaxX = { 0, (Size / 2), 0, (Size / 2) };
            int[] ApetureMinY = { -(Size / 2), -(Size / 2), 0, 0 };
            int[] ApetureMaxY = { 0, 0, (Size / 2), (Size / 2) };
            for (int x = 0; x < NewBitmap.Width; ++x)
            {
                for (int y = 0; y < NewBitmap.Height; ++y)
                {
                    int[] RValues = { 0, 0, 0, 0 };
                    int[] GValues = { 0, 0, 0, 0 };
                    int[] BValues = { 0, 0, 0, 0 };
                    int[] NumPixels = { 0, 0, 0, 0 };
                    int[] MaxRValue = { 0, 0, 0, 0 };
                    int[] MaxGValue = { 0, 0, 0, 0 };
                    int[] MaxBValue = { 0, 0, 0, 0 };
                    int[] MinRValue = { 255, 255, 255, 255 };
                    int[] MinGValue = { 255, 255, 255, 255 };
                    int[] MinBValue = { 255, 255, 255, 255 };
                    for (int i = 0; i < 4; ++i)
                    {
                        for (int x2 = ApetureMinX[i]; x2 < ApetureMaxX[i]; ++x2)
                        {
                            int TempX = x + x2;
                            if (TempX >= 0 && TempX < NewBitmap.Width)
                            {
                                for (int y2 = ApetureMinY[i]; y2 < ApetureMaxY[i]; ++y2)
                                {
                                    int TempY = y + y2;
                                    if (TempY >= 0 && TempY < NewBitmap.Height)
                                    {
                                        Color TempColor = OldData.GetPixel(TempX, TempY, OldPixelSize);
                                        RValues[i] += TempColor.R;
                                        GValues[i] += TempColor.G;
                                        BValues[i] += TempColor.B;
                                        if (TempColor.R > MaxRValue[i])
                                            MaxRValue[i] = TempColor.R;
                                        else if (TempColor.R < MinRValue[i])
                                            MinRValue[i] = TempColor.R;

                                        if (TempColor.G > MaxGValue[i])
                                            MaxGValue[i] = TempColor.G;
                                        else if (TempColor.G < MinGValue[i])
                                            MinGValue[i] = TempColor.G;

                                        if (TempColor.B > MaxBValue[i])
                                            MaxBValue[i] = TempColor.B;
                                        else if (TempColor.B < MinBValue[i])
                                            MinBValue[i] = TempColor.B;

                                        ++NumPixels[i];
                                    }
                                }
                            }
                        }
                    }
                    int j = 0;
                    int MinDifference = 10000;
                    for (int i = 0; i < 4; ++i)
                    {
                        int CurrentDifference = (MaxRValue[i] - MinRValue[i]) + (MaxGValue[i] - MinGValue[i]) + (MaxBValue[i] - MinBValue[i]);
                        if (CurrentDifference < MinDifference && NumPixels[i] > 0)
                        {
                            j = i;
                            MinDifference = CurrentDifference;
                        }
                    }

                    Color MeanPixel = Color.FromArgb(RValues[j] / NumPixels[j],
                        GValues[j] / NumPixels[j],
                        BValues[j] / NumPixels[j]);
                    NewData.SetPixel(x, y, MeanPixel, NewPixelSize);
                }
            }
            NewBitmap.UnlockImage(NewData);
            OriginalImage.UnlockImage(OldData);
            if (!string.IsNullOrEmpty(FileName))
                NewBitmap.Save(FileName, FormatUsing);
            return NewBitmap;
        }

        #endregion

        #region LaplaceEdgeDetection

        /// <summary>
        /// Laplace edge detection function
        /// </summary>
        /// <param name="Image">Image to manipulate</param>
        /// <param name="FileName">File to save to</param>
        /// <returns>A bitmap object</returns>
        public static Bitmap LaplaceEdgeDetection(this Bitmap Image, string FileName = "")
        {
            if (Image == null)
                throw new ArgumentNullException("Image");
            ImageFormat FormatUsing = FileName.GetImageFormat();
            using (Bitmap TempImage = Image.BlackAndWhite())
            {
                Filter TempFilter = new Filter(5, 5);
                TempFilter.MyFilter[0, 0] = -1;
                TempFilter.MyFilter[0, 1] = -1;
                TempFilter.MyFilter[0, 2] = -1;
                TempFilter.MyFilter[0, 3] = -1;
                TempFilter.MyFilter[0, 4] = -1;
                TempFilter.MyFilter[1, 0] = -1;
                TempFilter.MyFilter[1, 1] = -1;
                TempFilter.MyFilter[1, 2] = -1;
                TempFilter.MyFilter[1, 3] = -1;
                TempFilter.MyFilter[1, 4] = -1;
                TempFilter.MyFilter[2, 0] = -1;
                TempFilter.MyFilter[2, 1] = -1;
                TempFilter.MyFilter[2, 2] = 24;
                TempFilter.MyFilter[2, 3] = -1;
                TempFilter.MyFilter[2, 4] = -1;
                TempFilter.MyFilter[3, 0] = -1;
                TempFilter.MyFilter[3, 1] = -1;
                TempFilter.MyFilter[3, 2] = -1;
                TempFilter.MyFilter[3, 3] = -1;
                TempFilter.MyFilter[3, 4] = -1;
                TempFilter.MyFilter[4, 0] = -1;
                TempFilter.MyFilter[4, 1] = -1;
                TempFilter.MyFilter[4, 2] = -1;
                TempFilter.MyFilter[4, 3] = -1;
                TempFilter.MyFilter[4, 4] = -1;
                using (Bitmap NewImage = TempFilter.ApplyFilter(TempImage))
                {
                    Bitmap NewBitmap = NewImage.Negative();
                    if (!string.IsNullOrEmpty(FileName))
                        NewBitmap.Save(FileName, FormatUsing);
                    return NewBitmap;
                }
            }
        }

        #endregion

        #region LockImage

        /// <summary>
        /// Locks an image
        /// </summary>
        /// <param name="Image">Image to lock</param>
        /// <returns>The bitmap data for the image</returns>
        public static BitmapData LockImage(this Bitmap Image)
        {
            if (Image == null)
                throw new ArgumentNullException("Image");
            return Image.LockBits(new Rectangle(0, 0, Image.Width, Image.Height),
                ImageLockMode.ReadWrite, Image.PixelFormat);
        }

        #endregion

        #region Map

        private static int Map(int Value, int Min, int Max)
        {
            double TempVal = (Value - Min);
            TempVal /= (double)(Max - Min);
            return (int)(TempVal * 255);
        }

        #endregion

        #region MedianFilter

        /// <summary>
        /// Does smoothing using a median filter
        /// </summary>
        /// <param name="OriginalImage">Image to manipulate</param>
        /// <param name="FileName">File to save to</param>
        /// <param name="Size">Size of the aperture</param>
        /// <returns>A bitmap image</returns>
        public static Bitmap MedianFilter(this Bitmap OriginalImage, int Size = 3, string FileName = "")
        {
            if (OriginalImage == null)
                throw new ArgumentNullException("OriginalImage");
            ImageFormat FormatUsing = FileName.GetImageFormat();
            Bitmap NewBitmap = new Bitmap(OriginalImage.Width, OriginalImage.Height);
            BitmapData NewData = NewBitmap.LockImage();
            BitmapData OldData = OriginalImage.LockImage();
            int NewPixelSize = NewData.GetPixelSize();
            int OldPixelSize = OldData.GetPixelSize();
            int ApetureMin = -(Size / 2);
            int ApetureMax = (Size / 2);
            for (int x = 0; x < NewBitmap.Width; ++x)
            {
                for (int y = 0; y < NewBitmap.Height; ++y)
                {
                    List<int> RValues = new List<int>();
                    List<int> GValues = new List<int>();
                    List<int> BValues = new List<int>();
                    for (int x2 = ApetureMin; x2 < ApetureMax; ++x2)
                    {
                        int TempX = x + x2;
                        if (TempX >= 0 && TempX < NewBitmap.Width)
                        {
                            for (int y2 = ApetureMin; y2 < ApetureMax; ++y2)
                            {
                                int TempY = y + y2;
                                if (TempY >= 0 && TempY < NewBitmap.Height)
                                {
                                    Color TempColor = OldData.GetPixel(TempX, TempY, OldPixelSize);
                                    RValues.Add(TempColor.R);
                                    GValues.Add(TempColor.G);
                                    BValues.Add(TempColor.B);
                                }
                            }
                        }
                    }
                    Color MedianPixel = Color.FromArgb(RValues.Median(),
                        GValues.Median(),
                        BValues.Median());
                    NewData.SetPixel(x, y, MedianPixel, NewPixelSize);
                }
            }
            NewBitmap.UnlockImage(NewData);
            OriginalImage.UnlockImage(OldData);
            if (!string.IsNullOrEmpty(FileName))
                NewBitmap.Save(FileName, FormatUsing);
            return NewBitmap;
        }

        #endregion

        #region Negative

        /// <summary>
        /// gets the negative of the image
        /// </summary>
        /// <param name="OriginalImage">Image to manipulate</param>
        /// <param name="FileName">File to save to</param>
        /// <returns>A bitmap image</returns>
        public static Bitmap Negative(this Bitmap OriginalImage, string FileName = "")
        {
            if (OriginalImage == null)
                throw new ArgumentNullException("OriginalImage");
            ImageFormat FormatUsing = FileName.GetImageFormat();
            Bitmap NewBitmap = new Bitmap(OriginalImage.Width, OriginalImage.Height);
            BitmapData NewData = NewBitmap.LockImage();
            BitmapData OldData = OriginalImage.LockImage();
            int NewPixelSize = NewData.GetPixelSize();
            int OldPixelSize = OldData.GetPixelSize();
            for (int x = 0; x < NewBitmap.Width; ++x)
            {
                for (int y = 0; y < NewBitmap.Height; ++y)
                {
                    Color CurrentPixel = OldData.GetPixel(x, y, OldPixelSize);
                    Color TempValue = Color.FromArgb(255 - CurrentPixel.R, 255 - CurrentPixel.G, 255 - CurrentPixel.B);
                    NewData.SetPixel(x, y, TempValue, NewPixelSize);
                }
            }
            NewBitmap.UnlockImage(NewData);
            OriginalImage.UnlockImage(OldData);
            if (!string.IsNullOrEmpty(FileName))
                NewBitmap.Save(FileName, FormatUsing);
            return NewBitmap;
        }

        #endregion

        #region Or

        /// <summary>
        /// Ors two images
        /// </summary>
        /// <param name="Image1">Image to manipulate</param>
        /// <param name="Image2">Image to manipulate</param>
        /// <param name="FileName">File to save to</param>
        /// <returns>A bitmap image</returns>
        public static Bitmap Or(this Bitmap Image1, Bitmap Image2, string FileName = "")
        {
            if (Image1 == null)
                throw new ArgumentNullException("Image1");
            if (Image2 == null)
                throw new ArgumentNullException("Image2");
            ImageFormat FormatUsing = FileName.GetImageFormat();
            Bitmap NewBitmap = new Bitmap(Image1.Width, Image1.Height);
            BitmapData NewData = NewBitmap.LockImage();
            BitmapData OldData1 = Image1.LockImage();
            BitmapData OldData2 = Image2.LockImage();
            int NewPixelSize = NewData.GetPixelSize();
            int OldPixelSize1 = OldData1.GetPixelSize();
            int OldPixelSize2 = OldData2.GetPixelSize();
            for (int x = 0; x < NewBitmap.Width; ++x)
            {
                for (int y = 0; y < NewBitmap.Height; ++y)
                {
                    Color Pixel1 = OldData1.GetPixel(x, y, OldPixelSize1);
                    Color Pixel2 = OldData2.GetPixel(x, y, OldPixelSize2);
                    NewData.SetPixel(x, y,
                        Color.FromArgb(Pixel1.R | Pixel2.R,
                            Pixel1.G | Pixel2.G,
                            Pixel1.B | Pixel2.B),
                        NewPixelSize);
                }
            }
            NewBitmap.UnlockImage(NewData);
            Image1.UnlockImage(OldData1);
            Image2.UnlockImage(OldData2);
            if (!string.IsNullOrEmpty(FileName))
                NewBitmap.Save(FileName, FormatUsing);
            return NewBitmap;
        }

        #endregion

        #region Pixelate

        /// <summary>
        /// Pixelates an image
        /// </summary>
        /// <param name="OriginalImage">Image to pixelate</param>
        /// <param name="PixelSize">Size of the "pixels" in pixels</param>
        /// <param name="FileName">File to save to</param>
        /// <returns>A bitmap image</returns>
        public static Bitmap Pixelate(this Bitmap OriginalImage, int PixelSize = 5, string FileName = "")
        {
            if (OriginalImage == null)
                throw new ArgumentNullException("OriginalImage");
            ImageFormat FormatUsing = FileName.GetImageFormat();
            Bitmap NewBitmap = new Bitmap(OriginalImage.Width, OriginalImage.Height);
            BitmapData NewData = NewBitmap.LockImage();
            BitmapData OldData = OriginalImage.LockImage();
            int NewPixelSize = NewData.GetPixelSize();
            int OldPixelSize = OldData.GetPixelSize();
            for (int x = 0; x < NewBitmap.Width; x += (PixelSize / 2))
            {
                int MinX = (x - (PixelSize / 2)).Clamp(NewBitmap.Width, 0);
                int MaxX = (x + (PixelSize / 2)).Clamp(NewBitmap.Width, 0);
                for (int y = 0; y < NewBitmap.Height; y += (PixelSize / 2))
                {
                    int RValue = 0;
                    int GValue = 0;
                    int BValue = 0;
                    int MinY = (y - (PixelSize / 2)).Clamp(NewBitmap.Height, 0);
                    int MaxY = (y + (PixelSize / 2)).Clamp(NewBitmap.Height, 0);
                    for (int x2 = MinX; x2 < MaxX; ++x2)
                    {
                        for (int y2 = MinY; y2 < MaxY; ++y2)
                        {
                            Color Pixel = OldData.GetPixel(x2, y2, OldPixelSize);
                            RValue += Pixel.R;
                            GValue += Pixel.G;
                            BValue += Pixel.B;
                        }
                    }
                    RValue = RValue / (PixelSize * PixelSize);
                    GValue = GValue / (PixelSize * PixelSize);
                    BValue = BValue / (PixelSize * PixelSize);
                    Color TempPixel = Color.FromArgb(RValue, GValue, BValue);
                    for (int x2 = MinX; x2 < MaxX; ++x2)
                    {
                        for (int y2 = MinY; y2 < MaxY; ++y2)
                        {
                            NewData.SetPixel(x2, y2, TempPixel, NewPixelSize);
                        }
                    }
                }
            }
            NewBitmap.UnlockImage(NewData);
            OriginalImage.UnlockImage(OldData);
            if (!string.IsNullOrEmpty(FileName))
                NewBitmap.Save(FileName, FormatUsing);
            return NewBitmap;
        }

        #endregion

        #region RedFilter

        /// <summary>
        /// Gets the Red filter for an image
        /// </summary>
        /// <param name="Image">Image to change</param>
        /// <param name="FileName">File to save to</param>
        /// <returns>A bitmap image</returns>
        public static Bitmap RedFilter(this Bitmap Image, string FileName = "")
        {
            if (Image == null)
                throw new ArgumentNullException("Image");
            ImageFormat FormatUsing = FileName.GetImageFormat();
            ColorMatrix TempMatrix = new ColorMatrix();
            TempMatrix.Matrix = new float[][]{
                            new float[] {1, 0, 0, 0, 0},
                            new float[] {0, 0, 0, 0, 0},
                            new float[] {0, 0, 0, 0, 0},
                            new float[] {0, 0, 0, 1, 0},
                            new float[] {0, 0, 0, 0, 1}
                        };
            Bitmap NewBitmap = TempMatrix.Apply(Image);
            if (!string.IsNullOrEmpty(FileName))
                NewBitmap.Save(FileName, FormatUsing);
            return NewBitmap;
        }

        #endregion

        #region Resize

        /// <summary>
        /// Resizes an image to a certain height
        /// </summary>
        /// <param name="Image">Image to resize</param>
        /// <param name="MaxSide">Max height/width for the final image</param>
        /// <param name="Quality">Quality of the resizing</param>
        /// <param name="FileName">File to save to</param>
        /// <returns>A bitmap object of the resized image</returns>
        public static Bitmap Resize(this Bitmap Image, int MaxSide, Quality Quality = Quality.Low, string FileName = "")
        {
            if (Image == null)
                throw new ArgumentNullException("Image");
            int NewWidth;
            int NewHeight;

            int OldWidth = Image.Width;
            int OldHeight = Image.Height;
            int OldMaxSide;
            OldMaxSide = (OldWidth >= OldHeight) ? OldWidth : OldHeight;

            double Coefficient = (double)MaxSide / (double)OldMaxSide;
            NewWidth = Convert.ToInt32(Coefficient * OldWidth);
            NewHeight = Convert.ToInt32(Coefficient * OldHeight);
            if (NewWidth <= 0)
                NewWidth = 1;
            if (NewHeight <= 0)
                NewHeight = 1;
            return Image.Resize(NewWidth, NewHeight, Quality, FileName);
        }

        /// <summary>
        /// Resizes an image to a certain height
        /// </summary>
        /// <param name="Image">Image to resize</param>
        /// <param name="Width">New width for the final image</param>
        /// <param name="Height">New height for the final image</param>
        /// <param name="Quality">Quality of the resizing</param>
        /// <param name="FileName">File to save to</param>
        /// <returns>A bitmap object of the resized image</returns>
        public static Bitmap Resize(this Bitmap Image, int Width, int Height, Quality Quality = Quality.Low, string FileName = "")
        {
            if (Image == null)
                throw new ArgumentNullException("Image");
            ImageFormat FormatUsing = FileName.GetImageFormat();
            Bitmap NewBitmap = new Bitmap(Width, Height);
            using (Graphics NewGraphics = Graphics.FromImage(NewBitmap))
            {
                if (Quality == Quality.High)
                {
                    NewGraphics.CompositingQuality = CompositingQuality.HighQuality;
                    NewGraphics.SmoothingMode = SmoothingMode.HighQuality;
                    NewGraphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                }
                else
                {
                    NewGraphics.CompositingQuality = CompositingQuality.HighSpeed;
                    NewGraphics.SmoothingMode = SmoothingMode.HighSpeed;
                    NewGraphics.InterpolationMode = InterpolationMode.NearestNeighbor;
                }
                NewGraphics.DrawImage(Image, new System.Drawing.Rectangle(0, 0, Width, Height));
            }
            if (!string.IsNullOrEmpty(FileName))
                NewBitmap.Save(FileName, FormatUsing);
            return NewBitmap;
        }

        #endregion

        #region Rotate

        /// <summary>
        /// Rotates an image
        /// </summary>
        /// <param name="Image">Image to rotate</param>
        /// <param name="DegreesToRotate">Degrees to rotate the image</param>
        /// <param name="FileName">File to save to</param>
        /// <returns>A bitmap object containing the rotated image</returns>
        public static Bitmap Rotate(this Bitmap Image, float DegreesToRotate, string FileName = "")
        {
            if (Image == null)
                throw new ArgumentNullException("Image");
            ImageFormat FormatUsing = FileName.GetImageFormat();
            Bitmap NewBitmap = new Bitmap(Image.Width, Image.Height);
            using (Graphics NewGraphics = Graphics.FromImage(NewBitmap))
            {
                NewGraphics.TranslateTransform((float)Image.Width / 2.0f, (float)Image.Height / 2.0f);
                NewGraphics.RotateTransform(DegreesToRotate);
                NewGraphics.TranslateTransform(-(float)Image.Width / 2.0f, -(float)Image.Height / 2.0f);
                NewGraphics.DrawImage(Image,
                    new System.Drawing.Rectangle(0, 0, Image.Width, Image.Height),
                    new System.Drawing.Rectangle(0, 0, Image.Width, Image.Height),
                    GraphicsUnit.Pixel);
            }
            if (!string.IsNullOrEmpty(FileName))
                NewBitmap.Save(FileName, FormatUsing);
            return NewBitmap;
        }

        #endregion

        #region SepiaTone

        /// <summary>
        /// Converts an image to sepia tone
        /// </summary>
        /// <param name="Image">Image to change</param>
        /// <param name="FileName">File to save to</param>
        /// <returns>A bitmap object of the sepia tone image</returns>
        public static Bitmap SepiaTone(this Bitmap Image, string FileName = "")
        {
            if (Image == null)
                throw new ArgumentNullException("Image");
            ImageFormat FormatUsing = FileName.GetImageFormat();
            ColorMatrix TempMatrix = new ColorMatrix();
            TempMatrix.Matrix = new float[][]{
                            new float[] {.393f, .349f, .272f, 0, 0},
                            new float[] {.769f, .686f, .534f, 0, 0},
                            new float[] {.189f, .168f, .131f, 0, 0},
                            new float[] {0, 0, 0, 1, 0},
                            new float[] {0, 0, 0, 0, 1}
                        };
            Bitmap NewBitmap = TempMatrix.Apply(Image);
            if (!string.IsNullOrEmpty(FileName))
                NewBitmap.Save(FileName, FormatUsing);
            return NewBitmap;
        }

        #endregion

        #region SetPixel

        /// <summary>
        /// Sets a pixel at the x,y coords
        /// </summary>
        /// <param name="Data">Bitmap data</param>
        /// <param name="x">X coord</param>
        /// <param name="y">Y coord</param>
        /// <param name="PixelColor">Pixel color information</param>
        /// <param name="PixelSizeInBytes">Pixel size in bytes</param>
        public static unsafe void SetPixel(this BitmapData Data, int x, int y, Color PixelColor, int PixelSizeInBytes)
        {
            if (Data == null)
                throw new ArgumentNullException("Data");
            if (PixelColor == null)
                throw new ArgumentNullException("PixelColor");
            byte* DataPointer = (byte*)Data.Scan0;
            DataPointer = DataPointer + (y * Data.Stride) + (x * PixelSizeInBytes);
            if (PixelSizeInBytes == 3)
            {
                DataPointer[2] = PixelColor.R;
                DataPointer[1] = PixelColor.G;
                DataPointer[0] = PixelColor.B;
                return;
            }
            DataPointer[3] = PixelColor.A;
            DataPointer[2] = PixelColor.R;
            DataPointer[1] = PixelColor.G;
            DataPointer[0] = PixelColor.B;
        }

        #endregion

        #region Sharpen

        /// <summary>
        /// Sharpens an image
        /// </summary>
        /// <param name="Image">Image to manipulate</param>
        /// <param name="FileName">File to save to</param>
        /// <returns>A bitmap image</returns>
        public static Bitmap Sharpen(this Bitmap Image,string FileName="")
        {
            if (Image == null)
                throw new ArgumentNullException("Image");
            ImageFormat FormatUsing = FileName.GetImageFormat();
            Filter TempFilter = new Filter(3, 3);
            TempFilter.MyFilter[0, 0] = -1;
            TempFilter.MyFilter[0, 2] = -1;
            TempFilter.MyFilter[2, 0] = -1;
            TempFilter.MyFilter[2, 2] = -1;
            TempFilter.MyFilter[0, 1] = -2;
            TempFilter.MyFilter[1, 0] = -2;
            TempFilter.MyFilter[2, 1] = -2;
            TempFilter.MyFilter[1, 2] = -2;
            TempFilter.MyFilter[1, 1] = 16;
            Bitmap NewBitmap= TempFilter.ApplyFilter(Image);
            if (!string.IsNullOrEmpty(FileName))
                NewBitmap.Save(FileName, FormatUsing);
            return NewBitmap;
        }

        #endregion

        #region SharpenLess

        /// <summary>
        /// Sharpens an image
        /// </summary>
        /// <param name="Image">Image to manipulate</param>
        /// <param name="FileName">File to save to</param>
        /// <returns>A bitmap image</returns>
        public static Bitmap SharpenLess(this Bitmap Image, string FileName = "")
        {
            if (Image == null)
                throw new ArgumentNullException("Image");
            ImageFormat FormatUsing = FileName.GetImageFormat();
            Filter TempFilter = new Filter(3, 3);
            TempFilter.MyFilter[0, 0] = -1;
            TempFilter.MyFilter[0, 1] = 0;
            TempFilter.MyFilter[0, 2] = -1;
            TempFilter.MyFilter[1, 0] = 0;
            TempFilter.MyFilter[1, 1] = 7;
            TempFilter.MyFilter[1, 2] = 0;
            TempFilter.MyFilter[2, 0] = -1;
            TempFilter.MyFilter[2, 1] = 0;
            TempFilter.MyFilter[2, 2] = -1;
            Bitmap NewBitmap = TempFilter.ApplyFilter(Image);
            if (!string.IsNullOrEmpty(FileName))
                NewBitmap.Save(FileName, FormatUsing);
            return NewBitmap;
        }
        
        #endregion

        #region SinWave

        /// <summary>
        /// Does a "wave" effect on the image
        /// </summary>
        /// <param name="OriginalImage">Image to manipulate</param>
        /// <param name="Amplitude">Amplitude of the sine wave</param>
        /// <param name="Frequency">Frequency of the sine wave</param>
        /// <param name="XDirection">Determines if this should be done in the X direction</param>
        /// <param name="YDirection">Determines if this should be done in the Y direction</param>
        /// <param name="FileName">File to save to</param>
        /// <returns>A bitmap which has been modified</returns>
        public static Bitmap SinWave(this Bitmap OriginalImage, float Amplitude, float Frequency, bool XDirection, bool YDirection, string FileName = "")
        {
            if (OriginalImage == null)
                throw new ArgumentNullException("OriginalImage");
            ImageFormat FormatUsing = FileName.GetImageFormat();
            Bitmap NewBitmap = new Bitmap(OriginalImage.Width, OriginalImage.Height);
            BitmapData NewData = NewBitmap.LockImage();
            BitmapData OldData = OriginalImage.LockImage();
            int NewPixelSize = NewData.GetPixelSize();
            int OldPixelSize = OldData.GetPixelSize();
            for (int x = 0; x < NewBitmap.Width; ++x)
            {
                for (int y = 0; y < NewBitmap.Height; ++y)
                {
                    double Value1 = 0;
                    double Value2 = 0;
                    if (YDirection)
                        Value1 = System.Math.Sin(((x * Frequency) * System.Math.PI) / 180.0d) * Amplitude;
                    if (XDirection)
                        Value2 = System.Math.Sin(((y * Frequency) * System.Math.PI) / 180.0d) * Amplitude;
                    Value1 = y - (int)Value1;
                    Value2 = x - (int)Value2;
                    while (Value1 < 0)
                        Value1 += NewBitmap.Height;
                    while (Value2 < 0)
                        Value2 += NewBitmap.Width;
                    while (Value1 >= NewBitmap.Height)
                        Value1 -= NewBitmap.Height;
                    while (Value2 >= NewBitmap.Width)
                        Value2 -= NewBitmap.Width;
                    NewData.SetPixel(x, y,
                        OldData.GetPixel((int)Value2, (int)Value1, OldPixelSize),
                        NewPixelSize);
                }
            }
            NewBitmap.UnlockImage(NewData);
            OriginalImage.UnlockImage(OldData);
            if (!string.IsNullOrEmpty(FileName))
                NewBitmap.Save(FileName, FormatUsing);
            return NewBitmap;
        }

        #endregion

        #region SobelEdgeDetection

        /// <summary>
        /// Sobel edge detection function
        /// </summary>
        /// <param name="Image">Image to manipulate</param>
        /// <param name="FileName">File to save to</param>
        /// <returns>A bitmap image</returns>
        public static Bitmap SobelEdgeDetection(this Bitmap Input, string FileName = "")
        {
            if (Input == null)
                throw new ArgumentNullException("Input");
            ImageFormat FormatUsing = FileName.GetImageFormat();
            using (Bitmap TempImage = Input.BlackAndWhite())
            {
                Filter TempFilter = new Filter(3, 3);
                TempFilter.MyFilter[0, 0] = -1;
                TempFilter.MyFilter[0, 1] = 0;
                TempFilter.MyFilter[0, 2] = 1;
                TempFilter.MyFilter[1, 0] = -2;
                TempFilter.MyFilter[1, 1] = 0;
                TempFilter.MyFilter[1, 2] = 2;
                TempFilter.MyFilter[2, 0] = -1;
                TempFilter.MyFilter[2, 1] = 0;
                TempFilter.MyFilter[2, 2] = 1;
                TempFilter.Absolute = true;
                using (Bitmap TempImageX = TempFilter.ApplyFilter(TempImage))
                {
                    TempFilter = new Filter(3, 3);
                    TempFilter.MyFilter[0, 0] = 1;
                    TempFilter.MyFilter[0, 1] = 2;
                    TempFilter.MyFilter[0, 2] = 1;
                    TempFilter.MyFilter[1, 0] = 0;
                    TempFilter.MyFilter[1, 1] = 0;
                    TempFilter.MyFilter[1, 2] = 0;
                    TempFilter.MyFilter[2, 0] = -1;
                    TempFilter.MyFilter[2, 1] = -2;
                    TempFilter.MyFilter[2, 2] = -1;
                    TempFilter.Absolute = true;
                    using (Bitmap TempImageY = TempFilter.ApplyFilter(TempImage))
                    {
                        using (Bitmap NewBitmap = new Bitmap(TempImage.Width, TempImage.Height))
                        {
                            BitmapData NewData = NewBitmap.LockImage();
                            BitmapData OldData1 = TempImageX.LockImage();
                            BitmapData OldData2 = TempImageY.LockImage();
                            int NewPixelSize = NewData.GetPixelSize();
                            int OldPixelSize1 = OldData1.GetPixelSize();
                            int OldPixelSize2 = OldData2.GetPixelSize();
                            for (int x = 0; x < NewBitmap.Width; ++x)
                            {
                                for (int y = 0; y < NewBitmap.Height; ++y)
                                {
                                    Color Pixel1 = OldData1.GetPixel(x, y, OldPixelSize1);
                                    Color Pixel2 = OldData2.GetPixel(x, y, OldPixelSize2);
                                    NewData.SetPixel(x, y,
                                        Color.FromArgb((Pixel1.R + Pixel2.R).Clamp(255, 0),
                                            (Pixel1.G + Pixel2.G).Clamp(255, 0),
                                            (Pixel1.B + Pixel2.B).Clamp(255, 0)),
                                        NewPixelSize);
                                }
                            }
                            NewBitmap.UnlockImage(NewData);
                            TempImageX.UnlockImage(OldData1);
                            TempImageY.UnlockImage(OldData2);
                            Bitmap NewBitmap2 = NewBitmap.Negative();
                            if (!string.IsNullOrEmpty(FileName))
                                NewBitmap2.Save(FileName, FormatUsing);
                            return NewBitmap2;
                        }
                    }
                }
            }
        }

        #endregion

        #region SobelEmboss

        /// <summary>
        /// Sobel emboss function
        /// </summary>
        /// <param name="Image">Image to manipulate</param>
        /// <param name="FileName">File to save to</param>
        /// <returns>A bitmap image</returns>
        public static Bitmap SobelEmboss(this Bitmap Image, string FileName = "")
        {
            if (Image == null)
                throw new ArgumentNullException("Image");
            ImageFormat FormatUsing = FileName.GetImageFormat();
            Filter TempFilter = new Filter(3, 3);
            TempFilter.MyFilter[0, 0] = -1;
            TempFilter.MyFilter[0, 1] = 0;
            TempFilter.MyFilter[0, 2] = 1;
            TempFilter.MyFilter[1, 0] = -2;
            TempFilter.MyFilter[1, 1] = 0;
            TempFilter.MyFilter[1, 2] = 2;
            TempFilter.MyFilter[2, 0] = -1;
            TempFilter.MyFilter[2, 1] = 0;
            TempFilter.MyFilter[2, 2] = 1;
            TempFilter.Offset = 127;
            Bitmap NewBitmap= TempFilter.ApplyFilter(Image);
            if (!string.IsNullOrEmpty(FileName))
                NewBitmap.Save(FileName, FormatUsing);
            return NewBitmap;
        }

        #endregion

        #region SNNBlur

        /// <summary>
        /// Does smoothing using a SNN blur
        /// </summary>
        /// <param name="OriginalImage">Image to manipulate</param>
        /// <param name="FileName">File to save to</param>
        /// <param name="Size">Size of the aperture</param>
        /// <returns>The resulting bitmap</returns>
        public static Bitmap SNNBlur(this Bitmap OriginalImage, int Size = 3, string FileName = "")
        {
            if (OriginalImage == null)
                throw new ArgumentNullException("OriginalImage");
            ImageFormat FormatUsing = FileName.GetImageFormat();
            Bitmap NewBitmap = new Bitmap(OriginalImage.Width, OriginalImage.Height);
            BitmapData NewData = NewBitmap.LockImage();
            BitmapData OldData = OriginalImage.LockImage();
            int NewPixelSize = NewData.GetPixelSize();
            int OldPixelSize = OldData.GetPixelSize();
            int ApetureMinX = -(Size / 2);
            int ApetureMaxX = (Size / 2);
            int ApetureMinY = -(Size / 2);
            int ApetureMaxY = (Size / 2);
            for (int x = 0; x < NewBitmap.Width; ++x)
            {
                for (int y = 0; y < NewBitmap.Height; ++y)
                {
                    int RValue = 0;
                    int GValue = 0;
                    int BValue = 0;
                    int NumPixels = 0;
                    for (int x2 = ApetureMinX; x2 < ApetureMaxX; ++x2)
                    {
                        int TempX1 = x + x2;
                        int TempX2 = x - x2;
                        if (TempX1 >= 0 && TempX1 < NewBitmap.Width && TempX2 >= 0 && TempX2 < NewBitmap.Width)
                        {
                            for (int y2 = ApetureMinY; y2 < ApetureMaxY; ++y2)
                            {
                                int TempY1 = y + y2;
                                int TempY2 = y - y2;
                                if (TempY1 >= 0 && TempY1 < NewBitmap.Height && TempY2 >= 0 && TempY2 < NewBitmap.Height)
                                {
                                    Color TempColor = OldData.GetPixel(x, y, OldPixelSize);
                                    Color TempColor2 = OldData.GetPixel(TempX1, TempY1, OldPixelSize);
                                    Color TempColor3 = OldData.GetPixel(TempX2, TempY2, OldPixelSize);
                                    if (Distance(TempColor.R, TempColor2.R, TempColor.G, TempColor2.G, TempColor.B, TempColor2.B) <
                                        Distance(TempColor.R, TempColor3.R, TempColor.G, TempColor3.G, TempColor.B, TempColor3.B))
                                    {
                                        RValue += TempColor2.R;
                                        GValue += TempColor2.G;
                                        BValue += TempColor2.B;
                                    }
                                    else
                                    {
                                        RValue += TempColor3.R;
                                        GValue += TempColor3.G;
                                        BValue += TempColor3.B;
                                    }
                                    ++NumPixels;
                                }
                            }
                        }
                    }
                    Color MeanPixel = Color.FromArgb(RValue / NumPixels,
                        GValue / NumPixels,
                        BValue / NumPixels);
                    NewData.SetPixel(x, y, MeanPixel, NewPixelSize);
                }
            }
            NewBitmap.UnlockImage(NewData);
            OriginalImage.UnlockImage(OldData);
            if (!string.IsNullOrEmpty(FileName))
                NewBitmap.Save(FileName, FormatUsing);
            return NewBitmap;
        }

        #endregion

        #region StretchContrast

        /// <summary>
        /// Stretches the contrast
        /// </summary>
        /// <param name="OriginalImage">Image to manipulate</param>
        /// <param name="FileName">File to save to</param>
        /// <returns>A bitmap image</returns>
        public static Bitmap StretchContrast(this Bitmap OriginalImage, string FileName = "")
        {
            if (OriginalImage == null)
                throw new ArgumentNullException("OriginalImage");
            ImageFormat FormatUsing = FileName.GetImageFormat();
            Bitmap NewBitmap = new Bitmap(OriginalImage.Width, OriginalImage.Height);
            BitmapData NewData = NewBitmap.LockImage();
            BitmapData OldData = OriginalImage.LockImage();
            int NewPixelSize = NewData.GetPixelSize();
            int OldPixelSize = OldData.GetPixelSize();
            Color MinValue;
            Color MaxValue;
            GetMinMaxPixel(out MinValue, out MaxValue, OldData, OldPixelSize);
            for (int x = 0; x < NewBitmap.Width; ++x)
            {
                for (int y = 0; y < NewBitmap.Height; ++y)
                {
                    Color CurrentPixel = OldData.GetPixel(x, y, OldPixelSize);
                    Color TempValue = Color.FromArgb(Map(CurrentPixel.R, MinValue.R, MaxValue.R),
                        Map(CurrentPixel.G, MinValue.G, MaxValue.G),
                        Map(CurrentPixel.B, MinValue.B, MaxValue.B));
                    NewData.SetPixel(x, y, TempValue, NewPixelSize);
                }
            }
            NewBitmap.UnlockImage(NewData);
            OriginalImage.UnlockImage(OldData);
            if (!string.IsNullOrEmpty(FileName))
                NewBitmap.Save(FileName, FormatUsing);
            return NewBitmap;
        }

        #endregion

        #region Threshold

        /// <summary>
        /// Does threshold manipulation of the image
        /// </summary>
        /// <param name="OriginalImage">Image to transform</param>
        /// <param name="Threshold">Float defining the threshold at which to set the pixel to black vs white.</param>
        /// <param name="FileName">File to save to</param>
        /// <returns>A bitmap object containing the new image</returns>
        public static Bitmap Threshold(this Bitmap OriginalImage, float Threshold=0.5f,string FileName="")
        {
            if (OriginalImage == null)
                throw new ArgumentNullException("OriginalImage");
            ImageFormat FormatUsing = FileName.GetImageFormat();
            Bitmap NewBitmap = new Bitmap(OriginalImage.Width, OriginalImage.Height);
            BitmapData NewData = NewBitmap.LockImage();
            BitmapData OldData = OriginalImage.LockImage();
            int NewPixelSize = NewData.GetPixelSize();
            int OldPixelSize = OldData.GetPixelSize();
            for (int x = 0; x < OriginalImage.Width; ++x)
            {
                for (int y = 0; y < OriginalImage.Height; ++y)
                {
                    Color TempColor = OldData.GetPixel(x, y, OldPixelSize);
                    if ((TempColor.R + TempColor.G + TempColor.B) / 755.0f > Threshold)
                        NewData.SetPixel(x, y, Color.White, NewPixelSize);
                    else
                        NewData.SetPixel(x, y, Color.Black, NewPixelSize);
                }
            }
            NewBitmap.UnlockImage(NewData);
            OriginalImage.UnlockImage(OldData);
            if (!string.IsNullOrEmpty(FileName))
                NewBitmap.Save(FileName, FormatUsing);
            return NewBitmap;
        }

        #endregion

        #region ToBase64

        /// <summary>
        /// Converts an image to a base64 string and returns it
        /// </summary>
        /// <param name="Image">Image to convert</param>
        /// <param name="DesiredFormat">Desired image format (defaults to Jpeg)</param>
        /// <returns>The image in base64 string format</returns>
        public static string ToBase64(this Bitmap Image,ImageFormat DesiredFormat=null)
        {
            DesiredFormat = DesiredFormat.NullCheck(ImageFormat.Jpeg);
            using (MemoryStream Stream = new MemoryStream())
            {
                Image.Save(Stream, DesiredFormat);
                return Stream.ToArray().ToBase64String();
            }
        }

        #endregion

        #region Turbulence

        /// <summary>
        /// Does turbulence manipulation of the image
        /// </summary>
        /// <param name="OriginalImage">Image to transform</param>
        /// <param name="Roughness">Roughness of the movement</param>
        /// <param name="Power">How strong the movement is</param>
        /// <param name="Seed">Random seed</param>
        /// <param name="FileName">File to save to</param>
        /// <returns>A bitmap object containing the new image</returns>
        public static Bitmap Turbulence(this Bitmap OriginalImage, int Roughness = 8, float Power = 5.0f, int Seed = 25123864, string FileName = "")
        {
            if (OriginalImage == null)
                throw new ArgumentNullException("OriginalImage");
            ImageFormat FormatUsing = FileName.GetImageFormat();
            int Width = OriginalImage.Width;
            int Height = OriginalImage.Height;
            BitmapData OriginalData = OriginalImage.LockImage();
            int OriginalPixelSize = OriginalData.GetPixelSize();
            Bitmap NewBitmap = new Bitmap(Width, Height);
            BitmapData ReturnData = NewBitmap.LockImage();
            int ReturnPixelSize = ReturnData.GetPixelSize();
            using (Bitmap XNoise = PerlinNoise.Generate(Width, Height, 255, 0, 0.0625f, 1.0f, 0.5f, Roughness, Seed))
            {
                BitmapData XNoiseData = XNoise.LockImage();
                int XNoisePixelSize = XNoiseData.GetPixelSize();
                using (Bitmap YNoise = PerlinNoise.Generate(Width, Height, 255, 0, 0.0625f, 1.0f, 0.5f, Roughness, Seed * 2))
                {
                    BitmapData YNoiseData = YNoise.LockImage();
                    int YNoisePixelSize = YNoiseData.GetPixelSize();
                    for (int y = 0; y < Height; ++y)
                    {
                        for (int x = 0; x < Width; ++x)
                        {
                            float XDistortion = x + (GetHeight(x, y, XNoiseData, XNoisePixelSize) * Power);
                            float YDistortion = y + (GetHeight(x, y, YNoiseData, YNoisePixelSize) * Power);
                            int X1 = ((int)XDistortion).Clamp(Width - 1, 0);
                            int Y1 = ((int)YDistortion).Clamp(Height - 1, 0);
                            ReturnData.SetPixel(x, y, OriginalData.GetPixel(X1, Y1, OriginalPixelSize), ReturnPixelSize);
                        }
                    }
                    YNoise.UnlockImage(YNoiseData);
                }
                XNoise.UnlockImage(XNoiseData);
            }
            NewBitmap.UnlockImage(ReturnData);
            UnlockImage(OriginalImage, OriginalData);
            if (!string.IsNullOrEmpty(FileName))
                NewBitmap.Save(FileName, FormatUsing);
            return NewBitmap;
        }

        #endregion

        #region UnlockImage

        /// <summary>
        /// Unlocks the image's data
        /// </summary>
        /// <param name="Image">Image to unlock</param>
        /// <param name="ImageData">The image data</param>
        /// <returns>Returns the image</returns>
        public static Bitmap UnlockImage(this Bitmap Image, BitmapData ImageData)
        {
            if (Image == null)
                throw new ArgumentNullException("Image");
            if (ImageData == null)
                throw new ArgumentNullException("ImageData");
            Image.UnlockBits(ImageData);
            return Image;
        }

        #endregion

        #region Watermark

        /// <summary>
        /// Adds a watermark to an image
        /// </summary>
        /// <param name="Image">image to add the watermark to</param>
        /// <param name="WatermarkImage">Watermark image</param>
        /// <param name="Opacity">Opacity of the watermark (1.0 to 0.0 with 1 being completely visible and 0 being invisible)</param>
        /// <param name="X">X position in pixels for the watermark</param>
        /// <param name="Y">Y position in pixels for the watermark</param>
        /// <param name="KeyColor">Transparent color used in watermark image, set to null if not used</param>
        /// <param name="FileName">File to save to</param>
        /// <returns>The results in the form of a bitmap object</returns>
        public static Bitmap Watermark(this Bitmap Image, Bitmap WatermarkImage, float Opacity, int X, int Y, Color KeyColor, string FileName = "")
        {
            if (Image == null)
                throw new ArgumentNullException("Image");
            if (WatermarkImage == null)
                throw new ArgumentNullException("WatermarkImage");
            ImageFormat FormatUsing = FileName.GetImageFormat();
            Bitmap NewBitmap = new Bitmap(Image, Image.Width, Image.Height);
            using (Graphics NewGraphics = Graphics.FromImage(NewBitmap))
            {
                float[][] FloatColorMatrix ={
                            new float[] {1, 0, 0, 0, 0},
                            new float[] {0, 1, 0, 0, 0},
                            new float[] {0, 0, 1, 0, 0},
                            new float[] {0, 0, 0, Opacity, 0},
                            new float[] {0, 0, 0, 0, 1}
                        };

                System.Drawing.Imaging.ColorMatrix NewColorMatrix = new System.Drawing.Imaging.ColorMatrix(FloatColorMatrix);
                using (ImageAttributes Attributes = new ImageAttributes())
                {
                    Attributes.SetColorMatrix(NewColorMatrix);
                    if (KeyColor != null)
                    {
                        Attributes.SetColorKey(KeyColor, KeyColor);
                    }
                    NewGraphics.DrawImage(WatermarkImage,
                        new System.Drawing.Rectangle(X, Y, WatermarkImage.Width, WatermarkImage.Height),
                        0, 0, WatermarkImage.Width, WatermarkImage.Height,
                        GraphicsUnit.Pixel,
                        Attributes);
                }
            }
            if (!string.IsNullOrEmpty(FileName))
                NewBitmap.Save(FileName, FormatUsing);
            return NewBitmap;
        }

        #endregion

        #region Xor

        /// <summary>
        /// Xors two images
        /// </summary>
        /// <param name="Image1">Image to manipulate</param>
        /// <param name="Image2">Image to manipulate</param>
        /// <param name="FileName">File to save to</param>
        /// <returns>A bitmap image</returns>
        public static Bitmap Xor(this Bitmap Image1, Bitmap Image2, string FileName = "")
        {
            if (Image1 == null)
                throw new ArgumentNullException("Image1");
            if (Image2 == null)
                throw new ArgumentNullException("Image2");
            ImageFormat FormatUsing = FileName.GetImageFormat();
            Bitmap NewBitmap = new Bitmap(Image1.Width, Image1.Height);
            BitmapData NewData = NewBitmap.LockImage();
            BitmapData OldData1 = Image1.LockImage();
            BitmapData OldData2 = Image2.LockImage();
            int NewPixelSize = NewData.GetPixelSize();
            int OldPixelSize1 = OldData1.GetPixelSize();
            int OldPixelSize2 = OldData2.GetPixelSize();
            for (int x = 0; x < NewBitmap.Width; ++x)
            {
                for (int y = 0; y < NewBitmap.Height; ++y)
                {
                    Color Pixel1 = OldData1.GetPixel(x, y, OldPixelSize1);
                    Color Pixel2 = OldData2.GetPixel(x, y, OldPixelSize2);
                    NewData.SetPixel(x, y,
                        Color.FromArgb(Pixel1.R ^ Pixel2.R,
                            Pixel1.G ^ Pixel2.G,
                            Pixel1.B ^ Pixel2.B),
                        NewPixelSize);
                }
            }
            NewBitmap.UnlockImage(NewData);
            Image1.UnlockImage(OldData1);
            Image2.UnlockImage(OldData2);
            if (!string.IsNullOrEmpty(FileName))
                NewBitmap.Save(FileName, FormatUsing);
            return NewBitmap;
        }

        #endregion

        #endregion

        #region Enums

        /// <summary>
        /// Enum defining alignment
        /// </summary>
        public enum Align
        {
            Top,
            Bottom,
            Left,
            Right
        }

        /// <summary>
        /// Enum defining quality
        /// </summary>
        public enum Quality
        {
            High,
            Low
        }

        #endregion
    }
}