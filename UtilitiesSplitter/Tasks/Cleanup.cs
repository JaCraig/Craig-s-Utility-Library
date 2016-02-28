using Utilities.IO;
using UtilitiesSplitter.Tasks.Interfaces;

namespace UtilitiesSplitter.Tasks
{
    /// <summary>
    /// Cleans up the process.
    /// </summary>
    /// <seealso cref="UtilitiesSplitter.Tasks.Interfaces.ITask" />
    public class Cleanup : ITask
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name => "Cleanup";

        /// <summary>
        /// Gets the order.
        /// </summary>
        /// <value>
        /// The order.
        /// </value>
        public int Order => 99;

        /// <summary>
        /// Runs this instance.
        /// </summary>
        /// <param name="PushToNuget">if set to <c>true</c> [push to nuget].</param>
        /// <returns>
        /// True if it runs successfully, false otherwise
        /// </returns>
        public bool Run(bool PushToNuget)
        {
            new DirectoryInfo("..\\..\\..\\UtilitiesPackages\\Packages\\").Delete();
            new DirectoryInfo("..\\..\\..\\UtilitiesPackages\\lib").Delete();
            new DirectoryInfo("..\\..\\..\\UtilitiesPackages\\tools").Delete();
            new DirectoryInfo("..\\..\\..\\UtilitiesPackages\\content").Delete();
            return true;
        }
    }
}