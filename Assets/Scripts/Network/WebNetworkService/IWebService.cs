using System;
using Network.WebNetworkService.Requests;

namespace Network.WebNetworkService
{
    public interface IWebService
    {
        void SetWebToken(string token);
        void SendRequest<T>(WebRequestBase webRequestBase, Action<T> onSuccess, Action onFailure = null);
    }

    public enum HttpMethod
    {
        NONE,
        GET,
        POST
    }
    
    public enum WebEnvironment
    {
        Local
    }
}