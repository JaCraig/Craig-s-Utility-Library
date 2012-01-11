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
using Utilities.Reflection.ExtensionMethods;
#endregion

namespace Utilities.DataMapper
{
    /// <summary>
    /// Mapping class
    /// </summary>
    /// <typeparam name="Left">Left type</typeparam>
    /// <typeparam name="Right">Right type</typeparam>
    public class Mapping<Left, Right>
    {
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="LeftExpression">Left expression</param>
        /// <param name="RightExpression">Right expression</param>
        public Mapping(Expression<Func<Left, object>> LeftExpression, Expression<Func<Right, object>> RightExpression)
        {
            LeftGet = LeftExpression.Compile();
            LeftSet = LeftExpression.GetPropertySetter().Compile();
            RightGet = RightExpression.Compile();
            RightSet = RightExpression.GetPropertySetter().Compile();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="LeftGet">Left get function</param>
        /// <param name="LeftSet">Left set action</param>
        /// <param name="RightExpression">Right expression</param>
        public Mapping(Func<Left, object> LeftGet, Action<Left, object> LeftSet, Expression<Func<Right, object>> RightExpression)
        {
            this.LeftGet = LeftGet;
            this.LeftSet = LeftSet;
            RightGet = RightExpression.Compile();
            RightSet = RightExpression.GetPropertySetter().Compile();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="LeftExpression">Left expression</param>
        /// <param name="RightGet">Right get function</param>
        /// <param name="RightSet">Right set function</param>
        public Mapping(Expression<Func<Left, object>> LeftExpression, Func<Right, object> RightGet, Action<Right, object> RightSet)
        {
            LeftGet = LeftExpression.Compile();
            LeftSet = LeftExpression.GetPropertySetter().Compile();
            this.RightGet = RightGet;
            this.RightSet = RightSet;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="LeftGet">Left get function</param>
        /// <param name="LeftSet">Left set function</param>
        /// <param name="RightGet">Right get function</param>
        /// <param name="RightSet">Right set function</param>
        public Mapping(Func<Left, object> LeftGet, Action<Left, object> LeftSet, Func<Right, object> RightGet, Action<Right, object> RightSet)
        {
            this.LeftGet = LeftGet;
            this.LeftSet = LeftSet;
            this.RightGet = RightGet;
            this.RightSet = RightSet;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Left get function
        /// </summary>
        protected virtual Func<Left, object> LeftGet { get; set; }

        /// <summary>
        /// Right get function
        /// </summary>
        protected virtual Func<Right, object> RightGet { get; set; }

        /// <summary>
        /// Left set function
        /// </summary>
        protected virtual Action<Left, object> LeftSet { get; set; }

        /// <summary>
        /// Right set function
        /// </summary>
        protected virtual Action<Right, object> RightSet { get; set; }

        #endregion

        #region Functions

        /// <summary>
        /// Copies the source to the destination
        /// </summary>
        /// <param name="Source">Source object</param>
        /// <param name="Destination">Destination object</param>
        public virtual void Copy(Left Source, Right Destination)
        {
            if (RightSet == null || LeftGet == null || Source == null || Destination == null) return;
            RightSet(Destination, LeftGet(Source));
        }

        /// <summary>
        /// Copies the source to the destination
        /// </summary>
        /// <param name="Source">Source object</param>
        /// <param name="Destination">Destination object</param>
        public virtual void Copy(Right Source, Left Destination)
        {
            if (LeftSet == null || RightGet == null || Source == null || Destination == null) return;
            LeftSet(Destination, RightGet(Source));
        }

        /// <summary>
        /// Copies from the source to the destination (used in 
        /// instances when both Left and Right are the same type
        /// and thus Copy is ambiguous)
        /// </summary>
        /// <param name="Source">Source object</param>
        /// <param name="Destination">Destination object</param>
        public virtual void CopyLeftToRight(Left Source, Right Destination)
        {
            if (RightSet == null || LeftGet == null || Source == null || Destination == null) return;
            RightSet(Destination, LeftGet(Source));
        }

        /// <summary>
        /// Copies from the source to the destination (used in 
        /// instances when both Left and Right are the same type
        /// and thus Copy is ambiguous)
        /// </summary>
        /// <param name="Source">Source object</param>
        /// <param name="Destination">Destination object</param>
        public virtual void CopyRightToLeft(Right Source, Left Destination)
        {
            if (LeftSet == null || RightGet == null || Source == null || Destination == null) return;
            LeftSet(Destination, RightGet(Source));
        }

        #endregion
    }
}