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

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTests.Fixtures;
using Utilities.IO;
using Xunit;

namespace UnitTests.Media
{
    public class SwiftBitmap : TestingDirectoryFixture
    {
        [Fact]
        public void Create()
        {
            using (Utilities.Media.SwiftBitmap Temp = new Utilities.Media.SwiftBitmap(@"..\..\Data\Image\Lenna.jpg"))
            {
                Assert.Equal(220, Temp.Height);
                Assert.Equal(220, Temp.Width);
                Assert.NotNull(Temp.InternalBitmap);
            }
        }

        [Fact]
        public void Copy()
        {
            using (Utilities.Media.SwiftBitmap Temp = new Utilities.Media.SwiftBitmap(@"..\..\Data\Image\Lenna.jpg"))
            {
                using (Utilities.Media.SwiftBitmap Temp2 = new Utilities.Media.SwiftBitmap(Temp.Width, Temp.Height))
                {
                    Temp2.Copy(Temp);
                    Assert.Equal(Temp.Width, Temp2.Width);
                    Assert.Equal(Temp.Height, Temp2.Height);
                    Assert.NotSame(Temp.InternalBitmap, Temp2.InternalBitmap);
                }
            }
        }

        [Fact]
        public void LockUnlock()
        {
            using (Utilities.Media.SwiftBitmap Temp = new Utilities.Media.SwiftBitmap(@"..\..\Data\Image\Lenna.jpg"))
            {
                Temp.Lock();
                Temp.Unlock();
            }
        }

        [Fact]
        public void SaveTest()
        {
            using (Utilities.Media.SwiftBitmap Temp = new Utilities.Media.SwiftBitmap(@"..\..\Data\Image\Lenna.jpg"))
            {
                Temp.Save(@".\Testing\SwiftBitmapSaveResult.jpg");
                Assert.True(new FileInfo(@".\Testing\SwiftBitmapSaveResult.jpg").ReadBinary().SequenceEqual(new FileInfo(@"..\..\BitmapResults\SwiftBitmapSaveResult.jpg").ReadBinary()));
            }
        }

        [Fact]
        public void ApplyMatrix()
        {
            using (Utilities.Media.SwiftBitmap Temp = new Utilities.Media.SwiftBitmap(@"..\..\Data\Image\Lenna.jpg"))
            {
                Temp.ApplyColorMatrix(new ColorMatrix(new float[][]{
                            new float[] {1, 0, 0, 0, 0},
                            new float[] {0, 1, 0, 0, 0},
                            new float[] {0, 0, 1, 0, 0},
                            new float[] {0, 0, 0, 1, 0},
                            new float[] {0.5f, 0.5f, 0.5f, 1, 1}
                        }));
                Temp.Save(@".\Testing\SwiftBitmapApplyMatrixResult.jpg");
                Assert.True(new FileInfo(@".\Testing\SwiftBitmapApplyMatrixResult.jpg").ReadBinary().SequenceEqual(new FileInfo(@"..\..\BitmapResults\SwiftBitmapApplyMatrixResult.jpg").ReadBinary()));
            }
        }

        [Fact]
        public void And()
        {
            using (Utilities.Media.SwiftBitmap Temp = new Utilities.Media.SwiftBitmap(@"..\..\Data\Image\Lenna.jpg"))
            {
                using (Utilities.Media.SwiftBitmap TestObject2 = new Utilities.Media.SwiftBitmap(@"..\..\Data\Image\Image2.jpg"))
                {
                    using (Utilities.Media.SwiftBitmap Temp2 = Temp & TestObject2)
                    {
                        Temp2.Save(@".\Testing\SwiftBitmapAndResult.jpg");
                        Assert.True(new FileInfo(@".\Testing\SwiftBitmapAndResult.jpg").ReadBinary().SequenceEqual(new FileInfo(@"..\..\BitmapResults\SwiftBitmapAndResult.jpg").ReadBinary()));
                    }
                }
            }
        }

        [Fact]
        public void Or()
        {
            using (Utilities.Media.SwiftBitmap Temp = new Utilities.Media.SwiftBitmap(@"..\..\Data\Image\Lenna.jpg"))
            {
                using (Utilities.Media.SwiftBitmap TestObject2 = new Utilities.Media.SwiftBitmap(@"..\..\Data\Image\Image2.jpg"))
                {
                    using (Utilities.Media.SwiftBitmap Temp2 = Temp | TestObject2)
                    {
                        Temp2.Save(@".\Testing\SwiftBitmapOrResult.jpg");
                        Assert.True(new FileInfo(@".\Testing\SwiftBitmapOrResult.jpg").ReadBinary().SequenceEqual(new FileInfo(@"..\..\BitmapResults\SwiftBitmapOrResult.jpg").ReadBinary()));
                    }
                }
            }
        }

