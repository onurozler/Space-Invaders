namespace Network.WebNetworkService.Requests
{
    public class WebRegisterRequest : WebRequestBase
    {
        public override HttpMethod Method => HttpMethod.GET;
        public override string EndPoint => "v1/auth/register";
        
    }
}