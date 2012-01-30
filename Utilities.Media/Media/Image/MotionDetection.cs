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
using System;
using Utilities.Media.Image.ExtensionMethods;
using Utilities.DataTypes.ExtensionMethods;
using System.Threading.Tasks;
#endregion

namespace Utilities.Media.Image
{
    /// <summary>
    /// Used for motion detection
    /// </summary>
    public static class MotionDetection
    {
        #region Public Static Functions

        /// <summary>
        /// Runs the motion detection algorithm
        /// </summary>
        /// <param name="NewImage">The "new" frame</param>
        /// <param name="OldImage">The "old" frame</param>
        /// <param name="Threshold">The threshold used to detect changes in the image</param>
        /// <param name="DetectionColor">Color to display changes in the images as</param>
        /// <returns>A bitmap indicating where changes between frames have occurred overlayed on top of the new image.</returns>
        public static Bitmap Process(Bitmap NewImage, Bitmap OldImage, int Threshold, Color DetectionColor)
        {
            NewImage.ThrowIfNull("NewImage");
            OldImage.ThrowIfNull("OldImage");
            DetectionColor.ThrowIfNull("DetectionColor");
            using (Bitmap NewImage1 = NewImage.BlackAndWhite())
            {
                using (Bitmap OldImage1 = OldImage.BlackAndWhite())
                {
                    using (Bitmap NewImage2 = NewImage1.SNNBlur(5))
                    {
                        using (Bitmap OldImage2 = OldImage1.SNNBlur(5))
                        {
                            using (Bitmap OutputImage = new Bitmap(NewImage2, NewImage2.Width, NewImage2.Height))
                            {
                                using (Bitmap Overlay = new Bitmap(NewImage, NewImage.Width, NewImage.Height))
                                {
                                    BitmapData NewImage2Data = NewImage2.LockImage();
                                    int NewImage2PixelSize = NewImage2Data.GetPixelSize();
                                    BitmapData OldImage2Data = OldImage2.LockImage();
                                    int OldImage2PixelSize = OldImage2Data.GetPixelSize();
                                    BitmapData OverlayData = Overlay.LockImage();
                                    int OverlayPixelSize = OverlayData.GetPixelSize();
                                    int Width = OutputImage.Width;
                                    int Height = OutputImage.Height;
                                    Parallel.For(0, Width, x =>
                                    {
                                        for (int y = 0; y < Height; ++y)
                                        {
                                            Color NewPixel = NewImage2Data.GetPixel(x, y, NewImage2PixelSize);
                                            Color OldPixel = OldImage2Data.GetPixel(x, y, OldImage2PixelSize);
                                            if (System.Math.Pow((double)(NewPixel.R - OldPixel.R), 2.0) > Threshold)
                                            {
                                                OverlayData.SetPixel(x, y, Color.FromArgb(100, 0, 100), OverlayPixelSize);
                                            }
                                            else
                                            {
                                                OverlayData.SetPixel(x, y, Color.FromArgb(200, 0, 200), OverlayPixelSize);
                                            }
                                        }
                                    });
                                    Overlay.UnlockImage(OverlayData);
                                    NewImage2.UnlockImage(NewImage2Data);
                                    OldImage2.UnlockImage(OldImage2Data);
                                    using (Bitmap Overlay2 = Overlay.EdgeDetection(25, DetectionColor))
                                    {
                                        BitmapData Overlay2Data = Overlay2.LockImage();
                                        int Overlay2PixelSize = Overlay2Data.GetPixelSize();
                                        Width = OutputImage.Width;
                                        Height = OutputImage.Height;
                                        Parallel.For(0, Width, x =>
                                        {
                                            for (int y = 0; y < Height; ++y)
                                            {
                                                Color Pixel1 = Overlay2Data.GetPixel(x, y, Overlay2PixelSize);
                                                if (Pixel1.R != DetectionColor.R || Pixel1.G != DetectionColor.G || Pixel1.B != DetectionColor.B)
                                                {
                                                    Overlay2Data.SetPixel(x, y, Color.FromArgb(200, 0, 200), Overlay2PixelSize);
                                                }
                                            }
                                        });
                                        Overlay2.UnlockImage(Overlay2Data);
                                        return OutputImage.Watermark(Overlay2, 1.0f, 0, 0, Color.FromArgb(200, 0, 200));
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        #endregion
    }
}