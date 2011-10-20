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
using Utilities.IoC.Mappings;
using Utilities.IoC.Mappings.Interfaces;
using System.Reflection;
using Utilities.IoC.Mappings.Attributes;
using Utilities.IoC.Providers.BaseClasses;
using Utilities.DataTypes.ExtensionMethods;
#endregion

namespace Utilities.IoC.Providers.Implementations
{
    /// <summary>
    /// Standard implementation class
    /// </summary>
    public class Standard : BaseImplementation
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ImplementationType">Implementation type</param>
        /// <param name="MappingManager">Mapping manager</param>
        public Standard(Type ImplementationType, MappingManager MappingManager)
        {
            this.ReturnType = ImplementationType;
            this.MappingManager = MappingManager;
        }

        #endregion

        #region Functions

        public override object Create()
        {
            ConstructorInfo Constructor = Utils.ConstructorList.ChooseConstructor(ReturnType, MappingManager);
            object Instance = CreateInstance(Constructor);
            SetupProperties(Instance);
            SetupMethods(Instance);
            return Instance;
        }

        #region SetupMethods

        private void SetupMethods(object Instance)
        {
            if (Instance == null)
                return;
            Type ObjectType = Instance.GetType();
            MethodInfo[] Methods = ObjectType.GetMethods();
            foreach (MethodInfo Method in Methods)
            {
                if (IsInjectable(Method))
                {
                    ParameterInfo[] Parameters = Method.GetParameters();
                    List<object> ParameterValues = new List<object>();
                    foreach (ParameterInfo Parameter in Parameters)
                    {
                        ParameterValues.Add(CreateInstance(Parameter));
                    }
                    Method.Invoke(Instance, ParameterValues.ToArray());
                }
            }
        }

        #endregion

        #region SetupProperties

        private void SetupProperties(object Instance)
        {
            if (Instance == null)
                return;
            Type ObjectType = Instance.GetType();
            PropertyInfo[] Properties = ObjectType.GetProperties();
            foreach (PropertyInfo Property in Properties)
            {
                if (IsInjectable(Property))
                {
                    Property.SetValue(Instance, CreateInstance(Property), null);
                }
            }
        }

        #endregion

        #region IsInjectable

        private bool IsInjectable(MethodInfo Method)
        {
            return IsInjectable(Method.GetCustomAttributes(false));
        }

        private bool IsInjectable(PropertyInfo Property)
        {
            return IsInjectable(Property.GetCustomAttributes(false));
        }

        private bool IsInjectable(object[] Attributes)
        {
            IEnumerable<Inject> InjectAttributes = Attributes.OfType<Inject>();
            foreach (Inject InjectAttribute in InjectAttributes)
            {
                return true;
            }
            return false;
        }

        #endregion

        #region CreateInstance

        private object CreateInstance(ConstructorInfo Constructor)
        {
            if (Constructor.IsNull())
                return null;
            if (MappingManager.IsNull())
                return null;
            List<object> ParameterValues = new List<object>();
            return Constructor.GetParameters()
                       .ForEach(x => ParameterValues.Add(CreateInstance(x)))
                       .Chain<IEnumerable<ParameterInfo>, object>(x => Constructor.Invoke(x.ToArray()));
        }

        private object CreateInstance(ParameterInfo Parameter)
        {
            return CreateInstance(Parameter.GetCustomAttributes(false), Parameter.ParameterType);
        }

        private object CreateInstance(PropertyInfo Property)
        {
            return CreateInstance(Property.GetCustomAttributes(false), Property.PropertyType);
        }

        private object CreateInstance(object[] Attributes, Type Type)
        {
            if (Attributes.Length > 0)
            {
                foreach (Attribute Attribute in Attributes)
                {
                    object TempObject = GetObject(Type, Attribute.GetType());
                    if (!TempObject.IsNull())
                        return TempObject;
                }
            }
            return GetObject(Type);
        }

        #endregion

        #region GetObject

        private object GetObject(Type Type)
        {
            IMapping Mapping = MappingManager.GetMapping(Type);
            return Mapping.IsNull() ? null : Mapping.Implementation.Create();
        }

        private object GetObject(Type Type, Type AttributeType)
        {
            IMapping Mapping = MappingManager.GetMapping(Type, AttributeType);
            return Mapping.IsNull() ? null : Mapping.Implementation.Create();
        }

        #endregion

        #endregion

        #region Properties

        /// <summary>
        /// Mapping manager
        /// </summary>
        protected virtual MappingManager MappingManager { get; set; }

        #endregion
    }
}
