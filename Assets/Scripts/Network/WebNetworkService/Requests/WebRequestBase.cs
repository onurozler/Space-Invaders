using System.Collections.Generic;

namespace Network.WebNetworkService.Requests
{
    public abstract class WebRequestBase
    {
        public readonly Dictionary<string, string> Parameters = new Dictionary<string, string>();
        public abstract HttpMethod Method { get; }
        public abstract string EndPoint { get; }
    }
}
