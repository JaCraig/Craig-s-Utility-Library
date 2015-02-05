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

using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnitTests.Fixtures;
using Utilities.Media.Image.ExtensionMethods;
using Xunit;

namespace UnitTests.Media.Image.ExtensionMethods
{
    public class BitmapExtensions : TestingDirectoryFixture
    {
        [Fact]
        public void AddNoise()
        {
            using (Bitmap TestObject = new Bitmap(@"..\..\Data\Image\Lenna.jpg"))
            {
                using (Bitmap Image = TestObject.AddNoise(40, @".\Testing\LennaNoise.jpg"))
                {
                    Assert.NotNull(Image);
                }
            }
        }

        [Fact]
        public void AdjustBrightness()
        {
            using (Bitmap TestObject = new Bitmap(@"..\..\Data\Image\Lenna.jpg"))
            {
                using (Bitmap Image = TestObject.AdjustBrightness(40, @".\Testing\LennaBrightness.jpg"))
                {
                    Assert.NotNull(Image);
                }
            }
        }

        [Fact]
        public void AdjustContrast()
        {
            using (Bitmap TestObject = new Bitmap(@"..\..\Data\Image\Lenna.jpg"))
            {
                using (Bitmap Image = TestObject.AdjustContrast(40, @".\Testing\LennaContrast.jpg"))
                {
                    Assert.NotNull(Image);
                }
            }
        }

        [Fact]
        public void AdjustGamma()
        {
            using (Bitmap TestObject = new Bitmap(@"..\..\Data\Image\Lenna.jpg"))
            {
                using (Bitmap Image = TestObject.AdjustGamma(.2f, @".\Testing\LennaGamma.jpg"))
                {
                    Assert.NotNull(Image);
                }
            }
        }

        [Fact]
        public void And()
        {
            using (Bitmap TestObject = new Bitmap(@"..\..\Data\Image\Lenna.jpg"))
            {
                using (Bitmap TestObject2 = new Bitmap(@"..\..\Data\Image\Image2.jpg"))
                {
                    using (Bitmap Image = TestObject.And(TestObject2, @".\Testing\LennaAnd.jpg"))
                    {
                        Assert.NotNull(Image);
                    }
                }
            }
        }

        [Fact]
        public void ASCIIArt()
        {
            using (Bitmap TestObject = new Bitmap(@"..\..\Data\Image\Lenna.jpg"))
            {
                string Value = TestObject.ToASCIIArt();
                Assert.NotNull(Value);
                Assert.NotEmpty(Value);
                Assert.Equal(24420, Value.Length);
            }
        }

        [Fact]
        public void BlackAndWhite()
        {
            using (Bitmap TestObject = new Bitmap(@"..\..\Data\Image\Lenna.jpg"))
            {
                using (Bitmap Image = TestObject.BlackAndWhite(@".\Testing\LennaBlackAndWhite.jpg"))
                {
                    Assert.NotNull(Image);
                }
            }
        }

        [Fact]
        public void BlueFilter()
        {
            using (Bitmap TestObject = new Bitmap(@"..\..\Data\Image\Lenna.jpg"))
            {
                using (Bitmap Image = TestObject.BlueFilter(@".\Testing\LennaBlueFilter.jpg"))
                {
                    Assert.NotNull(Image);
                }
            }
        }

        [Fact]
        public void BoxBlur()
        {
            using (Bitmap TestObject = new Bitmap(@"..\..\Data\Image\Lenna.jpg"))
            {
                using (Bitmap Image = TestObject.BoxBlur(5, @".\Testing\LennaBoxBlur.jpg"))
                {
                    Assert.NotNull(Image);
                }
            }
        }

        [Fact]
        public void BumpMap()
        {
            using (Bitmap TestObject = new Bitmap(@"..\..\Data\Image\Lenna.jpg"))
            {
                using (Bitmap Value = TestObject.BumpMap())
                {
                    Assert.NotNull(Value);
                }
            }
        }

        [Fact]
        public void Colorize()
        {
            using (Bitmap TestObject = new Bitmap(@"..\..\Data\Image\Lenna.jpg"))
            {
                List<Color> Colors = new List<Color>();
                for (int x = 0; x < 256; ++x)
                    Colors.Add(Color.FromArgb(255 - x, 0, x));
                using (Bitmap TestObject2 = TestObject.BlackAndWhite())
                {
                    using (Bitmap Image = TestObject2.Colorize(Colors.ToArray(), @".\Testing\LennaColorize.jpg"))
                    {
                        Assert.NotNull(Image);
                    }
                }
            }
        }

