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
        /// <param name="Services">The services.</param>
        /// <param name="Modules">The workflow modules.</param>
        /// <param name="WorkflowManager">The workflow manager.</param>
        public Manager(IEnumerable<IAPIMapping> Mappings, IEnumerable<IService> Services, IEnumerable<IWorkflowModule> Modules, Utilities.Workflow.Manager.Manager WorkflowManager)
        {
            Contract.Requires<ArgumentNullException>(Mappings != null);
            Contract.Requires<ArgumentNullException>(Services != null);
            Contract.Requires<ArgumentNullException>(Modules != null);
            Contract.Requires<ArgumentNullException>(WorkflowManager != null);
            this.WorkflowManager = WorkflowManager;
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
            if (AppDomain.CurrentDomain.GetAssemblies()
                .Where(x => !x.FullName.Contains("vshost32") && !x.IsDynamic && !string.IsNullOrEmpty(x.Location))
                .Any(x => new System.IO.FileInfo(x.Location).LastWriteTime <= WorkflowManager.LastModified))
                return;

            foreach (int Version in this.Mappings.Keys)
            {
                foreach (IWorkflowModule Module in Modules.Where(x => x.Versions.Contains(Version)))
                {
                    foreach (IAPIMapping Mapping in this.Mappings[Version])
                    {
                        foreach (WorkflowType ActionType in Enum.GetValues(typeof(WorkflowType))
                            .OfType<WorkflowType>()
                            .Where(x => !x.HasFlag(WorkflowType.PreService)
                                && !x.HasFlag(WorkflowType.PostService)
                                && Module.ActionsTypes.HasFlag(x)))
                        {
                            Module.Setup(Mapping.Name, WorkflowManager.CreateWorkflow<WorkflowInfo>(Mapping.Name + "_" + ActionType.ToString() + "_" + Version));
                        }
                    }
                }
            }
            foreach (int Version in this.Services.Keys)
            {
                foreach (IWorkflowModule Module in Modules.Where(x => x.Versions.Contains(Version)))
                {
                    foreach (IService Service in this.Services[Version])
                    {
                        foreach (WorkflowType ActionType in new WorkflowType[] { WorkflowType.PreService, WorkflowType.PostService }
                            .Where(x => Module.ActionsTypes.HasFlag(x)))
                        {
                            Module.Setup(Service.Name, WorkflowManager.CreateWorkflow<WorkflowInfo>(Service.Name + "_" + ActionType.ToString() + "_" + Version));
                        }
                    }
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
        /// Gets or sets the workflow manager.
        /// </summary>
        /// <value>The workflow manager.</value>
        private Utilities.Workflow.Manager.Manager WorkflowManager { get; set; }

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
                if (!WorkflowManager.CreateWorkflow<WorkflowInfo>(Mapping + "_PreAll_" + Version).Start(new WorkflowInfo(Mapping, WorkflowType.PreAll, Version, null)).Continue)
                    return new List<Dynamo>();
                IDictionary<string, IAPIMapping> TempMappings = Mappings.GetValue(Version).Mappings;
                if (!TempMappings.ContainsKey(Mapping))
                    return new List<Dynamo>();
                var ReturnValue = TempMappings[Mapping].All(Mappings.GetValue(Version), EmbeddedProperties);
                if (!WorkflowManager.CreateWorkflow<WorkflowInfo>(Mapping + "_PostAll_" + Version).Start(new WorkflowInfo(Mapping, WorkflowType.PostAll, Version, ReturnValue)).Continue)
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
                if (!WorkflowManager.CreateWorkflow<WorkflowInfo>(Mapping + "_PreAny_" + Version).Start(new WorkflowInfo(Mapping, WorkflowType.PreAny, Version, null)).Continue)
                    return Error("Error getting item");
                IDictionary<string, IAPIMapping> TempMappings = Mappings.GetValue(Version).Mappings;
                if (!TempMappings.ContainsKey(Mapping))
                    return Error("Error getting item");
                var ReturnValue = TempMappings[Mapping].Any(ID, Mappings.GetValue(Version), EmbeddedProperties);
                if (!WorkflowManager.CreateWorkflow<WorkflowInfo>(Mapping + "_PostAny_" + Version).Start(new WorkflowInfo(Mapping, WorkflowType.PostAny, Version, ReturnValue)).Continue)
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
                if (!WorkflowManager.CreateWorkflow<WorkflowInfo>(Mapping + "_PreService_" + Version).Start(new WorkflowInfo(Mapping, WorkflowType.PreService, Version, Value)).Continue)
                    return Error("Error running service");
                IDictionary<string, IService> TempMappings = Services.GetValue(Version).Services;
                if (!TempMappings.ContainsKey(Mapping))
                    return Error("Error getting item");
                var ReturnValue = TempMappings[Mapping].Process(Value);
                if (!WorkflowManager.CreateWorkflow<WorkflowInfo>(Mapping + "_PostService_" + Version).Start(new WorkflowInfo(Mapping, WorkflowType.PostService, Version, ReturnValue)).Continue)
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
                if (!WorkflowManager.CreateWorkflow<WorkflowInfo>(Mapping + "_PreDelete_" + Version).Start(new WorkflowInfo(Mapping, WorkflowType.PreDelete, Version, ID)).Continue)
                    return Error("Error deleting the object");
                IDictionary<string, IAPIMapping> TempMappings = Mappings.GetValue(Version).Mappings;
                if (!TempMappings.ContainsKey(Mapping) || !TempMappings[Mapping].Delete(ID))
                    return Error("Error deleting the object");
                if (!WorkflowManager.CreateWorkflow<WorkflowInfo>(Mapping + "_PostDelete_" + Version).Start(new WorkflowInfo(Mapping, WorkflowType.PostDelete, Version, ID)).Continue)
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
        /// Gets the page count for the mapping specified
        /// </summary>
        /// <param name="Version">API version number</param>
        /// <param name="Mapping">Mapping name</param>
        /// <param name="PageSize">Size of the page.</param>
        /// <returns>The resulting items</returns>
        public Dynamo PageCount(int Version, string Mapping, int PageSize)
        {
            try
            {
                IDictionary<string, IAPIMapping> TempMappings = Mappings.GetValue(Version).Mappings;
                if (!TempMappings.ContainsKey(Mapping))
                    return new Dynamo(new { PageCount = 0 });
                var ReturnValue = new Dynamo(new { PageCount = TempMappings[Mapping].PageCount(Mappings.GetValue(Version), PageSize) });
                return ReturnValue;
            }
            catch
            {
                return new Dynamo(new { PageCount = 0 });
            }
        }

        /// <summary>
        /// Gets all items of the specified mapping of the specified page
        /// </summary>
        /// <param name="Version">API version number</param>
        /// <param name="Mapping">Mapping name</param>
        /// <param name="PageSize">Size of the page.</param>
        /// <param name="Page">The page desired</param>
        /// <param name="OrderBy">The order by clause</param>
        /// <param name="EmbeddedProperties">Embedded properties</param>
        /// <returns>The resulting items</returns>
        public IEnumerable<Dynamo> Paged(int Version, string Mapping, int PageSize, int Page, string[] OrderBy, string[] EmbeddedProperties)
        {
            try
            {
                if (!WorkflowManager.CreateWorkflow<WorkflowInfo>(Mapping + "_PrePaged_" + Version).Start(new WorkflowInfo(Mapping, WorkflowType.PreAll, Version, null)).Continue)
                    return new List<Dynamo>();
                IDictionary<string, IAPIMapping> TempMappings = Mappings.GetValue(Version).Mappings;
                if (!TempMappings.ContainsKey(Mapping))
                    return new List<Dynamo>();
                var ReturnValue = TempMappings[Mapping].Paged(Mappings.GetValue(Version), PageSize, Page, OrderBy, EmbeddedProperties);
                if (!WorkflowManager.CreateWorkflow<WorkflowInfo>(Mapping + "_PostPaged_" + Version).Start(new WorkflowInfo(Mapping, WorkflowType.PostAll, Version, ReturnValue)).Continue)
                    return new List<Dynamo>();
                return ReturnValue;
            }
            catch
            {
                return new List<Dynamo>();
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
            if (string.IsNullOrEmpty(AreaName))
                AreaName = ControllerName;
            foreach (int VersionNumber in Mappings.Keys)
            {
                routes.MapRoute(
"Ironman_API_Service",
AreaName + "/v" + VersionNumber + "/Services/{ServiceName}",
new { controller = ControllerName, action = "Services" },
new { httpMethod = new HttpMethodConstraint("POST", "GET") }
                );
                routes.MapRoute(
"Ironman_API_Service_Ending",
AreaName + "/v" + VersionNumber + "/Services/{ServiceName}.{ending}",
new { controller = ControllerName, action = "Services" },
new { httpMethod = new HttpMethodConstraint("POST", "GET") }
                );
                routes.MapRoute(
"Ironman_API_Save",
AreaName + "/v" + VersionNumber + "/{ModelName}",
new { controller = ControllerName, action = "Save" },
new { httpMethod = new HttpMethodConstraint("POST", "PUT", "PATCH") }
                );

                routes.MapRoute(
"Ironman_API_Save2",
AreaName + "/v" + VersionNumber + "/{ModelName}/{ID}",
new { controller = ControllerName, action = "Save" },
new { httpMethod = new HttpMethodConstraint("POST", "PUT", "PATCH") }
                );

                routes.MapRoute(
"Ironman_API_Delete",
AreaName + "/v" + VersionNumber + "/{ModelName}/{ID}",
new { controller = ControllerName, action = "Delete" },
new { httpMethod = new HttpMethodConstraint("DELETE") }
                );

                routes.MapRoute(
"Ironman_API_SaveProperty",
AreaName + "/v" + VersionNumber + "/{ModelName}/{ID}/{PropertyName}",
new { controller = ControllerName, action = "SaveProperty" },
new { httpMethod = new HttpMethodConstraint("POST", "PUT", "PATCH") }
                );

                routes.MapRoute(
"Ironman_API_SaveProperty2",
AreaName + "/v" + VersionNumber + "/{ModelName}/{ID}/{PropertyName}/{PropertyID}",
new { controller = ControllerName, action = "SaveProperty" },
new { httpMethod = new HttpMethodConstraint("POST", "PUT", "PATCH") }
                );

                routes.MapRoute(
"Ironman_API_DeleteProperty",
AreaName + "/v" + VersionNumber + "/{ModelName}/{ID}/{PropertyName}/{PropertyID}",
new { controller = ControllerName, action = "DeleteProperty" },
new { httpMethod = new HttpMethodConstraint("DELETE") }
                );

                routes.MapRoute(
"Ironman_API_GetPageCount",
AreaName + "/v" + VersionNumber + "/{ModelName}/Paged",
new { controller = ControllerName, action = "PageCount" },
new { httpMethod = new HttpMethodConstraint("GET") }
                );

                routes.MapRoute(
"Ironman_API_GetPaged",
AreaName + "/v" + VersionNumber + "/{ModelName}/Paged/{PageNumber}",
new { controller = ControllerName, action = "Paged" },
new { httpMethod = new HttpMethodConstraint("GET") }
                );

                routes.MapRoute(
"Ironman_API_GetProperty",
AreaName + "/v" + VersionNumber + "/{ModelName}/{ID}/{PropertyName}",
new { controller = ControllerName, action = "GetProperty" },
new { httpMethod = new HttpMethodConstraint("GET") }
                );

                routes.MapRoute(
"Ironman_API_Any",
AreaName + "/v" + VersionNumber + "/{ModelName}/{ID}",
new { controller = ControllerName, action = "Any" },
new { httpMethod = new HttpMethodConstraint("GET") }
                );

                routes.MapRoute(
"Ironman_API_All",
AreaName + "/v" + VersionNumber + "/{ModelName}",
new { controller = ControllerName, action = "All" },
new { httpMethod = new HttpMethodConstraint("GET") }
                );

                routes.MapRoute(
"Ironman_API_Save_Ending",
AreaName + "/v" + VersionNumber + "/{ModelName}.{ending}",
new { controller = ControllerName, action = "Save" },
new { httpMethod = new HttpMethodConstraint("POST", "PUT", "PATCH") }
                );

                routes.MapRoute(
"Ironman_API_Save2_Ending",
AreaName + "/v" + VersionNumber + "/{ModelName}/{ID}.{ending}",
new { controller = ControllerName, action = "Save" },
new { httpMethod = new HttpMethodConstraint("POST", "PUT", "PATCH") }
                );

                routes.MapRoute(
"Ironman_API_Delete_Ending",
AreaName + "/v" + VersionNumber + "/{ModelName}/{ID}.{ending}",
new { controller = ControllerName, action = "Delete" },
new { httpMethod = new HttpMethodConstraint("DELETE") }
                );

                routes.MapRoute(
"Ironman_API_SaveProperty_Ending",
AreaName + "/v" + VersionNumber + "/{ModelName}/{ID}/{PropertyName}.{ending}",
new { controller = ControllerName, action = "SaveProperty" },
new { httpMethod = new HttpMethodConstraint("POST", "PUT", "PATCH") }
                );

                routes.MapRoute(
"Ironman_API_SaveProperty2_Ending",
AreaName + "/v" + VersionNumber + "/{ModelName}/{ID}/{PropertyName}/{PropertyID}.{ending}",
new { controller = ControllerName, action = "SaveProperty" },
new { httpMethod = new HttpMethodConstraint("POST", "PUT", "PATCH") }
                );

                routes.MapRoute(
"Ironman_API_DeleteProperty_Ending",
AreaName + "/v" + VersionNumber + "/{ModelName}/{ID}/{PropertyName}/{PropertyID}.{ending}",
new { controller = ControllerName, action = "DeleteProperty" },
new { httpMethod = new HttpMethodConstraint("DELETE") }
                );

                routes.MapRoute(
"Ironman_API_GetProperty_Ending",
AreaName + "/v" + VersionNumber + "/{ModelName}/{ID}/{PropertyName}.{ending}",
new { controller = ControllerName, action = "GetProperty" },
new { httpMethod = new HttpMethodConstraint("GET") }
                );

                routes.MapRoute(
"Ironman_API_Any_Ending",
AreaName + "/v" + VersionNumber + "/{ModelName}/{ID}.{ending}",
new { controller = ControllerName, action = "Any" },
new { httpMethod = new HttpMethodConstraint("GET") }
                );

                routes.MapRoute(
"Ironman_API_All_Ending",
AreaName + "/v" + VersionNumber + "/{ModelName}.{ending}",
new { controller = ControllerName, action = "All" },
new { httpMethod = new HttpMethodConstraint("GET") }
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
                if (!WorkflowManager.CreateWorkflow<WorkflowInfo>(Mapping + "_PreSave_" + Version).Start(new WorkflowInfo(Mapping, WorkflowType.PreSave, Version, Objects)).Continue)
                    return Error("Error saving the object");
                IDictionary<string, IAPIMapping> TempMappings = Mappings.GetValue(Version).Mappings;
                foreach (Dynamo Object in Objects)
                {
                    if (!TempMappings.ContainsKey(Mapping) || !TempMappings[Mapping].Save(Object))
                        return Error("Error saving the object");
                }
                if (!WorkflowManager.CreateWorkflow<WorkflowInfo>(Mapping + "_PostSave_" + Version).Start(new WorkflowInfo(Mapping, WorkflowType.PostSave, Version, Objects)).Continue)
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
            return "API Mappings:\r\n" + Mappings.ToString(x => "Version: " + x.Key + "\r\n" + x.Value.ToString() + "\r\n");
        }

        private static Dynamo Error(string Message)
        {
            if (HttpContext.Current != null)
            {
                HttpContext.Current.Response.StatusCode = 400;
                HttpContext.Current.Response.StatusDescription = "Bad Request";
            }
            return new Dynamo(new { Status = "Error", Message });
        }

        private static Dynamo Success(string Message)
        {
            if (HttpContext.Current != null)
            {
                HttpContext.Current.Response.StatusCode = 200;
                HttpContext.Current.Response.StatusDescription = "Success";
            }
            return new Dynamo(new { Status = "Success", Message });
        }
    }
}