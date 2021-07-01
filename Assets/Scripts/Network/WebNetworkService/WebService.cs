using System;
using Architecture.ServiceLocator;
using Network.WebNetworkService.LocalAPI;
using Network.WebNetworkService.Requests;
using UnityEngine;
using UnityEngine.Networking;
using Logger = Helpers.Logger.Logger;

namespace Network.WebNetworkService
{
    public class WebService : IWebService
    {
        private readonly ILocalRestAPI _localRestAPI;
        private readonly WebEnvironment _webEnvironment;
        private string _webToken;

        public WebService(IServiceLocator serviceLocator, WebEnvironment webEnvironment)
        {
            _localRestAPI = serviceLocator.Get<ILocalRestAPI>();
            _webEnvironment = webEnvironment;
        }
        
        public void SetWebToken(string token)
        {
            _webToken = token;
        }

        public void SendRequest<T>(WebRequestBase webRequestBase, Action<T> onSuccess, Action onFailure = null)
        {
            var url = $"{_webEnvironment.ToUrl()}/{webRequestBase.EndPoint}";
            
            UnityWebRequest unityWebRequest;
            if (webRequestBase.Method == HttpMethod.GET)
            {
                url = WebUrlBuilder.Build(url,webRequestBase.Parameters);
                unityWebRequest = UnityWebRequest.Get(url);
            }
            else
            {
                unityWebRequest = UnityWebRequest.Post(url,webRequestBase.Parameters);
            }
            
            if (!string.IsNullOrEmpty(_webToken))
            {
                unityWebRequest.SetRequestHeader("token",_webToken);
            }
            
            Logger.Log($"***** {unityWebRequest.method} - {url} ***** ");


            if (_webEnvironment == WebEnvironment.Local)
            {
               var sentRequest = _localRestAPI.FetchData(unityWebRequest);
               sentRequest.GetAwaiter().OnCompleted(() =>
               {
                   if (sentRequest.IsFaulted)
                   {
                       onFailure?.Invoke();
                       Logger.Log($"***** {unityWebRequest.method} - Local Response Failed ***** ");
                   }
                   else
                   {
                       var responseModel = JsonUtility.FromJson<T>(sentRequest.Result);
                       Logger.Log($"***** {unityWebRequest.method} - Local Response Received ***** {sentRequest.Result}");
                       onSuccess?.Invoke(responseModel);
                   }
               });
            }
            else
            {
                var sentRequest = unityWebRequest.SendWebRequest();
                sentRequest.completed += operation =>
                {
                    if (unityWebRequest.isNetworkError)
                    {
                        onFailure?.Invoke();
                        Logger.Log($"***** {unityWebRequest.method} - Web Response Failed ***** ");
                    }
                    else
                    {
                        var response = unityWebRequest.downloadHandler.text;
                        var responseModel = JsonUtility.FromJson<T>(unityWebRequest.downloadHandler.text);
                        Logger.Log($"***** {unityWebRequest.method} - Web Response Received ***** {response}");
                        onSuccess?.Invoke(responseModel);
                    }
                };
            }
        }
    }
}