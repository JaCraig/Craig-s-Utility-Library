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

using System.Threading.Tasks;
using Utilities.DataTypes.Patterns.BaseClasses;
using Utilities.IO.Messaging.Interfaces;

namespace Utilities.IO.Messaging.BaseClasses
{
    /// <summary>
    /// Message base
    /// </summary>
    public abstract class MessageBase : SafeDisposableBaseClass, IMessage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="MessagingSystem">Messaging system used to create the message</param>
        protected MessageBase(IMessagingSystem MessagingSystem)
        {
            this.MessagingSystem = MessagingSystem;
        }

        /// <summary>
        /// Body of the text
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Whom the message is from
        /// </summary>
        public string From { get; set; }

        /// <summary>
        /// The subject of the Communicator
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Whom the message is to
        /// </summary>
        public string To { get; set; }

        /// <summary>
        /// Messaging system
        /// </summary>
        private IMessagingSystem MessagingSystem { get; set; }

        /// <summary>
        /// Send a message
        /// </summary>
        /// <typeparam name="T">Model type</typeparam>
        /// <param name="Model">Model object</param>
        /// <returns>The async task object</returns>
        public virtual Task Send<T>(T Model = default(T))
            where T : class
        {
            return MessagingSystem.Send(this, Model);
        }

        /// <summary>
        /// Send a message
        /// </summary>
        /// <returns>The async task object</returns>
        public virtual Task Send()
        {
            return MessagingSystem.Send(this);
        }
    }
}