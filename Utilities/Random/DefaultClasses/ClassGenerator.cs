using System;
using System.Reflection;
using Utilities.Random.BaseClasses;
using Utilities.Random.Interfaces;
using Utilities.Reflection.ExtensionMethods;

namespace Utilities.Random.DefaultClasses
{
    /// <summary>
    /// Randomly generates a class
    /// </summary>
    public class ClassGenerator<T> : IGenerator<T>
        where T : class,new()
    {
        public T Next(System.Random Rand)
        {
            T ReturnItem = new T();
            System.Type ObjectType = typeof(T);
            System.Type[] Interfaces = ObjectType.GetInterfaces();
            foreach (PropertyInfo Property in ObjectType.GetProperties())
            {
                GeneratorAttributeBase Attribute = Property.GetAttribute<GeneratorAttributeBase>();
                ReturnItem.SetProperty(Property, Attribute.NextObj(Rand));
            }
            return ReturnItem;
        }

        public T Next(System.Random Rand, T Min, T Max)
        {
            return new T();
        }

        public object NextObj(System.Random Rand)
        {
            return new T();
        }
    }
}