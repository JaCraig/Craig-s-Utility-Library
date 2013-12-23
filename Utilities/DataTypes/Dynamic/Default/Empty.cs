using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.DataTypes.Dynamic.BaseClasses;

namespace Utilities.DataTypes.Dynamic.Default
{
    /// <summary>
    /// Empty extension
    /// </summary>
    public class Empty : DynamoExtensionBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Empty() : base() { }

        /// <summary>
        /// Extends the given dynamo object
        /// </summary>
        /// <param name="Object">Object to extend</param>
        public override void Extend(Dynamo Object)
        {
        }
    }
}