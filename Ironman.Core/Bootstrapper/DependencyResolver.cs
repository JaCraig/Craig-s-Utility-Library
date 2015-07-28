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
using System.Web.Mvc;
using Utilities.IoC.Interfaces;

namespace Ironman.Core.Bootstrapper
{
    /// <summary>
    /// Dependency resolver base class
    /// </summary>
    public class DependencyResolver : IDependencyResolver
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DependencyResolver(IBootstrapper Container)
        {
            if (Container == null)
                throw new ArgumentNullException(nameof(Container));
            this.Container = Container;
        }

        /// <summary>
        /// The IoC container
        /// </summary>
        protected IBootstrapper Container { get; private set; }

        /// <summary>
        /// Gets the service based on the type specified
        /// </summary>
        /// <param name="serviceType">Service type</param>
        /// <returns>The object associated with the type</returns>
        public object GetService(Type serviceType)
        {
            return Container.Resolve(serviceType, "", null);
        }

        /// <summary>
        /// Gets the services based on the type specified
        /// </summary>
        /// <param name="serviceType">Service type</param>
        /// <returns>The objects associated with the type</returns>
        public IEnumerable<object> GetServices(Type serviceType)
        {
            return Container.ResolveAll(serviceType);
        }
    }
}