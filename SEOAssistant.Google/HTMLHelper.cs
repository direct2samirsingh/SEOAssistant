

namespace SEOAssistant.SearchEngine
{
    public class HTMLHelper
    {

        public string RemoveTagPair(string tagName, string rawHTML)
        {
            while (rawHTML.IndexOf($"<{tagName}") > 0) { 
                var startIndex = rawHTML.IndexOf($"<{tagName}"); 
                var endIndex = rawHTML.IndexOf($"</{tagName}>") + $"</{tagName}>".Length; 
                rawHTML = rawHTML.Replace(rawHTML.Substring(startIndex, endIndex - startIndex), ""); 
            }
            return rawHTML;
        }

        public string RemoveContentBetweenStrings(string startText, string endText, string rawHTML) {
            while (rawHTML.IndexOf(startText) > 0) {
                var startIndex = rawHTML.IndexOf(startText);
                var endIndex = rawHTML.IndexOf(endText) + endText.Length;
                rawHTML = rawHTML.Replace(rawHTML.Substring(startIndex, endIndex - startIndex), "");
            }
            return rawHTML;
        }

        public string RemoveWord(string word, string rawHTML) {
            
            return rawHTML.Replace(word,"");
        }

        public string RemoveSinglularTag(string tagName, string rawHTML)
        {
            while (rawHTML.IndexOf($"<{tagName}") > 0) {
                int startIndex = rawHTML.IndexOf($"<{tagName}");
                int endIndex = rawHTML.IndexOf($">", startIndex) + $">".Length; 
                rawHTML = rawHTML.Replace(rawHTML.Substring(startIndex, endIndex - startIndex), ""); 
            }
            return rawHTML;
        }

    }
}
