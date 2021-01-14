using Microsoft.VisualStudio.TestTools.UnitTesting;
using SEOAssistant.Core;
using SEOAssistant.SearchEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SEOAssistant.Tests
{
    [TestClass]
    public class SearchEngineTest
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void EnsureThatExceptionIsThrownIfPlaceholderNotSuppliedForGetResultsInSearchEngineBase() {
            GoogleSearchEngine searchEngine = new GoogleSearchEngine();
            string queryString = "/search?client=firefox-b-d&q=google";

            List<ResultItem> results = new List<ResultItem>();
            Assert.ThrowsException<FormatException>(() => results = searchEngine.GetResults(queryString, "stackoverflow.com"));
        }

        [TestMethod]
        public void EnsureThatPopulateItemsExitsWhenResultsExceedMaxSpecifiedValue() {
            GoogleSearchEngine searchEngine = new GoogleSearchEngine();
            List<ResultItem> results = new List<ResultItem>();
            string file = Path.Combine(TestContext.DeploymentDirectory, "TestData", "GoogleSearchResultPage.xml");
            string body = File.ReadAllText(file);

            for (int i = 1; i <= 100; i++) {
                results.Add(new ResultItem() { Content = "https://google.com", Index = i, IsMatch = false, Source = "Google" });
            }

            Assert.IsFalse(searchEngine.PopulateItems("stackoverflow.com", body, results, true));
        }

        [TestMethod]
        public void EnsureFindURLsMethodPopulatesDataFromHrefAttribute() {
            GoogleSearchEngine searchEngine = new GoogleSearchEngine();
            string file = Path.Combine(TestContext.DeploymentDirectory, "TestData", "BingSearchResultPage.xml");
            string body = File.ReadAllText(file);

            List<string> results = searchEngine.FindURLs(body, "//h3/parent::a");

            Assert.IsTrue(results.Count > 0);
        }
    }
}
