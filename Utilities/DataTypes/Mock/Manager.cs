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

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.DataTypes.Mock
{
    /// <summary>
    /// Mock manager
    /// </summary>
    public class Manager
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Manager" /> class.
        /// </summary>
        /// <param name="Mockers">The mockers.</param>
        public Manager(IEnumerable<IMocker> Mockers)
        {
            Contract.Requires<ArgumentNullException>(Mockers != null);
            Contract.Requires<ArgumentException>(Mockers.Count() > 0);
            Mocker = Mockers.FirstOrDefault();
        }

        /// <summary>
        /// Gets the mocker used.
        /// </summary>
        /// <value>
        /// The mocker.
        /// </value>
        public static IMocker Mocker { get; private set; }

        /// <summary>
        /// Creates a mock of the type specified
        /// </summary>
        /// <typeparam name="T">The type to mock</typeparam>
        /// <returns>The mock object of the type specified</returns>
        public static IMock<T> Mock<T>()
        {
            return Mocker.Mock<T>();
        }
    }
}