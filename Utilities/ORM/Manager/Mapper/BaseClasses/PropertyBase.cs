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
using System.Data;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using Utilities.DataTypes;
using Utilities.ORM.Manager.Mapper.Interfaces;

#endregion Usings

namespace Utilities.ORM.Manager.Mapper.BaseClasses
{
    /// <summary>
    /// Property base class
    /// </summary>
    public abstract class PropertyBase<ClassType, DataType, ReturnType> : IProperty<ClassType, DataType, ReturnType>
        where ClassType : class,new()
        where ReturnType : IProperty<ClassType, DataType, ReturnType>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Expression">Expression used to point to the property</param>
        /// <param name="Mapping">Mapping the StringID is added to</param>
        protected PropertyBase(Expression<Func<ClassType, DataType>> Expression, Utilities.ORM.Manager.Mapper.Interfaces.IMapping Mapping)
        {
            Contract.Requires<ArgumentNullException>(Expression != null, "Expression");
            this.Expression = Expression;
            this.Name = Expression.PropertyName();
            this.Type = typeof(DataType);
            this.DerivedFieldName = "_" + Name + "Derived";
            this.Mapping = Mapping;
            this.CompiledExpression = this.Expression.Compile();
            this.MaxLength = typeof(DataType) == typeof(string) ? 100 : 0;
            this.DefaultValue = () => default(DataType);
        }

        /// <summary>
        /// Auto increment
        /// </summary>
        public bool AutoIncrement { get; private set; }

        /// <summary>
        /// Cascade
        /// </summary>
        public bool Cascade { get; private set; }

        /// <summary>
        /// Compiled expression
        /// </summary>
        public Func<ClassType, DataType> CompiledExpression { get; private set; }

        /// <summary>
        /// Default value
        /// </summary>
        public Func<DataType> DefaultValue { get; private set; }

        /// <summary>
        /// Derived field name
        /// </summary>
        public string DerivedFieldName { get; private set; }

        /// <summary>
        /// Expression
        /// </summary>
        public Expression<Func<ClassType, DataType>> Expression { get; private set; }

        /// <summary>
        /// Field name
        /// </summary>
        public string FieldName { get; private set; }

        /// <summary>
        /// Foreign mapping
        /// </summary>
        public Utilities.ORM.Manager.Mapper.Interfaces.IMapping ForeignMapping { get; private set; }

        /// <summary>
        /// Index
        /// </summary>
        public bool Index { get; private set; }

        /// <summary>
        /// Command used to load the property
        /// </summary>
        public string LoadCommand { get; private set; }

        /// <summary>
        /// Command type for the load command
        /// </summary>
        public CommandType LoadCommandType { get; private set; }

        /// <summary>
        /// Mapping
        /// </summary>
        public Utilities.ORM.Manager.Mapper.Interfaces.IMapping Mapping { get; private set; }

        /// <summary>
        /// Max length
        /// </summary>
        public int MaxLength { get; private set; }

        /// <summary>
        /// Property name
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Not null
        /// </summary>
        public bool NotNull { get; private set; }

        /// <summary>
        /// Table name
        /// </summary>
        public string TableName { get; private set; }

        /// <summary>
        /// Property type
        /// </summary>
        public Type Type { get; protected set; }

        /// <summary>
        /// Unique
        /// </summary>
        public bool Unique { get; private set; }

        /// <summary>
        /// != operator
        /// </summary>
        /// <param name="first">First item</param>
        /// <param name="second">Second item</param>
        /// <returns>returns true if they are not equal, false otherwise</returns>
        public static bool operator !=(PropertyBase<ClassType, DataType, ReturnType> first, PropertyBase<ClassType, DataType, ReturnType> second)
        {
            return !(first == second);
        }

        /// <summary>
        /// The &lt; operator
        /// </summary>
        /// <param name="first">First item</param>
        /// <param name="second">Second item</param>
        /// <returns>True if the first item is less than the second, false otherwise</returns>
        public static bool operator <(PropertyBase<ClassType, DataType, ReturnType> first, PropertyBase<ClassType, DataType, ReturnType> second)
        {
            if (Object.ReferenceEquals(first, second))
                return false;
            if ((object)first == null || (object)second == null)
                return false;
            return first.GetHashCode() < second.GetHashCode();
        }

        /// <summary>
        /// The == operator
        /// </summary>
        /// <param name="first">First item</param>
        /// <param name="second">Second item</param>
        /// <returns>true if the first and second item are the same, false otherwise</returns>
        public static bool operator ==(PropertyBase<ClassType, DataType, ReturnType> first, PropertyBase<ClassType, DataType, ReturnType> second)
        {
            if (Object.ReferenceEquals(first, second))
                return true;

            if ((object)first == null || (object)second == null)
                return false;

            return first.GetHashCode() == second.GetHashCode();
        }

