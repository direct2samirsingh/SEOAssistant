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
    public class SearchEngineBase
    {
        private Uri _baseURI { get; init; }
        private string _searchEngineName { get; init; }
        private string _searchItemXPath { get; init; }

        public SearchEngineBase(string baseUrl, string searchItemXPath, string searchEngineName) {
            _baseURI = new Uri(baseUrl);
            _searchEngineName = searchEngineName;
            _searchItemXPath = searchItemXPath;
        }

        public List<ResultItem> GetResults(string queryString, string linkToSearch)
        {
            if (!queryString.Contains("{0}"))
            { 
                //e.g. /search?first={0}&q=
                throw new FormatException("query string does not contain place holder for pageindex");
            }
            List<ResultItem> resultItems = new List<ResultItem>();
            int pageIndex = 0;
            bool continueSearch = true;

            string body = GetNextPage(queryString, pageIndex);

            do
            {
                continueSearch = PopulateItems(linkToSearch, body, resultItems, continueSearch);

                if (continueSearch == true && pageIndex < 10)
                {
                    pageIndex++;
                    body = GetNextPage(queryString, pageIndex);
                }
                else
                {
                    continueSearch = false;
                }
            }
            while (continueSearch);

            return resultItems;
        }

        public string GetNextPage(string queryString, int pageIndex)
        {            
            Uri? urlAddress = new Uri(this._baseURI, string.Format(queryString, pageIndex * 10));
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlAddress);

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse()) {
                if (response.StatusCode == HttpStatusCode.OK) {
                    Stream receiveStream = response.GetResponseStream();
                    StreamReader readStream = response.CharacterSet == null ? new StreamReader(receiveStream)
                        : new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));

                    string htmldata = readStream.ReadToEnd();
                    string bodyTagAsXML = CleanHTML(htmldata);

                    response.Close();
                    readStream.Close();

                    return bodyTagAsXML;
                }
            }
            return string.Empty;
        }

        public virtual string CleanHTML(string rawHTML) {
            var htmlHelper = new HTMLHelper();

            var startIndex = rawHTML.IndexOf("<body");
            var endIndex = rawHTML.LastIndexOf("</body>") + "</body>".Length;

            rawHTML = rawHTML.Substring(startIndex, endIndex - startIndex);

            rawHTML = htmlHelper.RemoveTagPair("script", rawHTML);
            rawHTML = htmlHelper.RemoveTagPair("style", rawHTML);
            rawHTML = htmlHelper.RemoveTagPair("svg", rawHTML);

            rawHTML = htmlHelper.RemoveSinglularTag("input", rawHTML);
            rawHTML = htmlHelper.RemoveSinglularTag("meta", rawHTML); rawHTML = htmlHelper.RemoveSinglularTag("img", rawHTML);
            rawHTML = htmlHelper.RemoveSinglularTag("br", rawHTML); rawHTML = htmlHelper.RemoveSinglularTag("hr", rawHTML);
            rawHTML = htmlHelper.RemoveSinglularTag("etc", rawHTML);


            return rawHTML;
        }

        public bool PopulateItems(string linkToSearch, string body,
            List<ResultItem> resultItems, bool continueSearch)
        {
            foreach (var url in FindURLs(body, _searchItemXPath))
            {
                resultItems.Add(new ResultItem()
                {
                    Content = new Uri(_baseURI, url).AbsoluteUri,
                    Source = _searchEngineName,
                    IsMatch = url.Contains(linkToSearch, StringComparison.OrdinalIgnoreCase),
                    Index = resultItems.Count() + 1
                });

                if (resultItems.Count() >= 100)
                {
                    continueSearch = false;
                    break;
                }
            }

            return continueSearch;
        }

        public List<string> FindURLs(string html, string xpath) {
            List<string> links = new List<string>();

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.DtdProcessing = DtdProcessing.Ignore;
            settings.IgnoreWhitespace = true;
            settings.CheckCharacters = false;
            settings.IgnoreComments = true;
            settings.ValidationType = ValidationType.None;
            settings.ValidationFlags = System.Xml.Schema.XmlSchemaValidationFlags.None;

            XmlReader xmlReader = XmlReader.Create(new StringReader(html), settings);
            XmlDocument document = new XmlDocument();
            document.Load(xmlReader);

            XmlNodeList? nodeList = document.SelectNodes(xpath);

            if (nodeList != null) {
                foreach (XmlNode node in nodeList) {
                    if (node?.Attributes?["href"] != null) {
                        links.Add(node.Attributes["href"].Value);
                    }
                }
            }

            return links;


        }
    }
}