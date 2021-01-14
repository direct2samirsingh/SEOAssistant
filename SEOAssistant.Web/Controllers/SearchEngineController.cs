using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SEOAssistant.Core;
using SEOAssistant.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEOAssistant.Web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class SearchEngineController : ControllerBase
    {        
        private readonly ILogger<SearchEngineController> _logger;

        public SearchEngineController(ILogger<SearchEngineController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("GetResults")]
        public string Get(string keyword, string linkToSearch, string engines)
        {
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

            return returnString.ToString();
        }
    }
}
