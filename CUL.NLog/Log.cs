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

using NLog;
using System.Globalization;
using Utilities.IO.Logging.BaseClasses;
using Utilities.IO.Logging.Enums;

#endregion Usings

namespace CUL.Logging
{
    /// <summary>
    /// NLog logging class
    /// </summary>
    public class Log : LogBase<Log>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Name"></param>
        public Log(string Name)
            : base(Name)
        {
            this.InternalLogger = LogManager.GetLogger(Name);
            Start = x => InternalLogger.Log(LogLevel.Debug, "Logging started");
            End = x => InternalLogger.Log(LogLevel.Debug, "Logging ended");
            if (Log != null)
            {
                Log.Add(MessageType.Debug, x => InternalLogger.Log(LogLevel.Debug, x));
                Log.Add(MessageType.Error, x => InternalLogger.Log(LogLevel.Error, x));
                Log.Add(MessageType.General, x => InternalLogger.Log(LogLevel.Info, x));
                Log.Add(MessageType.Info, x => InternalLogger.Log(LogLevel.Info, x));
                Log.Add(MessageType.Trace, x => InternalLogger.Log(LogLevel.Trace, x));
                Log.Add(MessageType.Warn, x => InternalLogger.Log(LogLevel.Warn, x));
            }
            FormatMessage = (Message, Type, args) => args.Length > 0 ? string.Format(CultureInfo.InvariantCulture, Message, args) : Message;
        }

        /// <summary>
        /// Internal logger
        /// </summary>
        private NLog.Logger InternalLogger { get; set; }
    }
}