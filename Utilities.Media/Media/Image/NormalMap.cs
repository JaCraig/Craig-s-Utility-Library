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
        public virtual bool InvertX { get; set; }

        /// <summary>
        /// Determines the direction of the normal map in the y direction
        /// </summary>
        public virtual bool InvertY { get; set; }

        /// <summary>
        /// X filter
        /// </summary>
        protected virtual BumpMap FilterX { get; set; }

        /// <summary>
        /// Y filter
        /// </summary>
        protected virtual BumpMap FilterY { get; set; }

        #endregion

        #region Protected Functions

        /// <summary>
        /// Sets up the edge detection filter
        /// </summary>
        protected virtual void CreateFilter()
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
        public virtual Bitmap Create(Bitmap ImageUsing)
        {
            if (ImageUsing == null)
                throw new ArgumentNullException("ImageUsing");
            CreateFilter();
            using (Bitmap TempImageX = FilterX.Create(ImageUsing))
            {
                using (Bitmap TempImageY = FilterY.Create(ImageUsing))
                {
                    Bitmap ReturnImage = new Bitmap(TempImageX.Width, TempImageX.Height);
                    Math.Vector3 TempVector = new Utilities.Math.Vector3(0.0, 0.0, 0.0);
                    BitmapData TempImageXData = TempImageX.LockImage();
                    BitmapData TempImageYData = TempImageY.LockImage();
                    BitmapData ReturnImageData = ReturnImage.LockImage();
                    int TempImageXPixelSize = TempImageXData.GetPixelSize();
                    int TempImageYPixelSize = TempImageYData.GetPixelSize();
                    int ReturnImagePixelSize = ReturnImageData.GetPixelSize();
                    for (int y = 0; y < TempImageX.Height; ++y)
                    {
                        for (int x = 0; x < TempImageX.Width; ++x)
                        {
                            Color TempPixelX = TempImageXData.GetPixel(x, y, TempImageXPixelSize);
                            Color TempPixelY = TempImageYData.GetPixel(x, y, TempImageYPixelSize);
                            TempVector.X = (double)(TempPixelX.R) / 255.0;
                            TempVector.Y = (double)(TempPixelY.R) / 255.0;
                            TempVector.Z = 1.0;
                            TempVector.Normalize();
                            TempVector.X = ((TempVector.X + 1.0) / 2.0) * 255.0;
                            TempVector.Y = ((TempVector.Y + 1.0) / 2.0) * 255.0;
                            TempVector.Z = ((TempVector.Z + 1.0) / 2.0) * 255.0;
                            ReturnImageData.SetPixel(x, y,
                                Color.FromArgb((int)TempVector.X,
                                    (int)TempVector.Y,
                                    (int)TempVector.Z),
                                ReturnImagePixelSize);
                        }
                    }
                    TempImageX.UnlockImage(TempImageXData);
                    TempImageY.UnlockImage(TempImageYData);
                    ReturnImage.UnlockImage(ReturnImageData);
                    return ReturnImage;
                }
            }
        }

        #endregion
    }
}