using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;
using SEOAssistant.Core;


namespace SEOAssistant.SearchEngine
{
    public class DuckDuckGoSearchEngine : SearchEngineBase, ISearchEngine
    {
        const string searchEngineName = "DuckDuckGo";
        const string baseURL = "https://www.DuckDuckGo.com";
        const string searchItemXPath = "//h2/a";

        public List<ResultItem> ResultItems { get; set; }
        public string Name => searchEngineName;

        public DuckDuckGoSearchEngine() : base(baseURL, searchItemXPath, searchEngineName)
        {
            ResultItems = new List<ResultItem>();
        }

        public void Process(string searchText, string linkToSearch)
        {
            string queryString = "/search?first={0}&q=" + searchText.Replace("  ", " ").Replace(" ", "+");
            //this.ResultItems = base.GetResults(queryString, linkToSearch);
        }

    }
}