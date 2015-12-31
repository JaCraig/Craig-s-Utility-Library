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
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using Utilities.DataTypes;
using Utilities.IO.Messaging.BaseClasses;
using Utilities.IO.Messaging.Interfaces;

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
        /// <param name="Formatters">The formatters.</param>
        /// <param name="MessagingSystems">The messaging systems.</param>
        public Manager(IEnumerable<IFormatter> Formatters, IEnumerable<IMessagingSystem> MessagingSystems)
        {
            Contract.Requires<ArgumentNullException>(Formatters != null, "Formatters");
            Contract.Requires<ArgumentNullException>(MessagingSystems != null, "MessagingSystems");
            this.Formatters = Formatters.Where(x => !x.GetType().Namespace.StartsWith("UTILITIES", StringComparison.OrdinalIgnoreCase)).ToList();
            if (this.Formatters.Count() == 0)
                this.Formatters = Formatters.Where(x => x.GetType().Namespace.StartsWith("UTILITIES", StringComparison.OrdinalIgnoreCase)).ToList();
            this.MessagingSystems = new Dictionary<Type, IMessagingSystem>();
            MessagingSystems.ForEach(x =>
            {
                ((MessagingSystemBase)x).Initialize(Formatters);
                this.MessagingSystems.Add(x.MessageType, x);
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
            var Builder = new StringBuilder();
            Builder.AppendLineFormat("Formatters: {0}\r\nMessaging Systems: {1}", Formatters.ToString(x => x.Name), MessagingSystems.ToString(x => x.Value.Name));
            return Builder.ToString();
        }
    }
}