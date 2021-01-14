using SEOAssistant.Core;
using SEOAssistant.SearchEngine;
using System;

namespace SEOAssistant.UI.Services
{
    public class SearchEngineFactory
    {
        public ISearchEngine Create(string searchEngine) {
            if (searchEngine.Equals("google", StringComparison.OrdinalIgnoreCase)) {
                return new GoogleSearchEngine();
            }
            else if (searchEngine.Equals("bing", StringComparison.OrdinalIgnoreCase))
            {
                return new BingSearchEngine();
            }
            else if (searchEngine.Equals("duckduckgo", StringComparison.OrdinalIgnoreCase))
            {
                return new DuckDuckGoSearchEngine();
            }
            else {
                throw new NotImplementedException($"No Search engines found for : {searchEngine}");
            }
        }
    }

    
}
