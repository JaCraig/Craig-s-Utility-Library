/*
Copyright (c) 2013 <a href="http://www.gutgames.com">James Craig</a>

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
using Batman.Core.Bootstrapper.Interfaces;
using System.Web.Mvc;
using System.Collections.Generic;
using Batman.MVC.Assets.Interfaces;
using Batman.MVC.Assets.Enums;
using Batman.Core.FileSystem;
using System.Web.Optimization;
using Utilities.DataTypes.ExtensionMethods;
using System.Linq;
using System.IO;
using Utilities.IO.ExtensionMethods;
using Batman.Core;
#endregion

namespace Batman.MVC.Assets.Transformers
{
    /// <summary>
    /// Transformer class used
    /// </summary>
    public class Transformer : IBundleTransform
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public Transformer()
        {
            Manager = BatComputer.Bootstrapper.Resolve<AssetManager>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Manager that loads basic asset stuff
        /// </summary>
        protected AssetManager Manager { get; private set; }

        #endregion

        #region Functions

        public void Process(BundleContext context, BundleResponse response)
        {
            if (!context.EnableInstrumentation)
            {
                Manager.Process(response.Files.Select(x => new Asset(x.IncludedVirtualPath)).ToList<IAsset>(), context, response);
            }
        }

        #endregion
    }
}