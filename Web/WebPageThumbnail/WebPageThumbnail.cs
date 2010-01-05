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
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using System.Windows.Forms;
#endregion

namespace Utilities.Web.WebPageThumbnail
{
    /// <summary>
    /// Class for taking a screen shot of a web page
    /// </summary>
    public class WebPageThumbnail
    {
        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public WebPageThumbnail()
        {
        }
        #endregion

        #region Private Variables
        private Bitmap Image;
        private string FileName;
        private string Url;
        private int Width;
        private int Height;
        #endregion

        #region Public Functions
        /// <summary>
        /// Generates a screen shot of a web site
        /// </summary>
        /// <param name="FileName">File name to save as</param>
        /// <param name="Url">Url to take the screen shot of</param>
        /// <param name="Width">Width of the image (-1 for full size)</param>
        /// <param name="Height">Height of the image (-1 for full size)</param>
        public void GenerateBitmap(string FileName, string Url, int Width, int Height)
        {
            this.Url = Url;
            this.FileName = FileName;
            this.Width = Width;
            this.Height = Height;
            Thread TempThread = new Thread(new ThreadStart(CreateBrowser));
            TempThread.SetApartmentState(ApartmentState.STA);
            TempThread.Start();
            TempThread.Join();
        }
        #endregion

        #region Private Functions
        /// <summary>
        /// Creates the browser
        /// </summary>
        private void CreateBrowser()
        {
            WebBrowser Browser = new WebBrowser();
            try
            {
                Browser.ScrollBarsEnabled = false;
                DateTime TimeoutStart = DateTime.Now;
                TimeSpan Timeout = new TimeSpan(0, 0, 10);
                Browser.Navigate(Url);
                Browser.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(Browser_DocumentCompleted);
                while (Browser.ReadyState != WebBrowserReadyState.Complete)
                {
                    if (DateTime.Now - TimeoutStart > Timeout)
                        break;
                    Application.DoEvents();
                }
            }
            catch { }
            finally
            {
                Browser.Dispose();
            }
        }
        /// <summary>
        /// Called when the browser is completed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Browser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            WebBrowser Browser = (WebBrowser)sender;
            Browser.ScriptErrorsSuppressed = true;
            Browser.ScrollBarsEnabled = false;
            if (Width == -1)
            {
                Browser.Width = Browser.Document.Body.ScrollRectangle.Width;
            }
            else
            {
                Browser.Width = Width;
            }
            if (Height == -1)
            {
                Browser.Height = Browser.Document.Body.ScrollRectangle.Height;
            }
            else
            {
                Browser.Height = Height;
            }
            Image = new Bitmap(Browser.Width, Browser.Height);
            Browser.BringToFront();
            Browser.DrawToBitmap(Image, new Rectangle(0, 0, Browser.Width, Browser.Height));
            Image.Save(FileName,ImageFormat.Bmp);
        }
        #endregion
    }
}
