using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Utilities.DataTypes.AOP.Generators.BaseClasses;
using Utilities.DataTypes.AOP.Interfaces;

namespace Utilities.DataTypes.AOP.Generators
{
    /// <summary>
    /// Constructor generator
    /// </summary>
    /// <seealso cref="Utilities.DataTypes.AOP.Generators.BaseClasses.GeneratorBase"/>
    public class ConstructorGenerator : GeneratorBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConstructorGenerator"/> class.
        /// </summary>
        /// <param name="constructor">The constructor.</param>
        /// <param name="declaringType">Type of the declaring.</param>
        public ConstructorGenerator(ConstructorInfo constructor, Type declaringType)
        {
            Constructor = constructor;
            DeclaringType = declaringType;
        }

        /// <summary>
        /// Gets or sets the constructor.
        /// </summary>
        /// <value>The constructor.</value>
        public ConstructorInfo Constructor { get; set; }

        /// <summary>
        /// Gets or sets the type of the declaring.
        /// </summary>
        /// <value>The type of the declaring.</value>
        public Type DeclaringType { get; set; }

        /// <summary>
        /// Generates the specified assemblies using.
        /// </summary>
        /// <param name="assembliesUsing">The assemblies using.</param>
        /// <param name="aspects">The aspects.</param>
        /// <returns></returns>
        public string Generate(List<Assembly> assembliesUsing, IEnumerable<IAspect> aspects)
        {
            StringBuilder Builder = new StringBuilder();
            Builder.AppendLineFormat(@"
                public {0}()
                    {1}
                {{
                    {2}
                }}",
                DeclaringType.Name + "Derived",
                DeclaringType.IsInterface ? "" : ":base()",
                aspects.ToString(x => x.SetupDefaultConstructor(DeclaringType)));
            return Builder.ToString();
        }
    }
}