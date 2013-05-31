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

#region Usings
using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Utilities.FileFormats.BaseClasses;
using Utilities.IO.ExtensionMethods;
#endregion

namespace Utilities.FileFormats.RSD
{
    /// <summary>
    /// Basic helper for RSD
    /// </summary>
    public class RSD : StringFormatBase<RSD>
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public RSD()
        {
            APIs = new APIs();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="FileContent">Content of the RSD file</param>
        public RSD(string FileContent)
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(FileContent),"FileContent");
            LoadContent(FileContent);
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Engine name
        /// </summary>
        public virtual string EngineName { get; set; }

        /// <summary>
        /// Link to the engine
        /// </summary>
        public virtual string EngineLink { get; set; }

        /// <summary>
        /// Link to the home page
        /// </summary>
        public virtual string HomePageLink { get; set; }

        /// <summary>
        /// API definitions
        /// </summary>
        public virtual APIs APIs { get; set; }

        #endregion

        #region Public Overridden Functions

        /// <summary>
        /// Loads the object from the data specified
        /// </summary>
        /// <param name="Data">Data to load into the object</param>
        protected override void LoadFromData(string Data)
        {
            LoadContent(Data);
        }

        private void LoadContent(string Content)
        {
            XDocument Document = XDocument.Parse(Content);
            foreach (XElement Service in Document.Elements("RSD").Elements("service"))
            {
                if (Service.Element("engineName") != null)
                    EngineName = Service.Element("engineName").Value;
                if (Service.Element("engineLink") != null)
                    EngineLink = Service.Element("engineLink").Value;
                if (Service.Element("homePageLink") != null)
                    HomePageLink = Service.Element("homePageLink").Value;
                if (Service.Element("apis") != null)
                    APIs = new APIs(Service.Element("apis"));
            }
        }

        /// <summary>
        /// Outputs the RSD file
        /// </summary>
        /// <returns>return the properly formatted RSD file</returns>
        public override string ToString()
        {
            StringBuilder Builder = new StringBuilder("<?xml version=\"1.0\" encoding=\"utf-8\"?><rsd version=\"1.0\"><service><engineName>");
            Builder.Append(EngineName).Append("</engineName><engineLink>").Append(EngineLink).Append("</engineLink><homePageLink>")
                .Append(HomePageLink).Append("</homePageLink>");
            Builder.Append(APIs.ToString());
            Builder.Append("</service></rsd>");
            return Builder.ToString();
        }

        #endregion
    }
}