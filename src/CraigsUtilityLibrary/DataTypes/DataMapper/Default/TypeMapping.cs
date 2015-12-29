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

using System;
using System.Linq;
using System.Linq.Expressions;
using Utilities.DataTypes.DataMapper.BaseClasses;
using Utilities.DataTypes.DataMapper.Interfaces;

namespace Utilities.DataTypes.DataMapper.Default
{
    /// <summary>
    /// Type mapping default class
    /// </summary>
    public class TypeMapping<Left, Right> : TypeMappingBase<Left, Right>
    {
        /// <summary>
        /// Adds a mapping
        /// </summary>
        /// <param name="leftExpression">Left expression</param>
        /// <param name="rightExpression">Right expression</param>
        /// <returns>This</returns>
        public override ITypeMapping<Left, Right> AddMapping(Expression<Func<Left, object>> leftExpression, Expression<Func<Right, object>> rightExpression)
        {
            Mappings.Add(new Mapping<Left, Right>(leftExpression, rightExpression));
            return this;
        }

        /// <summary>
        /// Adds a mapping
        /// </summary>
        /// <param name="leftGet">Left get function</param>
        /// <param name="leftSet">Left set action</param>
        /// <param name="rightExpression">Right expression</param>
        /// <returns>This</returns>
        public override ITypeMapping<Left, Right> AddMapping(Func<Left, object> leftGet, Action<Left, object> leftSet, Expression<Func<Right, object>> rightExpression)
        {
            Mappings.Add(new Mapping<Left, Right>(leftGet, leftSet, rightExpression));
            return this;
        }

        /// <summary>
        /// Adds a mapping
        /// </summary>
        /// <param name="leftExpression">Left expression</param>
        /// <param name="rightGet">Right get function</param>
        /// <param name="rightSet">Right set function</param>
        /// <returns>This</returns>
        public override ITypeMapping<Left, Right> AddMapping(Expression<Func<Left, object>> leftExpression, Func<Right, object> rightGet, Action<Right, object> rightSet)
        {
            Mappings.Add(new Mapping<Left, Right>(leftExpression, rightGet, rightSet));
            return this;
        }

        /// <summary>
        /// Adds a mapping
        /// </summary>
        /// <param name="leftGet">Left get function</param>
        /// <param name="leftSet">Left set function</param>
        /// <param name="rightGet">Right get function</param>
        /// <param name="rightSet">Right set function</param>
        /// <returns>This</returns>
        public override ITypeMapping<Left, Right> AddMapping(Func<Left, object> leftGet, Action<Left, object> leftSet, Func<Right, object> rightGet, Action<Right, object> rightSet)
        {
            Mappings.Add(new Mapping<Left, Right>(leftGet, leftSet, rightGet, rightSet));
            return this;
        }

        /// <summary>
        /// Copies from the source to the destination
        /// </summary>
        /// <param name="source">Source object</param>
        /// <param name="destination">Destination object</param>
        public override void Copy(Left source, Right destination)
        {
            foreach (Mapping<Left, Right> Mapping in Mappings.OfType<Mapping<Left, Right>>())
            {
                Mapping.Copy(source, destination);
            }
        }

        /// <summary>
        /// Copies from the source to the destination
        /// </summary>
        /// <param name="source">Source object</param>
        /// <param name="destination">Destination object</param>
        public override void Copy(Right source, Left destination)
        {
            foreach (Mapping<Left, Right> Mapping in Mappings.OfType<Mapping<Left, Right>>())
            {
                Mapping.Copy(source, destination);
            }
        }

        /// <summary>
        /// Copies from the source to the destination (used in instances when both Left and Right
        /// are the same type and thus Copy is ambiguous)
        /// </summary>
        /// <param name="source">Source</param>
        /// <param name="destination">Destination</param>
        public override void CopyLeftToRight(Left source, Right destination)
        {
            foreach (Mapping<Left, Right> Mapping in Mappings.OfType<Mapping<Left, Right>>())
            {
                Mapping.CopyLeftToRight(source, destination);
            }
        }

        /// <summary>
        /// Copies from the source to the destination (used in instances when both Left and Right
        /// are the same type and thus Copy is ambiguous)
        /// </summary>
        /// <param name="source">Source</param>
        /// <param name="destination">Destination</param>
        public override void CopyRightToLeft(Right source, Left destination)
        {
            foreach (Mapping<Left, Right> Mapping in Mappings.OfType<Mapping<Left, Right>>())
            {
                Mapping.CopyRightToLeft(source, destination);
            }
        }
    }
}