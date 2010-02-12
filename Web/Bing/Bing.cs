using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Utilities.Web.Bing
{
    public class Bing:OpenSearch.OpenSearch
    {
        public Bing()
            : base()
        {
            APILocation = "http://api.search.live.net/xml.aspx?query={0}&{1}";
            APPID = "";
        }

        public List<string> CheckSpelling(string Item)
        {
            List<string> ReturnList=new List<string>();
            string AdditionalInfo="&sources={0}&appid={1}";
            AdditionalInfo=string.Format(AdditionalInfo,"spell",APPID);
            XmlDocument Doc = new XmlDocument();
            Doc.LoadXml(Search(Item, AdditionalInfo));
            XmlNamespaceManager NamespaceManager = new XmlNamespaceManager(Doc.NameTable);
            NamespaceManager.AddNamespace("api", "http://schemas.microsoft.com/LiveSearch/2008/04/XML/element");
            NamespaceManager.AddNamespace("spl", "http://schemas.microsoft.com/LiveSearch/2008/04/XML/spell");
            XmlNodeList Nodes = Doc.DocumentElement.SelectNodes("./spl:Spell/spl:Results/spl:SpellResult/spl:Value", NamespaceManager);
            foreach (XmlNode Element in Nodes)
            {
                ReturnList.Add(Element.InnerText);
            }
            return ReturnList;
        }

        public string APPID { get; set; }
    }
}