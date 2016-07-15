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

using System.Linq;
using Utilities.IoC.Interfaces;
using Utilities.ORM.Aspect;
using Utilities.ORM.Interfaces;
using Utilities.ORM.Manager.Mapper.Interfaces;
using Utilities.ORM.Manager.Schema.Interfaces;

namespace Utilities.ORM.Manager.Module
{
    /// <summary>
    /// ORM module
    /// </summary>
    public class ORMModule : IModule
    {
        /// <summary>
        /// Order to run it in
        /// </summary>
        public int Order
        {
            get { return 3; }
        }

        /// <summary>
        /// Loads the module
        /// </summary>
        /// <param name="Bootstrapper">Bootstrapper to register with</param>
        public void Load(IBootstrapper Bootstrapper)
        {
            Bootstrapper.RegisterAll<IMapping>();
            Bootstrapper.Register(new Mapper.Manager(Bootstrapper.ResolveAll<IMapping>()));

            Bootstrapper.RegisterAll<QueryProvider.Interfaces.IQueryProvider>();
            Bootstrapper.Register(new QueryProvider.Manager(Bootstrapper.ResolveAll<QueryProvider.Interfaces.IQueryProvider>()));

            Bootstrapper.RegisterAll<IDatabase>();
            Bootstrapper.Register(new SourceProvider.Manager(Bootstrapper.ResolveAll<IDatabase>()));

            Bootstrapper.RegisterAll<ISchemaGenerator>();
            Bootstrapper.Register(new Schema.Manager(Bootstrapper.ResolveAll<ISchemaGenerator>()));

            Bootstrapper.Register(new ORMManager(Bootstrapper.Resolve<Mapper.Manager>(),
                Bootstrapper.Resolve<QueryProvider.Manager>(),
                Bootstrapper.Resolve<Schema.Manager>(),
                Bootstrapper.Resolve<SourceProvider.Manager>(),
                Bootstrapper.ResolveAll<IDatabase>()));

            ORMAspect.Mapper = Bootstrapper.Resolve<Mapper.Manager>();
            Bootstrapper.Resolve<DataTypes.AOP.Manager>().Setup(ORMAspect.Mapper.Select(x => x.ObjectType).ToArray());
        }
    }
}