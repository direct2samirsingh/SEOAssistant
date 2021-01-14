using System;
using System.Collections.Generic;
using System.Text;

namespace SEOAssistant.Core
{
    public class ResultItem
    {
        public string Content { get; init; } = "";
        public string Source { get; init; } = "";
        public bool IsMatch { get; init; }
        public int Index { get; init; }
    }
}
