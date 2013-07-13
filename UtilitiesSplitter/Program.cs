using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml;
using Utilities.IO.ExtensionMethods;
using System.Linq;

namespace UtilitiesSplitter
{
    class Program
    {
        static void Main(string[] args)
        {
            UpdateProjects();
            ReversionPackages();
            if (args.Length > 0 && args[0] == "Push")
            {
                SetupDependencies();
                SetupInternalDependencies();
                CreatePackages();
                PushPackages();
            }
        }

        private static void PushPackages()
        {
            foreach (FileInfo File in new DirectoryInfo("..\\..\\..\\UtilitiesPackages\\").EnumerateFiles("*.nupkg", SearchOption.AllDirectories))
            {
                Process NugetProcess = new FileInfo("..\\..\\..\\.nuget\\nuget.exe").Execute("push \"" + File.FullName + "\"");
                NugetProcess.WaitForExit();
            }
        }

        private static void CreatePackages()
        {
            new DirectoryInfo("..\\..\\..\\UtilitiesPackages\\Packages").Create();
            new DirectoryInfo("..\\..\\..\\UtilitiesPackages\\Packages").DeleteFiles();
            new FileInfo("..\\..\\..\\README.md").CopyTo("..\\..\\..\\UtilitiesPackages\\readme.txt");

            foreach (FileInfo File in new DirectoryInfo("..\\..\\..\\UtilitiesPackages\\").EnumerateFiles("*.nuspec", SearchOption.AllDirectories).Where(x => !x.Name.Contains("Documentation")))
            {
                new DirectoryInfo("..\\..\\..\\UtilitiesPackages\\lib").Create();
                new DirectoryInfo("..\\..\\..\\UtilitiesPackages\\tools").Create();
                new DirectoryInfo("..\\..\\..\\UtilitiesPackages\\content").Create();

                new DirectoryInfo("..\\..\\..\\" + File.Name.Replace(".nuspec", "") + "\\bin\\Release").CopyTo("..\\..\\..\\UtilitiesPackages\\lib");
                Process NugetProcess = new FileInfo("..\\..\\..\\.nuget\\nuget.exe").Execute("pack \"" + File.FullName + "\"", WorkingDirectory: "..\\..\\..\\UtilitiesPackages\\Packages");
                NugetProcess.WaitForExit();

                new DirectoryInfo("..\\..\\..\\UtilitiesPackages\\lib").DeleteAll();
                new DirectoryInfo("..\\..\\..\\UtilitiesPackages\\tools").DeleteAll();
                new DirectoryInfo("..\\..\\..\\UtilitiesPackages\\content").DeleteAll();
            }
            foreach (FileInfo File in new DirectoryInfo("..\\..\\..\\UtilitiesPackages\\").EnumerateFiles("*.nuspec", SearchOption.AllDirectories).Where(x => x.Name.Contains("Documentation")))
            {
                new DirectoryInfo("..\\..\\..\\UtilitiesPackages\\lib").Create();
                new DirectoryInfo("..\\..\\..\\UtilitiesPackages\\tools").Create();
                new DirectoryInfo("..\\..\\..\\UtilitiesPackages\\content").Create();

                Process DoxygenProcess = "doxygen.exe".Execute("\"" + File.FullName.Replace(".nuspec", ".doxy") + "\"", WorkingDirectory: "..\\..\\..\\UtilitiesPackages");
                DoxygenProcess.WaitForExit();
                Process NugetProcess = new FileInfo("..\\..\\..\\.nuget\\nuget.exe").Execute("pack \"" + File.FullName + "\"", WorkingDirectory: "..\\..\\..\\UtilitiesPackages\\Packages");
                NugetProcess.WaitForExit();

                new DirectoryInfo("..\\..\\..\\UtilitiesPackages\\lib").DeleteAll();
                new DirectoryInfo("..\\..\\..\\UtilitiesPackages\\tools").DeleteAll();
                new DirectoryInfo("..\\..\\..\\UtilitiesPackages\\content").DeleteAll();
            }
            new FileInfo("..\\..\\..\\UtilitiesPackages\\readme.txt").Delete();
        }

        private static void UpdateProjects()
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

        private static void ReversionPackages()
        {
            foreach (FileInfo File in new DirectoryInfo("..\\..\\..\\UtilitiesPackages\\").EnumerateFiles("*.nuspec", SearchOption.AllDirectories))
            {
                string FileContents = File.Read();
                Match VersionMatch = Regex.Match(FileContents, "<version>(?<VersionNumber>.*)</version>");
                string[] VersionInfo = VersionMatch.Groups["VersionNumber"].Value.Split('.');
                string NewVersion = VersionInfo[0] + "." + VersionInfo[1] + ".";
                if (VersionInfo.Length > 2)
                    NewVersion += (int.Parse(VersionInfo[2]) + 1).ToString("D4");
                else
                    NewVersion += "0001";
                File.Save(Regex.Replace(FileContents, "<version>(?<VersionNumber>.*)</version>", "<version>" + NewVersion + "</version>"));
            }
        }

        private static void SetupDependencies()
        {
            foreach (FileInfo File in new DirectoryInfo("..\\..\\..\\UtilitiesPackages\\").EnumerateFiles("*.nuspec", SearchOption.AllDirectories))
            {
                FileInfo PackagesFile = new FileInfo("..\\..\\..\\" + File.Name.Replace(".nuspec", "") + "\\packages.config");
                if (PackagesFile.Exists)
                {
                    string PackagesContent = PackagesFile.Read();
                    string FileContent = File.Read();
                    foreach (Match Package in Regex.Matches(PackagesContent, @"<package id=""(?<Package>[^""]*)"" version=""(?<Version>[^""]*)"""))
                    {
                        if (Regex.IsMatch(FileContent, @"<dependency id=""" + Package.Groups["Package"].Value + @""" version=""(?<VersionNumber>[^""]*)"" />"))
                        {
                            Match TempMatch = Regex.Match(FileContent, @"<dependency id=""" + Package.Groups["Package"].Value + @""" version=""(?<VersionNumber>[^""]*)"" />");
                            FileContent = FileContent.Replace(TempMatch.Value, @"<dependency id=""" + Package.Groups["Package"].Value + @""" version=""[" + Package.Groups["Version"].Value + @"]"" />");
                        }
                    }
                    File.Save(FileContent);
                }
            }
        }

        private static void SetupInternalDependencies()
        {
            foreach (FileInfo File in new DirectoryInfo("..\\..\\..\\UtilitiesPackages\\").EnumerateFiles("*.nuspec", SearchOption.AllDirectories))
            {
                string FileContent = File.Read();
                string CurrentVersion = Regex.Match(FileContent, "<version>(?<VersionNumber>.*)</version>").Groups["VersionNumber"].Value;
                FileContent = Regex.Replace(FileContent,
                                @"<dependency id=""CraigsUtilityLibrary-(?<Project>[^""]*)"" version=""(?<VersionNumber>[^""]*)"" />",
                                x => @"<dependency id=""CraigsUtilityLibrary-" + x.Groups["Project"].Value + @""" version=""[" + CurrentVersion + @"]"" />");
                File.Save(FileContent);
            }
        }
    }

    public class Project
    {
        public string Name { get; set; }
        public List<string> Includes { get; set; }
    }
}