using System;

namespace Network.WebNetworkService.Responses
{
    [Serializable]
    public class WebLeaderboardResponse
    {
        public WebLeaderboardGroup group;
    }

    [Serializable]
    public class WebLeaderboardGroup
    {
        public int week;
        public string start;
        public string end;
        public string tournamentId;
        public WebLeaderboardPlayers[] players;
    }
    
    [Serializable]
    public class WebLeaderboardPlayers
    {
        public string uid;
        public string name;
        public int score;
    }
}