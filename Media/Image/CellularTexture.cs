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
    /// Cellular texture helper
    /// </summary>
    public static class CellularTexture
    {
        #region Functions
        
        /// <summary>
        /// Generates a cellular texture image
        /// </summary>
        /// <param name="Width">Width</param>
        /// <param name="Height">Height</param>
        /// <param name="NumberOfPoints">Number of points</param>
        /// <param name="Seed">Random seed</param>
        /// <returns>Returns an image of a cellular texture</returns>
        public static Bitmap Generate(int Width, int Height, int NumberOfPoints, int Seed)
        {
            float[,] DistanceBuffer = new float[Width, Height];
            Points[] PointArray = new Points[NumberOfPoints];
            float MinimumDistance = float.MaxValue;
            float MaxDistance = float.MinValue;
            System.Random Generator = new System.Random(Seed);
            for (int x = 0; x < NumberOfPoints; ++x)
            {
                PointArray[x].X = Generator.Next(0, Width);
                PointArray[x].Y = Generator.Next(0, Height);
            }
            for (int y = 0; y < Height; ++y)
            {
                for (int x = 0; x < Width; ++x)
                {
                    DistanceBuffer[x, y] = DistanceNearestPoint(x, y, PointArray);
                    if (DistanceBuffer[x, y] > MaxDistance)
                        MaxDistance = DistanceBuffer[x, y];
                    else if (DistanceBuffer[x, y] < MinimumDistance)
                        MinimumDistance = DistanceBuffer[x, y];
                }
            }
            Bitmap ReturnValue = new Bitmap(Width, Height);
            BitmapData ImageData = Image.LockImage(ReturnValue);
            int ImagePixelSize = Image.GetPixelSize(ImageData);
            for (int x = 0; x < Width; ++x)
            {
                for (int y = 0; y < Height; ++y)
                {
                    float Value = GetHeight(x, y, DistanceBuffer, MinimumDistance, MaxDistance);
                    Value *= 255;
                    int RGBValue = Math.MathHelper.Clamp((int)Value, 255, 0);
                    Image.SetPixel(ImageData, x, y, Color.FromArgb(RGBValue, RGBValue, RGBValue), ImagePixelSize);
                }
            }
            Image.UnlockImage(ReturnValue, ImageData);
            return ReturnValue;
        }

        private static float GetHeight(float X, float Y, float[,] DistanceBuffer,
            float MinimumDistance,float MaxDistance)
        {
            return (DistanceBuffer[(int)X, (int)Y] - MinimumDistance) / (MaxDistance - MinimumDistance);
        }

        private static float DistanceNearestPoint(int x, int y,Points[] PointArray)
        {
            float Lowest = float.MaxValue;
            for (int z = 0; z < PointArray.Length; ++z)
            {
                float Distance = (float)System.Math.Sqrt(((PointArray[z].X - x) * (PointArray[z].X - x)) + ((PointArray[z].Y - y) * (PointArray[z].Y - y)));
                if (Distance < Lowest)
                {
                    Lowest = Distance;
                }
            }
            return Lowest;
        }

        #endregion
    }

    #region Structs

    internal struct Points
    {
        public float X;
        public float Y;
    }

    #endregion
}