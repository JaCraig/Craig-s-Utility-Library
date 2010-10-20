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
using System.Collections.Generic;
using System.Xml;
#endregion

namespace Utilities.Web.Bing
{
    /// <summary>
    /// Bing helper class
    /// </summary>
    public class Bing : OpenSearch.OpenSearch
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public Bing()
            : base()
        {
            APILocation = "http://api.search.live.net/xml.aspx?query={0}&{1}";
            APPID = "";
        }

        #endregion

        #region Public Functions

        /// <summary>
        /// Checks the spelling of a word
        /// </summary>
        /// <param name="Item">Item to check</param>
        /// <returns>List of words that may be correct spellings</returns>
        public List<string> CheckSpelling(string Item)
        {
            List<string> ReturnList = new List<string>();
            string AdditionalInfo = "&sources={0}&appid={1}";
            AdditionalInfo = string.Format(AdditionalInfo, "spell", APPID);
            XmlDocument Doc = new XmlDocument();
            Doc.LoadXml(Search(Item, AdditionalInfo));
            XmlNamespaceManager NamespaceManager = new XmlNamespaceManager(Doc.NameTable);
            NamespaceManager.AddNamespace("api", "http://schemas.microsoft.com/LiveSearch/2008/04/XML/element");
            NamespaceManager.AddNamespace("spl", "http://schemas.microsoft.com/LiveSearch/2008/04/XML/spell");
            XmlNodeList Nodes = Doc.DocumentElement.SelectNodes("./spl:Spell/spl:Results/spl:SpellResult/spl:Value", NamespaceManager);
            foreach (XmlNode Element in Nodes)
            {
                ReturnList.Add(Element.InnerText);
            }
            return ReturnList;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// App ID
        /// </summary>
        public string APPID { get; set; }

        #endregion
    }
}