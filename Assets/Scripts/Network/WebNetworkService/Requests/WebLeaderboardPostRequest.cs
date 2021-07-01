namespace Network.WebNetworkService.Requests
{
    public class WebLeaderboardPostRequest : WebRequestBase
    {
        public override HttpMethod Method => HttpMethod.POST;
        public override string EndPoint => WebConstants.LeaderboardPostEndPoint;
    }
}