        /// <summary>
        /// The &gt; operator
        /// </summary>
        /// <param name="first">First item</param>
        /// <param name="second">Second item</param>
        /// <returns>True if the first item is greater than the second, false otherwise</returns>
        public static bool operator >(PropertyBase<ClassType, DataType, ReturnType> first, PropertyBase<ClassType, DataType, ReturnType> second)
        {
            if (Object.ReferenceEquals(first, second))
                return false;
            if ((object)first == null || (object)second == null)
                return false;
            return first.GetHashCode() > second.GetHashCode();
        }

        /// <summary>
        /// Determines if the two objects are equal and returns true if they are, false otherwise
        /// </summary>
        /// <param name="obj">Object to compare to</param>
        /// <returns>True if they are equal, false otherwise</returns>
        public override bool Equals(object obj)
        {
            PropertyBase<ClassType, DataType, ReturnType> SecondObj = obj as PropertyBase<ClassType, DataType, ReturnType>;
            if (((object)SecondObj) == null)
                return false;
            return this == SecondObj;
        }

        /// <summary>
        /// Returns the hash code for the property
        /// </summary>
        /// <returns>The hash code for the property</returns>
        public override int GetHashCode()
        {
            return (Name.GetHashCode() * Mapping.GetHashCode()) % int.MaxValue;
        }

        /// <summary>
        /// Gets the property's value from the object sent in
        /// </summary>
        /// <param name="Object">Object to get the value from</param>
        /// <returns>The value of the property</returns>
        public object GetValue(ClassType Object)
        {
            return CompiledExpression(Object);
        }

        /// <summary>
        /// Turns on autoincrement for this property
        /// </summary>
        /// <returns>This</returns>
        public ReturnType SetAutoIncrement()
        {
            this.AutoIncrement = true;
            return (ReturnType)((IProperty<ClassType, DataType, ReturnType>)this);
        }

        /// <summary>
        /// Turns on cascade for saving/deleting
        /// </summary>
        /// <returns>this</returns>
        public ReturnType SetCascade()
        {
            this.Cascade = true;
            return (ReturnType)((IProperty<ClassType, DataType, ReturnType>)this);
        }

        /// <summary>
        /// Sets the default value of the property
        /// </summary>
        /// <param name="Value">Default value</param>
        /// <returns>This IProperty object</returns>
        public ReturnType SetDefaultValue(Func<DataType> Value)
        {
            this.DefaultValue = Value;
            return (ReturnType)((IProperty<ClassType, DataType, ReturnType>)this);
        }

        /// <summary>
        /// Sets the name of the field in the database
        /// </summary>
        /// <param name="FieldName">Field name</param>
        /// <returns>this</returns>
        public ReturnType SetFieldName(string FieldName)
        {
            this.FieldName = FieldName;
            return (ReturnType)((IProperty<ClassType, DataType, ReturnType>)this);
        }

        /// <summary>
        /// Turns on indexing for this property
        /// </summary>
        /// <returns>This</returns>
        public ReturnType SetIndex()
        {
            this.Index = true;
            return (ReturnType)((IProperty<ClassType, DataType, ReturnType>)this);
        }

        /// <summary>
        /// Allows you to load a property based on a specified command
        /// </summary>
        /// <param name="Command">Command used to load the property</param>
        /// <param name="CommandType">Command type</param>
        /// <returns>this</returns>
        public ReturnType SetLoadUsingCommand(string Command, CommandType CommandType)
        {
            this.LoadCommand = Command;
            this.LoadCommandType = CommandType;
            return (ReturnType)((IProperty<ClassType, DataType, ReturnType>)this);
        }

        /// <summary>
        /// Sets the max length for the property (or precision for items like decimal values)
        /// </summary>
        /// <param name="MaxLength">Max length</param>
        /// <returns>this</returns>
        public ReturnType SetMaxLength(int MaxLength)
        {
            this.MaxLength = MaxLength;
            return (ReturnType)((IProperty<ClassType, DataType, ReturnType>)this);
        }

        /// <summary>
        /// Sets the field such that null values are not allowed
        /// </summary>
        /// <returns>this</returns>
        public ReturnType SetNotNull()
        {
            this.NotNull = true;
            return (ReturnType)((IProperty<ClassType, DataType, ReturnType>)this);
        }

        /// <summary>
        /// Set database table name
        /// </summary>
        /// <param name="TableName">Table name</param>
        /// <returns>this</returns>
        public ReturnType SetTableName(string TableName)
        {
            this.TableName = TableName;
            return (ReturnType)((IProperty<ClassType, DataType, ReturnType>)this);
        }

        /// <summary>
        /// Ensures the field is unique
        /// </summary>
        /// <returns>this</returns>
        public ReturnType SetUnique()
        {
            this.Unique = true;
            return (ReturnType)((IProperty<ClassType, DataType, ReturnType>)this);
        }

        /// <summary>
        /// Gets the property as a string
        /// </summary>
        /// <returns>The string representation of the property</returns>
        public override string ToString()
        {
            return Type.GetName() + " " + Mapping.ToString() + "." + Name;
        }
    }
}