/*
Copyright (c) 2010 <a href="http://www.gutgames.com">James Craig</a>

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
using System.Data;
using System.Data.SqlClient;

#endregion

namespace Utilities.SQL
{
    /// <summary>
    /// SQL Helper class
    /// </summary>
    public class SQLHelper:IDisposable
    {
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Command">Stored procedure/SQL Text to use</param>
        /// <param name="ConnectionUsing">The connection string to user</param>
        /// <param name="CommandType">The command type of the command sent in</param>
        public SQLHelper(string Command, string ConnectionUsing,CommandType CommandType)
        {
            try
            {
                Connection = new SqlConnection(ConnectionUsing);
                _Command = Command;
                _ExecutableCommand = new SqlCommand(_Command, Connection);
                _ExecutableCommand.CommandType = CommandType;
                this._CommandType = CommandType;
            }
            catch { throw; }
        }

        #endregion

        #region Public Functions

        /// <summary>
        /// Begins a transaction
        /// </summary>
        public void BeginTransaction()
        {
            try
            {
                Transaction = Connection.BeginTransaction();
                Command = _Command;
            }
            catch { throw; }
        }

        /// <summary>
        /// Commits a transaction
        /// </summary>
        public void Commit()
        {
            try
            {
                if (Transaction != null)
                {
                    Transaction.Commit();
                }
            }
            catch { throw; }
        }

        /// <summary>
        /// Rolls back a transaction
        /// </summary>
        public void Rollback()
        {
            try
            {
                if (Transaction != null)
                {
                    Transaction.Rollback();
                }
            }
            catch { throw; }
        }

        /// <summary>
        /// Opens the connection
        /// </summary>
        public void Open()
        {
            try
            {
                if (_ExecutableCommand != null)
                {
                    if (_ExecutableCommand.Connection != null)
                    {
                        _ExecutableCommand.Connection.Open();
                    }
                }
            }
            catch { throw; }
        }

        /// <summary>
        /// Closes the connection
        /// </summary>
        public void Close()
        {
            try
            {
                if (_ExecutableCommand != null)
                {
                    if (_ExecutableCommand.Connection != null)
                    {
                        _ExecutableCommand.Connection.Close();
                    }
                }
            }
            catch { throw; }
        }

        /// <summary>
        /// Adds a parameter to the call (for strings only)
        /// </summary>
        /// <param name="ID">Name of the parameter</param>
        /// <param name="Value">Value to add</param>
        /// <param name="Length">Size of the string(either -1 or 5000 should be used to indicate nvarchar(max))</param>
        public void AddParameter(string ID, string Value, int Length)
        {
            try
            {
                if (Length == 5000)
                {
                    Length = -1;
                }
                if (_ExecutableCommand != null)
                {
                    if (_ExecutableCommand.Parameters.Contains(ID))
                    {
                        if (string.IsNullOrEmpty(Value))
                        {
                            _ExecutableCommand.Parameters[ID].IsNullable = true;
                            _ExecutableCommand.Parameters[ID].Value = System.DBNull.Value;
                        }
                        else
                        {
                            _ExecutableCommand.Parameters[ID].Value = Value;
                        }
                    }
                    else
                    {
                        SqlParameter Parameter = _ExecutableCommand.Parameters.Add(ID, SqlDbType.NVarChar, Length);
                        if (string.IsNullOrEmpty(Value))
                        {
                            Parameter.IsNullable = true;
                            Parameter.Value = System.DBNull.Value;
                        }
                        else
                        {
                            Parameter.Value = Value;
                        }
                    }
                }
            }
            catch { throw; }
        }

        /// <summary>
        /// Adds an output parameter
        /// </summary>
        /// <param name="ID">Name of the parameter</param>
        /// <param name="Type">SQL type of the parameter</param>
        public void AddOutputParameter(string ID, SqlDbType Type)
        {
            try
            {
                if (_ExecutableCommand != null)
                {
                    if (_ExecutableCommand.Parameters.Contains(ID))
                    {
                        _ExecutableCommand.Parameters[ID].Value = null;
                        _ExecutableCommand.Parameters[ID].Direction = ParameterDirection.Output;
                    }
                    else
                    {
                        SqlParameter Parameter = _ExecutableCommand.Parameters.Add(ID, Type);
                        Parameter.Value = null;
                        Parameter.Direction = ParameterDirection.Output;
                    }
                }
            }
            catch { throw; }
        }

        /// <summary>
        /// Adds an output parameter
        /// </summary>
        /// <param name="ID">Name of the parameter</param>
        /// <param name="Length">Length of the string (either -1 or 5000 should be used to indicate nvarchar(max))</param>
        public void AddOutputParameter(string ID, int Length)
        {
            try
            {
                if (Length == 5000)
                {
                    Length = -1;
                }
                if (_ExecutableCommand != null)
                {
                    if (_ExecutableCommand.Parameters.Contains(ID))
                    {
                        _ExecutableCommand.Parameters[ID].Value = null;
                        _ExecutableCommand.Parameters[ID].Direction = ParameterDirection.Output;
                    }
                    else
                    {
                        SqlParameter Parameter = _ExecutableCommand.Parameters.Add(ID, SqlDbType.NVarChar, Length);
                        Parameter.Value = null;
                        Parameter.Direction = ParameterDirection.Output;
                    }
                }
            }
            catch { throw; }
        }

        /// <summary>
        /// Adds a parameter to the call (for all types other than strings)
        /// </summary>
        /// <param name="ID">Name of the parameter</param>
        /// <param name="Value">Value to add</param>
        /// <param name="Type">SQL type of the parameter</param>
        public void AddParameter(string ID, object Value, SqlDbType Type)
        {
            try
            {
                if (_ExecutableCommand != null)
                {
                    if (_ExecutableCommand.Parameters.Contains(ID))
                    {
                        if (Value == null)
                        {
                            _ExecutableCommand.Parameters[ID].IsNullable = true;
                            _ExecutableCommand.Parameters[ID].Value = System.DBNull.Value;
                        }
                        else
                        {
                            _ExecutableCommand.Parameters[ID].Value = Value;
                        }
                    }
                    else
                    {
                        SqlParameter Parameter = _ExecutableCommand.Parameters.Add(ID, Type);
                        if (Value == null)
                        {
                            Parameter.IsNullable = true;
                            Parameter.Value = System.DBNull.Value;
                        }
                        else
                        {
                            Parameter.Value = Value;
                        }
                    }
                }
            }
            catch { throw; }
        }

        /// <summary>
        /// Executes the stored procedure and returns a reader object
        /// </summary>
        public void ExecuteReader()
        {
            try
            {
                if (_ExecutableCommand != null)
                {
                    _Reader = _ExecutableCommand.ExecuteReader();
                }
            }
            catch { throw; }
        }

        /// <summary>
        /// Executes the stored procedure as a non query
        /// </summary>
        /// <returns>Number of rows effected</returns>
        public int ExecuteNonQuery()
        {
            try
            {
                if (_ExecutableCommand != null)
                {
                    return _ExecutableCommand.ExecuteNonQuery();
                }
                return 0;
            }
            catch { throw; }
        }

        /// <summary>
        /// Executes the stored procedure as a scalar query
        /// </summary>
        /// <returns>The object of the first row and first column</returns>
        public object ExecuteScalar()
        {
            try
            {
                if (_ExecutableCommand != null)
                {
                    return _ExecutableCommand.ExecuteScalar();
                }
                return null;
            }
            catch { throw; }
        }

        /// <summary>
        /// Is there more information?
        /// </summary>
        /// <returns>True if there is more rows, false otherwise</returns>
        public bool Read()
        {
            try
            {
                if (_Reader != null)
                {
                    return _Reader.Read();
                }
                return false;
            }
            catch { throw; }
        }

        /// <summary>
        /// Returns an output parameter's value
        /// </summary>
        /// <param name="ID">Parameter name</param>
        /// <param name="Default">Default value for the parameter</param>
        /// <returns>if the parameter exists (and isn't null or empty), it returns the parameter's value. Otherwise the default value is returned.</returns>
        public object GetOutputParameter(string ID, object Default)
        {
            try
            {
                if (_ExecutableCommand != null)
                {
                    if (_ExecutableCommand.Parameters[ID] != null && !string.IsNullOrEmpty(_ExecutableCommand.Parameters[ID].ToString()))
                    {
                        return _ExecutableCommand.Parameters[ID].Value;
                    }
                }
                return Default;
            }
            catch { throw; }
        }

        /// <summary>
        /// Returns a parameter's value
        /// </summary>
        /// <param name="ID">Parameter name</param>
        /// <param name="Default">Default value for the parameter</param>
        /// <returns>if the parameter exists (and isn't null or empty), it returns the parameter's value. Otherwise the default value is returned.</returns>
        public object GetParameter(string ID, object Default)
        {
            try
            {
                if (_Reader != null)
                {
                    if (_Reader[ID] != null && !string.IsNullOrEmpty(_Reader[ID].ToString()))
                    {
                        return _Reader[ID];
                    }
                }
                return Default;
            }
            catch { throw; }
        }

        /// <summary>
        /// Returns a parameter's value
        /// </summary>
        /// <param name="Position">Position in the row</param>
        /// <param name="Default">Default value for the parameter</param>
        /// <returns>if the parameter exists (and isn't null or empty), it returns the parameter's value. Otherwise the default value is returned.</returns>
        public object GetParameter(int Position, object Default)
        {
            try
            {
                if (_Reader != null)
                {
                    if (_Reader[Position] != null && !string.IsNullOrEmpty(_Reader[Position].ToString()))
                    {
                        return _Reader[Position];
                    }
                }
                return Default;
            }
            catch { throw; }
        }

        /// <summary>
        /// Clears the parameters
        /// </summary>
        public void ClearParameters()
        {
            try
            {
                if (_ExecutableCommand != null)
                {
                    _ExecutableCommand.Parameters.Clear();
                }
            }
            catch { throw; }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Stored procedure's name or SQL Text
        /// </summary>
        public string Command
        {
            get { return _Command; }
            set
            {
                try
                {
                    _Command = value;
                    if (_Reader != null)
                    {
                        _Reader.Close();
                        _Reader.Dispose();
                        _Reader = null;
                    }
                    if (_ExecutableCommand != null)
                    {
                        _ExecutableCommand.Dispose();
                        _ExecutableCommand = null;
                    }
                    if (Transaction != null)
                    {
                        _ExecutableCommand = new SqlCommand(_Command, Connection, Transaction);
                    }
                    else
                    {
                        _ExecutableCommand = new SqlCommand(_Command, Connection);
                    }
                    _ExecutableCommand.CommandType = _CommandType;
                }
                catch { throw; }
            }
        }

        /// <summary>
        /// Command Type
        /// </summary>
        public CommandType CommandType
        {
            get { return _CommandType; }
            set
            {
                try
                {
                    _CommandType = value;
                    if (_Reader != null)
                    {
                        _Reader.Close();
                        _Reader.Dispose();
                        _Reader = null;
                    }
                    if (_ExecutableCommand != null)
                    {
                        _ExecutableCommand.Dispose();
                        _ExecutableCommand = null;
                    }
                    if (Transaction != null)
                    {
                        _ExecutableCommand = new SqlCommand(_Command, Connection, Transaction);
                    }
                    else
                    {
                        _ExecutableCommand = new SqlCommand(_Command, Connection);
                    }
                    _ExecutableCommand.CommandType = _CommandType;
                }
                catch { throw; }
            }
        }
        
        #endregion

        #region Private Variables
        private SqlConnection Connection = null;
        private string _Command = null;
        private SqlCommand _ExecutableCommand = null;
        private SqlDataReader _Reader = null;
        private CommandType _CommandType;
        private SqlTransaction Transaction = null;
        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            try
            {
                if (Connection != null)
                {
                    Connection.Dispose();
                    Connection = null;
                }
                if (Transaction != null)
                {
                    Transaction.Dispose();
                    Transaction = null;
                }
                if (_ExecutableCommand != null)
                {
                    _ExecutableCommand.Dispose();
                    _ExecutableCommand = null;
                }
                if (_Reader != null)
                {
                    _Reader.Dispose();
                    _Reader = null;
                }
            }
            catch { throw; }
        }

        #endregion
    }
}
