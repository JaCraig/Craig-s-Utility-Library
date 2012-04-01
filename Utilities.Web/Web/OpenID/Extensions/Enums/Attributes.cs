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

#endregion

namespace Utilities.Web.OpenID.Extensions.Enums
{
    /// <summary>
    /// Attribute exchange attributes (plus ID, which is used for returning the ID to the user)
    /// </summary>
    [Flags]
    public enum Attributes
    {
        /// <summary>
        /// None
        /// </summary>
        None=0,
        /// <summary>
        /// Email
        /// </summary>
        Email=1,
        /// <summary>
        /// User name
        /// </summary>
        UserName=2,
        /// <summary>
        /// Full name
        /// </summary>
        FullName=4,
        /// <summary>
        /// First name
        /// </summary>
        FirstName=8,
        /// <summary>
        /// Last name
        /// </summary>
        LastName=16,
        /// <summary>
        /// Company name
        /// </summary>
        CompanyName=32,
        /// <summary>
        /// Job title
        /// </summary>
        JobTitle=64,
        /// <summary>
        /// Birth date
        /// </summary>
        BirthDate=128,
        /// <summary>
        /// Phone number
        /// </summary>
        Phone=256,
        /// <summary>
        /// Gender
        /// </summary>
        Gender=512,
        /// <summary>
        /// Address
        /// </summary>
        Address=1024,
        /// <summary>
        /// Postal code
        /// </summary>
        PostalCode=2048,
        /// <summary>
        /// Country
        /// </summary>
        Country=4096,
        /// <summary>
        /// Language
        /// </summary>
        Language=8192,
        /// <summary>
        /// Time zone
        /// </summary>
        TimeZone=16384,
        /// <summary>
        /// ID
        /// </summary>
        ID=32768
    }
}
