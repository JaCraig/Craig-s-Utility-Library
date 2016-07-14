using System.Text.RegularExpressions;
using Utilities.IO;
using UtilitiesSplitter.Tasks.Interfaces;

namespace UtilitiesSplitter.Tasks
{
    public class SetupDependencies : ITask
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name => "Setup Dependencies";

        /// <summary>
        /// Gets or sets the order.
        /// </summary>
        /// <value>The order.</value>
        public int Order => 3;

        /// <summary>
        /// Runs this instance.
        /// </summary>
        /// <param name="PushToNuget">if set to <c>true</c> [push to nuget].</param>
        /// <returns>True if it runs successfully, false otherwise</returns>
        public bool Run(bool PushToNuget)
        {
            foreach (FileInfo File in new DirectoryInfo("..\\..\\..\\UtilitiesPackages\\").EnumerateFiles("*.nuspec", System.IO.SearchOption.AllDirectories))
            {
                var PackagesFile = new FileInfo("..\\..\\..\\" + File.Name.Replace(".nuspec", "") + "\\packages.config");
                if (PackagesFile.Exists)
                {
                    var PackagesContent = PackagesFile.Read();
                    var FileContent = File.Read();
                    foreach (Match Package in Regex.Matches(PackagesContent, @"<package id=""(?<Package>[^""]*)"" version=""(?<Version>[^""]*)"""))
                    {
                        if (Regex.IsMatch(FileContent, @"<dependency id=""" + Package.Groups["Package"].Value + @""" version=""(?<VersionNumber>[^""]*)"" />"))
                        {
                            var TempMatch = Regex.Match(FileContent, @"<dependency id=""" + Package.Groups["Package"].Value + @""" version=""(?<VersionNumber>[^""]*)"" />");
                            FileContent = FileContent.Replace(TempMatch.Value, @"<dependency id=""" + Package.Groups["Package"].Value + @""" version=""[" + Package.Groups["Version"].Value + @"]"" />");
                        }
                    }
                    File.Write(FileContent);
                }
            }
            return true;
        }
    }
}