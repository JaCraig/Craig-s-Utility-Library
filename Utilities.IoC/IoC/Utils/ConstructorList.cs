/*
Copyright (c) 2011 <a href="http://www.gutgames.com">James Craig</a>

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

#region Usings
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Utilities.IoC.Mappings;
#endregion

namespace Utilities.IoC.Utils
{
    public static class ConstructorList
    {
        public static ConstructorInfo ChooseConstructor(Type ImplementationType, MappingManager MappingManager)
        {
            ConstructorInfo[] Constructors = ImplementationType.GetConstructors();
            int MaxValue = int.MinValue;
            ConstructorInfo CurrentConstructor = null;
            foreach (ConstructorInfo Constructor in Constructors)
            {
                int Count = GetParameterCount(Constructor, MappingManager);
                if (Count > MaxValue)
                {
                    CurrentConstructor = Constructor;
                    MaxValue = Count;
                }
            }
            return CurrentConstructor;
        }

        private static int GetParameterCount(ConstructorInfo Constructor, MappingManager MappingManager)
        {
            int Count = 0;
            ParameterInfo[] Parameters = Constructor.GetParameters();
            foreach (ParameterInfo Parameter in Parameters)
            {
                bool Inject = true;
                object[] Attributes = Parameter.GetCustomAttributes(false);
                if (Attributes.Length > 0)
                {
                    foreach (Attribute Attribute in Attributes)
                    {
                        if (MappingManager.GetMapping(Parameter.ParameterType, Attribute.GetType()) != null)
                        {
                            ++Count;
                            Inject = false;
                            break;
                        }
                    }
                }
                if (Inject)
                {
                    if (MappingManager.GetMapping(Parameter.ParameterType) != null)
                        ++Count;
                }
            }
            if (Count == Parameters.Length)
                return Count;
            return int.MinValue;
        }
    }
}
