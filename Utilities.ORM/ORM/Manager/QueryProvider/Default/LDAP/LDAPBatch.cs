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
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics.Contracts;
using System.DirectoryServices;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Utilities.DataTypes;
using Utilities.ORM.Manager.QueryProvider.Interfaces;
using Utilities.ORM.Manager.SourceProvider.Interfaces;

namespace Utilities.ORM.Manager.QueryProvider.Default.LDAP
{
    /// <summary>
    /// LDAP batch class
    /// </summary>
    public class LDAPBatch : IBatch
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Source">Source info</param>
        public LDAPBatch(ISourceInfo Source)
        {
            this.Commands = new List<Command>();
            this.Source = Source;
        }

        /// <summary>
        /// Number of commands being batched
        /// </summary>
        public int CommandCount
        {
            get { return Commands.Count; }
        }

        /// <summary>
        /// Gets or sets the commands.
        /// </summary>
        /// <value>
        /// The commands.
        /// </value>
        private List<Command> Commands { get; set; }

        /// <summary>
        /// Gets or sets the source.
        /// </summary>
        /// <value>
        /// The source.
        /// </value>
        private ISourceInfo Source { get; set; }

        /// <summary>
        /// Adds a command to be batched
        /// </summary>
        /// <param name="CallBack">Callback action</param>
        /// <param name="Object">Object used in the callback action</param>
        /// <param name="CommandType">Command type</param>
        /// <param name="Command">Command (SQL or stored procedure) to run</param>
        /// <returns>
        /// This
        /// </returns>
        public IBatch AddCommand(Action<Command, IList<dynamic>> CallBack, object Object, CommandType CommandType, string Command)
        {
            Commands.Add(new Command(CallBack, Object, Command, CommandType, "", null));
            return this;
        }

        /// <summary>
        /// Adds a command to be batched
        /// </summary>
        /// <param name="CallBack">Callback action</param>
        /// <param name="Object">Object used in the callback action</param>
        /// <param name="Command">Command (SQL or stored procedure) to run</param>
        /// <param name="CommandType">Command type</param>
        /// <param name="Parameters">Parameters to add</param>
        /// <returns>
        /// This
        /// </returns>
        public IBatch AddCommand(Action<Command, IList<dynamic>> CallBack, object Object, string Command, CommandType CommandType, params object[] Parameters)
        {
            Commands.Add(new Command(CallBack, Object, Command, CommandType, "", Parameters));
            return this;
        }

        /// <summary>
        /// Adds a command to be batched
        /// </summary>
        /// <param name="CallBack">Callback action</param>
        /// <param name="Object">Object used in the callback action</param>
        /// <param name="Command">Command (SQL or stored procedure) to run</param>
        /// <param name="CommandType">Command type</param>
        /// <param name="Parameters">Parameters to add</param>
        /// <returns>
        /// This
        /// </returns>
        public IBatch AddCommand(Action<Command, IList<dynamic>> CallBack, object Object, string Command, CommandType CommandType, params IParameter[] Parameters)
        {
            Commands.Add(new Command(CallBack, Object, Command, CommandType, "", Parameters));
            return this;
        }

        /// <summary>
        /// Adds a batch's commands to the current batch
        /// </summary>
        /// <param name="Batch">Batch to add</param>
        /// <returns>
        /// This
        /// </returns>
        public IBatch AddCommand(IBatch Batch)
        {
            var TempValue = Batch as LDAPBatch;
            if (TempValue == null)
                return this;
            Commands.Add(TempValue.Commands);
            return this;
        }

        /// <summary>
        /// Executes the commands and returns the results
        /// </summary>
        /// <returns>
        /// The results of the batched commands
        /// </returns>
        public IList<IList<dynamic>> Execute()
        {
            var ReturnValue = new List<IList<dynamic>>();
            if (Commands.Count == 0)
            {
                ReturnValue.Add(new List<dynamic>());
                return ReturnValue;
            }
            using (DirectoryEntry Entry = new DirectoryEntry(Source.Server, Source.UserName, Source.Password, AuthenticationTypes.Secure))
            {
                using (DirectorySearcher Searcher = new DirectorySearcher(Entry))
                {
                    Searcher.PageSize = 1000;
                    foreach (Command Command in Commands)
                    {
                        Searcher.Filter = Command.SQLCommand;
                        using (SearchResultCollection Results = Searcher.FindAll())
                        {
                            var ReturnValues = new List<dynamic>();
                            foreach (SearchResult Result in Results)
                            {
                                var TempValue = new Dynamo();
                                foreach (PropertyValueCollection Property in Result.GetDirectoryEntry().Properties)
                                {
                                    TempValue[Property.PropertyName] = Property.Value;
                                    ReturnValues.Add(TempValue);
                                }
                            }
                            ReturnValue.Add(ReturnValues);
                        }
                    }
                }
                Entry.Close();
            }
            for (int x = 0; x < Commands.Count(); ++x)
            {
                Commands[x].Finalize(ReturnValue[x]);
            }
            return ReturnValue;
        }

        /// <summary>
        /// Removes duplicate commands from the batch
        /// </summary>
        /// <returns>
        /// This
        /// </returns>
        public IBatch RemoveDuplicateCommands()
        {
            Commands = Commands.Distinct().ToList();
            return this;
        }
    }
}