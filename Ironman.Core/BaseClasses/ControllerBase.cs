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
using Utilities.DataTypes.ExtensionMethods;
using System.Linq;
using System.Web.Optimization;
using System.IO;
using Batman.MVC.Assets.Utils;

using Batman.Core.FileSystem.Interfaces;
using System.Web;
using Utilities.DataTypes;
using Batman.Core.Tasks.Interfaces;
using Batman.MVC.Assets;
using Batman.Core;
using System.Configuration;
using Batman.Core.Communication;
using Batman.Core.Communication.Interfaces;
using Batman.Core.Logging.BaseClasses;
using Utilities.IO.Logging.Enums;
using Batman.Core.Profiling.Interfaces;
using Batman.Core.Serialization;
using System.Text;
#endregion

namespace Batman.MVC.BaseClasses
{
    /// <summary>
    /// Controller base class
    /// </summary>
    public abstract class ControllerBase : Controller
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        protected ControllerBase()
            : base()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Encoding used for the controller (defaults to UTF8)
        /// </summary>
        protected Encoding Encoding { get; set; }

        #endregion

        #region Functions

        /// <summary>
        /// Initializes the controller
        /// </summary>
        /// <param name="requestContext">Request context</param>
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            requestContext.HttpContext.Response.ContentEncoding = Encoding.Check(new UTF8Encoding());
            base.Initialize(requestContext);
        }

        /// <summary>
        /// Creates a message object
        /// </summary>
        /// <typeparam name="T">Message type</typeparam>
        /// <returns>The created message object</returns>
        protected T CreateMessage<T>()
            where T : class,IMessage
        {
            return DependencyResolver.Current.GetService<CommunicationManager>()[typeof(T)].CreateMessage() as T;
        }

        /// <summary>
        /// Gets a file based on the path entered
        /// </summary>
        /// <param name="Path">Path to the file</param>
        /// <returns>The file pointed to by the path</returns>
        protected IFile FileInfo(string Path)
        {
            return DependencyResolver.Current.GetService<FileManager>().File(Path);
        }

        /// <summary>
        /// Gets a directory based on the path entered
        /// </summary>
        /// <param name="Path">Path to the directory</param>
        /// <returns>The directory pointed to by the path</returns>
        protected IDirectory DirectoryInfo(string Path)
        {
            return DependencyResolver.Current.GetService<FileManager>().Directory(Path);
        }

        /// <summary>
        /// Logs a message to the Batman log file
        /// </summary>
        /// <param name="Message">Message to log</param>
        /// <param name="Type">Message level/type</param>
        /// <param name="args">Extra args used to format the message</param>
        /// <returns>this</returns>
        protected ControllerBase Log(string Message,MessageType Type,params object[] args)
        {
            DependencyResolver.Current.GetService<LogBase>().LogMessage(Message, Type, args);
            return this;
        }

        /// <summary>
        /// Starts profiling the section starting with this call and stopping when the profiler is disposed of
        /// </summary>
        /// <param name="Name">Name of the section to profile</param>
        /// <returns>The profiler object</returns>
        protected IProfiler StartProfiling(string Name)
        {
            return DependencyResolver.Current.GetService<IProfiler>().Step(Name);
        }

        /// <summary>
        /// Serializes the object into an action result
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="Object">Object to serialize</param>
        /// <param name="ContentType">Content type to serialize it as</param>
        /// <returns>Action result</returns>
        protected virtual ActionResult Serialize<T>(T Object, string ContentType)
        {
            return DependencyResolver.Current.GetService<SerializationManager>().Serialize(Object, ContentType);
        }

        /// <summary>
        /// Serializes the object into an action result based on the content type requested by the user (defaults to json)
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="Object">Object to serialize</param>
        /// <returns>Action result</returns>
        protected virtual ActionResult Serialize<T>(T Object)
        {
            SerializationManager Manager = DependencyResolver.Current.GetService<SerializationManager>();
            string ContentType = Request.AcceptTypes.Length > 0 ? Request.AcceptTypes.FirstOrDefault(x => Manager.Serializers.ContainsKey(x)) : "";
            if (string.IsNullOrEmpty(ContentType))
                ContentType = Manager.Serializers.FirstOrDefault(x => Request.Path.ToUpperInvariant().EndsWith(x.Value.FileType.ToUpperInvariant())).Chain(x => x.Key, "");
            if (string.IsNullOrEmpty(ContentType))
                ContentType = "application/json";
            return Manager.Serialize(Object, ContentType);
        }

        #endregion
    }
}