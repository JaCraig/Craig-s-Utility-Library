/*
Copyright (c) 2012 <a href="http://www.gutgames.com">James Craig</a>

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
using System.Reflection;
#endregion

namespace Utilities.DataTypes.Patterns.BaseClasses
{
    /// <summary>
    /// Base class used for singletons
    /// </summary>
    /// <typeparam name="T">The class type</typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1053:StaticHolderTypesShouldNotHaveConstructors")]
    public class Singleton<T> where T : class
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        protected Singleton() { }

        #endregion

        #region Private Variables

        private static T _Instance = null;
        private static object Temp = 1;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the instance of the singleton
        /// </summary>
        public static T Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock (Temp)
                    {
                        if (_Instance == null)
                        {
                            ConstructorInfo Constructor = typeof(T).GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic,
                                null, new Type[0], null);
                            if (Constructor == null || Constructor.IsAssembly)
                                throw new InvalidOperationException("Constructor is not private or protected for type " + typeof(T).Name);
                            _Instance = (T)Constructor.Invoke(null);
                        }
                    }
                }
                return _Instance;
            }
        }

        #endregion
    }
}