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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities.Media.Image.BarCode.Interfaces;
using Utilities.Media.Image.BarCode.Symbologies;
using System.Drawing;
using System.Drawing.Drawing2D;
#endregion

namespace Utilities.Media.Image.BarCode
{
    /// <summary>
    /// Class for generating bar codes
    /// </summary>
    public class BarCode
    {
        #region Public Static Functions

        /// <summary>
        /// Encodes and writes a bar code to an image
        /// </summary>
        /// <param name="Data">Data string to encode</param>
        /// <param name="Width">Desired width of the image</param>
        /// <param name="Height">Desired height of the image</param>
        /// <param name="EncodingType">Encoding type to use</param>
        /// <returns>An image of a bar code corresponding to the data</returns>
        public static Bitmap Encode(string Data, int Width, int Height, Enums.Type EncodingType)
        {
            if (string.IsNullOrEmpty(Data))
                throw new ArgumentNullException("Data can't be empty/null");

            string EncodedData = GetSymbology(Data, EncodingType);
            return GenerateImage(EncodingType, Width, Height, EncodedData);
        }

        #endregion

        #region Private Static Functions

        private static Bitmap GenerateImage(Enums.Type EncodingType, int Width, int Height,string EncodedValue)
        {
            Bitmap ReturnValue = new Bitmap(Width, Height);
            int BarWidth = 0;
            int ShiftAdjustment = 0;

            if (EncodingType == Enums.Type.ITF14)
            {
                int BearerWidth = (int)(Width / 12.05);
                int QuietZone = Convert.ToInt32(Width * 0.05);
                BarWidth = (Width - (BearerWidth * 2) - (QuietZone * 2)) / EncodedValue.Length;
                ShiftAdjustment = ((Width - (BearerWidth * 2) - (QuietZone * 2)) % EncodedValue.Length) / 2;

                if (BarWidth <= 0 || QuietZone <= 0)
                    throw new Exception("Image size is too small");

                using(Graphics Graphics=Graphics.FromImage(ReturnValue))
                {
                    Graphics.Clear(Color.White);
                    using (Pen Pen = new Pen(Color.Black, BarWidth))
                    {
                        Pen.Alignment = PenAlignment.Right;
                        for(int x=0;x<EncodedValue.Length;++x)
                        {
                            if (EncodedValue[x] == '1')
                                Graphics.DrawLine(Pen,
                                    new Point((x * BarWidth) + ShiftAdjustment + BearerWidth + QuietZone, 0),
                                    new Point((x * BarWidth) + ShiftAdjustment + BearerWidth + QuietZone, Height));
                        }
                        Pen.Width = (float)(Height / 8);
                        Pen.Color = Color.Black;
                        Pen.Alignment = PenAlignment.Center;
                        Graphics.DrawLine(Pen, new Point(0, 0), new Point(Width, 0));
                        Graphics.DrawLine(Pen, new Point(0, Height), new Point(Width, Height));
                        Graphics.DrawLine(Pen, new Point(0, 0), new Point(0, Height));
                        Graphics.DrawLine(Pen, new Point(Width, 0), new Point(Width, Height));
                    }
                }
                return ReturnValue;
            }

            BarWidth = Width / EncodedValue.Length;
            int BarWidthModifier = 1;

            if (EncodingType == Enums.Type.PostNet)
                BarWidthModifier = 2;

            ShiftAdjustment = (Width % EncodedValue.Length) / 2;

            if (BarWidth <= 0)
                throw new Exception("Image size is too small");

            using (Graphics Graphics = Graphics.FromImage(ReturnValue))
            {
                Graphics.Clear(Color.White);
                using (Pen Background = new Pen(Color.White, BarWidth / BarWidthModifier))
                {
                    using (Pen Foreground = new Pen(Color.Black, BarWidth / BarWidthModifier))
                    {
                        for(int x=0;x<EncodedValue.Length;++x)
                        {
                            if (EncodingType == Enums.Type.PostNet)
                            {
                                if (EncodedValue[x] != '1')
                                    Graphics.DrawLine(Foreground,
                                        new Point(x * BarWidth + ShiftAdjustment + 1, Height),
                                        new Point(x * BarWidth + ShiftAdjustment + 1, Height / 2));
                                Graphics.DrawLine(Background,
                                    new Point(x * (BarWidth * BarWidthModifier) + ShiftAdjustment + BarWidth + 1, 0),
                                    new Point(x * (BarWidth * BarWidthModifier) + ShiftAdjustment + BarWidth + 1, Height));
                            }

                            if (EncodedValue[x] == '1')
                                Graphics.DrawLine(Foreground,
                                    new Point(x * BarWidth + ShiftAdjustment + 1, 0),
                                    new Point(x * BarWidth + ShiftAdjustment + 1, Height));
                        }
                    }
                }
            }
            return ReturnValue;
        }

