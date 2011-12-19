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
    /// Utility class used for image manipulation
    /// </summary>
    public static class Image
    {
        #region Static Functions
        /// <summary>
        /// Checks to make sure this is an image
        /// </summary>
        /// <param name="FileName">Name of the file to check</param>
        /// <returns>returns true if it is an image, false otherwise</returns>
        public static bool IsGraphic(string FileName)
        {
            System.Text.RegularExpressions.Regex Regex = new System.Text.RegularExpressions.Regex(@"\.gif$|\.jpg$|\.jpeg$|\.png$|\.bmp$", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            if (Regex.IsMatch(FileName))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// Returns the image format this file is using
        /// </summary>
        /// <param name="FileName"></param>
        /// <returns></returns>
        public static ImageFormat GetFormat(string FileName)
        {
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

        /// <summary>
        /// Gets the dimensions of an image
        /// </summary>
        /// <param name="FileName">File name</param>
        /// <param name="Width">Width of the image</param>
        /// <param name="Height">Height of the image</param>
        public static void GetDimensions(string FileName, out int Width, out int Height)
        {
            if (!IsGraphic(FileName))
            {
                Width = 0;
                Height = 0;
                return;
            }
            System.Drawing.Image TempImage = System.Drawing.Image.FromFile(FileName);
            Width = TempImage.Width;
            Height = TempImage.Height;
            TempImage.Dispose();
        }

        /// <summary>
        /// Gets the dimensions of an image
        /// </summary>
        /// <param name="Image">Image object</param>
        /// <param name="Width">Width of the image</param>
        /// <param name="Height">Height of the image</param>
        public static void GetDimensions(Bitmap Image, out int Width, out int Height)
        {
            if (Image == null)
            {
                Width = 0;
                Height = 0;
                return;
            }
            Width = Image.Width;
            Height = Image.Height;
        }

        /// <summary>
        /// Crops an image
        /// </summary>
        /// <param name="FileName">Name of the file to crop</param>
        /// <param name="NewFileName">The name to save the new file as</param>
        /// <param name="Width">Width of the cropped image</param>
        /// <param name="Height">Height of the cropped image</param>
        /// <param name="VAlignment">The verticle alignment of the cropping (top or bottom)</param>
        /// <param name="HAlignment">The horizontal alignment of the cropping (left or right)</param>
        public static void CropImage(string FileName, string NewFileName, int Width, int Height, Image.Align VAlignment, Image.Align HAlignment)
        {
            if (!IsGraphic(FileName))
                return;
            ImageFormat FormatUsing = GetFormat(NewFileName);
            System.Drawing.Bitmap CroppedBitmap = Image.CropImage(FileName, Width, Height, VAlignment, HAlignment);
            CroppedBitmap.Save(NewFileName, FormatUsing);
            CroppedBitmap.Dispose();
        }

        /// <summary>
        /// Crops an image
        /// </summary>
        /// <param name="FileName">Name of the file to crop</param>
        /// <param name="Width">Width of the cropped image</param>
        /// <param name="Height">Height of the cropped image</param>
        /// <param name="VAlignment">The verticle alignment of the cropping (top or bottom)</param>
        /// <param name="HAlignment">The horizontal alignment of the cropping (left or right)</param>
        /// <returns>A Bitmap object of the cropped image</returns>
        public static Bitmap CropImage(string FileName, int Width, int Height, Image.Align VAlignment, Image.Align HAlignment)
        {
            if (!IsGraphic(FileName))
                return new Bitmap(1, 1);
            System.Drawing.Image TempImage = System.Drawing.Image.FromFile(FileName);
            System.Drawing.Bitmap TempBitmap = new System.Drawing.Bitmap(TempImage, TempImage.Width, TempImage.Height);
            System.Drawing.Bitmap ReturnImage=Image.CropImage(TempBitmap, Width, Height, VAlignment, HAlignment);
            TempBitmap.Dispose();
            TempImage.Dispose();
            return ReturnImage;
        }

        /// <summary>
        /// Crops an image
        /// </summary>
        /// <param name="ImageUsing">Image to crop</param>
        /// <param name="Width">Width of the cropped image</param>
        /// <param name="Height">Height of the cropped image</param>
        /// <param name="VAlignment">The verticle alignment of the cropping (top or bottom)</param>
        /// <param name="HAlignment">The horizontal alignment of the cropping (left or right)</param>
        /// <returns>A Bitmap object of the cropped image</returns>
        public static Bitmap CropImage(Bitmap ImageUsing, int Width, int Height, Image.Align VAlignment, Image.Align HAlignment)
        {
            System.Drawing.Bitmap TempBitmap = ImageUsing;
            System.Drawing.Rectangle TempRectangle = new System.Drawing.Rectangle();
            TempRectangle.Height = Height;
            TempRectangle.Width = Width;
            if (VAlignment == Image.Align.Top)
            {
                TempRectangle.Y = 0;
            }
            else
            {
                TempRectangle.Y = TempBitmap.Height - Height;
                if (TempRectangle.Y < 0)
                    TempRectangle.Y = 0;
            }
            if (HAlignment == Image.Align.Left)
            {
                TempRectangle.X = 0;
            }
            else
            {
                TempRectangle.X = TempBitmap.Width - Width;
                if (TempRectangle.X < 0)
                    TempRectangle.X = 0;
            }
            System.Drawing.Bitmap CroppedBitmap = TempBitmap.Clone(TempRectangle, TempBitmap.PixelFormat);
            return CroppedBitmap;
        }

        /// <summary>
        /// Resizes an image to a certain height
        /// </summary>
        /// <param name="FileName">File to resize</param>
        /// <param name="NewFileName">Name to save the file to</param>
        /// <param name="MaxSide">Max height/width for the final image</param>
        public static void ResizeImage(string FileName, string NewFileName, int MaxSide)
        {
            try
            {
                if (!IsGraphic(FileName))
                    return;
                ImageFormat FormatUsing = GetFormat(NewFileName);
                System.Drawing.Bitmap TempBitmap = Image.ResizeImage(FileName, MaxSide);
                TempBitmap.Save(NewFileName, FormatUsing);
                TempBitmap.Dispose();
            }
            catch (Exception a)
            {
                throw a;
            }
        }

        /// <summary>
        /// Resizes an image to a certain height
        /// </summary>
        /// <param name="FileName">File to resize</param>
        /// <param name="MaxSide">Max height/width for the final image</param>
        /// <returns>A bitmap object of the resized image</returns>
        public static Bitmap ResizeImage(string FileName, int MaxSide)
        {
            try
            {
                if (!IsGraphic(FileName))
                    return new Bitmap(1, 1);
                System.Drawing.Image TempImage = System.Drawing.Image.FromFile(FileName);
                System.Drawing.Bitmap TempBitmap = new System.Drawing.Bitmap(TempImage, TempImage.Width, TempImage.Height);
                System.Drawing.Bitmap ReturnBitmap=Image.ResizeImage(TempBitmap, MaxSide);
                TempBitmap.Dispose();
                TempImage.Dispose();
                return ReturnBitmap;
            }
            catch (Exception a)
            {
                throw a;
            }
        }

        /// <summary>
        /// Resizes an image to a certain height
        /// </summary>
        /// <param name="Image">Image to resize</param>
        /// <param name="MaxSide">Max height/width for the final image</param>
        /// <returns>A bitmap object of the resized image</returns>
        public static Bitmap ResizeImage(Bitmap Image, int MaxSide)
        {
            try
            {
                System.Drawing.Image TempImage = Image;
                int NewWidth;
                int NewHeight;

                System.Drawing.Imaging.ImageFormat ImageFormat = TempImage.RawFormat;

                int OldWidth = TempImage.Width;
                int OldHeight = TempImage.Height;

                int OldMaxSide;

                if (OldWidth >= OldHeight)
                {
                    OldMaxSide = OldWidth;
                }
                else
                {
                    OldMaxSide = OldHeight;
                }

                if (OldMaxSide > MaxSide)
                {
                    double Coefficient = (double)MaxSide / (double)OldMaxSide;
                    NewWidth = Convert.ToInt32(Coefficient * OldWidth);
                    NewHeight = Convert.ToInt32(Coefficient * OldHeight);
                    if (NewWidth <= 0)
                        NewWidth = 1;
                    if (NewHeight <= 0)
                        NewHeight = 1;
                }
                else
                {
                    NewHeight = OldHeight;
                    NewWidth = OldWidth;
                }

                System.Drawing.Bitmap TempBitmap = new System.Drawing.Bitmap(TempImage, NewWidth, NewHeight);
                return TempBitmap;
            }
            catch (Exception a)
            {
                throw a;
            }
        }

        /// <summary>
        /// Draws text on an image within the bounding box specified.
        /// </summary>
        /// <param name="FileName">Name of the file to load</param>
        /// <param name="NewFileName">Name of the file to save to</param>
        /// <param name="TextToDraw">The text to draw on the image</param>
        /// <param name="FontToUse">Font in which to draw the text</param>
        /// <param name="BrushUsing">Defines the brush using</param>
        /// <param name="BoxToDrawWithin">Rectangle to draw the image within</param>
        public static void DrawText(string FileName, string NewFileName, string TextToDraw,
            Font FontToUse, Brush BrushUsing, RectangleF BoxToDrawWithin)
        {
            if (!IsGraphic(FileName))
                return;
            ImageFormat FormatUsing = GetFormat(NewFileName);
            System.Drawing.Bitmap TempBitmap = Image.DrawText(FileName, TextToDraw, FontToUse, BrushUsing, BoxToDrawWithin);
            TempBitmap.Save(NewFileName, FormatUsing);
            TempBitmap.Dispose();
        }

        /// <summary>
        /// Draws text on an image within the bounding box specified.
        /// </summary>
        /// <param name="FileName">Name of the file to load</param>
        /// <param name="TextToDraw">The text to draw on the image</param>
        /// <param name="FontToUse">Font in which to draw the text</param>
        /// <param name="BrushUsing">Defines the brush using</param>
        /// <param name="BoxToDrawWithin">Rectangle to draw the image within</param>
        /// <returns>A bitmap object with the text drawn on it</returns>
        public static Bitmap DrawText(string FileName, string TextToDraw,
            Font FontToUse, Brush BrushUsing, RectangleF BoxToDrawWithin)
        {
            if (!IsGraphic(FileName))
                return new Bitmap(1, 1);
            System.Drawing.Image TempImage = System.Drawing.Image.FromFile(FileName);
            System.Drawing.Bitmap TempBitmap = new System.Drawing.Bitmap(TempImage, TempImage.Width, TempImage.Height);
            System.Drawing.Bitmap ReturnBitmap = Image.DrawText(TempBitmap, TextToDraw, FontToUse, BrushUsing, BoxToDrawWithin);
            TempBitmap.Dispose();
            TempImage.Dispose();
            return ReturnBitmap;
        }

        /// <summary>
        /// Draws text on an image within the bounding box specified.
        /// </summary>
        /// <param name="Image">Image to draw on</param>
        /// <param name="TextToDraw">The text to draw on the image</param>
        /// <param name="FontToUse">Font in which to draw the text</param>
        /// <param name="BrushUsing">Defines the brush using</param>
        /// <param name="BoxToDrawWithin">Rectangle to draw the image within</param>
        /// <returns>A bitmap object with the text drawn on it</returns>
        public static Bitmap DrawText(Bitmap Image, string TextToDraw,
            Font FontToUse, Brush BrushUsing, RectangleF BoxToDrawWithin)
        {
            System.Drawing.Bitmap TempBitmap = new Bitmap(Image, Image.Width, Image.Height);
            System.Drawing.Graphics TempGraphics = System.Drawing.Graphics.FromImage(TempBitmap);

            TempGraphics.DrawString(TextToDraw, FontToUse, BrushUsing, BoxToDrawWithin);
            TempGraphics.Dispose();
            return TempBitmap;
        }

        /// <summary>
        /// Rotates an image
        /// </summary>
        /// <param name="FileName">Image to rotate</param>
        /// <param name="NewFileName">Location to save the image to</param>
        /// <param name="DegreesToRotate">Degrees to rotate the image</param>
        public static void Rotate(string FileName, string NewFileName, float DegreesToRotate)
        {
            if (!IsGraphic(FileName))
                return;
            ImageFormat FormatUsing = GetFormat(NewFileName);
            System.Drawing.Bitmap NewBitmap = Image.Rotate(FileName, DegreesToRotate);
            NewBitmap.Save(NewFileName, FormatUsing);
            NewBitmap.Dispose();
        }

        /// <summary>
        /// Rotates an image
        /// </summary>
        /// <param name="FileName">Image to rotate</param>
        /// <param name="DegreesToRotate">Degrees to rotate the image</param>
        /// <returns>A bitmap object containing the rotated image</returns>
        public static Bitmap Rotate(string FileName, float DegreesToRotate)
        {
            if (!IsGraphic(FileName))
                return new Bitmap(1, 1);
            System.Drawing.Image TempImage = System.Drawing.Image.FromFile(FileName);
            System.Drawing.Bitmap TempBitmap = new System.Drawing.Bitmap(TempImage, TempImage.Width, TempImage.Height);
            System.Drawing.Bitmap ReturnBitmap = Image.Rotate(TempBitmap, DegreesToRotate);
            TempBitmap.Dispose();
            TempImage.Dispose();
            return ReturnBitmap;
        }

        /// <summary>
        /// Rotates an image
        /// </summary>
        /// <param name="Image">Image to rotate</param>
        /// <param name="DegreesToRotate">Degrees to rotate the image</param>
        /// <returns>A bitmap object containing the rotated image</returns>
        public static Bitmap Rotate(Bitmap Image, float DegreesToRotate)
        {
            System.Drawing.Bitmap TempBitmap = Image;
            System.Drawing.Bitmap NewBitmap = new System.Drawing.Bitmap(TempBitmap.Width, TempBitmap.Height);
            System.Drawing.Graphics NewGraphics = System.Drawing.Graphics.FromImage(NewBitmap);

            NewGraphics.TranslateTransform((float)TempBitmap.Width / 2.0f, (float)TempBitmap.Height / 2.0f);
            NewGraphics.RotateTransform(DegreesToRotate);
            NewGraphics.TranslateTransform(-(float)TempBitmap.Width / 2.0f, -(float)TempBitmap.Height / 2.0f);
            NewGraphics.DrawImage(TempBitmap, new System.Drawing.Rectangle(0, 0, TempBitmap.Width, TempBitmap.Height), new System.Drawing.Rectangle(0, 0, TempBitmap.Width, TempBitmap.Height), System.Drawing.GraphicsUnit.Pixel);
            NewGraphics.Dispose();
            return NewBitmap;
        }


        /// <summary>
        /// Converts an image to black and white
        /// </summary>
        /// <param name="FileName">File to change</param>
        /// <param name="NewFileName">Location to save the black and white image to</param>
        public static void ConvertBlackAndWhite(string FileName, string NewFileName)
        {
            if (!IsGraphic(FileName))
                return;
            ImageFormat FormatUsing = GetFormat(NewFileName);
            System.Drawing.Bitmap NewBitmap = ConvertBlackAndWhite(FileName);
            NewBitmap.Save(NewFileName, FormatUsing);
            NewBitmap.Dispose();
        }

        /// <summary>
        /// Converts an image to black and white
        /// </summary>
        /// <param name="FileName">File to change</param>
        /// <returns>A bitmap object of the black and white image</returns>
        public static Bitmap ConvertBlackAndWhite(string FileName)
        {
            if (!IsGraphic(FileName))
                return new Bitmap(1, 1);
            System.Drawing.Image TempImage = System.Drawing.Image.FromFile(FileName);
            System.Drawing.Bitmap TempBitmap = new System.Drawing.Bitmap(TempImage, TempImage.Width, TempImage.Height);
            System.Drawing.Bitmap ReturnBitmap = Image.ConvertBlackAndWhite(TempBitmap);
            TempBitmap.Dispose();
            TempImage.Dispose();
            return ReturnBitmap;
        }

        /// <summary>
        /// Converts an image to black and white
        /// </summary>
        /// <param name="Image">Image to change</param>
        /// <returns>A bitmap object of the black and white image</returns>
        public static Bitmap ConvertBlackAndWhite(Bitmap Image)
        {
            System.Drawing.Bitmap TempBitmap = Image;

            System.Drawing.Bitmap NewBitmap = new System.Drawing.Bitmap(TempBitmap.Width, TempBitmap.Height);
            System.Drawing.Graphics NewGraphics = System.Drawing.Graphics.FromImage(NewBitmap);
            float[][] FloatColorMatrix ={
                    new float[] {.3f, .3f, .3f, 0, 0},
                    new float[] {.59f, .59f, .59f, 0, 0},
                    new float[] {.11f, .11f, .11f, 0, 0},
                    new float[] {0, 0, 0, 1, 0},
                    new float[] {0, 0, 0, 0, 1}
                };

            System.Drawing.Imaging.ColorMatrix NewColorMatrix = new System.Drawing.Imaging.ColorMatrix(FloatColorMatrix);
            System.Drawing.Imaging.ImageAttributes Attributes = new System.Drawing.Imaging.ImageAttributes();
            Attributes.SetColorMatrix(NewColorMatrix);
            NewGraphics.DrawImage(TempBitmap, new System.Drawing.Rectangle(0, 0, TempBitmap.Width, TempBitmap.Height), 0, 0, TempBitmap.Width, TempBitmap.Height, System.Drawing.GraphicsUnit.Pixel, Attributes);
            Attributes.Dispose();
            NewGraphics.Dispose();
            return NewBitmap;
        }

        /// <summary>
        /// Does threshold manipulation of the image
        /// </summary>
        /// <param name="FileName">File to change</param>
        /// <param name="Threshold">Float defining the threshold at which to set the pixel to black vs white.</param>
        /// <param name="NewFileName">Location to save the black and white image to</param>
        public static void Threshold(string FileName, string NewFileName, float Threshold)
        {
            if (!IsGraphic(FileName))
                return;
            ImageFormat FormatUsing = GetFormat(NewFileName);
            System.Drawing.Bitmap NewBitmap = Image.Threshold(FileName, Threshold);
            NewBitmap.Save(NewFileName, FormatUsing);
            NewBitmap.Dispose();
        }

        /// <summary>
        /// Does threshold manipulation of the image
        /// </summary>
        /// <param name="FileName">File to change</param>
        /// <param name="Threshold">Float defining the threshold at which to set the pixel to black vs white.</param>
        /// <returns>A bitmap object containing the new image</returns>
        public static Bitmap Threshold(string FileName, float Threshold)
        {
            if (!IsGraphic(FileName))
                return new Bitmap(1, 1);
            System.Drawing.Image TempImage = System.Drawing.Image.FromFile(FileName);
            System.Drawing.Bitmap TempBitmap = new System.Drawing.Bitmap(TempImage, TempImage.Width, TempImage.Height);
            System.Drawing.Bitmap ReturnBitmap = Image.Threshold(TempBitmap, Threshold);
            TempBitmap.Dispose();
            TempImage.Dispose();
            return ReturnBitmap;
        }

        /// <summary>
        /// Does threshold manipulation of the image
        /// </summary>
        /// <param name="Image">Image to transform</param>
        /// <param name="Threshold">Float defining the threshold at which to set the pixel to black vs white.</param>
        /// <returns>A bitmap object containing the new image</returns>
        public static Bitmap Threshold(Bitmap Image, float Threshold)
        {
            System.Drawing.Bitmap TempBitmap = Image;

            System.Drawing.Bitmap NewBitmap = new System.Drawing.Bitmap(TempBitmap.Width, TempBitmap.Height);
            System.Drawing.Graphics NewGraphics = System.Drawing.Graphics.FromImage(NewBitmap);
            for (int x = 0; x < TempBitmap.Width; ++x)
            {
                for (int y = 0; y < TempBitmap.Height; ++y)
                {
                    Color TempColor = TempBitmap.GetPixel(x, y);
                    if ((TempColor.R + TempColor.G + TempColor.B) / 755.0f > Threshold)
                    {
                        TempBitmap.SetPixel(x, y, Color.White);
                    }
                    else
                    {
                        TempBitmap.SetPixel(x, y, Color.Black);
                    }
                }
            }
            NewGraphics.DrawImage(TempBitmap, new System.Drawing.Rectangle(0, 0, TempBitmap.Width, TempBitmap.Height), 0, 0, TempBitmap.Width, TempBitmap.Height, System.Drawing.GraphicsUnit.Pixel);
            NewGraphics.Dispose();
            return NewBitmap;
        }

        /// <summary>
        /// Adds a watermark to an image
        /// </summary>
        /// <param name="FileName">File of the image to add the watermark to</param>
        /// <param name="WatermarkFileName">Watermark file name</param>
        /// <param name="NewFileName">Location to save the resulting image</param>
        /// <param name="Opacity">Opacity of the watermark (1.0 to 0.0 with 1 being completely visible and 0 being invisible)</param>
        /// <param name="X">X position in pixels for the watermark</param>
        /// <param name="KeyColor">Transparent color used in watermark image, set to null if not used</param>
        /// <param name="Y">Y position in pixels for the watermark</param>
        public static void Watermark(string FileName, string WatermarkFileName, string NewFileName, float Opacity, int X, int Y, Color KeyColor)
        {
            if (!IsGraphic(FileName) || !IsGraphic(WatermarkFileName))
                return;
            ImageFormat FormatUsing = GetFormat(NewFileName);
            System.Drawing.Bitmap NewBitmap = Image.Watermark(FileName, WatermarkFileName, Opacity, X, Y, KeyColor);
            NewBitmap.Save(NewFileName, FormatUsing);
            NewBitmap.Dispose();
        }

        /// <summary>
        /// Adds a watermark to an image
        /// </summary>
        /// <param name="FileName">File of the image to add the watermark to</param>
        /// <param name="WatermarkFileName">Watermark file name</param>
        /// <param name="Opacity">Opacity of the watermark (1.0 to 0.0 with 1 being completely visible and 0 being invisible)</param>
        /// <param name="X">X position in pixels for the watermark</param>
        /// <param name="Y">Y position in pixels for the watermark</param>
        /// <param name="KeyColor">Transparent color used in watermark image, set to null if not used</param>
        /// <returns>The results in the form of a bitmap object</returns>
        public static Bitmap Watermark(string FileName, string WatermarkFileName, float Opacity, int X, int Y, Color KeyColor)
        {
            if (!IsGraphic(FileName) || !IsGraphic(WatermarkFileName))
                return new Bitmap(1, 1);
            System.Drawing.Image TempImage = System.Drawing.Image.FromFile(FileName);
            System.Drawing.Image TempWatermarkImage = System.Drawing.Image.FromFile(WatermarkFileName);
            System.Drawing.Bitmap TempBitmap = new System.Drawing.Bitmap(TempImage, TempImage.Width, TempImage.Height);
            System.Drawing.Bitmap TempWatermarkBitmap = new System.Drawing.Bitmap(TempWatermarkImage, TempWatermarkImage.Width, TempWatermarkImage.Height);
            System.Drawing.Bitmap ReturnBitmap = Image.Watermark(TempBitmap, TempWatermarkBitmap, Opacity, X, Y, KeyColor);
            TempBitmap.Dispose();
            TempWatermarkBitmap.Dispose();
            TempWatermarkImage.Dispose();
            TempImage.Dispose();
            return ReturnBitmap;
        }

        /// <summary>
        /// Adds a watermark to an image
        /// </summary>
        /// <param name="Image">image to add the watermark to</param>
        /// <param name="WatermarkImage">Watermark image</param>
        /// <param name="Opacity">Opacity of the watermark (1.0 to 0.0 with 1 being completely visible and 0 being invisible)</param>
        /// <param name="X">X position in pixels for the watermark</param>
        /// <param name="Y">Y position in pixels for the watermark</param>
        /// <param name="KeyColor">Transparent color used in watermark image, set to null if not used</param>
        /// <returns>The results in the form of a bitmap object</returns>
        public static Bitmap Watermark(Bitmap Image, Bitmap WatermarkImage, float Opacity, int X, int Y, Color KeyColor)
        {
            System.Drawing.Bitmap TempBitmap = Image;

            System.Drawing.Bitmap NewBitmap = new System.Drawing.Bitmap(TempBitmap.Width, TempBitmap.Height);
            System.Drawing.Graphics NewGraphics = System.Drawing.Graphics.FromImage(NewBitmap);
            NewGraphics.DrawImage(TempBitmap, new System.Drawing.Rectangle(0, 0, TempBitmap.Width, TempBitmap.Height), 0, 0, TempBitmap.Width, TempBitmap.Height, System.Drawing.GraphicsUnit.Pixel);
            float[][] FloatColorMatrix ={
                    new float[] {1, 0, 0, 0, 0},
                    new float[] {0, 1, 0, 0, 0},
                    new float[] {0, 0, 1, 0, 0},
                    new float[] {0, 0, 0, Opacity, 0},
                    new float[] {0, 0, 0, 0, 1}
                };

            System.Drawing.Imaging.ColorMatrix NewColorMatrix = new System.Drawing.Imaging.ColorMatrix(FloatColorMatrix);
            System.Drawing.Imaging.ImageAttributes Attributes = new System.Drawing.Imaging.ImageAttributes();
            Attributes.SetColorMatrix(NewColorMatrix);
            if (KeyColor != null)
            {
                Attributes.SetColorKey(KeyColor, KeyColor);
            }
            NewGraphics.DrawImage(WatermarkImage, new System.Drawing.Rectangle(X, Y, X + TempBitmap.Width, Y + TempBitmap.Height), 0, 0, TempBitmap.Width, TempBitmap.Height, System.Drawing.GraphicsUnit.Pixel, Attributes);
            Attributes.Dispose();
            NewGraphics.Dispose();
            return NewBitmap;
        }

        /// <summary>
        /// Flips an image
        /// </summary>
        /// <param name="FileName">Image to flip</param>
        /// <param name="NewFileName">Location to save the image to</param>
        /// <param name="FlipX">Flips an image along the X axis</param>
        /// <param name="FlipY">Flips an image along the Y axis</param>
        public static void Flip(string FileName, string NewFileName, bool FlipX, bool FlipY)
        {
            if (!IsGraphic(FileName))
                return;
            ImageFormat FormatUsing = GetFormat(NewFileName);
            System.Drawing.Bitmap NewBitmap = Image.Flip(FileName, FlipX, FlipY);
            NewBitmap.Save(NewFileName, FormatUsing);
            NewBitmap.Dispose();
        }

        /// <summary>
        /// Flips an image
        /// </summary>
        /// <param name="FileName">Image to flip</param>
        /// <param name="FlipX">Flips an image along the X axis</param>
        /// <param name="FlipY">Flips an image along the Y axis</param>
        /// <returns>A bitmap which is flipped</returns>
        public static Bitmap Flip(string FileName, bool FlipX, bool FlipY)
        {
            if (!IsGraphic(FileName))
                return new Bitmap(1, 1);
            System.Drawing.Image TempImage = System.Drawing.Image.FromFile(FileName);
            System.Drawing.Bitmap TempBitmap = new System.Drawing.Bitmap(TempImage, TempImage.Width, TempImage.Height);
            System.Drawing.Bitmap ReturnBitmap = Image.Flip(TempBitmap, FlipX, FlipY);
            TempBitmap.Dispose();
            TempImage.Dispose();
            return ReturnBitmap;
        }

        /// <summary>
        /// Flips an image
        /// </summary>
        /// <param name="Image">Image to flip</param>
        /// <param name="FlipX">Flips an image along the X axis</param>
        /// <param name="FlipY">Flips an image along the Y axis</param>
        /// <returns>A bitmap which is flipped</returns>
        public static Bitmap Flip(Bitmap Image, bool FlipX, bool FlipY)
        {
            System.Drawing.Bitmap TempBitmap = Image;
            System.Drawing.Bitmap NewBitmap = new System.Drawing.Bitmap(TempBitmap.Width, TempBitmap.Height);
            System.Drawing.Graphics NewGraphics = System.Drawing.Graphics.FromImage(NewBitmap);
            NewGraphics.DrawImage(TempBitmap, new System.Drawing.Rectangle(0, 0, TempBitmap.Width, TempBitmap.Height), new System.Drawing.Rectangle(0, 0, TempBitmap.Width, TempBitmap.Height), System.Drawing.GraphicsUnit.Pixel);
            NewGraphics.Dispose();
            if (FlipX && !FlipY)
            {
                NewBitmap.RotateFlip(RotateFlipType.RotateNoneFlipX);
            }
            else if (!FlipX && FlipY)
            {
                NewBitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);
            }
            else if (FlipX && FlipY)
            {
                NewBitmap.RotateFlip(RotateFlipType.RotateNoneFlipXY);
            }
            return NewBitmap;
        }

        /// <summary>
        /// Gets a palette listing in HTML string format
        /// </summary>
        /// <param name="FileName">Image to get the palette of</param>
        /// <returns>A list containing HTML color values (ex: #041845)</returns>
        public static List<string> GetHTMLPalette(string FileName)
        {
            if (!IsGraphic(FileName))
                return new List<string>();
            System.Drawing.Image TempImage = System.Drawing.Image.FromFile(FileName);
            System.Drawing.Bitmap TempBitmap = new System.Drawing.Bitmap(TempImage, TempImage.Width, TempImage.Height);
            List<string>Palette= GetHTMLPalette(TempBitmap);
            TempBitmap.Dispose();
            TempImage.Dispose();
            return Palette;
        }

        /// <summary>
        /// Gets a palette listing in HTML string format
        /// </summary>
        /// <param name="Image">Image to get the palette of</param>
        /// <returns>A list containing HTML color values (ex: #041845)</returns>
        public static List<string> GetHTMLPalette(Bitmap Image)
        {
            List<string> ReturnArray = new List<string>();
            if (Image.Palette != null && Image.Palette.Entries.Length > 0)
            {
                for (int x = 0; x < Image.Palette.Entries.Length; ++x)
                {
                    string TempColor = ColorTranslator.ToHtml(Image.Palette.Entries[x]);
                    if (!ReturnArray.Contains(TempColor))
                    {
                        ReturnArray.Add(TempColor);
                    }
                }
                return ReturnArray;
            }
            for (int x = 0; x < Image.Width; ++x)
            {
                for (int y = 0; y < Image.Height; ++y)
                {
                    string TempColor = ColorTranslator.ToHtml(Image.GetPixel(x, y));
                    if (!ReturnArray.Contains(TempColor))
                    {
                        ReturnArray.Add(TempColor);
                    }
                }
            }
            return ReturnArray;
        }

        /// <summary>
        /// Pixelates an image
        /// </summary>
        /// <param name="FileName">Image to pixelate</param>
        /// <param name="NewFileName">Location to save the image to</param>
        /// <param name="PixelSize">Size of the "pixels" in pixels</param>
        public static void Pixelate(string FileName, string NewFileName, int PixelSize)
        {
            if (!IsGraphic(FileName))
                return;
            ImageFormat FormatUsing = GetFormat(NewFileName);
            System.Drawing.Bitmap NewBitmap = Image.Pixelate(FileName, PixelSize);
            NewBitmap.Save(NewFileName, FormatUsing);
            NewBitmap.Dispose();
        }

        /// <summary>
        /// Pixelates an image
        /// </summary>
        /// <param name="FileName">Image to pixelate</param>
        /// <param name="PixelSize">Size of the "pixels" in pixels</param>
        /// <returns>A bitmap which is pixelated</returns>
        public static Bitmap Pixelate(string FileName, int PixelSize)
        {
            if (!IsGraphic(FileName))
                return new Bitmap(1, 1);
            System.Drawing.Image TempImage = System.Drawing.Image.FromFile(FileName);
            System.Drawing.Bitmap TempBitmap = new System.Drawing.Bitmap(TempImage, TempImage.Width, TempImage.Height);
            System.Drawing.Bitmap ReturnBitmap = Image.Pixelate(TempBitmap, PixelSize);
            TempBitmap.Dispose();
            TempImage.Dispose();
            return ReturnBitmap;
        }

        /// <summary>
        /// Pixelates an image
        /// </summary>
        /// <param name="Image">Image to pixelate</param>
        /// <param name="PixelSize">Size of the "pixels" in pixels</param>
        /// <returns>A bitmap which is pixelated</returns>
        public static Bitmap Pixelate(Bitmap Image, int PixelSize)
        {
            System.Drawing.Bitmap TempBitmap = Image;
            System.Drawing.Bitmap NewBitmap = new System.Drawing.Bitmap(TempBitmap.Width, TempBitmap.Height);
            System.Drawing.Graphics NewGraphics = System.Drawing.Graphics.FromImage(NewBitmap);
            NewGraphics.DrawImage(TempBitmap, new System.Drawing.Rectangle(0, 0, TempBitmap.Width, TempBitmap.Height), new System.Drawing.Rectangle(0, 0, TempBitmap.Width, TempBitmap.Height), System.Drawing.GraphicsUnit.Pixel);
            NewGraphics.Dispose();
            int TempX = 0;
            int TempY = 0;
            while (true)
            {
                int RValue = 0;
                int GValue = 0;
                int BValue = 0;
                for (int x = TempX; x < TempX + PixelSize; ++x)
                {
                    if (x >= NewBitmap.Width)
                        break;
                    for (int y = TempY; y < TempY + PixelSize; ++y)
                    {
                        if (y >= NewBitmap.Height)
                            break;
                        Color TempPixel = NewBitmap.GetPixel(x, y);
                        RValue += TempPixel.R;
                        GValue += TempPixel.G;
                        BValue += TempPixel.B;
                    }
                }
                RValue = RValue / (PixelSize * PixelSize);
                GValue = GValue / (PixelSize * PixelSize);
                BValue = BValue / (PixelSize * PixelSize);
                for (int x = TempX; x < TempX + PixelSize; ++x)
                {
                    if (x >= NewBitmap.Width)
                        break;
                    for (int y = TempY; y < TempY + PixelSize; ++y)
                    {
                        if (y >= NewBitmap.Height)
                            break;
                        NewBitmap.SetPixel(x, y, Color.FromArgb(RValue, GValue, BValue));
                    }
                }
                TempX += PixelSize;
                if (TempX + PixelSize > NewBitmap.Width)
                {
                    TempX = 0;
                    TempY += PixelSize;
                }
                if (TempY >= NewBitmap.Height)
                {
                    break;
                }
            }
            return NewBitmap;
        }

        /// <summary>
        /// Does basic edge detection on an image
        /// </summary>
        /// <param name="FileName">Image to do edge detection on</param>
        /// <param name="NewFileName">Location to save the image to</param>
        /// <param name="Threshold">Decides what is considered an edge</param>
        /// <param name="EdgeColor">Color of the edge</param>
        public static void EdgeDetection(string FileName, string NewFileName, float Threshold,Color EdgeColor)
        {
            if (!IsGraphic(FileName))
                return;
            ImageFormat FormatUsing = GetFormat(NewFileName);
            System.Drawing.Bitmap NewBitmap = Image.EdgeDetection(FileName, Threshold, EdgeColor);
            NewBitmap.Save(NewFileName, FormatUsing);
            NewBitmap.Dispose();
        }

        /// <summary>
        /// Does basic edge detection on an image
        /// </summary>
        /// <param name="FileName">Image to do edge detection on</param>
        /// <param name="Threshold">Decides what is considered an edge</param>
        /// <param name="EdgeColor">Color of the edge</param>
        /// <returns>A bitmap which has the edges drawn on it</returns>
        public static Bitmap EdgeDetection(string FileName, float Threshold, Color EdgeColor)
        {
            if (!IsGraphic(FileName))
                return new Bitmap(1, 1);
            System.Drawing.Image TempImage = System.Drawing.Image.FromFile(FileName);
            System.Drawing.Bitmap TempBitmap = new System.Drawing.Bitmap(TempImage, TempImage.Width, TempImage.Height);
            System.Drawing.Bitmap ReturnBitmap = Image.EdgeDetection(TempBitmap, Threshold, EdgeColor);
            TempBitmap.Dispose();
            TempImage.Dispose();
            return ReturnBitmap;
        }

        /// <summary>
        /// Does basic edge detection on an image
        /// </summary>
        /// <param name="Image">Image to do edge detection on</param>
        /// <param name="Threshold">Decides what is considered an edge</param>
        /// <param name="EdgeColor">Color of the edge</param>
        /// <returns>A bitmap which has the edges drawn on it</returns>
        public static Bitmap EdgeDetection(Bitmap Image, float Threshold, Color EdgeColor)
        {
            System.Drawing.Bitmap TempBitmap = Image;
            System.Drawing.Bitmap NewBitmap = new System.Drawing.Bitmap(TempBitmap.Width, TempBitmap.Height);
            System.Drawing.Graphics NewGraphics = System.Drawing.Graphics.FromImage(NewBitmap);
            NewGraphics.DrawImage(TempBitmap, new System.Drawing.Rectangle(0, 0, TempBitmap.Width, TempBitmap.Height), new System.Drawing.Rectangle(0, 0, TempBitmap.Width, TempBitmap.Height), System.Drawing.GraphicsUnit.Pixel);
            NewGraphics.Dispose();
            for (int x = 0; x < NewBitmap.Width; ++x)
            {
                for (int y = 0; y < NewBitmap.Height; ++y)
                {
                    bool EdgeSet = false;
                    Color CurrentColor = NewBitmap.GetPixel(x, y);
                    if (y < NewBitmap.Height - 1 && x < NewBitmap.Width - 1)
                    {
                        Color TempColor = NewBitmap.GetPixel(x + 1, y + 1);
                        if (Math.Sqrt(((CurrentColor.R - TempColor.R) * (CurrentColor.R - TempColor.R)) +
                            ((CurrentColor.G - TempColor.G) * (CurrentColor.G - TempColor.G)) +
                            ((CurrentColor.B - TempColor.B) * (CurrentColor.B - TempColor.B))) > Threshold)
                        {
                            NewBitmap.SetPixel(x, y, EdgeColor);
                        }
                        EdgeSet = true;
                    }
                    if (y < NewBitmap.Height - 1&&!EdgeSet)
                    {
                        Color TempColor = NewBitmap.GetPixel(x, y + 1);
                        if (Math.Sqrt(((CurrentColor.R - TempColor.R) * (CurrentColor.R - TempColor.R)) +
                            ((CurrentColor.G - TempColor.G) * (CurrentColor.G - TempColor.G)) +
                            ((CurrentColor.B - TempColor.B) * (CurrentColor.B - TempColor.B))) > Threshold)
                        {
                            NewBitmap.SetPixel(x, y, EdgeColor);
                        }
                        EdgeSet = true;
                    }
                    if (x < NewBitmap.Width - 1&&!EdgeSet)
                    {
                        Color TempColor = NewBitmap.GetPixel(x + 1, y);
                        if (Math.Sqrt(((CurrentColor.R - TempColor.R) * (CurrentColor.R - TempColor.R)) +
                            ((CurrentColor.G - TempColor.G) * (CurrentColor.G - TempColor.G)) +
                            ((CurrentColor.B - TempColor.B) * (CurrentColor.B - TempColor.B))) > Threshold)
                        {
                            NewBitmap.SetPixel(x, y, EdgeColor);
                        }
                        EdgeSet = true;
                    }
                }
            }
            return NewBitmap;
        }

        /// <summary>
        /// Does a "wave" effect on the image
        /// </summary>
        /// <param name="FileName">Image to manipulate</param>
        /// <param name="NewFileName">Location to save the image to</param>
        /// <param name="Amplitude">Amplitude of the sine wave</param>
        /// <param name="Frequency">Frequency of the sine wave</param>
        /// <param name="XDirection">Determines if this should be done in the X direction</param>
        /// <param name="YDirection">Determines if this should be done in the Y direction</param>
        public static void SinWave(string FileName, string NewFileName, float Amplitude, float Frequency, bool XDirection, bool YDirection)
        {
            if (!IsGraphic(FileName))
                return;
            ImageFormat FormatUsing = GetFormat(NewFileName);
            System.Drawing.Bitmap NewBitmap = Image.SinWave(FileName, Amplitude, Frequency,XDirection,YDirection);
            NewBitmap.Save(NewFileName, FormatUsing);
            NewBitmap.Dispose();
        }

        /// <summary>
        /// Does a "wave" effect on the image
        /// </summary>
        /// <param name="FileName">Image to manipulate</param>
        /// <param name="Amplitude">Amplitude of the sine wave</param>
        /// <param name="Frequency">Frequency of the sine wave</param>
        /// <param name="XDirection">Determines if this should be done in the X direction</param>
        /// <param name="YDirection">Determines if this should be done in the Y direction</param>
        /// <returns>A bitmap which has been modified</returns>
        public static Bitmap SinWave(string FileName, float Amplitude, float Frequency, bool XDirection, bool YDirection)
        {
            if (!IsGraphic(FileName))
                return new Bitmap(1, 1);
            System.Drawing.Image TempImage = System.Drawing.Image.FromFile(FileName);
            System.Drawing.Bitmap TempBitmap = new System.Drawing.Bitmap(TempImage, TempImage.Width, TempImage.Height);
            System.Drawing.Bitmap ReturnBitmap = Image.SinWave(TempBitmap, Amplitude, Frequency, XDirection, YDirection);
            TempBitmap.Dispose();
            TempImage.Dispose();
            return ReturnBitmap;
        }

        /// <summary>
        /// Does a "wave" effect on the image
        /// </summary>
        /// <param name="Image">Image to manipulate</param>
        /// <param name="Amplitude">Amplitude of the sine wave</param>
        /// <param name="Frequency">Frequency of the sine wave</param>
        /// <param name="XDirection">Determines if this should be done in the X direction</param>
        /// <param name="YDirection">Determines if this should be done in the Y direction</param>
        /// <returns>A bitmap which has been modified</returns>
        public static Bitmap SinWave(Bitmap Image, float Amplitude, float Frequency, bool XDirection, bool YDirection)
        {
            System.Drawing.Bitmap TempBitmap = Image;
            System.Drawing.Bitmap NewBitmap = new System.Drawing.Bitmap(TempBitmap.Width, TempBitmap.Height);
            System.Drawing.Graphics NewGraphics = System.Drawing.Graphics.FromImage(NewBitmap);
            NewGraphics.DrawImage(TempBitmap, new System.Drawing.Rectangle(0, 0, TempBitmap.Width, TempBitmap.Height), new System.Drawing.Rectangle(0, 0, TempBitmap.Width, TempBitmap.Height), System.Drawing.GraphicsUnit.Pixel);
            NewGraphics.Dispose();
            for (int x = 0; x < NewBitmap.Width; ++x)
            {
                for (int y = 0; y < NewBitmap.Height; ++y)
                {
                    double Value1=0;
                    double Value2=0;
                    if (YDirection)
                        Value1 = Math.Sin(((x * Frequency) * Math.PI) / 180.0d) * Amplitude;
                    if (XDirection)
                        Value2 = Math.Sin(((y * Frequency) * Math.PI) / 180.0d) * Amplitude;
                    Value1 = y - (int)Value1;
                    Value2 = x - (int)Value2;
                    while (Value1 < 0)
                        Value1 += NewBitmap.Height;
                    while (Value2 < 0)
                        Value2 += NewBitmap.Width;
                    while (Value1 >= NewBitmap.Height)
                        Value1 -= NewBitmap.Height;
                    while (Value2 >=NewBitmap.Width)
                        Value2 -= NewBitmap.Width;
                    NewBitmap.SetPixel(x, y, Image.GetPixel((int)Value2, (int)Value1));
                }
            }
            return NewBitmap;
        }

        /// <summary>
        /// Causes a "Jitter" effect
        /// </summary>
        /// <param name="FileName">Image to manipulate</param>
        /// <param name="NewFileName">Location to save the image to</param>
        /// <param name="MaxJitter">Maximum number of pixels the item can move</param>
        public static void Jitter(string FileName, string NewFileName, int MaxJitter)
        {
            if (!IsGraphic(FileName))
                return;
            ImageFormat FormatUsing = GetFormat(NewFileName);
            System.Drawing.Bitmap NewBitmap = Image.Jitter(FileName,MaxJitter);
            NewBitmap.Save(NewFileName, FormatUsing);
            NewBitmap.Dispose();
        }

        /// <summary>
        /// Causes a "Jitter" effect
        /// </summary>
        /// <param name="FileName">Image to manipulate</param>
        /// <param name="MaxJitter">Maximum number of pixels the item can move</param>
        public static Bitmap Jitter(string FileName, int MaxJitter)
        {
            if (!IsGraphic(FileName))
                return new Bitmap(1, 1);
            System.Drawing.Image TempImage = System.Drawing.Image.FromFile(FileName);
            System.Drawing.Bitmap TempBitmap = new System.Drawing.Bitmap(TempImage, TempImage.Width, TempImage.Height);
            System.Drawing.Bitmap ReturnBitmap = Image.Jitter(TempBitmap, MaxJitter);
            TempBitmap.Dispose();
            TempImage.Dispose();
            return ReturnBitmap;
        }

        /// <summary>
        /// Causes a "Jitter" effect
        /// </summary>
        /// <param name="Image">Image to manipulate</param>
        /// <param name="MaxJitter">Maximum number of pixels the item can move</param>
        public static Bitmap Jitter(Bitmap Image, int MaxJitter)
        {
            System.Drawing.Bitmap TempBitmap = Image;
            System.Drawing.Bitmap NewBitmap = new System.Drawing.Bitmap(TempBitmap.Width, TempBitmap.Height);
            System.Drawing.Graphics NewGraphics = System.Drawing.Graphics.FromImage(NewBitmap);
            NewGraphics.DrawImage(TempBitmap, new System.Drawing.Rectangle(0, 0, TempBitmap.Width, TempBitmap.Height), new System.Drawing.Rectangle(0, 0, TempBitmap.Width, TempBitmap.Height), System.Drawing.GraphicsUnit.Pixel);
            NewGraphics.Dispose();
            Random TempRandom = new Random();
            for (int x = 0; x < NewBitmap.Width; ++x)
            {
                for (int y = 0; y < NewBitmap.Height; ++y)
                {
                    int NewX=TempRandom.Next(-MaxJitter, MaxJitter);
                    int NewY = TempRandom.Next(-MaxJitter, MaxJitter);
                    NewX += x;
                    NewY += y;
                    
                    if (NewX >= NewBitmap.Width)
                        NewX = NewBitmap.Width - 1;
                    else if (NewX < 0)
                        NewX = 0;

                    if (NewY >= NewBitmap.Height)
                        NewY = NewBitmap.Height - 1;
                    else if (NewY < 0)
                        NewY = 0;

                    NewBitmap.SetPixel(NewX, NewY, Image.GetPixel(x,y));
                }
            }
            return NewBitmap;
        }

        /// <summary>
        /// Does smoothing using a median filter
        /// </summary>
        /// <param name="FileName">Image to manipulate</param>
        /// <param name="NewFileName">Location to save the image to</param>
        /// <param name="Size">Size of the aperture</param>
        public static void MedianFilter(string FileName, string NewFileName, int Size)
        {
            if (!IsGraphic(FileName))
                return;
            ImageFormat FormatUsing = GetFormat(NewFileName);
            System.Drawing.Bitmap NewBitmap = Image.MedianFilter(FileName, Size);
            NewBitmap.Save(NewFileName, FormatUsing);
            NewBitmap.Dispose();
        }

        /// <summary>
        /// Does smoothing using a median filter
        /// </summary>
        /// <param name="FileName">Image to manipulate</param>
        /// <param name="Size">Size of the aperture</param>
        public static Bitmap MedianFilter(string FileName, int Size)
        {
            if (!IsGraphic(FileName))
                return new Bitmap(1, 1);
            System.Drawing.Image TempImage = System.Drawing.Image.FromFile(FileName);
            System.Drawing.Bitmap TempBitmap = new System.Drawing.Bitmap(TempImage, TempImage.Width, TempImage.Height);
            System.Drawing.Bitmap ReturnBitmap = Image.MedianFilter(TempBitmap, Size);
            TempBitmap.Dispose();
            TempImage.Dispose();
            return ReturnBitmap;
        }

        /// <summary>
        /// Does smoothing using a median filter
        /// </summary>
        /// <param name="Image">Image to manipulate</param>
        /// <param name="Size">Size of the aperture</param>
        public static Bitmap MedianFilter(Bitmap Image, int Size)
        {
            System.Drawing.Bitmap TempBitmap = Image;
            System.Drawing.Bitmap NewBitmap = new System.Drawing.Bitmap(TempBitmap.Width, TempBitmap.Height);
            System.Drawing.Graphics NewGraphics = System.Drawing.Graphics.FromImage(NewBitmap);
            NewGraphics.DrawImage(TempBitmap, new System.Drawing.Rectangle(0, 0, TempBitmap.Width, TempBitmap.Height), new System.Drawing.Rectangle(0, 0, TempBitmap.Width, TempBitmap.Height), System.Drawing.GraphicsUnit.Pixel);
            NewGraphics.Dispose();
            Random TempRandom = new Random();
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
                                    Color TempColor = TempBitmap.GetPixel(TempX, TempY);
                                    RValues.Add(TempColor.R);
                                    GValues.Add(TempColor.G);
                                    BValues.Add(TempColor.B);
                                }
                            }
                        }
                    }
                    RValues.Sort();
                    GValues.Sort();
                    BValues.Sort();
                    Color MedianPixel = Color.FromArgb(RValues[RValues.Count / 2],
                        GValues[GValues.Count / 2], 
                        BValues[BValues.Count / 2]);
                    NewBitmap.SetPixel(x, y, MedianPixel);
                }
            }
            return NewBitmap;
        }

        /// <summary>
        /// Uses an RGB histogram to equalize the image
        /// </summary>
        /// <param name="FileName">Image to manipulate</param>
        /// <param name="NewFileName">Location to save the image to</param>
        public static void Equalize(string FileName, string NewFileName)
        {
            if (!IsGraphic(FileName))
                return;
            ImageFormat FormatUsing = GetFormat(NewFileName);
            System.Drawing.Bitmap NewBitmap = Image.Equalize(FileName);
            NewBitmap.Save(NewFileName, FormatUsing);
            NewBitmap.Dispose();
        }

        /// <summary>
        /// Uses an RGB histogram to equalize the image
        /// </summary>
        /// <param name="FileName">Image to manipulate</param>
        public static Bitmap Equalize(string FileName)
        {
            if (!IsGraphic(FileName))
                return new Bitmap(1, 1);
            System.Drawing.Image TempImage = System.Drawing.Image.FromFile(FileName);
            System.Drawing.Bitmap TempBitmap = new System.Drawing.Bitmap(TempImage, TempImage.Width, TempImage.Height);
            System.Drawing.Bitmap ReturnBitmap = Image.Equalize(TempBitmap);
            TempBitmap.Dispose();
            TempImage.Dispose();
            return ReturnBitmap;
        }

        /// <summary>
        /// Uses an RGB histogram to equalize the image
        /// </summary>
        /// <param name="Image">Image to manipulate</param>
        public static Bitmap Equalize(Bitmap Image)
        {
            System.Drawing.Bitmap TempBitmap = Image;
            System.Drawing.Bitmap NewBitmap = new System.Drawing.Bitmap(TempBitmap.Width, TempBitmap.Height);
            System.Drawing.Graphics NewGraphics = System.Drawing.Graphics.FromImage(NewBitmap);
            NewGraphics.DrawImage(TempBitmap, new System.Drawing.Rectangle(0, 0, TempBitmap.Width, TempBitmap.Height), new System.Drawing.Rectangle(0, 0, TempBitmap.Width, TempBitmap.Height), System.Drawing.GraphicsUnit.Pixel);
            NewGraphics.Dispose();
            RGBHistogram TempHistogram = new RGBHistogram(NewBitmap);
            TempHistogram.Equalize();
            for (int x = 0; x < NewBitmap.Width; ++x)
            {
                for (int y = 0; y < NewBitmap.Height; ++y)
                {
                    Color Current=NewBitmap.GetPixel(x,y);
                    int NewR = (int)TempHistogram.R[Current.R];
                    int NewG = (int)TempHistogram.G[Current.G];
                    int NewB = (int)TempHistogram.B[Current.B];
                    if (NewR >= 256)
                        NewR = 255;
                    if (NewG >= 256)
                        NewG = 255;
                    if (NewB >= 256)
                        NewB = 255;
                    NewBitmap.SetPixel(x, y, Color.FromArgb(NewR, NewG, NewB));
                }
            }
            return NewBitmap;
        }

        /// <summary>
        /// Does dilation
        /// </summary>
        /// <param name="FileName">Image to manipulate</param>
        /// <param name="NewFileName">Location to save the image to</param>
        /// <param name="Size">Size of the aperture</param>
        public static void Dilate(string FileName, string NewFileName, int Size)
        {
            if (!IsGraphic(FileName))
                return;
            ImageFormat FormatUsing = GetFormat(NewFileName);
            System.Drawing.Bitmap NewBitmap = Image.Dilate(FileName, Size);
            NewBitmap.Save(NewFileName, FormatUsing);
            NewBitmap.Dispose();
        }

        /// <summary>
        /// Does dilation
        /// </summary>
        /// <param name="FileName">Image to manipulate</param>
        /// <param name="Size">Size of the aperture</param>
        public static Bitmap Dilate(string FileName, int Size)
        {
            if (!IsGraphic(FileName))
                return new Bitmap(1, 1);
            System.Drawing.Image TempImage = System.Drawing.Image.FromFile(FileName);
            System.Drawing.Bitmap TempBitmap = new System.Drawing.Bitmap(TempImage, TempImage.Width, TempImage.Height);
            System.Drawing.Bitmap ReturnBitmap = Image.Dilate(TempBitmap, Size);
            TempBitmap.Dispose();
            TempImage.Dispose();
            return ReturnBitmap;
        }

        /// <summary>
        /// Does dilation
        /// </summary>
        /// <param name="Image">Image to manipulate</param>
        /// <param name="Size">Size of the aperture</param>
        public static Bitmap Dilate(Bitmap Image, int Size)
        {
            System.Drawing.Bitmap TempBitmap = Image;
            System.Drawing.Bitmap NewBitmap = new System.Drawing.Bitmap(TempBitmap.Width, TempBitmap.Height);
            System.Drawing.Graphics NewGraphics = System.Drawing.Graphics.FromImage(NewBitmap);
            NewGraphics.DrawImage(TempBitmap, new System.Drawing.Rectangle(0, 0, TempBitmap.Width, TempBitmap.Height), new System.Drawing.Rectangle(0, 0, TempBitmap.Width, TempBitmap.Height), System.Drawing.GraphicsUnit.Pixel);
            NewGraphics.Dispose();
            Random TempRandom = new Random();
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
                                    Color TempColor = TempBitmap.GetPixel(TempX, TempY);
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
                    NewBitmap.SetPixel(x, y, TempPixel);
                }
            }
            return NewBitmap;
        }

        /// <summary>
        /// Converts an image to sepia tone
        /// </summary>
        /// <param name="FileName">File to change</param>
        /// <param name="NewFileName">Location to save the image to</param>
        public static void ConvertSepiaTone(string FileName, string NewFileName)
        {
            if (!IsGraphic(FileName))
                return;
            ImageFormat FormatUsing = GetFormat(NewFileName);
            System.Drawing.Bitmap NewBitmap = ConvertSepiaTone(FileName);
            NewBitmap.Save(NewFileName, FormatUsing);
            NewBitmap.Dispose();
        }

        /// <summary>
        /// Converts an image to sepia tone
        /// </summary>
        /// <param name="FileName">File to change</param>
        /// <returns>A bitmap object of the sepia tone image</returns>
        public static Bitmap ConvertSepiaTone(string FileName)
        {
            if (!IsGraphic(FileName))
                return new Bitmap(1, 1);
            System.Drawing.Image TempImage = System.Drawing.Image.FromFile(FileName);
            System.Drawing.Bitmap TempBitmap = new System.Drawing.Bitmap(TempImage, TempImage.Width, TempImage.Height);
            System.Drawing.Bitmap ReturnBitmap = Image.ConvertSepiaTone(TempBitmap);
            TempBitmap.Dispose();
            TempImage.Dispose();
            return ReturnBitmap;
        }

        /// <summary>
        /// Converts an image to sepia tone
        /// </summary>
        /// <param name="Image">Image to change</param>
        /// <returns>A bitmap object of the sepia tone image</returns>
        public static Bitmap ConvertSepiaTone(Bitmap Image)
        {
            System.Drawing.Bitmap TempBitmap = Image;

            System.Drawing.Bitmap NewBitmap = new System.Drawing.Bitmap(TempBitmap.Width, TempBitmap.Height);
            System.Drawing.Graphics NewGraphics = System.Drawing.Graphics.FromImage(NewBitmap);
            float[][] FloatColorMatrix ={
                    new float[] {.393f, .349f, .272f, 0, 0},
                    new float[] {.769f, .686f, .534f, 0, 0},
                    new float[] {.189f, .168f, .131f, 0, 0},
                    new float[] {0, 0, 0, 1, 0},
                    new float[] {0, 0, 0, 0, 1}
                };

            System.Drawing.Imaging.ColorMatrix NewColorMatrix = new System.Drawing.Imaging.ColorMatrix(FloatColorMatrix);
            System.Drawing.Imaging.ImageAttributes Attributes = new System.Drawing.Imaging.ImageAttributes();
            Attributes.SetColorMatrix(NewColorMatrix);
            NewGraphics.DrawImage(TempBitmap, new System.Drawing.Rectangle(0, 0, TempBitmap.Width, TempBitmap.Height), 0, 0, TempBitmap.Width, TempBitmap.Height, System.Drawing.GraphicsUnit.Pixel, Attributes);
            Attributes.Dispose();
            NewGraphics.Dispose();
            return NewBitmap;
        }

        /// <summary>
        /// Does smoothing using a box blur
        /// </summary>
        /// <param name="FileName">Image to manipulate</param>
        /// <param name="NewFileName">Location to save the image to</param>
        /// <param name="Size">Size of the aperture</param>
        public static void BoxBlur(string FileName, string NewFileName, int Size)
        {
            if (!IsGraphic(FileName))
                return;
            ImageFormat FormatUsing = GetFormat(NewFileName);
            System.Drawing.Bitmap NewBitmap = Image.BoxBlur(FileName, Size);
            NewBitmap.Save(NewFileName, FormatUsing);
            NewBitmap.Dispose();
        }

        /// <summary>
        /// Does smoothing using a box blur
        /// </summary>
        /// <param name="FileName">Image to manipulate</param>
        /// <param name="Size">Size of the aperture</param>
        public static Bitmap BoxBlur(string FileName, int Size)
        {
            if (!IsGraphic(FileName))
                return new Bitmap(1, 1);
            System.Drawing.Image TempImage = System.Drawing.Image.FromFile(FileName);
            System.Drawing.Bitmap TempBitmap = new System.Drawing.Bitmap(TempImage, TempImage.Width, TempImage.Height);
            System.Drawing.Bitmap ReturnBitmap = Image.BoxBlur(TempBitmap, Size);
            TempBitmap.Dispose();
            TempImage.Dispose();
            return ReturnBitmap;
        }

        /// <summary>
        /// Does smoothing using a box blur
        /// </summary>
        /// <param name="Image">Image to manipulate</param>
        /// <param name="Size">Size of the aperture</param>
        public static Bitmap BoxBlur(Bitmap Image, int Size)
        {
            Filter TempFilter = new Filter(Size, Size);
            for (int x = 0; x < Size; ++x)
            {
                for (int y = 0; y < Size; ++y)
                {
                    TempFilter.MyFilter[x, y] = 1;
                }
            }
            return TempFilter.ApplyFilter(Image);
        }

        /// <summary>
        /// Does smoothing using a gaussian blur
        /// </summary>
        /// <param name="FileName">Image to manipulate</param>
        /// <param name="NewFileName">Location to save the image to</param>
        /// <param name="Size">Size of the aperture</param>
        public static void GaussianBlur(string FileName, string NewFileName, int Size)
        {
            if (!IsGraphic(FileName))
                return;
            ImageFormat FormatUsing = GetFormat(NewFileName);
            System.Drawing.Bitmap NewBitmap = Image.GaussianBlur(FileName, Size);
            NewBitmap.Save(NewFileName, FormatUsing);
            NewBitmap.Dispose();
        }

        /// <summary>
        /// Does smoothing using a gaussian blur
        /// </summary>
        /// <param name="FileName">Image to manipulate</param>
        /// <param name="Size">Size of the aperture</param>
        public static Bitmap GaussianBlur(string FileName, int Size)
        {
            if (!IsGraphic(FileName))
                return new Bitmap(1, 1);
            System.Drawing.Image TempImage = System.Drawing.Image.FromFile(FileName);
            System.Drawing.Bitmap TempBitmap = new System.Drawing.Bitmap(TempImage, TempImage.Width, TempImage.Height);
            System.Drawing.Bitmap ReturnBitmap = Image.GaussianBlur(TempBitmap, Size);
            TempBitmap.Dispose();
            TempImage.Dispose();
            return ReturnBitmap;
        }

        /// <summary>
        /// Does smoothing using a gaussian blur
        /// </summary>
        /// <param name="Image">Image to manipulate</param>
        /// <param name="Size">Size of the aperture</param>
        public static Bitmap GaussianBlur(Bitmap Image, int Size)
        {
            System.Drawing.Bitmap ReturnBitmap = BoxBlur(Image, Size);
            System.Drawing.Bitmap ReturnBitmap2 = BoxBlur(ReturnBitmap, Size);
            ReturnBitmap.Dispose();
            System.Drawing.Bitmap ReturnBitmap3 = BoxBlur(ReturnBitmap2, Size);
            ReturnBitmap2.Dispose();
            return ReturnBitmap3;
        }

        /// <summary>
        /// Does smoothing using a Kuwahara blur
        /// </summary>
        /// <param name="FileName">Image to manipulate</param>
        /// <param name="NewFileName">Location to save the image to</param>
        /// <param name="Size">Size of the aperture</param>
        public static void KuwaharaBlur(string FileName, string NewFileName, int Size)
        {
            if (!IsGraphic(FileName))
                return;
            ImageFormat FormatUsing = GetFormat(NewFileName);
            System.Drawing.Bitmap NewBitmap = Image.KuwaharaBlur(FileName, Size);
            NewBitmap.Save(NewFileName, FormatUsing);
            NewBitmap.Dispose();
        }

        /// <summary>
        /// Does smoothing using a kuwahara blur
        /// </summary>
        /// <param name="FileName">Image to manipulate</param>
        /// <param name="Size">Size of the aperture</param>
        public static Bitmap KuwaharaBlur(string FileName, int Size)
        {
            if (!IsGraphic(FileName))
                return new Bitmap(1, 1);
            System.Drawing.Image TempImage = System.Drawing.Image.FromFile(FileName);
            System.Drawing.Bitmap TempBitmap = new System.Drawing.Bitmap(TempImage, TempImage.Width, TempImage.Height);
            System.Drawing.Bitmap ReturnBitmap = Image.KuwaharaBlur(TempBitmap, Size);
            TempBitmap.Dispose();
            TempImage.Dispose();
            return ReturnBitmap;
        }

        /// <summary>
        /// Does smoothing using a kuwahara blur
        /// </summary>
        /// <param name="Image">Image to manipulate</param>
        /// <param name="Size">Size of the aperture</param>
        public static Bitmap KuwaharaBlur(Bitmap Image, int Size)
        {
            System.Drawing.Bitmap TempBitmap = Image;
            System.Drawing.Bitmap NewBitmap = new System.Drawing.Bitmap(TempBitmap.Width, TempBitmap.Height);
            System.Drawing.Graphics NewGraphics = System.Drawing.Graphics.FromImage(NewBitmap);
            NewGraphics.DrawImage(TempBitmap, new System.Drawing.Rectangle(0, 0, TempBitmap.Width, TempBitmap.Height), new System.Drawing.Rectangle(0, 0, TempBitmap.Width, TempBitmap.Height), System.Drawing.GraphicsUnit.Pixel);
            NewGraphics.Dispose();
            Random TempRandom = new Random();
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
                                        Color TempColor = TempBitmap.GetPixel(TempX, TempY);
                                        RValues[i] += TempColor.R;
                                        GValues[i] += TempColor.G;
                                        BValues[i] += TempColor.B;
                                        if (TempColor.R > MaxRValue[i])
                                        {
                                            MaxRValue[i] = TempColor.R;
                                        }
                                        else if (TempColor.R < MinRValue[i])
                                        {
                                            MinRValue[i] = TempColor.R;
                                        }

                                        if (TempColor.G > MaxGValue[i])
                                        {
                                            MaxGValue[i] = TempColor.G;
                                        }
                                        else if (TempColor.G < MinGValue[i])
                                        {
                                            MinGValue[i] = TempColor.G;
                                        }

                                        if (TempColor.B > MaxBValue[i])
                                        {
                                            MaxBValue[i] = TempColor.B;
                                        }
                                        else if (TempColor.B < MinBValue[i])
                                        {
                                            MinBValue[i] = TempColor.B;
                                        }
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
                    NewBitmap.SetPixel(x, y, MeanPixel);
                }
            }
            return NewBitmap;
        }

        /// <summary>
        /// Does smoothing using a SNN blur
        /// </summary>
        /// <param name="FileName">Image to manipulate</param>
        /// <param name="NewFileName">Location to save the image to</param>
        /// <param name="Size">Size of the aperture</param>
        public static void SNNBlur(string FileName, string NewFileName, int Size)
        {
            if (!IsGraphic(FileName))
                return;
            ImageFormat FormatUsing = GetFormat(NewFileName);
            System.Drawing.Bitmap NewBitmap = Image.SNNBlur(FileName, Size);
            NewBitmap.Save(NewFileName, FormatUsing);
            NewBitmap.Dispose();
        }

        /// <summary>
        /// Does smoothing using a SNN blur
        /// </summary>
        /// <param name="FileName">Image to manipulate</param>
        /// <param name="Size">Size of the aperture</param>
        public static Bitmap SNNBlur(string FileName, int Size)
        {
            if (!IsGraphic(FileName))
                return new Bitmap(1, 1);
            System.Drawing.Image TempImage = System.Drawing.Image.FromFile(FileName);
            System.Drawing.Bitmap TempBitmap = new System.Drawing.Bitmap(TempImage, TempImage.Width, TempImage.Height);
            System.Drawing.Bitmap ReturnBitmap = Image.SNNBlur(TempBitmap, Size);
            TempBitmap.Dispose();
            TempImage.Dispose();
            return ReturnBitmap;
        }

        /// <summary>
        /// Does smoothing using a SNN blur
        /// </summary>
        /// <param name="Image">Image to manipulate</param>
        /// <param name="Size">Size of the aperture</param>
        public static Bitmap SNNBlur(Bitmap Image, int Size)
        {
            System.Drawing.Bitmap TempBitmap = Image;
            System.Drawing.Bitmap NewBitmap = new System.Drawing.Bitmap(TempBitmap.Width, TempBitmap.Height);
            System.Drawing.Graphics NewGraphics = System.Drawing.Graphics.FromImage(NewBitmap);
            NewGraphics.DrawImage(TempBitmap, new System.Drawing.Rectangle(0, 0, TempBitmap.Width, TempBitmap.Height), new System.Drawing.Rectangle(0, 0, TempBitmap.Width, TempBitmap.Height), System.Drawing.GraphicsUnit.Pixel);
            NewGraphics.Dispose();
            Random TempRandom = new Random();
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
                    int NumPixels=0;
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
                                    Color TempColor = TempBitmap.GetPixel(x, y);
                                    Color TempColor2 = TempBitmap.GetPixel(TempX1, TempY1);
                                    Color TempColor3 = TempBitmap.GetPixel(TempX2, TempY2);
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
                    NewBitmap.SetPixel(x, y, MeanPixel);
                }
            }
            return NewBitmap;
        }

        /// <summary>
        /// Sharpens an image
        /// </summary>
        /// <param name="FileName">Image to manipulate</param>
        /// <param name="NewFileName">Location to save the image to</param>
        public static void Sharpen(string FileName, string NewFileName)
        {
            if (!IsGraphic(FileName))
                return;
            ImageFormat FormatUsing = GetFormat(NewFileName);
            System.Drawing.Bitmap NewBitmap = Image.Sharpen(FileName);
            NewBitmap.Save(NewFileName, FormatUsing);
            NewBitmap.Dispose();
        }

        /// <summary>
        /// Sharpens an image
        /// </summary>
        /// <param name="FileName">Image to manipulate</param>
        /// <returns>A bitmap image</returns>
        public static Bitmap Sharpen(string FileName)
        {
            if (!IsGraphic(FileName))
                return new Bitmap(1, 1);
            System.Drawing.Image TempImage = System.Drawing.Image.FromFile(FileName);
            System.Drawing.Bitmap TempBitmap = new System.Drawing.Bitmap(TempImage, TempImage.Width, TempImage.Height);
            System.Drawing.Bitmap ReturnBitmap = Image.Sharpen(TempBitmap);
            TempBitmap.Dispose();
            TempImage.Dispose();
            return ReturnBitmap;
        }

        /// <summary>
        /// Sharpens an image
        /// </summary>
        /// <param name="Image">Image to manipulate</param>
        /// <returns>A bitmap image</returns>
        public static Bitmap Sharpen(Bitmap Image)
        {
            Filter TempFilter = new Filter(3,3);
            TempFilter.MyFilter[0, 0] = -1;
            TempFilter.MyFilter[0, 2] = -1;
            TempFilter.MyFilter[2, 0] = -1;
            TempFilter.MyFilter[2, 2] = -1;
            TempFilter.MyFilter[0, 1] = -2;
            TempFilter.MyFilter[1, 0] = -2;
            TempFilter.MyFilter[2, 1] = -2;
            TempFilter.MyFilter[1, 2] = -2;
            TempFilter.MyFilter[1, 1] = 16;
            return TempFilter.ApplyFilter(Image);
        }

        /// <summary>
        /// Emboss function
        /// </summary>
        /// <param name="FileName">Image to manipulate</param>
        /// <param name="NewFileName">Location to save the image to</param>
        public static void Emboss(string FileName, string NewFileName)
        {
            if (!IsGraphic(FileName))
                return;
            ImageFormat FormatUsing = GetFormat(NewFileName);
            System.Drawing.Bitmap NewBitmap = Image.Emboss(FileName);
            NewBitmap.Save(NewFileName, FormatUsing);
            NewBitmap.Dispose();
        }

        /// <summary>
        /// Emboss function
        /// </summary>
        /// <param name="FileName">Image to manipulate</param>
        /// <returns>A bitmap image</returns>
        public static Bitmap Emboss(string FileName)
        {
            if (!IsGraphic(FileName))
                return new Bitmap(1, 1);
            System.Drawing.Image TempImage = System.Drawing.Image.FromFile(FileName);
            System.Drawing.Bitmap TempBitmap = new System.Drawing.Bitmap(TempImage, TempImage.Width, TempImage.Height);
            System.Drawing.Bitmap ReturnBitmap = Image.Emboss(TempBitmap);
            TempBitmap.Dispose();
            TempImage.Dispose();
            return ReturnBitmap;
        }

        /// <summary>
        /// Emboss function
        /// </summary>
        /// <param name="Image">Image to manipulate</param>
        /// <returns>A bitmap image</returns>
        public static Bitmap Emboss(Bitmap Image)
        {
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
            return TempFilter.ApplyFilter(Image);
        }

        /// <summary>
        /// Sobel emboss function
        /// </summary>
        /// <param name="FileName">Image to manipulate</param>
        /// <param name="NewFileName">Location to save the image to</param>
        public static void SobelEmboss(string FileName, string NewFileName)
        {
            if (!IsGraphic(FileName))
                return;
            ImageFormat FormatUsing = GetFormat(NewFileName);
            System.Drawing.Bitmap NewBitmap = Image.SobelEmboss(FileName);
            NewBitmap.Save(NewFileName, FormatUsing);
            NewBitmap.Dispose();
        }

        /// <summary>
        /// Sobel emboss function
        /// </summary>
        /// <param name="FileName">Image to manipulate</param>
        /// <returns>A bitmap image</returns>
        public static Bitmap SobelEmboss(string FileName)
        {
            if (!IsGraphic(FileName))
                return new Bitmap(1, 1);
            System.Drawing.Image TempImage = System.Drawing.Image.FromFile(FileName);
            System.Drawing.Bitmap TempBitmap = new System.Drawing.Bitmap(TempImage, TempImage.Width, TempImage.Height);
            System.Drawing.Bitmap ReturnBitmap = Image.SobelEmboss(TempBitmap);
            TempBitmap.Dispose();
            TempImage.Dispose();
            return ReturnBitmap;
        }

        /// <summary>
        /// Sobel emboss function
        /// </summary>
        /// <param name="Image">Image to manipulate</param>
        /// <returns>A bitmap image</returns>
        public static Bitmap SobelEmboss(Bitmap Image)
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
            TempFilter.Offset = 127;
            return TempFilter.ApplyFilter(Image);
        }
        #endregion

        #region Private Functions
        private static double Distance(int R1,int R2,int G1,int G2,int B1,int B2)
        {
            return Math.Sqrt(((R1-R2)*(R1-R2))+((G1-G2)*(G1-G2))+((B1-B2)*(B1-B2)));
        }
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

        #endregion
    }
}
