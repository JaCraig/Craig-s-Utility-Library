using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Utilities.DataTypes.AOP.Generators.BaseClasses;
using Utilities.DataTypes.AOP.Interfaces;

namespace Utilities.DataTypes.AOP.Generators
{
    /// <summary>
    /// Class generator interface
    /// </summary>
    /// <seealso cref="Utilities.DataTypes.AOP.Interfaces.IClassGenerator"/>
    public class ClassGenerator : GeneratorBase, IClassGenerator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClassGenerator"/> class.
        /// </summary>
        /// <param name="declaringType">Type of the declaring.</param>
        /// <param name="aspects">The aspects.</param>
        public ClassGenerator(Type declaringType, ConcurrentBag<IAspect> aspects)
        {
            Aspects = aspects;
            DeclaringType = declaringType;
        }

        /// <summary>
        /// Gets the aspects.
        /// </summary>
        /// <value>The aspects.</value>
        protected ConcurrentBag<IAspect> Aspects { get; private set; }

        /// <summary>
        /// Gets or sets the type of the declaring.
        /// </summary>
        /// <value>The type of the declaring.</value>
        protected Type DeclaringType { get; private set; }

        /// <summary>
        /// Generates the specified type.
        /// </summary>
        /// <param name="namespace">The namespace.</param>
        /// <param name="className">Name of the class.</param>
        /// <param name="usings">The usings.</param>
        /// <param name="interfaces">The interfaces.</param>
        /// <param name="assembliesUsing">The assemblies using.</param>
        /// <returns>The string representation of the generated class</returns>
        public string Generate(string @namespace, string className, List<string> usings, List<Type> interfaces, List<Assembly> assembliesUsing)
        {
            var Builder = new StringBuilder();
            Builder.AppendLineFormat(@"namespace {1}
{{
    {0}

    public class {2} : {3}{4} {5}
    {{
", usings.ToString(x => "using " + x + ";", "\r\n"),
 @namespace,
 className,
 DeclaringType.FullName.Replace("+", "."),
 interfaces.Count > 0 ? "," : "", interfaces.ToString(x => x.Name));
            if (DeclaringType.HasDefaultConstructor() || DeclaringType.IsInterface)
            {
                Builder.AppendLine(new ConstructorGenerator(DeclaringType.GetConstructors(BindingFlags.Public | BindingFlags.Instance)
                                                                         .FirstOrDefault(x => x.GetParameters().Length == 0),
                                                            DeclaringType)
                                   .Generate(assembliesUsing, Aspects));
            }

            Aspects.ForEach(x => Builder.AppendLine(x.SetupInterfaces(DeclaringType)));

            Type TempType = DeclaringType;
            var MethodsAlreadyDone = new List<string>();
            while (TempType != null)
            {
                foreach (PropertyInfo Property in TempType.GetProperties(BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Instance))
                {
                    var GetMethodInfo = Property.GetGetMethod();
                    var SetMethodInfo = Property.GetSetMethod();
                    if (!MethodsAlreadyDone.Contains("get_" + Property.Name)
                        && !MethodsAlreadyDone.Contains("set_" + Property.Name)
                        && GetMethodInfo != null
                        && GetMethodInfo.IsVirtual
                        && SetMethodInfo != null
                        && SetMethodInfo.IsPublic
                        && !GetMethodInfo.IsFinal
                        && Property.GetIndexParameters().Length == 0)
                    {
                        Builder.AppendLine(new PropertyGenerator(Property).Generate(assembliesUsing, Aspects));
                        MethodsAlreadyDone.Add(GetMethodInfo.Name);
                        MethodsAlreadyDone.Add(SetMethodInfo.Name);
                    }
                    else if (!MethodsAlreadyDone.Contains("get_" + Property.Name)
                        && GetMethodInfo != null
                        && GetMethodInfo.IsVirtual
                        && SetMethodInfo == null
                        && !GetMethodInfo.IsFinal
                        && Property.GetIndexParameters().Length == 0)
                    {
                        Builder.AppendLine(new PropertyGenerator(Property).Generate(assembliesUsing, Aspects));
                        MethodsAlreadyDone.Add(GetMethodInfo.Name);
                    }
                    else
                    {
                        if (GetMethodInfo != null)
                            MethodsAlreadyDone.Add(GetMethodInfo.Name);
                        if (SetMethodInfo != null)
                            MethodsAlreadyDone.Add(SetMethodInfo.Name);
                    }
                }
                foreach (MethodInfo Method in TempType.GetMethods(BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Instance)
                                                        .Where(x => !MethodsAlreadyDone.Contains(x.Name)
                                                            && x.IsVirtual
                                                            && !x.IsFinal
                                                            && !x.IsPrivate
                                                            && !x.Name.StartsWith("add_", StringComparison.InvariantCultureIgnoreCase)
                                                            && !x.Name.StartsWith("remove_", StringComparison.InvariantCultureIgnoreCase)
                                                            && !x.IsGenericMethod))
                {
                    Builder.AppendLine(new MethodGenerator(Method).Generate(assembliesUsing, Aspects));
                    MethodsAlreadyDone.Add(Method.Name);
                }
                TempType = TempType.BaseType;
                if (TempType == typeof(object))
                    break;
            }
            Builder.AppendLine(@"   }
}");
            return Builder.ToString();
        }
    }
}