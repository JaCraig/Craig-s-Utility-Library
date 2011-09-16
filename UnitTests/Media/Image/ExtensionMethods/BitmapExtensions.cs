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
using MoonUnit.Attributes;
using MoonUnit;
using System.Windows.Forms;
using Utilities.Media.Image.ExtensionMethods;
using Utilities.IO.ExtensionMethods;
using System.IO;
using System.Drawing;

namespace UnitTests.Media.Image.ExtensionMethods
{
    public class BitmapExtensions:IDisposable
    {
        public BitmapExtensions()
        {
            new DirectoryInfo(@".\Testing").Create();
        }

        [Test]
        public void AddNoise()
        {
            using (Bitmap TestObject = new Bitmap(@"..\..\Data\Image\Lenna.jpg"))
            {
                using (Bitmap Image = Assert.Do<Bitmap>(() => TestObject.AddNoise(40, @".\Testing\LennaNoise.jpg")))
                {
                    Assert.NotNull(Image);
                }
            }
        }

        [Test]
        public void AdjustBrightness()
        {
            using (Bitmap TestObject = new Bitmap(@"..\..\Data\Image\Lenna.jpg"))
            {
                using (Bitmap Image = Assert.Do<Bitmap>(() => TestObject.AdjustBrightness(40, @".\Testing\LennaBrightness.jpg")))
                {
                    Assert.NotNull(Image);
                }
            }
        }

        [Test]
        public void AdjustContrast()
        {
            using (Bitmap TestObject = new Bitmap(@"..\..\Data\Image\Lenna.jpg"))
            {
                using (Bitmap Image = Assert.Do<Bitmap>(() => TestObject.AdjustContrast(40, @".\Testing\LennaContrast.jpg")))
                {
                    Assert.NotNull(Image);
                }
            }
        }

        [Test]
        public void AdjustGamma()
        {
            using (Bitmap TestObject = new Bitmap(@"..\..\Data\Image\Lenna.jpg"))
            {
                using (Bitmap Image = Assert.Do<Bitmap>(() => TestObject.AdjustGamma(.2f, @".\Testing\LennaGamma.jpg")))
                {
                    Assert.NotNull(Image);
                }
            }
        }

        [Test]
        public void And()
        {
            using (Bitmap TestObject = new Bitmap(@"..\..\Data\Image\Lenna.jpg"))
            {
                using (Bitmap TestObject2 = new Bitmap(@"..\..\Data\Image\Image2.jpg"))
                {
                    using (Bitmap Image = Assert.Do<Bitmap>(() => TestObject.And(TestObject2, @".\Testing\LennaAnd.jpg")))
                    {
                        Assert.NotNull(Image);
                    }
                }
            }
        }

        [Test]
        public void BlueFilter()
        {
            using (Bitmap TestObject = new Bitmap(@"..\..\Data\Image\Lenna.jpg"))
            {
                using (Bitmap Image = Assert.Do<Bitmap>(() => TestObject.BlueFilter(@".\Testing\LennaBlueFilter.jpg")))
                {
                    Assert.NotNull(Image);
                }
            }
        }

        [Test]
        public void BoxBlur()
        {
            using (Bitmap TestObject = new Bitmap(@"..\..\Data\Image\Lenna.jpg"))
            {
                using (Bitmap Image = Assert.Do<Bitmap>(() => TestObject.BoxBlur(5, @".\Testing\LennaBoxBlur.jpg")))
                {
                    Assert.NotNull(Image);
                }
            }
        }

        [Test]
        public void Colorize()
        {
            using (Bitmap TestObject = new Bitmap(@"..\..\Data\Image\Lenna.jpg"))
            {
                List<Color> Colors = new List<Color>();
                for (int x = 0; x < 256; ++x)
                    Colors.Add(Color.FromArgb(255 - x, 0, x));
                using (Bitmap TestObject2 = TestObject.BlackAndWhite())
                {
                    using (Bitmap Image = Assert.Do<Bitmap>(() => TestObject2.Colorize(Colors.ToArray(), @".\Testing\LennaColorize.jpg")))
                    {
                        Assert.NotNull(Image);
                    }
                }
            }
        }

        [Test]
        public void BlackAndWhite()
        {
            using (Bitmap TestObject = new Bitmap(@"..\..\Data\Image\Lenna.jpg"))
            {
                using (Bitmap Image = Assert.Do<Bitmap>(() => TestObject.BlackAndWhite(@".\Testing\LennaBlackAndWhite.jpg")))
                {
                    Assert.NotNull(Image);
                }
            }
        }

        [Test]
        public void SepiaTone()
        {
            using (Bitmap TestObject = new Bitmap(@"..\..\Data\Image\Lenna.jpg"))
            {
                using (Bitmap Image = Assert.Do<Bitmap>(() => TestObject.SepiaTone(@".\Testing\LennaSepiaTone.jpg")))
                {
                    Assert.NotNull(Image);
                }
            }
        }

        [Test]
        public void Crop()
        {
            using (Bitmap TestObject = new Bitmap(@"..\..\Data\Image\Lenna.jpg"))
            {
                using (Bitmap Image = Assert.Do<Bitmap>(() => TestObject.Crop(100,100,
                                                                Utilities.Media.Image.ExtensionMethods.BitmapExtensions.Align.Bottom,
                                                                Utilities.Media.Image.ExtensionMethods.BitmapExtensions.Align.Right,
                                                                @".\Testing\LennaSepiaTone.jpg")))
                {
                    Assert.NotNull(Image);
                }
            }
        }

        public void Dispose()
        {
            //new DirectoryInfo(@".\Testing").DeleteAll();
        }
    }
}
