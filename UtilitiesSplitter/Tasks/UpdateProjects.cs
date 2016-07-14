using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Utilities.IO;
using UtilitiesSplitter.Tasks.Interfaces;
using UtilitiesSplitter.Tasks.UtilClasses;

namespace UtilitiesSplitter.Tasks
{
    /// <summary>
    /// Update projects
    /// </summary>
    public class UpdateProjects : ITask
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name => "Update Projects";

        /// <summary>
        /// Gets or sets the order.
        /// </summary>
        /// <value>The order.</value>
        public int Order => 1;

        /// <summary>
        /// Runs this instance.
        /// </summary>
        /// <param name="PushToNuget">if set to <c>true</c> [push to nuget].</param>
        /// <returns>True if it runs successfully, false otherwise</returns>
        public bool Run(bool PushToNuget)
        {
            var Text = new FileInfo(@"..\..\..\Utilities\Utilities.csproj").Read();
            var Doc = new XmlDocument();
            Doc.LoadXml(Text);
            var Manager = new XmlNamespaceManager(Doc.NameTable);
            Manager.AddNamespace("Util", "http://schemas.microsoft.com/developer/msbuild/2003");
            var Nodes = Doc.DocumentElement.SelectNodes("//Util:ItemGroup/Util:Compile", Manager);
            var Projects = new List<Project>();
            foreach (XmlNode Node in Nodes)
            {
                string Path = Node.Attributes["Include"].InnerText;
                var Splitter = new string[] { "\\" };
                var PathItems = Path.Split(Splitter, StringSplitOptions.None);
                if (Projects.Find(x => x.Name == PathItems[0]) != null)
                {
                    Projects.Find(x => x.Name == PathItems[0]).Includes.Add(Path);
                }
                else if (PathItems[0] != "Properties" && PathItems[0] != "GlobalSuppressions.cs")
                {
                    var Temp = new Project();
                    Temp.Includes = new List<string>();
                    Temp.Name = PathItems[0];
                    Temp.Includes.Add(Path);
                    Projects.Add(Temp);
                }
            }
            Projects.ForEach(x => new DirectoryInfo("..\\..\\..\\Utilities\\" + x.Name)
                                    .CopyTo(new DirectoryInfo("..\\..\\..\\Utilities." + x.Name + "\\" + x.Name),
                                            Utilities.IO.Enums.CopyOptions.CopyIfNewer));
            foreach (Project Project in Projects.Where(x => new FileInfo("..\\..\\..\\Utilities." + x.Name + "\\Utilities." + x.Name + ".csproj").Exists))
            {
                bool Changed = false;
                var ProjectText = new FileInfo("..\\..\\..\\Utilities." + Project.Name + "\\Utilities." + Project.Name + ".csproj").Read();
                var ProjectDoc = new XmlDocument();

                ProjectDoc.LoadXml(ProjectText);
                var Manager2 = new XmlNamespaceManager(ProjectDoc.NameTable);
                Manager2.AddNamespace("Util", "http://schemas.microsoft.com/developer/msbuild/2003");
                Nodes = ProjectDoc.DocumentElement.SelectNodes("//Util:ItemGroup/Util:Compile", Manager2);
                foreach (XmlNode Node in Nodes)
                {
                    string Path = Node.Attributes["Include"].InnerText;
                    if (!Project.Includes.Remove(Path) && !Path.StartsWith("Properties", StringComparison.Ordinal))
                    {
                        Node.ParentNode.RemoveChild(Node);
                        new FileInfo("..\\..\\..\\Utilities." + Project.Name + "\\" + Path).Delete();
                        Changed = true;
                    }
                }

                XmlNode Parent = null;
                foreach (XmlNode Node in Nodes)
                {
                    Parent = Node.ParentNode;
                    if (Parent != null)
                        break;
                }
                foreach (string Path in Project.Includes)
                {
                    var Node = ProjectDoc.CreateElement("Compile", "http://schemas.microsoft.com/developer/msbuild/2003");
                    Node.RemoveAllAttributes();
                    var Attribute = ProjectDoc.CreateAttribute("Include");
                    Node.Attributes.Append(Attribute);
                    Attribute.InnerText = Path;
                    Parent.AppendChild(Node);
                    Changed = true;
                }
                if (Changed)
                {
                    ProjectDoc.Save("..\\..\\..\\Utilities." + Project.Name + "\\Utilities." + Project.Name + ".csproj");
                }
            }
            return true;
        }
    }
}