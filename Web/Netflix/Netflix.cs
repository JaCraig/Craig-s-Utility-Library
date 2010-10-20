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
using System.Xml;
using Utilities.IO;
using Utilities.Web.OAuth;

#endregion

namespace Utilities.Web.Netflix
{
    /// <summary>
    /// Helper class to be used with the Netflix API
    /// </summary>
    public class Netflix : OAuth.OAuth
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public Netflix()
            : base()
        {
            this.SignatureType = Signature.HMACSHA1;
        }

        #endregion

        #region Public Functions

        /// <summary>
        /// Calls netflix's auto complete function
        /// </summary>
        /// <param name="Term">Your search term</param>
        /// <returns>A list of possible matches</returns>
        public List<string> AutoComplete(string Term)
        {
            List<string> Results = new List<string>();
            string Content = FileManager.GetFileContents(new Uri("http://api.netflix.com/catalog/titles/autocomplete?oauth_consumer_key=" + ConsumerKey + "&term=" + Term));
            XmlDocument Document = new XmlDocument();
            Document.LoadXml(Content);
            foreach (XmlNode Children in Document.ChildNodes)
            {
                if (Children.Name.Equals("autocomplete", StringComparison.CurrentCultureIgnoreCase))
                {
                    foreach (XmlNode Child in Children.ChildNodes)
                    {
                        if (Child.Name.Equals("autocomplete_item", StringComparison.CurrentCultureIgnoreCase))
                        {

                            foreach (XmlNode Title in Child.ChildNodes)
                            {
                                if (Title.Name.Equals("title", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    Results.Add(Title.Attributes["short"].Value);
                                }
                            }
                        }
                    }
                }
            }
            return Results;
        }

        /// <summary>
        /// Title search
        /// </summary>
        /// <param name="Term">Term to search for</param>
        /// <param name="MaxResults">Max results to return</param>
        /// <param name="StartIndex">The starting index</param>
        /// <returns>List of title information</returns>
        public Titles TitleSearch(string Term, int MaxResults, int StartIndex)
        {
            AddParameter("term", Term);
            AddParameter("max_results", MaxResults.ToString());
            AddParameter("start_index", StartIndex.ToString());
            this.Token = "";
            this.TokenSecret = "";
            this.Method = HTTPMethod.GET;
            this.Url = new Uri("http://api.netflix.com/catalog/titles/");
            return new Titles(FileManager.GetFileContents(new Uri(GenerateRequest())));
        }

        /// <summary>
        /// Title synopsis
        /// </summary>
        /// <param name="Title">Title to search for</param>
        /// <returns>The synopsis info</returns>
        public string TitleSynopsis(Title Title)
        {
            this.Token = "";
            this.TokenSecret = "";
            this.Method = HTTPMethod.GET;
            this.Url = new Uri(Title.SynopsisLink);
            string Content = FileManager.GetFileContents(new Uri(GenerateRequest()));

            XmlDocument Document = new XmlDocument();
            Document.LoadXml(Content);
            foreach (XmlNode Children in Document.ChildNodes)
            {
                if (Children.Name.Equals("synopsis", StringComparison.CurrentCultureIgnoreCase))
                {
                    return Children.InnerText;
                }
            }

            return "";
        }

        /// <summary>
        /// Cast search
        /// </summary>
        /// <param name="Title">Title to search for</param>
        /// <returns>List of people information</returns>
        public People CastLookup(Title Title)
        {
            this.Token = "";
            this.TokenSecret = "";
            this.Method = HTTPMethod.GET;
            this.Url = new Uri(Title.CastLink);
            return new People(FileManager.GetFileContents(new Uri(GenerateRequest())));
        }

        /// <summary>
        /// Director search
        /// </summary>
        /// <param name="Title">Title to search for</param>
        /// <returns>List of people information</returns>
        public People DirectorLookup(Title Title)
        {
            this.Token = "";
            this.TokenSecret = "";
            this.Method = HTTPMethod.GET;
            this.Url = new Uri(Title.DirectorsLink);
            return new People(FileManager.GetFileContents(new Uri(GenerateRequest())));
        }

        /// <summary>
        /// Similar title search
        /// </summary>
        /// <param name="Title">Title to use as our search base</param>
        /// <returns>List of title information</returns>
        public Titles SimilarTitles(Title Title)
        {
            this.Token = "";
            this.TokenSecret = "";
            this.Method = HTTPMethod.GET;
            this.Url = new Uri(Title.SimilarTitleLink);
            return new Titles(FileManager.GetFileContents(new Uri(GenerateRequest())));
        }

        /// <summary>
        /// Formats available for the title
        /// </summary>
        /// <param name="Title">Title to use as our search base</param>
        /// <returns>List of available formats</returns>
        public List<string> FormatsAvailable(Title Title)
        {
            this.Token = "";
            this.TokenSecret = "";
            this.Method = HTTPMethod.GET;
            this.Url = new Uri(Title.FormatsAvailableLink);
            string Content = FileManager.GetFileContents(new Uri(GenerateRequest()));
            List<string> Results = new List<string>();
            if (!string.IsNullOrEmpty(Content))
            {
                XmlDocument Document = new XmlDocument();
                Document.LoadXml(Content);
                foreach (XmlNode Children in Document.ChildNodes)
                {
                    if (Children.Name.Equals("delivery_formats", StringComparison.CurrentCultureIgnoreCase))
                    {
                        foreach (XmlNode Child in Children.ChildNodes)
                        {
                            if (Child.Name.Equals("availability", StringComparison.CurrentCultureIgnoreCase))
                            {
                                foreach (XmlNode Category in Child.ChildNodes)
                                {
                                    if (Category.Name.Equals("category", StringComparison.CurrentCultureIgnoreCase))
                                    {
                                        Results.Add(Category.Attributes["term"].Value);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return Results;
        }

        #endregion
    }
}