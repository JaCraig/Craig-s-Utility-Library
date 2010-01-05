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
using System.Text;
#endregion

namespace Utilities.Media.Image
{
    /// <summary>
    /// Used to create ASCII art
    /// </summary>
    public class ASCIIArt
    {
        #region Public Static Functions

        /// <summary>
        /// Converts an image to ASCII art
        /// </summary>
        /// <param name="Input">The image you wish to convert</param>
        /// <returns>A string containing the art</returns>
        public static string ConvertToASCII(Bitmap Input)
        {
            bool ShowLine = true;
            Bitmap TempImage = Image.ConvertBlackAndWhite(Input);
            StringBuilder Builder = new StringBuilder();
            for (int x = 0; x < TempImage.Height; ++x)
            {
                for (int y = 0; y < TempImage.Width; ++y)
                {
                    if (ShowLine)
                    {
                        Color CurrentPixel = TempImage.GetPixel(y, x);
                        Builder.Append(_ASCIICharacters[((CurrentPixel.R * _ASCIICharacters.Length)/255)]);
                    }

                }
                if (ShowLine)
                {
                    Builder.Append(System.Environment.NewLine);
                    ShowLine = false;
                }
                else
                {
                    ShowLine = true;
                }
            }
            TempImage.Dispose();
            return Builder.ToString();
        }

        #endregion

        #region Private Variables

        private static string[] _ASCIICharacters = { "#", "#", "@", "%", "=", "+", "*", ":", "-", ".", " " };

        #endregion
    }
}
