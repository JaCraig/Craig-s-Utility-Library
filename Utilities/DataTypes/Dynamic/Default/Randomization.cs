/*
Copyright (c) 2014 <a href="http://www.gutgames.com">James Craig</a>

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
using System.Diagnostics.Contracts;
using System.Linq;
using Utilities.DataTypes.Dynamic.BaseClasses;

#endregion Usings

namespace Utilities.DataTypes.Dynamic.Default
{
    /// <summary>
    /// Randomization extension
    /// </summary>
    public class Randomization : DynamoExtensionBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Randomization()
            : base()
        {
        }

        /// <summary>
        /// Extends the given dynamo object
        /// </summary>
        /// <param name="Object">Object to extend</param>
        public override void Extend(Dynamo Object)
        {
            Contract.Requires<ArgumentNullException>(Object != null, "Object");
            if (Object.ContainsKey("Randomize"))
                return;
            dynamic Temp = Object;
            Temp.Randomize = new Action(() => Randomize(Object));
        }

        private static void Randomize(Dynamo Object)
        {
            Contract.Requires<ArgumentNullException>(Object != null, "Object");
            System.Random Rand = new System.Random();
            foreach (string Key in Object.Keys.ToList())
            {
                Type ObjectType = Object[Key].GetType();
                if (!ObjectType.Is<Delegate>())
                {
                    Object[Key] = typeof(Utilities.Random.RandomExtensions).GetMethods()
                                                             .First(x => x.Name.ToUpperInvariant() == "NEXT"
                                                                        && x.GetParameters().Length == 2)
                                                             .MakeGenericMethod(ObjectType)
                                                             .Invoke(null, new object[] { Rand, null })
                                                             .To(ObjectType, null);
                }
            }
        }
    }
}