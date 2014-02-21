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
using Glimpse.AspNet.Extensibility;
using Glimpse.Core.Extensibility;

#endregion Usings

namespace Glimpse.CUL
{
    /// <summary>
    /// Plugin that displays info from Batman
    /// </summary>
    public class Plugin : AspNetTab, IDocumentation
    {
        /// <summary>
        /// Documentation URL
        /// </summary>
        public string DocumentationUri { get { return "http://www.gutgames.com"; } }

        /// <summary>
        /// Name of the plugin
        /// </summary>
        public override string Name { get { return "Craig's Utility Library"; } }

        public override object GetData(ITabContext context)
        {
            Dictionary<string, string[]> Return = new Dictionary<string, string[]>();
            Return.Add("IoC Container", new string[] { Utilities.IoC.Manager.Bootstrapper.Name });
            Return.Add("File systems", Utilities.IoC.Manager.Bootstrapper.Resolve<Utilities.IO.FileSystem.Manager>().ToString().Split(new string[] { System.Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries));
            Return.Add("Configuration systems", Utilities.IoC.Manager.Bootstrapper.Resolve<Utilities.Configuration.Manager.Manager>().ToString().Split(new string[] { System.Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries));
            Return.Add("Logging systems", Utilities.IoC.Manager.Bootstrapper.Resolve<Utilities.IO.Logging.Manager>().ToString().Split(new string[] { System.Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries));
            Return.Add("Serializers", Utilities.IoC.Manager.Bootstrapper.Resolve<Utilities.IO.Serializers.Manager>().ToString().Split(new string[] { System.Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries));
            return Return;
        }
    }
}