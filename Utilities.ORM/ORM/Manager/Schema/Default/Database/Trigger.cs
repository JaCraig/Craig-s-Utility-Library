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

using Utilities.ORM.Manager.Schema.Enums;
using Utilities.ORM.Manager.Schema.Interfaces;

namespace Utilities.ORM.Manager.Schema.Default.Database
{
    /// <summary>
    /// Trigger class
    /// </summary>
    public class Trigger : ITrigger
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Name">Name</param>
        /// <param name="Definition">Definition</param>
        /// <param name="Type">Type</param>
        /// <param name="ParentTable">Parent table</param>
        public Trigger(string Name, string Definition, TriggerType Type, ITable ParentTable)
        {
            this.Name = Name;
            this.Definition = Definition;
            this.Type = Type;
            this.ParentTable = ParentTable;
        }

        /// <summary>
        /// Definition of the trigger
        /// </summary>
        public string Definition { get; set; }

        /// <summary>
        /// Name of the trigger
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Parent table
        /// </summary>
        public ITable ParentTable { get; set; }

        /// <summary>
        /// Trigger type
        /// </summary>
        public TriggerType Type { get; set; }
    }
}