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
using System.Linq;
using System.Text;
using Utilities.ORM.Mapping.Interfaces;
using System.Linq.Expressions;
using Utilities.ORM.Mapping.PropertyTypes;
using System.Reflection;
using Utilities.DataTypes;
using Utilities.Reflection.ExtensionMethods;
#endregion

namespace Utilities.ORM.Mapping
{
    /// <summary>
    /// Mapping manager
    /// </summary>
    public class MappingManager : IMappingManager
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="AssemblyUsing">Assembly using</param>
        public MappingManager(Assembly AssemblyUsing)
        {
            Setup(AssemblyUsing);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="AssembliesUsing">Assemblies using</param>
        public MappingManager(Assembly[] AssembliesUsing)
        {
            foreach (Assembly Assembly in AssembliesUsing)
            {
                Setup(Assembly);
            }
        }

        #endregion

        #region Functions

        /// <summary>
        /// Sets up the mappings
        /// </summary>
        /// <param name="AssemblyUsing">Assembly using</param>
        private void Setup(Assembly AssemblyUsing)
        {
            if (Mappings == null)
                Mappings = new ListMapping<Type, IMapping>();
            IEnumerable<Type> Types = AssemblyUsing.GetTypes(typeof(IMapping));
            foreach (Type Type in Types)
            {
                Type BaseType = Type.BaseType;
                IMapping TempObject = (IMapping)AssemblyUsing.CreateInstance(Type.FullName);
                TempObject.Manager = this;
                Mappings.Add(BaseType.GetGenericArguments()[0], TempObject);
            }
        }

        /// <summary>
        /// Initializes the mappings
        /// </summary>
        public void Initialize()
        {
            foreach (Type Key in Mappings.Keys)
            {
                foreach (IMapping Mapping in Mappings[Key])
                {
                    Mapping.Initialize();
                }
            }
        }

        #endregion

        #region Properties

        public virtual ListMapping<Type, IMapping> Mappings { get; set; }

        #endregion
    }
}