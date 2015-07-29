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

using SpeedTests.Models;
using System;
using System.Data;
using Utilities.DataTypes;
using Utilities.IoC.Interfaces;
using Utilities.ORM;
using Utilities.Profiler.Manager.Default;
using Utilities.Random;

namespace SpeedTests
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var StopWatch = new StopWatch();
            StopWatch.Start();
            IBootstrapper Bootstrapper = Utilities.IoC.Manager.Bootstrapper;
            StopWatch.Stop();
            Console.WriteLine("Start up: " + StopWatch.ElapsedTime);
            using (IDisposable StartProfiler = Utilities.Profiler.Profiler.StartProfiling())
            {
                var Rand = new System.Random();
                SingleSourceCached(Rand);
                MultipleSourceCached(Rand);
                MultipleSourceCachedCascade(Rand);
            }
            Console.WriteLine(Utilities.Profiler.Profiler.StopProfiling(false).ToString());
            QueryProvider.Batch("Data Source=localhost;Initial Catalog=SpeedTest;Integrated Security=SSPI;Pooling=false")
                        .AddCommand(null, null, CommandType.Text, "ALTER DATABASE SpeedTest SET OFFLINE WITH ROLLBACK IMMEDIATE")
                        .AddCommand(null, null, CommandType.Text, "ALTER DATABASE SpeedTest SET ONLINE")
                        .AddCommand(null, null, CommandType.Text, "DROP DATABASE SpeedTest")
                        .Execute();
            Console.ReadKey();
        }

        private static void MultipleSourceCached(System.Random Rand)
        {
            foreach (Employee2 Employee2 in 100.Times(x => Rand.NextClass<Employee2>()))
            {
                using (Utilities.Profiler.Profiler Profiler = new Utilities.Profiler.Profiler("Multiple Source Cached Save"))
                {
                    Employee2.Save();
                }
            }
            for (int x = 0; x < 100; ++x)
            {
                using (Utilities.Profiler.Profiler Profiler = new Utilities.Profiler.Profiler("Multiple Source Cached All"))
                {
                    Employee2.All();
                }
            }
            for (int x = 0; x < 100; ++x)
            {
                using (Utilities.Profiler.Profiler Profiler = new Utilities.Profiler.Profiler("Multiple Source Cached Any"))
                {
                    Employee2.Any();
                }
            }
            for (int x = 0; x < 100; ++x)
            {
                using (Utilities.Profiler.Profiler Profiler = new Utilities.Profiler.Profiler("Multiple Source Cached Paged"))
                {
                    Employee2.Paged();
                }
            }
        }

        private static void MultipleSourceCachedCascade(System.Random Rand)
        {
            foreach (Employee3 Employee3 in 100.Times(x => Rand.NextClass<Employee3>()))
            {
                Employee3.Boss = Rand.NextClass<Employee3>();
                using (Utilities.Profiler.Profiler Profiler = new Utilities.Profiler.Profiler("Multiple Source Cached Cascade Save"))
                {
                    Employee3.Save();
                }
            }
            for (int x = 0; x < 100; ++x)
            {
                using (Utilities.Profiler.Profiler Profiler = new Utilities.Profiler.Profiler("Multiple Source Cached Cascade All"))
                {
                    Employee3.All().ForEach(y => { var Temp = y.Boss; });
                }
            }
            for (int x = 0; x < 100; ++x)
            {
                using (Utilities.Profiler.Profiler Profiler = new Utilities.Profiler.Profiler("Multiple Source Cached Cascade Any"))
                {
                    var Temp = Employee3.Any().Boss;
                }
            }
            for (int x = 0; x < 100; ++x)
            {
                using (Utilities.Profiler.Profiler Profiler = new Utilities.Profiler.Profiler("Multiple Source Cached Cascade Paged"))
                {
                    Employee3.Paged().ForEach(y => { var Temp = y.Boss; });
                }
            }
        }

        private static void SingleSourceCached(System.Random Rand)
        {
            foreach (Employee Employee in 100.Times(x => Rand.NextClass<Employee>()))
            {
                using (Utilities.Profiler.Profiler Profiler = new Utilities.Profiler.Profiler("Single Source Cached Save"))
                {
                    Employee.Save();
                }
            }
            for (int x = 0; x < 100; ++x)
            {
                using (Utilities.Profiler.Profiler Profiler = new Utilities.Profiler.Profiler("Single Source Cached All"))
                {
                    Employee.All();
                }
            }
            for (int x = 0; x < 100; ++x)
            {
                using (Utilities.Profiler.Profiler Profiler = new Utilities.Profiler.Profiler("Single Source Cached Any"))
                {
                    Employee.Any();
                }
            }
            for (int x = 0; x < 100; ++x)
            {
                using (Utilities.Profiler.Profiler Profiler = new Utilities.Profiler.Profiler("Single Source Cached Paged"))
                {
                    Employee.Paged();
                }
            }
        }
    }
}