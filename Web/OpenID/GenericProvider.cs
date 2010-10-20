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
using System.Linq;
using Utilities.DataTypes;
using Utilities.Web.OpenID.Extensions;
using Utilities.Web.OpenID.Extensions.Enums;

#endregion

namespace Utilities.Web.OpenID
{
    /// <summary>
    /// Generic OpenID provider
    /// </summary>
    public class GenericProvider : OpenID
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public GenericProvider()
            : base()
        {
        }

        #endregion

        #region Functions

        /// <summary>
        /// Generates login URL
        /// </summary>
        /// <returns>The login URL based on request</returns>
        public string GenerateLoginURL(string Redirect, string Server, Attributes Required)
        {
            this.RedirectURL = Redirect;
            this.ServerURL = Server;
            foreach (AttributeExchange Extension in Extensions.OfType<AttributeExchange>())
            {
                Extension.Required = Required;
            }
            return GenerateLoginURL();
        }

        /// <summary>
        /// Gets a list of attributes based on what was requested
        /// </summary>
        /// <param name="URL">The response URL from the provider</param>
        /// <param name="Required">Attributes that are required</param>
        /// <returns>A list of attributes based on what was requested or an exception if the login failed</returns>
        public Dictionary<Attributes, string> GetRequestedAttributes(string URL, Attributes Required)
        {
            foreach (AttributeExchange Extension in Extensions.OfType<AttributeExchange>())
            {
                Extension.Required = Required;
            }
            System.Collections.Generic.List<Pair<string, string>> AttributesList = this.GetAttributes(URL);
            if (AttributesList == null)
                throw new Exception("The information requested was not received");
            Dictionary<Attributes, string> FinalValues = new Dictionary<Attributes, string>();
            foreach (AttributeExchange Extension in Extensions.OfType<AttributeExchange>())
            {
                FinalValues = Extension.GetValues(AttributesList);
            }
            Pair<string, string> ID = AttributesList.Find(x => x.Left.Equals("openid.claimed_id", StringComparison.CurrentCultureIgnoreCase));
            FinalValues.Add(Attributes.ID, ID.Right);
            return FinalValues;
        }

        #endregion
    }
}