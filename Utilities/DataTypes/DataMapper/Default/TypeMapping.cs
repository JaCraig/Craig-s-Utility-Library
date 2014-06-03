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
using System.Linq.Expressions;
using System.Threading.Tasks;
using Utilities.DataTypes.DataMapper.BaseClasses;
using Utilities.DataTypes.DataMapper.Interfaces;

#endregion Usings

namespace Utilities.DataTypes.DataMapper.Default
{
    /// <summary>
    /// Type mapping default class
    /// </summary>
    public class TypeMapping<Left, Right> : TypeMappingBase<Left, Right>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public TypeMapping()
            : base()
        {
        }

        /// <summary>
        /// Adds a mapping
        /// </summary>
        /// <param name="LeftExpression">Left expression</param>
        /// <param name="RightExpression">Right expression</param>
        /// <returns>This</returns>
        public override ITypeMapping<Left, Right> AddMapping(Expression<Func<Left, object>> LeftExpression, Expression<Func<Right, object>> RightExpression)
        {
            this.Mappings.Add(new Mapping<Left, Right>(LeftExpression, RightExpression));
            return this;
        }

        /// <summary>
        /// Adds a mapping
        /// </summary>
        /// <param name="LeftGet">Left get function</param>
        /// <param name="LeftSet">Left set action</param>
        /// <param name="RightExpression">Right expression</param>
        /// <returns>This</returns>
        public override ITypeMapping<Left, Right> AddMapping(Func<Left, object> LeftGet, Action<Left, object> LeftSet, Expression<Func<Right, object>> RightExpression)
        {
            this.Mappings.Add(new Mapping<Left, Right>(LeftGet, LeftSet, RightExpression));
            return this;
        }

        /// <summary>
        /// Adds a mapping
        /// </summary>
        /// <param name="LeftExpression">Left expression</param>
        /// <param name="RightGet">Right get function</param>
        /// <param name="RightSet">Right set function</param>
        /// <returns>This</returns>
        public override ITypeMapping<Left, Right> AddMapping(Expression<Func<Left, object>> LeftExpression, Func<Right, object> RightGet, Action<Right, object> RightSet)
        {
            this.Mappings.Add(new Mapping<Left, Right>(LeftExpression, RightGet, RightSet));
            return this;
        }

        /// <summary>
        /// Adds a mapping
        /// </summary>
        /// <param name="LeftGet">Left get function</param>
        /// <param name="LeftSet">Left set function</param>
        /// <param name="RightGet">Right get function</param>
        /// <param name="RightSet">Right set function</param>
        /// <returns>This</returns>
        public override ITypeMapping<Left, Right> AddMapping(Func<Left, object> LeftGet, Action<Left, object> LeftSet, Func<Right, object> RightGet, Action<Right, object> RightSet)
        {
            this.Mappings.Add(new Mapping<Left, Right>(LeftGet, LeftSet, RightGet, RightSet));
            return this;
        }

        /// <summary>
        /// Copies from the source to the destination
        /// </summary>
        /// <param name="Source">Source object</param>
        /// <param name="Destination">Destination object</param>
        public override void Copy(Left Source, Right Destination)
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
        public override void Copy(Right Source, Left Destination)
        {
            foreach (Mapping<Left, Right> Mapping in Mappings)
            {
                Mapping.Copy(Source, Destination);
            }
        }

        /// <summary>
        /// Copies from the source to the destination (used in instances when both Left and Right
        /// are the same type and thus Copy is ambiguous)
        /// </summary>
        /// <param name="Source">Source</param>
        /// <param name="Destination">Destination</param>
        public override void CopyLeftToRight(Left Source, Right Destination)
        {
            foreach (Mapping<Left, Right> Mapping in Mappings)
            {
                Mapping.CopyLeftToRight(Source, Destination);
            }
        }

        /// <summary>
        /// Copies from the source to the destination (used in instances when both Left and Right
        /// are the same type and thus Copy is ambiguous)
        /// </summary>
        /// <param name="Source">Source</param>
        /// <param name="Destination">Destination</param>
        public override void CopyRightToLeft(Right Source, Left Destination)
        {
            foreach (Mapping<Left, Right> Mapping in Mappings)
            {
                Mapping.CopyRightToLeft(Source, Destination);
            }
        }
    }
}