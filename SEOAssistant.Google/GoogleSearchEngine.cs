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
    public class GoogleSearchEngine : SearchEngineBase, ISearchEngine
    {
        const string searchEngineName = "Google";
        const string baseURL = "https://www.google.com";
        const string searchItemXPath = "//h3/parent::a";

        public List<ResultItem> ResultItems { get; set; }
        public string Name => searchEngineName;

        public GoogleSearchEngine() : base(baseURL, searchItemXPath, searchEngineName)
        {            
            ResultItems = new List<ResultItem>();
        }

        public void Process(string searchText, string linkToSearch)
        {
            string queryString = "/search?client=firefox-b-d&start={0}&q=" + searchText.Replace("  ", " ").Replace(" ", "+");
            this.ResultItems = base.GetResults(queryString, linkToSearch);
        }
    }
}