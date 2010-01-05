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
        public static Bitmap Process(Bitmap NewImage, Bitmap OldImage, int Threshold,Color DetectionColor)
        {
            Bitmap NewImage1 = Utilities.Media.Image.Image.ConvertBlackAndWhite(NewImage);
            Bitmap OldImage1 = Utilities.Media.Image.Image.ConvertBlackAndWhite(OldImage);
            Bitmap NewImage2 = Utilities.Media.Image.Image.SNNBlur(NewImage1, 5);
            NewImage1.Dispose();
            Bitmap OldImage2 = Utilities.Media.Image.Image.SNNBlur(OldImage1, 5);
            OldImage1.Dispose();

            Bitmap OutputImage = new Bitmap(NewImage, NewImage.Width, NewImage.Height);
            Bitmap Overlay = new Bitmap(NewImage, NewImage.Width, NewImage.Height);

            for (int x = 0; x < OutputImage.Width; ++x)
            {
                for (int y = 0; y < OutputImage.Height; ++y)
                {
                    Color NewPixel = NewImage2.GetPixel(x, y);
                    Color OldPixel = OldImage2.GetPixel(x, y);
                    OutputImage.SetPixel(x, y, NewPixel);
                    if (System.Math.Pow((double)(NewPixel.R - OldPixel.R), 2.0) > Threshold)
                    {
                        Overlay.SetPixel(x, y, Color.FromArgb(100,0,100));
                    }
                    else
                    {
                        Overlay.SetPixel(x, y, Color.FromArgb(200, 0, 200));
                    }
                }
            }
            OldImage2.Dispose();
            NewImage2.Dispose();
            Bitmap Overlay2 = Utilities.Media.Image.Image.EdgeDetection(Overlay, 25, DetectionColor);
            for (int x = 0; x < OutputImage.Width; ++x)
            {
                for (int y = 0; y < OutputImage.Height; ++y)
                {
                    Color Pixel1 = Overlay2.GetPixel(x, y);
                    if (Pixel1.R != DetectionColor.R || Pixel1.G != DetectionColor.G || Pixel1.B != DetectionColor.B)
                    {
                        Overlay2.SetPixel(x, y, Color.FromArgb(200, 0, 200));
                    }
                }
            }
            Overlay.Dispose();
            Bitmap ReturnImage = Utilities.Media.Image.Image.Watermark(OutputImage, Overlay2, 1.0f, 0, 0, Color.FromArgb(200, 0, 200));
            Overlay2.Dispose();
            OutputImage.Dispose();
            return ReturnImage;
        }

        #endregion
    }
}
