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
using System.Data;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using Utilities.DataTypes;
using Utilities.ORM.Manager.QueryProvider.Interfaces;

namespace Utilities.ORM.Manager.QueryProvider.Default
{
    /// <summary>
    /// Command class
    /// </summary>
    public class Command : ICommand
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="SQLCommand">SQL Command</param>
        /// <param name="CommandType">Command type</param>
        /// <param name="Parameters">Parameters</param>
        /// <param name="CallBack">Called when command has been executed</param>
        /// <param name="Object">Object</param>
        public Command(Action<Command, IList<dynamic>> CallBack, object Object, string SQLCommand, CommandType CommandType, IParameter[] Parameters)
        {
            this.SQLCommand = SQLCommand;
            this.CommandType = CommandType;
            this.CallBack = CallBack.Check((x, y) => { });
            this.Object = Object;
            this.Parameters = Parameters.Check(new IParameter[0]);
            this.Finalizable = SQLCommand.Check("").ToUpperInvariant().Contains("SELECT");
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="SQLCommand">SQL Command</param>
        /// <param name="CommandType">Command type</param>
        /// <param name="Parameters">Parameters</param>
        /// <param name="ParameterStarter">Parameter starter</param>
        /// <param name="CallBack">Called when command has been executed</param>
        /// <param name="Object">Object</param>
        public Command(Action<Command, IList<dynamic>> CallBack, object Object, string SQLCommand, CommandType CommandType, string ParameterStarter, object[] Parameters)
        {
            this.SQLCommand = SQLCommand;
            this.CommandType = CommandType;
            this.Parameters = new List<IParameter>();
            this.CallBack = CallBack.Check((x, y) => { });
            this.Object = Object;
            this.Finalizable = SQLCommand.Check("").ToUpperInvariant().Contains("SELECT");
            if (Parameters != null)
            {
                foreach (object Parameter in Parameters)
                {
                    var TempParameter = Parameter as string;
                    if (Parameter == null)
                        this.Parameters.Add(new Parameter<object>(this.Parameters.Count().ToString(CultureInfo.InvariantCulture), default(DbType), null, ParameterDirection.Input, ParameterStarter));
                    else if (TempParameter != null)
                        this.Parameters.Add(new StringParameter(this.Parameters.Count().ToString(CultureInfo.InvariantCulture), TempParameter, ParameterDirection.Input, ParameterStarter));
                    else
                        this.Parameters.Add(new Parameter<object>(this.Parameters.Count().ToString(CultureInfo.InvariantCulture), Parameter, ParameterDirection.Input, ParameterStarter));
                }
            }
        }

        /// <summary>
        /// Call back
        /// </summary>
        public Action<Command, IList<dynamic>> CallBack { get; private set; }

        /// <summary>
        /// Command type
        /// </summary>
        public CommandType CommandType { get; set; }

        /// <summary>
        /// Used to determine if Finalize should be called.
        /// </summary>
        public bool Finalizable { get; private set; }

        /// <summary>
        /// Object
        /// </summary>
        public object Object { get; private set; }

        /// <summary>
        /// Parameters
        /// </summary>
        public ICollection<IParameter> Parameters { get; private set; }

        /// <summary>
        /// SQL command
        /// </summary>
        public string SQLCommand { get; set; }

        /// <summary>
        /// Determines if the objects are equal
        /// </summary>
        /// <param name="obj">Object to compare to</param>
        /// <returns>Determines if the commands are equal</returns>
        public override bool Equals(object obj)
        {
            var OtherCommand = obj as Command;
            if (OtherCommand == null)
                return false;

            if (OtherCommand.SQLCommand != SQLCommand
                || OtherCommand.CommandType != CommandType
                || Parameters.Count != OtherCommand.Parameters.Count)
                return false;

            foreach (IParameter Parameter in Parameters)
                if (!OtherCommand.Parameters.Contains(Parameter))
                    return false;

            foreach (IParameter Parameter in OtherCommand.Parameters)
                if (!Parameters.Contains(Parameter))
                    return false;

            return true;
        }

        /// <summary>
        /// Called after the command is run
        /// </summary>
        /// <param name="Result">Result of the command</param>
        public void Finalize(IList<dynamic> Result)
        {
            Contract.Requires<NullReferenceException>(CallBack != null, "CallBack");
            CallBack(this, Result);
        }

        /// <summary>
        /// Returns the hash code for the command
        /// </summary>
        /// <returns>The hash code for the object</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int ParameterTotal = 0;
                foreach (IParameter Parameter in Parameters)
                {
                    ParameterTotal += Parameter.GetHashCode();
                }
                if (ParameterTotal > 0)
                    return (SQLCommand.GetHashCode() * 23 + CommandType.GetHashCode()) * 23 + ParameterTotal;
                return SQLCommand.GetHashCode() * 23 + CommandType.GetHashCode();
            }
        }

        /// <summary>
        /// Returns the string representation of the command
        /// </summary>
        /// <returns>The string representation of the command</returns>
        public override string ToString()
        {
            string TempCommand = SQLCommand.Check("");
            Parameters.ForEach(x => { TempCommand = x.AddParameter(TempCommand); });
            return TempCommand;
        }
    }
}