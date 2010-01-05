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
#endregion

namespace Utilities.Web.Netflix
{
    /// <summary>
    /// Title information
    /// </summary>
    public class Title
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Element">Title information</param>
        public Title(XmlElement Element)
        {
            Categories = new List<string>();
            foreach (XmlNode Children in Element.ChildNodes)
            {
                if (Children.Name.Equals("id", StringComparison.CurrentCultureIgnoreCase))
                {
                    ID = Children.InnerText;
                }
                else if (Children.Name.Equals("title", StringComparison.CurrentCultureIgnoreCase))
                {
                    TitleInformation = new TitleInformation((XmlElement)Children);
                }
                else if (Children.Name.Equals("box_art", StringComparison.CurrentCultureIgnoreCase))
                {
                    BoxArt = new BoxArt((XmlElement)Children);
                }
                else if (Children.Name.Equals("release_year", StringComparison.CurrentCultureIgnoreCase))
                {
                    ReleaseYear = Children.InnerText;
                }
                else if (Children.Name.Equals("runtime", StringComparison.CurrentCultureIgnoreCase))
                {
                    RunTime = int.Parse(Children.InnerText);
                }
                else if (Children.Name.Equals("average_rating", StringComparison.CurrentCultureIgnoreCase))
                {
                    AverageRating = double.Parse(Children.InnerText);
                }

                else if (Children.Name.Equals("category", StringComparison.CurrentCultureIgnoreCase))
                {
                    if (Children.Attributes["scheme"] != null
                        && (Children.Attributes["scheme"].Value.Equals("http://api.netflix.com/categories/mpaa_ratings",
                            StringComparison.CurrentCultureIgnoreCase)
                            || Children.Attributes["scheme"].Value.Equals("http://api.netflix.com/categories/tv_ratingss",
                            StringComparison.CurrentCultureIgnoreCase)))
                    {
                        Rating = Children.Attributes["label"].Value;
                    }
                    else if (Children.Attributes["scheme"].Value.Equals("http://api.netflix.com/categories/genres",
                        StringComparison.CurrentCultureIgnoreCase))
                    {
                        Categories.Add(Children.Attributes["label"].Value);
                    }
                }
                else if (Children.Name.Equals("link", StringComparison.CurrentCultureIgnoreCase))
                {
                    if (Children.Attributes["rel"] != null
                        && Children.Attributes["rel"].Value.Equals("http://schemas.netflix.com/catalog/titles/synopsis",
                            StringComparison.CurrentCultureIgnoreCase))
                    {
                        SynopsisLink = Children.Attributes["href"].Value;
                    }
                    else if (Children.Attributes["rel"] != null
                        && Children.Attributes["rel"].Value.Equals("http://schemas.netflix.com/catalog/people.cast",
                            StringComparison.CurrentCultureIgnoreCase))
                    {
                        CastLink = Children.Attributes["href"].Value;
                    }
                    else if (Children.Attributes["rel"] != null
                        && Children.Attributes["rel"].Value.Equals("http://schemas.netflix.com/catalog/people.directors",
                            StringComparison.CurrentCultureIgnoreCase))
                    {
                        DirectorsLink = Children.Attributes["href"].Value;
                    }
                    else if (Children.Attributes["rel"] != null
                        && Children.Attributes["rel"].Value.Equals("http://schemas.netflix.com/catalog/titles/format_availability",
                            StringComparison.CurrentCultureIgnoreCase))
                    {
                        FormatsAvailableLink = Children.Attributes["href"].Value;
                    }
                    else if (Children.Attributes["rel"] != null
                        && Children.Attributes["rel"].Value.Equals("http://schemas.netflix.com/catalog/titles/screen_formats",
                            StringComparison.CurrentCultureIgnoreCase))
                    {
                        ScreenFormatsLink = Children.Attributes["href"].Value;
                    }
                    else if (Children.Attributes["rel"] != null
                        && Children.Attributes["rel"].Value.Equals("http://schemas.netflix.com/catalog/titles/languages_and_audio",
                            StringComparison.CurrentCultureIgnoreCase))
                    {
                        LanguagesLink = Children.Attributes["href"].Value;
                    }
                    else if (Children.Attributes["rel"] != null
                        && Children.Attributes["rel"].Value.Equals("http://schemas.netflix.com/catalog/titles.similars",
                            StringComparison.CurrentCultureIgnoreCase))
                    {
                        SimilarTitleLink = Children.Attributes["href"].Value;
                    }
                    else if (Children.Attributes["rel"] != null
                        && Children.Attributes["rel"].Value.Equals("alternate",
                            StringComparison.CurrentCultureIgnoreCase))
                    {
                         this.NetflixWebPage= Children.Attributes["href"].Value;
                    }
                    else if (Children.Attributes["rel"] != null
                        && Children.Attributes["rel"].Value.Equals("http://schemas.netflix.com/catalog/titles.series",
                            StringComparison.CurrentCultureIgnoreCase))
                    {
                        this.SeriesLink = Children.Attributes["href"].Value;
                    }
                    else if (Children.Attributes["rel"] != null
                        && Children.Attributes["rel"].Value.Equals("http://schemas.netflix.com/catalog/programs",
                            StringComparison.CurrentCultureIgnoreCase))
                    {
                        this.EpisodeLink = Children.Attributes["href"].Value;
                    }
                    else if (Children.Attributes["rel"] != null
                        && Children.Attributes["rel"].Value.Equals("http://schemas.netflix.com/catalog/titles/official_url",
                            StringComparison.CurrentCultureIgnoreCase))
                    {
                        this.StudioWebPage = Children.Attributes["href"].Value;
                    }
                }
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Title ID
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// Title information
        /// </summary>
        public TitleInformation TitleInformation { get; set; }

        /// <summary>
        /// Box artwork
        /// </summary>
        public BoxArt BoxArt { get; set; }

        /// <summary>
        /// Synopsis link
        /// </summary>
        public string SynopsisLink { get; set; }

        /// <summary>
        /// Year released
        /// </summary>
        public string ReleaseYear { get; set; }

        /// <summary>
        /// Average rating
        /// </summary>
        public string Rating { get; set; }

        /// <summary>
        /// list of categories
        /// </summary>
        public List<string> Categories { get; set; }

        /// <summary>
        /// Run time in seconds
        /// </summary>
        public int RunTime { get; set; }

        /// <summary>
        /// Link to cast information
        /// </summary>
        public string CastLink { get; set; }

        /// <summary>
        /// Link to directories information
        /// </summary>
        public string DirectorsLink { get; set; }

        /// <summary>
        /// Formats available link
        /// </summary>
        public string FormatsAvailableLink { get; set; }

        /// <summary>
        /// Screen formats available link
        /// </summary>
        public string ScreenFormatsLink { get; set; }

        /// <summary>
        /// Language information link
        /// </summary>
        public string LanguagesLink { get; set; }

        /// <summary>
        /// Average rating
        /// </summary>
        public double AverageRating { get; set; }

        /// <summary>
        /// Similar titles to this one link
        /// </summary>
        public string SimilarTitleLink { get; set; }

        /// <summary>
        /// Link to netflix webpage
        /// </summary>
        public string NetflixWebPage { get; set; }

        /// <summary>
        /// Link to studio web page
        /// </summary>
        public string StudioWebPage { get; set; }

        /// <summary>
        /// Link to awards
        /// </summary>
        public string Awards { get; set; }

        /// <summary>
        /// Link to bonus material
        /// </summary>
        public string BonusMaterial { get; set; }

        /// <summary>
        /// Link for series info
        /// </summary>
        public string SeriesLink { get; set; }

        /// <summary>
        /// Link for episodes
        /// </summary>
        public string EpisodeLink { get; set; }

        /// <summary>
        /// Season info link
        /// </summary>
        public string SeasonLink { get; set; }

        #endregion
    }
}