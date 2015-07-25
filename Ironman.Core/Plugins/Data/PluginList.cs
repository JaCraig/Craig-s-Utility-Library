/*
Copyright (c) 2014 <a href="http://www.gutgames.com">James Craig</a>

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

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Serialization;
using Utilities.DataTypes;
using Utilities.IO;
using Utilities.IO.Logging.Enums;

namespace Ironman.Models.Plugins
{
    /// <summary>
    /// Plugin list
    /// </summary>
    [Serializable]
    public class PluginList
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PluginList" /> class.
        /// </summary>
        public PluginList()
            : base()
        {
            Plugins = new List<Plugin>();
        }

        /// <summary>
        /// Gets or sets the plugins.
        /// </summary>
        /// <value>The plugins.</value>
        public List<Plugin> Plugins { get; set; }

        /// <summary>
        /// Deletes this instance.
        /// </summary>
        public static void Delete()
        {
            try
            {
                string FileLocation = HttpContext.Current != null ? HttpContext.Current.Server.MapPath("~/App_Data/PluginList.txt") : AppDomain.CurrentDomain.BaseDirectory + "/App_Data/PluginList.txt";
                new System.IO.FileInfo(FileLocation).Delete();
            }
            catch { }
        }

        /// <summary>
        /// Loads this instance.
        /// </summary>
        /// <returns>The current plugin list</returns>
        public static PluginList Load()
        {
            string FileLocation = HttpContext.Current != null ? HttpContext.Current.Server.MapPath("~/App_Data/PluginList.txt") : AppDomain.CurrentDomain.BaseDirectory + "/App_Data/PluginList.txt";
            if (!new System.IO.FileInfo(FileLocation).Exists)
                return new PluginList();
            using (StreamReader Reader = new System.IO.FileInfo(FileLocation).OpenText())
            {
                return Deserialize(Reader.ReadToEnd());
            }
        }

        /// <summary>
        /// Adds the specified plugin.
        /// </summary>
        /// <param name="Plugin">The plugin.</param>
        public void Add(Plugin Plugin)
        {
            Plugins.Add(Plugin);
        }

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="ID">The identifier.</param>
        /// <returns></returns>
        public Plugin Get(string ID)
        {
            return Plugins.FirstOrDefault(x => string.Equals(x.PluginID, ID, StringComparison.InvariantCultureIgnoreCase));
        }

        /// <summary>
        /// Removes the specified plugin.
        /// </summary>
        /// <param name="Plugin">The plugin.</param>
        public void Remove(Plugin Plugin)
        {
            Plugins.Remove(Plugin);
        }

        /// <summary>
        /// Saves this instance.
        /// </summary>
        public void Save()
        {
            Plugins.ForEach(x => x.Save());
            string FileLocation = HttpContext.Current != null ? HttpContext.Current.Server.MapPath("~/App_Data/PluginList.txt") : AppDomain.CurrentDomain.BaseDirectory + "/App_Data/PluginList.txt";
            string DirectoryLocation = HttpContext.Current != null ? HttpContext.Current.Server.MapPath("~/App_Data/") : AppDomain.CurrentDomain.BaseDirectory + "/App_Data/";
            new System.IO.DirectoryInfo(DirectoryLocation).Create();
            using (FileStream Writer = new System.IO.FileInfo(FileLocation).Open(FileMode.Create, FileAccess.Write))
            {
                byte[] Content = Serialize(this).ToByteArray();
                Writer.Write(Content, 0, Content.Length);
            }
        }

        /// <summary>
        /// Deserializes the data
        /// </summary>
        /// <param name="Data">Data to deserialize</param>
        /// <returns>The deserialized data</returns>
        private static PluginList Deserialize(string Data)
        {
            if (string.IsNullOrEmpty(Data))
                return null;
            using (MemoryStream Stream = new MemoryStream(Encoding.UTF8.GetBytes(Data)))
            {
                XmlSerializer Serializer = new XmlSerializer(typeof(PluginList));
                return (PluginList)Serializer.Deserialize(Stream);
            }
        }

        /// <summary>
        /// Serializes the object
        /// </summary>
        /// <param name="Data">Data to serialize</param>
        /// <returns>The serialized data</returns>
        private static string Serialize(PluginList Data)
        {
            if (Data == null)
                return null;
            using (MemoryStream Stream = new MemoryStream())
            {
                XmlSerializer Serializer = new XmlSerializer(typeof(PluginList));
                Serializer.Serialize(Stream, Data);
                Stream.Flush();
                return Encoding.UTF8.GetString(Stream.GetBuffer(), 0, (int)Stream.Position);
            }
        }
    }
}