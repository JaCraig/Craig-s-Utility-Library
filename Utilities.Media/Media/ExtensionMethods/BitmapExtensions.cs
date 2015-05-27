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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.DataTypes;
using Utilities.Media.Procedural;

namespace Utilities.Media
{
    /// <summary>
    /// Enum defining alignment
    /// </summary>
    public enum Align
    {
        /// <summary>
        /// Top
        /// </summary>
        Top,

        /// <summary>
        /// Bottom
        /// </summary>
        Bottom,

        /// <summary>
        /// Left
        /// </summary>
        Left,

        /// <summary>
        /// Right
        /// </summary>
        Right
    }

    /// <summary>
    /// Direction
    /// </summary>
    public enum Direction
    {
        /// <summary>
        /// Top to bottom
        /// </summary>
        TopBottom = 0,

        /// <summary>
        /// Left to right
        /// </summary>
        LeftRight
    };

    /// <summary>
    /// Enum defining quality
    /// </summary>
    public enum Quality
    {
        /// <summary>
        /// High
        /// </summary>
        High,

        /// <summary>
        /// Low
        /// </summary>
        Low
    }

    /// <summary>
    /// SwiftBitmap extensions
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class SwiftBitmapExtensions
    {
        /// <summary>
        /// Characters used for ASCII art
        /// </summary>
        private static string[] _ASCIICharacters = { "#", "#", "@", "%", "=", "+", "*", ":", "-", ".", " " };

        /// <summary>
        /// adds noise to the image
        /// </summary>
        /// <param name="Image">The image to add noise to</param>
        /// <param name="Amount">Amount of noise to add (defaults to 10)</param>
        /// <returns>
        /// New SwiftBitmap object with the noise added
        /// </returns>
        public static SwiftBitmap AddNoise(this SwiftBitmap Image, int Amount = 10)
        {
            Contract.Requires<ArgumentNullException>(Image != null, "Image");
            Image.Lock();
            Parallel.For(0, Image.Width, x =>
            {
                for (int y = 0; y < Image.Height; ++y)
                {
                    Color CurrentPixel = Image.GetPixel(x, y);
                    int R = CurrentPixel.R + Random.Random.ThreadSafeNext(-Amount, Amount + 1);
                    int G = CurrentPixel.G + Random.Random.ThreadSafeNext(-Amount, Amount + 1);
                    int B = CurrentPixel.B + Random.Random.ThreadSafeNext(-Amount, Amount + 1);
                    Color TempValue = Color.FromArgb(R.Clamp(255, 0), G.Clamp(255, 0), B.Clamp(255, 0));
                    Image.SetPixel(x, y, TempValue);
                }
            });
            return Image.Unlock();
        }

        /// <summary>
        /// Adjusts the brightness
        /// </summary>
        /// <param name="Image">Image to change</param>
        /// <param name="Value">The value.</param>
        /// <returns>
        /// Modified Image object
        /// </returns>
        public static SwiftBitmap AdjustBrightness(this SwiftBitmap Image, int Value = 0)
        {
            Contract.Requires<ArgumentNullException>(Image != null, "Image");
            float FinalValue = (float)Value / 255.0f;
            return Image.ApplyColorMatrix(new ColorMatrix(new float[][]{
                            new float[] {1, 0, 0, 0, 0},
                            new float[] {0, 1, 0, 0, 0},
                            new float[] {0, 0, 1, 0, 0},
                            new float[] {0, 0, 0, 1, 0},
                            new float[] {FinalValue, FinalValue, FinalValue, 1, 1}
                        }));
        }

        /// <summary>
        /// Adjusts the Contrast
        /// </summary>
        /// <param name="OriginalImage">Image to change</param>
        /// <param name="Value">Used to set the contrast (-100 to 100)</param>
        /// <returns>
        /// A SwiftBitmap object
        /// </returns>
        public static SwiftBitmap AdjustContrast(this SwiftBitmap OriginalImage, float Value = 0)
        {
            Contract.Requires<ArgumentNullException>(OriginalImage != null, "OriginalImage");
            OriginalImage.Lock();
            Value = (100.0f + Value) / 100.0f;
            Value *= Value;
            Parallel.For(0, OriginalImage.Width, x =>
            {
                for (int y = 0; y < OriginalImage.Height; ++y)
                {
                    Color Pixel = OriginalImage.GetPixel(x, y);
                    float Red = Pixel.R / 255.0f;
                    float Green = Pixel.G / 255.0f;
                    float Blue = Pixel.B / 255.0f;
                    Red = (((Red - 0.5f) * Value) + 0.5f) * 255.0f;
                    Green = (((Green - 0.5f) * Value) + 0.5f) * 255.0f;
                    Blue = (((Blue - 0.5f) * Value) + 0.5f) * 255.0f;
                    OriginalImage.SetPixel(x, y,
                        Color.FromArgb(((int)Red).Clamp(255, 0),
                        ((int)Green).Clamp(255, 0),
                        ((int)Blue).Clamp(255, 0)));
                }
            });
            return OriginalImage.Unlock();
        }

        /// <summary>
        /// Adjusts the Gamma
        /// </summary>
        /// <param name="OriginalImage">Image to change</param>
        /// <param name="Value">Used to build the gamma ramp (usually .2 to 5)</param>
        /// <returns>
        /// A SwiftBitmap object
        /// </returns>
        public static SwiftBitmap AdjustGamma(this SwiftBitmap OriginalImage, float Value = 1.0f)
        {
            Contract.Requires<ArgumentNullException>(OriginalImage != null, "OriginalImage");
            OriginalImage.Lock();
            int[] Ramp = new int[256];
            Parallel.For(0, 256, x =>
            {
                Ramp[x] = ((int)((255.0 * System.Math.Pow(x / 255.0, 1.0 / Value)) + 0.5)).Clamp(255, 0);
            });
            Parallel.For(0, OriginalImage.Width, x =>
            {
                for (int y = 0; y < OriginalImage.Height; ++y)
                {
                    Color Pixel = OriginalImage.GetPixel(x, y);
                    int Red = Ramp[Pixel.R];
                    int Green = Ramp[Pixel.G];
                    int Blue = Ramp[Pixel.B];
                    OriginalImage.SetPixel(x, y, Color.FromArgb(Red, Green, Blue));
                }
            });
            return OriginalImage.Unlock();
        }

        /// <summary>
        /// Converts an SwiftBitmap to black and white
        /// </summary>
        /// <param name="Image">Image to change</param>
        /// <returns>
        /// A SwiftBitmap object of the black and white image
        /// </returns>
        public static SwiftBitmap BlackAndWhite(this SwiftBitmap Image)
        {
            Contract.Requires<ArgumentNullException>(Image != null, "Image");
            return Image.ApplyColorMatrix(new ColorMatrix(new float[][]{
                            new float[] {.3f, .3f, .3f, 0, 0},
                            new float[] {.59f, .59f, .59f, 0, 0},
                            new float[] {.11f, .11f, .11f, 0, 0},
                            new float[] {0, 0, 0, 1, 0},
                            new float[] {0, 0, 0, 0, 1}
                        }));
        }

        /// <summary>
        /// Gets the blue filter for an image
        /// </summary>
        /// <param name="Image">Image to change</param>
        /// <returns>
        /// A SwiftBitmap object
        /// </returns>
        public static SwiftBitmap BlueFilter(this SwiftBitmap Image)
        {
            Contract.Requires<ArgumentNullException>(Image != null, "Image");
            return Image.ApplyColorMatrix(new ColorMatrix(new float[][]{
                            new float[] {0, 0, 0, 0, 0},
                            new float[] {0, 0, 0, 0, 0},
                            new float[] {0, 0, 1, 0, 0},
                            new float[] {0, 0, 0, 1, 0},
                            new float[] {0, 0, 0, 0, 1}
                        }));
        }

        /// <summary>
        /// Does smoothing using a box blur
        /// </summary>
        /// <param name="Image">Image to manipulate</param>
        /// <param name="Size">Size of the aperture</param>
        /// <returns>
        /// A SwiftBitmap object
        /// </returns>
        public static SwiftBitmap BoxBlur(this SwiftBitmap Image, int Size = 3)
        {
            Contract.Requires<ArgumentNullException>(Image != null, "Image");
            int[][] TempFilter = new int[Size][];
            for (int x = 0; x < Size; ++x)
            {
                TempFilter[x] = new int[Size];
                for (int y = 0; y < Size; ++y)
                {
                    TempFilter[x][y] = 1;
                }
            }
            return Image.ApplyConvolutionFilter(TempFilter);
        }

        /// <summary>
        /// Creates the bump map
        /// </summary>
        /// <param name="Direction">Direction of the bump map</param>
        /// <param name="Image">Image to create a bump map from</param>
        /// <param name="Invert">Inverts the direction of the bump map</param>
        /// <returns>The resulting bump map</returns>
        public static SwiftBitmap BumpMap(this SwiftBitmap Image, Direction Direction = Direction.TopBottom, bool Invert = false)
        {
            Contract.Requires<ArgumentNullException>(Image != null, "Image");
            int[][] EdgeDetectionFilter = null;
            if (Direction == Direction.TopBottom)
            {
                if (!Invert)
                {
                    EdgeDetectionFilter = new int[][]{
                            new int[] {1, 2, 1},
                            new int[] {0, 0, 0},
                            new int[] {-1, -2, -1}
                        };
                }
                else
                {
                    EdgeDetectionFilter = new int[][]{
                            new int[] {-1, -2, -1},
                            new int[] {0, 0, 0},
                            new int[] {1, 2, 1}
                        };
                }
            }
            else
            {
                if (!Invert)
                {
                    EdgeDetectionFilter = new int[][]{
                            new int[] {-1, 0, 1},
                            new int[] {-2, 0, 2},
                            new int[] {-1, 0, 1}
                        };
                }
                else
                {
                    EdgeDetectionFilter = new int[][]{
                            new int[] {1, 0, -1},
                            new int[] {2, 0, -2},
                            new int[] {1, 0, -1}
                        };
                }
            }
            return Image.ApplyConvolutionFilter(EdgeDetectionFilter, false, 127).BlackAndWhite();
        }

        /// <summary>
        /// Colorizes a black and white image
        /// </summary>
        /// <param name="OriginalImage">Black and white image</param>
        /// <param name="Colors">Color array to use for the image</param>
        /// <returns>
        /// The colorized image
        /// </returns>
        public static SwiftBitmap Colorize(this SwiftBitmap OriginalImage, Color[] Colors)
        {
            Contract.Requires<ArgumentNullException>(OriginalImage != null, "OriginalImage");
            Contract.Requires<ArgumentNullException>(Colors != null, "Colors");
            if (Colors.Length < 256)
                return new SwiftBitmap(1, 1);
            OriginalImage.Lock();
            Parallel.For(0, OriginalImage.Width, x =>
            {
                for (int y = 0; y < OriginalImage.Height; ++y)
                {
                    int ColorUsing = OriginalImage.GetPixel(x, y).R;
                    OriginalImage.SetPixel(x, y, Colors[ColorUsing]);
                }
            });
            return OriginalImage.Unlock();
        }

        /// <summary>
        /// Does dilation
        /// </summary>
        /// <param name="OriginalImage">Image to manipulate</param>
        /// <param name="Size">Size of the aperture</param>
        /// <returns>
        /// A SwiftBitmap object of the resulting image
        /// </returns>
        public static SwiftBitmap Dilate(this SwiftBitmap OriginalImage, int Size)
        {
            Contract.Requires<ArgumentNullException>(OriginalImage != null, "OriginalImage");
            using (SwiftBitmap TempImage = (SwiftBitmap)OriginalImage.Clone())
            {
                OriginalImage.Lock();
                TempImage.Lock();
                int ApetureMin = -(Size / 2);
                int ApetureMax = (Size / 2);
                Parallel.For(0, OriginalImage.Width, x =>
                {
                    for (int y = 0; y < OriginalImage.Height; ++y)
                    {
                        int RValue = 0;
                        int GValue = 0;
                        int BValue = 0;
                        for (int x2 = ApetureMin; x2 < ApetureMax; ++x2)
                        {
                            int TempX = x + x2;
                            if (TempX >= 0 && TempX < OriginalImage.Width)
                            {
                                for (int y2 = ApetureMin; y2 < ApetureMax; ++y2)
                                {
                                    int TempY = y + y2;
                                    if (TempY >= 0 && TempY < OriginalImage.Height)
                                    {
                                        Color TempColor = OriginalImage.GetPixel(TempX, TempY);
                                        RValue = RValue.Max(TempColor.R);
                                        GValue = GValue.Max(TempColor.G);
                                        BValue = BValue.Max(TempColor.B);
                                    }
                                }
                            }
                        }
                        TempImage.SetPixel(x, y, Color.FromArgb(RValue, GValue, BValue));
                    }
                });
                TempImage.Unlock();
                return OriginalImage.Unlock().Copy(TempImage);
            }
        }

        /// <summary>
        /// Draws a rounded rectangle on a SwiftBitmap
        /// </summary>
        /// <param name="Image">Image to draw on</param>
        /// <param name="BoxColor">The color that the box should be</param>
        /// <param name="XPosition">The upper right corner's x position</param>
        /// <param name="YPosition">The upper right corner's y position</param>
        /// <param name="Height">Height of the box</param>
        /// <param name="Width">Width of the box</param>
        /// <param name="CornerRadius">Radius of the corners</param>
        /// <returns>
        /// The SwiftBitmap with the rounded box on it
        /// </returns>
        public static SwiftBitmap DrawRoundedRectangle(this SwiftBitmap Image, Color BoxColor, int XPosition, int YPosition,
            int Height, int Width, int CornerRadius)
        {
            Contract.Requires<ArgumentNullException>(Image != null, "Image");
            Contract.Requires<ArgumentNullException>(BoxColor != null, "BoxColor");
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
                    Image.DrawPath(BoxPen, Path);
                }
            }
            return Image;
        }

        /// <summary>
        /// Does basic edge detection on an image
        /// </summary>
        /// <param name="OriginalImage">Image to do edge detection on</param>
        /// <param name="Threshold">Decides what is considered an edge</param>
        /// <param name="EdgeColor">Color of the edge</param>
        /// <returns>
        /// A SwiftBitmap which has the edges drawn on it
        /// </returns>
        public static SwiftBitmap EdgeDetection(this SwiftBitmap OriginalImage, float Threshold, Color EdgeColor)
        {
            Contract.Requires<ArgumentNullException>(OriginalImage != null, "OriginalImage");
            Contract.Requires<ArgumentNullException>(EdgeColor != null, "EdgeColor");
            using (SwiftBitmap NewImage = (SwiftBitmap)OriginalImage.Clone())
            {
                NewImage.Lock();
                OriginalImage.Lock();
                Parallel.For(0, OriginalImage.Width, x =>
                {
                    for (int y = 0; y < OriginalImage.Height; ++y)
                    {
                        Color CurrentColor = OriginalImage.GetPixel(x, y);
                        if (y < OriginalImage.Height - 1 && x < OriginalImage.Width - 1)
                        {
                            Color TempColor = OriginalImage.GetPixel(x + 1, y + 1);
                            if (Distance(CurrentColor.R, TempColor.R, CurrentColor.G, TempColor.G, CurrentColor.B, TempColor.B) > Threshold)
                                NewImage.SetPixel(x, y, EdgeColor);
                        }
                        else if (y < OriginalImage.Height - 1)
                        {
                            Color TempColor = OriginalImage.GetPixel(x, y + 1);
                            if (Distance(CurrentColor.R, TempColor.R, CurrentColor.G, TempColor.G, CurrentColor.B, TempColor.B) > Threshold)
                                NewImage.SetPixel(x, y, EdgeColor);
                        }
                        else if (x < OriginalImage.Width - 1)
                        {
                            Color TempColor = OriginalImage.GetPixel(x + 1, y);
                            if (Distance(CurrentColor.R, TempColor.R, CurrentColor.G, TempColor.G, CurrentColor.B, TempColor.B) > Threshold)
                                NewImage.SetPixel(x, y, EdgeColor);
                        }
                    }
                });
                NewImage.Unlock();
                return OriginalImage.Unlock().Copy(NewImage);
            }
        }

        /// <summary>
        /// Emboss function
        /// </summary>
        /// <param name="Image">Image to manipulate</param>
        /// <returns>
        /// A SwiftBitmap image
        /// </returns>
        public static SwiftBitmap Emboss(this SwiftBitmap Image)
        {
            Contract.Requires<ArgumentNullException>(Image != null, "Image");
            return Image.ApplyConvolutionFilter(new int[][]{
                            new int[] {-2, -1, 0},
                            new int[] {-1, 1,  1},
                            new int[] {0,  1,  2}
                        });
        }

        /// <summary>
        /// Uses an RGB histogram to equalize the image
        /// </summary>
        /// <param name="OriginalImage">Image to manipulate</param>
        /// <returns>
        /// The resulting SwiftBitmap image
        /// </returns>
        public static SwiftBitmap Equalize(this SwiftBitmap OriginalImage)
        {
            Contract.Requires<ArgumentNullException>(OriginalImage != null, "OriginalImage");
            using (SwiftBitmap NewSwiftBitmap = new SwiftBitmap(OriginalImage.Width, OriginalImage.Height))
            {
                RGBHistogram TempHistogram = new RGBHistogram(OriginalImage);
                TempHistogram.Equalize();
                NewSwiftBitmap.Lock();
                OriginalImage.Lock();
                Parallel.For(0, OriginalImage.Width, x =>
                {
                    for (int y = 0; y < OriginalImage.Height; ++y)
                    {
                        Color Current = OriginalImage.GetPixel(x, y);
                        int NewR = (int)TempHistogram.R[Current.R];
                        int NewG = (int)TempHistogram.G[Current.G];
                        int NewB = (int)TempHistogram.B[Current.B];
                        NewR = NewR.Clamp(255, 0);
                        NewG = NewG.Clamp(255, 0);
                        NewB = NewB.Clamp(255, 0);
                        NewSwiftBitmap.SetPixel(x, y, Color.FromArgb(NewR, NewG, NewB));
                    }
                });
                NewSwiftBitmap.Unlock();
                return OriginalImage.Unlock().Copy(NewSwiftBitmap);
            }
        }

        /// <summary>
        /// Does smoothing using a gaussian blur
        /// </summary>
        /// <param name="Image">Image to manipulate</param>
        /// <param name="Size">Size of the aperture</param>
        /// <returns>
        /// The resulting SwiftBitmap
        /// </returns>
        public static SwiftBitmap GaussianBlur(this SwiftBitmap Image, int Size = 3)
        {
            Contract.Requires<ArgumentNullException>(Image != null, "Image");
            return Image.BoxBlur(Size).BoxBlur(Size).BoxBlur(Size);
        }

        /// <summary>
        /// Gets the Green filter for an image
        /// </summary>
        /// <param name="Image">Image to change</param>
        /// <returns>
        /// A SwiftBitmap object
        /// </returns>
        public static SwiftBitmap GreenFilter(this SwiftBitmap Image)
        {
            Contract.Requires<ArgumentNullException>(Image != null, "Image");
            return Image.ApplyColorMatrix(new ColorMatrix(new float[][]{
                            new float[] {0, 0, 0, 0, 0},
                            new float[] {0, 1, 0, 0, 0},
                            new float[] {0, 0, 0, 0, 0},
                            new float[] {0, 0, 0, 1, 0},
                            new float[] {0, 0, 0, 0, 1}
                        }));
        }

        /// <summary>
        /// Causes a "Jitter" effect
        /// </summary>
        /// <param name="OriginalImage">Image to manipulate</param>
        /// <param name="MaxJitter">Maximum number of pixels the item can move</param>
        /// <returns>
        /// A SwiftBitmap object
        /// </returns>
        public static SwiftBitmap Jitter(this SwiftBitmap OriginalImage, int MaxJitter = 5)
        {
            Contract.Requires<ArgumentNullException>(OriginalImage != null, "OriginalImage");
            using (SwiftBitmap NewSwiftBitmap = new SwiftBitmap(OriginalImage.Width, OriginalImage.Height))
            {
                NewSwiftBitmap.Lock();
                OriginalImage.Lock();
                Parallel.For(0, OriginalImage.Width, x =>
                {
                    for (int y = 0; y < OriginalImage.Height; ++y)
                    {
                        int NewX = Random.Random.ThreadSafeNext(-MaxJitter, MaxJitter);
                        int NewY = Random.Random.ThreadSafeNext(-MaxJitter, MaxJitter);
                        NewX += x;
                        NewY += y;
                        NewX = NewX.Clamp(OriginalImage.Width - 1, 0);
                        NewY = NewY.Clamp(OriginalImage.Height - 1, 0);

                        NewSwiftBitmap.SetPixel(x, y, OriginalImage.GetPixel(NewX, NewY));
                    }
                });
                NewSwiftBitmap.Unlock();
                return OriginalImage.Unlock().Copy(NewSwiftBitmap);
            }
        }

        /// <summary>
        /// Does smoothing using a kuwahara blur
        /// </summary>
        /// <param name="OriginalImage">Image to manipulate</param>
        /// <param name="Size">Size of the aperture</param>
        /// <returns>
        /// A SwiftBitmap object
        /// </returns>
        public static SwiftBitmap KuwaharaBlur(this SwiftBitmap OriginalImage, int Size = 3)
        {
            Contract.Requires<ArgumentNullException>(OriginalImage != null, "OriginalImage");
            using (SwiftBitmap NewSwiftBitmap = new SwiftBitmap(OriginalImage.Width, OriginalImage.Height))
            {
                NewSwiftBitmap.Lock();
                OriginalImage.Lock();
                int[] ApetureMinX = { -(Size / 2), 0, -(Size / 2), 0 };
                int[] ApetureMaxX = { 0, (Size / 2), 0, (Size / 2) };
                int[] ApetureMinY = { -(Size / 2), -(Size / 2), 0, 0 };
                int[] ApetureMaxY = { 0, 0, (Size / 2), (Size / 2) };
                int Width = NewSwiftBitmap.Width;
                int Height = NewSwiftBitmap.Height;
                Parallel.For(0, Width, x =>
                {
                    for (int y = 0; y < Height; ++y)
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
                                if (TempX >= 0 && TempX < Width)
                                {
                                    for (int y2 = ApetureMinY[i]; y2 < ApetureMaxY[i]; ++y2)
                                    {
                                        int TempY = y + y2;
                                        if (TempY >= 0 && TempY < Height)
                                        {
                                            Color TempColor = OriginalImage.GetPixel(TempX, TempY);
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
                        NewSwiftBitmap.SetPixel(x, y, MeanPixel);
                    }
                });
                NewSwiftBitmap.Unlock();
                return OriginalImage.Unlock().Copy(NewSwiftBitmap);
            }
        }

        /// <summary>
        /// Laplace edge detection function
        /// </summary>
        /// <param name="Image">Image to manipulate</param>
        /// <returns>
        /// A SwiftBitmap object
        /// </returns>
        public static SwiftBitmap LaplaceEdgeDetection(this SwiftBitmap Image)
        {
            Contract.Requires<ArgumentNullException>(Image != null, "Image");
            return Image.BlackAndWhite().ApplyConvolutionFilter(new int[][]{
                            new int[] {-1, -1, -1, -1, -1},
                            new int[] {-1, -1, -1, -1, -1},
                            new int[] {-1, -1, 24, -1, -1},
                            new int[] {-1, -1, -1, -1, -1},
                            new int[] {-1, -1, -1, -1, -1},
                        });
        }

        /// <summary>
        /// Does smoothing using a median filter
        /// </summary>
        /// <param name="OriginalImage">Image to manipulate</param>
        /// <param name="Size">Size of the aperture</param>
        /// <returns>
        /// A SwiftBitmap image
        /// </returns>
        public static SwiftBitmap MedianFilter(this SwiftBitmap OriginalImage, int Size = 3)
        {
            Contract.Requires<ArgumentNullException>(OriginalImage != null, "OriginalImage");
            using (SwiftBitmap NewSwiftBitmap = new SwiftBitmap(OriginalImage.Width, OriginalImage.Height))
            {
                NewSwiftBitmap.Lock();
                OriginalImage.Lock();
                int ApetureMin = -(Size / 2);
                int ApetureMax = (Size / 2);
                int Width = NewSwiftBitmap.Width;
                int Height = NewSwiftBitmap.Height;
                Parallel.For(0, Width, x =>
                {
                    for (int y = 0; y < Height; ++y)
                    {
                        List<int> RValues = new List<int>();
                        List<int> GValues = new List<int>();
                        List<int> BValues = new List<int>();
                        for (int x2 = ApetureMin; x2 < ApetureMax; ++x2)
                        {
                            int TempX = x + x2;
                            if (TempX >= 0 && TempX < Width)
                            {
                                for (int y2 = ApetureMin; y2 < ApetureMax; ++y2)
                                {
                                    int TempY = y + y2;
                                    if (TempY >= 0 && TempY < Height)
                                    {
                                        Color TempColor = OriginalImage.GetPixel(TempX, TempY);
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
                        NewSwiftBitmap.SetPixel(x, y, MedianPixel);
                    }
                });
                NewSwiftBitmap.Unlock();
                return OriginalImage.Unlock().Copy(NewSwiftBitmap);
            }
        }

        /// <summary>
        /// gets the negative of the image
        /// </summary>
        /// <param name="OriginalImage">Image to manipulate</param>
        /// <returns>
        /// A SwiftBitmap image
        /// </returns>
        public static SwiftBitmap Negative(this SwiftBitmap OriginalImage)
        {
            Contract.Requires<ArgumentNullException>(OriginalImage != null, "OriginalImage");
            using (SwiftBitmap NewSwiftBitmap = new SwiftBitmap(OriginalImage.Width, OriginalImage.Height))
            {
                NewSwiftBitmap.Lock();
                OriginalImage.Lock();
                int Width = NewSwiftBitmap.Width;
                int Height = NewSwiftBitmap.Height;
                Parallel.For(0, Width, x =>
                {
                    for (int y = 0; y < Height; ++y)
                    {
                        Color CurrentPixel = OriginalImage.GetPixel(x, y);
                        Color TempValue = Color.FromArgb(255 - CurrentPixel.R, 255 - CurrentPixel.G, 255 - CurrentPixel.B);
                        NewSwiftBitmap.SetPixel(x, y, TempValue);
                    }
                });
                NewSwiftBitmap.Unlock();
                return OriginalImage.Unlock().Copy(NewSwiftBitmap);
            }
        }

        /// <summary>
        /// Creates the normal map
        /// </summary>
        /// <param name="ImageUsing">Image to create the normal map from</param>
        /// <param name="InvertX">Invert the X direction</param>
        /// <param name="InvertY">Invert the Y direction</param>
        /// <returns>Returns the resulting normal map</returns>
        public static SwiftBitmap NormalMap(this SwiftBitmap ImageUsing, bool InvertX = false, bool InvertY = false)
        {
            Contract.Requires<ArgumentNullException>(ImageUsing != null, "ImageUsing");
            using (SwiftBitmap TempImageX = ((SwiftBitmap)ImageUsing.Clone()).BumpMap(Direction.LeftRight, InvertX))
            {
                using (SwiftBitmap TempImageY = ((SwiftBitmap)ImageUsing.Clone()).BumpMap(Direction.TopBottom, InvertY))
                {
                    using (SwiftBitmap ReturnImage = new SwiftBitmap(TempImageX.Width, TempImageX.Height))
                    {
                        TempImageX.Lock();
                        TempImageY.Lock();
                        ReturnImage.Lock();
                        int Width = TempImageX.Width;
                        int Height = TempImageX.Height;
                        Parallel.For(0, Height, y =>
                        {
                            Vector3 TempVector = new Vector3(0.0, 0.0, 0.0);
                            for (int x = 0; x < Width; ++x)
                            {
                                Color TempPixelX = TempImageX.GetPixel(x, y);
                                Color TempPixelY = TempImageY.GetPixel(x, y);
                                TempVector.X = (double)(TempPixelX.R) / 255.0;
                                TempVector.Y = (double)(TempPixelY.R) / 255.0;
                                TempVector.Z = 1.0;
                                TempVector.Normalize();
                                TempVector.X = ((TempVector.X + 1.0) / 2.0) * 255.0;
                                TempVector.Y = ((TempVector.Y + 1.0) / 2.0) * 255.0;
                                TempVector.Z = ((TempVector.Z + 1.0) / 2.0) * 255.0;
                                ReturnImage.SetPixel(x, y,
                                    Color.FromArgb((int)TempVector.X,
                                        (int)TempVector.Y,
                                        (int)TempVector.Z));
                            }
                        });
                        TempImageX.Unlock();
                        TempImageY.Unlock();
                        ReturnImage.Unlock();
                        return ImageUsing.Copy(ReturnImage);
                    }
                }
            }
        }

        /// <summary>
        /// Pixelates an image
        /// </summary>
        /// <param name="OriginalImage">Image to pixelate</param>
        /// <param name="PixelSize">Size of the "pixels" in pixels</param>
        /// <returns>
        /// A SwiftBitmap image
        /// </returns>
        public static SwiftBitmap Pixelate(this SwiftBitmap OriginalImage, int PixelSize = 5)
        {
            Contract.Requires<ArgumentNullException>(OriginalImage != null, "OriginalImage");
            using (SwiftBitmap NewSwiftBitmap = new SwiftBitmap(OriginalImage.Width, OriginalImage.Height))
            {
                NewSwiftBitmap.Lock();
                OriginalImage.Lock();
                for (int x = 0; x < NewSwiftBitmap.Width; x += (PixelSize / 2))
                {
                    int MinX = (x - (PixelSize / 2)).Clamp(NewSwiftBitmap.Width, 0);
                    int MaxX = (x + (PixelSize / 2)).Clamp(NewSwiftBitmap.Width, 0);
                    for (int y = 0; y < NewSwiftBitmap.Height; y += (PixelSize / 2))
                    {
                        int RValue = 0;
                        int GValue = 0;
                        int BValue = 0;
                        int MinY = (y - (PixelSize / 2)).Clamp(NewSwiftBitmap.Height, 0);
                        int MaxY = (y + (PixelSize / 2)).Clamp(NewSwiftBitmap.Height, 0);
                        for (int x2 = MinX; x2 < MaxX; ++x2)
                        {
                            for (int y2 = MinY; y2 < MaxY; ++y2)
                            {
                                Color Pixel = OriginalImage.GetPixel(x2, y2);
                                RValue += Pixel.R;
                                GValue += Pixel.G;
                                BValue += Pixel.B;
                            }
                        }
                        RValue = RValue / (PixelSize * PixelSize);
                        GValue = GValue / (PixelSize * PixelSize);
                        BValue = BValue / (PixelSize * PixelSize);
                        Color TempPixel = Color.FromArgb(RValue.Clamp(255, 0), GValue.Clamp(255, 0), BValue.Clamp(255, 0));
                        Parallel.For(MinX, MaxX, x2 =>
                        {
                            for (int y2 = MinY; y2 < MaxY; ++y2)
                            {
                                NewSwiftBitmap.SetPixel(x2, y2, TempPixel);
                            }
                        });
                    }
                }
                NewSwiftBitmap.Unlock();
                return OriginalImage.Unlock().Copy(NewSwiftBitmap);
            }
        }

        /// <summary>
        /// Gets the Red filter for an image
        /// </summary>
        /// <param name="Image">Image to change</param>
        /// <returns>
        /// A SwiftBitmap image
        /// </returns>
        public static SwiftBitmap RedFilter(this SwiftBitmap Image)
        {
            Contract.Requires<ArgumentNullException>(Image != null, "Image");
            return Image.ApplyColorMatrix(new ColorMatrix(new float[][]{
                            new float[] {1, 0, 0, 0, 0},
                            new float[] {0, 0, 0, 0, 0},
                            new float[] {0, 0, 0, 0, 0},
                            new float[] {0, 0, 0, 1, 0},
                            new float[] {0, 0, 0, 0, 1}
                        }));
        }

        /// <summary>
        /// Converts an SwiftBitmap to sepia tone
        /// </summary>
        /// <param name="Image">Image to change</param>
        /// <returns>
        /// A SwiftBitmap object of the sepia tone image
        /// </returns>
        public static SwiftBitmap SepiaTone(this SwiftBitmap Image)
        {
            Contract.Requires<ArgumentNullException>(Image != null, "Image");
            return Image.ApplyColorMatrix(new ColorMatrix(new float[][]{
                            new float[] {.393f, .349f, .272f, 0, 0},
                            new float[] {.769f, .686f, .534f, 0, 0},
                            new float[] {.189f, .168f, .131f, 0, 0},
                            new float[] {0, 0, 0, 1, 0},
                            new float[] {0, 0, 0, 0, 1}
                        }));
        }

        /// <summary>
        /// Sharpens an image
        /// </summary>
        /// <param name="Image">Image to manipulate</param>
        /// <returns>
        /// A SwiftBitmap image
        /// </returns>
        public static SwiftBitmap Sharpen(this SwiftBitmap Image)
        {
            Contract.Requires<ArgumentNullException>(Image != null, "Image");
            return Image.ApplyConvolutionFilter(new int[][]{
                            new int[] {-1, -2, -1},
                            new int[] {-2, 16, -2},
                            new int[] {-1, -2, -1}
                        });
        }

        /// <summary>
        /// Sharpens an image
        /// </summary>
        /// <param name="Image">Image to manipulate</param>
        /// <returns>
        /// A SwiftBitmap image
        /// </returns>
        public static SwiftBitmap SharpenLess(this SwiftBitmap Image)
        {
            Contract.Requires<ArgumentNullException>(Image != null, "Image");
            return Image.ApplyConvolutionFilter(new int[][]{
                            new int[] {-1, 0, -1},
                            new int[] {0,  7,  0},
                            new int[] {-1, 0, -1}
                        });
        }

        /// <summary>
        /// Does a "wave" effect on the image
        /// </summary>
        /// <param name="OriginalImage">Image to manipulate</param>
        /// <param name="Amplitude">Amplitude of the sine wave</param>
        /// <param name="Frequency">Frequency of the sine wave</param>
        /// <param name="XDirection">Determines if this should be done in the X direction</param>
        /// <param name="YDirection">Determines if this should be done in the Y direction</param>
        /// <returns>
        /// A SwiftBitmap which has been modified
        /// </returns>
        public static SwiftBitmap SinWave(this SwiftBitmap OriginalImage, float Amplitude, float Frequency, bool XDirection, bool YDirection)
        {
            Contract.Requires<ArgumentNullException>(OriginalImage != null, "OriginalImage");
            using (SwiftBitmap NewSwiftBitmap = new SwiftBitmap(OriginalImage.Width, OriginalImage.Height))
            {
                NewSwiftBitmap.Lock();
                OriginalImage.Lock();
                int Width = NewSwiftBitmap.Width;
                int Height = NewSwiftBitmap.Height;
                Parallel.For(0, Width, x =>
                {
                    for (int y = 0; y < Height; ++y)
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
                            Value1 += Height;
                        while (Value2 < 0)
                            Value2 += Width;
                        while (Value1 >= Height)
                            Value1 -= Height;
                        while (Value2 >= Width)
                            Value2 -= Width;
                        NewSwiftBitmap.SetPixel(x, y, OriginalImage.GetPixel((int)Value2, (int)Value1));
                    }
                });
                NewSwiftBitmap.Unlock();
                return OriginalImage.Unlock().Copy(NewSwiftBitmap);
            }
        }

        /// <summary>
        /// Does smoothing using a SNN blur
        /// </summary>
        /// <param name="OriginalImage">Image to manipulate</param>
        /// <param name="Size">Size of the aperture</param>
        /// <returns>
        /// The resulting SwiftBitmap
        /// </returns>
        public static SwiftBitmap SNNBlur(this SwiftBitmap OriginalImage, int Size = 3)
        {
            Contract.Requires<ArgumentNullException>(OriginalImage != null, "OriginalImage");
            using (SwiftBitmap NewSwiftBitmap = new SwiftBitmap(OriginalImage.Width, OriginalImage.Height))
            {
                NewSwiftBitmap.Lock();
                OriginalImage.Lock();
                int ApetureMinX = -(Size / 2);
                int ApetureMaxX = (Size / 2);
                int ApetureMinY = -(Size / 2);
                int ApetureMaxY = (Size / 2);
                int Width = NewSwiftBitmap.Width;
                int Height = NewSwiftBitmap.Height;
                Parallel.For(0, Width, x =>
                {
                    for (int y = 0; y < Height; ++y)
                    {
                        int RValue = 0;
                        int GValue = 0;
                        int BValue = 0;
                        int NumPixels = 0;
                        for (int x2 = ApetureMinX; x2 < ApetureMaxX; ++x2)
                        {
                            int TempX1 = x + x2;
                            int TempX2 = x - x2;
                            if (TempX1 >= 0 && TempX1 < Width && TempX2 >= 0 && TempX2 < Width)
                            {
                                for (int y2 = ApetureMinY; y2 < ApetureMaxY; ++y2)
                                {
                                    int TempY1 = y + y2;
                                    int TempY2 = y - y2;
                                    if (TempY1 >= 0 && TempY1 < Height && TempY2 >= 0 && TempY2 < Height)
                                    {
                                        Color TempColor = OriginalImage.GetPixel(x, y);
                                        Color TempColor2 = OriginalImage.GetPixel(TempX1, TempY1);
                                        Color TempColor3 = OriginalImage.GetPixel(TempX2, TempY2);
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
                        NewSwiftBitmap.SetPixel(x, y, MeanPixel);
                    }
                });
                NewSwiftBitmap.Unlock();
                return OriginalImage.Unlock().Copy(NewSwiftBitmap);
            }
        }

        /// <summary>
        /// Sobel edge detection function
        /// </summary>
        /// <param name="Input">Image to manipulate</param>
        /// <returns>
        /// A SwiftBitmap image
        /// </returns>
        public static SwiftBitmap SobelEdgeDetection(this SwiftBitmap Input)
        {
            Contract.Requires<ArgumentNullException>(Input != null, "Input");
            Input.BlackAndWhite();
            using (SwiftBitmap TempImageX = ((SwiftBitmap)Input.Clone()).ApplyConvolutionFilter(new int[][]{
                            new int[] {-1, 0, 1},
                            new int[] {-2, 0, 2},
                            new int[] {-1, 0, 1}
                        }, true))
            {
                using (SwiftBitmap TempImageY = ((SwiftBitmap)Input.Clone()).ApplyConvolutionFilter(new int[][]{
                            new int[] {1, 2, 1},
                            new int[] {0, 0, 0},
                            new int[] {-1, -2, -1}
                        }, true))
                {
                    using (SwiftBitmap NewBitmap = new SwiftBitmap(Input.Width, Input.Height))
                    {
                        NewBitmap.Lock();
                        TempImageX.Lock();
                        TempImageY.Lock();
                        int Width = NewBitmap.Width;
                        int Height = NewBitmap.Height;
                        Parallel.For(0, Width, x =>
                        {
                            for (int y = 0; y < Height; ++y)
                            {
                                Color Pixel1 = TempImageX.GetPixel(x, y);
                                Color Pixel2 = TempImageY.GetPixel(x, y);
                                NewBitmap.SetPixel(x, y,
                                    Color.FromArgb((Pixel1.R + Pixel2.R).Clamp(255, 0),
                                        (Pixel1.G + Pixel2.G).Clamp(255, 0),
                                        (Pixel1.B + Pixel2.B).Clamp(255, 0)));
                            }
                        });
                        NewBitmap.Unlock();
                        TempImageY.Unlock();
                        TempImageX.Unlock();
                        return Input.Copy(NewBitmap).Negative();
                    }
                }
            }
        }

        /// <summary>
        /// Sobel emboss function
        /// </summary>
        /// <param name="Image">Image to manipulate</param>
        /// <returns>
        /// A SwiftBitmap image
        /// </returns>
        public static SwiftBitmap SobelEmboss(this SwiftBitmap Image)
        {
            Contract.Requires<ArgumentNullException>(Image != null, "Image");
            return Image.ApplyConvolutionFilter(new int[][]{
                            new int[] {-1, 0, 1},
                            new int[] {-2, 0, 2},
                            new int[] {-1, 0, 1}
                        }, offset: 127);
        }

        /// <summary>
        /// Stretches the contrast
        /// </summary>
        /// <param name="OriginalImage">Image to manipulate</param>
        /// <returns>
        /// A SwiftBitmap image
        /// </returns>
        public static SwiftBitmap StretchContrast(this SwiftBitmap OriginalImage)
        {
            Contract.Requires<ArgumentNullException>(OriginalImage != null, "OriginalImage");
            using (SwiftBitmap NewBitmap = new SwiftBitmap(OriginalImage.Width, OriginalImage.Height))
            {
                NewBitmap.Lock();
                OriginalImage.Lock();
                Color MinValue;
                Color MaxValue;
                GetMinMaxPixel(out MinValue, out MaxValue, OriginalImage);
                int Width = NewBitmap.Width;
                int Height = NewBitmap.Height;
                Parallel.For(0, Width, x =>
                {
                    for (int y = 0; y < Height; ++y)
                    {
                        Color CurrentPixel = OriginalImage.GetPixel(x, y);
                        Color TempValue = Color.FromArgb(Map(CurrentPixel.R, MinValue.R, MaxValue.R),
                            Map(CurrentPixel.G, MinValue.G, MaxValue.G),
                            Map(CurrentPixel.B, MinValue.B, MaxValue.B));
                        NewBitmap.SetPixel(x, y, TempValue);
                    }
                });
                NewBitmap.Unlock();
                return OriginalImage.Unlock().Copy(NewBitmap);
            }
        }

        /// <summary>
        /// Does threshold manipulation of the image
        /// </summary>
        /// <param name="OriginalImage">Image to transform</param>
        /// <param name="Threshold">Float defining the threshold at which to set the pixel to black vs white.</param>
        /// <returns>
        /// A SwiftBitmap object containing the new image
        /// </returns>
        public static SwiftBitmap Threshold(this SwiftBitmap OriginalImage, float Threshold = 0.5f)
        {
            Contract.Requires<ArgumentNullException>(OriginalImage != null, "OriginalImage");
            using (SwiftBitmap NewSwiftBitmap = new SwiftBitmap(OriginalImage.Width, OriginalImage.Height))
            {
                NewSwiftBitmap.Lock();
                OriginalImage.Lock();
                int Width = NewSwiftBitmap.Width;
                int Height = NewSwiftBitmap.Height;
                Parallel.For(0, Width, x =>
                {
                    for (int y = 0; y < Height; ++y)
                    {
                        Color TempColor = OriginalImage.GetPixel(x, y);
                        if ((TempColor.R + TempColor.G + TempColor.B) / 755.0f > Threshold)
                            NewSwiftBitmap.SetPixel(x, y, Color.White);
                        else
                            NewSwiftBitmap.SetPixel(x, y, Color.Black);
                    }
                });
                NewSwiftBitmap.Unlock();
                return OriginalImage.Unlock().Copy(NewSwiftBitmap);
            }
        }

        /// <summary>
        /// Converts an SwiftBitmap to ASCII art
        /// </summary>
        /// <param name="Input">The SwiftBitmap you wish to convert</param>
        /// <returns>A string containing the art</returns>
        public static string ToASCIIArt(this SwiftBitmap Input)
        {
            Contract.Requires<ArgumentNullException>(Input != null, "Input");
            bool ShowLine = true;
            using (SwiftBitmap TempImage = ((SwiftBitmap)Input.Clone()).BlackAndWhite().Lock())
            {
                StringBuilder Builder = new StringBuilder();
                for (int x = 0; x < TempImage.Height; ++x)
                {
                    for (int y = 0; y < TempImage.Width; ++y)
                    {
                        if (ShowLine)
                        {
                            Color CurrentPixel = TempImage.GetPixel(y, x);
                            Builder.Append(_ASCIICharacters[((CurrentPixel.R * _ASCIICharacters.Length) / 255)]);
                        }
                    }
                    if (ShowLine)
                    {
                        Builder.Append(System.Environment.NewLine);
                        ShowLine = false;
                    }
                    else
                    {
                        ShowLine = true;
                    }
                }
                TempImage.Unlock();
                return Builder.ToString();
            }
        }

        /// <summary>
        /// Does turbulence manipulation of the image
        /// </summary>
        /// <param name="OriginalImage">Image to transform</param>
        /// <param name="Roughness">Roughness of the movement</param>
        /// <param name="Power">How strong the movement is</param>
        /// <param name="Seed">Random seed</param>
        /// <returns>
        /// A SwiftBitmap object containing the new image
        /// </returns>
        public static SwiftBitmap Turbulence(this SwiftBitmap OriginalImage, int Roughness = 8, float Power = 5.0f, int Seed = 25123864)
        {
            Contract.Requires<ArgumentNullException>(OriginalImage != null, "OriginalImage");
            int Width = OriginalImage.Width;
            int Height = OriginalImage.Height;
            OriginalImage.Lock();
            using (SwiftBitmap NewSwiftBitmap = new SwiftBitmap(Width, Height))
            {
                NewSwiftBitmap.Lock();
                using (SwiftBitmap XNoise = PerlinNoise.Generate(Width, Height, 255, 0, 0.0625f, 1.0f, 0.5f, Roughness, Seed))
                {
                    XNoise.Lock();
                    using (SwiftBitmap YNoise = PerlinNoise.Generate(Width, Height, 255, 0, 0.0625f, 1.0f, 0.5f, Roughness, Seed * 2))
                    {
                        YNoise.Lock();
                        Parallel.For(0, Height, y =>
                        {
                            for (int x = 0; x < Width; ++x)
                            {
                                float XDistortion = x + (GetHeight(x, y, XNoise) * Power);
                                float YDistortion = y + (GetHeight(x, y, YNoise) * Power);
                                int X1 = ((int)XDistortion).Clamp(Width - 1, 0);
                                int Y1 = ((int)YDistortion).Clamp(Height - 1, 0);
                                NewSwiftBitmap.SetPixel(x, y, OriginalImage.GetPixel(X1, Y1));
                            }
                        });
                        YNoise.Unlock();
                    }
                    XNoise.Unlock();
                }
                NewSwiftBitmap.Unlock();
                return OriginalImage.Unlock().Copy(NewSwiftBitmap);
            }
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
        /// <returns>
        /// The results in the form of a SwiftBitmap object
        /// </returns>
        public static SwiftBitmap Watermark(this SwiftBitmap Image, SwiftBitmap WatermarkImage, float Opacity, int X, int Y, Color KeyColor)
        {
            Contract.Requires<ArgumentNullException>(Image != null, "Image");
            Contract.Requires<ArgumentNullException>(WatermarkImage != null, "WatermarkImage");
            using (SwiftBitmap NewSwiftBitmap = new SwiftBitmap(Image.Width, Image.Height).Copy(Image))
            {
                using (Graphics NewGraphics = Graphics.FromImage(NewSwiftBitmap.InternalBitmap))
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
                        NewGraphics.DrawImage(WatermarkImage.InternalBitmap,
                            new System.Drawing.Rectangle(X, Y, WatermarkImage.Width, WatermarkImage.Height),
                            0, 0, WatermarkImage.Width, WatermarkImage.Height,
                            GraphicsUnit.Pixel,
                            Attributes);
                    }
                }
                return Image.Copy(NewSwiftBitmap);
            }
        }

        private static double Distance(int R1, int R2, int G1, int G2, int B1, int B2)
        {
            return ((double)(((R1 - R2) * (R1 - R2)) + ((G1 - G2) * (G1 - G2)) + ((B1 - B2) * (B1 - B2)))).Sqrt();
        }

        private static float GetHeight(int x, int y, SwiftBitmap BlackAndWhiteData)
        {
            Contract.Requires<ArgumentNullException>(BlackAndWhiteData != null, "BlackAndWhiteData");
            Color TempColor = BlackAndWhiteData.GetPixel(x, y);
            return GetHeight(TempColor);
        }

        private static float GetHeight(Color Color)
        {
            return (float)Color.R / 255.0f;
        }

        private static void GetMinMaxPixel(out Color Min, out Color Max, SwiftBitmap ImageData)
        {
            Contract.Requires<ArgumentNullException>(ImageData != null, "ImageData");
            int MinR = 255, MinG = 255, MinB = 255;
            int MaxR = 0, MaxG = 0, MaxB = 0;
            for (int x = 0; x < ImageData.Width; ++x)
            {
                for (int y = 0; y < ImageData.Height; ++y)
                {
                    Color TempImage = ImageData.GetPixel(x, y);
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

        private static int Map(int Value, int Min, int Max)
        {
            double TempVal = (Value - Min);
            TempVal /= (double)(Max - Min);
            return ((int)(TempVal * 255)).Clamp(255, 0);
        }
    }
}