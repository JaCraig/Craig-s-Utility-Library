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
using System.Web.Routing;
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
        /// <param name="Mappings">The mappings.</param>
        /// <param name="Workflows">The workflows.</param>
        /// <param name="Services">The services.</param>
        public Manager(IEnumerable<IAPIMapping> Mappings, IEnumerable<IWorkflow> Workflows, IEnumerable<IService> Services)
            : base()
        {
            this.Workflows = Workflows;
            this.Services = new Dictionary<int, ServiceHolder>();
            this.Mappings = new Dictionary<int, MappingHolder>();
            if (Mappings == null)
                return;
            foreach (IAPIMapping Mapping in Mappings)
            {
                foreach (int Version in Mapping.Versions)
                {
                    if (!this.Mappings.ContainsKey(Version))
                        this.Mappings.Add(Version, new MappingHolder());
                    this.Mappings[Version].Mappings.Add(Mapping.Name, Mapping);
                }
            }
            if (Services == null)
                return;
            foreach (IService Service in Services)
            {
                foreach (int Version in Service.Versions)
                {
                    if (!this.Services.ContainsKey(Version))
                        this.Services.Add(Version, new ServiceHolder());
                    this.Services[Version].Services.Add(Service.Name, Service);
                }
            }
        }

        /// <summary>
        /// Gets or sets the mappings.
        /// </summary>
        /// <value>The mappings.</value>
        private IDictionary<int, MappingHolder> Mappings { get; set; }

        /// <summary>
        /// Gets or sets the services.
        /// </summary>
        /// <value>The services.</value>
        private IDictionary<int, ServiceHolder> Services { get; set; }

        /// <summary>
        /// Gets or sets the workflows.
        /// </summary>
        /// <value>The workflows.</value>
        private IEnumerable<IWorkflow> Workflows { get; set; }

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
                if (Workflows.Any(x => !x.PreAll(Mapping)))
                    return new List<Dynamo>();
                IDictionary<string, IAPIMapping> TempMappings = Mappings.GetValue(Version).Mappings;
                if (!TempMappings.ContainsKey(Mapping))
                    return new List<Dynamo>();
                IEnumerable<Dynamo> ReturnValue = TempMappings[Mapping].All(Mappings.GetValue(Version), EmbeddedProperties);
                if (Workflows.Any(x => !x.PostAll(Mapping, ReturnValue)))
                    return new List<Dynamo>();
                return ReturnValue;
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
                if (Workflows.Any(x => !x.PreAny(Mapping)))
                    return Error("Error getting item");
                IDictionary<string, IAPIMapping> TempMappings = Mappings.GetValue(Version).Mappings;
                if (!TempMappings.ContainsKey(Mapping))
                    return Error("Error getting item");
                Dynamo ReturnValue = TempMappings[Mapping].Any(ID, Mappings.GetValue(Version), EmbeddedProperties);
                if (Workflows.Any(x => !x.PostAny(Mapping, ReturnValue)))
                    return Error("Error getting item");
                return ReturnValue;
            }
            catch
            {
                return new Dynamo();
            }
        }

        /// <summary>
        /// Runs the specified service
        /// </summary>
        /// <param name="Version">API version number</param>
        /// <param name="Mapping">Mapping name</param>
        /// <param name="Value">The value.</param>
        /// <returns>The result from running the service</returns>
        public Dynamo CallService(int Version, string Mapping, Dynamo Value)
        {
            try
            {
                if (Workflows.Any(x => !x.PreService(Mapping, Value)))
                    return Error("Error running service");
                IDictionary<string, IService> TempMappings = Services.GetValue(Version).Services;
                if (!TempMappings.ContainsKey(Mapping))
                    return Error("Error getting item");
                Dynamo ReturnValue = TempMappings[Mapping].Process(Value);
                if (Workflows.Any(x => !x.PostService(Mapping, ReturnValue)))
                    return Error("Error running service");
                return ReturnValue;
            }
            catch
            {
                return Error("Error running service");
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
                if (Workflows.Any(x => !x.PreDelete(Mapping, ID)))
                    return Error("Error deleting the object");
                IDictionary<string, IAPIMapping> TempMappings = Mappings.GetValue(Version).Mappings;
                if (!TempMappings.ContainsKey(Mapping) || !TempMappings[Mapping].Delete(ID))
                    return Error("Error deleting the object");
                if (Workflows.Any(x => !x.PostDelete(Mapping, ID)))
                    return Error("Error deleting the object");
                return Success("Object deleted successfully");
            }
            catch
            {
                return Error("Error deleting the object");
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
                return Error("Error getting the object");
            }
        }

        /// <summary>
        /// Registers the routes used by the API system
        /// </summary>
        /// <param name="routes">The routing table</param>
        /// <param name="ControllerName">Name of the controller.</param>
        /// <param name="AreaName">Name of the area (leave blank if you're not using one).</param>
        public void RegisterRoutes(RouteCollection routes, string ControllerName = "API", string AreaName = "")
        {
            Contract.Requires<ArgumentNullException>(routes != null, "routes");
            Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(ControllerName), "ControllerName");
            AreaName = AreaName.Check(ControllerName);
            foreach (int VersionNumber in Mappings.Keys)
            {
                routes.MapRoute(
                    name: "Ironman_API_Service",
                    url: AreaName + "/v" + VersionNumber + "/Services/{ServiceName}",
                    defaults: new { controller = ControllerName, action = "Services" }
                );
                routes.MapRoute(
                    name: "Ironman_API_Service_Ending",
                    url: AreaName + "/v" + VersionNumber + "/Services/{ServiceName}.{ending}",
                    defaults: new { controller = ControllerName, action = "Services" }
                );
                routes.MapRoute(
                    name: "Ironman_API_Save",
                    url: AreaName + "/v" + VersionNumber + "/{ModelName}",
                    defaults: new { controller = ControllerName, action = "Save" },
                    constraints: new { httpMethod = new HttpMethodConstraint("POST", "PUT", "PATCH") }
                );

                routes.MapRoute(
                    name: "Ironman_API_Save2",
                    url: AreaName + "/v" + VersionNumber + "/{ModelName}/{ID}",
                    defaults: new { controller = ControllerName, action = "Save" },
                    constraints: new { httpMethod = new HttpMethodConstraint("POST", "PUT", "PATCH") }
                );

                routes.MapRoute(
                    name: "Ironman_API_Delete",
                    url: AreaName + "/v" + VersionNumber + "/{ModelName}/{ID}",
                    defaults: new { controller = ControllerName, action = "Delete" },
                    constraints: new { httpMethod = new HttpMethodConstraint("DELETE") }
                );

                routes.MapRoute(
                    name: "Ironman_API_SaveProperty",
                    url: AreaName + "/v" + VersionNumber + "/{ModelName}/{ID}/{PropertyName}",
                    defaults: new { controller = ControllerName, action = "SaveProperty" },
                    constraints: new { httpMethod = new HttpMethodConstraint("POST", "PUT", "PATCH") }
                );

                routes.MapRoute(
                    name: "Ironman_API_SaveProperty2",
                    url: AreaName + "/v" + VersionNumber + "/{ModelName}/{ID}/{PropertyName}/{PropertyID}",
                    defaults: new { controller = ControllerName, action = "SaveProperty" },
                    constraints: new { httpMethod = new HttpMethodConstraint("POST", "PUT", "PATCH") }
                );

                routes.MapRoute(
                    name: "Ironman_API_DeleteProperty",
                    url: AreaName + "/v" + VersionNumber + "/{ModelName}/{ID}/{PropertyName}/{PropertyID}",
                    defaults: new { controller = ControllerName, action = "DeleteProperty" },
                    constraints: new { httpMethod = new HttpMethodConstraint("DELETE") }
                );

                routes.MapRoute(
                    name: "Ironman_API_GetProperty",
                    url: AreaName + "/v" + VersionNumber + "/{ModelName}/{ID}/{PropertyName}",
                    defaults: new { controller = ControllerName, action = "GetProperty" },
                    constraints: new { httpMethod = new HttpMethodConstraint("GET") }
                );

                routes.MapRoute(
                    name: "Ironman_API_Any",
                    url: AreaName + "/v" + VersionNumber + "/{ModelName}/{ID}",
                    defaults: new { controller = ControllerName, action = "Any" },
                    constraints: new { httpMethod = new HttpMethodConstraint("GET") }
                );

                routes.MapRoute(
                    name: "Ironman_API_All",
                    url: AreaName + "/v" + VersionNumber + "/{ModelName}",
                    defaults: new { controller = ControllerName, action = "All" },
                    constraints: new { httpMethod = new HttpMethodConstraint("GET") }
                );

                routes.MapRoute(
                    name: "Ironman_API_Save_Ending",
                    url: AreaName + "/v" + VersionNumber + "/{ModelName}.{ending}",
                    defaults: new { controller = ControllerName, action = "Save" },
                    constraints: new { httpMethod = new HttpMethodConstraint("POST", "PUT", "PATCH") }
                );

                routes.MapRoute(
                    name: "Ironman_API_Save2_Ending",
                    url: AreaName + "/v" + VersionNumber + "/{ModelName}/{ID}.{ending}",
                    defaults: new { controller = ControllerName, action = "Save" },
                    constraints: new { httpMethod = new HttpMethodConstraint("POST", "PUT", "PATCH") }
                );

                routes.MapRoute(
                    name: "Ironman_API_Delete_Ending",
                    url: AreaName + "/v" + VersionNumber + "/{ModelName}/{ID}.{ending}",
                    defaults: new { controller = ControllerName, action = "Delete" },
                    constraints: new { httpMethod = new HttpMethodConstraint("DELETE") }
                );

                routes.MapRoute(
                    name: "Ironman_API_SaveProperty_Ending",
                    url: AreaName + "/v" + VersionNumber + "/{ModelName}/{ID}/{PropertyName}.{ending}",
                    defaults: new { controller = ControllerName, action = "SaveProperty" },
                    constraints: new { httpMethod = new HttpMethodConstraint("POST", "PUT", "PATCH") }
                );

                routes.MapRoute(
                    name: "Ironman_API_SaveProperty2_Ending",
                    url: AreaName + "/v" + VersionNumber + "/{ModelName}/{ID}/{PropertyName}/{PropertyID}.{ending}",
                    defaults: new { controller = ControllerName, action = "SaveProperty" },
                    constraints: new { httpMethod = new HttpMethodConstraint("POST", "PUT", "PATCH") }
                );

                routes.MapRoute(
                    name: "Ironman_API_DeleteProperty_Ending",
                    url: AreaName + "/v" + VersionNumber + "/{ModelName}/{ID}/{PropertyName}/{PropertyID}.{ending}",
                    defaults: new { controller = ControllerName, action = "DeleteProperty" },
                    constraints: new { httpMethod = new HttpMethodConstraint("DELETE") }
                );

                routes.MapRoute(
                    name: "Ironman_API_GetProperty_Ending",
                    url: AreaName + "/v" + VersionNumber + "/{ModelName}/{ID}/{PropertyName}.{ending}",
                    defaults: new { controller = ControllerName, action = "GetProperty" },
                    constraints: new { httpMethod = new HttpMethodConstraint("GET") }
                );

                routes.MapRoute(
                    name: "Ironman_API_Any_Ending",
                    url: AreaName + "/v" + VersionNumber + "/{ModelName}/{ID}.{ending}",
                    defaults: new { controller = ControllerName, action = "Any" },
                    constraints: new { httpMethod = new HttpMethodConstraint("GET") }
                );

                routes.MapRoute(
                    name: "Ironman_API_All_Ending",
                    url: AreaName + "/v" + VersionNumber + "/{ModelName}.{ending}",
                    defaults: new { controller = ControllerName, action = "All" },
                    constraints: new { httpMethod = new HttpMethodConstraint("GET") }
                );
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
                if (Workflows.Any(x => !x.PreSave(Mapping, Objects)))
                    return Error("Error saving the object");
                IDictionary<string, IAPIMapping> TempMappings = Mappings.GetValue(Version).Mappings;
                foreach (Dynamo Object in Objects)
                {
                    if (!TempMappings.ContainsKey(Mapping) || !TempMappings[Mapping].Save(Object))
                        return Error("Error saving the object");
                }
                if (Workflows.Any(x => !x.PostSave(Mapping, Objects)))
                    return Error("Error saving the object");
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

        /// <summary>
        /// Outputs manager info as a string
        /// </summary>
        /// <returns>String version of the manager</returns>
        public override string ToString()
        {
            return "API Mappings:\r\n" + Mappings.ToString(x => "Version: " + x.Key + "\r\n" + x.Value.ToString() + "\r\n")
                + "Workflows:\r\n" + Workflows.ToString(x => x.Name, "\r\n");
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