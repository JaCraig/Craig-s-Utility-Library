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

using Ironman.Core.Assets.Enums;
using Ironman.Core.Assets.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Drawing.Imaging;
using System.Linq;
using System.Text.RegularExpressions;
using Utilities.DataTypes;
using Utilities.IO;
using Utilities.Media;

namespace Ironman.Core.Assets.Filters
{
    /// <summary>
    /// Embeds the images and fonts that are linked from the CSS file
    /// </summary>
    public class CSSFixImagesAndFonts : IFilter
    {
        /// <summary>
        /// Filter name
        /// </summary>
        public string Name { get { return "CSS Embed Images"; } }

        /// <summary>
        /// Time to run the filter
        /// </summary>
        public RunTime TimeToRun { get { return RunTime.PreCombine; } }

        /// <summary>
        /// Used to determine images in the CSS file
        /// </summary>
        private Regex ImageRegex = new Regex(@"url\([""']*(?<File>[^""')]*)[""']*\)", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// Filters the assets
        /// </summary>
        /// <param name="Assets">Assets to filter</param>
        /// <returns>The filtered assets</returns>
        public IList<IAsset> Filter(IList<IAsset> Assets)
        {
            if (Assets == null || Assets.Count == 0)
                return new List<IAsset>();
            if (Assets.FirstOrDefault().Type != AssetType.CSS)
                return Assets;
            foreach (IAsset Asset in Assets)
            {
                foreach (Match ImageMatch in ImageRegex.Matches(Asset.Content))
                {
                    string TempFile = ImageMatch.Groups["File"].Value;
                    string MatchString = ImageMatch.Value;
                    FileInfo File = new FileInfo(TempFile);
                    File = DetermineFile(File, Asset, TempFile);
                    if (File == null || !File.Exists)
                    {
                        FileInfo AssetFile = new FileInfo(Asset.Path);
                        File = new FileInfo(AssetFile.Directory.FullName + "\\" + TempFile);
                    }
                    if (File.Exists)
                    {
                        if (File.Extension.ToUpperInvariant() == ".TTF"
                            || File.Extension.ToUpperInvariant() == ".OTF"
                            || File.Extension.ToUpperInvariant() == ".WOFF"
                            || File.Extension.ToUpperInvariant() == ".SVG"
                            || File.Extension.ToUpperInvariant() == ".EOT")
                        {
                            string MIME = "";
                            if (File.Extension.ToUpperInvariant() == ".WOFF")
                                MIME = "application/x-font-woff";
                            else if (File.Extension.ToUpperInvariant() == ".OTF")
                                MIME = "application/x-font-opentype";
                            else if (File.Extension.ToUpperInvariant() == ".TTF")
                                MIME = "application/x-font-ttf";
                            else if (File.Extension.ToUpperInvariant() == ".SVG")
                                MIME = "image/svg+xml";
                            else if (File.Extension.ToUpperInvariant() == ".EOT")
                                MIME = "application/vnd.ms-fontobject";

                            Asset.Content = Asset.Content.Replace(MatchString, "url(data:" + MIME + ";base64," + File.ReadBinary().ToString(Base64FormattingOptions.None) + ")");
                        }
                        else
                        {
                            using (SwiftBitmap TempImage = new SwiftBitmap(File.FullName))
                            {
                                string MIMEType = "image/jpeg";
                                string Content = "";
                                if (File.FullName.ToUpperInvariant().EndsWith(".PNG", StringComparison.Ordinal))
                                {
                                    MIMEType = "image/png";
                                    Content = TempImage.ToString(ImageFormat.Png);
                                }
                                else if (File.FullName.ToUpperInvariant().EndsWith(".JPG", StringComparison.Ordinal) || File.FullName.ToUpperInvariant().EndsWith(".JPEG", StringComparison.Ordinal))
                                {
                                    MIMEType = "image/jpeg";
                                    Content = TempImage.ToString(ImageFormat.Jpeg);
                                }
                                else if (File.FullName.ToUpperInvariant().EndsWith(".GIF", StringComparison.Ordinal))
                                {
                                    MIMEType = "image/gif";
                                    Content = TempImage.ToString(ImageFormat.Gif);
                                }
                                else if (File.FullName.ToUpperInvariant().EndsWith(".TIFF", StringComparison.Ordinal))
                                {
                                    MIMEType = "image/tiff";
                                    Content = TempImage.ToString(ImageFormat.Tiff);
                                }
                                else if (File.FullName.ToUpperInvariant().EndsWith(".BMP", StringComparison.Ordinal))
                                {
                                    MIMEType = "image/bmp";
                                    Content = TempImage.ToString(ImageFormat.Bmp);
                                }
                                Asset.Content = Asset.Content.Replace(MatchString, "url(data:" + MIMEType + ";base64," + Content + ")");
                            }
                        }
                    }
                }
            }
            return Assets;
        }

        private FileInfo DetermineFile(FileInfo File, IAsset Asset, string TempFile)
        {
            Contract.Requires<ArgumentNullException>(Asset != null, "Asset");
            if (File == null || !File.Exists)
            {
                FileInfo AssetFile = new FileInfo(Asset.Path);
                File = new FileInfo(AssetFile.Directory.FullName + "\\" + TempFile);
            }
            if (File == null || !File.Exists)
            {
                foreach (IAsset SubAsset in Asset.Included)
                {
                    FileInfo Temp = DetermineFile(File, SubAsset, TempFile);
                    if (Temp.Exists)
                        return Temp;
                }
            }
            return File;
        }
    }
}