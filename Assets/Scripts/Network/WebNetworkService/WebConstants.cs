namespace Network.WebNetworkService
{
    public static class WebConstants
    {
        public const string FakeWebServerUrl = "https://localrestapi.com";
        public const string RegisterEndPoint = "v1/auth/register";
        public const string LeaderboardGetEndPoint = "v1/leaderboards";
        public const string LeaderboardPostEndPoint = "v1/leaderboards/submit";

        public const string LocalLeaderboardPath = "LocalDB/Leaderboard";
        
        public const int LocalRestAPIResponseDelayMax = 2;
        public const int LocalRestAPIResponseDelayMin = 0;

        public const string Get = "GET";
        public const string Post = "POST";
    }
}