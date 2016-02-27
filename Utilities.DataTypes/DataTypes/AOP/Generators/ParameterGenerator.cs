using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Reflection;
using Utilities.DataTypes.AOP.Generators.BaseClasses;

namespace Utilities.DataTypes.AOP.Generators
{
    /// <summary>
    /// Parameter generator
    /// </summary>
    /// <seealso cref="Utilities.DataTypes.AOP.Generators.BaseClasses.GeneratorBase"/>
    public class ParameterGenerator : GeneratorBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParameterGenerator"/> class.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        public ParameterGenerator(ParameterInfo parameter)
        {
            Contract.Requires(parameter != null);
            Parameter = parameter;
        }

        /// <summary>
        /// Gets or sets the parameter.
        /// </summary>
        /// <value>The parameter.</value>
        public ParameterInfo Parameter { get; set; }

        /// <summary>
        /// Generates the specified assemblies using.
        /// </summary>
        /// <param name="assembliesUsing">The assemblies using.</param>
        /// <returns>The string version of the parameter</returns>
        public string Generate(List<Assembly> assembliesUsing)
        {
            if (assembliesUsing != null)
            {
                assembliesUsing.AddIfUnique(GetAssemblies(Parameter.ParameterType));
            }
            return ToString();
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        public override string ToString()
        {
            return (Parameter.IsOut ? "out " : "") + Parameter.ParameterType.GetName() + " " + Parameter.Name;
        }
    }
}