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
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using Utilities.DataMapper;
using Utilities.DataTypes.ExtensionMethods;
using Utilities.Reflection.ExtensionMethods;
using Utilities.SQL.MicroORM.Enums;
using Utilities.SQL.MicroORM.Interfaces;

#endregion

namespace Utilities.SQL.MicroORM
{
    /// <summary>
    /// Class that acts as a mapping within the micro ORM
    /// </summary>
    /// <typeparam name="ClassType">Class type that this will accept</typeparam>
    public class Mapping<ClassType> : IMapping, IMapping<ClassType> where ClassType : class,new()
    {
        #region Constructors

        /// <summary>
        /// Constructor (can be used if supplying own SQLHelper)
        /// </summary>
        /// <param name="TableName">Table name</param>
        /// <param name="PrimaryKey">Primary key</param>
        /// <param name="AutoIncrement">Is the primary key set to auto increment?</param>
        public Mapping(string TableName, string PrimaryKey, bool AutoIncrement = true)
        {
            Mappings = new TypeMapping<ClassType, SQLHelper>();
            ParameterNames = new List<string>();
            this.TableName = TableName;
            this.PrimaryKey = PrimaryKey;
            this.AutoIncrement = AutoIncrement;
        }

        /// <summary>
        /// Constructor (used internally to create instance versions of static mappings
        /// </summary>
        /// <param name="MappingToCopyFrom">Mapping to copy from</param>
        public Mapping(Mapping<ClassType> MappingToCopyFrom)
        {
            this.Mappings = MappingToCopyFrom.Mappings;
            this.TableName = MappingToCopyFrom.TableName;
            this.PrimaryKey = MappingToCopyFrom.PrimaryKey;
            this.AutoIncrement = MappingToCopyFrom.AutoIncrement;
            this.ParameterNames = MappingToCopyFrom.ParameterNames;
            this.GetPrimaryKey = MappingToCopyFrom.GetPrimaryKey;
            this.PrimaryKeyMapping = MappingToCopyFrom.PrimaryKeyMapping;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Mapper used to map properties to SQLHelper
        /// </summary>
        public TypeMapping<ClassType, SQLHelper> Mappings { get;private set; }

        /// <summary>
        /// Table name
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// Primary key
        /// </summary>
        public string PrimaryKey { get; set; }

        /// <summary>
        /// Used to get/set primary key from an object
        /// </summary>
        public Mapping<ClassType, object> PrimaryKeyMapping { get; set; }

        /// <summary>
        /// Gets the primary key from an object
        /// </summary>
        public Func<ClassType, object> GetPrimaryKey { get; set; }

        /// <summary>
        /// Auto increment?
        /// </summary>
        public bool AutoIncrement { get; set; }

        /// <summary>
        /// Parameter listing
        /// </summary>
        public ICollection<string> ParameterNames { get;private set; }

        #endregion

        #region Map

        /// <summary>
        /// Maps a property to a database property name (required to actually get data from the database)
        /// </summary>
        /// <typeparam name="DataType">Data type of the property</typeparam>
        /// <param name="Property">Property to add a mapping for</param>
        /// <param name="DatabasePropertyName">Property name</param>
        /// <param name="Mode">This determines if the mapping should have read or write access</param>
        /// <param name="DefaultValue">Default value</param>
        /// <returns>This mapping</returns>
        public virtual IMapping<ClassType> Map<DataType>(Expression<Func<ClassType, DataType>> Property,
            string DatabasePropertyName = "",
            DataType DefaultValue = default(DataType),
            Mode Mode = Mode.Read|Mode.Write)
        {
            if (Property==null)
                throw new ArgumentNullException("Property");
            if (Mappings == null)
                throw new ArgumentNullException("Mappings");
            if (string.IsNullOrEmpty(DatabasePropertyName))
                DatabasePropertyName = Property.GetPropertyName();
            Expression Convert = Expression.Convert(Property.Body, typeof(object));
            Expression<Func<ClassType, object>> PropertyExpression = Expression.Lambda<Func<ClassType, object>>(Convert, Property.Parameters);
            Mappings.AddMapping(PropertyExpression,
                ((Mode & Mode.Read) == Mode.Read) ? new Func<SQLHelper, object>((x) => x.GetParameter(DatabasePropertyName, DefaultValue)) : null,
                ((Mode & Mode.Write) == Mode.Write) ? new Action<SQLHelper, object>((x, y) => x.AddParameter(DatabasePropertyName, y)) : null);
            ParameterNames.Add(DatabasePropertyName);
            if (DatabasePropertyName == PrimaryKey)
            {
                PrimaryKeyMapping = new Mapping<ClassType, object>(PropertyExpression, x => x, (x, y) => x = y);
                GetPrimaryKey = PropertyExpression.Compile();
            }
            return this;
        }

        /// <summary>
        /// Maps a property to a database property name (required to actually get data from the database)
        /// </summary>
        /// <param name="Property">Property to add a mapping for</param>
        /// <param name="DatabasePropertyName">Property name</param>
        /// <param name="Mode">This determines if the mapping should have read or write access</param>
        /// <param name="DefaultValue">Default value</param>
        /// <returns>This mapping</returns>
        public virtual IMapping<ClassType> Map(Expression<Func<ClassType, string>> Property,
            string DatabasePropertyName = "",
            string DefaultValue = "",
            Mode Mode = Mode.Read|Mode.Write)
        {
            if (Property == null)
                throw new ArgumentNullException("Property");
            if (Mappings == null)
                throw new ArgumentNullException("Mappings");
            if (string.IsNullOrEmpty(DatabasePropertyName))
                DatabasePropertyName = Property.GetPropertyName();
            Expression Convert = Expression.Convert(Property.Body, typeof(object));
            Expression<Func<ClassType, object>> PropertyExpression = Expression.Lambda<Func<ClassType, object>>(Convert, Property.Parameters);
            Mappings.AddMapping(PropertyExpression,
                ((Mode & Mode.Read) == Mode.Read) ? new Func<SQLHelper, object>((x) => x.GetParameter(DatabasePropertyName, DefaultValue)) : null,
                ((Mode & Mode.Write) == Mode.Write) ? new Action<SQLHelper, object>((x, y) => x.AddParameter(DatabasePropertyName, (string)y)) : null);
            ParameterNames.Add(DatabasePropertyName);
            if (DatabasePropertyName == PrimaryKey)
            {
                PrimaryKeyMapping = new Mapping<ClassType, object>(PropertyExpression, x => x, (x, y) => x = y);
                GetPrimaryKey = PropertyExpression.Compile();
            }
            return this;
        }

        #endregion
    }
}