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

using Ironman.Core.API.Manager;
using Ironman.Core.API.Manager.Interfaces;
using Ironman.Core.Assets;
using Ironman.Core.Assets.Interfaces;
using Ironman.Core.Serialization.BaseClasses;
using Ironman.Core.Tasks;
using Ironman.Core.Tasks.Interfaces;
using Utilities.IoC.Interfaces;

#endregion Usings

namespace Ironman.Core.Bootstrapper
{
    /// <summary>
    /// Module for registering the asset module
    /// </summary>
    public class IronmanModule : IModule
    {
        /// <summary>
        /// Order in which to load it
        /// </summary>
        public int Order
        {
            get { return 2; }
        }

        /// <summary>
        /// Loads the various managers
        /// </summary>
        /// <param name="Bootstrapper">Bootstrapper</param>
        public void Load(IBootstrapper Bootstrapper)
        {
            if (Bootstrapper == null)
                return;
            Bootstrapper.ResolveAll<IFilter>();
            Bootstrapper.ResolveAll<IContentFilter>();
            Bootstrapper.ResolveAll<ITranslator>();
            Bootstrapper.Register<AssetManager>(new AssetManager(Bootstrapper.ResolveAll<IFilter>(), Bootstrapper.ResolveAll<IContentFilter>(), Bootstrapper.ResolveAll<ITranslator>()));
            Bootstrapper.RegisterAll<VPFactoryBase>();
            Bootstrapper.RegisterAll<ITask>();
            Bootstrapper.Register<TaskManager>(new TaskManager(Bootstrapper.ResolveAll<ITask>()));
            Bootstrapper.RegisterAll<IAPIMapping>();
            Bootstrapper.RegisterAll<IService>();
            Bootstrapper.RegisterAll<IWorkflowModule>();
            Bootstrapper.Register<Manager>(new Manager(Bootstrapper.ResolveAll<IAPIMapping>(), Bootstrapper.ResolveAll<IService>(), Bootstrapper.ResolveAll<IWorkflowModule>(), Bootstrapper.Resolve<Utilities.Workflow.Manager.Manager>()));
        }
    }
}