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
    /// Used for creating bump maps
    /// </summary>
    public class BumpMap:IDisposable
    {
        #region Private Variables
        private System.Drawing.Bitmap _Image=null;
        private Filter _EdgeDetectionFilter=null;
        private bool _Invert = false;
        private Direction _Direction=Direction.TopBottom;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        public BumpMap()
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
        public BumpMap(string FileName)
        {
            try
            {
                _Image = (Bitmap)System.Drawing.Bitmap.FromFile(FileName);
                CreateFilter();
                CreateBumpMap();
            }
            catch (Exception e) { throw e; }
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Image">Image to use</param>
        public BumpMap(System.Drawing.Bitmap Image)
        {
            try
            {
                _Image = new Bitmap(Image);
                CreateFilter();
                CreateBumpMap();
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
                    if (_Image != null)
                    {
                        _Image.Dispose();
                    }
                    _Image = new Bitmap(value);
                    CreateBumpMap();
                }
                catch (Exception e) { throw e; }
            }
        }

        /// <summary>
        /// Determines the direction of the bump map
        /// </summary>
        public bool Invert
        {
            get { return _Invert; }
            set
            {
                try
                {
                    if (_Invert != value)
                    {
                        _Invert = value;
                        CreateFilter();
                        CreateBumpMap();
                    }
                }
                catch (Exception e) { throw e; }
            }
        }

        /// <summary>
        /// Determines the direction of the bump map
        /// </summary>
        public Direction Direction
        {
            get { return _Direction; }
            set
            {
                try
                {
                    if (_Direction != value)
                    {
                        _Direction = value;
                        CreateFilter();
                        CreateBumpMap();
                    }
                }
                catch (Exception e) { throw e; }
            }
        }
        #endregion

        #region Private Functions
        /// <summary>
        /// Creates the bump map
        /// </summary>
        private void CreateBumpMap()
        {
            try
            {
                if (_Image != null)
                {
                    Bitmap TempImage = _EdgeDetectionFilter.ApplyFilter(_Image);
                    Bitmap TempImage2 = Media.Image.Image.ConvertBlackAndWhite(TempImage);
                    TempImage.Dispose();
                    _Image.Dispose();
                    _Image = TempImage2;
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
                _EdgeDetectionFilter = new Filter(3, 3);
                if (_Direction == Direction.TopBottom)
                {
                    if (!_Invert)
                    {
                        _EdgeDetectionFilter.MyFilter[0, 0] = 1;
                        _EdgeDetectionFilter.MyFilter[1, 0] = 2;
                        _EdgeDetectionFilter.MyFilter[2, 0] = 1;
                        _EdgeDetectionFilter.MyFilter[0, 1] = 0;
                        _EdgeDetectionFilter.MyFilter[1, 1] = 0;
                        _EdgeDetectionFilter.MyFilter[2, 1] = 0;
                        _EdgeDetectionFilter.MyFilter[0, 2] = -1;
                        _EdgeDetectionFilter.MyFilter[1, 2] = -2;
                        _EdgeDetectionFilter.MyFilter[2, 2] = -1;
                    }
                    else
                    {
                        _EdgeDetectionFilter.MyFilter[0, 0] = -1;
                        _EdgeDetectionFilter.MyFilter[1, 0] = -2;
                        _EdgeDetectionFilter.MyFilter[2, 0] = -1;
                        _EdgeDetectionFilter.MyFilter[0, 1] = 0;
                        _EdgeDetectionFilter.MyFilter[1, 1] = 0;
                        _EdgeDetectionFilter.MyFilter[2, 1] = 0;
                        _EdgeDetectionFilter.MyFilter[0, 2] = 1;
                        _EdgeDetectionFilter.MyFilter[1, 2] = 2;
                        _EdgeDetectionFilter.MyFilter[2, 2] = 1;
                    }
                }
                else
                {
                    if (!_Invert)
                    {
                        _EdgeDetectionFilter.MyFilter[0, 0] = -1;
                        _EdgeDetectionFilter.MyFilter[0, 1] = -2;
                        _EdgeDetectionFilter.MyFilter[0, 2] = -1;
                        _EdgeDetectionFilter.MyFilter[1, 0] = 0;
                        _EdgeDetectionFilter.MyFilter[1, 1] = 0;
                        _EdgeDetectionFilter.MyFilter[1, 2] = 0;
                        _EdgeDetectionFilter.MyFilter[2, 0] = 1;
                        _EdgeDetectionFilter.MyFilter[2, 1] = 2;
                        _EdgeDetectionFilter.MyFilter[2, 2] = 1;
                    }
                    else
                    {
                        _EdgeDetectionFilter.MyFilter[0, 0] = 1;
                        _EdgeDetectionFilter.MyFilter[0, 1] = 2;
                        _EdgeDetectionFilter.MyFilter[0, 2] = 1;
                        _EdgeDetectionFilter.MyFilter[1, 0] = 0;
                        _EdgeDetectionFilter.MyFilter[1, 1] = 0;
                        _EdgeDetectionFilter.MyFilter[1, 2] = 0;
                        _EdgeDetectionFilter.MyFilter[2, 0] = -1;
                        _EdgeDetectionFilter.MyFilter[2, 1] = -2;
                        _EdgeDetectionFilter.MyFilter[2, 2] = -1;
                    }
                }
                _EdgeDetectionFilter.Offset = 127;
            }
            catch (Exception e) { throw e; }
        }
        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            if (_Image != null)
            {
                _Image.Dispose();
            }
        }

        #endregion
    }

    #region Enum
    /// <summary>
    /// Direction
    /// </summary>
    public enum Direction
    {
        TopBottom = 0,
        LeftRight
    };
    #endregion
}
