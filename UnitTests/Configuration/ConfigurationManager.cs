/*
Copyright (c) 2011 <a href="http://www.gutgames.com">James Craig</a>

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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MoonUnit;
using MoonUnit.Attributes;
using System.Data;
using Utilities.Configuration;
using Utilities.Configuration.Interfaces;
using Utilities.IO.ExtensionMethods;
using System.IO;

namespace UnitTests.Configuration
{
    public class ConfigurationManager:IDisposable
    {
        public ConfigurationManager()
        {
            new DirectoryInfo(@".\Test").Create();
        }

        [Test]
        public void SingleTest()
        {
            Assert.DoesNotThrow<Exception>(() => Utilities.Configuration.ConfigurationManager.RegisterConfigFile<TestConfig>());
            Assert.True(Utilities.Configuration.ConfigurationManager.ContainsConfigFile<TestConfig>("TestConfig"));
            IConfig Config = Utilities.Configuration.ConfigurationManager.GetConfigFile<TestConfig>("TestConfig");
            Assert.NotNull(Config);
            Assert.Equal("TestConfig", Config.Name);
        }

        [Test]
        public void AssemblyTest()
        {
            Assert.DoesNotThrow<Exception>(() => Utilities.Configuration.ConfigurationManager.RegisterConfigFile(typeof(TestConfig).Assembly));
            Assert.True(Utilities.Configuration.ConfigurationManager.ContainsConfigFile<TestConfig>("TestConfig"));
            IConfig Config = Utilities.Configuration.ConfigurationManager.GetConfigFile<TestConfig>("TestConfig");
            Assert.NotNull(Config);
            Assert.Equal("TestConfig", Config.Name);
        }

        public void Dispose()
        {
            new DirectoryInfo(@".\Test").DeleteAll();
        }
    }

    public class TestConfig : Config<TestConfig>
    {
        public override string Name
        {
            get { return "TestConfig"; }
        }
    }

    public class TestConfig2 : Config<TestConfig2>
    {
        public override string Name
        {
            get { return "TestConfig2"; }
        }
    }
}
