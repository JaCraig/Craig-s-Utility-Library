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

using Utilities.DataTypes.ExtensionMethods;
using System.Data;
using Utilities.DataTypes.Formatters;

namespace UnitTests.DataTypes.ExtensionMethods
{
    public class StringExtensions
    {

        enum EnumValues
        {
            Value1,
            Value2
        }

        [Fact]
        public void StringEncodingTest()
        {
            string Value = "ASDF";
            Assert.Equal("ASDF", Value.Encode(new ASCIIEncoding()));
            Assert.Equal("ASDF", Value.Encode(new ASCIIEncoding(), new UTF32Encoding()).Encode(new UTF32Encoding(), new ASCIIEncoding()));
        }

        [Fact]
        public void ByteArrayTest()
        {
            string Value = "ASDF";
            Assert.Equal("ASDF", Value.ToByteArray().ToEncodedString());
        }

        [Fact]
        public void Base64Test()
        {
            string Value = "ASDF";
            Assert.Equal("ASDF", Value.ToBase64().FromBase64(new ASCIIEncoding()));
            Assert.Equal("QVNERg==", Value.ToBase64());
        }

        [Fact]
        public void LeftTest()
        {
            string Value = "ASDF";
            Assert.Equal("AS", Value.Left(2));
        }

        [Fact]
        public void RightTest()
        {
            string Value = "ASDF";
            Assert.Equal("DF", Value.Right(2));
        }

        [Fact]
        public void ToFirstCharacterUppercase()
        {
            string Value = " this is a test";
            Assert.Equal(" This is a test", Value.ToFirstCharacterUpperCase());
        }

        [Fact]
        public void ToSentenceCapitalize()
        {
            string Value = " this is a test. of the sytem.";
            Assert.Equal(" This is a test. Of the sytem.", Value.ToSentenceCapitalize());
        }

        [Fact]
        public void ToTitleCase()
        {
            string Value = " this is a test";
            Assert.Equal(" This is a Test", Value.ToTitleCase());
        }

        [Fact]
        public void NumberTimesOccurs()
        {
            string Value = "The brown fox is awsome. But the blue fox is not";
            Assert.Equal(2, Value.NumberTimesOccurs("is"));
        }

        [Fact]
        public void Reverse()
        {
            string Value = " this is a test";
            Assert.Equal("tset a si siht ", Value.Reverse());
        }

        [Fact]
        public void FilterOutText()
        {
            string Value = "The brown fox is awsome. But the blue fox is not";
            Assert.Equal("The brown  is awsome. But the blue  is not", Value.FilterOutText("fox"));
        }

        [Fact]
        public void KeepFilterText()
        {
            string Value = "The brown fox is awsome. But the blue fox is not";
            Assert.Equal("foxfox", Value.KeepFilterText("fox"));
        }

        [Fact]
        public void AlphaNumericOnly()
        {
            string Value = "The brown fox is awsome. But the blue fox is not. 2222";
            Assert.Equal("ThebrownfoxisawsomeButthebluefoxisnot2222", Value.AlphaNumericOnly());
        }

        [Fact]
        public void AlphaCharactersOnly()
        {
            string Value = "The brown fox is awsome. But the blue fox is not. 2222";
            Assert.Equal("ThebrownfoxisawsomeButthebluefoxisnot", Value.AlphaCharactersOnly());
        }

        [Fact]
        public void ExpandTabs()
        {
            Assert.Equal("The    brown    fox    is    awsome. But the blue fox is not. 2222", "The\tbrown\tfox\tis\tawsome. But the blue fox is not. 2222".ExpandTabs());
        }

        [Fact]
        public void NumericOnly()
        {
            string Value = "The brown fox is awsome. But the blue fox is not. 2222";
            Assert.Equal("2222", Value.NumericOnly(false));
        }

        [Fact]
        public void IsUnicode()
        {
            string Value = "\u25EF\u25EF\u25EF";
            Assert.True(Value.IsUnicode());
        }

        [Fact]
        public void FormatString()
        {
            Assert.Equal("(555) 555-1010", "5555551010".FormatString("(###) ###-####"));
            Assert.Equal("(555) 555-1010", string.Format(new GenericStringFormatter(), "{0:(###) ###-####}", "5555551010"));
        }

        public class StringFormatClass
        {
            public StringFormatClass()
            {
                A = "This is a test";
                B = 10;
                C = 1.5f;
            }

            public string A { get; set; }

            public int B { get; set; }

            public float C { get; set; }
        }

        [Fact]
        public void FormatString2()
        {
            Assert.Equal("<A>This is a test</A><B>10</B><C>1.5</C>", "<A>{A}</A><B>{B}</B><C>{C}</C>".FormatString(new StringFormatClass()));
        }

        [Fact]
        public void FormatString3()
        {
            Assert.Equal("<A>This is a test</A><B>10</B><C>1.5</C>", "<A>{A}</A><B>{B}</B><C>{C}</C>".FormatString(new KeyValuePair<string, string>("{A}", "This is a test"),
                new KeyValuePair<string, string>("{B}", "10"),
                new KeyValuePair<string, string>("{C}", "1.5")));
        }

        [Fact]
        public void RegexFormat()
        {
            Assert.Equal("(555) 555-1010", "5555551010".RegexFormat(@"(\d{3})(\d{3})(\d{4})", "($1) $2-$3"));
        }

        [Fact]
        public void StripLeft()
        {
            Assert.Equal("1010", "5555551010".StripLeft("5432"));
        }

        [Fact]
        public void StripRight()
        {
            Assert.Equal("555555", "5555551010".StripRight("10"));
        }

        [Fact]
        public void NextSequence()
        {
            Assert.Equal("b", "a".NextSequence());
            Assert.Equal("c", "b".NextSequence());
            Assert.Equal("  ", "~".NextSequence());
            Assert.Equal("!", " ".NextSequence());
        }

        [Fact]
        public void MaskRight()
        {
            Assert.Equal("5555######", "5555551010".MaskRight());
        }

        [Fact]
        public void MaskLeft()
        {
            Assert.Equal("####551010", "5555551010".MaskLeft());
        }

        [Fact]
        public void Center()
        {
            Assert.Equal("****This is a test****", "This is a test".Center(22, "*"));
            Assert.Equal("abcaThis is a testabca", "This is a test".Center(22, "abc"));
        }

        [Fact]
        public void LevenshteinDistance()
        {
            Assert.Equal(0, "".LevenshteinDistance(""));
            Assert.Equal(5, "".LevenshteinDistance("Tests"));
            Assert.Equal(5, "Tests".LevenshteinDistance(""));
            Assert.Equal(1, "Test".LevenshteinDistance("Tests"));
            Assert.Equal(3, "Test".LevenshteinDistance("Testing"));
            Assert.Equal(1, "Rest".LevenshteinDistance("Test"));
        }

        [Fact]
        public void AppendLineFormat()
        {
            StringBuilder Builder=new StringBuilder();
            Builder.AppendLineFormat("This is test {0}",1);
            Assert.Equal("This is test 1" + System.Environment.NewLine, Builder.ToString());
            Builder.Clear();
            Builder.AppendLineFormat("Test {0}", 2)
                .AppendLineFormat("And {0}", 3);
            Assert.Equal("Test 2" + System.Environment.NewLine + "And 3" + System.Environment.NewLine, Builder.ToString());
        }

        [Fact]
        public void IsCreditCard()
        {
            Assert.True("4408041234567893".IsCreditCard());
        }
    }
}