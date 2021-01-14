using System;
using System.Collections.Generic;
using System.Text;

namespace SEOAssistant.Core
{
    public interface ISearchEngine
    {
        List<ResultItem> ResultItems { get; set; }
        string Name { get; }

        public void Process(string searchText, string linkToSearch);
    }
}

/*
Task
The CEO at StackOverflow is very interested in SEO and how this can improve Sales. 
Every morning he logs into google.com.au and types in the same key words “e-settlements” and counts down to see where and 
how many times their company, www.sympli.com.au sits on the list.
Seeing the CEO do this every day, a smart software developer at StackOverflow decides to write an application for him 
that will automatically perform this operation and return the result to the screen. They design and code some software 
that receives a string of keywords, and a string URL. This is then processed to return a string of numbers for 
where the resulting URL is found in the Google results.
For example, “1, 10, 33” or “0”.
The CEO is only interested if their URL appears in the first 100 results.

Extension 1
The CTO at StackOverflow has become interested in this project and has added a requirement that the number of calls made to 
google be limited to one call per hour per search. He has proposed that you introduce caching to satisfy this requirement.
Extension 2
The CEO is impressed with your work. He would like the application to be extended so that he can see and compare 
results from other search engines, such as Bing. As a developer, you anticipate that further search engines 
may also require support in the future.
Extension 3
Please provide a brief written summary of how you would address any issues regarding performance, availability, and reliability, 
should they occur.
*/