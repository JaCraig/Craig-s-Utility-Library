/*
Copyright (c) 2012 <a href="http://www.gutgames.com">James Craig</a>

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
using Xunit;

using System.Data;
using Utilities.Configuration;
using Utilities.Configuration.Interfaces;
using Utilities.IO.ExtensionMethods;
using System.IO;
using UnitTests.Fixtures;

namespace UnitTests.Configuration
{
    public class Config:IUseFixture<TestingDirectoryFixture>
    {
        public Config()
        {
            Utilities.Configuration.ConfigurationManager.RegisterConfigFile(typeof(TestConfig).Assembly);
        }

        [Fact]
        public void Test()
        {
            TestConfig3 Config = Utilities.Configuration.ConfigurationManager.GetConfigFile<TestConfig3>("TestConfig3");
            Assert.NotNull(Config);
            Assert.Equal("TestConfig3", Config.Name);
            Assert.Equal(10, Config.Value1);
            Assert.Equal("DefaultValue", Config.Value2);
            Assert.Equal(2f, Config.Value3);
            Config.Value1 = 20;
            Config.Value2 = "DefaultValue2";
            Config.Value3 = 3f;
            Config.Save();
            Config.Value1 = 50;
            Config.Value2 = "DefaultValue5";
            Config.Value3 = 5f;
            Config.Load();
            Assert.Equal(20, Config.Value1);
            Assert.Equal("DefaultValue2", Config.Value2);
            Assert.Equal(3f, Config.Value3);
        }

        public void SetFixture(TestingDirectoryFixture data)
        {
            
        }
    }

    public class TestConfig3 : Config<TestConfig3>
    {
        public TestConfig3()
        {
            Value1 = 10;
            Value2 = "DefaultValue";
            Value3 = 2.0f;
        }

        public override string Name
        {
            get { return "TestConfig3"; }
        }

        protected override string ConfigFileLocation
        {
            get
            {
                return @".\Testing\Config.xml";
            }
        }

        public virtual int Value1 { get; set; }
        public virtual string Value2 { get; set; }
        public virtual float Value3 { get; set; }
    }
}