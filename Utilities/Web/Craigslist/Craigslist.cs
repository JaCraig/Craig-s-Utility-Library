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
        /// <summary>
        /// All for sale
        /// </summary>
        All_For_Sale,
        /// <summary>
        /// For sale arts and crafts
        /// </summary>
        For_Sale_Arts_Crafts,
        /// <summary>
        /// For sale auto parts
        /// </summary>
        For_Sale_Auto_Parts,
        /// <summary>
        /// For sale baby kid stuff
        /// </summary>
        For_Sale_Baby_Kid_Stuff,
        /// <summary>
        /// for sale barter
        /// </summary>
        For_Sale_Barter,
        /// <summary>
        /// for sale bicycles
        /// </summary>
        For_Sale_Bicycles,
        /// <summary>
        /// for sale boats
        /// </summary>
        For_Sale_Boats,
        /// <summary>
        /// for sale books
        /// </summary>
        For_Sale_Books,
        /// <summary>
        /// for sale business
        /// </summary>
        For_Sale_Business,
        /// <summary>
        /// for sale cars and trucks all
        /// </summary>
        For_Sale_Cars_And_Trucks_All,
        /// <summary>
        /// for sale cars and trucks dealer
        /// </summary>
        For_Sale_Cars_And_Trucks_Dealer,
        /// <summary>
        /// for sale cars and trucks owner
        /// </summary>
        For_Sale_Cars_And_Trucks_Owner,
        /// <summary>
        /// for sale CDs, DVDs, VHS
        /// </summary>
        For_Sale_CDs_DVDs_VHS,
        /// <summary>
        /// for sale clothing
        /// </summary>
        For_Sale_Clothing,
        /// <summary>
        /// for sale collectibles
        /// </summary>
        For_Sale_Collectibles,
        /// <summary>
        /// for sale computer tech
        /// </summary>
        For_Sale_Computers_Tech,
        /// <summary>
        /// for sale electronics
        /// </summary>
        For_Sale_Electronics,
        /// <summary>
        /// for sale farm garden
        /// </summary>
        For_Sale_Farm_Garden,
        /// <summary>
        /// for sale free stuff
        /// </summary>
        For_Sale_Free_Stuff,
        /// <summary>
        /// for sale furniture all
        /// </summary>
        For_Sale_Furniture_All,
        /// <summary>
        /// for sale furniture by dealer
        /// </summary>
        For_Sale_Furniture_By_Dealer,
        /// <summary>
        /// for sale furniture by owner
        /// </summary>
        For_Sale_Furniture_By_Owner,
        /// <summary>
        /// for sale games toys
        /// </summary>
        For_Sale_Games_Toys,
        /// <summary>
        /// for sale garage sales
        /// </summary>
        For_Sale_Garage_Sales,
        /// <summary>
        /// for sale general
        /// </summary>
        For_Sale_General,
        /// <summary>
        /// for sale household
        /// </summary>
        For_Sale_Household,
        /// <summary>
        /// for sale items wanted
        /// </summary>
        For_Sale_Items_Wanted,
        /// <summary>
        /// for sale jewelry
        /// </summary>
        For_Sale_Jewelry,
        /// <summary>
        /// for sale materials
        /// </summary>
        For_Sale_Materials,
        /// <summary>
        /// for sale motorcycles
        /// </summary>
        For_Sale_Motorcycles,
        /// <summary>
        /// for sale musical instruments
        /// </summary>
        For_Sale_Musical_Instruments,
        /// <summary>
        /// for sale photo/video
        /// </summary>
        For_Sale_Photo_Video,
        /// <summary>
        /// for sale recreational vehicles
        /// </summary>
        For_Sale_Recreational_Vehicles,
        /// <summary>
        /// for sale sporting goods
        /// </summary>
        For_Sale_Sporting_Goods,
        /// <summary>
        /// for sale tickets
        /// </summary>
        For_Sale_Tickets,
        /// <summary>
        /// for sale tools
        /// </summary>
        For_Sale_Tools
    }

    #endregion
}