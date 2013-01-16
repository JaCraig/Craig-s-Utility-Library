using System;
using System.Data;
using Utilities.DataTypes;
using System.Collections.Generic;
using System.Linq;

namespace Utilities.SQL.MicroORM
{
    /// <summary>
    /// Holds a set of tablular info for caching purposes
    /// </summary>
    public class CacheTables : IDataReader
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Reader">Reader to copy from</param>
        public CacheTables(IDataReader Reader)
        {
            if (Reader is CacheTables)
            {
                CacheTables Temp = Reader as CacheTables;
                this.Tables = Temp.Tables;
                CurrentTable = Tables.FirstOrDefault();
                CurrentRow = CurrentTable[0];
                CurrentTablePosition = 0;
                RowPosition = -1;
                return;
            }
            Tables = new System.Collections.Generic.List<Table>();
            Tables.Add(new Table(Reader));
            while (Reader.NextResult())
            {
                Tables.Add(new Table(Reader));
            }
            CurrentTable = Tables.FirstOrDefault();
            CurrentRow = CurrentTable[0];
            CurrentTablePosition = 0;
            RowPosition = -1;
        }

        #endregion

        #region Properties

        /// <summary>
        /// List of tables being held
        /// </summary>
        public ICollection<Table> Tables { get; private set; }

        /// <summary>
        /// Current row
        /// </summary>
        protected Row CurrentRow { get; set; }

        /// <summary>
        /// Current table to get info from
        /// </summary>
        protected Table CurrentTable { get; set; }

        /// <summary>
        /// Position of the current table
        /// </summary>
        protected int CurrentTablePosition { get; set; }

        /// <summary>
        /// Row position
        /// </summary>
        protected int RowPosition { get; set; }

        /// <summary>
        /// Field count
        /// </summary>
        public int FieldCount
        {
            get { return CurrentTable.ColumnNames.Length; }
        }

        #endregion

        #region Functions

        /// <summary>
        /// Goes to the next result set
        /// </summary>
        /// <returns>True if it exists, false otherwise</returns>
        public virtual bool NextResult()
        {
            ++CurrentTablePosition;
            RowPosition = -1;
            if (Tables.Count <= CurrentTablePosition)
                return false;
            CurrentTable = Tables.ElementAt(CurrentTablePosition);
            CurrentRow = CurrentTable[0];
            return true;
        }

        /// <summary>
        /// Goes to the next row of the result set
        /// </summary>
        /// <returns>True if there is another row, false otherwise</returns>
        public bool Read()
        {
            ++RowPosition;
            CurrentRow = CurrentTable[RowPosition];
            return CurrentTable.Rows.Count > RowPosition;
        }

        /// <summary>
        /// Gets the name of the column at a specific position
        /// </summary>
        /// <param name="i">Specific position</param>
        /// <returns>Name of the column</returns>
        public string GetName(int i)
        {
            return CurrentRow.ColumnNames[i];
        }

        /// <summary>
        /// Gets the specified value
        /// </summary>
        /// <param name="i">Position to get the value from</param>
        /// <returns>The value at the position specified</returns>
        public object GetValue(int i)
        {
            return CurrentRow[i];
        }

        /// <summary>
        /// Gets the value specified
        /// </summary>
        /// <param name="name">Name of the column</param>
        /// <returns>The value specified</returns>
        public object this[string name]
        {
            get { return CurrentRow[name]; }
        }

        /// <summary>
        /// Gets the value specified
        /// </summary>
        /// <param name="i">Column to get the value from</param>
        /// <returns>The value specified</returns>
        public object this[int i]
        {
            get { return CurrentRow[i]; }
        }

        #endregion

        #region Not implemented items from IDataReader

        /// <summary>
        /// Not implemented
        /// </summary>
        public void Close()
        {

        }

        /// <summary>
        /// Not implemented
        /// </summary>
        public int Depth
        {
            get { return 0; }
        }

        /// <summary>
        /// Not implemented
        /// </summary>
        /// <returns>Null</returns>
        public DataTable GetSchemaTable()
        {
            return null;
        }

        /// <summary>
        /// Not implemented
        /// </summary>
        public bool IsClosed
        {
            get { return true; }
        }

        /// <summary>
        /// Not implemented
        /// </summary>
        public int RecordsAffected
        {
            get { return 0; }
        }

        /// <summary>
        /// Not implemented
        /// </summary>
        public void Dispose()
        {

        }

        /// <summary>
        /// Not implemented
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public bool GetBoolean(int i)
        {
            return false;//return CurrentTable[RowPosition]
        }

        /// <summary>
        /// Not implemented
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public byte GetByte(int i)
        {
            return 0;
        }

        /// <summary>
        /// Not implemented
        /// </summary>
        /// <param name="i"></param>
        /// <param name="fieldOffset"></param>
        /// <param name="buffer"></param>
        /// <param name="bufferoffset"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            return 0;
        }

        /// <summary>
        /// Not implemented
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public char GetChar(int i)
        {
            return 'a';
        }

        /// <summary>
        /// Not implemented
        /// </summary>
        /// <param name="i"></param>
        /// <param name="fieldoffset"></param>
        /// <param name="buffer"></param>
        /// <param name="bufferoffset"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            return 0;
        }

        /// <summary>
        /// Not implemtned
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public IDataReader GetData(int i)
        {
            return this;
        }

        /// <summary>
        /// Not implemented
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public string GetDataTypeName(int i)
        {
            return "";
        }

        /// <summary>
        /// Not implemented
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public DateTime GetDateTime(int i)
        {
            return DateTime.MinValue;
        }

        /// <summary>
        /// Not implemented
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public decimal GetDecimal(int i)
        {
            return 0;
        }

        /// <summary>
        /// Not implemented
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public double GetDouble(int i)
        {
            return 0;
        }

        /// <summary>
        /// Not implemented
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public Type GetFieldType(int i)
        {
            return typeof(int);
        }

        /// <summary>
        /// Not implemented
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public float GetFloat(int i)
        {
            return 0f;
        }

        /// <summary>
        /// Not implemented
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public Guid GetGuid(int i)
        {
            return Guid.Empty;
        }

        /// <summary>
        /// Not implemented
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public short GetInt16(int i)
        {
            return 0;
        }

        /// <summary>
        /// Not implemented
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public int GetInt32(int i)
        {
            return 0;
        }

        /// <summary>
        /// Not implemented
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public long GetInt64(int i)
        {
            return 0;
        }

        /// <summary>
        /// Not implemented
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public int GetOrdinal(string name)
        {
            return 0;
        }

        /// <summary>
        /// Not implemented
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public string GetString(int i)
        {
            return "";
        }

        /// <summary>
        /// Not implemented
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public int GetValues(object[] values)
        {
            return 0;
        }

        /// <summary>
        /// Not implemented
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public bool IsDBNull(int i)
        {
            return false;
        }

        #endregion
    }
}