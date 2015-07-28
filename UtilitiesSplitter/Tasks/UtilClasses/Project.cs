using System.Collections.Generic;

namespace UtilitiesSplitter.Tasks.UtilClasses
{
    /// <summary>
    /// Project info holder
    /// </summary>
    public class Project
    {
        /// <summary>
        /// Gets or sets the includes.
        /// </summary>
        /// <value>The includes.</value>
        public List<string> Includes { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }
    }
}