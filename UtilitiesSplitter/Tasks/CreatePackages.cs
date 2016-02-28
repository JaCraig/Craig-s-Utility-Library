using System.Diagnostics;
using System.Linq;
using Utilities.IO;
using UtilitiesSplitter.Tasks.Interfaces;

namespace UtilitiesSplitter.Tasks
{
    public class CreatePackages : ITask
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name => "Create Packages";

        /// <summary>
        /// Gets or sets the order.
        /// </summary>
        /// <value>The order.</value>
        public int Order => 5;

        /// <summary>
        /// Runs this instance.
        /// </summary>
        /// <param name="PushToNuget">if set to <c>true</c> [push to nuget].</param>
        /// <returns>True if it runs successfully, false otherwise</returns>
        public bool Run(bool PushToNuget)
        {
            if (!PushToNuget)
                return true;
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
                Process NugetProcess = new FileInfo("C:\\dev\\nuget\\nuget.exe").Execute(new ProcessStartInfo { Arguments = "pack \"" + File.FullName + "\"", WorkingDirectory = "..\\..\\..\\UtilitiesPackages\\Packages", CreateNoWindow = false });
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

                new FileInfo(@"C:\Program Files\doxygen\bin\doxygen.exe")
                    .Execute(new ProcessStartInfo { Arguments = "\"" + File.FullName.Replace(".nuspec", ".doxy") + "\"", WorkingDirectory = "..\\..\\..\\UtilitiesPackages", CreateNoWindow = false })
                    .WaitForExit();
                new FileInfo("C:\\dev\\nuget\\nuget.exe")
                    .Execute(new ProcessStartInfo { Arguments = "pack \"" + File.FullName + "\"", WorkingDirectory = "..\\..\\..\\UtilitiesPackages\\Packages", CreateNoWindow = false })
                    .WaitForExit();

                new DirectoryInfo("..\\..\\..\\UtilitiesPackages\\lib").Delete();
                new DirectoryInfo("..\\..\\..\\UtilitiesPackages\\tools").Delete();
                new DirectoryInfo("..\\..\\..\\UtilitiesPackages\\content").Delete();
            }
            new FileInfo("..\\..\\..\\UtilitiesPackages\\readme.txt").Delete();
            return true;
        }
    }
}