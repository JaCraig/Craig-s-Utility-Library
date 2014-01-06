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
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using System.Reflection;
using Utilities.DataTypes.DataMapper.Interfaces;

#endregion Usings

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
        protected TypeMappingBase()
        {
            this.Mappings = new List<IMapping<Left, Right>>();
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// List of mappings
        /// </summary>
        protected ICollection<IMapping<Left, Right>> Mappings { get; private set; }

        #endregion Properties

        #region Functions

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
        /// Automatically maps properties that are named the same thing
        /// </summary>
        /// <returns>This</returns>
        public virtual ITypeMapping AutoMap()
        {
            if (Mappings.Count > 0)
                return this;
            Type LeftType = typeof(Left);
            Type RightType = typeof(Right);
            if (RightType.Is<IDictionary<string, object>>() && LeftType.Is<IDictionary<string, object>>())
            {
                AddIDictionaryMappings();
            }
            else if (RightType.Is<IDictionary<string, object>>())
            {
                AddRightIDictionaryMapping(LeftType, RightType);
            }
            else if (LeftType.Is<IDictionary<string, object>>())
            {
                AddLeftIDictionaryMapping(LeftType, RightType);
            }
            else
            {
                PropertyInfo[] Properties = typeof(Left).GetProperties();
                for (int x = 0; x < Properties.Length; ++x)
                {
                    PropertyInfo DestinationProperty = RightType.GetProperty(Properties[x].Name);
                    if (DestinationProperty != null)
                    {
                        Expression<Func<Left, object>> LeftGet = Properties[x].PropertyGetter<Left>();
                        Expression<Func<Right, object>> RightGet = DestinationProperty.PropertyGetter<Right>();
                        this.AddMapping(LeftGet, RightGet);
                    }
                }
            }
            return this;
        }

        /// <summary>
        /// Copies from the source to the destination
        /// </summary>
        /// <param name="Source">Source object</param>
        /// <param name="Destination">Destination object</param>
        public void Copy(object Source, object Destination)
        {
            Copy((Left)Source, (Right)Destination);
        }

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
        /// Copies from the source to the destination (used in instances when both Left and Right
        /// are the same type and thus Copy is ambiguous)
        /// </summary>
        /// <param name="Source">Source</param>
        /// <param name="Destination">Destination</param>
        public abstract void CopyLeftToRight(Left Source, Right Destination);

        /// <summary>
        /// Copies from the source to the destination (used in instances when both Left and Right
        /// are the same type and thus Copy is ambiguous)
        /// </summary>
        /// <param name="Source">Source</param>
        /// <param name="Destination">Destination</param>
        public abstract void CopyRightToLeft(Right Source, Left Destination);

        private void AddIDictionaryMappings()
        {
            this.AddMapping(x => x,
            new Action<Left, object>((x, y) =>
            {
                IDictionary<string, object> LeftSide = (IDictionary<string, object>)x;
                IDictionary<string, object> RightSide = (IDictionary<string, object>)y;
                foreach (string Key in RightSide.Keys)
                {
                    if (LeftSide.ContainsKey(Key))
                        LeftSide[Key] = RightSide[Key];
                    else
                        LeftSide.Add(Key, RightSide[Key]);
                }
            }),
            x => x,
            new Action<Right, object>((x, y) =>
            {
                IDictionary<string, object> LeftSide = (IDictionary<string, object>)y;
                IDictionary<string, object> RightSide = (IDictionary<string, object>)x;
                foreach (string Key in LeftSide.Keys)
                {
                    if (RightSide.ContainsKey(Key))
                        RightSide[Key] = LeftSide[Key];
                    else
                        RightSide.Add(Key, LeftSide[Key]);
                }
            }));
        }

        private void AddLeftIDictionaryMapping(Type LeftType, Type RightType)
        {
            Contract.Requires<ArgumentNullException>(RightType != null, "RightType");
            PropertyInfo[] Properties = RightType.GetProperties();
            for (int x = 0; x < Properties.Length; ++x)
            {
                PropertyInfo Property = Properties[x];
                Expression<Func<Right, object>> RightGet = Properties[x].PropertyGetter<Right>();
                PropertyInfo LeftProperty = LeftType.GetProperty(Property.Name);
                if (LeftProperty != null)
                {
                    Expression<Func<Left, object>> LeftGet = LeftProperty.PropertyGetter<Left>();
                    this.AddMapping(LeftGet, RightGet);
                }
                else
                {
                    this.AddMapping(new Func<Left, object>(y =>
                    {
                        IDictionary<string, object> Temp = (IDictionary<string, object>)y;
                        if (Temp.ContainsKey(Property.Name))
                            return Temp[Property.Name];
                        return null;
                    }),
                    new Action<Left, object>((y, z) =>
                    {
                        IDictionary<string, object> LeftSide = (IDictionary<string, object>)y;
                        if (LeftSide.ContainsKey(Property.Name))
                            LeftSide[Property.Name] = z;
                        else
                            LeftSide.Add(Property.Name, z);
                    }),
                    RightGet);
                }
            }
        }

        private void AddRightIDictionaryMapping(Type LeftType, Type RightType)
        {
            Contract.Requires<ArgumentNullException>(LeftType != null, "LeftType");
            PropertyInfo[] Properties = LeftType.GetProperties();
            for (int x = 0; x < Properties.Length; ++x)
            {
                PropertyInfo Property = Properties[x];
                Expression<Func<Left, object>> LeftGet = Property.PropertyGetter<Left>();
                PropertyInfo RightProperty = RightType.GetProperty(Property.Name);
                if (RightProperty != null)
                {
                    Expression<Func<Right, object>> RightGet = RightProperty.PropertyGetter<Right>();
                    this.AddMapping(LeftGet, RightGet);
                }
                else
                {
                    this.AddMapping(LeftGet,
                    new Func<Right, object>(y =>
                    {
                        IDictionary<string, object> Temp = (IDictionary<string, object>)y;
                        if (Temp.ContainsKey(Property.Name))
                            return Temp[Property.Name];
                        return null;
                    }),
                    new Action<Right, object>((y, z) =>
                    {
                        IDictionary<string, object> LeftSide = (IDictionary<string, object>)y;
                        if (LeftSide.ContainsKey(Property.Name))
                            LeftSide[Property.Name] = z;
                        else
                            LeftSide.Add(Property.Name, z);
                    }));
                }
            }
        }

        #endregion Functions
    }
}