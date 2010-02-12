using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities.Web.OpenSearch
{
    public class OpenSearch
    {
        public OpenSearch() 
        {
            RestHelper = new Utilities.Web.REST.REST();
        }

        public string Search(string QueryString,string AdditionalInfo)
        {
            StringBuilder Builder=new StringBuilder();
            Builder.AppendFormat(APILocation, QueryString, AdditionalInfo);
            RestHelper.Url = new Uri(Builder.ToString());
            return RestHelper.GET();
        }

        protected REST.REST RestHelper { get; set; }
        protected string APILocation { get; set; }
    }
}
