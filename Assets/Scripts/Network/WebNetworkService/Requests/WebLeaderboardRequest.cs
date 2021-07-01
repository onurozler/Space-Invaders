namespace Network.WebNetworkService.Requests
{
    public class WebLeaderboardRequest : WebRequestBase
    {
        public override HttpMethod Method => HttpMethod.GET;
        public override string EndPoint => WebConstants.LeaderboardGetEndPoint;
    }
}