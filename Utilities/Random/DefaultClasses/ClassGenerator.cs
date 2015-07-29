using System;
using System.Reflection;
using Utilities.DataTypes;
using Utilities.Random.BaseClasses;
using Utilities.Random.Interfaces;

namespace Utilities.Random.DefaultClasses
{
    /// <summary>
    /// Randomly generates a class
    /// </summary>
    /// <typeparam name="T">Class type to generate</typeparam>
    public class ClassGenerator<T> : IGenerator<T>
        where T : class,new()
    {
        /// <summary>
        /// Generates a random version of the class
        /// </summary>
        /// <param name="Rand">Random generator to use</param>
        /// <returns>The randomly generated class</returns>
        public T Next(System.Random Rand)
        {
            var ReturnItem = new T();
            System.Type ObjectType = typeof(T);
            foreach (PropertyInfo Property in ObjectType.GetProperties())
            {
                GeneratorAttributeBase Attribute = Property.Attribute<GeneratorAttributeBase>();
                if (Attribute != null)
                    ReturnItem.Property(Property, Attribute.NextObj(Rand));
            }
            return ReturnItem;
        }

        /// <summary>
        /// Generates a random version of the class
        /// </summary>
        /// <param name="Rand">Random generator to use</param>
        /// <param name="Min">Min value (not used)</param>
        /// <param name="Max">Max value (not used)</param>
        /// <returns>The randomly generated class</returns>
        public T Next(System.Random Rand, T Min, T Max)
        {
            return new T();
        }

        /// <summary>
        /// Gets a random version of the class
        /// </summary>
        /// <param name="Rand">Random generator used</param>
        /// <returns>The randonly generated class</returns>
        public object NextObj(System.Random Rand)
        {
            return new T();
        }
    }
}