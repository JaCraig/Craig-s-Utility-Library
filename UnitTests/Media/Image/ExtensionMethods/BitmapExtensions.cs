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
                                                                @".\Testing\LennaCrop.jpg")))
                {
                    Assert.NotNull(Image);
                }
            }
        }

        [Test]
        public void Dilate()
        {
            using (Bitmap TestObject = new Bitmap(@"..\..\Data\Image\Lenna.jpg"))
            {
                using (Bitmap Image = Assert.Do<Bitmap>(() => TestObject.Dilate(5, @".\Testing\LennaDilate.jpg")))
                {
                    Assert.NotNull(Image);
                }
            }
        }

        [Test]
        public void DrawRoundedRectangle()
        {
            using (Bitmap TestObject = new Bitmap(@"..\..\Data\Image\Lenna.jpg"))
            {
                using (Bitmap Image = Assert.Do<Bitmap>(() => TestObject.DrawRoundedRectangle(Color.Red, 20, 20, 40, 40, 4, @".\Testing\LennaDrawRoundedRectangle.jpg")))
                {
                    Assert.NotNull(Image);
                }
            }
        }

        [Test]
        public void DrawText()
        {
            using (Bitmap TestObject = new Bitmap(@"..\..\Data\Image\Lenna.jpg"))
            {
                using (Bitmap Image = Assert.Do<Bitmap>(() => TestObject.DrawText("Test text.", new Font(FontFamily.GenericSansSerif, 20.0f), Brushes.Black, new RectangleF(0, 0, 300, 300), @".\Testing\LennaDrawText.jpg")))
                {
                    Assert.NotNull(Image);
                }
            }
        }

        [Test]
        public void EdgeDetection()
        {
            using (Bitmap TestObject = new Bitmap(@"..\..\Data\Image\Lenna.jpg"))
            {
                using (Bitmap Image = Assert.Do<Bitmap>(() => TestObject.EdgeDetection(25.0f,Color.Red, @".\Testing\LennaEdgeDetection.jpg")))
                {
                    Assert.NotNull(Image);
                }
            }
        }

        [Test]
        public void Emboss()
        {
            using (Bitmap TestObject = new Bitmap(@"..\..\Data\Image\Lenna.jpg"))
            {
                using (Bitmap Image = Assert.Do<Bitmap>(() => TestObject.Emboss(@".\Testing\LennaEmboss.jpg")))
                {
                    Assert.NotNull(Image);
                }
            }
        }

        [Test]
        public void Equalize()
        {
            using (Bitmap TestObject = new Bitmap(@"..\..\Data\Image\Lenna.jpg"))
            {
                using (Bitmap Image = Assert.Do<Bitmap>(() => TestObject.Equalize(@".\Testing\LennaEqualize.jpg")))
                {
                    Assert.NotNull(Image);
                }
            }
        }

        [Test]
        public void Flip()
        {
            using (Bitmap TestObject = new Bitmap(@"..\..\Data\Image\Lenna.jpg"))
            {
                using (Bitmap Image = Assert.Do<Bitmap>(() => TestObject.Flip(true, true, @".\Testing\LennaFlip.jpg")))
                {
                    Assert.NotNull(Image);
                }
            }
        }

        [Test]
        public void GaussianBlur()
        {
            using (Bitmap TestObject = new Bitmap(@"..\..\Data\Image\Lenna.jpg"))
            {
                using (Bitmap Image = Assert.Do<Bitmap>(() => TestObject.GaussianBlur(3, @".\Testing\LennaGaussianBlur.jpg")))
                {
                    Assert.NotNull(Image);
                }
            }
        }

        [Test("Takes a while to run")]
        public void GetHTMLPalette()
        {
            using (Bitmap TestObject = new Bitmap(@"..\..\Data\Image\Lenna.jpg"))
            {
                List<string> Image = Assert.Do<List<string>>(() => TestObject.GetHTMLPalette());
                Assert.NotNull(Image);
                Assert.Equal(37270, Image.Count);
            }
        }

        [Test]
        public void GreenFilter()
        {
            using (Bitmap TestObject = new Bitmap(@"..\..\Data\Image\Lenna.jpg"))
            {
                using (Bitmap Image = Assert.Do<Bitmap>(() => TestObject.GreenFilter(@".\Testing\LennaGreenFilter.jpg")))
                {
                    Assert.NotNull(Image);
                }
            }
        }

        [Test]
        public void Jitter()
        {
            using (Bitmap TestObject = new Bitmap(@"..\..\Data\Image\Lenna.jpg"))
            {
                using (Bitmap Image = Assert.Do<Bitmap>(() => TestObject.Jitter(5,@".\Testing\LennaJitter.jpg")))
                {
                    Assert.NotNull(Image);
                }
            }
        }

        [Test]
        public void KuwaharaBlur()
        {
            using (Bitmap TestObject = new Bitmap(@"..\..\Data\Image\Lenna.jpg"))
            {
                using (Bitmap Image = Assert.Do<Bitmap>(() => TestObject.KuwaharaBlur(5, @".\Testing\LennaKuwaharaBlur.jpg")))
                {
                    Assert.NotNull(Image);
                }
            }
        }

        [Test]
        public void LaplaceEdgeDetection()
        {
            using (Bitmap TestObject = new Bitmap(@"..\..\Data\Image\Lenna.jpg"))
            {
                using (Bitmap Image = Assert.Do<Bitmap>(() => TestObject.LaplaceEdgeDetection(@".\Testing\LennaLaplaceEdgeDetection.jpg")))
                {
                    Assert.NotNull(Image);
                }
            }
        }

        [Test]
        public void MedianFilter()
        {
            using (Bitmap TestObject = new Bitmap(@"..\..\Data\Image\Lenna.jpg"))
            {
                using (Bitmap Image = Assert.Do<Bitmap>(() => TestObject.MedianFilter(5, @".\Testing\LennaMedianFilter.jpg")))
                {
                    Assert.NotNull(Image);
                }
            }
        }

        [Test]
        public void Negative()
        {
            using (Bitmap TestObject = new Bitmap(@"..\..\Data\Image\Lenna.jpg"))
            {
                using (Bitmap Image = Assert.Do<Bitmap>(() => TestObject.Negative(@".\Testing\LennaNegative.jpg")))
                {
                    Assert.NotNull(Image);
                }
            }
        }

        [Test]
        public void Or()
        {
            using (Bitmap TestObject = new Bitmap(@"..\..\Data\Image\Lenna.jpg"))
            {
                using (Bitmap TestObject2 = new Bitmap(@"..\..\Data\Image\Image2.jpg"))
                {
                    using (Bitmap Image = Assert.Do<Bitmap>(() => TestObject.Or(TestObject2, @".\Testing\LennaOr.jpg")))
                    {
                        Assert.NotNull(Image);
                    }
                }
            }
        }

        [Test]
        public void Pixelate()
        {
            using (Bitmap TestObject = new Bitmap(@"..\..\Data\Image\Lenna.jpg"))
            {
                using (Bitmap Image = Assert.Do<Bitmap>(() => TestObject.Pixelate(10, @".\Testing\LennaPixelate.jpg")))
                {
                    Assert.NotNull(Image);
                }
            }
        }

        [Test]
        public void RedFilter()
        {
            using (Bitmap TestObject = new Bitmap(@"..\..\Data\Image\Lenna.jpg"))
            {
                using (Bitmap Image = Assert.Do<Bitmap>(() => TestObject.RedFilter(@".\Testing\LennaRedFilter.jpg")))
                {
                    Assert.NotNull(Image);
                }
            }
        }

        [Test]
        public void Resize()
        {
            using (Bitmap TestObject = new Bitmap(@"..\..\Data\Image\Lenna.jpg"))
            {
                using (Bitmap Image = Assert.Do<Bitmap>(() => TestObject.Resize(50, Utilities.Media.Image.ExtensionMethods.BitmapExtensions.Quality.Low, @".\Testing\LennaResize.jpg")))
                {
                    Assert.NotNull(Image);
                    Assert.Equal(50,Image.Width);
                    Assert.Equal(50, Image.Height);
                }
            }
        }

        [Test]
        public void Rotate()
        {
            using (Bitmap TestObject = new Bitmap(@"..\..\Data\Image\Lenna.jpg"))
            {
                using (Bitmap Image = Assert.Do<Bitmap>(() => TestObject.Rotate(50.0f, @".\Testing\LennaRotate.jpg")))
                {
                    Assert.NotNull(Image);
                }
            }
        }

        [Test]
        public void Sharpen()
        {
            using (Bitmap TestObject = new Bitmap(@"..\..\Data\Image\Lenna.jpg"))
            {
                using (Bitmap Image = Assert.Do<Bitmap>(() => TestObject.Sharpen(@".\Testing\LennaSharpen.jpg")))
                {
                    Assert.NotNull(Image);
                }
            }
        }

        [Test]
        public void SharpenLess()
        {
            using (Bitmap TestObject = new Bitmap(@"..\..\Data\Image\Lenna.jpg"))
            {
                using (Bitmap Image = Assert.Do<Bitmap>(() => TestObject.SharpenLess(@".\Testing\LennaSharpenLess.jpg")))
                {
                    Assert.NotNull(Image);
                }
            }
        }

        [Test]
        public void SinWave()
        {
            using (Bitmap TestObject = new Bitmap(@"..\..\Data\Image\Lenna.jpg"))
            {
                using (Bitmap Image = Assert.Do<Bitmap>(() => TestObject.SinWave(10f, 5f, false, true, @".\Testing\LennaSinWave.jpg")))
                {
                    Assert.NotNull(Image);
                }
            }
        }

        [Test]
        public void SobelEdgeDetection()
        {
            using (Bitmap TestObject = new Bitmap(@"..\..\Data\Image\Lenna.jpg"))
            {
                using (Bitmap Image = Assert.Do<Bitmap>(() => TestObject.SobelEdgeDetection(@".\Testing\LennaSobelEdgeDetection.jpg")))
                {
                    Assert.NotNull(Image);
                }
            }
        }

        [Test]
        public void SobelEmboss()
        {
            using (Bitmap TestObject = new Bitmap(@"..\..\Data\Image\Lenna.jpg"))
            {
                using (Bitmap Image = Assert.Do<Bitmap>(() => TestObject.SobelEmboss(@".\Testing\LennaSobelEmboss.jpg")))
                {
                    Assert.NotNull(Image);
                }
            }
        }

        [Test]
        public void SNNBlur()
        {
            using (Bitmap TestObject = new Bitmap(@"..\..\Data\Image\Lenna.jpg"))
            {
                using (Bitmap Image = Assert.Do<Bitmap>(() => TestObject.SNNBlur(5, @".\Testing\LennaSNNBlur.jpg")))
                {
                    Assert.NotNull(Image);
                }
            }
        }

        [Test]
        public void StretchContrast()
        {
            using (Bitmap TestObject = new Bitmap(@"..\..\Data\Image\Lenna.jpg"))
            {
                using (Bitmap Image = Assert.Do<Bitmap>(() => TestObject.StretchContrast(@".\Testing\LennaStretchContrast.jpg")))
                {
                    Assert.NotNull(Image);
                }
            }
        }

        [Test]
        public void Threshold()
        {
            using (Bitmap TestObject = new Bitmap(@"..\..\Data\Image\Lenna.jpg"))
            {
                using (Bitmap Image = Assert.Do<Bitmap>(() => TestObject.Threshold(0.5f, @".\Testing\LennaThreshold.jpg")))
                {
                    Assert.NotNull(Image);
                }
            }
        }

        [Test]
        public void Turbulence()
        {
            using (Bitmap TestObject = new Bitmap(@"..\..\Data\Image\Lenna.jpg"))
            {
                using (Bitmap Image = Assert.Do<Bitmap>(() => TestObject.Turbulence(8, 5.0f, 26542346, @".\Testing\LennaTurbulence.jpg")))
                {
                    Assert.NotNull(Image);
                }
            }
        }

        [Test]
        public void Watermark()
        {
            using (Bitmap TestObject = new Bitmap(@"..\..\Data\Image\Lenna.jpg"))
            {
                using (Bitmap TestObject2 = new Bitmap(@"..\..\Data\Image\Image2.jpg"))
                {
                    using (Bitmap Image = Assert.Do<Bitmap>(() => TestObject.Watermark(TestObject2, 0.5f, 0, 0, Color.Black, @".\Testing\LennaWatermark.jpg")))
                    {
                        Assert.NotNull(Image);
                    }
                }
            }
        }

        [Test]
        public void Xor()
        {
            using (Bitmap TestObject = new Bitmap(@"..\..\Data\Image\Lenna.jpg"))
            {
                using (Bitmap TestObject2 = new Bitmap(@"..\..\Data\Image\Image2.jpg"))
                {
                    using (Bitmap Image = Assert.Do<Bitmap>(() => TestObject.Xor(TestObject2, @".\Testing\LennaXor.jpg")))
                    {
                        Assert.NotNull(Image);
                    }
                }
            }
        }

        public void Dispose()
        {
            new DirectoryInfo(@".\Testing").DeleteAll();
        }
    }
}
