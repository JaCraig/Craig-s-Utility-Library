using System.Linq;
using System.Text.RegularExpressions;
using Utilities.IO;
using UtilitiesSplitter.Tasks.Interfaces;

namespace UtilitiesSplitter.Tasks
{
    public class SetupInternalDependencies : ITask
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name => "Setup Internal Dependencies";

        /// <summary>
        /// Gets or sets the order.
        /// </summary>
        /// <value>The order.</value>
        public int Order => 4;

        /// <summary>
        /// Runs this instance.
        /// </summary>
        /// <param name="PushToNuget">if set to <c>true</c> [push to nuget].</param>
        /// <returns>True if it runs successfully, false otherwise</returns>
        public bool Run(bool PushToNuget)
        {
            foreach (FileInfo File in new DirectoryInfo("..\\..\\..\\UtilitiesPackages\\").EnumerateFiles("*.nuspec", System.IO.SearchOption.AllDirectories))
            {
                string FileContent = File.Read();
                string CurrentVersion = Regex.Match(FileContent, "<version>(?<VersionNumber>.*)</version>").Groups["VersionNumber"].Value;
                string CurrentID = Regex.Match(FileContent, "<id>(?<ProjectID>.*)</id>").Groups["ProjectID"].Value;
                foreach (FileInfo File2 in new DirectoryInfo("..\\..\\..\\UtilitiesPackages\\").EnumerateFiles("*.nuspec", System.IO.SearchOption.AllDirectories).Where(x => (FileInfo)x != File))
                {
                    FileContent = File2.Read();
                    FileContent = Regex.Replace(FileContent,
                                @"<dependency id=""" + CurrentID + @""" version=""(?<VersionNumber>[^""]*)"" />",
                                x => @"<dependency id=""" + CurrentID + @""" version=""[" + CurrentVersion + @"]"" />");
                    File2.Write(FileContent);
                }
            }
            return true;
        }
    }
}