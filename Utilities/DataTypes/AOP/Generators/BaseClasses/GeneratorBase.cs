using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Utilities.DataTypes.AOP.Generators.BaseClasses
{
    /// <summary>
    /// Generator base class
    /// </summary>
    public abstract class GeneratorBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GeneratorBase"/> class.
        /// </summary>
        protected GeneratorBase()
        {
        }

        /// <summary>
        /// Gets the assemblies associated with the type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The assemblies associated with the type</returns>
        protected Assembly[] GetAssemblies(Type type)
        {
            var Types = new List<Assembly>();
            Type TempType = type;
            while (TempType != null)
            {
                Types.AddIfUnique(TempType.Assembly.GetReferencedAssemblies().ForEach(x =>
                {
                    try
                    {
                        return Assembly.Load(x);
                    }
                    catch
                    {
                        return null;
                    }
                }).Where(x => x != null));
                Types.AddIfUnique(TempType.Assembly);
                TempType.GetInterfaces().ForEach(x => Types.AddIfUnique(GetAssembliesSimple(x)));
                TempType.GetEvents().ForEach(x => Types.AddIfUnique(GetAssembliesSimple(x.EventHandlerType)));
                TempType.GetFields().ForEach(x => Types.AddIfUnique(GetAssembliesSimple(x.FieldType)));
                TempType.GetProperties().ForEach(x => Types.AddIfUnique(GetAssembliesSimple(x.PropertyType)));
                TempType.GetMethods().ForEach(x =>
                {
                    Types.AddIfUnique(GetAssembliesSimple(x.ReturnType));
                    x.GetParameters().ForEach(y => Types.AddIfUnique(GetAssembliesSimple(y.ParameterType)));
                });
                TempType = TempType.BaseType;
                if (TempType == typeof(object))
                    break;
            }
            return Types.ToArray();
        }

        /// <summary>
        /// Gets the assemblies associated with the type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The assemblies associated with the type</returns>
        protected Assembly[] GetAssembliesSimple(Type type)
        {
            var Types = new List<Assembly>();
            Type TempType = type;
            while (TempType != null)
            {
                Types.AddIfUnique(TempType.Assembly);
                TempType.GetInterfaces().ForEach(x => Types.AddIfUnique(GetAssembliesSimple(x)));
                TempType = TempType.BaseType;
                if (TempType == typeof(object))
                    break;
            }
            return Types.ToArray();
        }
    }
}