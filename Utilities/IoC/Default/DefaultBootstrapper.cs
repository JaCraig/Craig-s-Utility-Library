/*
Copyright (c) 2013 <a href="http://www.gutgames.com">James Craig</a>

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
using Utilities.IoC.BaseClasses;
using Utilities.IoC.Default.Interfaces;
#endregion

namespace Utilities.IoC.Default
{
    /// <summary>
    /// Default bootstrapper if one isn't found
    /// </summary>
    public class DefaultBootstrapper : BootstrapperBase<IDictionary<Tuple<Type, string>, ITypeBuilder>>
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public DefaultBootstrapper()
            : base()
        {
            _AppContainer = new Dictionary<Tuple<Type, string>, ITypeBuilder>();
        }

        #endregion

        #region Properties

        private IDictionary<Tuple<Type, string>, ITypeBuilder> _AppContainer = null;

        /// <summary>
        /// App container
        /// </summary>
        protected override IDictionary<Tuple<Type, string>, ITypeBuilder> AppContainer
        {
            get { return _AppContainer; }
        }

        /// <summary>
        /// Name of the bootstrapper
        /// </summary>
        public override string Name
        {
            get { return "Default bootstrapper"; }
        }

        #endregion

        #region Functions

        #region Register

        /// <summary>
        /// Registers an object
        /// </summary>
        /// <typeparam name="T">Type to register</typeparam>
        /// <param name="Object">Object to return</param>
        /// <param name="Name">Name to associate with it</param>
        public override void Register<T>(T Object, string Name="")
        {
            Register(() => Object, Name);
        }

        /// <summary>
        /// Registers an object
        /// </summary>
        /// <typeparam name="T">Type to register</typeparam>
        /// <param name="Name">Name to associate with it</param>
        public override void Register<T>(string Name="")
        {
            Register(() => new T(), Name);
        }

        /// <summary>
        /// Registers two types together
        /// </summary>
        /// <typeparam name="T1">Interface/base class</typeparam>
        /// <typeparam name="T2">Implementation</typeparam>
        /// <param name="Name">Name to associate with it</param>
        public override void Register<T1, T2>(string Name="")
        {
            Register<T1>(() => new T2(), Name);
        }

        /// <summary>
        /// Registers a function with a type
        /// </summary>
        /// <typeparam name="T">Type to register</typeparam>
        /// <param name="Function">Function used to create the type</param>
        /// <param name="Name">Name to associate with the function</param>
        public override void Register<T>(Func<T> Function, string Name="")
        {
            Tuple<Type, string> Key = new Tuple<Type, string>(typeof(T), Name);
            if (AppContainer.ContainsKey(Key))
            {
                AppContainer[Key] = new TypeBuilder<T>(Function);
            }
            else
            {
                AppContainer.Add(Key, new TypeBuilder<T>(Function));
            }
        }

        #endregion

        #region Resolve

        /// <summary>
        /// Resolves an object based on the type specified
        /// </summary>
        /// <typeparam name="T">Type of object to return</typeparam>
        /// <param name="DefaultObject">Default value if type is not registered or error occurs</param>
        /// <returns>Object of the type specified</returns>
        public override T Resolve<T>(T DefaultObject = default(T))
        {
            return (T)Resolve(typeof(T), "", DefaultObject);
        }
        /// <summary>
        /// Resolves an object based on the type specified
        /// </summary>
        /// <typeparam name="T">Type of object to return</typeparam>
        /// <param name="DefaultObject">Default value if type is not registered or error occurs</param>
        /// <param name="Name">Name of the object to return</param>
        /// <returns>Object of the type specified</returns>
        public override T Resolve<T>(string Name, T DefaultObject = default(T))
        {
            return (T)Resolve(typeof(T), Name, DefaultObject);
        }

        /// <summary>
        /// Resolves an object based on the type specified
        /// </summary>
        /// <param name="ObjectType">Object type</param>
        /// <param name="DefaultObject">Default value if type is not registered or error occurs</param>
        /// <returns>Object of the type specified</returns>
        public override object Resolve(Type ObjectType, object DefaultObject = null)
        {
            return Resolve(ObjectType, "", DefaultObject);
        }

        /// <summary>
        /// Resolves an object based on the type specified
        /// </summary>
        /// <param name="Name">Name of the object to return</param>
        /// <param name="ObjectType">Object type</param>
        /// <param name="DefaultObject">Default value if type is not registered or error occurs</param>
        /// <returns>Object of the type specified</returns>
        public override object Resolve(Type ObjectType, string Name, object DefaultObject = null)
        {
            try
            {
                Tuple<Type, string> Key = new Tuple<Type, string>(ObjectType, Name);
                if (!AppContainer.ContainsKey(Key))
                    return DefaultObject;
                return AppContainer[Key].Create();
            }
            catch { return DefaultObject; }
        }

        #endregion

        #region ResolveAll

        /// <summary>
        /// Resolves all objects of the type specified
        /// </summary>
        /// <typeparam name="T">Type of objects to return</typeparam>
        /// <returns>An IEnumerable containing all objects of the type specified</returns>
        public override IEnumerable<T> ResolveAll<T>()
        {
            List<T> ReturnValues = new List<T>();
            Type TypeWanted = typeof(T);
            foreach (Tuple<Type, string> Key in _AppContainer.Keys)
            {
                if (Key.Item1 == TypeWanted)
                {
                    ReturnValues.Add((T)Resolve(Key.Item1, Key.Item2, default(T)));
                }
            }
            return ReturnValues;
        }

        /// <summary>
        /// Resolves all objects of the type specified
        /// </summary>
        /// <param name="ObjectType">Object type to return</param>
        /// <returns>An IEnumerable containing all objects of the type specified</returns>
        public override IEnumerable<object> ResolveAll(Type ObjectType)
        {
            List<object> ReturnValues = new List<object>();
            foreach (Tuple<Type, string> Key in _AppContainer.Keys)
            {
                if (Key.Item1 == ObjectType)
                {
                    ReturnValues.Add(Resolve(Key.Item1, Key.Item2, null));
                }
            }
            return ReturnValues;
        }

        #endregion

        #region Dispose

        /// <summary>
        /// Disposes of the object
        /// </summary>
        /// <param name="Managed">Not used</param>
        protected override void Dispose(bool Managed)
        {
            if (_AppContainer != null)
            {
                foreach (IDisposable Item in _AppContainer.Values.OfType<IDisposable>())
                {
                    Item.Dispose();
                }
                _AppContainer.Clear();
                _AppContainer = null;
            }
            base.Dispose(Managed);
        }

        #endregion

        #endregion
    }
}