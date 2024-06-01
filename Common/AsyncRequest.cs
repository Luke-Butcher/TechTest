using System.Collections.Generic;

namespace Common
{
    public class AsyncRequest
    {
        public string Path { get; set; }
        public string Body { get; set; }
        public Dictionary<string, string> Headers { get; set; }
    }
}
