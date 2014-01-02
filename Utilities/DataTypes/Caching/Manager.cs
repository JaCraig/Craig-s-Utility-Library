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
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Utilities.DataTypes.Caching.Default;
using Utilities.DataTypes.Caching.Interfaces;
using Utilities.DataTypes.Patterns.BaseClasses;
#endregion

namespace Utilities.DataTypes.Caching
{
    /// <summary>
    /// Caching manager class
    /// </summary>
    public class Manager : SafeDisposableBaseClass
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public Manager()
        {
            Caches = AppDomain.CurrentDomain.GetAssemblies()
                                            .Types<ICache>()
                                            .Where(x => !x.Namespace.StartsWith("UTILITIES", StringComparison.OrdinalIgnoreCase))
                                            .Create<ICache>()
                                            .ToDictionary(x => x.Name);
            if (!Caches.ContainsKey("Default"))
                Caches.Add("Default", new Cache());
            if (HttpContext.Current != null)
            {
                if (!Caches.ContainsKey("Cache"))
                    Caches.Add("Cache", new CacheCache());
                if (!Caches.ContainsKey("Session"))
                    Caches.Add("Session", new SessionCache());
                if (!Caches.ContainsKey("Item"))
                    Caches.Add("Item", new ItemCache());
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Caches
        /// </summary>
        protected IDictionary<string, ICache> Caches { get; private set; }

        #endregion

        #region Functions

        /// <summary>
        /// Gets the specified cache
        /// </summary>
        /// <param name="Name">Name of the cache (defaults to Default)</param>
        /// <returns>Returns the ICache specified if it exists, otherwise creates a default cache and associates it with the name</returns>
        public ICache Cache(string Name = "Default")
        {
            if (!Caches.ContainsKey(Name))
                Caches.Add(Name, new Cache());
            return Caches[Name];
        }

        /// <summary>
        /// Disposes of the object
        /// </summary>
        /// <param name="Managed">Determines if all objects should be disposed or just managed objects</param>
        protected override void Dispose(bool Managed)
        {
            if (Caches != null)
            {
                Caches.ForEach(x => x.Value.Dispose());
                Caches.Clear();
            }
        }

        #endregion
    }
}