        [Fact]
        public void Crop()
        {
            using (Bitmap TestObject = new Bitmap(@"..\..\Data\Image\Lenna.jpg"))
            {
                using (Bitmap Image = TestObject.Crop(100, 100,
                                                                Utilities.Media.Image.ExtensionMethods.Align.Bottom,
                                                                Utilities.Media.Image.ExtensionMethods.Align.Right,
                                                                @".\Testing\LennaCrop.jpg"))
                {
                    Assert.NotNull(Image);
                }
            }
        }

        [Fact]
        public void Dilate()
        {
            using (Bitmap TestObject = new Bitmap(@"..\..\Data\Image\Lenna.jpg"))
            {
                using (Bitmap Image = TestObject.Dilate(5, @".\Testing\LennaDilate.jpg"))
                {
                    Assert.NotNull(Image);
                }
            }
        }

        [Fact]
        public void DrawRoundedRectangle()
        {
            using (Bitmap TestObject = new Bitmap(@"..\..\Data\Image\Lenna.jpg"))
            {
                using (Bitmap Image = TestObject.DrawRoundedRectangle(Color.Red, 20, 20, 40, 40, 4, @".\Testing\LennaDrawRoundedRectangle.jpg"))
                {
                    Assert.NotNull(Image);
                }
            }
        }

        [Fact]
        public void DrawText()
        {
            using (Bitmap TestObject = new Bitmap(@"..\..\Data\Image\Lenna.jpg"))
            {
                using (Bitmap Image = TestObject.DrawText("Test text.", new Font(FontFamily.GenericSansSerif, 20.0f), Brushes.Black, new RectangleF(0, 0, 300, 300), @".\Testing\LennaDrawText.jpg"))
                {
                    Assert.NotNull(Image);
                }
            }
        }

        [Fact]
        public void EdgeDetection()
        {
            using (Bitmap TestObject = new Bitmap(@"..\..\Data\Image\Lenna.jpg"))
            {
                using (Bitmap Image = TestObject.EdgeDetection(25.0f, Color.Red, @".\Testing\LennaEdgeDetection.jpg"))
                {
                    Assert.NotNull(Image);
                }
            }
        }

        [Fact]
        public void Emboss()
        {
            using (Bitmap TestObject = new Bitmap(@"..\..\Data\Image\Lenna.jpg"))
            {
                using (Bitmap Image = TestObject.Emboss(@".\Testing\LennaEmboss.jpg"))
                {
                    Assert.NotNull(Image);
                }
            }
        }

        [Fact]
        public void Equalize()
        {
            using (Bitmap TestObject = new Bitmap(@"..\..\Data\Image\Lenna.jpg"))
            {
                using (Bitmap Image = TestObject.Equalize(@".\Testing\LennaEqualize.jpg"))
                {
                    Assert.NotNull(Image);
                }
            }
        }

        [Fact]
        public void Flip()
        {
            using (Bitmap TestObject = new Bitmap(@"..\..\Data\Image\Lenna.jpg"))
            {
                using (Bitmap Image = TestObject.Flip(true, true, @".\Testing\LennaFlip.jpg"))
                {
                    Assert.NotNull(Image);
                }
            }
        }

        [Fact]
        public void GaussianBlur()
        {
            using (Bitmap TestObject = new Bitmap(@"..\..\Data\Image\Lenna.jpg"))
            {
                using (Bitmap Image = TestObject.GaussianBlur(3, @".\Testing\LennaGaussianBlur.jpg"))
                {
                    Assert.NotNull(Image);
                }
            }
        }

        [Fact]
        public void GetHTMLPalette()
        {
            using (Bitmap TestObject = new Bitmap(@"..\..\Data\Image\Lenna.jpg"))
            {
                IEnumerable<string> Image = TestObject.GetHTMLPalette();
                Assert.NotNull(Image);
                Assert.Equal(37270, Image.Count());
            }
        }

        [Fact]
        public void GreenFilter()
        {
            using (Bitmap TestObject = new Bitmap(@"..\..\Data\Image\Lenna.jpg"))
            {
                using (Bitmap Image = TestObject.GreenFilter(@".\Testing\LennaGreenFilter.jpg"))
                {
                    Assert.NotNull(Image);
                }
            }
        }

        [Fact]
        public void Jitter()
        {
            using (Bitmap TestObject = new Bitmap(@"..\..\Data\Image\Lenna.jpg"))
            {
                using (Bitmap Image = TestObject.Jitter(5, @".\Testing\LennaJitter.jpg"))
                {
                    Assert.NotNull(Image);
                }
            }
        }

