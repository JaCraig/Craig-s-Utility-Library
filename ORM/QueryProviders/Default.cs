/*
Copyright (c) 2011 <a href="http://www.gutgames.com">James Craig</a>

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
using System.Linq;
using System.Text;
using System.Reflection;
using Utilities.ORM.QueryProviders.Interfaces;
using Utilities.ORM.Mapping.Interfaces;
using Utilities.DataTypes;
using Utilities.SQL.MicroORM;
#endregion

namespace Utilities.ORM.QueryProviders
{
    /// <summary>
    /// Default query provider
    /// </summary>
    public class Default
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="AssemblyUsing">Assembly using</param>
        public Default(Assembly AssemblyUsing)
        {
            Setup(AssemblyUsing);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="AssembliesUsing">Assemblies using</param>
        public Default(Assembly[] AssembliesUsing)
        {
            foreach (Assembly Assembly in AssembliesUsing)
            {
                Setup(Assembly);
            }
        }

        #endregion

        #region Functions

        /// <summary>
        /// Sets up the system
        /// </summary>
        /// <param name="AssemblyUsing">Assembly to set up</param>
        private void Setup(Assembly AssemblyUsing)
        {
            if (Databases == null)
                Databases = new System.Collections.Generic.List<IDatabase>();
            System.Collections.Generic.List<Type> Types = Utilities.Reflection.Reflection.GetTypes(AssemblyUsing, typeof(IDatabase));
            foreach (Type Type in Types)
            {
                Type BaseType = Type.BaseType;
                IDatabase TempObject = (IDatabase)Activator.CreateInstance(Type);
                if (!string.IsNullOrEmpty(TempObject.ConnectionString))
                {
                    MicroORM.Database(TempObject.ConnectionString, TempObject.Name);
                    Databases.Add(TempObject);
                }
            }
        }

        /// <summary>
        /// Adds a mapping
        /// </summary>
        /// <param name="Mapping">Mapping to add</param>
        public virtual void AddMapping(IMapping Mapping)
        {
            if (Mappings == null)
                Mappings = new ListMapping<IDatabase, IMapping>();
            IDatabase Database = Databases.First(x => Utilities.Reflection.Reflection.IsOfType(x, Mapping.DatabaseConfigType)
                                                        && !string.IsNullOrEmpty(x.ConnectionString));
            Mapping.AddToQueryProvider(Database);
            Mappings.Add(Database, Mapping);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Mappings associated to databases
        /// </summary>
        public virtual ListMapping<IDatabase, IMapping> Mappings { get; set; }

        /// <summary>
        /// List of database configurations
        /// </summary>
        public virtual System.Collections.Generic.List<IDatabase> Databases { get; set; }

        #endregion
    }
}