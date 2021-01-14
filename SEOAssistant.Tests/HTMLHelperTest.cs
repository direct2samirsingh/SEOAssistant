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
    public class HTMLHelperTest
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void EnsureGetXMLAsBodyRemovesAllTagsBeforeAndAfterBodyTag() {
            GoogleSearchEngine searchEngine = new GoogleSearchEngine();

            string file = Path.Combine(TestContext.DeploymentDirectory, "TestData", "GoogleSearchResultPage.html");
            string html = File.ReadAllText(file);

            string result = searchEngine.CleanHTML(html);

            Assert.IsTrue(result.StartsWith("<body") && result.EndsWith("</body>"));
        }

        [TestMethod]
        public void Ensure_RemoveTagPair_Method_RemovesAllTagsAndItsEnclosingContents() {
            HTMLHelper htmlHelper = new HTMLHelper();

            string file = Path.Combine(TestContext.DeploymentDirectory, "TestData", "GoogleSearchResultPage.html");
            string html = File.ReadAllText(file);

            string result = htmlHelper.RemoveTagPair("script", html);

            Assert.IsFalse(result.Contains("<script") || result.Contains("</script>"));
        }

        [TestMethod]
        public void Ensure_RemoveSinglularTag_Method_RemovesAllTagsThatDoNotRequireAPair() {
            HTMLHelper htmlHelper = new HTMLHelper();

            string file = Path.Combine(TestContext.DeploymentDirectory, "TestData", "GoogleSearchResultPage.html");
            string html = File.ReadAllText(file);

            string result = htmlHelper.RemoveSinglularTag("input", html);

            Assert.IsFalse(result.Contains("<input"));
        }
    }
}
