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
using System.Web;
using System.Web.Caching;
using Utilities.DataTypes.ExtensionMethods;
#endregion

namespace Utilities.Caching.ExtensionMethods
{
    /// <summary>
    /// Extension methods relating to caching of data
    /// </summary>
    public static class CachingExtensions
    {
        /// <summary>
        /// Caches an object to the specified cache, using the specified key
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="Object">Object to cache</param>
        /// <param name="Type">Caching type</param>
        /// <param name="Key">Key to cache the item under</param>
        public static void Cache<T>(this T Object, string Key, CacheType Type)
        {
            if (HttpContext.Current == null && !Type.HasFlag(CacheType.Internal))
                return;

            if (HttpContext.Current != null && Type.HasFlag(CacheType.Cache))
            {
                HttpContext.Current.Cache.Add(Key, Object, null,
                    System.Web.Caching.Cache.NoAbsoluteExpiration,
                    System.Web.Caching.Cache.NoSlidingExpiration,
                    CacheItemPriority.Normal, null);
            }
            else if (HttpContext.Current != null && Type.HasFlag(CacheType.Item))
            {
                HttpContext.Current.Items[Key] = Object;
            }
            else if (HttpContext.Current != null && Type.HasFlag(CacheType.Session))
            {
                HttpContext.Current.Session[Key] = Object;
            }
            else if (HttpContext.Current != null && Type.HasFlag(CacheType.Cookie))
            {
                HttpContext.Current.Response.Cookies.Add(new HttpCookie(Key, Object.ToString()));
            }
            else if (Type.HasFlag(CacheType.Internal))
            {
                new Cache<string>().Add(Key, Object);
            }
        }

        /// <summary>
        /// Gets the specified object from the cache if it exists, otherwise the default value is returned
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="Key">Key that the object is under</param>
        /// <param name="Type">Cache types to search</param>
        /// <param name="DefaultValue">Default value to return</param>
        /// <returns>The specified object if it exists, otherwise the default value</returns>
        public static T GetFromCache<T>(this string Key, CacheType Type, T DefaultValue = default(T))
        {
            if (HttpContext.Current == null && !Type.HasFlag(CacheType.Internal))
                return DefaultValue;

            if (HttpContext.Current != null && Type.HasFlag(CacheType.Cache))
            {
                return HttpContext.Current.Cache.Get(Key).To(DefaultValue);
            }
            if (HttpContext.Current != null && Type.HasFlag(CacheType.Item))
            {
                return HttpContext.Current.Items[Key].To(DefaultValue);
            }
            else if (HttpContext.Current != null && Type.HasFlag(CacheType.Session))
            {
                return HttpContext.Current.Session[Key].To(DefaultValue);
            }
            else if (HttpContext.Current != null && Type.HasFlag(CacheType.Cookie))
            {
                return HttpContext.Current.Response.Cookies[Key].Value.To(DefaultValue);
            }
            else if (Type.HasFlag(CacheType.Internal))
            {
                return new Cache<string>().Get<T>(Key);
            }
            return DefaultValue;
        }
    }

    /// <summary>
    /// Determines where an item is cached
    /// </summary>
    [Flags]
    public enum CacheType
    {
        /// <summary>
        /// Cache (ASP.Net only)
        /// </summary>
        Cache = 1,
        /// <summary>
        /// Item (ASP.Net only)
        /// </summary>
        Item = 2,
        /// <summary>
        /// Session (ASP.Net only)
        /// </summary>
        Session = 4,
        /// <summary>
        /// Cookie (ASP.Net only)
        /// </summary>
        Cookie = 8,
        /// <summary>
        /// Internal caching
        /// </summary>
        Internal = 16
    }
}