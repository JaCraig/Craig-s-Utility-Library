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
using System.Reflection;
using Utilities.ORM.QueryProviders;
using Utilities.ORM.Mapping;
using Utilities.ORM.Mapping.Interfaces;
using Utilities.ORM.Aspect;
using Utilities.ORM.Database;
#endregion

namespace Utilities.ORM
{
    /// <summary>
    /// Main ORM class
    /// </summary>
    public class ORM
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Assembly">Assembly containing business object mappings</param>
        public ORM(Assembly Assembly)
        {
            Setup(Assembly);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Assemblies">Assemblies containing business object mappings</param>
        public ORM(Assembly[] Assemblies)
        {
            Setup(Assemblies);
        }

        #endregion

        #region Functions

        private void Setup(Assembly[] Assemblies)
        {
            MappingManager = new MappingManager(Assemblies);
            QueryProvider = new Default(Assemblies);
            foreach (Type Key in MappingManager.Mappings.Keys)
                foreach (IMapping Mapping in MappingManager.Mappings[Key])
                    QueryProvider.AddMapping(Mapping);
            Utilities.Reflection.AOP.AOPManager Manager = new Reflection.AOP.AOPManager();
            Manager.AddAspect(new ORMAspect(MappingManager.Mappings));
            DatabaseManager = new DatabaseManager(QueryProvider.Mappings);
            DatabaseManager.Setup();
        }

        private void Setup(Assembly Assembly)
        {
            MappingManager = new MappingManager(Assembly);
            QueryProvider = new Default(Assembly);
            foreach (Type Key in MappingManager.Mappings.Keys)
                foreach (IMapping Mapping in MappingManager.Mappings[Key])
                    QueryProvider.AddMapping(Mapping);
            Utilities.Reflection.AOP.AOPManager Manager = new Reflection.AOP.AOPManager();
            Manager.AddAspect(new ORMAspect(MappingManager.Mappings));
            DatabaseManager = new DatabaseManager(QueryProvider.Mappings);
            DatabaseManager.Setup();
        }

        /// <summary>
        /// Creates a session to allow you to make queries to the system
        /// </summary>
        /// <returns>A session object</returns>
        public static Session CreateSession()
        {
            return new Session(QueryProvider);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Query provider
        /// </summary>
        private static Default QueryProvider { get; set; }

        /// <summary>
        /// Mapping manager
        /// </summary>
        private static MappingManager MappingManager { get; set; }

        /// <summary>
        /// Database manager
        /// </summary>
        private static DatabaseManager DatabaseManager { get; set; }

        #endregion
    }
}