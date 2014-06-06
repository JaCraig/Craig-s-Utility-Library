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

#region Usings

using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web.Optimization;
using Ironman.Core.Assets.Interfaces;
using Utilities.IoC.Interfaces;

#endregion Usings

namespace Ironman.Core.Assets.Transformers
{
    /// <summary>
    /// Transformer class used
    /// </summary>
    public class Transformer : IBundleTransform
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Manager">The manager.</param>
        public Transformer(AssetManager Manager)
        {
            Contract.Requires<ArgumentNullException>(Manager != null, "Manager");
            this.Manager = Manager;
        }

        /// <summary>
        /// Manager that loads basic asset stuff
        /// </summary>
        /// <value>The manager.</value>
        protected AssetManager Manager { get; private set; }

        /// <summary>
        /// Processes the bundle
        /// </summary>
        /// <param name="context">Bundle context</param>
        /// <param name="response">Bundle response</param>
        public void Process(BundleContext context, BundleResponse response)
        {
            if (context == null || response == null)
                return;
            if (!context.EnableInstrumentation)
            {
                Manager.Process(response.Files.Select(x => new Asset(x.IncludedVirtualPath, Manager)).ToList<IAsset>(), response);
            }
        }
    }
}