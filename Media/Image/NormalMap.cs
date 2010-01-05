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
using System;
using System.Drawing;
#endregion

namespace Utilities.Media.Image
{
    /// <summary>
    /// Class for creating a normal map
    /// </summary>
    public class NormalMap : IDisposable
    {
        #region Private Variables
        private System.Drawing.Bitmap _Image = null;
        private BumpMap _FilterX = null;
        private BumpMap _FilterY = null;
        private bool _InvertX = false;
        private bool _InvertY = false;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        public NormalMap()
        {
            try
            {
                CreateFilter();
            }
            catch (Exception e) { throw e; }
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="FileName">Name of the file</param>
        public NormalMap(string FileName)
        {
            try
            {
                _Image = (Bitmap)System.Drawing.Bitmap.FromFile(FileName);
                CreateFilter();
                CreateNormalMap();
            }
            catch (Exception e) { throw e; }
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Image">Image to use</param>
        public NormalMap(System.Drawing.Bitmap Image)
        {
            try
            {
                _Image = new Bitmap(Image);
                CreateFilter();
                CreateNormalMap();
            }
            catch (Exception e) { throw e; }
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// The internal image
        /// </summary>
        public System.Drawing.Bitmap Image
        {
            get { return _Image; }
            set
            {
                try
                {
                    _Image = value;
                    CreateNormalMap();
                }
                catch (Exception e) { throw e; }
            }
        }

        /// <summary>
        /// Determines the direction of the normal map in the x direction
        /// </summary>
        public bool InvertX
        {
            get { return _InvertX; }
            set
            {
                try
                {
                    _InvertX = value;
                    CreateFilter();
                    CreateNormalMap();
                }
                catch (Exception e) { throw e; }
            }
        }

        /// <summary>
        /// Determines the direction of the normal map in the y direction
        /// </summary>
        public bool InvertY
        {
            get { return _InvertY; }
            set
            {
                try
                {
                    _InvertY = value;
                    CreateFilter();
                    CreateNormalMap();
                }
                catch (Exception e) { throw e; }
            }
        }
        #endregion

        #region Private Functions
        /// <summary>
        /// Creates the bump map
        /// </summary>
        private void CreateNormalMap()
        {
            try
            {
                Bitmap TempImageX = _FilterX.Image;
                Bitmap TempImageY = _FilterY.Image;
                _Image.Dispose();
                _Image = new Bitmap(TempImageX.Width, TempImageX.Height);
                Math.Vector3 TempVector = new Utilities.Math.Vector3(0.0, 0.0, 0.0);
                for (int y = 0; y < TempImageX.Height; ++y)
                {
                    for (int x = 0; x < TempImageX.Width; ++x)
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
                        _Image.SetPixel(x, y, Color.FromArgb((int)TempVector.X, (int)TempVector.Y, (int)TempVector.Z));
                    }
                }
            }
            catch (Exception e) { throw e; }
        }

        /// <summary>
        /// Sets up the edge detection filter
        /// </summary>
        private void CreateFilter()
        {
            try
            {
                _FilterX = new BumpMap(_Image);
                _FilterY = new BumpMap(_Image);
                _FilterX.Invert = InvertX;
                _FilterY.Invert = InvertY;
                _FilterX.Direction = Direction.LeftRight;
                _FilterY.Direction = Direction.TopBottom;
            }
            catch (Exception e) { throw e; }
        }
        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            if (_FilterX != null)
            {
                _FilterX.Dispose();
            }
            if (_FilterY != null)
            {
                _FilterY.Dispose();
            }
            if (_Image != null)
            {
                _Image.Dispose();
            }
        }

        #endregion
    }
}