        private static string GetSymbology(string Data, Enums.Type EncodingType)
        {
            ISymbology SymbologyUsing = null;
            switch (EncodingType)
            {
                case Enums.Type.UCC12:
                case Enums.Type.UPCA:
                    SymbologyUsing = new UPCA(Data);
                    break;
                case Enums.Type.UCC13:
                case Enums.Type.EAN13:
                    SymbologyUsing = new EAN13(Data);
                    break;
                case Enums.Type.Interleaved2of5:
                    SymbologyUsing = new Interleaved2of5(Data);
                    break;
                case Enums.Type.Industrial2of5:
                case Enums.Type.Standard2of5:
                    SymbologyUsing = new Standard2of5(Data);
                    break;
                case Enums.Type.LOGMARS:
                case Enums.Type.CODE39:
                    SymbologyUsing = new Code39(Data);
                    break;
                case Enums.Type.CODE39Extended:
                    SymbologyUsing = new Code39(Data);
                    break;
                case Enums.Type.Codabar:
                    SymbologyUsing = new Codabar(Data);
                    break;
                case Enums.Type.PostNet:
                    SymbologyUsing = new Postnet(Data);
                    break;
                case Enums.Type.ISBN:
                case Enums.Type.BOOKLAND:
                    SymbologyUsing = new ISBN(Data);
                    break;
                case Enums.Type.JAN13:
                    SymbologyUsing = new JAN13(Data);
                    break;
                case Enums.Type.UPC_SUPPLEMENTAL_2DIGIT:
                    SymbologyUsing = new UPCSupplement2(Data);
                    break;
                case Enums.Type.MSI_Mod10:
                case Enums.Type.MSI_2Mod10:
                case Enums.Type.MSI_Mod11:
                case Enums.Type.MSI_Mod11_Mod10:
                case Enums.Type.Modified_Plessey:
                    SymbologyUsing = new MSI(Data);
                    break;
                case Enums.Type.UPC_SUPPLEMENTAL_5DIGIT:
                    SymbologyUsing = new UPCSupplement5(Data);
                    break;
                case Enums.Type.UPCE:
                    SymbologyUsing = new UPCE(Data);
                    break;
                case Enums.Type.EAN8:
                    SymbologyUsing = new EAN8(Data);
                    break;
                case Enums.Type.USD8:
                case Enums.Type.CODE11:
                    SymbologyUsing = new Code11(Data);
                    break;
                case Enums.Type.CODE128:
                    SymbologyUsing = new Code128(Data);
                    break;
                case Enums.Type.CODE128A:
                    SymbologyUsing = new Code128A(Data);
                    break;
                case Enums.Type.CODE128B:
                    SymbologyUsing = new Code128B(Data);
                    break;
                case Enums.Type.CODE128C:
                    SymbologyUsing = new Code128C(Data);
                    break;
                case Enums.Type.ITF14:
                    SymbologyUsing = new ITF14(Data);
                    break;
                case Enums.Type.CODE93:
                    SymbologyUsing = new Code93(Data);
                    break;
                case Enums.Type.TELEPEN:
                    SymbologyUsing = new Telepen(Data);
                    break;
            }
            return SymbologyUsing.Encode();
        }

        #endregion

        #region Properties

        /// <summary>
        /// bar code encoding
        /// </summary>
        protected Enums.Type Encoding { get; set; }

        /// <summary>
        /// Symbology using
        /// </summary>
        protected ISymbology Symbology { get; set; }

        /// <summary>
        /// The string to encode
        /// </summary>
        protected string Data { get; set; }

        #endregion
    }
}