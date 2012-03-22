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
using MoonUnit;
using MoonUnit.Attributes;
using Utilities.DataTypes.ExtensionMethods;
using System.Data;
using Utilities.DataTypes.Formatters;

namespace UnitTests.DataTypes.ExtensionMethods
{
    public class StringExtensions
    {
        [Test]
        public void ToEnumTest()
        {
            Assert.Equal(EnumValues.Value1, "Value1".TryTo<string, EnumValues>());
        }

        enum EnumValues
        {
            Value1,
            Value2
        }

        [Test]
        public void StringEncodingTest()
        {
            string Value = "ASDF";
            Assert.Equal("ASDF", Value.Encode());
            Assert.Equal("ASDF", Value.Encode(new ASCIIEncoding(), new UTF32Encoding()).Encode(new UTF32Encoding(), new ASCIIEncoding()));
        }

        [Test]
        public void ByteArrayTest()
        {
            string Value = "ASDF";
            Assert.Equal("ASDF", Value.ToByteArray().ToEncodedString());
        }

        [Test]
        public void Base64Test()
        {
            string Value = "ASDF";
            Assert.Equal("ASDF", Value.ToBase64().FromBase64(new ASCIIEncoding()));
            Assert.Equal("QVNERg==", Value.ToBase64());
        }

        [Test]
        public void LeftTest()
        {
            string Value = "ASDF";
            Assert.Equal("AS", Value.Left(2));
        }

        [Test]
        public void RightTest()
        {
            string Value = "ASDF";
            Assert.Equal("DF", Value.Right(2));
        }

        [Test]
        public void ToFirstCharacterUppercase()
        {
            string Value = " this is a test";
            Assert.Equal(" This is a test", Value.ToFirstCharacterUpperCase());
        }

        [Test]
        public void ToSentenceCapitalize()
        {
            string Value = " this is a test. of the sytem.";
            Assert.Equal(" This is a test. Of the sytem.", Value.ToSentenceCapitalize());
        }

        [Test]
        public void ToTitleCase()
        {
            string Value = " this is a test";
            Assert.Equal(" This is a Test", Value.ToTitleCase());
        }

        [Test]
        public void NumberTimesOccurs()
        {
            string Value = "The brown fox is awsome. But the blue fox is not";
            Assert.Equal(2, Value.NumberTimesOccurs("is"));
        }

        [Test]
        public void Reverse()
        {
            string Value = " this is a test";
            Assert.Equal("tset a si siht ", Value.Reverse());
        }

        [Test]
        public void FilterOutText()
        {
            string Value = "The brown fox is awsome. But the blue fox is not";
            Assert.Equal("The brown  is awsome. But the blue  is not", Value.FilterOutText("fox"));
        }

        [Test]
        public void KeepFilterText()
        {
            string Value = "The brown fox is awsome. But the blue fox is not";
            Assert.Equal("foxfox", Value.KeepFilterText("fox"));
        }

        [Test]
        public void AlphaNumericOnly()
        {
            string Value = "The brown fox is awsome. But the blue fox is not. 2222";
            Assert.Equal("ThebrownfoxisawsomeButthebluefoxisnot2222", Value.AlphaNumericOnly());
        }

        [Test]
        public void AlphaCharactersOnly()
        {
            string Value = "The brown fox is awsome. But the blue fox is not. 2222";
            Assert.Equal("ThebrownfoxisawsomeButthebluefoxisnot", Value.AlphaCharactersOnly());
        }

        [Test]
        public void ExpandTabs()
        {
            Assert.Equal("The    brown    fox    is    awsome. But the blue fox is not. 2222", "The\tbrown\tfox\tis\tawsome. But the blue fox is not. 2222".ExpandTabs());
        }

        [Test]
        public void NumericOnly()
        {
            string Value = "The brown fox is awsome. But the blue fox is not. 2222";
            Assert.Equal("2222", Value.NumericOnly(false));
        }

        [Test]
        public void IsUnicode()
        {
            string Value = "\u25EF\u25EF\u25EF";
            Assert.True(Value.IsUnicode());
        }

        [Test]
        public void TryTo()
        {
            Assert.OfType<int>("123".TryTo<string,int>());
            Assert.Equal(123, "123".TryTo<string,int>());
            Assert.DoesNotThrow<Exception>(() => "ASD".TryTo<string, int>());
        }

        [Test]
        public void FormatString()
        {
            Assert.Equal("(555) 555-1010", "5555551010".FormatString("(###) ###-####"));
            Assert.Equal("(555) 555-1010", string.Format(new GenericStringFormatter(), "{0:(###) ###-####}", "5555551010"));
        }

        [Test]
        public void RegexFormat()
        {
            Assert.Equal("(555) 555-1010", "5555551010".RegexFormat(@"(\d{3})(\d{3})(\d{4})", "($1) $2-$3"));
        }

        [Test]
        public void StripLeft()
        {
            Assert.Equal("1010", "5555551010".StripLeft("5432"));
        }

        [Test]
        public void StripRight()
        {
            Assert.Equal("555555", "5555551010".StripRight("10"));
        }

        [Test]
        public void NextSequence()
        {
            Assert.Equal("b", "a".NextSequence());
            Assert.Equal("c", "b".NextSequence());
            Assert.Equal("  ", "~".NextSequence());
            Assert.Equal("!", " ".NextSequence());
        }

        [Test]
        public void MaskRight()
        {
            Assert.Equal("5555######", "5555551010".MaskRight());
        }

        [Test]
        public void MaskLeft()
        {
            Assert.Equal("####551010", "5555551010".MaskLeft());
        }

        [Test]
        public void Center()
        {
            Assert.Equal("****This is a test****", "This is a test".Center(22, "*"));
            Assert.Equal("abcaThis is a testabca", "This is a test".Center(22, "abc"));
        }

        [Test]
        public void Pluralize()
        {
            Assert.Equal("sheep", "sheep".Pluralize());
            Assert.Equal("children", "child".Pluralize());
            Assert.Equal("mice", "mice".Pluralize());
            Assert.Equal("tests", "test".Pluralize());
        }

        [Test]
        public void Singularize()
        {
            Assert.Equal("sheep", "sheep".Singularize());
            Assert.Equal("child", "children".Singularize());
            Assert.Equal("mouse", "mice".Singularize());
            Assert.Equal("test", "tests".Singularize());
        }
    }
}