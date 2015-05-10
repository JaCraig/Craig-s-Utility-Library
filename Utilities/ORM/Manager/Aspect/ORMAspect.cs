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
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Text;
using Utilities.DataTypes;
using Utilities.DataTypes.AOP.Interfaces;
using Utilities.ORM.Manager.Aspect.Interfaces;
using Utilities.ORM.Manager.Mapper.Interfaces;

namespace Utilities.ORM.Aspect
{
    /// <summary>
    /// ORM Aspect (used internally)
    /// </summary>
    public class ORMAspect : IAspect
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ORMAspect()
        {
        }

        /// <summary>
        /// Mapper
        /// </summary>
        public static Utilities.ORM.Manager.Mapper.Manager Mapper { get; set; }

        /// <summary>
        /// Assemblies using
        /// </summary>
        public ICollection<Assembly> AssembliesUsing { get { return new Assembly[] { typeof(ORMAspect).Assembly }; } }

        /// <summary>
        /// Interfaces using
        /// </summary>
        public ICollection<Type> InterfacesUsing { get { return new Type[] { typeof(IORMObject) }; } }

        /// <summary>
        /// Usings using
        /// </summary>
        public ICollection<string> Usings
        {
            get
            {
                return new string[] {
                    "Utilities.DataTypes",
                    "Utilities.ORM.Manager",
                    "Utilities.ORM.Manager.Aspect.Interfaces",
                    "System.ComponentModel",
                    "System.Runtime.CompilerServices"
                };
            }
        }

        /// <summary>
        /// Fields that have been completed already
        /// </summary>
        private List<IProperty> Fields { get; set; }

        /// <summary>
        /// Sets up the aspect
        /// </summary>
        /// <param name="Object">Object to set up</param>
        public void Setup(object Object)
        {
            IORMObject TempObject = (IORMObject)Object;
            TempObject.Session0 = new Utilities.ORM.Manager.Session();
            TempObject.PropertiesChanged0 = new List<string>();
            TempObject.PropertiesLoaded0 = new List<string>();
            TempObject.PropertyChanged += (object sender, PropertyChangedEventArgs e) =>
            {
                IORMObject x = (IORMObject)sender;
                x.PropertiesChanged0.Add(e.PropertyName);
            };
        }

        /// <summary>
        /// Sets up the default constructor
        /// </summary>
        /// <param name="BaseType">Base type</param>
        /// <returns>The code used</returns>
        public string SetupDefaultConstructor(Type BaseType)
        {
            return "";
        }

        /// <summary>
        /// Sets up the end of a method
        /// </summary>
        /// <param name="Method">Method information</param>
        /// <param name="BaseType">Base type</param>
        /// <param name="ReturnValueName">Return value name</param>
        /// <returns>The code used</returns>
        public string SetupEndMethod(MethodInfo Method, Type BaseType, string ReturnValueName)
        {
            StringBuilder Builder = new StringBuilder();
            if (Mapper[BaseType] != null && Method.Name.StartsWith("get_", StringComparison.Ordinal))
            {
                foreach (IMapping Mapping in Mapper[BaseType])
                {
                    IProperty Property = Mapping.Properties.FirstOrDefault(x => x.Name == Method.Name.Replace("get_", ""));
                    if (Property != null)
                    {
                        if (Property is IManyToOne || Property is IMap)
                            Builder.AppendLine(SetupSingleProperty(ReturnValueName, Property));
                        else if (Property is IIEnumerableManyToOne || Property is IManyToMany)
                            Builder.AppendLine(SetupIEnumerableProperty(ReturnValueName, Property));
                        else if (Property is IListManyToMany || Property is IListManyToOne)
                            Builder.AppendLine(SetupListProperty(ReturnValueName, Property));
                        return Builder.ToString();
                    }
                }
            }
            return Builder.ToString();
        }

        /// <summary>
        /// Sets up exception method
        /// </summary>
        /// <param name="Method">Method information</param>
        /// <param name="BaseType">Base type</param>
        /// <returns>The code used</returns>
        public string SetupExceptionMethod(MethodInfo Method, Type BaseType)
        {
            return "var Exception=CaughtException;";
        }

