using System;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using Utilities.IO.ExtensionMethods;

namespace UtilitiesSplitter
{
    class Program
    {
        static void Main(string[] args)
        {
            string Text = new FileInfo(@"..\..\..\Utilities\Utilities.csproj").Read();
            XmlDocument Doc = new XmlDocument();

            Doc.LoadXml(Text);
            XmlNamespaceManager Manager = new XmlNamespaceManager(Doc.NameTable);
            Manager.AddNamespace("Util", "http://schemas.microsoft.com/developer/msbuild/2003");
            XmlNodeList Nodes = Doc.DocumentElement.SelectNodes("//Util:ItemGroup/Util:Compile", Manager);
            List<Project> Projects = new List<Project>();
            foreach (XmlNode Node in Nodes)
            {
                string Path = Node.Attributes["Include"].InnerText;
                string[] Splitter = new string[] { "\\" };
                string[] PathItems = Path.Split(Splitter, StringSplitOptions.None);
                if (Projects.Find(x => x.Name == PathItems[0]) != null)
                {
                    Projects.Find(x => x.Name == PathItems[0]).Includes.Add(Path);
                }
                else if (PathItems[0] != "Properties")
                {
                    Project Temp = new Project();
                    Temp.Includes = new List<string>();
                    Temp.Name = PathItems[0];
                    Temp.Includes.Add(Path);
                    Projects.Add(Temp);
                }
            }
            foreach (Project Project in Projects)
            {
                new DirectoryInfo("..\\..\\..\\Utilities\\" + Project.Name).CopyTo("..\\..\\..\\Utilities." + Project.Name + "\\" + Project.Name, true, Utilities.IO.ExtensionMethods.Enums.CopyOptions.CopyIfNewer);
            }
            foreach (Project Project in Projects)
            {
                if (new FileInfo("..\\..\\..\\Utilities." + Project.Name + "\\Utilities." + Project.Name + ".csproj").Exists)
                {
                    bool Changed = false;
                    string ProjectText = new FileInfo("..\\..\\..\\Utilities." + Project.Name + "\\Utilities." + Project.Name + ".csproj").Read();
                    XmlDocument ProjectDoc = new XmlDocument();

                    ProjectDoc.LoadXml(ProjectText);
                    XmlNamespaceManager Manager2 = new XmlNamespaceManager(ProjectDoc.NameTable);
                    Manager2.AddNamespace("Util", "http://schemas.microsoft.com/developer/msbuild/2003");
                    Nodes = ProjectDoc.DocumentElement.SelectNodes("//Util:ItemGroup/Util:Compile", Manager2);
                    foreach (XmlNode Node in Nodes)
                    {
                        string Path = Node.Attributes["Include"].InnerText;
                        if (Project.Includes.Contains(Path))
                        {
                            Project.Includes.Remove(Path);
                        }
                        else if (!Path.StartsWith("Properties"))
                        {
                            Node.ParentNode.RemoveChild(Node);
                            new FileInfo("..\\..\\..\\Utilities." + Project.Name + "\\" + Path).Delete();
                            Changed = true;
                        }
                    }

                    XmlNode Parent = Nodes[0].ParentNode;
                    foreach (string Path in Project.Includes)
                    {
                        XmlElement Node = ProjectDoc.CreateElement("Compile", "http://schemas.microsoft.com/developer/msbuild/2003");
                        Node.RemoveAllAttributes();
                        XmlAttribute Attribute = ProjectDoc.CreateAttribute("Include");
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
            }
        }
    }

    public class Project
    {
        public string Name { get; set; }
        public List<string> Includes { get; set; }
    }
}