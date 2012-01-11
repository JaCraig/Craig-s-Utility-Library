/*
Copyright (c) 2012 <a href="http://www.gutgames.com">James Craig</a>

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
using System.Linq.Expressions;
using Utilities.DataMapper.Interfaces;
using System.Reflection;
using Utilities.Reflection.ExtensionMethods;
#endregion

namespace Utilities.DataMapper
{
    /// <summary>
    /// Maps two types together
    /// </summary>
    /// <typeparam name="Left">Left type</typeparam>
    /// <typeparam name="Right">Right type</typeparam>
    public class TypeMapping<Left, Right>:ITypeMapping
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public TypeMapping() { Mappings = new List<Mapping<Left, Right>>(); }

        #endregion

        #region Properties

        /// <summary>
        /// List of mappings
        /// </summary>
        protected virtual List<Mapping<Left, Right>> Mappings { get; set; }

        #endregion

        #region Functions

        /// <summary>
        /// Automatically maps properties that are named the same thing
        /// </summary>
        public virtual TypeMapping<Left,Right> AutoMap()
        {
            PropertyInfo[] Properties = typeof(Left).GetProperties();
            Type DestinationType = typeof(Right);
            for (int x = 0; x < Properties.Length; ++x)
            {
                PropertyInfo DestinationProperty=DestinationType.GetProperty(Properties[x].Name);
                if (DestinationProperty != null)
                {
                    Expression<Func<Left, object>> LeftGet = Properties[x].GetPropertyGetter<Left>();
                    Expression<Action<Left, object>> LeftSet = LeftGet.GetPropertySetter();
                    Expression<Func<Right, object>> RightGet = DestinationProperty.GetPropertyGetter<Right>();
                    Expression<Action<Right, object>> RightSet = RightGet.GetPropertySetter();
                    this.AddMapping(LeftGet.Compile(), LeftSet.Compile(), RightGet.Compile(), RightSet.Compile());
                }
            }
            return this;
        }

        /// <summary>
        /// Adds a mapping
        /// </summary>
        /// <param name="LeftExpression">Left expression</param>
        /// <param name="RightExpression">Right expression</param>
        public virtual TypeMapping<Left, Right> AddMapping(Expression<Func<Left, object>> LeftExpression, Expression<Func<Right, object>> RightExpression)
        {
            Mappings.Add(new Mapping<Left, Right>(LeftExpression, RightExpression));
            return this;
        }

        /// <summary>
        /// Adds a mapping
        /// </summary>
        /// <param name="LeftGet">Left get function</param>
        /// <param name="LeftSet">Left set action</param>
        /// <param name="RightExpression">Right expression</param>
        public virtual TypeMapping<Left, Right> AddMapping(Func<Left, object> LeftGet, Action<Left, object> LeftSet, Expression<Func<Right, object>> RightExpression)
        {
            Mappings.Add(new Mapping<Left, Right>(LeftGet, LeftSet, RightExpression));
            return this;
        }

        /// <summary>
        /// Adds a mapping
        /// </summary>
        /// <param name="LeftExpression">Left expression</param>
        /// <param name="RightGet">Right get function</param>
        /// <param name="RightSet">Right set function</param>
        public virtual TypeMapping<Left, Right> AddMapping(Expression<Func<Left, object>> LeftExpression, Func<Right, object> RightGet, Action<Right, object> RightSet)
        {
            Mappings.Add(new Mapping<Left, Right>(LeftExpression, RightGet, RightSet));
            return this;
        }

        /// <summary>
        /// Adds a mapping
        /// </summary>
        /// <param name="LeftGet">Left get function</param>
        /// <param name="LeftSet">Left set function</param>
        /// <param name="RightGet">Right get function</param>
        /// <param name="RightSet">Right set function</param>
        public virtual TypeMapping<Left, Right> AddMapping(Func<Left, object> LeftGet, Action<Left, object> LeftSet, Func<Right, object> RightGet, Action<Right, object> RightSet)
        {
            Mappings.Add(new Mapping<Left, Right>(LeftGet, LeftSet, RightGet, RightSet));
            return this;
        }

        /// <summary>
        /// Copies from the source to the destination
        /// </summary>
        /// <param name="Source">Source object</param>
        /// <param name="Destination">Destination object</param>
        public void Copy(Left Source, Right Destination)
        {
            foreach (Mapping<Left, Right> Mapping in Mappings)
            {
                Mapping.Copy(Source, Destination);
            }
        }

        /// <summary>
        /// Copies from the source to the destination
        /// </summary>
        /// <param name="Source">Source object</param>
        /// <param name="Destination">Destination object</param>
        public void Copy(Right Source, Left Destination)
        {
            foreach (Mapping<Left, Right> Mapping in Mappings)
            {
                Mapping.Copy(Source, Destination);
            }
        }

        /// <summary>
        /// Copies from the source to the destination (used in 
        /// instances when both Left and Right are the same type
        /// and thus Copy is ambiguous)
        /// </summary>
        /// <param name="Source">Source</param>
        /// <param name="Destination">Destination</param>
        public void CopyLeftToRight(Left Source, Right Destination)
        {
            foreach (Mapping<Left, Right> Mapping in Mappings)
            {
                Mapping.CopyLeftToRight(Source, Destination);
            }
        }

        /// <summary>
        /// Copies from the source to the destination (used in 
        /// instances when both Left and Right are the same type
        /// and thus Copy is ambiguous)
        /// </summary>
        /// <param name="Source">Source</param>
        /// <param name="Destination">Destination</param>
        public void CopyRightToLeft(Right Source, Left Destination)
        {
            foreach (Mapping<Left, Right> Mapping in Mappings)
            {
                Mapping.CopyRightToLeft(Source, Destination);
            }
        }

        #endregion
    }
}
