/*
Copyright (c) 2010 <a href="http://www.gutgames.com">James Craig</a>

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

#region Usings
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using Utilities.IO;
#endregion

namespace Utilities.Web.Youtube
{
    /// <summary>
    /// Class to help with dealing with YouTube
    /// </summary>
    public static class Youtube
    {
        #region Public Static Functions

        /// <summary>
        /// Gets a movie and saves it
        /// </summary>
        /// <param name="FileLocation">Location to save the file</param>
        /// <param name="Url">The youtube url for the movie</param>
        public static void GetMovie(string FileLocation, string Url)
        {
            string Content = FileManager.GetFileContents(new Uri(Url));
            Regex TempRegex = new Regex("'SWF_ARGS': {(.*)}");
            Match Match = TempRegex.Match(Content);
            Content = Match.Value;
            TempRegex = new Regex("\"video_id\": \"(.*?\")");
            string VideoLocation = "http://www.youtube.com/get_video?video_id=";
            Match = TempRegex.Match(Content);
            VideoLocation += Match.Groups[1].Value.Replace("\"", "");
            VideoLocation += "&l=";
            TempRegex = new Regex("\"length_seconds\": \"(.*?\")");
            Match = TempRegex.Match(Content);
            VideoLocation += Match.Groups[1].Value.Replace("\"", "");
            VideoLocation += "&t=";
            TempRegex = new Regex("\"t\": \"(.*?\")");
            Match = TempRegex.Match(Content);
            VideoLocation += Match.Groups[1].Value.Replace("\"", "");
            Stream TempStream;
            WebClient Client;
            FileManager.GetFileContents(new Uri(VideoLocation), out TempStream, out Client);
            BinaryReader TempReader = new BinaryReader(TempStream);
            List<byte> Bytes = new List<byte>();
            while (true)
            {
                byte[] Buffer = new byte[1024];
                int Length = TempReader.Read(Buffer, 0, 1024);
                if (Length == 0)
                    break;
                for (int x = 0; x < Length; ++x)
                {
                    Bytes.Add(Buffer[x]);
                }
            }
            TempStream.Dispose();
            Client.Dispose();
            FileManager.SaveFile(Bytes.ToArray(), FileLocation);
        }

        /// <summary>
        /// Generates an HTML embed for a youtube video
        /// </summary>
        /// <param name="ID">ID of the video</param>
        /// <returns>The needed embed html tag for the video</returns>
        public static string GenerateEmbed(string ID)
        {
            return GenerateEmbed(ID, false, 320, 240);
        }

        /// <summary>
        /// Generates an HTML embed tag for a youtube video
        /// </summary>
        /// <param name="ID">ID of the video</param>
        /// <param name="AutoPlay">Should the video auto play?</param>
        /// <returns>The needed embed html tag</returns>
        public static string GenerateEmbed(string ID, bool AutoPlay)
        {
            return GenerateEmbed(ID, AutoPlay, 320, 240);
        }

        /// <summary>
        /// Generates an HTML embed tag for a youtube video
        /// </summary>
        /// <param name="ID">ID of the video</param>
        /// <param name="AutoPlay">Should the video auto play?</param>
        /// <param name="Height">Height of the video</param>
        /// <param name="Width">Width of the video</param>
        /// <returns>The needed embed html tag</returns>
        public static string GenerateEmbed(string ID, bool AutoPlay, int Width, int Height)
        {
            StringBuilder Builder = new StringBuilder();
            Builder.Append(@"<embed src='http://www.youtube.com/v/")
                .Append(ID).Append("&autoplay=").Append(AutoPlay ? 1 : 0)
                .Append(@"' type='application/x-shockwave-flash' 
                        allowscriptaccess='never' enableJavascript ='false' 
                        allowfullscreen='true' width='").Append(Width)
                .Append("' height='").Append(Height).Append(@"'></embed>");
            return Builder.ToString();
        }

        #endregion
    }
}