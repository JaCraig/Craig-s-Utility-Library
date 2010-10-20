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
    /// Used for creating bump maps
    /// </summary>
    public class BumpMap
    {
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public BumpMap()
        {
            Invert = false;
            Direction = Direction.TopBottom;
        }

        #endregion

        #region Protected Properties

        protected Filter EdgeDetectionFilter { get; set; }

        #endregion

        #region Public Properties

        /// <summary>
        /// Determines the direction of the bump map
        /// </summary>
        public bool Invert { get; set; }

        /// <summary>
        /// Determines the direction of the bump map
        /// </summary>
        public Direction Direction { get; set; }

        #endregion

        #region Protected Functions

        /// <summary>
        /// Sets up the edge detection filter
        /// </summary>
        protected void CreateFilter()
        {
            EdgeDetectionFilter = new Filter(3, 3);
            if (Direction == Direction.TopBottom)
            {
                if (!Invert)
                {
                    EdgeDetectionFilter.MyFilter[0, 0] = 1;
                    EdgeDetectionFilter.MyFilter[1, 0] = 2;
                    EdgeDetectionFilter.MyFilter[2, 0] = 1;
                    EdgeDetectionFilter.MyFilter[0, 1] = 0;
                    EdgeDetectionFilter.MyFilter[1, 1] = 0;
                    EdgeDetectionFilter.MyFilter[2, 1] = 0;
                    EdgeDetectionFilter.MyFilter[0, 2] = -1;
                    EdgeDetectionFilter.MyFilter[1, 2] = -2;
                    EdgeDetectionFilter.MyFilter[2, 2] = -1;
                }
                else
                {
                    EdgeDetectionFilter.MyFilter[0, 0] = -1;
                    EdgeDetectionFilter.MyFilter[1, 0] = -2;
                    EdgeDetectionFilter.MyFilter[2, 0] = -1;
                    EdgeDetectionFilter.MyFilter[0, 1] = 0;
                    EdgeDetectionFilter.MyFilter[1, 1] = 0;
                    EdgeDetectionFilter.MyFilter[2, 1] = 0;
                    EdgeDetectionFilter.MyFilter[0, 2] = 1;
                    EdgeDetectionFilter.MyFilter[1, 2] = 2;
                    EdgeDetectionFilter.MyFilter[2, 2] = 1;
                }
            }
            else
            {
                if (!Invert)
                {
                    EdgeDetectionFilter.MyFilter[0, 0] = -1;
                    EdgeDetectionFilter.MyFilter[0, 1] = -2;
                    EdgeDetectionFilter.MyFilter[0, 2] = -1;
                    EdgeDetectionFilter.MyFilter[1, 0] = 0;
                    EdgeDetectionFilter.MyFilter[1, 1] = 0;
                    EdgeDetectionFilter.MyFilter[1, 2] = 0;
                    EdgeDetectionFilter.MyFilter[2, 0] = 1;
                    EdgeDetectionFilter.MyFilter[2, 1] = 2;
                    EdgeDetectionFilter.MyFilter[2, 2] = 1;
                }
                else
                {
                    EdgeDetectionFilter.MyFilter[0, 0] = 1;
                    EdgeDetectionFilter.MyFilter[0, 1] = 2;
                    EdgeDetectionFilter.MyFilter[0, 2] = 1;
                    EdgeDetectionFilter.MyFilter[1, 0] = 0;
                    EdgeDetectionFilter.MyFilter[1, 1] = 0;
                    EdgeDetectionFilter.MyFilter[1, 2] = 0;
                    EdgeDetectionFilter.MyFilter[2, 0] = -1;
                    EdgeDetectionFilter.MyFilter[2, 1] = -2;
                    EdgeDetectionFilter.MyFilter[2, 2] = -1;
                }
            }
            EdgeDetectionFilter.Offset = 127;
        }

        #endregion

        #region Public Functions

        /// <summary>
        /// Creates the bump map
        /// </summary>
        public Bitmap Create(Bitmap Image)
        {
            CreateFilter();
            using (Bitmap TempImage = EdgeDetectionFilter.ApplyFilter(Image))
            {
                return Media.Image.Image.ConvertBlackAndWhite(TempImage);
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