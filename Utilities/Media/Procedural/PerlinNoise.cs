/*
Copyright (c) 2014 <a href="http://www.gutgames.com">James Craig</a>

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

using System;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Drawing.Imaging;
using Utilities.DataTypes;

namespace Utilities.Media.Procedural
{
    /// <summary>
    /// Perlin noise helper class
    /// </summary>
    public static class PerlinNoise
    {
        /// <summary>
        /// Generates perlin noise
        /// </summary>
        /// <param name="Width">Width of the resulting image</param>
        /// <param name="Height">Height of the resulting image</param>
        /// <param name="MaxRGBValue">MaxRGBValue</param>
        /// <param name="MinRGBValue">MinRGBValue</param>
        /// <param name="Frequency">Frequency</param>
        /// <param name="Amplitude">Amplitude</param>
        /// <param name="Persistance">Persistance</param>
        /// <param name="Octaves">Octaves</param>
        /// <param name="Seed">Random seed</param>
        /// <returns>An image containing perlin noise</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        public static Bitmap Generate(int Width, int Height, int MaxRGBValue, int MinRGBValue,
            float Frequency, float Amplitude, float Persistance, int Octaves, int Seed)
        {
            Bitmap ReturnValue = new Bitmap(Width, Height);
            BitmapData ImageData = ReturnValue.LockImage();
            int ImagePixelSize = ImageData.GetPixelSize();
            float[,] Noise = GenerateNoise(Seed, Width, Height);
            for (int x = 0; x < Width; ++x)
            {
                for (int y = 0; y < Height; ++y)
                {
                    float Value = GetValue(x, y, Width, Height, Frequency, Amplitude, Persistance, Octaves, Noise);
                    Value = (Value * 0.5f) + 0.5f;
                    Value *= 255;
                    int RGBValue = ((int)Value).Clamp(MaxRGBValue, MinRGBValue);
                    ImageData.SetPixel(x, y, Color.FromArgb(RGBValue, RGBValue, RGBValue), ImagePixelSize);
                }
            }
            ReturnValue.UnlockImage(ImageData);
            return ReturnValue;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1814:PreferJaggedArraysOverMultidimensional", MessageId = "Body"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1814:PreferJaggedArraysOverMultidimensional", MessageId = "Return")]
        private static float[,] GenerateNoise(int Seed, int Width, int Height)
        {
            float[,] Noise = new float[Width, Height];
            System.Random RandomGenerator = new System.Random(Seed);
            for (int x = 0; x < Width; ++x)
            {
                for (int y = 0; y < Height; ++y)
                {
                    Noise[x, y] = ((float)(RandomGenerator.NextDouble()) - 0.5f) * 2.0f;
                }
            }
            return Noise;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1814:PreferJaggedArraysOverMultidimensional", MessageId = "4#")]
        private static float GetSmoothNoise(float X, float Y, int Width, int Height, float[,] Noise)
        {
            if (Noise == null)
                return 0.0f;
            float FractionX = X - (int)X;
            float FractionY = Y - (int)Y;
            int X1 = ((int)X + Width) % Width;
            int Y1 = ((int)Y + Height) % Height;
            int X2 = ((int)X + Width - 1) % Width;
            int Y2 = ((int)Y + Height - 1) % Height;

            float FinalValue = 0.0f;
            FinalValue += FractionX * FractionY * Noise[X1, Y1];
            FinalValue += FractionX * (1 - FractionY) * Noise[X1, Y2];
            FinalValue += (1 - FractionX) * FractionY * Noise[X2, Y1];
            FinalValue += (1 - FractionX) * (1 - FractionY) * Noise[X2, Y2];

            return FinalValue;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1814:PreferJaggedArraysOverMultidimensional", MessageId = "8#")]
        private static float GetValue(int X, int Y, int Width, int Height, float Frequency, float Amplitude,
            float Persistance, int Octaves, float[,] Noise)
        {
            Contract.Requires<ArgumentException>(Octaves >= 0, "Octaves should be greater than or equal to 0");
            if (Noise == null)
                return 0.0f;
            float FinalValue = 0.0f;
            for (int i = 0; i < Octaves; ++i)
            {
                FinalValue += GetSmoothNoise(X * Frequency, Y * Frequency, Width, Height, Noise) * Amplitude;
                Frequency *= 2.0f;
                Amplitude *= Persistance;
            }
            FinalValue = FinalValue.Clamp(1.0f, -1.0f);
            return FinalValue;
        }
    }
}