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

using System.Linq;
using System.Text;
using System.Web.Mvc;
using Utilities.DataTypes;
using Utilities.IO;
using Utilities.IO.FileSystem.Interfaces;
using Utilities.IO.Logging.Enums;
using Utilities.Profiler;

namespace Ironman.Core.BaseClasses
{
    /// <summary>
    /// Controller base class
    /// </summary>
    public abstract class ControllerBase : Controller
    {
        /// <summary>
        /// Constructor
        /// </summary>
        protected ControllerBase()
            : base()
        {
        }

        /// <summary>
        /// Encoding used for the controller (defaults to UTF8)
        /// </summary>
        protected Encoding Encoding { get; set; }

        /// <summary>
        /// Gets a directory based on the path entered
        /// </summary>
        /// <param name="Path">Path to the directory</param>
        /// <returns>The directory pointed to by the path</returns>
        protected IDirectory DirectoryInfo(string Path)
        {
            return new Utilities.IO.DirectoryInfo(Path);
        }

        /// <summary>
        /// Gets a file based on the path entered
        /// </summary>
        /// <param name="Path">Path to the file</param>
        /// <returns>The file pointed to by the path</returns>
        protected FileInfo FileInfo(string Path)
        {
            return new Utilities.IO.FileInfo(Path);
        }

        /// <summary>
        /// Initializes the controller
        /// </summary>
        /// <param name="requestContext">Request context</param>
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            if (requestContext == null)
                return;
            requestContext.HttpContext.Response.ContentEncoding = Encoding.Check(new UTF8Encoding());
            base.Initialize(requestContext);
        }

        /// <summary>
        /// Logs a message to the Ironman log file
        /// </summary>
        /// <param name="Message">Message to log</param>
        /// <param name="Type">Message level/type</param>
        /// <param name="args">Extra args used to format the message</param>
        /// <returns>this</returns>
        protected ControllerBase Log(string Message, MessageType Type, params object[] args)
        {
            Utilities.IO.Log.Get().LogMessage(Message, Type, args);
            return this;
        }

        /// <summary>
        /// Serializes the object into an action result based on the content type requested by the
        /// user (defaults to json)
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="Object">Object to serialize</param>
        /// <param name="ContentType">Content type to serialize it as</param>
        /// <returns>Action result</returns>
        protected virtual ActionResult Serialize<T>(T Object, string ContentType = "")
        {
            Utilities.IO.Serializers.Manager Manager = Utilities.IoC.Manager.Bootstrapper == null ? null : Utilities.IoC.Manager.Bootstrapper.Resolve<Utilities.IO.Serializers.Manager>();
            var Result = new ContentResult();
            if (Manager == null)
                return Result;
            if (string.IsNullOrEmpty(ContentType))
            {
                if (Request.AcceptTypes != null)
                    ContentType = Request.AcceptTypes.Length > 0 ? Request.AcceptTypes.FirstOrDefault(Manager.CanSerialize) : "";
                if (string.IsNullOrEmpty(ContentType) && Request.Path.Contains('.'))
                    ContentType = Manager.FileTypeToContentType(Request.Path.ToUpperInvariant().Right((Request.Path.Length - Request.Path.LastIndexOf('.'))));
                if (string.IsNullOrEmpty(ContentType))
                    ContentType = "application/json";
            }
            Result.Content = Object.Serialize<string, T>(ContentType);
            Result.ContentType = ContentType;
            return Result;
        }

        /// <summary>
        /// Starts profiling the section starting with this call and stopping when the profiler is
        /// disposed of
        /// </summary>
        /// <param name="Name">Name of the section to profile</param>
        /// <returns>The profiler object</returns>
        protected Profiler StartProfiling(string Name)
        {
            return new Utilities.Profiler.Profiler(Name);
        }
    }
}