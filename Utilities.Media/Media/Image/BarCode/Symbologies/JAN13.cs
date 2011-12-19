using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities.Media.Image.BarCode.Interfaces;

namespace Utilities.Media.Image.BarCode.Symbologies
{
    public class JAN13 : ISymbology
    {
        public JAN13(string Data)
        { }

        public string Encode()
        {
            throw new NotImplementedException();
        }

        public string Input
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
/*class JAN13 : BarcodeCommon, IBarcode
    {
        public JAN13(string input)
        {
            Raw_Data = input;
        }
        /// <summary>
        /// Encode the raw data using the JAN-13 algorithm.
        /// </summary>
        private string Encode_JAN13()
        {
            if (!Raw_Data.StartsWith("49")) Error("EJAN13-1: Invalid Country Code for JAN13 (49 required)");
            if (!BarcodeLib.Barcode.CheckNumericOnly(Raw_Data))
                Error("EJAN13-2: Numeric Data Only");

            EAN13 ean13 = new EAN13(Raw_Data);
            return ean13.Encoded_Value;
        }//Encode_JAN13

        #region IBarcode Members

        public string Encoded_Value
        {
            get { return Encode_JAN13(); }
        }

        #endregion
    }*/