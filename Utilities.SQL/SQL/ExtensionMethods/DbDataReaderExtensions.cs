/*
Copyright (c) 2012 <a href="http://www.gutgames.com">James Craig</a>

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
using System.Xml;
using System.Data.Common;
using Utilities.DataTypes.ExtensionMethods;
using Utilities.DataTypes.Comparison;
#endregion

namespace Utilities.SQL.ExtensionMethods
{
    /// <summary>
    /// Extension methods for DbDataReader objects
    /// </summary>
    public static class DbDataReaderExtensions
    {
        #region Functions
        
        #region GetParameter

        /// <summary>
        /// Returns a parameter's value
        /// </summary>
        /// <param name="Reader">Reader object</param>
        /// <param name="ID">Parameter name</param>
        /// <param name="Default">Default value for the parameter</param>
        /// <returns>if the parameter exists (and isn't null or empty), it returns the parameter's value. Otherwise the default value is returned.</returns>
        public static DataType GetParameter<DataType>(this DbDataReader Reader, string ID, DataType Default = default(DataType))
        {
            if (Reader.IsNull())
                return Default;
            bool Found = false;
            for (int x = 0; x < Reader.FieldCount; ++x)
            {
                if (Reader.GetName(x) == ID)
                {
                    Found = true;
                    break;
                }
            }
            return Found && Reader[ID].IsNotNullOrDBNull() ? Reader[ID].TryTo<object, DataType>() : Default;
        }

        /// <summary>
        /// Returns a parameter's value
        /// </summary>
        /// <param name="Reader">Reader object</param>
        /// <param name="Position">Position in the reader row</param>
        /// <param name="Default">Default value for the parameter</param>
        /// <returns>if the parameter exists (and isn't null or empty), it returns the parameter's value. Otherwise the default value is returned.</returns>
        public static DataType GetParameter<DataType>(this DbDataReader Reader, int Position, DataType Default = default(DataType))
        {
            if (Reader.IsNull())
                return Default;
            return Reader[Position].IsNotNullOrDBNull() ? Reader[Position].TryTo<object, DataType>() : Default;
        }

        #endregion

        #endregion
    }
}
