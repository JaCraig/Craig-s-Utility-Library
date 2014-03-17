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
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.DataTypes;
using Utilities.IoC.Interfaces;
using Utilities.ORM;
using Utilities.Random;

namespace SpeedTests
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IBootstrapper Bootstrapper = Utilities.IoC.Manager.Bootstrapper;
            using (IDisposable StartProfiler = Utilities.Profiler.Profiler.StartProfiling())
            {
                System.Random Rand = new System.Random();
                foreach (Employee Employee in 500.Times(x => Rand.NextClass<Employee>()))
                {
                    using (Utilities.Profiler.Profiler Profiler = new Utilities.Profiler.Profiler("Save"))
                    {
                        Employee.Save();
                    }
                }
                for (int x = 0; x < 500; ++x)
                {
                    using (Utilities.Profiler.Profiler Profiler = new Utilities.Profiler.Profiler("All"))
                    {
                        Employee.All();
                    }
                }
                for (int x = 0; x < 500; ++x)
                {
                    using (Utilities.Profiler.Profiler Profiler = new Utilities.Profiler.Profiler("Any"))
                    {
                        Employee.Any();
                    }
                }
                for (int x = 0; x < 500; ++x)
                {
                    using (Utilities.Profiler.Profiler Profiler = new Utilities.Profiler.Profiler("Paged"))
                    {
                        Employee.Paged();
                    }
                }
            }
            Console.WriteLine(Utilities.Profiler.Profiler.StopProfiling(false).ToString());
            QueryProvider.Batch("Data Source=localhost;Initial Catalog=SpeedTest;Integrated Security=SSPI;Pooling=false")
                        .AddCommand(null, null, CommandType.Text, "ALTER DATABASE SpeedTest SET OFFLINE WITH ROLLBACK IMMEDIATE")
                        .AddCommand(null, null, CommandType.Text, "ALTER DATABASE SpeedTest SET ONLINE")
                        .AddCommand(null, null, CommandType.Text, "DROP DATABASE SpeedTest")
                        .Execute();
            Console.ReadKey();
        }
    }
}