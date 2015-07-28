using System.Diagnostics;
using Utilities.DataTypes;
using Utilities.IO;
using UtilitiesSplitter.Tasks.Interfaces;

namespace UtilitiesSplitter.Tasks
{
    public class PushPackages : ITask
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name => "Push Packages";

        /// <summary>
        /// Gets or sets the order.
        /// </summary>
        /// <value>The order.</value>
        public int Order => 6;

        /// <summary>
        /// Runs this instance.
        /// </summary>
        /// <param name="PushToNuget">if set to <c>true</c> [push to nuget].</param>
        /// <returns>True if it runs successfully, false otherwise</returns>
        public bool Run(bool PushToNuget)
        {
            if (!PushToNuget)
                return true;
            new DirectoryInfo("..\\..\\..\\UtilitiesPackages\\")
                .EnumerateFiles("*.nupkg", System.IO.SearchOption.AllDirectories)
                .ForEach(x =>
            {
                new FileInfo("..\\..\\..\\.nuget\\nuget.exe")
                    .Execute(new ProcessStartInfo() { Arguments = "push \"" + x.FullName + "\"", CreateNoWindow = false })
                    .WaitForExit();
            });
            new DirectoryInfo("..\\..\\..\\UtilitiesPackages\\Packages\\").Delete();
            return true;
        }
    }
}