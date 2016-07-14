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
    /// Property generator
    /// </summary>
    /// <seealso cref="Utilities.DataTypes.AOP.Generators.BaseClasses.GeneratorBase"/>
    public class PropertyGenerator : GeneratorBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyGenerator"/> class.
        /// </summary>
        /// <param name="propertyInfo">The property information.</param>
        public PropertyGenerator(PropertyInfo propertyInfo)
        {
            Contract.Requires(propertyInfo != null);
            PropertyInfo = propertyInfo;
            DeclaringType = PropertyInfo.DeclaringType;
        }

        /// <summary>
        /// Gets or sets the type of the declaring.
        /// </summary>
        /// <value>The type of the declaring.</value>
        public Type DeclaringType { get; set; }

        /// <summary>
        /// Gets or sets the property information.
        /// </summary>
        /// <value>The property information.</value>
        public PropertyInfo PropertyInfo { get; set; }

        /// <summary>
        /// Generates this instance.
        /// </summary>
        /// <param name="assembliesUsing">The assemblies using.</param>
        /// <param name="aspects">The aspects.</param>
        /// <returns>The string version of this property</returns>
        public string Generate(List<Assembly> assembliesUsing, IEnumerable<IAspect> aspects)
        {
            aspects = aspects ?? new List<IAspect>();
            var Builder = new StringBuilder();
            var GetMethodInfo = PropertyInfo.GetGetMethod();
            var SetMethodInfo = PropertyInfo.GetSetMethod();
            if (assembliesUsing != null)
                assembliesUsing.AddIfUnique(GetAssemblies(PropertyInfo.PropertyType));
            if (GetMethodInfo != null && SetMethodInfo != null)
            {
                Builder.AppendLineFormat(@"
        {0}
        {{
            get
            {{
                {1}
            }}
            set
            {{
                {2}
            }}
        }}

        {3}",
                                            ToString(),
                                            SetupMethod(DeclaringType, GetMethodInfo, aspects),
                                            SetupMethod(DeclaringType, SetMethodInfo, aspects),
                                            CreateBackingField(GetMethodInfo.IsAbstract | DeclaringType.IsInterface));
            }
            else if (GetMethodInfo != null)
            {
                Builder.AppendLineFormat(@"
        {0}
        {{
            get
            {{
                {1}
            }}
        }}",
                                            ToString(),
                                            SetupMethod(DeclaringType, GetMethodInfo, aspects));
            }
            return Builder.ToString();
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        public override string ToString()
        {
            var Method = PropertyInfo.GetGetMethod() ?? PropertyInfo.GetSetMethod();
            return string.Format(@"{0} {1} {2} {3}",
                "public",
                (Method.IsAbstract | Method.IsVirtual) & !DeclaringType.IsInterface ? "override" : "",
                Method.ReturnType.GetName(),
                PropertyInfo.Name);
        }

        private string CreateBackingField(bool v)
        {
            if (!v)
                return "";
            return string.Format("{0} {1};\r\n", PropertyInfo.PropertyType.GetName(), "_" + PropertyInfo.Name);
        }

        private string SetupMethod(Type type, MethodInfo methodInfo, IEnumerable<IAspect> aspects)
        {
            if (methodInfo == null)
                return "";
            var Builder = new StringBuilder();
            var BaseMethodName = methodInfo.Name.Replace("get_", "").Replace("set_", "");
            string ReturnValue = methodInfo.ReturnType != typeof(void) ? "FinalReturnValue" : "";
            string BaseCall = "";
            if (!methodInfo.IsAbstract & !DeclaringType.IsInterface)
            {
                BaseCall = string.IsNullOrEmpty(ReturnValue) ? "base." + BaseMethodName : ReturnValue + "=base." + BaseMethodName;
            }
            else
            {
                BaseCall += (string.IsNullOrEmpty(ReturnValue) ? "_" : ReturnValue + "=_") + PropertyInfo.Name;
            }
            var Parameters = methodInfo.GetParameters();
            BaseCall += Parameters.Length > 0 ? "=" + Parameters.ToString(x => x.Name) + ";" : ";";
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
                methodInfo.ReturnType != typeof(void) ? methodInfo.ReturnType.GetName() + " " + ReturnValue + ";" : "",
                aspects.ForEach(x => x.SetupStartMethod(methodInfo, type)).ToString(x => x, "\r\n"),
                BaseCall,
                aspects.ForEach(x => x.SetupEndMethod(methodInfo, type, ReturnValue)).ToString(x => x, "\r\n"),
                string.IsNullOrEmpty(ReturnValue) ? "" : "return " + ReturnValue + ";",
                aspects.ForEach(x => x.SetupExceptionMethod(methodInfo, type)).ToString(x => x, "\r\n"));
            return Builder.ToString();
        }
    }
}