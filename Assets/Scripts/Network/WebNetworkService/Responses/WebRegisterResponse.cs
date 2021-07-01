using System;

namespace Network.WebNetworkService.Responses
{
    [Serializable]
    public class WebRegisterResponse
    {
        public string idToken;
        public string refreshToken;
        public WebUser user;

        // Default datas for LocalAPI
        public WebRegisterResponse()
        {
            idToken = "AKS1321?@";
            refreshToken = "sadKSDsa3433";
            user = new WebUser() {id = "1"};
        }
    }

    [Serializable]
    public class WebUser
    {
        public string id;
    }
}