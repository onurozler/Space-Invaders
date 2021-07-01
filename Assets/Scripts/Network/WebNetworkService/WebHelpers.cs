using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Network.WebNetworkService
{
    public static class WebEnvironmentHelper
    {
        public static string ToUrl(this WebEnvironment webEnvironment)
        {
            switch (webEnvironment)
            {
                case WebEnvironment.Local:
                    return WebConstants.FakeWebServerUrl;
                    break;
            }

            return string.Empty;
        }
    }
    
    public static class HttpMethodHelper
    {
        public static HttpMethod ToType(string httpMethod)
        {
            switch (httpMethod)
            {
                case WebConstants.Get:
                    return HttpMethod.GET;
                case WebConstants.Post:
                    return HttpMethod.POST;
            }

            return HttpMethod.NONE;
        }
    }
    
    public static class WebUrlBuilder
    {
        public static string Build(string url, Dictionary<string,string> parameters)
        {
            var parameterStringBuilder = new StringBuilder();
            parameterStringBuilder.Append(url);
            
            for (int i = 0; i < parameters.Count; i++)
            {
                var key = parameters.ElementAt(i).Key;
                var value = parameters.ElementAt(i).Value;
                parameterStringBuilder.Append(i == 0 ? "?" : "&");
                parameterStringBuilder.Append($"{key}={value}");
            }

            return parameterStringBuilder.ToString();
        }
    }
}