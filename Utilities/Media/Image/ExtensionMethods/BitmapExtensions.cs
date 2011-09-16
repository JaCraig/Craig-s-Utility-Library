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
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System;
using System.Collections.Generic;
using Utilities.Math.ExtensionMethods;
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
            ImageFormat FormatUsing = GetImageFormat(FileName);
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
            if(!string.IsNullOrEmpty(FileName))
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
            ImageFormat FormatUsing = GetImageFormat(FileName);
            float FinalValue = (float)Value / 255.0f;
            ColorMatrix TempMatrix = new ColorMatrix();
            TempMatrix.Matrix = new float[][]{
                            new float[] {1, 0, 0, 0, 0},
                            new float[] {0, 1, 0, 0, 0},
                            new float[] {0, 0, 1, 0, 0},
                            new float[] {0, 0, 0, 1, 0},
                            new float[] {FinalValue, FinalValue, FinalValue, 1, 1}
                        };
            Bitmap NewBitmap=TempMatrix.Apply(Image);
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
            ImageFormat FormatUsing = GetImageFormat(FileName);
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
                    Image.SetPixel(NewData, x, y,
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
        public static Bitmap AdjustGamma(this Bitmap OriginalImage, float Value = 1.0f,string FileName = "")
        {
            if (OriginalImage == null)
                throw new ArgumentNullException("OriginalImage");
            ImageFormat FormatUsing = GetImageFormat(FileName);
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
        public static Bitmap And(this Bitmap Image1, Bitmap Image2,string FileName="")
        {
            if (Image1 == null)
                throw new ArgumentNullException("Image1");
            if (Image2 == null)
                throw new ArgumentNullException("Image2");
            ImageFormat FormatUsing = GetImageFormat(FileName);
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
            ImageFormat FormatUsing = GetImageFormat(FileName);
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
            ImageFormat FormatUsing = GetImageFormat(FileName);
            ColorMatrix TempMatrix = new ColorMatrix();
            TempMatrix.Matrix = new float[][]{
                            new float[] {0, 0, 0, 0, 0},
                            new float[] {0, 0, 0, 0, 0},
                            new float[] {0, 0, 1, 0, 0},
                            new float[] {0, 0, 0, 1, 0},
                            new float[] {0, 0, 0, 0, 1}
                        };
            Bitmap NewBitmap= TempMatrix.Apply(Image);
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
            ImageFormat FormatUsing = GetImageFormat(FileName);
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
            ImageFormat FormatUsing = GetImageFormat(FileName);
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
            ImageFormat FormatUsing = GetImageFormat(FileName);
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
            ImageFormat FormatUsing = GetImageFormat(FileName);
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
