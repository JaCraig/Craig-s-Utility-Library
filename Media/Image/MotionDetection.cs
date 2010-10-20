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
            using (Bitmap NewImage1 = Utilities.Media.Image.Image.ConvertBlackAndWhite(NewImage))
            {
                using (Bitmap OldImage1 = Utilities.Media.Image.Image.ConvertBlackAndWhite(OldImage))
                {
                    using (Bitmap NewImage2 = Utilities.Media.Image.Image.SNNBlur(NewImage1, 5))
                    {
                        using (Bitmap OldImage2 = Utilities.Media.Image.Image.SNNBlur(OldImage1, 5))
                        {
                            using (Bitmap OutputImage = new Bitmap(NewImage2, NewImage2.Width, NewImage2.Height))
                            {
                                using (Bitmap Overlay = new Bitmap(NewImage, NewImage.Width, NewImage.Height))
                                {
                                    BitmapData NewImage2Data = Image.LockImage(NewImage2);
                                    int NewImage2PixelSize = Image.GetPixelSize(NewImage2Data);
                                    BitmapData OldImage2Data = Image.LockImage(OldImage2);
                                    int OldImage2PixelSize = Image.GetPixelSize(OldImage2Data);
                                    BitmapData OverlayData = Image.LockImage(Overlay);
                                    int OverlayPixelSize = Image.GetPixelSize(OverlayData);
                                    for (int x = 0; x < OutputImage.Width; ++x)
                                    {
                                        for (int y = 0; y < OutputImage.Height; ++y)
                                        {
                                            Color NewPixel = Image.GetPixel(NewImage2Data, x, y, NewImage2PixelSize);
                                            Color OldPixel = Image.GetPixel(OldImage2Data, x, y, OldImage2PixelSize);
                                            if (System.Math.Pow((double)(NewPixel.R - OldPixel.R), 2.0) > Threshold)
                                            {
                                                Image.SetPixel(OverlayData, x, y, Color.FromArgb(100, 0, 100), OverlayPixelSize);
                                            }
                                            else
                                            {
                                                Image.SetPixel(OverlayData, x, y, Color.FromArgb(200, 0, 200), OverlayPixelSize);
                                            }
                                        }
                                    }
                                    Image.UnlockImage(Overlay, OverlayData);
                                    Image.UnlockImage(NewImage2, NewImage2Data);
                                    Image.UnlockImage(OldImage2, OldImage2Data);
                                    using (Bitmap Overlay2 = Utilities.Media.Image.Image.EdgeDetection(Overlay, 25, DetectionColor))
                                    {
                                        BitmapData Overlay2Data = Image.LockImage(Overlay2);
                                        int Overlay2PixelSize = Image.GetPixelSize(Overlay2Data);
                                        for (int x = 0; x < OutputImage.Width; ++x)
                                        {
                                            for (int y = 0; y < OutputImage.Height; ++y)
                                            {
                                                Color Pixel1 = Image.GetPixel(Overlay2Data, x, y, Overlay2PixelSize);
                                                if (Pixel1.R != DetectionColor.R || Pixel1.G != DetectionColor.G || Pixel1.B != DetectionColor.B)
                                                {
                                                    Image.SetPixel(Overlay2Data, x, y, Color.FromArgb(200, 0, 200), Overlay2PixelSize);
                                                }
                                            }
                                        }
                                        Image.UnlockImage(Overlay2, Overlay2Data);
                                        return Utilities.Media.Image.Image.Watermark(OutputImage, Overlay2, 1.0f, 0, 0, Color.FromArgb(200, 0, 200));
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