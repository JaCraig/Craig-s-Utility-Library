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

using Ironman.Core.API.Manager.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Utilities.DataTypes;

namespace Ironman.Core.API.Manager
{
    /// <summary>
    /// API manager
    /// </summary>
    public class Manager
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Manager()
            : base()
        {
            Mappings = new Dictionary<int, MappingHolder>();
            foreach (IAPIMapping Mapping in AppDomain.CurrentDomain.GetAssemblies().Objects<IAPIMapping>())
            {
                foreach (int Version in Mapping.Versions)
                {
                    if (!Mappings.ContainsKey(Version))
                        Mappings.Add(Version, new MappingHolder());
                    Mappings[Version].Mappings.Add(Mapping.Name, Mapping);
                }
            }
        }

        private IDictionary<int, MappingHolder> Mappings { get; set; }

        /// <summary>
        /// Gets the specified mapping
        /// </summary>
        /// <param name="Version">API version number</param>
        /// <param name="Key">Name of the mapped type</param>
        /// <returns>The mapping specified</returns>
        public IAPIMapping this[int Version, string Key] { get { return Mappings.GetValue(Version).Mappings.GetValue(Key); } }

        /// <summary>
        /// Gets all items of the specified mapping
        /// </summary>
        /// <param name="Version">API version number</param>
        /// <param name="Mapping">Mapping name</param>
        /// <param name="EmbeddedProperties">Embedded properties</param>
        /// <returns>The resulting items</returns>
        public IEnumerable<Dynamo> All(int Version, string Mapping, params string[] EmbeddedProperties)
        {
            try
            {
                IDictionary<string, IAPIMapping> TempMappings = Mappings.GetValue(Version).Mappings;
                if (!TempMappings.ContainsKey(Mapping))
                    return new List<Dynamo>();
                return TempMappings[Mapping].All(Mappings.GetValue(Version), EmbeddedProperties);
            }
            catch
            {
                return new List<Dynamo>();
            }
        }

        /// <summary>
        /// Gets the specified item
        /// </summary>
        /// <param name="Mapping">Mapping name</param>
        /// <param name="Version">API version number</param>
        /// <param name="ID">ID of the item to get</param>
        /// <param name="EmbeddedProperties">Embedded properties</param>
        /// <returns>The item specified</returns>
        public Dynamo Any(int Version, string Mapping, string ID, params string[] EmbeddedProperties)
        {
            try
            {
                IDictionary<string, IAPIMapping> TempMappings = Mappings.GetValue(Version).Mappings;
                if (!TempMappings.ContainsKey(Mapping))
                    return Error("Error getting item");
                return TempMappings[Mapping].Any(ID, Mappings.GetValue(Version), EmbeddedProperties);
            }
            catch
            {
                return Error("Error saving the object");
            }
        }

        /// <summary>
        /// Deletes an object
        /// </summary>
        /// <param name="Mapping">Mapping name</param>
        /// <param name="Version">API version number</param>
        /// <param name="ID">ID of the object to delete</param>
        /// <returns>The result</returns>
        public Dynamo Delete(int Version, string Mapping, string ID)
        {
            try
            {
                IDictionary<string, IAPIMapping> TempMappings = Mappings.GetValue(Version).Mappings;
                if (!TempMappings.ContainsKey(Mapping) || !TempMappings[Mapping].Delete(ID))
                    return Error("Error deleting the object");
                return Success("Object deleted successfully");
            }
            catch
            {
                return Error("Error saving the object");
            }
        }

        /// <summary>
        /// Deletes a property value
        /// </summary>
        /// <param name="Version">Version number</param>
        /// <param name="Mapping">Model name</param>
        /// <param name="ID">ID of the item to get</param>
        /// <param name="PropertyName">Property name</param>
        /// <param name="PropertyID">Property ID</param>
        /// <returns>The result</returns>
        public Dynamo DeleteProperty(int Version, string Mapping, string ID, string PropertyName, string PropertyID)
        {
            try
            {
                IDictionary<string, IAPIMapping> TempMappings = Mappings.GetValue(Version).Mappings;
                if (!TempMappings.ContainsKey(Mapping))
                    return Error("Error deleting item");
                if (TempMappings[Mapping].DeleteProperty(ID, Mappings.GetValue(Version), PropertyName, PropertyID))
                    return Success("Object deleted successfully");
                return Error("Error deleting item");
            }
            catch
            {
                return Error("Error deleting the object");
            }
        }

        /// <summary>
        /// Gets the specified item
        /// </summary>
        /// <param name="Mapping">Mapping name</param>
        /// <param name="Version">API version number</param>
        /// <param name="ID">ID of the item to get</param>
        /// <param name="EmbeddedProperties">Embedded properties</param>
        /// <param name="Property">Property name</param>
        /// <returns>The item specified</returns>
        public dynamic GetProperty(int Version, string Mapping, string ID, string Property, params string[] EmbeddedProperties)
        {
            try
            {
                IDictionary<string, IAPIMapping> TempMappings = Mappings.GetValue(Version).Mappings;
                if (!TempMappings.ContainsKey(Mapping))
                    return Error("Error getting item");
                return TempMappings[Mapping].GetProperty(ID, Mappings.GetValue(Version), Property, EmbeddedProperties);
            }
            catch
            {
                return Error("Error saving the object");
            }
        }

        /// <summary>
        /// Saves a list of objects
        /// </summary>
        /// <param name="Mapping">Mapping name</param>
        /// <param name="Version">API version number</param>
        /// <param name="Objects">Objects to save</param>
        /// <returns>The result</returns>
        public Dynamo Save(int Version, string Mapping, IEnumerable<Dynamo> Objects)
        {
            Contract.Requires<ArgumentNullException>(Objects != null, "Objects");
            try
            {
                IDictionary<string, IAPIMapping> TempMappings = Mappings.GetValue(Version).Mappings;
                foreach (Dynamo Object in Objects)
                {
                    if (!TempMappings.ContainsKey(Mapping) || !TempMappings[Mapping].Save(Object))
                        return Error("Error saving the object");
                }
                return Success("Object saved successfully");
            }
            catch
            {
                return Error("Error saving the object");
            }
        }

        /// <summary>
        /// Saves a property
        /// </summary>
        /// <param name="Version">Version</param>
        /// <param name="Mapping">Model name</param>
        /// <param name="ID">ID</param>
        /// <param name="PropertyName">Property name</param>
        /// <param name="Models">Models</param>
        /// <returns>The result</returns>
        public Dynamo SaveProperty(int Version, string Mapping, string ID, string PropertyName, IEnumerable<Dynamo> Models)
        {
            try
            {
                IDictionary<string, IAPIMapping> TempMappings = Mappings.GetValue(Version).Mappings;
                if (!TempMappings.ContainsKey(Mapping) || !TempMappings[Mapping].SaveProperty(ID, Mappings.GetValue(Version), PropertyName, Models))
                    return Error("Error saving the object");
                return Success("Object saved successfully");
            }
            catch
            {
                return Error("Error saving the object");
            }
        }

        private static Dynamo Error(string Message)
        {
            if (HttpContext.Current != null)
            {
                HttpContext.Current.Response.StatusCode = 400;
                HttpContext.Current.Response.StatusDescription = "Bad Request";
            }
            return new Dynamo(new { Status = "Error", Message = Message });
        }

        private static Dynamo Success(string Message)
        {
            if (HttpContext.Current != null)
            {
                HttpContext.Current.Response.StatusCode = 200;
                HttpContext.Current.Response.StatusDescription = "Success";
            }
            return new Dynamo(new { Status = "Success", Message = Message });
        }
    }
}