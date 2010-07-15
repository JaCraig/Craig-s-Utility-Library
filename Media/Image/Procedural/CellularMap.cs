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
using System.Collections.Generic;

#endregion

namespace Utilities.Media.Image.Procedural
{
    /// <summary>
    /// A cellular map creator
    /// </summary>
    public class CellularMap
    {
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Seed">Seed for random generation</param>
        /// <param name="Width">Width of the image</param>
        /// <param name="Height">Height of the image</param>
        /// <param name="NumberOfPoints">Number of cells</param>
        public CellularMap(int Seed, int Width, int Height,int NumberOfPoints)
        {
            _Width = Width;
            _Height = Height;
            Random.Random Rand = new Random.Random(Seed);
            Distances = new float[Width, Height];
            ClosestPoint = new int[Width, Height];
            for (int x = 0; x < NumberOfPoints; ++x)
            {
                Point TempPoint = new Point();
                TempPoint.X = Rand.Next(0, Width);
                TempPoint.Y = Rand.Next(0, Height);
                Points.Add(TempPoint);
            }
            CalculateDistances();
        }

        #endregion

        #region Private Functions

        /// <summary>
        /// Calculate the distance between the points
        /// </summary>
        private void CalculateDistances()
        {
            for (int x = 0; x < _Width; ++x)
            {
                for (int y = 0; y < _Height; ++y)
                {
                    FindClosestPoint(x, y);
                }
            }
        }

        /// <summary>
        /// Finds the closest cell center
        /// </summary>
        /// <param name="x">x axis</param>
        /// <param name="y">y axis</param>
        private void FindClosestPoint(int x, int y)
        {
            float MaxDistance=float.MaxValue;
            int Index = -1;
            for (int z = 0; z < Points.Count; ++z)
            {
                float Distance = (float)System.Math.Sqrt(((Points[z].X - x) * (Points[z].X - x)) + ((Points[z].Y - y) * (Points[z].Y - y)));
                if (Distance < MaxDistance)
                {
                    MaxDistance = Distance;
                    Index = z;
                }
            }
            ClosestPoint[x, y] = Index;
            Distances[x, y] = MaxDistance;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// List of closest cells
        /// </summary>
        public int[,] ClosestPoint { get; set; }

        /// <summary>
        /// Distances to the closest cell
        /// </summary>
        public float[,] Distances { get; set; }

        #endregion

        #region Private Properties
        private List<Point> Points = new List<Point>();
        private int _Width = 0;
        private int _Height = 0;
        #endregion
    }

    #region Internal Classes

    /// <summary>
    /// Individual point
    /// </summary>
    internal class Point
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public Point() { }

        #endregion

        #region Public Properties

        /// <summary>
        /// X axis
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Y axis
        /// </summary>
        public int Y { get; set; }

        #endregion
    }

    #endregion
}
