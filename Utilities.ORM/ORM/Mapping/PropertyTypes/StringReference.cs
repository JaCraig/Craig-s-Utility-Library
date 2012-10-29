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
using System.Linq.Expressions;
using Utilities.ORM.Mapping.BaseClasses;
using Utilities.ORM.Mapping.Interfaces;
using Utilities.ORM.QueryProviders.Interfaces;
using Utilities.SQL.Interfaces;
using Utilities.SQL.MicroORM;
using Utilities.SQL.MicroORM.Enums;
using Utilities.SQL;
using System.Collections;
using System.Collections.Generic;
#endregion

namespace Utilities.ORM.Mapping.PropertyTypes
{
    /// <summary>
    /// Reference class
    /// </summary>
    /// <typeparam name="ClassType">Class type</typeparam>
    public class StringReference<ClassType> : PropertyBase<ClassType, string, IReference<ClassType, string>>,
        IReference<ClassType, string>
        where ClassType : class,new()
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Expression">Expression pointing to the property</param>
        /// <param name="Mapping">Mapping that the reference is added to</param>
        public StringReference(Expression<Func<ClassType, string>> Expression, IMapping Mapping)
            : base(Expression, Mapping)
        {
            SetDefaultValue(() => "");
            SetFieldName(Name + "_");
            this.MaxLength = 100;
        }

        #endregion

        #region Functions

        /// <summary>
        /// Sets up the default load commands
        /// </summary>
        public override void SetupLoadCommands()
        {

        }

        /// <summary>
        /// Deletes the object from join tables
        /// </summary>
        /// <param name="Object">Object to remove</param>
        /// <param name="MicroORM">Micro ORM object</param>
        /// <returns>The list of commands needed to do this</returns>
        public override IEnumerable<Command> JoinsDelete(ClassType Object, MicroORM MicroORM)
        {
            return new List<Command>();
        }

        /// <summary>
        /// Saves the object to various join tables
        /// </summary>
        /// <param name="Object">Object to add</param>
        /// <param name="MicroORM">Micro ORM object</param>
        /// <returns>The list of commands needed to do this</returns>
        public override IEnumerable<Command> JoinsSave(ClassType Object, MicroORM MicroORM)
        {
            return new List<Command>();
        }

        /// <summary>
        /// Deletes the object to from join tables on cascade
        /// </summary>
        /// <param name="Object">Object</param>
        /// <param name="MicroORM">Micro ORM object</param>
        /// <returns>The list of commands needed to do this</returns>
        public override IEnumerable<Command> CascadeJoinsDelete(ClassType Object, MicroORM MicroORM)
        {
            return new List<Command>();
        }

        /// <summary>
        /// Saves the object to various join tables on cascade
        /// </summary>
        /// <param name="Object">Object to add</param>
        /// <param name="MicroORM">Micro ORM object</param>
        /// <returns>The list of commands needed to do this</returns>
        public override IEnumerable<Command> CascadeJoinsSave(ClassType Object, MicroORM MicroORM)
        {
            return new List<Command>();
        }

        /// <summary>
        /// Deletes the object on cascade
        /// </summary>
        /// <param name="Object">Object</param>
        /// <param name="MicroORM">Micro ORM object</param>
        public override void CascadeDelete(ClassType Object, MicroORM MicroORM)
        {

        }

        /// <summary>
        /// Saves the object on cascade
        /// </summary>
        /// <param name="Object">Object</param>
        /// <param name="MicroORM">Micro ORM object</param>
        public override void CascadeSave(ClassType Object, MicroORM MicroORM)
        {
        }

        /// <summary>
        /// Gets it as a parameter
        /// </summary>
        /// <param name="Object">Object</param>
        /// <returns>The value as a parameter</returns>
        public override IParameter GetAsParameter(ClassType Object)
        {
            return null;
        }

        /// <summary>
        /// Gets it as an object
        /// </summary>
        /// <param name="Object">Object</param>
        /// <returns>The value as an object</returns>
        public override object GetAsObject(ClassType Object)
        {
            return null;
        }

        /// <summary>
        /// Sets the loading command used
        /// </summary>
        /// <param name="Command">Command to use</param>
        /// <param name="CommandType">Command type</param>
        /// <returns>This</returns>
        public override IReference<ClassType, string> LoadUsingCommand(string Command, System.Data.CommandType CommandType)
        {
            this.CommandToLoad = new Command(Command, CommandType);
            return (IReference<ClassType, string>)this;
        }

        /// <summary>
        /// Add to query provider
        /// </summary>
        /// <param name="Database">Database object</param>
        /// <param name="Mapping">Mapping object</param>
        public override void AddToQueryProvider(IDatabase Database, Mapping<ClassType> Mapping)
        {
            Mode Mode = Mode.Neither;
            if (Database.Readable)
                Mode |= Mode.Read;
            if (Database.Writable)
                Mode |= Mode.Write;
            Mapping.Map(this.Expression, this.FieldName, this.MaxLength, DefaultValue(), Mode);
        }

        /// <summary>
        /// Set a default value
        /// </summary>
        /// <param name="DefaultValue">Default value</param>
        /// <returns>This</returns>
        public override IReference<ClassType, string> SetDefaultValue(Func<string> DefaultValue)
        {
            this.DefaultValue = DefaultValue;
            return (IReference<ClassType, string>)this;
        }

        /// <summary>
        /// Does not allow null values
        /// </summary>
        /// <returns>This</returns>
        public override IReference<ClassType, string> DoNotAllowNullValues()
        {
            this.NotNull = true;

            return (IReference<ClassType, string>)this;
        }

        /// <summary>
        /// This should be unique
        /// </summary>
        /// <returns>This</returns>
        public override IReference<ClassType, string> ThisShouldBeUnique()
        {
            this.Unique = true;
            return (IReference<ClassType, string>)this;
        }

        /// <summary>
        /// Turn on indexing
        /// </summary>
        /// <returns>This</returns>
        public override IReference<ClassType, string> TurnOnIndexing()
        {
            this.Index = true;
            return (IReference<ClassType, string>)this;
        }

        /// <summary>
        /// Turn on auto increment
        /// </summary>
        /// <returns>This</returns>
        public override IReference<ClassType, string> TurnOnAutoIncrement()
        {
            this.AutoIncrement = true;
            return (IReference<ClassType, string>)this;
        }

        /// <summary>
        /// Set field name
        /// </summary>
        /// <param name="FieldName">Field name</param>
        /// <returns>This</returns>
        public override IReference<ClassType, string> SetFieldName(string FieldName)
        {
            this.FieldName = FieldName;
            return (IReference<ClassType, string>)this;
        }

        /// <summary>
        /// Set the table name
        /// </summary>
        /// <param name="TableName">Table name</param>
        /// <returns>This</returns>
        public override IReference<ClassType, string> SetTableName(string TableName)
        {
            this.TableName = TableName;
            return (IReference<ClassType, string>)this;
        }

        /// <summary>
        /// Turn on cascade
        /// </summary>
        /// <returns>This</returns>
        public override IReference<ClassType, string> TurnOnCascade()
        {
            this.Cascade = true;
            return (IReference<ClassType, string>)this;
        }

        /// <summary>
        /// Set max length
        /// </summary>
        /// <param name="MaxLength">Max length</param>
        /// <returns>This</returns>
        public override IReference<ClassType, string> SetMaxLength(int MaxLength)
        {
            this.MaxLength = MaxLength;
            return (IReference<ClassType, string>)this;
        }

        #endregion
    }
}