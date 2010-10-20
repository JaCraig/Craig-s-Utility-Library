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

#endregion

namespace Utilities.Web.OpenID.Extensions.Enums
{
    /// <summary>
    /// Attribute exchange attributes (plus ID, which is used for returning the ID to the user)
    /// </summary>
    [Flags]
    public enum Attributes
    {
        None=0,
        Email=1,
        UserName=2,
        FullName=4,
        FirstName=8,
        LastName=16,
        CompanyName=32,
        JobTitle=64,
        BirthDate=128,
        Phone=256,
        Gender=512,
        Address=1024,
        PostalCode=2048,
        Country=4096,
        Language=8192,
        TimeZone=16384,
        ID=32768
    }
}
