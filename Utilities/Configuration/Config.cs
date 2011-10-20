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
            if (StringToObject == null)
                StringToObject = (x) => (ConfigClassType)x.XMLToObject(this.GetType());
            if (ObjectToString == null)
                ObjectToString = (x) => x.ToXML(ConfigFileLocation);
            this.ObjectToString = ObjectToString;
            this.StringToObject = StringToObject;
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
            if (string.IsNullOrEmpty(ConfigFileLocation))
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
            if (string.IsNullOrEmpty(ConfigFileLocation))
                return;
            Encrypt();
            new FileInfo(ConfigFileLocation).Save(ObjectToString(this));
            Decrypt();
        }

        #endregion

        #region Private Functions

        private void LoadProperties(ConfigClassType Temp)
        {
            if (Temp == null)
                return;
            foreach (PropertyInfo Property in Temp.GetType().GetProperties())
                if (Property.CanWrite && Property.CanRead)
                    this.SetProperty(Property, Temp.GetProperty(Property));
        }

        private void Encrypt()
        {
            if (string.IsNullOrEmpty(EncryptionPassword))
                return;
            foreach (PropertyInfo Property in this.GetType().GetProperties())
                if (Property.CanWrite && Property.CanRead && Property.PropertyType == typeof(string))
                    this.SetProperty(Property, ((string)this.GetProperty(Property)).Encrypt(EncryptionPassword));
        }

        private void Decrypt()
        {
            if (string.IsNullOrEmpty(EncryptionPassword))
                return;
            foreach (PropertyInfo Property in this.GetType().GetProperties())
                if (Property.CanWrite && Property.CanRead && Property.PropertyType == typeof(string))
                    this.SetProperty(Property, ((string)this.GetProperty(Property)).Decrypt(EncryptionPassword));
        }

        #endregion
    }
}