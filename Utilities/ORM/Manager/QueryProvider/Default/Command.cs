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

using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using Utilities.DataTypes;
using Utilities.ORM.Manager.QueryProvider.Interfaces;

#endregion Usings

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
        public Command(string SQLCommand, CommandType CommandType, IParameter[] Parameters)
        {
            this.SQLCommand = SQLCommand;
            this.CommandType = CommandType;
            this.Parameters = Parameters.Check(new IParameter[0]);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="SQLCommand">SQL Command</param>
        /// <param name="CommandType">Command type</param>
        /// <param name="Parameters">Parameters</param>
        /// <param name="ParameterStarter">Parameter starter</param>
        public Command(string SQLCommand, CommandType CommandType, string ParameterStarter, object[] Parameters)
        {
            this.SQLCommand = SQLCommand;
            this.CommandType = CommandType;
            this.Parameters = new List<IParameter>();
            if (Parameters != null)
            {
                foreach (object Parameter in Parameters)
                {
                    string TempParameter = Parameter as string;
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
        /// Command type
        /// </summary>
        public CommandType CommandType { get; set; }

        /// <summary>
        /// Parameters
        /// </summary>
        public ICollection<IParameter> Parameters { get; private set; }

        /// <summary>
        /// SQL command
        /// </summary>
        public string SQLCommand { get; set; }
    }
}