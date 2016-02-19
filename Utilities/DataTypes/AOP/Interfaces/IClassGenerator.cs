using System;
using System.Collections.Generic;
using System.Reflection;

namespace Utilities.DataTypes.AOP.Interfaces
{
    /// <summary>
    /// Class generator interface
    /// </summary>
    public interface IClassGenerator
    {
        /// <summary>
        /// Generates the specified type.
        /// </summary>
        /// <param name="namespace">The namespace.</param>
        /// <param name="className">Name of the class.</param>
        /// <param name="usings">The usings.</param>
        /// <param name="interfaces">The interfaces.</param>
        /// <param name="assembliesUsing">The assemblies using.</param>
        /// <returns>The string representation of the generated class</returns>
        string Generate(string @namespace, string className, List<string> usings, List<Type> interfaces, List<Assembly> assembliesUsing);
    }
}