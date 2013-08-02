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
using System.Net.Mail;
using System.Reflection;
using System.Text;
using Utilities.IO.Messaging;
using Utilities.IO.Messaging.BaseClasses;
using Utilities.IO.Messaging.Interfaces;
#endregion

namespace Utilities.IO
{
    /// <summary>
    /// Email message class
    /// </summary>
    public class EmailMessage : MessageBase, IMessage,IDisposable
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public EmailMessage()
            : base(IoC.Manager.Bootstrapper.Resolve<Manager>().MessagingSystems[typeof(EmailMessage)])
        {
            Attachments = new List<Attachment>();
            EmbeddedResources = new List<LinkedResource>();
            Priority = MailPriority.Normal;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Attachments
        /// </summary>
        public ICollection<Attachment> Attachments { get; private set; }

        /// <summary>
        /// Embedded resource
        /// </summary>
        public ICollection<LinkedResource> EmbeddedResources { get; private set; }

        /// <summary>
        /// Priority
        /// </summary>
        public MailPriority Priority { get; set; }

        /// <summary>
        /// Server
        /// </summary>
        public string Server { get; set; }

        /// <summary>
        /// User name for the user
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Password for the user
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Port to use
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Use SSL?
        /// </summary>
        public bool UseSSL { get; set; }

        /// <summary>
        /// CC
        /// </summary>
        public string CC { get; set; }

        /// <summary>
        /// BCC
        /// </summary>
        public string Bcc { get; set; }

        #endregion

        #region Functions

        /// <summary>
        /// Disposes the object
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes of the objects
        /// </summary>
        /// <param name="Disposing">True to dispose of all resources, false only disposes of native resources</param>
        protected virtual void Dispose(bool Disposing)
        {
            if (Attachments != null)
            {
                foreach (Attachment Attachment in Attachments)
                {
                    Attachment.Dispose();
                }
                Attachments = null;
            }
            if (EmbeddedResources != null)
            {
                foreach (LinkedResource Resource in EmbeddedResources)
                {
                    Resource.Dispose();
                }
                EmbeddedResources = null;
            }
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~EmailMessage()
        {
            Dispose(false);
        }

        #endregion
    }
}