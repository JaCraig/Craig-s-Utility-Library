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
using System.ComponentModel;
using System.Data;
using Utilities.DataTypes;

namespace Utilities.ORM
{
    /// <summary>
    /// Extension methods for DbDataReader objects
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class IDataReaderExtensions
    {
        /// <summary>
        /// Returns a parameter's value
        /// </summary>
        /// <param name="Reader">Reader object</param>
        /// <param name="ID">Parameter name</param>
        /// <param name="Default">Default value for the parameter</param>
        /// <returns>
        /// if the parameter exists (and isn't null or empty), it returns the parameter's value.
        /// Otherwise the default value is returned.
        /// </returns>
        public static DataType GetParameter<DataType>(this IDataRecord Reader, string ID, DataType Default = default(DataType))
        {
            if (Reader == null)
                return Default;
            for (int x = 0; x < Reader.FieldCount; ++x)
            {
                if (Reader.GetName(x) == ID)
                    return Reader.GetParameter<DataType>(x, Default);
            }
            return Default;
        }

        /// <summary>
        /// Returns a parameter's value
        /// </summary>
        /// <param name="Reader">Reader object</param>
        /// <param name="Position">Position in the reader row</param>
        /// <param name="Default">Default value for the parameter</param>
        /// <returns>
        /// if the parameter exists (and isn't null or empty), it returns the parameter's value.
        /// Otherwise the default value is returned.
        /// </returns>
        public static DataType GetParameter<DataType>(this IDataRecord Reader, int Position, DataType Default = default(DataType))
        {
            if (Reader == null)
                return Default;
            object Value = Reader[Position];
            return (Value == null || Convert.IsDBNull(Value)) ? Default : Value.To<object, DataType>(Default);
        }
    }
}