        [Fact]
        public void XOr()
        {
            using (Utilities.Media.SwiftBitmap Temp = new Utilities.Media.SwiftBitmap(@"..\..\Data\Image\Lenna.jpg"))
            {
                using (Utilities.Media.SwiftBitmap TestObject2 = new Utilities.Media.SwiftBitmap(@"..\..\Data\Image\Image2.jpg"))
                {
                    using (Utilities.Media.SwiftBitmap Temp2 = Temp ^ TestObject2)
                    {
                        Temp2.Save(@".\Testing\SwiftBitmapXOrResult.jpg");
                        Assert.True(new FileInfo(@".\Testing\SwiftBitmapXOrResult.jpg").ReadBinary().SequenceEqual(new FileInfo(@"..\..\BitmapResults\SwiftBitmapXOrResult.jpg").ReadBinary()));
                    }
                }
            }
        }

        [Fact]
        public void Crop()
        {
            using (Utilities.Media.SwiftBitmap Temp = new Utilities.Media.SwiftBitmap(@"..\..\Data\Image\Lenna.jpg"))
            {
                Temp.Crop(100, 100, Utilities.Media.Align.Top, Utilities.Media.Align.Left)
                    .Save(@".\Testing\SwiftBitmapCropResult.jpg");
                Assert.True(new FileInfo(@".\Testing\SwiftBitmapCropResult.jpg").ReadBinary().SequenceEqual(new FileInfo(@"..\..\BitmapResults\SwiftBitmapCropResult.jpg").ReadBinary()));
            }
        }

        [Fact]
        public void GetPixel()
        {
            using (Utilities.Media.SwiftBitmap Temp = new Utilities.Media.SwiftBitmap(@"..\..\Data\Image\Lenna.jpg"))
            {
                Temp.Lock();
                Assert.Equal(Color.FromArgb(224, 136, 127), Temp.GetPixel(1, 1));
                Assert.Equal(Color.FromArgb(191, 127, 125), Temp.GetPixel(200, 100));
                Assert.Equal(Color.FromArgb(180, 69, 75), Temp.GetPixel(53, 91));
                Temp.Unlock();
            }
        }

        [Fact]
        public void GetPixel2()
        {
            using (Utilities.Media.SwiftBitmap Temp = new Utilities.Media.SwiftBitmap(@"..\..\Data\Image\Lenna.jpg"))
            {
                Temp.Lock();
                Assert.Equal(Color.FromArgb(224, 136, 127), Temp.GetPixel((1 * Temp.Width) + 1));
                Assert.Equal(Color.FromArgb(191, 127, 125), Temp.GetPixel((100 * Temp.Width) + 200));
                Assert.Equal(Color.FromArgb(180, 69, 75), Temp.GetPixel((91 * Temp.Width) + 53));
                Temp.Unlock();
            }
        }

        [Fact]
        public void SetPixel()
        {
            using (Utilities.Media.SwiftBitmap Temp = new Utilities.Media.SwiftBitmap(@"..\..\Data\Image\Lenna.jpg"))
            {
                Temp.Lock();
                Temp.SetPixel(22, 43, Color.FromArgb(200, 100, 50));
                Assert.Equal(Color.FromArgb(200, 100, 50), Temp.GetPixel(22, 43));
                Temp.Unlock();
            }
        }

        [Fact]
        public void SetPixels()
        {
            using (Utilities.Media.SwiftBitmap Temp = new Utilities.Media.SwiftBitmap(@"..\..\Data\Image\Lenna.jpg"))
            {
                Temp.Lock();
                Temp.SetPixels(22, 43, new Color[] { Color.FromArgb(200, 100, 50), Color.FromArgb(201, 101, 51), Color.FromArgb(202, 102, 52), Color.FromArgb(203, 103, 53) });
                Assert.Equal(Color.FromArgb(200, 100, 50), Temp.GetPixel(22, 43));
                Assert.Equal(Color.FromArgb(201, 101, 51), Temp.GetPixel(23, 43));
                Assert.Equal(Color.FromArgb(202, 102, 52), Temp.GetPixel(24, 43));
                Assert.Equal(Color.FromArgb(203, 103, 53), Temp.GetPixel(25, 43));
                Temp.Unlock();
            }
        }
    }
}