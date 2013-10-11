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
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Utilities.DataTypes.CodeGen;
using Utilities.DataTypes.DataMapper.Interfaces;
#endregion

namespace Utilities.DataTypes.DataMapper.BaseClasses
{
    /// <summary>
    /// Type mapping base class
    /// </summary>
    public abstract class TypeMappingBase<Left, Right> : ITypeMapping<Left, Right>
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Compiler">Compiler</param>
        protected TypeMappingBase(Compiler Compiler)
        {
            this.Compiler = Compiler;
            this.Mappings = new List<IMapping<Left, Right>>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Compiler that is potentially used
        /// </summary>
        protected Compiler Compiler { get; private set; }

        /// <summary>
        /// List of mappings
        /// </summary>
        protected ICollection<IMapping<Left, Right>> Mappings { get; private set; }

        #endregion

        #region Functions

        /// <summary>
        /// Automatically maps properties that are named the same thing
        /// </summary>
        /// <returns>This</returns>
        public virtual ITypeMapping<Left, Right> AutoMap()
        {
            PropertyInfo[] Properties = typeof(Left).GetProperties();
            Type DestinationType = typeof(Right);
            for (int x = 0; x < Properties.Length; ++x)
            {
                PropertyInfo DestinationProperty = DestinationType.GetProperty(Properties[x].Name);
                if (DestinationProperty != null)
                {
                    Expression<Func<Left, object>> LeftGet = Properties[x].PropertyGetter<Left>();
                    Expression<Func<Right, object>> RightGet = DestinationProperty.PropertyGetter<Right>();
                    this.AddMapping(LeftGet, RightGet);
                }
            }
            return this;
        }

        /// <summary>
        /// Adds a mapping
        /// </summary>
        /// <param name="LeftExpression">Left expression</param>
        /// <param name="RightExpression">Right expression</param>
        /// <returns>This</returns>
        public abstract ITypeMapping<Left, Right> AddMapping(Expression<Func<Left, object>> LeftExpression, Expression<Func<Right, object>> RightExpression);

        /// <summary>
        /// Adds a mapping
        /// </summary>
        /// <param name="LeftGet">Left get function</param>
        /// <param name="LeftSet">Left set action</param>
        /// <param name="RightExpression">Right expression</param>
        /// <returns>This</returns>
        public abstract ITypeMapping<Left, Right> AddMapping(Func<Left, object> LeftGet, Action<Left, object> LeftSet, Expression<Func<Right, object>> RightExpression);

        /// <summary>
        /// Adds a mapping
        /// </summary>
        /// <param name="LeftExpression">Left expression</param>
        /// <param name="RightGet">Right get function</param>
        /// <param name="RightSet">Right set function</param>
        /// <returns>This</returns>
        public abstract ITypeMapping<Left, Right> AddMapping(Expression<Func<Left, object>> LeftExpression, Func<Right, object> RightGet, Action<Right, object> RightSet);

        /// <summary>
        /// Adds a mapping
        /// </summary>
        /// <param name="LeftGet">Left get function</param>
        /// <param name="LeftSet">Left set function</param>
        /// <param name="RightGet">Right get function</param>
        /// <param name="RightSet">Right set function</param>
        /// <returns>This</returns>
        public abstract ITypeMapping<Left, Right> AddMapping(Func<Left, object> LeftGet, Action<Left, object> LeftSet, Func<Right, object> RightGet, Action<Right, object> RightSet);

        /// <summary>
        /// Copies from the source to the destination
        /// </summary>
        /// <param name="Source">Source object</param>
        /// <param name="Destination">Destination object</param>
        public abstract void Copy(Left Source, Right Destination);

        /// <summary>
        /// Copies from the source to the destination
        /// </summary>
        /// <param name="Source">Source object</param>
        /// <param name="Destination">Destination object</param>
        public abstract void Copy(Right Source, Left Destination);

        /// <summary>
        /// Copies from the source to the destination (used in 
        /// instances when both Left and Right are the same type
        /// and thus Copy is ambiguous)
        /// </summary>
        /// <param name="Source">Source</param>
        /// <param name="Destination">Destination</param>
        public abstract void CopyLeftToRight(Left Source, Right Destination);

        /// <summary>
        /// Copies from the source to the destination (used in 
        /// instances when both Left and Right are the same type
        /// and thus Copy is ambiguous)
        /// </summary>
        /// <param name="Source">Source</param>
        /// <param name="Destination">Destination</param>
        public abstract void CopyRightToLeft(Right Source, Left Destination);

        #endregion
    }
}