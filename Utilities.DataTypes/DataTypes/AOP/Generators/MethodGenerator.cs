using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Reflection;
using System.Text;
using Utilities.DataTypes.AOP.Generators.BaseClasses;
using Utilities.DataTypes.AOP.Interfaces;

namespace Utilities.DataTypes.AOP.Generators
{
    /// <summary>
    /// Method generator
    /// </summary>
    /// <seealso cref="Utilities.DataTypes.AOP.Generators.BaseClasses.GeneratorBase"/>
    public class MethodGenerator : GeneratorBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MethodGenerator"/> class.
        /// </summary>
        /// <param name="methodInfo">The method information.</param>
        public MethodGenerator(MethodInfo methodInfo)
        {
            Contract.Requires(methodInfo != null);
            MethodInfo = methodInfo;
            DeclaringType = MethodInfo.DeclaringType;
        }

        /// <summary>
        /// Gets or sets the type of the declaring.
        /// </summary>
        /// <value>The type of the declaring.</value>
        private Type DeclaringType { get; set; }

        /// <summary>
        /// Gets or sets the method information.
        /// </summary>
        /// <value>The method information.</value>
        private MethodInfo MethodInfo { get; set; }

        /// <summary>
        /// Generates this instance.
        /// </summary>
        /// <param name="assembliesUsing">The assemblies using.</param>
        /// <param name="aspects">The aspects.</param>
        /// <returns>The generated string of the method</returns>
        public string Generate(List<Assembly> assembliesUsing, IEnumerable<IAspect> aspects)
        {
            aspects = aspects ?? new List<IAspect>();
            StringBuilder Builder = new StringBuilder();
            if (assembliesUsing != null)
                assembliesUsing.AddIfUnique(GetAssemblies(MethodInfo.ReturnType));
            Builder.AppendLineFormat(@"
        {0}
        {{
            {1}
        }}",
            ToString(),
            SetupMethod(aspects));
            return Builder.ToString();
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        public override string ToString()
        {
            return string.Format(@"{4} {5} {0} {1} {2}({3})",
                MethodInfo.IsStatic ? "static" : "",
                MethodInfo.ReturnType.GetName(),
            MethodInfo.Name,
            MethodInfo.GetParameters().ToString(x => new ParameterGenerator(x).Generate(null)),
            "public",
            (MethodInfo.IsAbstract | MethodInfo.IsVirtual) & !DeclaringType.IsInterface ? "override" : "");
        }

        private string SetupMethod(IEnumerable<IAspect> aspects)
        {
            if (MethodInfo == null)
                return "";
            var Builder = new StringBuilder();
            string BaseMethodName = MethodInfo.Name;
            string ReturnValue = MethodInfo.ReturnType != typeof(void) ? "FinalReturnValue" : "";
            string BaseCall = "";
            if (!MethodInfo.IsAbstract & !DeclaringType.IsInterface)
            {
                BaseCall = string.IsNullOrEmpty(ReturnValue) ? "base." + BaseMethodName + "(" : ReturnValue + "=base." + BaseMethodName + "(";
                ParameterInfo[] Parameters = MethodInfo.GetParameters();
                BaseCall += Parameters.Length > 0 ? Parameters.ToString(x => (x.IsOut ? "out " : "") + x.Name) : "";
                BaseCall += ");\r\n";
            }
            else if (!string.IsNullOrEmpty(ReturnValue))
            {
                BaseCall = ReturnValue + "=default(" + MethodInfo.ReturnType.Name + ");\r\n";
            }
            Builder.AppendLineFormat(@"
                try
                {{
                    {0}
                    {1}
                    {2}
                    {3}
                    {4}
                }}
                catch(Exception CaughtException)
                {{
                    {5}
                    throw;
                }}",
                MethodInfo.ReturnType != typeof(void) ? MethodInfo.ReturnType.GetName() + " " + ReturnValue + ";" : "",
                aspects.ForEach(x => x.SetupStartMethod(MethodInfo, DeclaringType)).ToString(x => x, "\r\n"),
                BaseCall,
                aspects.ForEach(x => x.SetupEndMethod(MethodInfo, DeclaringType, ReturnValue)).ToString(x => x, "\r\n"),
                string.IsNullOrEmpty(ReturnValue) ? "" : "return " + ReturnValue + ";",
                aspects.ForEach(x => x.SetupExceptionMethod(MethodInfo, DeclaringType)).ToString(x => x, "\r\n"));
            return Builder.ToString();
        }
    }
}