        [Fact]
        public void KuwaharaBlur()
        {
            using (Bitmap TestObject = new Bitmap(@"..\..\Data\Image\Lenna.jpg"))
            {
                using (Bitmap Image = TestObject.KuwaharaBlur(5, @".\Testing\LennaKuwaharaBlur.jpg"))
                {
                    Assert.NotNull(Image);
                }
            }
        }

        [Fact]
        public void LaplaceEdgeDetection()
        {
            using (Bitmap TestObject = new Bitmap(@"..\..\Data\Image\Lenna.jpg"))
            {
                using (Bitmap Image = TestObject.LaplaceEdgeDetection(@".\Testing\LennaLaplaceEdgeDetection.jpg"))
                {
                    Assert.NotNull(Image);
                }
            }
        }

        [Fact]
        public void MedianFilter()
        {
            using (Bitmap TestObject = new Bitmap(@"..\..\Data\Image\Lenna.jpg"))
            {
                using (Bitmap Image = TestObject.MedianFilter(5, @".\Testing\LennaMedianFilter.jpg"))
                {
                    Assert.NotNull(Image);
                }
            }
        }

        [Fact]
        public void MotionDetection()
        {
            using (Bitmap TestObject = new Bitmap(@"..\..\Data\Image\Lenna.jpg"))
            {
                using (Bitmap TestObject2 = TestObject.Rotate(10.0f))
                {
                    using (Bitmap Value = TestObject.MotionDetection(TestObject2, 25, Color.Red))
                    {
                        Assert.NotNull(Value);
                    }
                }
            }
        }

        [Fact]
        public void Negative()
        {
            using (Bitmap TestObject = new Bitmap(@"..\..\Data\Image\Lenna.jpg"))
            {
                using (Bitmap Image = TestObject.Negative(@".\Testing\LennaNegative.jpg"))
                {
                    Assert.NotNull(Image);
                }
            }
        }

        [Fact]
        public void NormalMap()
        {
            using (Bitmap TestObject = new Bitmap(@"..\..\Data\Image\Lenna.jpg"))
            {
                using (Bitmap Value = TestObject.NormalMap())
                {
                    Assert.NotNull(Value);
                }
            }
        }

        [Fact]
        public void OilPainting()
        {
            using (Bitmap TestObject = new Bitmap(@"..\..\Data\Image\Lenna.jpg"))
            {
                using (Bitmap OilPainting = TestObject.OilPainting(613409124, 500))
                {
                    Assert.NotNull(OilPainting);
                }
            }
        }

        [Fact]
        public void Or()
        {
            using (Bitmap TestObject = new Bitmap(@"..\..\Data\Image\Lenna.jpg"))
            {
                using (Bitmap TestObject2 = new Bitmap(@"..\..\Data\Image\Image2.jpg"))
                {
                    using (Bitmap Image = TestObject.Or(TestObject2, @".\Testing\LennaOr.jpg"))
                    {
                        Assert.NotNull(Image);
                    }
                }
            }
        }

        [Fact]
        public void Pixelate()
        {
            using (Bitmap TestObject = new Bitmap(@"..\..\Data\Image\Lenna.jpg"))
            {
                using (Bitmap Image = TestObject.Pixelate(10, @".\Testing\LennaPixelate.jpg"))
                {
                    Assert.NotNull(Image);
                }
            }
        }

        [Fact]
        public void RedFilter()
        {
            using (Bitmap TestObject = new Bitmap(@"..\..\Data\Image\Lenna.jpg"))
            {
                using (Bitmap Image = TestObject.RedFilter(@".\Testing\LennaRedFilter.jpg"))
                {
                    Assert.NotNull(Image);
                }
            }
        }

        [Fact]
        public void Resize()
        {
            using (Bitmap TestObject = new Bitmap(@"..\..\Data\Image\Lenna.jpg"))
            {
                using (Bitmap Image = TestObject.Resize(50, Utilities.Media.Image.ExtensionMethods.Quality.Low, @".\Testing\LennaResize.jpg"))
                {
                    Assert.NotNull(Image);
                    Assert.Equal(50, Image.Width);
                    Assert.Equal(50, Image.Height);
                }
            }
        }

        [Fact]
        public void Rotate()
        {
            using (Bitmap TestObject = new Bitmap(@"..\..\Data\Image\Lenna.jpg"))
            {
                using (Bitmap Image = TestObject.Rotate(50.0f, @".\Testing\LennaRotate.jpg"))
                {
                    Assert.NotNull(Image);
                }
            }
        }

        [Fact]
        public void SepiaTone()
        {
            using (Bitmap TestObject = new Bitmap(@"..\..\Data\Image\Lenna.jpg"))
            {
                using (Bitmap Image = TestObject.SepiaTone(@".\Testing\LennaSepiaTone.jpg"))
                {
                    Assert.NotNull(Image);
                }
            }
        }

