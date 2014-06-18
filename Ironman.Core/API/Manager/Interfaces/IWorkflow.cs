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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities.DataTypes;

namespace Ironman.Core.API.Manager.Interfaces
{
    /// <summary>
    /// Workflow interface
    /// </summary>
    public interface IWorkflow
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        string Name { get; }

        /// <summary>
        /// Called after All
        /// </summary>
        /// <param name="Mapping">The mapping.</param>
        /// <param name="ReturnValues">The return values.</param>
        /// <returns>True if the call can continue, false otherwise</returns>
        bool PostAll(string Mapping, IEnumerable<Dynamo> ReturnValues);

        /// <summary>
        /// Called after Any
        /// </summary>
        /// <param name="Mapping">The mapping.</param>
        /// <param name="ReturnValue">The return value.</param>
        /// <returns>True if the call can continue, false otherwise</returns>
        bool PostAny(string Mapping, Dynamo ReturnValue);

        /// <summary>
        /// Called after the object is deleted
        /// </summary>
        /// <param name="Mapping">The mapping.</param>
        /// <param name="ID">The identifier.</param>
        /// <returns>True if the call can continue, false otherwise</returns>
        bool PostDelete(string Mapping, string ID);

        /// <summary>
        /// Called after the objects are saved
        /// </summary>
        /// <param name="Mapping">The mapping.</param>
        /// <param name="Objects">The objects.</param>
        /// <returns>True if the call can continue, false otherwise</returns>
        bool PostSave(string Mapping, IEnumerable<Dynamo> Objects);

        /// <summary>
        /// Called after the service is run
        /// </summary>
        /// <param name="Mapping">The mapping.</param>
        /// <param name="ReturnValue">The return value.</param>
        /// <returns>True if the call can continue, false otherwise</returns>
        bool PostService(string Mapping, Dynamo ReturnValue);

        /// <summary>
        /// Called prior to All
        /// </summary>
        /// <param name="Mapping">The mapping.</param>
        /// <returns>True if it can continue, false otherwise</returns>
        bool PreAll(string Mapping);

        /// <summary>
        /// Called before Any
        /// </summary>
        /// <param name="Mapping">The mapping.</param>
        /// <returns>True if the call can continue, false otherwise</returns>
        bool PreAny(string Mapping);

        /// <summary>
        /// Called before the object is deleted
        /// </summary>
        /// <param name="Mapping">The mapping.</param>
        /// <param name="ID">The identifier.</param>
        /// <returns>True if the call can continue, false otherwise</returns>
        bool PreDelete(string Mapping, string ID);

        /// <summary>
        /// Called before the objects are saved
        /// </summary>
        /// <param name="Mapping">The mapping.</param>
        /// <param name="Objects">The objects.</param>
        /// <returns>True if the call can continue, false otherwise</returns>
        bool PreSave(string Mapping, IEnumerable<Dynamo> Objects);

        /// <summary>
        /// Called before the service is run
        /// </summary>
        /// <param name="Mapping">The mapping.</param>
        /// <param name="Value">The value.</param>
        /// <returns>True if the call can continue, false otherwise</returns>
        bool PreService(string Mapping, Dynamo Value);
    }
}