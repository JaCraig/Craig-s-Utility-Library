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
using Utilities.DataTypes;

namespace UnitTests.DataTypes
{
    public class BTree
    {
        [Test]
        public void Creation()
        {
            BinaryTree<int> Tree = new BinaryTree<int>();
            Tree.Add(1);
            Tree.Add(2);
            Tree.Add(0);
            Tree.Add(-1);
            Assert.Equal(-1, Tree.MinValue);
            Assert.Equal(2, Tree.MaxValue);
        }

        [Test]
        public void Random()
        {
            BinaryTree<int> Tree = new BinaryTree<int>();
            System.Collections.Generic.List<int> Values = new System.Collections.Generic.List<int>();
            System.Random Rand = new System.Random();
            for (int x = 0; x < 10; ++x)
            {
                int Value = Rand.Next();
                Values.Add(Value);
                Tree.Add(Value);
            }
            for (int x = 0; x < 10; ++x)
            {
                Assert.Contains(Values[x], Tree);
            }
            Values.Sort();
            StringBuilder Builder = new StringBuilder();
            Values.ForEach((x) => Builder.Append(x.ToString() + " "));
            Assert.Equal(Builder.ToString(), Tree.ToString());
        }
    }
}
