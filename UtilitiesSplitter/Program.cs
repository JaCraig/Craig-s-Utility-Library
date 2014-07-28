using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml;
using Utilities.IO;

namespace UtilitiesSplitter
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var Bootstrapper = Utilities.IoC.Manager.Bootstrapper;
                UpdateProjects();
                ReversionPackages();
                SetupDependencies();
                SetupInternalDependencies();
                if (args.Length > 0 && args[0] == "Push")
                {
                    CreatePackages();
                    PushPackages();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Console.ReadKey();
            }
        }

        private static void CreatePackages()
        {
            new DirectoryInfo("..\\..\\..\\UtilitiesPackages\\Packages").Create();
            new DirectoryInfo("..\\..\\..\\UtilitiesPackages\\Packages").EnumerateFiles("*", System.IO.SearchOption.AllDirectories);
            new FileInfo("..\\..\\..\\README.md").CopyTo(new DirectoryInfo("..\\..\\..\\UtilitiesPackages"), true).Rename("readme.txt");

            foreach (FileInfo File in new DirectoryInfo("..\\..\\..\\UtilitiesPackages\\").EnumerateFiles("*.nuspec", System.IO.SearchOption.AllDirectories).Where(x => !x.Name.Contains("Documentation")))
            {
                new DirectoryInfo("..\\..\\..\\UtilitiesPackages\\lib").Create();
                new DirectoryInfo("..\\..\\..\\UtilitiesPackages\\tools").Create();
                new DirectoryInfo("..\\..\\..\\UtilitiesPackages\\content").Create();

                if (!File.Name.Contains("Ironman.Default"))
                    new DirectoryInfo("..\\..\\..\\" + File.Name.Replace(".nuspec", "") + "\\bin\\Release").CopyTo(new DirectoryInfo("..\\..\\..\\UtilitiesPackages\\lib"));
                Process NugetProcess = new FileInfo("..\\..\\..\\.nuget\\nuget.exe").Execute(new ProcessStartInfo() { Arguments = "pack \"" + File.FullName + "\"", WorkingDirectory = "..\\..\\..\\UtilitiesPackages\\Packages", CreateNoWindow = false });
                NugetProcess.WaitForExit();

                while (new DirectoryInfo("..\\..\\..\\UtilitiesPackages\\lib").Exists)
                {
                    try
                    {
                        new DirectoryInfo("..\\..\\..\\UtilitiesPackages\\lib").Delete();
                        new DirectoryInfo("..\\..\\..\\UtilitiesPackages\\tools").Delete();
                        new DirectoryInfo("..\\..\\..\\UtilitiesPackages\\content").Delete();
                    }
                    catch (System.IO.IOException) { }
                }
            }
            foreach (FileInfo File in new DirectoryInfo("..\\..\\..\\UtilitiesPackages\\").EnumerateFiles("*.nuspec", System.IO.SearchOption.AllDirectories).Where(x => x.Name.Contains("Documentation")))
            {
                new DirectoryInfo("..\\..\\..\\UtilitiesPackages\\lib").Create();
                new DirectoryInfo("..\\..\\..\\UtilitiesPackages\\tools").Create();
                new DirectoryInfo("..\\..\\..\\UtilitiesPackages\\content").Create();

                Process DoxygenProcess = new FileInfo(@"C:\Program Files\doxygen\bin\doxygen.exe").Execute(new ProcessStartInfo() { Arguments = "\"" + File.FullName.Replace(".nuspec", ".doxy") + "\"", WorkingDirectory = "..\\..\\..\\UtilitiesPackages", CreateNoWindow = false });
                DoxygenProcess.WaitForExit();
                Process NugetProcess = new FileInfo("..\\..\\..\\.nuget\\nuget.exe").Execute(new ProcessStartInfo() { Arguments = "pack \"" + File.FullName + "\"", WorkingDirectory = "..\\..\\..\\UtilitiesPackages\\Packages", CreateNoWindow = false });
                NugetProcess.WaitForExit();

                new DirectoryInfo("..\\..\\..\\UtilitiesPackages\\lib").Delete();
                new DirectoryInfo("..\\..\\..\\UtilitiesPackages\\tools").Delete();
                new DirectoryInfo("..\\..\\..\\UtilitiesPackages\\content").Delete();
            }
            new FileInfo("..\\..\\..\\UtilitiesPackages\\readme.txt").Delete();
        }

        private static void PushPackages()
        {
            foreach (FileInfo File in new DirectoryInfo("..\\..\\..\\UtilitiesPackages\\").EnumerateFiles("*.nupkg", System.IO.SearchOption.AllDirectories))
            {
                Process NugetProcess = new FileInfo("..\\..\\..\\.nuget\\nuget.exe").Execute(new ProcessStartInfo() { Arguments = "push \"" + File.FullName + "\"", CreateNoWindow = false });
                NugetProcess.WaitForExit();
            }
            try
            {
                new DirectoryInfo("..\\..\\..\\UtilitiesPackages\\Packages\\").Delete();
            }
            catch { }
        }

        private static void ReversionPackages()
        {
            foreach (FileInfo File in new DirectoryInfo("..\\..\\..\\UtilitiesPackages\\").EnumerateFiles("*.nuspec", System.IO.SearchOption.AllDirectories))
            {
                string FileContents = File.Read();
                Match VersionMatch = Regex.Match(FileContents, "<version>(?<VersionNumber>.*)</version>");
                string[] VersionInfo = VersionMatch.Groups["VersionNumber"].Value.Replace("-beta", "").Split('.');
                string NewVersion = VersionInfo[0] + "." + VersionInfo[1] + ".";
                if (VersionInfo.Length > 2)
                    NewVersion += (int.Parse(VersionInfo[2]) + 1).ToString();
                else
                    NewVersion += "1";
                File.Write(Regex.Replace(FileContents, "<version>(?<VersionNumber>.*)</version>", "<version>" + NewVersion + "-beta</version>"));
            }
        }

        private static void SetupDependencies()
        {
            foreach (FileInfo File in new DirectoryInfo("..\\..\\..\\UtilitiesPackages\\").EnumerateFiles("*.nuspec", System.IO.SearchOption.AllDirectories))
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
                    File.Write(FileContent);
                }
            }
        }

        private static void SetupInternalDependencies()
        {
            foreach (FileInfo File in new DirectoryInfo("..\\..\\..\\UtilitiesPackages\\").EnumerateFiles("*.nuspec", System.IO.SearchOption.AllDirectories))
            {
                string FileContent = File.Read();
                string Version = Regex.Match(FileContent, "<version>(?<VersionNumber>.*)</version>").Groups["VersionNumber"].Value;
                string[] VersionInfo = Version.Replace("-beta", "").Split('.');
                string CurrentVersion = "4." + VersionInfo[1] + ".";
                if (VersionInfo.Length > 2)
                    CurrentVersion += (int.Parse(VersionInfo[2])).ToString();
                else
                    CurrentVersion += "0";
                CurrentVersion += "-beta";
                FileContent = Regex.Replace(FileContent,
                                @"<dependency id=""CraigsUtilityLibrary-(?<Project>[^""]*)"" version=""(?<VersionNumber>[^""]*)"" />",
                                x => @"<dependency id=""CraigsUtilityLibrary-" + x.Groups["Project"].Value + @""" version=""[" + CurrentVersion + @"]"" />");
                FileContent = Regex.Replace(FileContent,
                                @"<dependency id=""CraigsUtilityLibrary"" version=""(?<VersionNumber>[^""]*)"" />",
                                x => @"<dependency id=""CraigsUtilityLibrary"" version=""[" + CurrentVersion + @"]"" />");
                FileContent = Regex.Replace(FileContent,
                                @"<dependency id=""Ironman\.Core"" version=""(?<VersionNumber>[^""]*)"" />",
                                x => @"<dependency id=""Ironman.Core"" version=""[" + Version + @"]"" />");
                FileContent = Regex.Replace(FileContent,
                                @"<dependency id=""Ironman\.Core\.(?<Project>[^""]*)"" version=""(?<VersionNumber>[^""]*)"" />",
                                x => @"<dependency id=""Ironman.Core." + x.Groups["Project"].Value + @""" version=""[" + Version + @"]"" />");
                FileContent = Regex.Replace(FileContent,
                                @"<dependency id=""CUL\.(?<Project>[^""]*)"" version=""(?<VersionNumber>[^""]*)"" />",
                                x => @"<dependency id=""CUL." + x.Groups["Project"].Value + @""" version=""[" + CurrentVersion + @"]"" />");
                FileContent = Regex.Replace(FileContent,
                                @"<dependency id=""Glimpse\.CUL"" version=""(?<VersionNumber>[^""]*)"" />",
                                x => @"<dependency id=""Glimpse.CUL"" version=""[" + CurrentVersion + @"]"" />");
                File.Write(FileContent);
            }
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
                else if (PathItems[0] != "Properties" && PathItems[0] != "GlobalSuppressions.cs")
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
                new DirectoryInfo("..\\..\\..\\Utilities\\" + Project.Name).CopyTo(new DirectoryInfo("..\\..\\..\\Utilities." + Project.Name + "\\" + Project.Name), Utilities.IO.Enums.CopyOptions.CopyIfNewer);
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

                    XmlNode Parent = null;
                    foreach (XmlNode Node in Nodes)
                    {
                        Parent = Node.ParentNode;
                        if (Parent != null)
                            break;
                    }
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
        public List<string> Includes { get; set; }

        public string Name { get; set; }
    }
}