/*
Copyright (c) 2014 <a href="http://www.gutgames.com">James Craig</a>

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.*/

#region Usings

using Ironman.Core.Assets;
using Ironman.Core.Tasks.Interfaces;

#endregion Usings

namespace Ironman.Core.Tasks
{
    /// <summary>
    /// Asset bundler task
    /// </summary>
    public class AssetBundlerTask : ITask
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AssetBundlerTask" /> class.
        /// </summary>
        /// <param name="Manager">The manager.</param>
        public AssetBundlerTask(AssetManager Manager)
        {
            this.Manager = Manager;
        }

        /// <summary>
        /// Name of the task
        /// </summary>
        public string Name
        {
            get { return "Asset Auto Bundler"; }
        }

        /// <summary>
        /// Time to run
        /// </summary>
        public Core.Tasks.Enums.RunTime TimeToRun
        {
            get { return Ironman.Core.Tasks.Enums.RunTime.PostStart; }
        }

        /// <summary>
        /// Gets or sets the manager.
        /// </summary>
        /// <value>The manager.</value>
        private AssetManager Manager { get; set; }

        /// <summary>
        /// Runs the task
        /// </summary>
        public void Run()
        {
            Manager.CreateBundles();
        }
    }
}