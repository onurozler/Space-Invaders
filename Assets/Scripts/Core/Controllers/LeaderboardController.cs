using System;
using System.Linq;
using Architecture.ServiceLocator;
using Core.Views.Leaderboard;
using Network.WebNetworkService;
using Network.WebNetworkService.Requests;
using Network.WebNetworkService.Responses;

namespace Core.Controllers
{
    public class LeaderboardController : IDisposable
    {
        private const int MaxPlayers = 5;
        private readonly IWebService _webService;
        private readonly ILeaderboardView _leaderboardView;
        
        public LeaderboardController(IServiceLocator serviceLocator)
        {
            _webService = serviceLocator.Get<IWebService>();
            _leaderboardView = serviceLocator.Get<ILeaderboardView>();
            _leaderboardView.OnClosedButtonPressed += OnControllerClosed;
        }

        private void OnControllerClosed()
        {
            SetActive(false);
        }

        public void SetActive(bool isActive)
        {
            _leaderboardView.SetActive(isActive);
            if (isActive)
            {
                var request = new WebLeaderboardRequest();
                request.Parameters.Add("country","tr");
                
                _webService.SendRequest<WebLeaderboardResponse>(request, (response) =>
                {
                    var sortedPlayers = response.@group.players.OrderByDescending(x => x.score).Take(MaxPlayers).ToList();
                    for (int i = 0; i < sortedPlayers.Count; i++)
                    {
                        var player = sortedPlayers[i];
                        _leaderboardView.AddRecord(i, player.name,player.score);
                    }
                });
            }
        }

        public void Dispose()
        {
            _leaderboardView.OnClosedButtonPressed -= OnControllerClosed;
        }
    }
}