        public void SetFixture(TestingDirectoryFixture data)
        {
        }

        [Fact]
        public void Sharpen()
        {
            using (Bitmap TestObject = new Bitmap(@"..\..\Data\Image\Lenna.jpg"))
            {
                using (Bitmap Image = TestObject.Sharpen(@".\Testing\LennaSharpen.jpg"))
                {
                    Assert.NotNull(Image);
                }
            }
        }

        [Fact]
        public void SharpenLess()
        {
            using (Bitmap TestObject = new Bitmap(@"..\..\Data\Image\Lenna.jpg"))
            {
                using (Bitmap Image = TestObject.SharpenLess(@".\Testing\LennaSharpenLess.jpg"))
                {
                    Assert.NotNull(Image);
                }
            }
        }

        [Fact]
        public void SinWave()
        {
            using (Bitmap TestObject = new Bitmap(@"..\..\Data\Image\Lenna.jpg"))
            {
                using (Bitmap Image = TestObject.SinWave(10f, 5f, false, true, @".\Testing\LennaSinWave.jpg"))
                {
                    Assert.NotNull(Image);
                }
            }
        }

        [Fact]
        public void SNNBlur()
        {
            using (Bitmap TestObject = new Bitmap(@"..\..\Data\Image\Lenna.jpg"))
            {
                using (Bitmap Image = TestObject.SNNBlur(5, @".\Testing\LennaSNNBlur.jpg"))
                {
                    Assert.NotNull(Image);
                }
            }
        }

        [Fact]
        public void SobelEdgeDetection()
        {
            using (Bitmap TestObject = new Bitmap(@"..\..\Data\Image\Lenna.jpg"))
            {
                using (Bitmap Image = TestObject.SobelEdgeDetection(@".\Testing\LennaSobelEdgeDetection.jpg"))
                {
                    Assert.NotNull(Image);
                }
            }
        }

        [Fact]
        public void SobelEmboss()
        {
            using (Bitmap TestObject = new Bitmap(@"..\..\Data\Image\Lenna.jpg"))
            {
                using (Bitmap Image = TestObject.SobelEmboss(@".\Testing\LennaSobelEmboss.jpg"))
                {
                    Assert.NotNull(Image);
                }
            }
        }

        [Fact]
        public void StretchContrast()
        {
            using (Bitmap TestObject = new Bitmap(@"..\..\Data\Image\Lenna.jpg"))
            {
                using (Bitmap Image = TestObject.StretchContrast(@".\Testing\LennaStretchContrast.jpg"))
                {
                    Assert.NotNull(Image);
                }
            }
        }

        [Fact]
        public void Threshold()
        {
            using (Bitmap TestObject = new Bitmap(@"..\..\Data\Image\Lenna.jpg"))
            {
                using (Bitmap Image = TestObject.Threshold(0.5f, @".\Testing\LennaThreshold.jpg"))
                {
                    Assert.NotNull(Image);
                }
            }
        }

        [Fact]
        public void ToBase64()
        {
            using (Bitmap TestObject = new Bitmap(@"..\..\Data\Image\Lenna.jpg"))
            {
                Assert.Contains(TestObject.ToBase64(), new string[]{
                    "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",
                    "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",
                    "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",
                    "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"
                });
            }
        }

        [Fact]
        public void Turbulence()
        {
            using (Bitmap TestObject = new Bitmap(@"..\..\Data\Image\Lenna.jpg"))
            {
                using (Bitmap Image = TestObject.Turbulence(8, 5.0f, 26542346, @".\Testing\LennaTurbulence.jpg"))
                {
                    Assert.NotNull(Image);
                }
            }
        }

        [Fact]
        public void Watermark()
        {
            using (Bitmap TestObject = new Bitmap(@"..\..\Data\Image\Lenna.jpg"))
            {
                using (Bitmap TestObject2 = new Bitmap(@"..\..\Data\Image\Image2.jpg"))
                {
                    using (Bitmap Image = TestObject.Watermark(TestObject2, 0.5f, 0, 0, Color.Black, @".\Testing\LennaWatermark.jpg"))
                    {
                        Assert.NotNull(Image);
                    }
                }
            }
        }

        [Fact]
        public void Xor()
        {
            using (Bitmap TestObject = new Bitmap(@"..\..\Data\Image\Lenna.jpg"))
            {
                using (Bitmap TestObject2 = new Bitmap(@"..\..\Data\Image\Image2.jpg"))
                {
                    using (Bitmap Image = TestObject.Xor(TestObject2, @".\Testing\LennaXor.jpg"))
                    {
                        Assert.NotNull(Image);
                    }
                }
            }
        }
    }
}