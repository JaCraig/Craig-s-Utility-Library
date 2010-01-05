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
using System.Web;
using Utilities.FileFormats.RSSHelper;
#endregion

namespace Utilities.Web.Craigslist
{
    /// <summary>
    /// Craigslist helper
    /// </summary>
    public static class Craigslist
    {
        #region Public Static Functions

        /// <summary>
        /// Searches craigslist
        /// </summary>
        /// <param name="Site">Site to search (for instance http://charlottesville.craigslist.org/ for Charlottesville, VA)</param>
        /// <param name="Category">Category to search within</param>
        /// <param name="SearchString">Search term</param>
        /// <returns>RSS feed object</returns>
        public static Document Search(string Site, Category Category, string SearchString)
        {
            return new Document(Site + "/search/" + Names[(int)Category] + "?query=" + HttpUtility.UrlEncode(SearchString) + "&catAbbreviation=" + Names[(int)Category] + "&minAsk=min&maxAsk=max&format=rss");
        }

        #endregion

        #region Private Static Variables

        /// <summary>
        /// Abbreviations for categories that Craigslist uses
        /// </summary>
        private static string[] Names ={"sss","art","pts","bab","bar","bik","boa","bks","bfs","cta","ctd",
            "cto","emd","clo","clt","sys","ele","grd","zip","fua","fud","fuo","tag","gms","for","hsh",
            "wan","jwl","mat","mcy","msg","pho","rvs","spo","tix","tls"};

        #endregion
    }

    #region Enum

    /// <summary>
    /// Category to search within
    /// </summary>
    public enum Category
    {
        All_For_Sale,
        For_Sale_Arts_Crafts,
        For_Sale_Auto_Parts,
        For_Sale_Baby_Kid_Stuff,
        For_Sale_Barter,
        For_Sale_Bicycles,
        For_Sale_Boats,
        For_Sale_Books,
        For_Sale_Business,
        For_Sale_Cars_And_Trucks_All,
        For_Sale_Cars_And_Trucks_Dealer,
        For_Sale_Cars_And_Trucks_Owner,
        For_Sale_CDs_DVDs_VHS,
        For_Sale_Clothing,
        For_Sale_Collectibles,
        For_Sale_Computers_Tech,
        For_Sale_Electronics,
        For_Sale_Farm_Garden,
        For_Sale_Free_Stuff,
        For_Sale_Furniture_All,
        For_Sale_Furniture_By_Dealer,
        For_Sale_Furniture_By_Owner,
        For_Sale_Games_Toys,
        For_Sale_Garage_Sales,
        For_Sale_General,
        For_Sale_Household,
        For_Sale_Items_Wanted,
        For_Sale_Jewelry,
        For_Sale_Materials,
        For_Sale_Motorcycles,
        For_Sale_Musical_Instruments,
        For_Sale_Photo_Video,
        For_Sale_Recreational_Vehicles,
        For_Sale_Sporting_Goods,
        For_Sale_Tickets,
        For_Sale_Tools
    }

    #endregion
}