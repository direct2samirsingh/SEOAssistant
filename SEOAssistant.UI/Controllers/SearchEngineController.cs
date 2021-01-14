using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using SEOAssistant.Core;
using SEOAssistant.UI.Services;
using System.Linq;
using System.Text;

namespace SEOAssistant.UI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SearchEngineController : ControllerBase
    {        
        private readonly ILogger<SearchEngineController> _logger;
        private IMemoryCache cache;

        public SearchEngineController(ILogger<SearchEngineController> logger, IMemoryCache cache)
        {
            _logger = logger;
            this.cache = cache;
        }

        [HttpGet]
        [Route("GetResults")]
        public string Get(string keyword, string linkToSearch, string engines)
        {
            string result;
            if (cache.TryGetValue<string>(keyword + linkToSearch + engines, out result)) {
                return result;
            }

            StringBuilder returnString = new StringBuilder();
            ISearchEngine searchEngine;

            foreach (string engine in engines.Split(",")) {

                searchEngine = new SearchEngineFactory().Create(engine);

                searchEngine.Process(keyword, linkToSearch);

                if (searchEngine.ResultItems != null && searchEngine.ResultItems.Count() > 0)
                {
                    returnString.AppendLine($"[{searchEngine.Name} : {string.Join(", ", searchEngine.ResultItems.Where(x => x.IsMatch).Select(x => x.Index))}]");
                }
                else {
                    returnString.AppendLine($"[{searchEngine.Name} : 0]");
                }
            }
            result = returnString.ToString();
            cache.Set(keyword + linkToSearch + engines, result, System.TimeSpan.FromMinutes(60));

            return result;
        }
    }
}
