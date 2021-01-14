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
    public class BingSearchEngine : SearchEngineBase, ISearchEngine
    {
        const string searchEngineName = "Bing";
        const string baseURL = "https://www.bing.com";
        const string searchItemXPath = "//h2/a";

        public List<ResultItem> ResultItems { get; set; }
        public string Name => searchEngineName;

        public BingSearchEngine() : base(baseURL, searchItemXPath, searchEngineName)
        {
            ResultItems = new List<ResultItem>();
        }

        public void Process(string searchText, string linkToSearch)
        {
            string queryString = "/search?first={0}&q=" + searchText.Replace("  ", " ").Replace(" ", "+");
            //this.ResultItems = base.GetResults(queryString, linkToSearch);
        }

        public override string CleanHTML(string rawHTML) {
            rawHTML = base.CleanHTML(rawHTML);

            var htmlHelper = new HTMLHelper();

            rawHTML = htmlHelper.RemoveContentBetweenStrings("<!--", "-->", rawHTML);//remove comments
            rawHTML = htmlHelper.RemoveWord("&nbsp;", rawHTML);

            rawHTML = htmlHelper.RemoveWord("<div class=\"b_sideBleed b_topBleed b_bottomBleed\">", rawHTML);
            rawHTML = htmlHelper.RemoveWord("<div id=\"placeAnswer\" class=\"np_answer compact_overlay\" data-tag=\"destination_ctr\">", rawHTML);

            return rawHTML;
        }

    }
}