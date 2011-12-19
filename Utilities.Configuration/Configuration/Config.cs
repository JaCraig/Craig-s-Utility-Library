/*
Copyright (c) 2011 <a href="http://www.gutgames.com">James Craig</a>

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
using System.Text;
using Utilities.Configuration.Interfaces;
using Utilities.IO.ExtensionMethods;
using Utilities.Encryption.ExtensionMethods;
using Utilities.Reflection.ExtensionMethods;
using System.Reflection;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.IO;
using Utilities.DataTypes.ExtensionMethods;
#endregion

namespace Utilities.Configuration
{
    /// <summary>
    /// Config object
    /// </summary>
    [Serializable()]
    public abstract class Config<ConfigClassType> : IConfig
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="StringToObject">String to object</param>
        /// <param name="ObjectToString">Object to string</param>
        public Config(Func<string, ConfigClassType> StringToObject = null, Func<IConfig, string> ObjectToString = null)
        {
            this.ObjectToString = ObjectToString.NullCheck((x) => x.ToXML(ConfigFileLocation));
            this.StringToObject = StringToObject.NullCheck((x) => (ConfigClassType)x.XMLToObject(this.GetType()));
        }

        #endregion

        #region Properties

        /// <summary>
        /// Location to save/load the config file from.
        /// If blank, it does not save/load but uses any defaults specified.
        /// </summary>
        protected virtual string ConfigFileLocation { get { return ""; } }

        /// <summary>
        /// Encryption password for properties/fields. Used only if set.
        /// </summary>
        protected virtual string EncryptionPassword { get { return ""; } }

        /// <summary>
        /// Gets the object
        /// </summary>
        private Func<string, ConfigClassType> StringToObject { get; set; }

        /// <summary>
        /// Gets a string representation of the object
        /// </summary>
        private Func<IConfig, string> ObjectToString { get; set; }

        /// <summary>
        /// Name of the config object
        /// </summary>
        public abstract string Name { get; }

        #endregion

        #region IConfig Members

        public void Load()
        {
            if (ConfigFileLocation.IsNullOrEmpty())
                return;
            string FileContent = new FileInfo(ConfigFileLocation).Read();
            if (string.IsNullOrEmpty(FileContent))
            {
                Save();
                return;
            }
            LoadProperties(StringToObject(FileContent));
            Decrypt();
        }

        public void Save()
        {
            if (ConfigFileLocation.IsNullOrEmpty())
                return;
            Encrypt();
            new FileInfo(ConfigFileLocation).Save(ObjectToString(this));
            Decrypt();
        }

        #endregion

        #region Private Functions

        private void LoadProperties(ConfigClassType Temp)
        {
            if (Temp.IsNull())
                return;
            foreach (PropertyInfo Property in Temp.GetType().GetProperties().Where(x => x.CanWrite && x.CanRead))
                this.SetProperty(Property, Temp.GetProperty(Property));
        }

        private void Encrypt()
        {
            if (EncryptionPassword.IsNullOrEmpty())
                return;
            foreach (PropertyInfo Property in this.GetType().GetProperties().Where(x => x.CanWrite && x.CanRead && x.PropertyType == typeof(string)))
                this.SetProperty(Property, ((string)this.GetProperty(Property)).Encrypt(EncryptionPassword));
        }

        private void Decrypt()
        {
            if (EncryptionPassword.IsNullOrEmpty())
                return;
            foreach (PropertyInfo Property in this.GetType().GetProperties().Where(x => x.CanWrite && x.CanRead && x.PropertyType == typeof(string)))
                this.SetProperty(Property, ((string)this.GetProperty(Property)).Decrypt(EncryptionPassword));
        }

        #endregion
    }
}