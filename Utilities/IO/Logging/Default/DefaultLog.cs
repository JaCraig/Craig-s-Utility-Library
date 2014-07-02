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
using System.Globalization;
using System.Web;
using Utilities.IO.Logging.BaseClasses;
using Utilities.IO.Logging.Enums;

namespace Utilities.IO.Logging.Default
{
    /// <summary>
    /// Outputs messages to a file in ~/App_Data/Logs/ if a web app or ~/Logs/ if windows app with
    /// the format Name+DateTime.Now+".log"
    /// </summary>
    public class DefaultLog : LogBase<DefaultLog>
    {
        private string _FileName = "";

        /// <summary>
        /// Constructor
        /// </summary>
        public DefaultLog(string Name)
            : base(Name)
        {
            File = new FileInfo(FileName);
            Start = x => File.Write("Logging started at " + DateTime.Now + System.Environment.NewLine);
            End = x => File.Write("Logging ended at " + DateTime.Now + System.Environment.NewLine, Mode: System.IO.FileMode.Append);
            Log.Add(MessageType.Debug, x => File.Write(x, Mode: System.IO.FileMode.Append));
            Log.Add(MessageType.Error, x => File.Write(x, Mode: System.IO.FileMode.Append));
            Log.Add(MessageType.General, x => File.Write(x, Mode: System.IO.FileMode.Append));
            Log.Add(MessageType.Info, x => File.Write(x, Mode: System.IO.FileMode.Append));
            Log.Add(MessageType.Trace, x => File.Write(x, Mode: System.IO.FileMode.Append));
            Log.Add(MessageType.Warn, x => File.Write(x, Mode: System.IO.FileMode.Append));
            FormatMessage = (Message, Type, args) => Type.ToString()
                + ": " + (args.Length > 0 ? string.Format(CultureInfo.InvariantCulture, Message, args) : Message)
                + System.Environment.NewLine;
            Start(this);
        }

        /// <summary>
        /// File name
        /// </summary>
        public string FileName
        {
            get
            {
                if (string.IsNullOrEmpty(_FileName))
                {
                    _FileName = HttpContext.Current == null ?
                        "~/Logs/" + Name + "-" + DateTime.Now.ToString("yyyyMMddhhmmss", CultureInfo.CurrentCulture) + ".log" :
                        "~/App_Data/Logs/" + Name + "-" + DateTime.Now.ToString("yyyyMMddhhmmss", CultureInfo.CurrentCulture) + ".log";
                }
                return _FileName;
            }
        }

        /// <summary>
        /// File object that the log uses
        /// </summary>
        protected FileInfo File { get; private set; }
    }
}