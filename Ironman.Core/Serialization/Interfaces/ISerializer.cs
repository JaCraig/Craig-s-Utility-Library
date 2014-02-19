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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Batman.Core.Bootstrapper.Interfaces;

using Utilities.DataTypes.ExtensionMethods;
using Batman.Core.Logging.BaseClasses;
using Utilities.IO.Logging.Enums;
using Batman.Core.Logging;
using System.IO;
using Batman.Core.Tasks;
using Batman.Core.Tasks.Enums;
using Batman.Core.FileSystem;
using Batman.Core.Communication;
using Batman.Core.Profiling.Interfaces;
using System.Web.Mvc;
#endregion

namespace Batman.Core.Serialization.Interfaces
{
    /// <summary>
    /// Serializer interface
    /// </summary>
    public interface ISerializer
    {
        /// <summary>
        /// Content type associated with this serializer
        /// </summary>
        string ContentType { get; }

        /// <summary>
        /// File ending associated with the serializer
        /// </summary>
        string FileType { get; }

        /// <summary>
        /// Name of the serializer
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Serializes the data to a string
        /// </summary>
        /// <param name="Data">Data to serialize</param>
        /// <param name="ObjectType">Type of the data</param>
        /// <returns>The serialized data</returns>
        ActionResult Serialize(Type ObjectType, object Data);

        /// <summary>
        /// Serializes the data to a string
        /// </summary>
        /// <typeparam name="T">Type of the data to serialize</typeparam>
        /// <param name="Data">Data to serialize</param>
        /// <returns>The serialized data</returns>
        ActionResult Serialize<T>(T Data);
    }
}