        /// <summary>
        /// Sets up the interfaces used
        /// </summary>
        /// <param name="Type">The object type</param>
        /// <returns>The code used</returns>
        public string SetupInterfaces(Type Type)
        {
            StringBuilder Builder = new StringBuilder();
            Builder.AppendLine(@"public Session Session0{ get; set; }");
            Builder.AppendLine(@"public IList<string> PropertiesChanged0{ get; set; }");
            Builder.AppendLine(@"public IList<string> PropertiesLoaded0{ get; set; }");
            if (!Type.Is<INotifyPropertyChanged>())
            {
                Builder.AppendLine(@"private PropertyChangedEventHandler propertyChanged_;
public event PropertyChangedEventHandler PropertyChanged
{
    add
    {
        propertyChanged_-=value;
        propertyChanged_+=value;
    }

    remove
    {
        propertyChanged_-=value;
    }
}");
                Builder.AppendLine(@"private void NotifyPropertyChanged0([CallerMemberName]string propertyName="""")
{
    var Handler = propertyChanged_;
    if (Handler != null)
        Handler(this, new PropertyChangedEventArgs(propertyName));
}");
            }
            Builder.AppendLine(SetupFields(Type));
            return Builder.ToString();
        }

        /// <summary>
        /// Sets up the start of the method
        /// </summary>
        /// <param name="Method">Method information</param>
        /// <param name="BaseType">Base type</param>
        /// <returns>The code used</returns>
        public string SetupStartMethod(MethodInfo Method, Type BaseType)
        {
            StringBuilder Builder = new StringBuilder();
            if (Mapper[BaseType] != null && Method.Name.StartsWith("set_", StringComparison.Ordinal))
            {
                foreach (IMapping Mapping in Mapper[BaseType])
                {
                    IProperty Property = Mapping.Properties.FirstOrDefault(x => x.Name == Method.Name.Replace("set_", ""));
                    if (Fields.Contains(Property))
                    {
                        Builder.AppendLineFormat("{0}=value;", Property.DerivedFieldName)
                            .AppendLine("NotifyPropertyChanged0();");
                    }
                }
            }
            return Builder.ToString();
        }

        private static string SetupIEnumerableProperty(string ReturnValueName, IProperty Property)
        {
            Contract.Requires<ArgumentNullException>(Property != null, "Property");
            Contract.Requires<ArgumentNullException>(Property.Mapping != null, "Property.Mapping");
            Contract.Requires<ArgumentNullException>(Property.Mapping.ObjectType != null, "Property.Mapping.ObjectType");
            StringBuilder Builder = new StringBuilder();
            Builder.AppendLineFormat("if(!{0}&&Session0!=null)", Property.DerivedFieldName + "Loaded")
                .AppendLine("{")
                .AppendLineFormat("{0}=Session0.LoadProperties<{1},{2}>(this,\"{3}\");",
                        Property.DerivedFieldName,
                        Property.Mapping.ObjectType.GetName(),
                        Property.Type.GetName(),
                        Property.Name)
                .AppendLineFormat("{0}=true;", Property.DerivedFieldName + "Loaded")
                .AppendLineFormat("PropertiesLoaded0.Add(\"{0}\");", Property.Name)
                .AppendLineFormat("((ObservableList<{1}>){0}).CollectionChanged += (x, y) => NotifyPropertyChanged0();", Property.DerivedFieldName, Property.Type.GetName())
                .AppendLine("}")
                .AppendLineFormat("{0}={1};",
                    ReturnValueName,
                    Property.DerivedFieldName);
            return Builder.ToString();
        }

        private static string SetupListProperty(string ReturnValueName, IProperty Property)
        {
            Contract.Requires<ArgumentNullException>(Property != null, "Property");
            Contract.Requires<ArgumentNullException>(Property.Mapping != null, "Property.Mapping");
            Contract.Requires<ArgumentNullException>(Property.Mapping.ObjectType != null, "Property.Mapping.ObjectType");
            StringBuilder Builder = new StringBuilder();
            Builder.AppendLineFormat("if(!{0}&&Session0!=null)", Property.DerivedFieldName + "Loaded")
                .AppendLine("{")
                .AppendLineFormat("{0}=Session0.LoadProperties<{1},{2}>(this,\"{3}\");",
                        Property.DerivedFieldName,
                        Property.Mapping.ObjectType.GetName(),
                        Property.Type.GetName(),
                        Property.Name)
                .AppendLineFormat("{0}=true;", Property.DerivedFieldName + "Loaded")
                .AppendLineFormat("PropertiesLoaded0.Add(\"{0}\");", Property.Name)
                .AppendLineFormat("((ObservableList<{1}>){0}).CollectionChanged += (x, y) => NotifyPropertyChanged0();", Property.DerivedFieldName, Property.Type.GetName())
                .AppendLine("}")
                .AppendLineFormat("{0}={1};",
                    ReturnValueName,
                    Property.DerivedFieldName);
            return Builder.ToString();
        }

        private static string SetupSingleProperty(string ReturnValueName, IProperty Property)
        {
            Contract.Requires<ArgumentNullException>(Property != null, "Property");
            Contract.Requires<ArgumentNullException>(Property.Mapping != null, "Property.Mapping");
            Contract.Requires<ArgumentNullException>(Property.Mapping.ObjectType != null, "Property.Mapping.ObjectType");
            StringBuilder Builder = new StringBuilder();
            Builder.AppendLineFormat("if(!{0}&&Session0!=null)", Property.DerivedFieldName + "Loaded")
                .AppendLine("{")
                .AppendLineFormat("{0}=Session0.LoadProperty<{1},{2}>(this,\"{3}\");",
                        Property.DerivedFieldName,
                        Property.Mapping.ObjectType.GetName(),
                        Property.Type.GetName(),
                        Property.Name)
                .AppendLineFormat("{0}=true;", Property.DerivedFieldName + "Loaded")
                .AppendLineFormat("PropertiesLoaded0.Add(\"{0}\");", Property.Name)
                .AppendLineFormat("if({0} as INotifyPropertyChanged!=null)", Property.DerivedFieldName)
                .AppendLine("{")
                .AppendLineFormat("({0} as INotifyPropertyChanged).PropertyChanged+=(x,y)=>NotifyPropertyChanged0();", Property.DerivedFieldName)
                .AppendLine("}")
                .AppendLine("}")
                .AppendLineFormat("{0}={1};",
                    ReturnValueName,
                    Property.DerivedFieldName);
            return Builder.ToString();
        }

        private string SetupFields(Type Type)
        {
            Fields = new List<IProperty>();
            StringBuilder Builder = new StringBuilder();
            if (Mapper[Type] != null)
            {
                foreach (IMapping Mapping in Mapper[Type])
                {
                    foreach (IProperty Property in Mapping.Properties)
                    {
                        if (Property is IManyToOne || Property is IMap)
                        {
                            if (Fields.FirstOrDefault(x => x.DerivedFieldName == Property.DerivedFieldName) == null)
                            {
                                Fields.Add(Property);
                                Builder.AppendLineFormat("private {0} {1};", Property.Type.GetName(), Property.DerivedFieldName);
                                Builder.AppendLineFormat("private bool {0};", Property.DerivedFieldName + "Loaded");
                            }
                        }
                        else if (Property is IIEnumerableManyToOne || Property is IManyToMany)
                        {
                            if (Fields.FirstOrDefault(x => x.DerivedFieldName == Property.DerivedFieldName) == null)
                            {
                                Fields.Add(Property);
                                Builder.AppendLineFormat("private {0} {1};", typeof(IEnumerable<>).MakeGenericType(Property.Type).GetName(), Property.DerivedFieldName);
                                Builder.AppendLineFormat("private bool {0};", Property.DerivedFieldName + "Loaded");
                            }
                        }
                        else if (Property is IListManyToOne || Property is IListManyToMany)
                        {
                            if (Fields.FirstOrDefault(x => x.DerivedFieldName == Property.DerivedFieldName) == null)
                            {
                                Fields.Add(Property);
                                Builder.AppendLineFormat("private {0} {1};", typeof(List<>).MakeGenericType(Property.Type).GetName(), Property.DerivedFieldName);
                                Builder.AppendLineFormat("private bool {0};", Property.DerivedFieldName + "Loaded");
                            }
                        }
                    }
                }
            }
            return Builder.ToString();
        }
    }
}