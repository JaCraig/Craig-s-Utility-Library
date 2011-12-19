using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities.Media.Image.BarCode.Interfaces;

namespace Utilities.Media.Image.BarCode.Symbologies
{
    public class Code128B : ISymbology
    {
        public Code128B(string Data)
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
