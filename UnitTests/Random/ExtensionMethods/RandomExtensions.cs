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
using MoonUnit.Attributes;
using MoonUnit;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Utilities.Random.ExtensionMethods;
using System.Drawing;
using Utilities.Random.StringGenerators;
using Utilities.Random.NameGenerators;
using Utilities.DataTypes.ExtensionMethods;
using Utilities.Random.ContactInfoGenerators;
using Utilities.Random.DefaultClasses;

namespace UnitTests.Random.ExtensionMethods
{
    public class RandomExtensions
    {
        [Test]
        public void Next()
        {
            System.Random Random = new System.Random();
            Assert.DoesNotThrow<Exception>(() => Random.Next<bool>());
            Assert.DoesNotThrow<Exception>(() => Random.Next<byte>());
            Assert.DoesNotThrow<Exception>(() => Random.Next<char>());
            Assert.DoesNotThrow<Exception>(() => Random.Next<decimal>());
            Assert.DoesNotThrow<Exception>(() => Random.Next<double>());
            Assert.DoesNotThrow<Exception>(() => Random.Next<float>());
            Assert.DoesNotThrow<Exception>(() => Random.Next<int>());
            Assert.DoesNotThrow<Exception>(() => Random.Next<long>());
            Assert.DoesNotThrow<Exception>(() => Random.Next<sbyte>());
            Assert.DoesNotThrow<Exception>(() => Random.Next<short>());
            Assert.DoesNotThrow<Exception>(() => Random.Next<uint>());
            Assert.DoesNotThrow<Exception>(() => Random.Next<ulong>());
            Assert.DoesNotThrow<Exception>(() => Random.Next<ushort>());
            Assert.DoesNotThrow<Exception>(() => Random.Next<DateTime>());
            Assert.DoesNotThrow<Exception>(() => Random.Next<Color>());
            Assert.DoesNotThrow<Exception>(() => Random.Next<TimeSpan>());
            Assert.DoesNotThrow<Exception>(() => Random.Next<string>());
        }

        [Test]
        public void Next2()
        {
            System.Random Random = new System.Random();
            Assert.DoesNotThrow<Exception>(() => Random.Next<bool>(false, true));
            Assert.Between(Random.Next<byte>(1, 29), 1, 29);
            Assert.Between(Random.Next<char>('a', 'z'), 'a', 'z');
            Assert.Between(Random.Next<decimal>(1.0m, 102.1m), 1, 102.1m);
            Assert.Between(Random.Next<double>(1.0d, 102.1d), 1, 102.1d);
            Assert.Between(Random.Next<float>(1, 102.1f), 1, 102.1f);
            Assert.Between(Random.Next<int>(1, 29), 1, 29);
            Assert.Between(Random.Next<long>(1, 29), 1, 29);
            Assert.Between(Random.Next<sbyte>(1, 29), 1, 29);
            Assert.Between(Random.Next<short>(1, 29), 1, 29);
            Assert.Between(Random.Next<uint>(1, 29), (uint)1, (uint)29);
            Assert.Between(Random.Next<ulong>(1, 29), (ulong)1, (ulong)29);
            Assert.Between(Random.Next<ushort>(1, 29), (ushort)1, (ushort)29);
            Assert.Between(Random.Next<DateTime>(new DateTime(1900, 1, 1), new DateTime(2000, 1, 1)), new DateTime(1900, 1, 1), new DateTime(2000, 1, 1));
            Assert.Equal(10, Random.Next<string>().Length);
        }

        [Test]
        public void NextIEnumerable()
        {
            System.Random Random = new System.Random();
            Assert.Between(Random.Next<int>(new int[] { 1, 2, 3, 4, 5 }), 1, 5);
        }


        [Test]
        public void NextLoremIpsumTest()
        {
            Utilities.Random.Random Rand = new Utilities.Random.Random();
            Assert.NotNull(Rand.Next<string>(new LoremIpsumGenerator(1, 4)));
        }

        [Test]
        public void NextStringTest()
        {
            Utilities.Random.Random Rand = new Utilities.Random.Random();
            Assert.Equal(10, Rand.Next<string>(new RegexStringGenerator(10)).Length);
        }

        [Test]
        public void NextStringTest2()
        {
            Utilities.Random.Random Rand = new Utilities.Random.Random();
            Assert.True(Regex.IsMatch(Rand.Next<string>(new PatternGenerator("(###)###-####")), @"\(\d{3}\)\d{3}-\d{4}"));
            Assert.True(Regex.IsMatch(Rand.Next<string>(new PatternGenerator("(@@@)###-####")), @"\([a-zA-Z]{3}\)\d{3}-\d{4}"));
        }

        [Test]
        public void NextName()
        {
            Utilities.Random.Random Rand = new Utilities.Random.Random();
            Assert.Equal(3, Rand.Next<string>(new NameGenerator(false, true, true, false)).Split(' ').Length);
            Assert.Equal(2, Rand.Next<string>(new NameGenerator(false, false, true, false)).Split(' ').Length);
            Assert.Equal(4, Rand.Next<string>(new NameGenerator(true, true, true, false)).Split(' ').Length);
            Assert.Equal(5, Rand.Next<string>(new NameGenerator(true, true, true, true)).Split(' ').Length);
        }

        [Test]
        public void RegisterGenerator()
        {
            Utilities.Random.Random Rand = new Utilities.Random.Random();
            Assert.DoesNotThrow<Exception>(() => Rand.RegisterGenerator<string>(new NameGenerator()));
            Assert.True(100.Times(x => Rand.Next<string>()).TrueForAll(x => x.Split(' ').Length == 2));
        }

        [Test]
        public void ClassGenerator()
        {
            System.Random Rand = new System.Random(1231415);
            RandomTestClass Item = Rand.NextClass<RandomTestClass>();
            Assert.Equal(202970450, Item.A);
            Assert.Equal("Lorem ipsum dolor sit amet. ", Item.B);
            Assert.Equal(System.Math.Round(0.9043f, 4), System.Math.Round(Item.C, 4));
        }
    }

    public class RandomTestClass
    {
        [IntGenerator]
        public int A { get; set; }

        [LoremIpsumGenerator]
        public string B { get; set; }

        [FloatGenerator]
        public float C { get; set; }
    }
}
