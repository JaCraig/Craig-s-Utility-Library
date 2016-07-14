using System.Text.RegularExpressions;
using Utilities.IO;
using UtilitiesSplitter.Tasks.Interfaces;

namespace UtilitiesSplitter.Tasks
{
    public class ReversionPackages : ITask
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name => "Reversion Packages";

        /// <summary>
        /// Gets or sets the order.
        /// </summary>
        /// <value>The order.</value>
        public int Order => 2;

        /// <summary>
        /// Runs this instance.
        /// </summary>
        /// <param name="PushToNuget">if set to <c>true</c> [push to nuget].</param>
        /// <returns>True if it runs successfully, false otherwise</returns>
        public bool Run(bool PushToNuget)
        {
            foreach (FileInfo File in new DirectoryInfo("..\\..\\..\\UtilitiesPackages\\").EnumerateFiles("*.nuspec", System.IO.SearchOption.AllDirectories))
            {
                var FileContents = File.Read();
                var VersionMatch = Regex.Match(FileContents, "<version>(?<VersionNumber>.*)</version>");
                var IsBeta = VersionMatch.Groups["VersionNumber"].Value.Contains("-beta");
                var VersionInfo = VersionMatch.Groups["VersionNumber"].Value.Replace("-beta", "").Split('.');
                string NewVersion = VersionInfo[0] + "." + VersionInfo[1] + ".";
                NewVersion += VersionInfo.Length > 2 ? (int.Parse(VersionInfo[2]) + 1).ToString() : "1";
                File.Write(Regex.Replace(FileContents, "<version>(?<VersionNumber>.*)</version>", "<version>" + NewVersion + (IsBeta ? "-beta" : "") + "</version>"));
            }
            return true;
        }
    }
}