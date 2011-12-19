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
#endregion

namespace Utilities.Media.Image.BarCode.Enums
{
    /// <summary>
    /// Symbology type
    /// </summary>
    public enum Type
    {
        #region Values

        UPCA,
        UPCE,
        UPC_SUPPLEMENTAL_2DIGIT,
        UPC_SUPPLEMENTAL_5DIGIT,
        EAN13,
        EAN8,
        Interleaved2of5,
        Standard2of5,
        Industrial2of5,
        CODE39,
        CODE39Extended,
        Codabar,
        PostNet,
        BOOKLAND,
        ISBN,
        JAN13,
        MSI_Mod10,
        MSI_2Mod10,
        MSI_Mod11,
        MSI_Mod11_Mod10,
        Modified_Plessey,
        CODE11,
        USD8,
        UCC12,
        UCC13,
        LOGMARS,
        CODE128,
        CODE128A,
        CODE128B,
        CODE128C,
        ITF14,
        CODE93,
        TELEPEN

        #endregion
    }
}
