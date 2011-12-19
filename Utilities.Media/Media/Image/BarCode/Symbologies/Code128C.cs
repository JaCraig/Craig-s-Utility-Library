using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities.Media.Image.BarCode.Interfaces;

namespace Utilities.Media.Image.BarCode.Symbologies
{
    public class Code128C : ISymbology
    {
        public Code128C(string Data)
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
