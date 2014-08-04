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
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ironman.Core.Serialization.BaseClasses;
using Utilities.DataTypes;
using Utilities.IO;

#endregion Usings

namespace Ironman.Core.Serialization
{
    /// <summary>
    /// General value provider
    /// </summary>
    public class GeneralValueProvider : VPFactoryBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GeneralValueProvider" /> class.
        /// </summary>
        /// <param name="Manager">The manager.</param>
        public GeneralValueProvider(Utilities.IO.Serializers.Manager Manager)
            : base()
        {
            this.Manager = Manager;
        }

        /// <summary>
        /// Gets or sets the manager.
        /// </summary>
        /// <value>The manager.</value>
        private Utilities.IO.Serializers.Manager Manager { get; set; }

        /// <summary>
        /// Adds the factory to the system
        /// </summary>
        public override void AddFactory()
        {
            JsonValueProviderFactory TempProvider = ValueProviderFactories.Factories.OfType<JsonValueProviderFactory>().FirstOrDefault();
            if (TempProvider != null)
                ValueProviderFactories.Factories.Remove(TempProvider);
            ValueProviderFactories.Factories.Add(this);
        }

        /// <summary>
        /// Gets the value provider
        /// </summary>
        /// <param name="controllerContext">Controller context</param>
        /// <returns>The value provider</returns>
        public override IValueProvider GetValueProvider(ControllerContext controllerContext)
        {
            if (controllerContext == null)
                throw new ArgumentNullException("controllerContext");
            HttpRequestBase Request = controllerContext.HttpContext.Request;
            if (string.IsNullOrEmpty(Request.ContentType))
                return null;
            if (!Manager.CanSerialize(Request.ContentType))
                return null;
            string Body = Request.InputStream.ReadAll();
            Request.InputStream.Seek(0, System.IO.SeekOrigin.Begin);
            return string.IsNullOrEmpty(Body) ?
                null :
                new DictionaryValueProvider<object>(Body.Deserialize<ExpandoObject, string>(Request.ContentType), CultureInfo.CurrentCulture);
        }
    }
}