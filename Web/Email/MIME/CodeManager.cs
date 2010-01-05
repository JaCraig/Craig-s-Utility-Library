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

#endregion

namespace Utilities.Web.Email.MIME
{
    /// <summary>
    /// Manager in charge of the various decode/encode classes.
    /// Is a singleton.
    /// </summary>
    public class CodeManager
    {
        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        private CodeManager()
        {
            Load();
        }
        #endregion

        #region Instance of the class
        private static CodeManager _Instance = null;
        private Dictionary<string, Code> Codes = new Dictionary<string, Code>();

        /// <summary>
        /// Instance of the class
        /// </summary>
        public static CodeManager Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new CodeManager();
                }
                return _Instance;
            }
        }
        #endregion

        #region Private Functions
        /// <summary>
        /// Loads the basic information for the class
        /// </summary>
        private void Load()
        {
            Code Field = new CodeTypes.CodeBase();
            this["Subject"]= Field;
            this["Comments"]= Field;
            this["Content-Description"]= Field;

            Field = new CodeTypes.CodeAddress();
            this["From"]= Field;
            this["To"]= Field;
            this["Resent-To"]= Field;
            this["Cc"]= Field;
            this["Resent-Cc"]= Field;
            this["Bcc"]= Field;
            this["Resent-Bcc"]= Field;
            this["Reply-To"]= Field;
            this["Resent-Reply-To"]= Field;

            Field = new CodeTypes.CodeParameter();
            this["Content-Type"]= Field;
            this["Content-Disposition"]= Field;

            Field = new Code();
            this["7bit"]= Field;
            this["8bit"]= Field;

            Field = new CodeTypes.CodeBase64();
            this["base64"]= Field;

            Field = new CodeTypes.CodeQP();
            this["quoted-printable"]= Field;
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets the coder assocaited with the type specified
        /// </summary>
        /// <param name="Key">Content type</param>
        /// <returns>The coder associated with the type</returns>
        public Code this[string Key]
        {
            get
            {
                Key = Key.ToLower();
                if (Codes.ContainsKey(Key))
                {
                    return Codes[Key];
                }
                else
                {
                    return null;
                }
            }
            set
            {
                Key = Key.ToLower();
                if (Codes.ContainsKey(Key))
                {
                    Codes[Key] = value;
                }
                else
                {
                    Codes.Add(Key, value);
                }
            }
        }
        #endregion
    }
}