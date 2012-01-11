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

        [Test]
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

        [Test]
        public void ToBase64()
        {
            using (Bitmap TestObject = new Bitmap(@"..\..\Data\Image\Lenna.jpg"))
            {
                Assert.True(TestObject.ToBase64() == "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"
                    || TestObject.ToBase64() == "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");
            }
        }

        public void Dispose()
        {
            new DirectoryInfo(@".\Testing").DeleteAll();
        }
    }
}
