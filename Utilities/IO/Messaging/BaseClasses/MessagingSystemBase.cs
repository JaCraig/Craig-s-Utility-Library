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
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using Utilities.IO.Messaging.Interfaces;
#endregion

namespace Utilities.IO.Messaging.BaseClasses
{
    /// <summary>
    /// Messaging system base class
    /// </summary>
    public abstract class MessagingSystemBase : IMessagingSystem
    {
        /// <summary>
        /// Constructor
        /// </summary>
        protected MessagingSystemBase()
        {
            Formatters = new List<IFormatter>();
        }

        /// <summary>
        /// Name of the messaging system
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Message type that this handles
        /// </summary>
        public abstract Type MessageType { get; }

        /// <summary>
        /// Formatters that the system have available
        /// </summary>
        public IEnumerable<IFormatter> Formatters { get; private set; }

        /// <summary>
        /// Initializes the system
        /// </summary>
        /// <param name="Formatters">Passes in the list of formatters that the system has found</param>
        public void Initialize(IEnumerable<IFormatter> Formatters)
        {
            Contract.Requires<ArgumentNullException>(Formatters != null, "Formatters");
            this.Formatters = Formatters;
        }

        /// <summary>
        /// Sends a message asynchronously
        /// </summary>
        /// <typeparam name="T">Model type</typeparam>
        /// <param name="Message">Message to send</param>
        /// <param name="Model">Model object</param>
        /// <returns>The async task</returns>
        public async Task Send<T>(IMessage Message, T Model = null)
            where T : class
        {
            if (Message == null)
                return;
            await Task.Run(() =>
            {
                if (Model != null)
                {
                    foreach (IFormatter Formatter in Formatters)
                    {
                        Formatter.Format(Message, Model);
                    }
                }
                InternalSend(Message);
            });
        }

        /// <summary>
        /// Sends a message asynchronously
        /// </summary>
        /// <param name="Message">Message to send</param>
        /// <returns>The async task</returns>
        public async Task Send(IMessage Message)
        {
            if (Message == null)
                return;
            await Task.Run(() =>
            {
                InternalSend(Message);
            });
        }

        /// <summary>
        /// Internal function 
        /// </summary>
        /// <param name="Message">Message to send</param>
        protected abstract void InternalSend(IMessage Message);
    }
}