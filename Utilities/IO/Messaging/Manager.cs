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
using System.Linq;
using System.Text;
using Utilities.DataTypes;
using Utilities.IO.Messaging.BaseClasses;
using Utilities.IO.Messaging.Interfaces;

#endregion Usings

namespace Utilities.IO.Messaging
{
    /// <summary>
    /// Messaging manager
    /// </summary>
    public class Manager
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Manager()
        {
            Formatters = AppDomain.CurrentDomain.GetAssemblies().Objects<IFormatter>().ToList();
            MessagingSystems = new Dictionary<Type, IMessagingSystem>();
            IEnumerable<IMessagingSystem> TempSystems = AppDomain.CurrentDomain.GetAssemblies().Objects<IMessagingSystem>();
            TempSystems.ForEach(x =>
            {
                ((MessagingSystemBase)x).Initialize(Formatters);
                MessagingSystems.Add(x.MessageType, x);
            });
        }

        /// <summary>
        /// Formatters
        /// </summary>
        public IList<IFormatter> Formatters { get; private set; }

        /// <summary>
        /// Messaging systems
        /// </summary>
        public IDictionary<Type, IMessagingSystem> MessagingSystems { get; private set; }

        /// <summary>
        /// String info for the manager
        /// </summary>
        /// <returns>The string info that the manager contains</returns>
        public override string ToString()
        {
            StringBuilder Builder = new StringBuilder();
            Builder.AppendLineFormat("Formatters: {0}\r\nMessaging Systems: {1}", Formatters.ToString(x => x.Name), MessagingSystems.ToString(x => x.Value.Name));
            return Builder.ToString();
        }
    }
}