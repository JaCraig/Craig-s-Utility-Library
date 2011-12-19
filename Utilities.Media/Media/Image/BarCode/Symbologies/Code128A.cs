using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities.Media.Image.BarCode.Interfaces;

namespace Utilities.Media.Image.BarCode.Symbologies
{
    public class Code128A : ISymbology
    {
        public Code128A(string Data)
        { }

        string ISymbology.Encode()
        {
            throw new NotImplementedException();
        }

        string ISymbology.Input
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
