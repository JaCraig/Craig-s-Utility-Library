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
    /// Class for creating a normal map
    /// </summary>
    public class NormalMap
    {
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public NormalMap()
        {
            InvertX = false;
            InvertY = false;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Determines the direction of the normal map in the x direction
        /// </summary>
        public bool InvertX { get; set; }

        /// <summary>
        /// Determines the direction of the normal map in the y direction
        /// </summary>
        public bool InvertY { get; set; }

        /// <summary>
        /// X filter
        /// </summary>
        protected BumpMap FilterX { get; set; }

        /// <summary>
        /// Y filter
        /// </summary>
        protected BumpMap FilterY { get; set; }

        #endregion

        #region Protected Functions

        /// <summary>
        /// Sets up the edge detection filter
        /// </summary>
        protected void CreateFilter()
        {
            FilterX = new BumpMap();
            FilterY = new BumpMap();
            FilterX.Invert = InvertX;
            FilterY.Invert = InvertY;
            FilterX.Direction = Direction.LeftRight;
            FilterY.Direction = Direction.TopBottom;
        }

        #endregion

        #region Public Functions

        /// <summary>
        /// Creates the bump map
        /// </summary>
        public Bitmap Create(Bitmap ImageUsing)
        {
            CreateFilter();
            using (Bitmap TempImageX = FilterX.Create(ImageUsing))
            {
                using (Bitmap TempImageY = FilterY.Create(ImageUsing))
                {
                    Bitmap ReturnImage = new Bitmap(TempImageX.Width, TempImageX.Height);
                    Math.Vector3 TempVector = new Utilities.Math.Vector3(0.0, 0.0, 0.0);
                    BitmapData TempImageXData = Image.LockImage(TempImageX);
                    BitmapData TempImageYData = Image.LockImage(TempImageY);
                    BitmapData ReturnImageData = Image.LockImage(ReturnImage);
                    int TempImageXPixelSize = Image.GetPixelSize(TempImageXData);
                    int TempImageYPixelSize = Image.GetPixelSize(TempImageYData);
                    int ReturnImagePixelSize = Image.GetPixelSize(ReturnImageData);
                    for (int y = 0; y < TempImageX.Height; ++y)
                    {
                        for (int x = 0; x < TempImageX.Width; ++x)
                        {
                            Color TempPixelX = Image.GetPixel(TempImageXData, x, y, TempImageXPixelSize);
                            Color TempPixelY = Image.GetPixel(TempImageYData, x, y, TempImageYPixelSize);
                            TempVector.X = (double)(TempPixelX.R) / 255.0;
                            TempVector.Y = (double)(TempPixelY.R) / 255.0;
                            TempVector.Z = 1.0;
                            TempVector.Normalize();
                            TempVector.X = ((TempVector.X + 1.0) / 2.0) * 255.0;
                            TempVector.Y = ((TempVector.Y + 1.0) / 2.0) * 255.0;
                            TempVector.Z = ((TempVector.Z + 1.0) / 2.0) * 255.0;
                            Image.SetPixel(ReturnImageData, x, y,
                                Color.FromArgb((int)TempVector.X,
                                    (int)TempVector.Y,
                                    (int)TempVector.Z),
                                ReturnImagePixelSize);
                        }
                    }
                    Image.UnlockImage(TempImageX, TempImageXData);
                    Image.UnlockImage(TempImageY, TempImageYData);
                    Image.UnlockImage(ReturnImage, ReturnImageData);
                    return ReturnImage;
                }
            }
        }

        #endregion
    }
}